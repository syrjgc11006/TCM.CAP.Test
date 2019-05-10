using System;

namespace TCM.CAP.Model
{
    public class OrderItems
    {
        string ID { get; set; }

        string ProductID { get; set; }

        decimal Price { get; set; }

        double Quantity { get; set; }
    }
}
