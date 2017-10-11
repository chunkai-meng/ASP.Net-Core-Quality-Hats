using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QualityHat.Data;
using QualityHat.Models;
using Microsoft.AspNetCore.Authorization;

namespace QualityHat.Controllers
{
	[AllowAnonymous]
	[Authorize(Roles = "Member")]
    public class ShoppingCartController : Controller
    {
		//public IActionResult Index()
		//{
		//    return View();
		//}
		ApplicationDbContext _context;

		public ShoppingCartController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var cart = ShoppingCart.GetCart(this.HttpContext);
			// Return the view
			return View(cart);
		}

		//
		// GET: /Store/AddToCart/5
		[Authorize(Roles = "Anonymous")]
		public ActionResult AddToCart(int id)
		{
			// Retrieve the album from the database
			var addedHat = _context.Hats
				.Single(hat => hat.HatID == id);
			// Add it to the shopping cart
			var cart = ShoppingCart.GetCart(this.HttpContext);
			cart.AddToCart(addedHat, _context);
			// Go back to the main store page for more shopping
			return RedirectToAction("Index", "Home");
		}

		public ActionResult RemoveFromCart(int id)
		{
			var cart = ShoppingCart.GetCart(this.HttpContext);
			int itemCount = cart.RemoveFromCart(id, _context);
			return Redirect(Request.Headers["Referer"].ToString());
		}

	}
}