using Microsoft.AspNetCore.Mvc;
using VidhayakApp.Core.Entities;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.Web.ViewModels;

namespace VidhayakApp.Web.Controllers
{
    public class AppUserController : Controller
    {
        private readonly VidhayakAppContext _db;
        public AppUserController(VidhayakAppContext db)
        {
            _db = db;
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AppUsers()
        {

            var model = _db.Users.Where(s => s.RoleId == 3).ToList();
            

            return View(model);
        }
    }
}
