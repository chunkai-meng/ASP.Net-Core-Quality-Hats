using Microsoft.AspNetCore.Mvc;
using QualityHat.Data;
using QualityHat.Models;
using QualityHat.Models.ShoppingCartViewModels;
using System;

namespace QualityHat.ViewComponents
{
	public class ShoppingCartViewModelViewComponent : ViewComponent    {
		private readonly ApplicationDbContext _context;
		public ShoppingCartViewModelViewComponent(ApplicationDbContext context)
		{
			_context = context;
		}

		public IViewComponentResult Invoke()
		{
			return View(ReturnCurrentCartViewModel());
		}

		public ShoppingCartViewModel ReturnCurrentCartViewModel()
		{
			var cart = ShoppingCart.GetCart(this.HttpContext);
			decimal total = cart.GetTotal(_context);
			decimal cartGst = 0.15m * total;
			decimal cartTotal = cartGst + total;
			// Set up our ViewModel
			var viewModel = new ShoppingCartViewModel
			{
				CartItems = cart.GetCartItems(_context),
				CartGST = Math.Round(cartGst, 2, MidpointRounding.AwayFromZero),
				CartTotal = Math.Round(cartTotal, 2, MidpointRounding.AwayFromZero)
			};
			return viewModel;
		}

	}
}
