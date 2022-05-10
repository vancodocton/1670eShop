namespace WebApplication1.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int StoreId { get; set; }

        public double TotalPrice { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }
}
