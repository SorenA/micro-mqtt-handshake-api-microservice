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

        public HandshakeController(ILogger<HandshakeController> logger, AppDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        [HttpPost]
        public HandshakeResponseModel Post(HandshakeRequestModel model)
        {
            // Check if we have an identity token
            if (string.IsNullOrEmpty(model.Identity))
            {
                return new HandshakeResponseModel
                {
                    IsSuccessful = false,
                };
            }

            // Process as handshake, matching identity token
            var user = dbContext.MqttUsers
                .Where(x => x.Identity == model.Identity)
                .FirstOrDefault();

            // Check if user is found
            if (user == null)
            {
                return new HandshakeResponseModel
                {
                    IsSuccessful = false,
                };
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

            return new HandshakeResponseModel
            {
                IsSuccessful = true,
            };
        }
    }
}