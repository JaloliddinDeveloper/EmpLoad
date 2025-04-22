using Microsoft.AspNetCore.Mvc;

namespace EmpLoad.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
