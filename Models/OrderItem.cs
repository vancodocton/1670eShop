namespace WebApplication1.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }

        public Order Order { get; set; }

        public string BookIsBn { get; set; }
        public Book Book { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}
