using Microsoft.AspNetCore.Mvc;

namespace VidhayakApp.Web.Controllers.Profile
{
    public class ProfileController : Controller
    {

        public ProfileController()
        {
            
        }
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
