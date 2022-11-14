using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingSystemWeb.Models
{
	public class Product
	{
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 5)]
        [Required]
		public string? Title { get; set; }

		[Display(Name = "Expired Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpiredDate { get; set; }
        
        [Required]
        [StringLength(50)]
        public string? Category { get; set; }

        [Range(1, 1000)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }
}
