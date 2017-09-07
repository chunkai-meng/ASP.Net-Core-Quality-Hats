using System.Collections.Generic;

namespace QualityHat.Models
{
	public class Supplier
    {
		public int SupplierID { get; set; }
		public string Name { get; set; }
		public string WorkPhone { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }

		public ICollection<Hat> Hats { get; set; }
    }
}
