using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QualityHat.Models
{
    public class OrderItem
    {
		public int OrderItemID { get; set; }
		public int HatID { get; set; }
		public int OrderID { get; set; }
		public int Quantity { get; set; }

		public Hat Hat { get; set; }
		public Order Order { get; set; }
    }
}
