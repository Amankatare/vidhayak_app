using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infastructure.Repositories;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.SharedKernel.Utilities;
using VidhayakApp.Web.ViewModels;

namespace VidhayakApp.Web.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly VidhayakAppContext _db;
        private readonly IUserRepository _user;
        private readonly IWardRepository _wardRepository;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleService _roleService;

        public AdminController(VidhayakAppContext db, IUserRepository user, IUserService userService, IHttpContextAccessor httpContextAccessor, IRoleService roleService, IWardRepository wardRepository)
        {
            _db = db;
            _user = user;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
            _wardRepository = wardRepository;
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
            var users = _db.Users.Where(u => u.RoleId == 3).ToList();

            // Retrieve all user details
            var userDetails = _db.UserDetails.ToList();

            // Retrieve all wards
            var wards = _db.Wards.ToList();

            // Create an instance of the ShowUserandItsDetails view model
            var viewModel = new ShowUserandItsDetails
            {
                usersTable = users,
                userDetailsTable = userDetails,
                userWardTable = wards
            };

            return View(viewModel);
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
                
                else
                {
                    var wardObject = await _wardRepository.GetByIdAsync(model.WardId);

                    var user = new User
                    {
                        Name = model.Name,
                        UserName = model.UserName,
                        WardId = model.WardId,
                        Ward = wardObject,
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

        public async Task<ActionResult> Edit(int id)
        {
            var userRecord = await _user.GetByIdAsync(id);
         
            return View(userRecord);
        }

        public async Task<ActionResult<User>> UpdateAppUser(User s)
        {
            var userToUpdate = await _user.GetByIdAsync(s.UserId);

            if (userToUpdate != null)
            {
                await _user.UpdateAsync(userToUpdate);
                TempData["Message"] = "User Updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "User not found.";
            }

            return RedirectToAction("AppUsers");
        }




    }
}
