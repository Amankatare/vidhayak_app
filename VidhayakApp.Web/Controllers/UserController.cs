
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.ViewModels;
using VidhayakApp.Web.ViewModels;
using VidhayakApp.Web.ViewModels.AppUser;
using X.PagedList;

namespace VidhayakApp.Web.Controllers
{
    //[Authorize(Policy = "RequireUserRole")]
    
    public class UserController : Controller
    {

        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly VidhayakAppContext _db;

        public UserController(IUserRepository userRepository, VidhayakAppContext db)
        {
            _userRepository = userRepository;
            _db = db;
        }

        public IActionResult Dashboard()
        {
            // Get the logged-in user's ID from session
            var loggedInId = HttpContext.Session.GetInt32("UserId");

            // Redirect to login page if user is not logged in
            if (loggedInId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch counts of submitted applications for Complaint, Demand, and Suggestion
            var complaintCount = _db.Items.Count(item => item.Type == ItemType.Complaint && item.UserId == loggedInId);
            var demandCount = _db.Items.Count(item => item.Type == ItemType.Demand && item.UserId == loggedInId);
            var suggestionCount = _db.Items.Count(item => item.Type == ItemType.Suggestion && item.UserId == loggedInId);

            // Pass the counts to the view
            ViewData["ComplaintCount"] = complaintCount;
            ViewData["DemandCount"] = demandCount;
            ViewData["SuggestionCount"] = suggestionCount;

            return View();
        }

            //public async Task<IActionResult> Dashboard()
            //{
            //    // Retrieve the user's details based on the user ID
            //    var userId = HttpContext.Session.GetInt32("UserId");
            //    if (userId == null)
            //    {
            //        // Handle if user ID is not found in the session
            //        return RedirectToAction("Login", "Account");
            //    }

            //    var user = await _userRepository.GetByIdAsync(userId.Value);

            //    // Retrieve complaints, demands, and suggestions filled by the user from the database

            //    var userEntries = _dbContext.Items
            //        .Include(i => i.User)
            //        .Where(i => i.UserId == userId).OrderByDescending(i => i.CreatedAt)
            //        .ToList();

            //    //var userEntries = _itemRepository.GetByIdAsync(userId.Value);
            //    Console.WriteLine(userEntries);
            //    // Pass the user entries to the view
            //    return View(userEntries);
            //}

            public IActionResult Profile()
        {
            return View();
        }



        public async Task<IActionResult> Complaints(int? pageId, UserFilter filter)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                // Handle if user ID is not found in the session
                return RedirectToAction("Login", "Account");
            }

            var user = await _userRepository.GetByIdAsync(userId.Value);

            // Retrieve complaints, demands, and suggestions filled by the user from the database
            var userEntries = _db.Items
                .Include(i => i.User)
                .Where(i => i.UserId == userId && i.Type == ItemType.Complaint);

            // Apply filtering based on the provided filter parameters
            if (filter?.FromDate != default && filter?.ToDate != default)
            {
                userEntries = userEntries.Where(s => s.CreatedAt >= filter.FromDate && s.CreatedAt <= filter.ToDate);
            }

            if (filter?.statusType != null)
            {
                userEntries = userEntries.Where(s => s.Status == filter.statusType);
            }

            // Order the entries by CreatedAt
            userEntries = userEntries.OrderByDescending(i => i.CreatedAt);

            // Pagination
            var pageNumber = pageId ?? 1;
            var pageSize = 10;
            var pagedData = userEntries.ToPagedList(pageNumber, pageSize);

            // Assign paged data directly to the Items property of the UserFilter
            filter.Items = pagedData;

            // Pass the filter object to the view
            return View(filter);
        }

        public async Task<IActionResult> Demand(int? pageId, UserFilter filter)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                // Handle if user ID is not found in the session
                return RedirectToAction("Login", "Account");
            }

            var user = await _userRepository.GetByIdAsync(userId.Value);

            // Retrieve complaints, demands, and suggestions filled by the user from the database
            var userEntries = _db.Items
                .Include(i => i.User)
                .Where(i => i.UserId == userId && i.Type == ItemType.Demand);

            // Apply filtering based on the provided filter parameters
            if (filter?.FromDate != default && filter?.ToDate != default)
            {
                userEntries = userEntries.Where(s => s.CreatedAt >= filter.FromDate && s.CreatedAt <= filter.ToDate);
            }

            if (filter?.statusType != null)
            {
                userEntries = userEntries.Where(s => s.Status == filter.statusType);
            }

            // Order the entries by CreatedAt
            userEntries = userEntries.OrderByDescending(i => i.CreatedAt);

            // Pagination
            var pageNumber = pageId ?? 1;
            var pageSize = 10;
            var pagedData = userEntries.ToPagedList(pageNumber, pageSize);

            // Assign paged data directly to the Items property of the UserFilter
            filter.Items = pagedData;

            // Pass the filter object to the view
            return View(filter);
        }

        public async Task<IActionResult> Suggestion(int? pageId, UserFilter filter)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                // Handle if user ID is not found in the session
                return RedirectToAction("Login", "Account");
            }

            var user = await _userRepository.GetByIdAsync(userId.Value);

            // Retrieve complaints, demands, and suggestions filled by the user from the database
            var userEntries = _db.Items
                .Include(i => i.User)
                .Where(i => i.UserId == userId && i.Type == ItemType.Suggestion);

            // Apply filtering based on the provided filter parameters
            if (filter?.FromDate != default && filter?.ToDate != default)
            {
                userEntries = userEntries.Where(s => s.CreatedAt >= filter.FromDate && s.CreatedAt <= filter.ToDate);
            }

            if (filter?.statusType != null)
            {
                userEntries = userEntries.Where(s => s.Status == filter.statusType);
            }

            // Order the entries by CreatedAt
            userEntries = userEntries.OrderByDescending(i => i.CreatedAt);

            // Pagination
            var pageNumber = pageId ?? 1;
            var pageSize = 10;
            var pagedData = userEntries.ToPagedList(pageNumber, pageSize);

            // Assign paged data directly to the Items property of the UserFilter
            filter.Items = pagedData;

            // Pass the filter object to the view
            return View(filter);
        }



        //public async Task<IActionResult> Complaints(int? pageId)
        //{
        //    var loggedInUserId = HttpContext.Session.GetInt32("UserId");
        //    if (loggedInUserId == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    // Fetch items asynchronously
        //    var items = await _itemRepository.GetItemsByUserIdAsync(loggedInUserId.Value);

        //    if (items == null || !items.Any())
        //    {
        //        return View("Nothing found");
        //    }

        //    // Filter complaint items
        //    var complaintItems = items.Where(item => item.Type == ItemType.Complaint).ToList();

        //    return View(complaintItems);
        //}



        // Create ViewModel and populate user details


        //    var complaintsQuery = _db.Items
        //.Where(item => item.UserId == loggedInUserId && item.Type == ItemType.Complaint)
        //.Select(item => new ComplaintViewModel
        //{
        //    ItemId = item.ItemId,
        //    Title = item.Title,
        //    Description = item.Description,
        //    Status = item.Status,
        //    CreatedAt = item.CreatedAt,
        //    UpdatedAt = item.UpdatedAt,
        //    AppUserId = item.UserId,
        //    Note = item.Note,
        //    Type = item.Type,
        //    UserId = item.UserId,
        //    DepartmentId = item.Department.DepartmentId,
        //    DepartmentName = item.Department.DepartmentName // Assuming DepartmentName is a property of Department entity
        //});

        //    // Configure pagination parameters
        //   int pageNumber = pageId ?? 1;
        //int pageSize = 10; // Number of items per page

        //// Create paged list
        //var pagedComplaints = complaintsQuery.ToPagedList(pageNumber, pageSize);

        //// Assign paged list to the ViewModel
        //viewModel.Complaints = pagedComplaints;

        // Pass ViewModel to the view
        //return View(complaintsQuery);



        //public async Task<IActionResult> Demand()
        //{
        //    var userId = HttpContext.Session.GetInt32("UserId");
        //    if (userId == null)
        //    {
        //        // Handle if user ID is not found in the session
        //        return RedirectToAction("Login", "Account");
        //    }

        //    var user = await _userRepository.GetByIdAsync(userId.Value);

        //    // Retrieve complaints, demands, and suggestions filled by the user from the database

        //    var userEntries = _db.Items
        //        .Include(i => i.User)
        //        .Where(i => i.UserId == userId).Where(i => i.Type == ItemType.Demand).OrderByDescending(i => i.CreatedAt)
        //        .ToList();

        //    //var userEntries = _itemRepository.GetByIdAsync(userId.Value);
        //    Console.WriteLine(userEntries);
        //    // Pass the user entries to the view
        //    return View(userEntries);

        //}

        //public async Task<IActionResult> Suggestion()
        //{
        //    var userId = HttpContext.Session.GetInt32("UserId");
        //    if (userId == null)
        //    {
        //        // Handle if user ID is not found in the session
        //        return RedirectToAction("Login", "Account");
        //    }

        //    var user = await _userRepository.GetByIdAsync(userId.Value);

        //    // Retrieve complaints, demands, and suggestions filled by the user from the database

        //    var userEntries = _db.Items
        //        .Include(i => i.User)
        //        .Where(i => i.UserId == userId).Where(i => i.Type == ItemType.Suggestion).OrderByDescending(i => i.CreatedAt)
        //        .ToList();

        //    //var userEntries = _itemRepository.GetByIdAsync(userId.Value);
        //    Console.WriteLine(userEntries);
        //    // Pass the user entries to the view
        //    return View(userEntries);

        //}
        //public IActionResult Feedback()
        //{
        //    return View();
        //}

    }
}
