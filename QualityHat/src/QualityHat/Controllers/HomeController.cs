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


		// GET: MemberHats
		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.Hats.Include(h => h.Category).Include(h => h.Supplier);
			return View(await applicationDbContext.ToListAsync());
		}

        public async Task<IActionResult> Category(int? id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            // var category = await _context.Categorys.SingleOrDefaultAsync(m => m.CategoryID == id);

            var category = await _context.Categorys.Include(h => h.Hats).AsNoTracking().SingleOrDefaultAsync(m => m.CategoryID == id);
            
            if (category == null)
            {
                return View("NotFound");
            }

            return View(category);
        }

		public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Welcome to Quality Hats.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
