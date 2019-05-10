using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TCM.CAP.Model;

namespace TCM.CAP.Test
{
    [Table("Orders")]
    public class Order:IOrder
    {
        [Column("OrderID")]
        public string ID { get; set; }

        [Column("OrderTime")]
        public DateTime OrderTime { get; set; }

        [Column("OrderUserID")]
        public string OrderUserID { get; set; }

        [NotMapped]
        public List<OrderItems> OrderItems { get; set; }

        [Column("ProductID")]
        public string ProductID { get; set; }
    }

    public class OrderDbContext : DbContext
    {
        public const string ConnectionString = "Data Source=peaceService.net;port=3306;Pooling=true;Initial Catalog=OrderDb;User Id=root;Password=123456;SslMode = none;";

        public OrderDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString);
        }
    }
}
