using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCM.CAP.Model;

namespace TCM.CAP.DeliveryService.Services
{
    public interface IOrderSubscriberService
    {
        Task ConsumeOrderMessage(OrderMessage message);
    }
}
