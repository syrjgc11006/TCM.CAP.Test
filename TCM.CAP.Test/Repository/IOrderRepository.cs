using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCM.CAP.Test;

namespace TCM.CAP.OrderService.Repository
{
    public interface IOrderRepository
    {
        bool CreateOrder(Order order);
    }
}
