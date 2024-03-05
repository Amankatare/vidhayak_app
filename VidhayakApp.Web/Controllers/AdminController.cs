using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infastructure.Repositories;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.Web.ViewModels;

namespace VidhayakApp.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly VidhayakAppContext _db;
        private readonly IUserRepository _user;
        private readonly IUserService _userService;

        
        
        public AdminController(VidhayakAppContext db, IUserRepository user, IUserService userService)
        {
            _db= db;
            _user= user;
            _userService= userService;
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


        public async Task<ActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [Route("Admin/CreateUser")]
        public async Task<ActionResult<User>> CreateUser(CreateUserModel model)
        {
            if (ModelState.IsValid)
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
                        PasswordHash = "$2a$11$82IdIaryQRhzpZWv8lDeZOFUevkJpdPh2MBCwdioBzcH2qSYRv2Mi",
                        RoleId = 3,
                    };

                    await _db.Users.AddAsync(user);
                    await _db.SaveChangesAsync();

                    TempData["Message"] = "User created successfully.";
                    Console.WriteLine(TempData["Message"]); 
                    Console.WriteLine(user);
                

                }
            }
            return RedirectToAction("Create", "Admin");
        }

        public async Task<ActionResult> Delete(int id)
        {
            var userRecord =await _user.GetByIdAsync(id);
            await DeleteAppUser(userRecord.UserId);
            return View();
        }


        
        public async Task<ActionResult<User>> DeleteAppUser(int id)
        {
            var userToDelete = await _user.GetByIdAsync(id);

            if (userToDelete != null)
            {
                await _user.DeleteAsync(userToDelete);
                TempData["Message"] = "User deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "User not found.";
            }

            return RedirectToAction("AppUsers");
        }

        public async Task<ActionResult<User>> UpdateAppUser(int id)
        {
            var userToDelete = await _user.GetByIdAsync(id);

            if (userToDelete != null)
            {
                await _user.DeleteAsync(userToDelete);
                TempData["Message"] = "User deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "User not found.";
            }

            return RedirectToAction("AppUsers");
        }




    }
}
