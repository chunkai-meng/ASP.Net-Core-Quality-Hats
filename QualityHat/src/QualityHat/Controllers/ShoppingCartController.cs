using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QualityHat.Data;
using QualityHat.Models;

namespace QualityHat.Controllers
{
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
		public ActionResult AddToCart(int id)
		{
			// Retrieve the album from the database
			var addedTutorial = _context.Hats
				.Single(hat => hat.HatID == id);
			// Add it to the shopping cart
			var cart = ShoppingCart.GetCart(this.HttpContext);
			cart.AddToCart(addedTutorial, _context);
			// Go back to the main store page for more shopping
			return RedirectToAction("Index", "Hats");
		}

		public ActionResult RemoveFromCart(int id)
		{
			var cart = ShoppingCart.GetCart(this.HttpContext);
			int itemCount = cart.RemoveFromCart(id, _context);
			return Redirect(Request.Headers["Referer"].ToString());
		}

	}
}