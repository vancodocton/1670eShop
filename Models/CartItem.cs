namespace WebApplication1.Models
{
    public class CartItem
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string BookIsBn { get; set; }
        public Book Book { get; set; }

        public int Quantity { get; set; }
    }
}
