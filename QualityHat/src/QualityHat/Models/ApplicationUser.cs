using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace QualityHat.Models
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	public class ApplicationUser : IdentityUser
    {
    	public String Address { get; set; }
		public string CustomerName { get; set; }
		public bool Enabled { get; set; }
	}
}