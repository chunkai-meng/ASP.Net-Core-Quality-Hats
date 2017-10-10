using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QualityHat.Data;

namespace QualityHat.Controllers
{
	public class HomeController : Controller
{

	private readonly ApplicationDbContext _context;

	public HomeController(ApplicationDbContext context)
	{
		_context = context;
	}

		//public IActionResult Index()
		//{
		//	return View();
		//}

		// GET: MemberHats
		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.Hats.Include(h => h.Category).Include(h => h.Supplier);
			return View(await applicationDbContext.ToListAsync());
		}


		public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
