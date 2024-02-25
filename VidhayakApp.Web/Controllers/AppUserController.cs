using Microsoft.AspNetCore.Mvc;

namespace VidhayakApp.Web.Controllers
{
    public class AppUserController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
