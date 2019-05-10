using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TCM.CAP.OrderService.Repository;
using TCM.CAP.Test;

namespace TCM.CAP.OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public IOrderRepository OrderRepository { get; }

        public OrderController(IOrderRepository OrderRepository)
        {
            this.OrderRepository = OrderRepository;
        }

        [HttpPost]
        public ActionResult Post([FromBody]Order orderDTO)
        {
            var result = OrderRepository.CreateOrder(orderDTO);

            return new JsonResult(result ? "Post Order Success" : "Post Order Failed");
        }
    }
}