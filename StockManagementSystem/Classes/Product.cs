using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagementSystem.Classes
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = "";

        public void UpdateStock(int qty) => QuantityInStock += qty;
        public decimal CalculateValue() => Price * QuantityInStock;
        public override string ToString() => Name;
    }
}
