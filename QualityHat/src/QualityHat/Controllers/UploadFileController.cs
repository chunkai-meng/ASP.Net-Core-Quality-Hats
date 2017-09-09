using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace QualityHat.Controllers
{
    public class UploadFileController : Controller
    {
		//public IActionResult Index()
		//{
		//    return View();
		//}

		#region snippet1
		[HttpPost("UploadFiles")]
		public async Task<IActionResult> Post(IFormFile file)
		{


			// full path to file in temp location
			//var filePath = Path.GetTempFileName();
			//var filePath = "./wwwroot/images/temp1.png";
			//var name = file.
			string filePath = Path.GetTempFileName().Replace(".tmp", ".csv");
			var myUniqueFileName = $@"{DateTime.Now.Ticks}.png";
			filePath = "./wwwroot/images/" + myUniqueFileName;

			if (file.Length > 0)
				{
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await file.CopyToAsync(stream);
					}
				}

			// process uploaded files
			// Don't rely on or trust the FileName property without validation.

			return Ok(new { filePath });
		}
		#endregion
	}
}