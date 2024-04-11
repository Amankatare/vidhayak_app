using Microsoft.AspNetCore.Mvc;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data; 
using VidhayakApp.Web.ViewModels;

namespace VidhayakApp.Web.Controllers.Form
{
    public class FormController : Controller
    {
   
        private readonly VidhayakAppContext _dbContext;
        private readonly IUserRepository _user;
        private readonly IGovtDepartmentRepository _govtDepartment;
        private readonly IGovtSchemeRepository _govtScheme;
        

        public FormController(IUserRepository user, VidhayakAppContext dbContext, IGovtDepartmentRepository govtDepartment, IGovtSchemeRepository govtScheme)
        {
            _user = user;
            _dbContext = dbContext;
            _govtDepartment = govtDepartment;
            _govtScheme = govtScheme;
        }

        public IActionResult Form()
        {
            return View();
        }

        public IActionResult CreateComplaint()
        {
            var viewModel = new FormViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComplaint(FormViewModel model)
        {

            //IFormFile is used to get the image file from the request and convert it in base 64 and then store it inside the db 
            //or for storing it in file format and path will be stored in the db.


            Console.WriteLine("pressed Complaint");

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {

                //Generate unique Filename for image

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;

                // Specify the path of the folder you want to check/create
                string uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                string complaintsFolderPath = Path.Combine(uploadsFolderPath, "complaints");

                // Check if the 'uploads' directory exists, if not, create it
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                // Check if the 'complaints' directory exists, if not, create it inside 'uploads'
                if (!Directory.Exists(complaintsFolderPath))
                {
                    Directory.CreateDirectory(complaintsFolderPath);
                }

                // Combine the unique filename with the path to the image folder

                string filePath = Path.Combine(complaintsFolderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }


                //        // Convert the image file to a byte array
                //        byte[] imageData;
                //        using (var stream = new MemoryStream())
                //        {
                //            await imageFile.CopyToAsync(stream);
                //            imageData = stream.ToArray();
                //        }

                //        // Convert the byte array to a Base64 string
                //        string base64Image = Convert.ToBase64String(imageData);

                //        // Assign the image path to the model
                //        model.ImagePath = base64Image;
                //    }
                //    catch (Exception ex)
                //    {
                //        ModelState.AddModelError("ImageFile", $"Error uploading image: {ex.Message}");
                //        
                //    }
                //}

                var userObjectId = HttpContext.Session.GetInt32("UserId");
                var user = await _user.GetByIdAsync(userObjectId.Value);
                var department = await _govtDepartment.GetByIdAsync(model.DepartmentId);
                // Map the view model to the entity
                var complaint = new Item
                {
                    Status = StatusType.Pending, // Use Status.Pending from enum
                    Title = model.Title,
                    Description = model.Description,
                    UserId = (int)userObjectId,
                    Type = model.Type,
                    SubCategoryTypeId = model.SubCategoryTypeId,
                    CreatedAt = DateTime.Now.Date,
                    ImagePath = string.IsNullOrEmpty(uniqueFileName) ? "" : "/uploads/complaints/" + uniqueFileName,
                    User = user,
                    DepartmentId = model.DepartmentId,
                    Department = department
                };

                // Save to the database
                await _dbContext.Items.AddAsync(complaint);
                await _dbContext.SaveChangesAsync();
                HttpContext.Session.SetInt32("CreatedItemId", complaint.ItemId);
                Console.WriteLine("CreatedItemId: " + complaint.ItemId);
            }
            else
            {
                var userObjectId = HttpContext.Session.GetInt32("UserId");
                var user = await _user.GetByIdAsync(userObjectId.Value);

                if (model.SubCategoryTypeId == SubCategoryType.PrivateOrganization)
                {


                    // Map the view model to the entity
                    var complaints = new Item
                    {
                        Status = StatusType.Pending, // Use Status.Pending from enum
                        Title = model.Title,
                        Description = model.Description,
                        UserId = (int)userObjectId,
                        Type = model.Type,
                        SubCategoryTypeId = model.SubCategoryTypeId,
                        CreatedAt = DateTime.Now.Date,
                        ImagePath = "",
                        User = user,

                        Department = null
                    };

                    // Save to the database
                    await _dbContext.Items.AddAsync(complaints);
                    await _dbContext.SaveChangesAsync();
                    HttpContext.Session.SetInt32("CreatedItemId", complaints.ItemId);
                    Console.WriteLine("CreatedItemId: " + complaints.ItemId);
                }
                else
                {
                    var department = await _govtDepartment.GetByIdAsync(model.DepartmentId);
                    // Map the view model to the entity
                    var complaint = new Item
                    {
                        Status = StatusType.Pending, // Use Status.Pending from enum
                        Title = model.Title,
                        Description = model.Description,
                        UserId = (int)userObjectId,
                        Type = model.Type,
                        SubCategoryTypeId = model.SubCategoryTypeId,
                        CreatedAt = DateTime.Now.Date,
                        ImagePath = "",
                        User = user,
                        DepartmentId = model.DepartmentId,
                        Department = department
                    };

                    // Save to the database
                    await _dbContext.Items.AddAsync(complaint);
                    await _dbContext.SaveChangesAsync();
                    HttpContext.Session.SetInt32("CreatedItemId", complaint.ItemId);
                    Console.WriteLine("CreatedItemId: " + complaint.ItemId);
                }
            }
                // Redirect to a success page or another action
                return RedirectToAction("Dashboard", "User");
        }
        
        public IActionResult CreateDemand()
        {
            var viewModel = new FormViewModel();
            return RedirectToAction("Dashboard", "User", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDemand(FormViewModel model)
        {
            Console.WriteLine("pressed");
            var loggedInUserId = HttpContext.Session.GetInt32("UserId");
            var user = await _user.GetByIdAsync(loggedInUserId.Value);
          
            if (model.SubCategoryTypeIdForDemand == SubCategoryType.PrivateOrganization)
            {

                // Map the view model to the entity
                var demands = new Item
                {
                    Status = StatusType.Pending, // Use Status.Pending from enum
                    Title = model.Title,
                    Description = model.Description,
                    Type = model.Type,
                    SubCategoryTypeId = model.SubCategoryTypeIdForDemand,
                    CreatedAt = DateTime.Now.Date,
                    UpdatedAt = null,
                    Scheme = null,
                    UserId = model.UserId,
                    User = user,
                };

                // Save to the database
                await _dbContext.Items.AddAsync(demands);
                await _dbContext.SaveChangesAsync();
                HttpContext.Session.SetInt32("CreatedItemId", demands.ItemId);
                Console.WriteLine("CreatedItemId: " + demands.ItemId);
            }
            else
            {
                var scheme = await _govtScheme.GetByIdAsync(model.SchemeId);
                // Map the view model to the entity
                var demand = new Item
                {
                    Status = StatusType.Pending, // Use Status.Pending from enum
                    Title = model.Title,
                    Description = model.Description,
                    Type = model.Type,
                    SubCategoryTypeId = model.SubCategoryTypeIdForDemand,
                    CreatedAt = DateTime.Now.Date,
                    UpdatedAt = null,
                    SchemeId = model.SchemeId,
                    Scheme = scheme,
                    UserId = model.UserId,
                    User = user,
                };

                // Save to the database
                await _dbContext.Items.AddAsync(demand);
                await _dbContext.SaveChangesAsync();
                HttpContext.Session.SetInt32("CreatedItemId", demand.ItemId);
                Console.WriteLine("CreatedItemId: " + demand.ItemId);
            }
            // Redirect to a success page or another action
            return RedirectToAction("Dashboard", "User");
        }

        public IActionResult CreateSuggestion()
        {
            var viewModel = new FormViewModel();
            return RedirectToAction("Dashboard", "User", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSuggestion(FormViewModel model)
        {
            Console.WriteLine("pressed");
            var loggedInUserId = HttpContext.Session.GetInt32("UserId");
            var user = await _user.GetByIdAsync(loggedInUserId.Value);
            //var scheme = await _govtScheme.GetByIdAsync(model.SchemeId);
            // Map the view model to the entity
            var suggestion = new Item
            {
                Status = StatusType.Pending, // Use Status.Null from enum
                Title = string.IsNullOrEmpty(model.Title) ? "" : model.Title,
                Type = model.Type,
                Description = model.Description,
                SubCategoryTypeId = SubCategoryType.Null,
                CreatedAt = DateTime.Now.Date,
                //UpdatedAt = null,
                //SchemeId = model.SchemeId,
                //Scheme = scheme,
                UserId = loggedInUserId.Value,
                User = user,
            };

            // Save to the database
            await _dbContext.Items.AddAsync(suggestion);
            await _dbContext.SaveChangesAsync();
            HttpContext.Session.SetInt32("CreatedItemId", suggestion.ItemId);
            Console.WriteLine("CreatedItemId: " + suggestion.ItemId);

            // Redirect to a success page or another action
            return RedirectToAction("Dashboard", "User");
        }
    }
}
