using Dapper;
using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TCM.CAP.Model;
using TCM.CAP.Test;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace TCM.CAP.OrderService.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public OrderDbContext DbContext { get; }

        public ICapPublisher CapPublisher { get; set; }

        public OrderRepository(OrderDbContext DbContext, ICapPublisher CapPublisher)
        {
            this.DbContext = DbContext;
            this.CapPublisher = CapPublisher;
        }
        public bool CreateOrder(Order order)
        {
            using (var conn = new MySqlConnection(OrderDbContext.ConnectionString))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    //1.模拟本地业务执行成功，消息执行失败

                    // business code here
                    string sqlCommand = @"insert into orders(orderid, ordertime, orderuserid, productid) values (@orderid, @ordertime, @orderuserid, @productid);";
                    order.ID = GenerateOrderID();
                    conn.Execute(sqlCommand, param: new
                    {
                        OrderID = order.ID,
                        OrderTime = DateTime.Now,
                        OrderUserID = order.OrderUserID,
                        ProductID = order.ProductID
                    }, transaction: trans);
                    // For Dapper/ADO.NET, need to pass transaction
                    var orderMessage = new OrderMessage()
                    {
                        ID = order.ID,
                        OrderUserID = order.OrderUserID,
                        OrderTime = order.OrderTime,
                        OrderItems = null,
                        MessageTime = DateTime.Now,
                        ProductID = order.ProductID // For demo use
                    };

                    //int a = 0;
                    //int b = 12 / a;
                    CapPublisher.Publish(EventConstants.EVENT_NAME_CREATE_ORDER, orderMessage);                 

                    trans.Commit();
                }
            }
            return true;
        }

        private string GenerateOrderID()
        {
            // TODO: Some business logic to generate Order ID
            return Guid.NewGuid().ToString();
        }

        private string GenerateEventID()
        {
            // TODO: Some business logic to generate Order ID
            return Guid.NewGuid().ToString();
        }
    }
}
