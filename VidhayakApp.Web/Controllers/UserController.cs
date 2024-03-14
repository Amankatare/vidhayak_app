
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data;

namespace VidhayakApp.Web.Controllers
{
    //[Authorize(Policy = "RequireUserRole")]
    
    public class UserController : Controller
    {

        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly VidhayakAppContext _dbContext;

        public UserController(IUserRepository userRepository, VidhayakAppContext dbContext)
        {
            _userRepository = userRepository;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Dashboard()
        {
            // Retrieve the user's details based on the user ID
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                // Handle if user ID is not found in the session
                return RedirectToAction("Login", "Account");
            }

            var user = await _userRepository.GetByIdAsync(userId.Value);

            // Retrieve complaints, demands, and suggestions filled by the user from the database

            var userEntries = _dbContext.Items
                .Include(i => i.User)
                .Where(i => i.UserId == userId)
                .ToList();

            //var userEntries = _itemRepository.GetByIdAsync(userId.Value);
            Console.WriteLine(userEntries);
            // Pass the user entries to the view
            return View(userEntries);
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
