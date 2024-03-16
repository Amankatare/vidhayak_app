using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.Web.ViewModels;
using VidhayakApp.Web.ViewModels.AppUser;

namespace VidhayakApp.Web.Controllers
{
    //[Authorize(Policy = "RequireAppUserRole")]
    public class AppUserController : Controller
    {
        private readonly VidhayakAppContext _db;
        private readonly IUserRepository _user;
        private readonly IUserDetailRepository _userDetails;
        private readonly IUserService _userService;
        private readonly IUserDetailService _userdetailService;



        public AppUserController(VidhayakAppContext db, IUserRepository user, IUserService userService, IUserDetailRepository userDetails, IUserDetailService userdetailService)
        {
            _db = db;
            _user = user;
            _userService = userService;
            _userDetails= userDetails;
            _userdetailService= userdetailService;
        }
        public IActionResult Dashboard()
        {
            var userDetailFormViewModels = _db.Users
            .Join(_db.Items,
                user => user.UserId,
                item => item.UserId,
                (user, item) => new { User = user, Item = item })
                .Where(join => (join.Item.Type == ItemType.Complaint ||
                            join.Item.Type == ItemType.Demand ||
                            join.Item.Type == ItemType.Suggession) &&
                            join.User.RoleId == 4 &&
                            join.User.Ward == "garha")
             .Select(join => new UserDetailAndFormDetailOnAppUserDashboardViewModel
                 {
                     UserId = join.User.UserId,
                     UserName = join.User.UserName,
                     Name = join.User.Name,
                     Address = join.User.Address,
                     MobileNumber = join.User.MobileNumber,
                     Type = join.Item.Type,
                     SubCategory = join.Item.SubCategoryTypeId,
                     Title = join.Item.Title,
                     Description = join.Item.Description,
                     CreatedAt = join.Item.CreatedAt,
                     UpdatedAt = join.Item.UpdatedAt,
                     Status = join.Item.Status,
                     Note = join.Item.Note
                 })
                 .ToList();

            return View(userDetailFormViewModels);

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

            if (userRecord != null)
            {
                await DeleteAppUser(id);
           
            }
            else
            {
                TempData["ErrorMessage"] = "User not found.";
            }
            return RedirectToAction("ListUsers");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAppUser(int id)
        {
            var userToDelete = await _user.GetByIdAsync(id);
            var userDetailToDelete = await _db.UserDetails.FindAsync(id);

            if (userToDelete != null && userDetailToDelete != null)
            {
             
                await _userDetails.DeleteAsync(userDetailToDelete);

            
                await _user.DeleteAsync(userToDelete);
                TempData["Message"] = "User deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "User not found.";
            }

            return RedirectToAction("ListUsers");
    }

        

        public async Task<ActionResult> Edit(int id)
        { 
            var user = _db.Users.Find(id);
            var userDetails = _db.UserDetails.FirstOrDefault(d => d.UserId == id);

          
            if (user == null || userDetails == null)
            {
                return NotFound();
            }

            var viewModel = new UserWithDetailsViewModel
            {
                UserName = user.UserName,
                Name = user.Name,
              
                Dob = user.Dob,
                Address = user.Address,
                Ward = user.Ward,
                MobileNumber = user.MobileNumber,
                PasswordHash = user.PasswordHash,
                RoleId = 4,

                // Update properties in userDetails entity
                Education = userDetails.Education,
                AadharNumber = userDetails.AadharNumber,
                SamagraID = userDetails.SamagraID,
                VoterID = userDetails.VoterID,
                Caste = userDetails.Caste,
         
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult<ShowUserandItsDetails>> UpdateUser(UserWithDetailsViewModel viewModel)
        {
            var userToUpdate = await _user.GetByUsernameAsync(viewModel.UserName);

            if (userToUpdate != null)
            {
                // Get user details based on the user ID
                var userDetailsToUpdate = await _userdetailService.GetByUserIdAsync(userToUpdate.UserId);

                // If user details exist, proceed with the update
                if (userDetailsToUpdate != null)
                {
                    userToUpdate.Name = viewModel.Name;
                    userToUpdate.UserName = viewModel.UserName;
                    userToUpdate.Dob = viewModel.Dob;
                    userToUpdate.Address = viewModel.Address;
                    userToUpdate.Ward = viewModel.Ward;
                    userToUpdate.MobileNumber = viewModel.MobileNumber;
                    userToUpdate.PasswordHash = "$2a$11$82IdIaryQRhzpZWv8lDeZOFUevkJpdPh2MBCwdioBzcH2qSYRv2Mi";
                    userToUpdate.RoleId = 4;

                    userDetailsToUpdate.Education = viewModel.Education;
                    userDetailsToUpdate.AadharNumber = viewModel.AadharNumber;
                    userDetailsToUpdate.SamagraID = viewModel.SamagraID;
                    userDetailsToUpdate.VoterID = viewModel.VoterID;
                    userDetailsToUpdate.Caste = viewModel.Caste;

                    // Perform updates
                    await _user.UpdateAsync(userToUpdate);
                    await _userDetails.UpdateAsync(userDetailsToUpdate);

                    TempData["Message"] = "User Updated successfully.";

                    // Create a new ShowUserandItsDetails view model
                    var showViewModel = new ShowUserandItsDetails
                    {
                        usersTable = new List<User> { userToUpdate },
                        userDetailsTable = new List<UserDetail> { userDetailsToUpdate }
                    };

                    // Return the ShowUserandItsDetails view with the updated data
                    return View("ListUsers", showViewModel);
                }
                else
                {
                    TempData["ErrorMessage"] = "User details not found.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "User not found.";
            }

            return View("ListUsers", "AppUser");
        }











    }
}
