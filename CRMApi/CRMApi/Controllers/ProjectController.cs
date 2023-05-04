using Microsoft.AspNetCore.Mvc;

namespace CRMApi.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
