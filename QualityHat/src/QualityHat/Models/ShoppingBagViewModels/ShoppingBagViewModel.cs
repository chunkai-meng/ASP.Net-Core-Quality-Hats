using Microsoft.EntityFrameworkCore;
using QualityHat.Data;
using System.Collections.Generic;
using System.Linq;

namespace QualityHat.Models.ShoppingBagViewModels
{
	public class ShoppingBagViewModel
	{
		public Hat Hat { get; set; }
		public Order Order { get; set; }
		public List<Recipient> Recipients { get; set; }
		public int HatID { get; set; }
		public string Name { get; set; }
		public bool Selected { get; set; }
	}
}
