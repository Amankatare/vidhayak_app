using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.Web.ViewModels;

namespace VidhayakApp.Web.Controllers
{
    //[Authorize(Policy = "RequireAppUserRole")]
    public class AppUserController : Controller
    {
        private readonly VidhayakAppContext _db;
        private readonly IUserRepository _user;
        private readonly IUserDetailRepository _userDetails;
        private readonly IUserService _userService;



        public AppUserController(VidhayakAppContext db, IUserRepository user, IUserService userService, IUserDetailRepository userDetails)
        {
            _db = db;
            _user = user;
            _userService = userService;
            _userDetails= userDetails;
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
        public IActionResult ListUsers()
        {
            // Retrieve all users from the Users table
         
            var viewModel = new ShowUserandItsDetails
            {
                usersTable = _db.Users.Where(s => s.RoleId == 4).ToList().ToList(),
                userDetailsTable = _db.UserDetails.ToList(),
                // Retrieve data for additional tables if needed
            };

            return View(viewModel);
        }


        public async Task<ActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [Route("AppUser/CreateUser")]
        public async Task<ActionResult<User>> CreateUser(CreateNormalUserModel model)
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
                        RoleId = 4,
                    };

                    var userDetail = new UserDetail
                    {
                        Education = model.Education,
                        AadharNumber = model.AadharNumber,
                        SamagraID = model.SamagraID,
                        VoterID = model.VoterID,
                        Caste = model.Caste,
                        UserId = user.UserId // Set the UserId here
                    };

                    await _db.Users.AddAsync(user);
                    await _db.SaveChangesAsync(); // SaveChanges to get the UserId

                    userDetail.UserId = user.UserId; // Set the UserId in UserDetail
                    await _db.UserDetails.AddAsync(userDetail);
                    await _db.SaveChangesAsync();

                    TempData["Message"] = "User created successfully.";
                    Console.WriteLine(TempData["Message"]);
                    Console.WriteLine(user);
                }
            }

            return RedirectToAction("Create", "AppUser");
        }


        public async Task<ActionResult> Delete(int id)
        {
            var userRecord = await _user.GetByIdAsync(id);
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
            var userDetails = await _userDetails.GetByIdAsync(id);

            var viewModel = new UserViewModel
            {
                Name = userRecord.Name,
                UserName = userRecord.UserName,
                Dob = userRecord.Dob,
                Address = userRecord.Address,
                Ward = userRecord.Ward,
                MobileNumber = userRecord.MobileNumber,
                RoleId = 4,

                // UserDetail properties

                Education = userDetails?.Education,
                AadharNumber = userDetails?.AadharNumber,
                SamagraID = userDetails?.SamagraID,
                VoterID = userDetails?.VoterID,
                Caste = userDetails?.Caste
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult<UserViewModel>> UpdateUser(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userToUpdate = await _user.GetByIdAsync(viewModel.UserId);
                var userDetailsToUpdate = await _userDetails.GetByIdAsync(viewModel.UserId);

                if (userToUpdate != null && userDetailsToUpdate != null)
                {
                    // Update properties in user entity
                    userToUpdate.Name = viewModel.Name;
                    userToUpdate.UserName = viewModel.UserName;
                    userToUpdate.Dob = viewModel.Dob;
                    userToUpdate.Address = viewModel.Address;
                    userToUpdate.Ward = viewModel.Ward;
                    userToUpdate.MobileNumber = viewModel.MobileNumber;
                    userToUpdate.PasswordHash = viewModel.PasswordHash;
                    userToUpdate.RoleId = viewModel.RoleId;

                    // Update properties in userDetails entity
                    userDetailsToUpdate.Education = viewModel.Education;
                    userDetailsToUpdate.AadharNumber = viewModel.AadharNumber;
                    userDetailsToUpdate.SamagraID = viewModel.SamagraID;
                    userDetailsToUpdate.VoterID = viewModel.VoterID;
                    userDetailsToUpdate.Caste = viewModel.Caste;

                    // Perform updates
                    await _user.UpdateAsync(userToUpdate);
                    await _userDetails.UpdateAsync(userDetailsToUpdate);

                    TempData["Message"] = "User Updated successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "User not found.";
                }
            }
            else
            {
                // ModelState is not valid, handle accordingly (e.g., redisplay the form with errors)
            }

            return RedirectToAction("AppUsers");
        }

    }
}
