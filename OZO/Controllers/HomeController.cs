using Microsoft.AspNetCore.Mvc;

namespace OZO.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}