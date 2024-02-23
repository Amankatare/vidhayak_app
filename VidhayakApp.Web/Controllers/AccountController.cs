using Microsoft.AspNetCore.Mvc;
//using VidhayakApp.Application.Services;
using VidhayakApp.Web.ViewModels;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Core.Entities;

namespace VidhayakApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        //private readonly IRoleService _roleService;
        private readonly IConfiguration _configuration;
       
        public AccountController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
           // _roleService = _roleService;
         
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
                   // RoleId = 1,
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
             

                if (AuthenticUser != null)
                {
                    return RedirectToAction("Dashboard", "User");
                    //var token = GenerateJwtToken(AuthenticUser);
                    //Console.WriteLine(token);
                    /*
                    if (token != null)
                    {
                        Response.Cookies.Append("JwtToken", token,new CookieOptions
                        {
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict,
                            Secure = true,  // Set to true if using HTTPS
                            Expires = DateTime.UtcNow.AddHours(1)

                        });
                    */
                    // return RedirectToLocal(returnUrl);
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
            return RedirectToAction("Index", "Home");

        }
            //  ViewData["ReturnUrl"] = returnUrl;

            

        //Generating custom Jwt token
        /*
        public string GenerateJwtToken(User user)
        {
            Console.WriteLine("GenerateJwtToken method entered");
            var claims = new List<Claim>
            {
                //Adding User ClaimTypes
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                
                // Add other claims as needed
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(1); // Token expiration time

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        */
        private IActionResult RedirectToLocal(string returnUrl)
            {
                if (Url.IsLocalUrl(returnUrl))  return Redirect(returnUrl);
                else return RedirectToAction("Index", "Home");

            }

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
    