using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityHat.Models
{
	public enum CustomerStatus
	{
		Active, Inactive, Suspended 
	}

    public class Customer
    {
		public int CustomerID { get; set; }
		public string Name { get; set; }
		public string MobilePhone { get; set; }
		public string HomePhone { get; set;	}
		public string WorkPhone { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }
		public CustomerStatus CustomerStatus { get; set; }

		public ICollection<Order> Orders { get; set; }
    }
}
