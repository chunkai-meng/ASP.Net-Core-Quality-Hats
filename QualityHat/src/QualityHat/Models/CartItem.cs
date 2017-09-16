using System; using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq; using System.Threading.Tasks;

namespace QualityHat.Models {
    public class CartItem {

        //[Key]
        public int CartItemID { get; set; }

        public string CartID { get; set; }

        public int Count { get; set; }

        public DateTime DateCreated { get; set; }

        public Hat Hat { get; set; }

        public ApplicationUser User { get; set; }
    }
}