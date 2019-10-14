using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroMQTT.Microservice.HandshakeAPI.Models
{
    [Table("mqtt_user_handshakes")]
    public class MqttUserHandshake
    {
        public ulong Id { get; set; }

        public ulong MqttUserId { get; set; }

        public DateTime HandshakeAt { get; set; }
    }
}
