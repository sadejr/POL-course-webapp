namespace WebApplication1.Models
{
    public class Order
    {
        public Customer Customer { get; set; }

        public string PaymentMethod { get; set; }

        public List<CartItem> CartItems { get; set; }
    }

    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
