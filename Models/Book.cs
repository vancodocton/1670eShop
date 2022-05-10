using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Book
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(20)]
        public string Isbn { get; set; }

        public int StoreId { get; set; } 
        
        [ValidateNever]
        public Store Store { get; set; }

        public string Title { get; set; }

        [Range(0, double.MaxValue,ErrorMessage = "Price is more than 0")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        ////
        /// another properties
        ///
    }
}
