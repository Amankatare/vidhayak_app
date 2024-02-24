using Microsoft.AspNetCore.Mvc;

namespace VidhayakApp.Web.Controllers
{
    public class DemandController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
