using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.ViewModels;
using VidhayakApp.Web.ViewModels;

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
                string folderPath = "~/wwwroot/uploads/complaints";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                    // Combine the unique filename with the path to the image folder

                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads","complaints", uniqueFileName);

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


                var user = await _user.GetByIdAsync(model.UserId);

                // Map the view model to the entity
                var complaint = new Item
                {
                    Status = StatusType.Pending, // Use Status.Pending from enum
                    Title = model.Title,
                    Description = model.Description,
                    UserId = model.UserId,
                    Type = model.Type,
                    SubCategoryTypeId = model.SubCategoryTypeId,
                    CreatedAt = DateTime.Now,
                    ImagePath = "/uploads/" + uniqueFileName, // Assign the image path
                    User = user,
                };

                // Save to the database
                await _dbContext.Items.AddAsync(complaint);
                await _dbContext.SaveChangesAsync();
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
            var user = await _user.GetByIdAsync(model.UserId);

            // Map the view model to the entity
            var demand = new Item
            {
                Status = StatusType.Pending, // Use Status.Pending from enum
                Title = model.Title,
                Description = model.Description,
                UserId = model.UserId,
                Type = model.Type,
                SubCategoryTypeId = model.SubCategoryTypeId,
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                User = user,
            };

            // Save to the database
            await _dbContext.Items.AddAsync(demand);
            await _dbContext.SaveChangesAsync();

            // Redirect to a success page or another action
            return RedirectToAction("Dashboard", "User");
        }
    }
}
