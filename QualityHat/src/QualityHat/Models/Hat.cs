using System.Collections.Generic;

namespace QualityHat.Models
{
	public class Hat
    {
		public int HatID { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public string Disc { get; set; }
		public string Image { get; set; }
		public int CategoryID { get; set; }
		public int SupplierID { get; set; }

		public Category Category { get; set; }
		public Supplier Supplier { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; }
    }
}
