using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
