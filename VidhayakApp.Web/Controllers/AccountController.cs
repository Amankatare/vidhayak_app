using Microsoft.AspNetCore.Mvc;
//using VidhayakApp.Application.Services;
using VidhayakApp.Web.ViewModels;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Core.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using VidhayakApp.Infastructure.Repositories;
using MySqlX.XDevAPI;

namespace VidhayakApp.Web.Controllers
{
  
    public class AccountController : Controller
    {
        private bool isAuthenticated = false;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        //private readonly RoleBasedAuthenticationMiddleware _middleware;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserDetailRepository _userDetailRepository;
        private readonly IUserDetailService _userDetailService;
        private readonly IWardRepository _wardRepository;

        public AccountController(IUserService userService, IConfiguration configuration, IRoleRepository roleRepository, IUserRepository userRepository,IWardRepository wardRepository, IUserDetailRepository userDetailRepository, IUserDetailService userDetailService)
        {
            _userService = userService;
            _configuration = configuration;

            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _wardRepository = wardRepository;
            _userDetailService = userDetailService;
            _userDetailRepository = userDetailRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Map ViewModel to Domain Model and call Application layer to register user
                // Return to login page or show an error

                var wardObject = await _wardRepository.GetByIdAsync(model.WardId);
                User user = new User
                {
                    UserName = model.UserName,
                    PasswordHash = model.Password,
                    MobileNumber = model.MobileNumber,
                    Name = model.Name,
                    Dob = model.Dob,
                    Address = model.Address,
                    RoleId = 4,
                    WardId = model.WardId,
                    Ward = wardObject,
                };

                var registerUser = await _userService.RegisterUserAsync(user);
                
                // if user is successfullly registered then redirected to account 

                if (registerUser != false) return RedirectToAction("Successfull", "Account");

                else return RedirectToAction("Unsuccessfull", "Account");

            }
            else
            {
                return View(model);
            }
        }
      
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            //if (ModelState.IsValid)
            //{
            //    return View(model);
            //}
                // Authenticate and redirect or show an error
                var AuthenticUser = await _userService.AuthenticateAsync(model.UserName, model.Password);
                IEnumerable<Role> roles = await _roleRepository.ListAllAsync();

                if (AuthenticUser != null)
                {

                    var user = await _userRepository.GetByUsernameAsync(AuthenticUser.UserName);

                   

                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("Name", user.Name);
                    HttpContext.Session.SetInt32("RoleId", user.RoleId);
                    HttpContext.Session.SetString("RoleName", user.Role.RoleName);

                Console.WriteLine(HttpContext.Session.GetInt32("UserId")) ;
                Console.WriteLine(HttpContext.Session.GetString("UserName"));
                Console.WriteLine(HttpContext.Session.GetString("Name"));
                Console.WriteLine(HttpContext.Session.GetInt32("RoleId"));
                Console.WriteLine(HttpContext.Session.GetString("RoleName"));

                ViewData["RoleId"] = user.RoleId;
                    var role = user.Role.RoleName;

                    var ss = HttpContext.Session.GetString("UserName");
                    Console.WriteLine("--------------------ss"+ss+"-------------------");

                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Role, role),
                         
                    });

                    var principal = new ClaimsPrincipal(identity);

                    

                    var logins = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    var token = GenerateJwtToken(user, roles);
                 
                    Console.WriteLine(token);

                    if (token != null)
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


                        var isValidToken = IsValidToken(token);

                        if (isValidToken)
                        {
                            Console.WriteLine("---------------" + isValidToken + "-------------------");

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


            
            return RedirectToAction("Index", "Home");

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
            if (HttpContext.Session.GetString("UserName") != null)
            {
                return RedirectToAction("Dashboard");
            }

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
            if (HttpContext.Session.GetString("UserName") != null)
            {
                HttpContext.Session.Remove("UserName");
                return RedirectToAction("Login");
            }
            // Handle logout logic
            return RedirectToAction("Index", "Home");
        }

        public bool IsValidToken(string token)
        {
            // Replace with your JWT token validation logic
            return !string.IsNullOrEmpty(token) && ValidateToken(token);
        }

        private bool ValidateToken(string token)
        {
            // Replace with actual JWT validation logic using JwtSecurityTokenHandler
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); // Replace with your actual secret key

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // Set to true if you want to validate the issuer
                    ValidateAudience = false, // Set to true if you want to validate the audience
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // You may adjust the clock skew as needed
                }, out _);

                return true; // Token is valid
            }
            catch (SecurityTokenException)
            {
                return false; // Token validation failed
            }
        }


        //Generating custom Jwt token
        public string GenerateJwtToken(User user, IEnumerable<Role> roles)
        {
            Console.WriteLine("GenerateJwtToken method entered");
            var claims = new List<Claim>
            {
                // Adding User ClaimTypes
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Name, user.Name), // Add user's name as a claim
                new Claim(ClaimTypes.Role, user.Role.RoleName), // Add user's role as a claim
                new Claim(ClaimTypes.MobilePhone, user.MobileNumber), // Add user's role as a claim

                // Add other claims as needed
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
            }

            var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.Aes128CbcHmacSha256);
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
    }
}
