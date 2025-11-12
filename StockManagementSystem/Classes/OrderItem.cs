using System;

namespace StockManagementSystem.Classes
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }       // PK
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }

        public Product Product { get; set; }       // Association to Product

        public decimal CalculateSubtotal()
        {
            return Quantity * PriceAtPurchase;
        }

        public OrderItem() { }

        public OrderItem(int productId, int quantity, decimal price)
        {
            ProductId = productId;
            Quantity = quantity;
            PriceAtPurchase = price;
        }
    }
}
