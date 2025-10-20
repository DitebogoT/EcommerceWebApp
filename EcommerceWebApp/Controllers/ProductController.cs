using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApp.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
