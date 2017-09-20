using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QualityHat.Models {

    public class Recipient {

        public int RecipientId { get; set; }
		public bool Default { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }
        public ApplicationUser User { get; set; }
    }

}