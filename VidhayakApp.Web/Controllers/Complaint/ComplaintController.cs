using Microsoft.AspNetCore.Mvc;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.ViewModels;

namespace VidhayakApp.Web.Controllers.Complaint
{
    public class ComplaintController : Controller
    {
        private readonly IUserRepository _user;
        private readonly IGovtDepartmentRepository _govtd;
        //private readonly IUserRepository _user;
        private readonly ISubCategoryRepository _sub;
        public ComplaintController(IUserRepository user, IGovtDepartmentRepository govtd, ISubCategoryRepository sub)
        {
            _user = user;
            _govtd = govtd;
            _sub = sub;
        }
        public IActionResult Index()
        {
            return View();
        }

        private readonly VidhayakAppContext dbContext;

        public ComplaintController(VidhayakAppContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Create()
        {
            var viewModel = new ComplaintViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(ComplaintViewModel model)
        {

            if (ModelState.IsValid)
            {

                var user = _user.GetByIdAsync(model.UserId);
                var govtDepartment = _govtd.GetByIdAsync(model.DepartmentId);

                // Map the view model to the entity
                var complaint = new Item
                {
                    Status = "Pending",
                    Title = model.Title,
                    Description = model.Description,
                    UserId = model.UserId,
                    SubCategoryId = model.SubCategoryId,
                    Type = model.Category,
                    DepartmentId = model.DepartmentId,
                   
                    
                    // Assuming your Item model has a DepartmentID property
                                                      // Map other properties as needed
                };

                // Save to the database
                dbContext.Items.Add(complaint);
                dbContext.SaveChanges();

                // Redirect to a success page or another action
                return RedirectToAction("Complaint","User");
            }

            // If the model state is not valid, redisplay the form
            return View(model);
        }
    }
}
