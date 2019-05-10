using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TCM.CAP.DeliveryService
{
    public interface IDelivery
    {
        string ID { get; set; }
        string OrderID { get; set; }
        string OrderUserID { get; set; }
        DateTime CreatedTime { get; set; }
        DateTime UpdatedTime { get; set; }
    }

    [Table("Deliveries")]
    public class Delivery : IDelivery
    {
        [Column("DeliveryID")]
        public string ID { get; set; }

        [Column("OrderID")]
        public string OrderID { get; set; }

        [Column("OrderUserID")]
        public string OrderUserID { get; set; }

        [Column("CreatedTime")]
        public DateTime CreatedTime { get; set; }

        [Column("UpdatedTime")]
        public DateTime UpdatedTime { get; set; }
    }
    public class DeliveryDbContext : DbContext
    {
        public const string ConnectionString = "Data Source=peaceService.net;port=3306;Pooling=true;Initial Catalog=deliverydb;User Id=root;Password=123456;SslMode = none;";

        public DeliveryDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Delivery> Deliveries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString);
        }
    }
}
