using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QualityHat.Models
{
	public class Category
    {
		public int CategoryID { get; set; }	
		public string Name { get; set; }

		public ICollection<Hat> Hats { get; set; }
    }
}
