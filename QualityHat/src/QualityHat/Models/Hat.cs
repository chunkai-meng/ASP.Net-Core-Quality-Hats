using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QualityHat.Models
{
	public class Hat
    {
		public int HatID { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		[DataType(DataType.Currency)]
		public decimal Price { get; set; }

		[Required]
		[Display(Name = "Description")]
		public string Disc { get; set; }

		[Required]
		public string Image { get; set; }
		
		public int CategoryID { get; set; }
		public int SupplierID { get; set; }

		public Category Category { get; set; }
		public Supplier Supplier { get; set; }
		//public ICollection<OrderItem> OrderItems { get; set; }
    }
}
