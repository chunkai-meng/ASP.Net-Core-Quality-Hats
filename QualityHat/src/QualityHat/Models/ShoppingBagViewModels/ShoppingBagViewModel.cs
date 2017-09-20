using System.Collections.Generic;

namespace QualityHat.Models.ShoppingBagViewModels
{
	public class ShoppingBagViewModel
	{
		public Hat Hat { get; set; }
		public Order Order { get; set; }
		public int HatID { get; set; }
		public string Name { get; set; }
		public bool Selected { get; set; }
	}
}
