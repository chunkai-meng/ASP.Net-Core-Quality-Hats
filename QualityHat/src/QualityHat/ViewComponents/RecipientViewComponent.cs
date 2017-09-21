using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QualityHat.Data;
using QualityHat.Models;
using QualityHat.Models.ShoppingBagViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityHat.ViewComponents
{
	public class RecipientViewComponent: ViewComponent
    {
		private readonly ApplicationDbContext _context;

		public RecipientViewComponent(ApplicationDbContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke(Order order)
		{
			IOrderedEnumerable<Recipient> recipients = order.User.Recipients.OrderByDescending(r => r.Default);

			var viewModel = new ShoppingBagViewModel
			{
				Recipients = recipients.ToList(),
				Order = order
			};

			return View(viewModel);
		}

	}
}
