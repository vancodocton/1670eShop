#pragma warning disable CS8618
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public string Address { get; set; }
        public Store Store { get; set; }

        public List<CartItem> CartItems { get; set; } = new();

        public List<Order> Orders { get; set; } = new();
    }
}
