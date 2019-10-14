using MicroMQTT.Microservice.HandshakeAPI.Models;
using MicroMQTT.Microservice.HandshakeAPI.RequestModels;
using MicroMQTT.Microservice.HandshakeAPI.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace MicroMQTT.Microservice.HandshakeAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class HandshakeController : ControllerBase
    {
        private readonly ILogger<HandshakeController> logger;
        private readonly AppDbContext dbContext;
        private static readonly Random random = new Random();

        private bool IsOnboardingTokenEnabled { get; set; }
        private string OnboardingToken { get; set; }


        public HandshakeController(ILogger<HandshakeController> logger, AppDbContext dbContext, IConfiguration config)
        {
            this.logger = logger;
            this.dbContext = dbContext;

            OnboardingToken = config.GetValue<string>("HandshakeOnboardingToken");
            IsOnboardingTokenEnabled = !string.IsNullOrEmpty(OnboardingToken);
        }

        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var charArray = Enumerable
                .Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)])
                .ToArray();

            return new string(charArray);
        }

        [HttpPost]
        public HandshakeResponseModel Post(HandshakeRequestModel model)
        {
            // Check if we have an identity token
            if (string.IsNullOrEmpty(model.Identity))
            {
                // Process as register, no identity token given
                if (IsOnboardingTokenEnabled && OnboardingToken != model.OnboardingToken)
                {
                    // Onboarding token enabled, no match found, reject registration
                    return new HandshakeResponseModel(false);
                }

                // Register new user, token either matches or is not needed
                var password = GenerateRandomString(32); // Generate password, length 32 chars
                var newUser = new MqttUser
                {
                    Identity = "id-" + GenerateRandomString(61), // Generate identity, length 64 chars
                    Username = "un-" + GenerateRandomString(29), // Generate username, length 32 chars
                    Password = BCrypt.Net.BCrypt.HashPassword(password), // Hash password
                    LastHandshakeAt = DateTime.Now,
                };
                dbContext.MqttUsers.Add(newUser);
                dbContext.SaveChanges();

                // Add ACL
                var newUserAclPub = new MqttUserAccessControlListItem
                {
                    MqttUserId = newUser.Id,
                    Type = MqttUserAccessControlListItem.TypePublish,
                    TopicPattern = "#",
                };
                var newUserAclSub = new MqttUserAccessControlListItem
                {
                    MqttUserId = newUser.Id,
                    Type = MqttUserAccessControlListItem.TypeSubscibe,
                    TopicPattern = "#",
                };
                dbContext.MqttUserAccessControlListItems.Add(newUserAclPub);
                dbContext.MqttUserAccessControlListItems.Add(newUserAclSub);

                // Add handshake
                var newUserHandshake = new MqttUserHandshake
                {
                    MqttUserId = newUser.Id,
                    HandshakeAt = newUser.LastHandshakeAt,
                };
                dbContext.MqttUserHandshakes.Add(newUserHandshake);

                dbContext.SaveChanges();

                // Return credentials
                return new HandshakeResponseModel(true)
                {
                    Identity = newUser.Identity,
                    Username = newUser.Username,
                    Password = newUser.Password,
                };
            }

            // Process as handshake, matching identity token
            var user = dbContext.MqttUsers
                .Where(x => x.Identity == model.Identity)
                .FirstOrDefault();

            // Check if user is found
            if (user == null)
            {
                return new HandshakeResponseModel(false);
            }

            // Add handshake
            user.LastHandshakeAt = DateTime.Now;
            var userHandshake = new MqttUserHandshake
            {
                MqttUserId = user.Id,
                HandshakeAt = user.LastHandshakeAt,
            };
            dbContext.MqttUserHandshakes.Add(userHandshake);
            dbContext.SaveChanges();

            return new HandshakeResponseModel(true)
            {
                Identity = user.Identity,
            };
        }
    }
}