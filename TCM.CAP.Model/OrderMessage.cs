using System;
using System.Collections.Generic;
using System.Text;

namespace TCM.CAP.Model
{
    public class OrderMessage
    {
        public string ID { get; set; }

        public DateTime OrderTime { get; set; }

        public List<OrderItems> OrderItems { get; set; }

        public string OrderUserID { get; set; }

        public string ProductID { get; set; } // 演示字段，实际应该在OrderItems中

        public DateTime MessageTime { get; set; }
    }
}
