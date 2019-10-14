using System.ComponentModel.DataAnnotations.Schema;

namespace MicroMQTT.Microservice.HandshakeAPI.Models
{
    [Table("mqtt_user_acl")]
    public class MqttUserAccessControlListItem
    {
        public ulong Id { get; set; }

        public ulong MqttUserId { get; set; }

        public string Type { get; set; }

        public string TopicPattern { get; set; }

        public static string TypePublish = "pub";
        public static string TypeSubscibe = "sub";
    }
}
