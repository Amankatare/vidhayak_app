using Microsoft.AspNetCore.Mvc;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.ViewModels;

namespace VidhayakApp.Web.Controllers.Form
{
    public class FormController : Controller
    {
   
        private readonly VidhayakAppContext _dbContext;
        private readonly IUserRepository _user;
        

        public FormController(IUserRepository user, VidhayakAppContext dbContext)
        {
            _user = user;
            _dbContext = dbContext;
        }

        public IActionResult Form()
        {
            return View();
        }

        public IActionResult CreateComplaint()
        {
            var viewModel = new ComplaintViewModel();
            return RedirectToAction("Dashboard", "User", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComplaint(ComplaintViewModel model)
        {

            var user = await _user.GetByIdAsync(model.UserId);

            // Map the view model to the entity
            var complaint = new Item
            {
                Status = StatusType.Pending, // Use Status.Pending from enum
                Title = model.Title,
                Description = model.Description,
                UserId = model.UserId,
                Type = model.Type,
                ItemId = model.ItemId,
                CreatedAt = DateTime.Now,
                User = user,
            };

            // Save to the database
            await _dbContext.Items.AddAsync(complaint);
            await _dbContext.SaveChangesAsync();

            // Redirect to a success page or another action
            return RedirectToAction("Complaint", "User");
        }

        public IActionResult Create()
        {
            var viewModel = new DemandViewModel();
            return RedirectToAction("Dashboard", "User", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DemandViewModel model)
        {

            var user = await _user.GetByIdAsync(model.UserId);

            // Map the view model to the entity
            var demand = new Item
            {
                Status = StatusType.Pending, // Use Status.Pending from enum
                Title = model.Title,
                Description = model.Description,
                UserId = model.UserId,
                Type = model.Type,
                ItemId = model.ItemId,
                CreatedAt = DateTime.Now,
                User = user,
            };

            // Save to the database
            await _dbContext.Items.AddAsync(demand);
            await _dbContext.SaveChangesAsync();

            // Redirect to a success page or another action
            return RedirectToAction("Demand", "User");
        }
    }
}
