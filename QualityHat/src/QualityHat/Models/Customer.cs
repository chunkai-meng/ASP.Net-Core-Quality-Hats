using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QualityHat.Models
{
	public enum CustomerStatus
	{
		Active, Inactive, Suspended
	}

    public class Customer
    {
		public int CustomerID { get; set; }
		[StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
		public string Name { get; set; }

		[DataType(DataType.PhoneNumber)]
		public string MobilePhone { get; set; }
		[DataType(DataType.PhoneNumber)]
		public string HomePhone { get; set;	}
		[DataType(DataType.PhoneNumber)]
		public string WorkPhone { get; set; }

		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		public string Address { get; set; }
		public CustomerStatus CustomerStatus { get; set; }

		public ICollection<Order> Orders { get; set; }
    }
}
