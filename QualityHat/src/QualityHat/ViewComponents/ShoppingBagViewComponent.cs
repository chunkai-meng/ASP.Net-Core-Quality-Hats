using Microsoft.AspNetCore.Mvc;
using QualityHat.Models;
using QualityHat.Models.ShoppingBagViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityHat.ViewComponents
{
    public class ShoppingBagViewComponent : ViewComponent
    {
		public IViewComponentResult Invoke(Order order)
		{
			var country = order.OrderStatus;
            var viewModel = new ShoppingBagViewModel
            {
                Order = order
            };
			// return Content("asdfasdfasl;dfjas;dlf" + country);
            return View(viewModel);
		}
	}
}
