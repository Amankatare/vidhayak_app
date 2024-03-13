
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

namespace VidhayakApp.Web.Controllers
{
    //[Authorize(Policy = "RequireUserRole")]
    public class UserController : Controller
    {
        
        public UserController() {

             
        }

        public IActionResult Dashboard()
        {
            var ss = HttpContext.Session.GetString("UserName");
            Console.WriteLine("#######################\n" + ss+ "\n#########################");

            //if (userName == null)
            //{
            //    return RedirectToAction("Login", "Account"); // Redirect to login page if user is not logged in
            //}

            // Retrieve complaints, demands, and suggestions filled by the user from the database
            //var userEntries = _dbContext.UserEntries.Where(entry => entry.UserName == userName).ToList();

            // Pass the user entries to the view
            //return View(userEntries);



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
