using MicroMQTT.Microservice.HandshakeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroMQTT.Microservice.HandshakeAPI
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MqttUser> MqttUsers { get; set; }
        public DbSet<MqttUserAccessControlListItem> MqttUserAccessControlListItems { get; set; }
        public DbSet<MqttUserHandshake> MqttUserHandshakes { get; set; }
    }
}
