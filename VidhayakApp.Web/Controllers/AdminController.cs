using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VidhayakApp.Core.Entities;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.Web.ViewModels;

namespace VidhayakApp.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly VidhayakAppContext _db;
        public AdminController(VidhayakAppContext db)
        {
            _db= db;
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        
        public IActionResult Tickets()
        {
            return View();
        }



        [HttpGet]
        public IActionResult AppUsers()
        {
            // Retrieve all users from the Users table
            var allUsers = _db.Users.Where(s=>s.RoleId==3).ToList();

            return View(allUsers);
        }



        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Name == null)
                    {
                        // Handle the case where Name is null
                        throw new ArgumentNullException(nameof(model.Name), "Name cannot be null");
                    }
                    else if (model.UserName == null)
                    {
                        throw new ArgumentException(nameof(model.UserName), "UserName doesn't contain @");
                    }
                    else if (model.Ward == null)
                    {
                        throw new ArgumentException(nameof(model.Ward), "Ward cannot contain numerical values");
                    }
                    else
                    {
                        var user = new User
                        {
                            Name = model.Name,
                            UserName = model.UserName,
                            Ward = model.Ward,
                            MobileNumber = model.MobileNumber,
                            Address = model.Address,
                            RoleId = 3
                        };

                        await _db.Users.AddAsync(user);
                        await _db.SaveChangesAsync();

                        TempData["Message"] = "User created successfully.";

                        return View(user);
                    }
                }
                catch (Exception ex)
                {
                  
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return BadRequest(ModelState); 
                }
            }

            return View(ModelState);
        }
    }
}
