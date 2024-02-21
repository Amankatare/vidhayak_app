using Microsoft.AspNetCore.Mvc;
//using VidhayakApp.Application.Services;
using VidhayakApp.Web.ViewModels;
using System.Threading.Tasks;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace VidhayakApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Map ViewModel to Domain Model and call Application layer to register user
                // Return to login page or show an error

                User user = new User
                {
                    UserName = model.UserName,
                    PasswordHash = model.Password,
                  
                    MobileNumber = model.MobileNumber,
                    Name = model.Name,
                    Dob = model.Dob,
                    Address = model.Address,
                    Ward = model.Ward
                };

                //we don't want this details from every user so
                //we can check if user filled this detail or not 
                //like Admin and AppUser might  not fill all these details
/*
                if (!string.IsNullOrEmpty(model.Education)) user.Education = model.Education;
                if (!string.IsNullOrEmpty(model.AadharNumber)) user.AadharNumber = model.AadharNumber;
                if (!string.IsNullOrEmpty(model.Caste)) user.Caste = model.Caste;
                if (!string.IsNullOrEmpty(model.SamagraID)) user.SamagraID = model.SamagraID;
                if (!string.IsNullOrEmpty(model.VoterID)) user.VoterID = model.VoterID;
*/
                var registerUser = await _userService.RegisterUserAsync(user);

                // if user is successfullly registered then redirected to account 
                
                if (registerUser) return RedirectToAction("Index", "Home");

                else return RedirectToAction("registration not possible");

            }
            else
            {
                return View(model);
            }
        }

           [HttpPost]
            //public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
            public async Task<IActionResult> Login(LoginViewModel model)
            {

            if (ModelState.IsValid)
            {
                Console.WriteLine("1");
                // Authenticate and redirect or show an error

                var AuthenticUser = await _userService.AuthenticateAsync(model.UserName, model.Password);
                Console.WriteLine(AuthenticUser + "2");




                // if (AuthenticUser.IsCompletedSuccessfully){
                //




                // return View(model);

             //    }

                // else return BadRequest();


            }

              return RedirectToAction("Index", "Home");
                 return View(model); // Return with errors if ModelState is not valid 
             }

        public IActionResult Register()
        {
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            // Handle logout logic
            return RedirectToAction("Index", "Home");
        }
    }
}