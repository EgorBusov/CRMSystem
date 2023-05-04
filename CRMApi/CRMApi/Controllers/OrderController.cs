using Microsoft.AspNetCore.Mvc;

namespace CRMApi.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
