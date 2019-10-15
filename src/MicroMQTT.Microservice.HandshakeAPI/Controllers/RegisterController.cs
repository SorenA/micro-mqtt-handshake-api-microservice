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
    [Route("/register")]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<HandshakeController> logger;
        private readonly AppDbContext dbContext;
        private static readonly Random random = new Random();

        private bool IsOnboardingTokenEnabled { get; set; }
        private string OnboardingToken { get; set; }


        public RegisterController(ILogger<HandshakeController> logger, AppDbContext dbContext, IConfiguration config)
        {
            this.logger = logger;
            this.dbContext = dbContext;

            OnboardingToken = config.GetValue<string>("OnboardingToken");
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
        public RegisterResponseModel Post(RegisterRequestModel model)
        {
            // Validate onboarding token if required
            if (IsOnboardingTokenEnabled && OnboardingToken != model.OnboardingToken)
            {
                // Onboarding token enabled, no match found, reject registration
                return new RegisterResponseModel
                {
                    IsSuccessful = false,
                };
            }

            // Register new user, token either matches or is not needed
            var password = GenerateRandomString(32); // Generate password, length 32 chars
            var user = new MqttUser
            {
                Identity = "id-" + GenerateRandomString(61), // Generate identity, length 64 chars
                Username = "un-" + GenerateRandomString(29), // Generate username, length 32 chars
                Password = BCrypt.Net.BCrypt.HashPassword(password), // Hash password
                LastHandshakeAt = DateTime.Now,
            };
            dbContext.MqttUsers.Add(user);
            dbContext.SaveChanges();

            // Add ACL
            var userAclPub = new MqttUserAccessControlListItem
            {
                MqttUserId = user.Id,
                Type = MqttUserAccessControlListItem.TypePublish,
                TopicPattern = "#",
            };
            var userAclSub = new MqttUserAccessControlListItem
            {
                MqttUserId = user.Id,
                Type = MqttUserAccessControlListItem.TypeSubscibe,
                TopicPattern = "#",
            };
            dbContext.MqttUserAccessControlListItems.Add(userAclPub);
            dbContext.MqttUserAccessControlListItems.Add(userAclSub);

            // Add handshake
            var userHandshake = new MqttUserHandshake
            {
                MqttUserId = user.Id,
                HandshakeAt = user.LastHandshakeAt,
            };
            dbContext.MqttUserHandshakes.Add(userHandshake);

            dbContext.SaveChanges();

            // Return credentials
            return new RegisterResponseModel
            {
                IsSuccessful = true,
                Identity = user.Identity,
                Username = user.Username,
                Password = password,
            };
        }
    }
}