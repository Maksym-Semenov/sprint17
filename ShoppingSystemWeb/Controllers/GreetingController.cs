using Microsoft.AspNetCore.Mvc;

namespace ShoppingSystemWeb.Controllers
{
	public class GreetingController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
