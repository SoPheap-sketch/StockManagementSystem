using System;
using System.Collections.Generic;

namespace StockManagementSystem.Classes
{
    public class Order
    {
        public int OrderID { get; set; }  
        public Customer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }

        public decimal CalculateTotal()
        {
            decimal total = 0;
            foreach (var item in OrderItems)
                total += item.CalculateSubtotal();
            return total;
        }
    }
}
