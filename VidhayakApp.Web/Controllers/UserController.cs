
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace VidhayakApp.Web.Controllers
{
    public class UserController : Controller
    {
        
        public UserController() {

             
        }

        public IActionResult Dashboard()
        {
            var ss = HttpContext.Session.GetString("UserName");
            Console.WriteLine("#######################\n" + ss+ "\n#########################");

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
        public IActionResult Feedback()
        {
            return View();
        }

    }
}
