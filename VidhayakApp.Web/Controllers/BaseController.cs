using Microsoft.AspNetCore.Mvc;

namespace VidhayakApp.Web.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
