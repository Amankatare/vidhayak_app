using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.Web.ViewModels;
using VidhayakApp.Web.ViewModels.AppUser;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VidhayakApp.Web.Controllers
{
    //[Authorize(Policy = "RequireAppUserRole")]
    public class AppUserController : Controller
    {
        private readonly VidhayakAppContext _db;
        private readonly IUserRepository _user;
        private readonly IUserDetailRepository _userDetails;
        private readonly IUserService _userService;
        private readonly IUserDetailService _userDetailService;
        private readonly IWardRepository _wardRepository;
        private readonly IItemRepository _itemRepository;


        public AppUserController(VidhayakAppContext db, IUserRepository user, IUserService userService, IUserDetailRepository userDetails, IUserDetailService userdetailService, IWardRepository wardRepository, IItemRepository _itemRepository)
        {
            _db = db;
            _user = user;
            _userService = userService;
            _userDetails = userDetails;
            _userDetailService = userdetailService;
            _wardRepository = wardRepository;
            _itemRepository = _itemRepository;
        }
        //public IActionResult Dashboard(UserDetailAndFormDetailOnAppUserDashboardViewModel model)
        //{ var viewmodel = new UserDetailAndFormDetailOnAppUserDashboardViewModel
        //    return View();
        //}

        public IActionResult Dashboard()
        {
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
                item.Status == StatusType.Open &&
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
                UsersTotalCount= usersTotalCount,
            };

            return View(viewModel);
        }
        public IActionResult DepartmentsCard()
        {
            var viewModel = new DepartmentViewModel();

            viewModel.Departments = _db.GovtDepartments
                .Select(department => new DepartmentViewModel
                {   DepartmentId = department.DepartmentId,
                    DepartmentName = department.DepartmentName,
                    ComplaintsCount = _db.Items.Count(item =>
                        item.DepartmentId == department.DepartmentId && item.Type == ItemType.Complaint),
                    //PendingCount = _db.Items.Count(item =>
                    //    item.DepartmentId == department.DepartmentId && item.Type == ItemType.Complaint && item.Status == StatusType.Pending)
                    //SuggestionsCount = _db.Items.Count(item =>
                    //    item.DepartmentId == department.DepartmentId && item.Type == ItemType.Suggestion),
                    //DemandsCount = _db.Items.Count(item =>
                    //    item.DepartmentId == department.DepartmentId && item.Type == ItemType.Demand),
                    //OtherCount = _db.Items.Count(item =>
                    //    item.DepartmentId == department.DepartmentId && item.Type != ItemType.Complaint
                    //    && item.Type != ItemType.Suggestion && item.Type != ItemType.Demand),
                    //InProgressCount = _db.Items.Count(item =>
                    //    item.DepartmentId == department.DepartmentId && item.Status == StatusType.InProgress),
                    //RejectedCount = _db.Items.Count(item =>
                    //    item.DepartmentId == department.DepartmentId && item.Status == StatusType.Rejected),
                    //CompletedCount = _db.Items.Count(item =>
                    //    item.DepartmentId == department.DepartmentId && item.Status == StatusType.Completed)
                })
                .ToList();

            return View(viewModel);
        }

        public IActionResult ComplaintDepartmentWise(int departmentId)
        {
            var loggedInUser = HttpContext.Session.GetInt32("WardId");
            Console.WriteLine(loggedInUser + "-------------------------");

            var userDetailFormViewModels = _db.Users
                .Join(_db.Items,
                    user => user.UserId,
                    item => item.UserId,
                    (user, item) => new { User = user, Item = item })
                .Where(join => join.Item.Type == ItemType.Complaint && join.Item.DepartmentId == departmentId &&
                               join.User.RoleId == 4 &&
                               join.User.WardId == loggedInUser)
                 .OrderByDescending(join => join.Item.CreatedAt)
                .Join(_db.GovtDepartments, // Join with the Departments table
                    join => join.Item.DepartmentId, // Join condition: Item.DepartmentId equals Department.DepartmentId
                    department => department.DepartmentId,
                    (join, department) => new UserDetailAndFormDetailOnAppUserDashboardViewModel
                    {
                        UserId = join.User.UserId,
                        UserName = join.User.UserName,
                        Name = join.User.Name,
                        Address = join.User.Address,
                        MobileNumber = join.User.MobileNumber,
                        ItemId = join.Item.ItemId,
                        Type = join.Item.Type,
                        SubCategory = join.Item.SubCategoryTypeId,
                        Title = join.Item.Title,
                        Description = join.Item.Description,
                        CreatedAt = join.Item.CreatedAt,
                        UpdatedAt = join.Item.UpdatedAt,
                        Status = join.Item.Status,
                        Note = join.Item.Note,
                        // Include the DepartmentName from the joined Department entity
                        DepartmentName = department.DepartmentName
                    })
                .ToList();

            return View(userDetailFormViewModels);
        }


        public IActionResult Complaint(int? pageId, FilterViewModel filter)
        {
            var loggedInUserRoleId = HttpContext.Session.GetInt32("RoleId");
            var loggedInUserWardId = HttpContext.Session.GetInt32("WardId");
            Console.WriteLine(loggedInUserRoleId + "-------------------------");

            IQueryable<UserDetailAndFormDetailOnAppUserDashboardViewModel> query;

            if (loggedInUserRoleId == 2)
            {
                // If RoleId is 2, show all items
                query = _db.Items
                    .Where(item => item.Type == ItemType.Complaint)
                    .OrderByDescending(item => item.CreatedAt)
                    .Select(item => new UserDetailAndFormDetailOnAppUserDashboardViewModel
                    {
                        UserId = item.UserId,
                        UserName = item.User.UserName,
                        Name = item.User.Name,
                        Address = item.User.Address,
                        MobileNumber = item.User.MobileNumber,
                        ItemId = item.ItemId,
                        Type = item.Type,
                        SubCategory = item.SubCategoryTypeId,
                        Title = item.Title,
                        Description = item.Description,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt,
                        Status = item.Status,
                        Note = item.Note,
                        DepartmentName = item.Department.DepartmentName
                    });
            }
            else
            {
                // If RoleId is not 2, filter by WardId
                query = _db.Users
                    .Where(user => user.RoleId == 4 && user.WardId == loggedInUserWardId)
                    .Join(_db.Items,
                        user => user.UserId,
                        item => item.UserId,
                        (user, item) => new { User = user, Item = item })
                    .Where(join => join.Item.Type == ItemType.Complaint)
                    .OrderByDescending(join => join.Item.CreatedAt)
                    .Join(_db.GovtDepartments,
                        join => join.Item.DepartmentId,
                        department => department.DepartmentId,
                        (join, department) => new UserDetailAndFormDetailOnAppUserDashboardViewModel
                        {
                            UserId = join.User.UserId,
                            UserName = join.User.UserName,
                            Name = join.User.Name,
                            Address = join.User.Address,
                            MobileNumber = join.User.MobileNumber,
                            ItemId = join.Item.ItemId,
                            Type = join.Item.Type,
                            SubCategory = join.Item.SubCategoryTypeId,
                            Title = join.Item.Title,
                            Description = join.Item.Description,
                            CreatedAt = join.Item.CreatedAt,
                            UpdatedAt = join.Item.UpdatedAt,
                            Status = join.Item.Status,
                            Note = join.Item.Note,
                            DepartmentName = department.DepartmentName
                        });
            }

            if (filter.statusType != null)
            {
                // Apply status filter if provided
                query = query.Where(item => item.Status == filter.statusType);
            }

            if (filter.FromDate != null && filter.ToDate != null)
            {
                // Apply date filter if provided
                query = query.Where(item => item.CreatedAt >= filter.FromDate && item.CreatedAt <= filter.ToDate);
            }

            var pageNumber = pageId ?? 1;
            var pageSize = 10;
            var pagedData = query.ToPagedList(pageNumber, pageSize);
            filter.userDetailAndFormDetailOnAppUserDashboardViewModel = pagedData;

            return View(filter);
        }



        public IActionResult Demand(int? pageId, FilterViewModel filter)
        {
            var loggedInUserRoleId = HttpContext.Session.GetInt32("RoleId");
            var loggedInUserWardId = HttpContext.Session.GetInt32("WardId");
            Console.WriteLine(loggedInUserWardId + "-------------------------");

            IQueryable<UserDetailAndFormDetailOnAppUserDashboardViewModel> query;

            if (loggedInUserRoleId == 2)
            {
                // If RoleId is 2, show all items
                query = _db.Items
                    .Where(item => item.Type == ItemType.Demand)
                    .OrderByDescending(item => item.CreatedAt)
                    .Select(item => new UserDetailAndFormDetailOnAppUserDashboardViewModel
                    {
                        UserId = item.UserId,
                        UserName = item.User.UserName,
                        Name = item.User.Name,
                        Address = item.User.Address,
                        MobileNumber = item.User.MobileNumber,
                        ItemId = item.ItemId,
                        Type = item.Type,
                        SubCategory = item.SubCategoryTypeId,
                        Title = item.Title,
                        Description = item.Description,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt,
                        Status = item.Status,
                        SchemeName = item.Scheme.SchemeName
                    });
            }
            else
            {
                // If RoleId is not 2, filter by WardId
                query = _db.Users
                    .Where(user => user.RoleId == 4 && user.WardId == loggedInUserWardId)
                    .Join(_db.Items,
                        user => user.UserId,
                        item => item.UserId,
                        (user, item) => new { User = user, Item = item })
                    .Where(join => join.Item.Type == ItemType.Demand)
                    .OrderByDescending(join => join.Item.CreatedAt)
                    .Join(_db.GovtSchemes,
                        join => join.Item.SchemeId,
                        scheme => scheme.SchemeId,
                        (join, scheme) => new UserDetailAndFormDetailOnAppUserDashboardViewModel
                        {
                            UserId = join.User.UserId,
                            UserName = join.User.UserName,
                            Name = join.User.Name,
                            Address = join.User.Address,
                            MobileNumber = join.User.MobileNumber,
                            ItemId = join.Item.ItemId,
                            Type = join.Item.Type,
                            SubCategory = join.Item.SubCategoryTypeId,
                            Title = join.Item.Title,
                            Description = join.Item.Description,
                            CreatedAt = join.Item.CreatedAt,
                            UpdatedAt = join.Item.UpdatedAt,
                            Status = join.Item.Status,
                            SchemeName = scheme.SchemeName
                        });
            }

            if (filter.statusType != null)
            {
                // Apply status filter if provided
                query = query.Where(item => item.Status == filter.statusType);
            }

            if (filter.FromDate != null && filter.ToDate != null)
            {
                // Apply date filter if provided
                query = query.Where(item => item.CreatedAt >= filter.FromDate && item.CreatedAt <= filter.ToDate);
            }

            var pageNumber = pageId ?? 1;
            var pageSize = 10;
            var pagedData = query.ToPagedList(pageNumber, pageSize);
            filter.userDetailAndFormDetailOnAppUserDashboardViewModel = pagedData;

            return View(filter);
        }

        public IActionResult Suggestion(int? pageId, SuggestionDetailsAndStatusUpdate model)
        {
            var loggedInUserRoleId = HttpContext.Session.GetInt32("RoleId");
            var loggedInUserWardId = HttpContext.Session.GetInt32("WardId");
            Console.WriteLine(loggedInUserWardId + "-------------------------");

            // Fetch suggestion data from the database or any other source
            IQueryable<UserDetailAndFormDetailOnAppUserDashboardViewModel> suggestions;

            if (loggedInUserRoleId == 2)
            {
                // If RoleId is 2, show all items
                suggestions = _db.Items
                    .Where(item => item.Type == ItemType.Suggestion)
                    .OrderByDescending(item => item.CreatedAt)
                    .Select(item => new UserDetailAndFormDetailOnAppUserDashboardViewModel
                    {
                        UserName = item.User.UserName,
                        Name = item.User.Name,
                        Address = item.User.Address,
                        MobileNumber = item.User.MobileNumber,
                        ItemId = item.ItemId,
                        Type = item.Type,
                        SubCategory = item.SubCategoryTypeId,
                        Title = item.Title,
                        Description = item.Description,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt,
                        Status = item.Status
                    });
            }
            else
            {
                // If RoleId is not 2, filter by WardId
                suggestions = _db.Items
                    .Where(item => item.Type == ItemType.Suggestion && item.User.RoleId == 4 && item.User.WardId == loggedInUserWardId)
                    .OrderByDescending(item => item.CreatedAt)
                    .Select(item => new UserDetailAndFormDetailOnAppUserDashboardViewModel
                    {
                        UserName = item.User.UserName,
                        Name = item.User.Name,
                        Address = item.User.Address,
                        MobileNumber = item.User.MobileNumber,
                        ItemId = item.ItemId,
                        Type = item.Type,
                        SubCategory = item.SubCategoryTypeId,
                        Title = item.Title,
                        Description = item.Description,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt,
                        Status = item.Status
                    });
            }

            if (model.statusType != null)
            {
                // Apply status filter if provided
                suggestions = suggestions.Where(item => item.Status == model.statusType);
            }

            if (model.FromDate != null && model.ToDate != null)
            {
                // Apply date filter if provided
                suggestions = suggestions.Where(item => item.CreatedAt >= model.FromDate && item.CreatedAt <= model.ToDate);
            }

            var pageNumber = pageId ?? 1;
            var pageSize = 10;
            var pagedData = suggestions.ToPagedList(pageNumber, pageSize);

            // Create the view model and pass the suggestions data to it
            var viewModel = new SuggestionDetailsAndStatusUpdate
            {
                UserDetailAndFormDetailOnAppUserDashboardViewModel = pagedData
            };

            // Render the view with the view model
            return View(viewModel);
        }



        public async Task<ActionResult> UpdateOnUserSuggestionOnAppUserDashboard(int itemId)
        {

            //var user = await _db.Users.FindAsync(userObject.UserId);
            var viewModel = new UpdateOnUserIssuesByAppUser
            {
                ItemId = itemId,
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<ActionResult> UpdateOnUserSuggestionOnAppUserDashboard(SuggestionDetailsAndStatusUpdate model)
        {
         
            // Find the ItemId associated with the specified user ID using a join
            var itemObject = await _db.Items.FindAsync(model.ItemId);
            Console.WriteLine(itemObject + "---------------------------------------------");
            itemObject.AppUserId = model.AppUserId;
            itemObject.Status = model.Status;
            itemObject.UpdatedAt = model.UpdatedAt ?? DateTime.Now.Date;
     
            await _db.SaveChangesAsync();

            return RedirectToAction("Dashboard", "AppUser");
        }

            public async Task<ActionResult> UpdateOnUserIssuesOnAppUserDashboard( int itemId)
            {


            //var user = await _db.Users.FindAsync(userObject.UserId);
            var viewModel = new UpdateOnUserIssuesByAppUser
            {
                ItemId = itemId,
            };


            return View(viewModel);
        }

       
        [HttpPost]
        public async Task<ActionResult> UpdateOnUserIssuesOnAppUserDashboard(int id, UpdateOnUserIssuesByAppUser model)
        {

            // Find the ItemId associated with the specified user ID using a join
            var itemObject = await _db.Items.FindAsync(id);
            Console.WriteLine(itemObject + "---------------------------------------------");
            itemObject.AppUserId = model.AppUserId;
            itemObject.Status = model.Status;
            itemObject.UpdatedAt = model.UpdatedAt ?? DateTime.Now.Date;
            itemObject.Note = model.Note;
            await _db.SaveChangesAsync();
            
            return RedirectToAction("Dashboard", "AppUser");

            //Console.WriteLine(itemObject.Result);



            //foreach (var item in itemObject)
            //            {
            //                item.AppUserId = model.AppUserId;
            //                item.Status = model.Status;
            //                item.Note = model.Note;
            //                item.UpdatedAt = model.UpdatedAt ?? DateTime.Now;
            //            }

            //            // Save changes to the database
            //            await _db.SaveChangesAsync();

            //             // Redirect to dashboard or any other desired page
            //        }
            //        else
            //        {
            //            ModelState.AddModelError(string.Empty, "No items found for the specified user ID and ItemId.");
            //        }
            //    }
            //    else
            //    {
            //        ModelState.AddModelError(string.Empty, "No items found for the specified user.");
            //    }


            // If ModelState is not valid or if the item is not found, return to the view with the model
           // return View(model);
        }






        public IActionResult Profile()
        {
            return View();
        }  

           
        
        
        
        
        
        /// <summary>

                    //below this the code is For user creation, updation and deletion and listing on AppUser Account

            /// </summary>
            /// <returns></returns>

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
                else
                {
                    var wardObject = await _wardRepository.GetByIdAsync(model.WardId);

                    var user = new User
                    {
                        Name = model.Name,
                        UserName = model.UserName,
                        Ward = wardObject,
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
                        VoterCount = model.VoterCount,
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


        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var userToDelete = await _user.GetByIdAsync(id);
            if (userToDelete == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("ListUsers", "AppUser");
            }

            return View(userToDelete);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var userToDelete = await _user.GetByIdAsync(id);
            var userDetailToDelete = await _userDetailService.GetUserDetailsByUserIdAsync(id);

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

            return RedirectToAction("ListUsers", "AppUser");
        }
    
    public async Task<ActionResult> Edit(int id)
        { 
            var user = await _user.GetByIdAsync(id);

            var userDetails = await _userDetailService.GetUserDetailsByUserIdAsync(id);

          
            if (user == null)
            {
                return NotFound();
            }

            var wardObject = await _wardRepository.GetByIdAsync(user.WardId);
            var viewModel = new UserWithDetailsViewModel
            {
                UserName = user.UserName,
                Name = user.Name,
                Dob = user.Dob,
                Address = user.Address,
                WardId = user.WardId,
                Ward = wardObject,
                MobileNumber = user.MobileNumber,
                PasswordHash = user.PasswordHash,
                RoleId = 4,

                // Update properties in userDetails entity
                Education = userDetails.Education, // Remove extra dot
                AadharNumber = userDetails.AadharNumber,
                SamagraID = userDetails.SamagraID,
                VoterID = userDetails.VoterID,
                VoterCount = userDetails.VoterCount,
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
                var userDetailsToUpdate = await _userDetailService.GetByUserIdAsync(userToUpdate.UserId);
                var wardObject = await _wardRepository.GetByIdAsync(viewModel.WardId);
                // If user details exist, proceed with the update
                if (userDetailsToUpdate != null)
                {
                    userToUpdate.Name = viewModel.Name;
                    userToUpdate.UserName = viewModel.UserName;
                    userToUpdate.Dob = viewModel.Dob;
                    userToUpdate.Address = viewModel.Address;
                    userToUpdate.WardId = viewModel.WardId;
                    userToUpdate.Ward = wardObject;
                    userToUpdate.MobileNumber = viewModel.MobileNumber;
                    userToUpdate.PasswordHash = "$2a$11$82IdIaryQRhzpZWv8lDeZOFUevkJpdPh2MBCwdioBzcH2qSYRv2Mi";
                    userToUpdate.RoleId = 4;

                    userDetailsToUpdate.Education = viewModel.Education;
                    userDetailsToUpdate.AadharNumber = viewModel.AadharNumber;
                    userDetailsToUpdate.SamagraID = viewModel.SamagraID;
                    userDetailsToUpdate.VoterID = viewModel.VoterID;
                    userDetailsToUpdate.VoterCount = viewModel.VoterCount;
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
