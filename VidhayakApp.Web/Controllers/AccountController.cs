using Microsoft.AspNetCore.Mvc;
//using VidhayakApp.Application.Services;
using VidhayakApp.Web.ViewModels;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Core.Entities;
using VidhayakApp.Web.MiddleWare;

namespace VidhayakApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private bool isAuthenticated = false;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly RoleBasedAuthenticationMiddleware _middleware;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
       
        public AccountController(IUserService userService, IConfiguration configuration, RoleBasedAuthenticationMiddleware middleware, IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _userService = userService;
            _configuration = configuration;
            _middleware = middleware;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
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
                    ViewData["UserName"] = AuthenticUser.UserName;
                    ViewData["UserRoleId"] = AuthenticUser.RoleId;

                    var user = await _userRepository.GetByUsernameAsync(AuthenticUser.UserName);

                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("Name", user.Name);
                    HttpContext.Session.SetString("RoleName", user.Role.RoleName);

                
                    var token = _middleware.GenerateJwtToken(user,roles);

                    Console.WriteLine(token);

                    if (token != null)
                    {
                        var isValidToken = _middleware.IsValidToken(token);

                        if (isValidToken)
                        {

                            var cookieOptions = new CookieOptions
                            {
                                HttpOnly = true,
                                SameSite = SameSiteMode.Strict,
                                Secure = true,  // Set to true if using HTTPS
                                Expires = DateTime.UtcNow.AddHours(1)
                            };

                            // Assuming 'Response' is an instance of HttpResponse in your ASP.NET Core controller
                            Response.Cookies.Append("JwtToken", token, cookieOptions);
                            //HttpContext.Session.SetString("JwtToken", token);

                        }

                    }

                    if (user.RoleId == 1)
                    {

                        return RedirectToAction("Dashboard", "SuperAdmin");

                    }
                    else if (user.RoleId == 2)
                    {

                        return RedirectToAction("Dashboard", "Admin");

                    }

                    else if (user.RoleId == 3)
                    {

                        return RedirectToAction("Dashboard", "AppUser");

                    }

                    else if (user.RoleId == 4)
                    {

                        return RedirectToAction("Dashboard", "User");

                    }

                    else
                    {
                        ModelState.AddModelError(string.Empty, "token failed");
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
    