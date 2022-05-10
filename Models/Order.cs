using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int StoreId { get; set; }

        public Store Store { get; set; }

        public double TotalPrice { get; private set; }

        private List<OrderItem> items = new();
        public List<OrderItem> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                TotalPrice = Items.Sum(i => i.Quantity * i.Price);
            }
        }

        //public OrderStatus Status { get; set; }
    }
}
