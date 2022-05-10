#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        [StringLength(80)]
        public string Name { get; set; }

        public List<Book> Books { get; set; } = new();

        ////
        /// another properties
        ///
    }
}
