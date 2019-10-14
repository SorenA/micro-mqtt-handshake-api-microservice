using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroMQTT.Microservice.HandshakeAPI.Models
{
    [Table("mqtt_users")]
    public class MqttUser
    {
        public ulong Id { get; set; }

        public string Identity { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime LastHandshakeAt { get; set; }

    }
}
