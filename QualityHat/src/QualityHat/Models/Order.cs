using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityHat.Models
{
	public enum OrderStatus
	{
		InCart, Orderd, Paid, Processing, Delivered
	} 

    public class Order
    {
		public int OrderID { get; set; }
		public OrderStatus OrderStatus { get; set; }
		public int CustomerID { get; set; }
		public double Subtotal { get; set; }
		public double GST { get; set; }
		public double GrandTotal { get; set; }

		public ICollection<OrderItem> OrderItems { get; set; }
    }
}
