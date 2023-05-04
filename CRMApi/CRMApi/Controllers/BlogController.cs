using Microsoft.AspNetCore.Mvc;

namespace CRMApi.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
