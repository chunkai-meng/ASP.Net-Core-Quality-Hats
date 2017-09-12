using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QualityHat.Models
{
	public class Supplier
    {
		public int SupplierID { get; set; }

		[Required]
		[StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
		[Display(Name = "Full Name")]
		public string Name { get; set; }

		[Required]
		[DataType(DataType.PhoneNumber)]
		[Display(Name = "Contact Number")]
		public string WorkPhone { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[StringLength(100, ErrorMessage = "Address cannot be longer than 50 characters.")]
		public string Address { get; set; }

		public ICollection<Hat> Hats { get; set; }
    }
}
