using System;

namespace StockManagementSystem.Classes
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public void ValidateContactInfo()
        {
            if (string.IsNullOrWhiteSpace(Phone) && string.IsNullOrWhiteSpace(Email))
                throw new Exception("Phone or Email required!");
        }
        public override string ToString() => Name;
    }
}
