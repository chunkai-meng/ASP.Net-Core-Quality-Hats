using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QualityHat.Models.AccountViewModels;

namespace QualityHat.Models
{
	public enum OrderStatus
	{
		InCart, Ordered, Paid, Processing, Delivered
	}

    public class Order
    {
		public int OrderID { get; set; }
		public OrderStatus OrderStatus { get; set; }
		public int CustomerID { get; set; }
		//public int AccountID { get; set; }
		public double Subtotal { get; set; }
		public double GST { get; set; }

		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
		public DateTime OrderedDate { get; set; }

		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
		public DateTime PaidDate { get; set; }

		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
		public DateTime DeveliverdDate { get; set; }

		[DataType(DataType.Currency)]
		public double GrandTotal { get; set; }


		public Customer Customer { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; }
    }
}
