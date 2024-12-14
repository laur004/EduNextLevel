using Microsoft.AspNetCore.Mvc;

namespace ProiectDAW.Controllers
{
	public class ArticlesController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
