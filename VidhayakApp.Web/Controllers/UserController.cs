using Microsoft.AspNetCore.Mvc;

namespace VidhayakApp.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Dashboard()
        {
            //if (HttpContext.Session.GetString("UserName") != null)
            //{
            //    return RedirectToAction("Dashboard");
            //}
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }


    }
}
