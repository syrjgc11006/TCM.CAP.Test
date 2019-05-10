using Dapper;
using DotNetCore.CAP;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TCM.CAP.Common;
using TCM.CAP.Model;

namespace TCM.CAP.DeliveryService.Services
{
    public class OrderSubscriberService : IOrderSubscriberService, ICapSubscribe
    {
        public OrderSubscriberService()
        {

        }


        [CapSubscribe(EventConstants.EVENT_NAME_CREATE_ORDER)]
        public async Task ConsumeOrderMessage(OrderMessage message)
        {
            await Console.Out.WriteLineAsync($"[DeliveryService] Received message : {JsonHelper.SerializeObject(message)}");
            await AddDeliveryRecordAsync(message);
        }

        /// <summary>
        /// 当消息消费异常，会发现，CAP会自动帮忙重试 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private async Task<bool> AddDeliveryRecordAsync(OrderMessage order)
        {
            //throw new Exception("test"); // just for demo use
            using (var conn = new MySqlConnection(DeliveryDbContext.ConnectionString))
            {
                string sqlCommand = @"insert into deliveries(deliveryid, orderid, orderuserid, createdtime, updatedtime) 
values (@deliveryid, @orderid, @orderuserid, @createdtime, @updatedtime)";

                int count = await conn.ExecuteAsync(sqlCommand, param: new
                {
                    DeliveryID = Guid.NewGuid().ToString(),
                    OrderID = order.ID,
                    OrderUserID = order.OrderUserID,
                    ProductID = order.ProductID,
                    CreatedTime = DateTime.Now,
                    updatedtime = DateTime.Now
                });

                return count > 0;
            }
        }
    }
}
