using Microsoft.AspNetCore.Mvc;

namespace VidhayakApp.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Dashboard()
        {

            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Complaint()
        {
            return View();
        }

        public IActionResult Demand()
        {
            return View();
        }

        public IActionResult Suggession()
        {
            return View();
        }

    }
}
