using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.ViewModels;

namespace VidhayakApp.Web.Controllers.Demand
{
    public class DemandController : Controller
    {
        private readonly VidhayakAppContext _dbContext;
        private readonly IUserRepository _user;
        private readonly IGovtDepartmentRepository _govtd;
     
        public DemandController(IUserRepository user, IGovtDepartmentRepository govtd, VidhayakAppContext dbContext)
        {
            _user = user;
            _govtd = govtd;
           
            _dbContext = dbContext;
        }
    
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            var viewModel = new DemandViewModel();
            return RedirectToAction("Dashboard","User", viewModel);
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







