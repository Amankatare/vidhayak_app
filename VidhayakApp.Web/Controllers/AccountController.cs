using Microsoft.AspNetCore.Mvc;
//using VidhayakApp.Application.Services;
using VidhayakApp.Web.ViewModels;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Core.Entities;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VidhayakApp.Web.MiddleWare;
using VidhayakApp.Infastructure.Repositories;

namespace VidhayakApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly RoleBasedAuthenticationMiddleware _middleware;
        private readonly RoleRepository _roleRepository;
       
        public AccountController(IUserService userService, IConfiguration configuration, RoleBasedAuthenticationMiddleware middleware, RoleRepository roleRepository)
        {
            _userService = userService;
            _configuration = configuration;
            _middleware = middleware;
            _roleRepository = roleRepository;
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
                    RoleId = 4,
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

                if (registerUser) return RedirectToAction("Successfull", "Account");

                else return RedirectToAction("Unsuccessfull","Account");

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

                // Authenticate and redirect or show an error

                var AuthenticUser = await _userService.AuthenticateAsync(model.UserName, model.Password);
                IEnumerable<Role> roles = await _roleRepository.ListAllAsync();

                if (AuthenticUser != null)
                {
                    if (AuthenticUser.Role.RoleName == "SuperAdmin")
                    {

                         RedirectToAction("Dashboard", "SuperAdmin");

                    }
                    else if (AuthenticUser.Role.RoleName == "Admin")
                    {

                         RedirectToAction("Dashboard", "Admin");

                    }

                    else if (AuthenticUser.Role.RoleName == "AppUser")
                    {

                         RedirectToAction("Dashboard", "AppUser");

                    }

                    else if (AuthenticUser.Role.RoleName == "User")
                    {

                         RedirectToAction("Dashboard", "User");

                    }

                    else
                    {
                        ModelState.AddModelError(string.Empty, "token failed");
                    }
                
                    var token = _middleware.GenerateJwtToken(AuthenticUser,roles);

                    Console.WriteLine(token);

                    if (token != null)
                    {
                        Response.Cookies.Append("JwtToken", token, new CookieOptions
                        {
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict,
                            Secure = true,  // Set to true if using HTTPS
                            Expires = DateTime.UtcNow.AddHours(1)

                        });

                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "token failed");
                }
                

            }
            return RedirectToAction("Index", "Home");

        }

            // return RedirectToLocal(returnUrl);



            //  ViewData["ReturnUrl"] = returnUrl;







           // private IActionResult RedirectToLocal(string returnUrl)
          //  {
            //    if (Url.IsLocalUrl(returnUrl))  return Redirect(returnUrl);
            //    else return RedirectToAction("Index", "Home");

            //}

            public IActionResult DashBoard()
            {
                return View();
            }
            public IActionResult Register()
            {
                    return View();
            }


            public IActionResult Login()
            {
                return View();
            }
            public IActionResult Unsuccessfull()
            {
                return View();
            }
            public IActionResult Successfull()
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
    