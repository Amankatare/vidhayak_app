using Microsoft.AspNetCore.Mvc;

namespace VidhayakApp.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
