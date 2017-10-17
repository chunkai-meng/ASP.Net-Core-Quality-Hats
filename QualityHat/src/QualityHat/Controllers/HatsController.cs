using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityHat.Data;
using QualityHat.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace QualityHat.Controllers
{
	[Authorize(Roles = "Admin")]
	public class HatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HatsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Hats
        public async Task<IActionResult> Index()
        {
            ViewData["ShowCartView"] = true;
            var shopContext = _context.Hats.Include(h => h.Category).Include(h => h.Supplier);
            return View(await shopContext.ToListAsync());
        }

        // GET: Hats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hat = await _context.Hats.SingleOrDefaultAsync(m => m.HatID == id);
            if (hat == null)
            {
                return NotFound();
            }

            return View(hat);
        }


		[HttpPost]
		public async Task<IActionResult> UploadFile(IFormFile file, int id)
		{
			string fileFullPath = file.FileName;
			var fileName = Path.GetFileName(fileFullPath);
			var fileExtension = Path.GetExtension(fileFullPath);

			//return Ok(new { fileExtension });

			var myUniqueFileName = $@"{DateTime.Now.Ticks}" + fileExtension;
			var filePath = "./wwwroot/images/hats/" + myUniqueFileName;
			// var imageURL = "/images/hats/" + myUniqueFileName;
			// To fix the tricky Domain problem
			var imageURL = "/mengc06/asp_assignment/images/hats/" + myUniqueFileName;

			if (file.Length > 0)
			{
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}
			}

			// process uploaded files
			// Don't rely on or trust the FileName property without validation.
            if(id == 0){
                return RedirectToAction("Create", "Hats", new { fileName = imageURL });
            } else {
                return RedirectToAction("Edit", "Hats", new { fileName = imageURL, id = id });
            }

		}


		// GET: Hats/Create
		public IActionResult Create(string fileName)
        {
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "CategoryID", "Name");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name");

			// if (!string.IsNullOrEmpty(fileName))
			// {
			// 	return View("NotFound");
			// }
			ViewData["Image"] = fileName;

            return View();
        }

        // POST: Hats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HatID,CategoryID,Disc,Image,Name,Price,SupplierID")] Hat hat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hat);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "CategoryID", "CategoryID", hat.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierID", hat.SupplierID);
            return View(hat);
        }

        // GET: Hats/Edit/5
        public async Task<IActionResult> Edit(string fileName, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hat = await _context.Hats.SingleOrDefaultAsync(m => m.HatID == id);
            if (hat == null)
            {
                return NotFound();
            }
            ViewData["Image"] = (fileName == null) ? hat.Image : fileName;
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "CategoryID", "Name", hat.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Name", hat.SupplierID);

            ViewData["HatID"] = id;
            return View(hat);
        }

        // POST: Hats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HatID,CategoryID,Disc,Image,Name,Price,SupplierID")] Hat hat)
        {
            if (id != hat.HatID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HatExists(hat.HatID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(hat);
        }

        // GET: Hats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hat = await _context.Hats.SingleOrDefaultAsync(m => m.HatID == id);
            if (hat == null)
            {
                return NotFound();
            }

            return View(hat);
        }

        // POST: Hats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartItems = await _context.Hats.SingleOrDefaultAsync(m => m.HatID == id);

            var hat = await _context.Hats.SingleOrDefaultAsync(m => m.HatID == id);
            _context.Hats.Remove(hat);
            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["HatUsed"]= "The Hat being deleted has been used in previous orders.Delete those orders before trying again.";
                return RedirectToAction("Delete");
            }
            return RedirectToAction("Index");
        }

		private bool HatExists(int id)
        {
            return _context.Hats.Any(e => e.HatID == id);
        }
    }
}
