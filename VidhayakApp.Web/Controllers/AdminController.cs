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
    [Authorize]
    public class AdminController : Controller
    {
        private readonly VidhayakAppContext _db;
        private readonly IUserRepository _user;
        private readonly IWardRepository _wardRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleService _roleService;

        public AdminController(VidhayakAppContext db, IUserRepository user, IUserService userService, IHttpContextAccessor httpContextAccessor, IRoleService roleService, IWardRepository wardRepository, IItemRepository itemRepository)
        {
            _db = db;
            _user = user;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
            _wardRepository = wardRepository;
            _itemRepository = itemRepository;
        }

        [HttpGet]
        [Route("Admin/Dashboard")]
        public IActionResult Dashboard()
        {
            Console.WriteLine("Admin Dashboard");
            var loggedInUser = HttpContext.Session.GetInt32("WardId");

            // Fetch counts of pending complaints, demands, and suggestions
            var pendingComplaintsCount = _db.Items.Count(item =>
                item.Type == ItemType.Complaint &&
                item.Status == StatusType.Pending &&
                item.User.WardId == loggedInUser);

            var pendingDemandsCount = _db.Items.Count(item =>
                item.Type == ItemType.Demand &&
                item.Status == StatusType.Pending &&
                item.User.WardId == loggedInUser);

            var pendingSuggestionsCount = _db.Items.Count(item =>
                item.Type == ItemType.Suggestion &&
                item.Status == StatusType.Pending &&
                item.User.WardId == loggedInUser);

            // Fetch counts of total complaints, demands, and suggestions
            var totalComplaintsCount = _db.Items.Count(item =>
                item.Type == ItemType.Complaint &&
                item.User.WardId == loggedInUser);

            var totalDemandsCount = _db.Items.Count(item =>
                item.Type == ItemType.Demand &&
                item.User.WardId == loggedInUser);

            var totalSuggestionsCount = _db.Items.Count(item =>
                item.Type == ItemType.Suggestion &&
                item.User.WardId == loggedInUser);

            // Fetch count of users in the same ward
            var usersInWardCount = _db.Users.Count(user =>
                user.WardId == loggedInUser);

            var usersTotalCount = _db.Users.Count();

            // Pass the counts to the view model
            var viewModel = new DashboardViewModel
            {
                PendingComplaintsCount = pendingComplaintsCount,
                PendingDemandsCount = pendingDemandsCount,
                PendingSuggestionsCount = pendingSuggestionsCount,
                TotalComplaintsCount = totalComplaintsCount,
                TotalDemandsCount = totalDemandsCount,
                TotalSuggestionsCount = totalSuggestionsCount,
                UsersInWardCount = usersInWardCount,
                UsersTotalCount = usersTotalCount,
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Departments()
        {
            var viewModel = new DepartmentViewModel();

            viewModel.Departments = _db.GovtDepartments
                .Select(department => new DepartmentViewModel
                {
                    DepartmentName = department.DepartmentName,
                    ComplaintsCount = _db.Items.Count(item =>
                        item.DepartmentId == department.DepartmentId && item.Type == ItemType.Complaint),
                    SuggestionsCount = _db.Items.Count(item =>
                        item.DepartmentId == department.DepartmentId && item.Type == ItemType.Suggestion),
                    DemandsCount = _db.Items.Count(item =>
                        item.DepartmentId == department.DepartmentId && item.Type == ItemType.Demand),
                    OtherCount = _db.Items.Count(item =>
                        item.DepartmentId == department.DepartmentId && item.Type != ItemType.Complaint
                        && item.Type != ItemType.Suggestion && item.Type != ItemType.Demand),
                    PendingCount = _db.Items.Count(item =>
                        item.DepartmentId == department.DepartmentId && item.Status == StatusType.Pending),
                    InProgressCount = _db.Items.Count(item =>
                        item.DepartmentId == department.DepartmentId && item.Status == StatusType.InProgress),
                    RejectedCount = _db.Items.Count(item =>
                        item.DepartmentId == department.DepartmentId && item.Status == StatusType.Rejected),
                    CompletedCount = _db.Items.Count(item =>
                        item.DepartmentId == department.DepartmentId && item.Status == StatusType.Completed)
                })
                .ToList();

            return View(viewModel);
        }

        public IActionResult Schemes()
        {
            return View();
        }

        [HttpGet]
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

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            // Retrieve all users from the Users table

            var viewModel = new ShowUserandItsDetails
            {
                usersTable = _db.Users.Where(s => s.RoleId == 4).ToList(),
                userDetailsTable = _db.UserDetails.ToList(),
                userWardTable = _db.Wards.ToList() // Populate userWardTable
                                                   // Retrieve data for additional tables if needed
            };

            return View(viewModel);
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
            return RedirectToAction("AppUsers", "Admin");
        }
        [HttpGet]
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

        [HttpGet]
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
