using System.Collections.Generic;

namespace QualityHat.Models
{
	public class Hat
    {
		public int ID { get; set; }
		public string Name { get; set; }
		public int CategoryID { get; set; }
		public double Price { get; set; }
		public string Disc { get; set; }
		public string Image { get; set; }

		public ICollection<OrderItem> OrderItems { get; set; }
    }
}
