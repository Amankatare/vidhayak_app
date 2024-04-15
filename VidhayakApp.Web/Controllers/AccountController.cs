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

                var registerUser = await _userService.RegisterUserAndGetUserAsync(user);

                if (registerUser !=null)
                {
                    
                UserDetail userDetails = new UserDetail
                {
                    UserId = registerUser.UserId,
                   
                   
                };

                    await _userDetailRepository.AddAsync(userDetails);
                }
                // if user is successfullly registered then redirected to account 

                if (registerUser != null )return RedirectToAction("Successfull", "Account");

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

            if (ModelState.IsValid)
            {
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
                    HttpContext.Session.SetInt32("WardId", user.WardId);
                    Console.WriteLine(HttpContext.Session.GetInt32("UserId"));
                    Console.WriteLine(HttpContext.Session.GetString("UserName"));
                    Console.WriteLine(HttpContext.Session.GetString("Name"));
                    Console.WriteLine(HttpContext.Session.GetInt32("RoleId"));
                    Console.WriteLine(HttpContext.Session.GetString("RoleName"));
                    Console.WriteLine(HttpContext.Session.GetInt32("WardId"));

                    ViewData["RoleId"] = user.RoleId;
                    var role = user.Role.RoleName;

                    var ss = HttpContext.Session.GetString("UserName");
                    Console.WriteLine("--------------------ss" + ss + "-------------------");

                    if (model.UserName == user.UserName) // Replace with actual validation
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.UserName),
                            new Claim(ClaimTypes.Role,user.Role.RoleName ),
                           
                            // Add more claims if needed
                        };


                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,  // Whether the authentication session is persisted across browser sessions
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),  // Expiration time for the authentication session
                            AllowRefresh = true  // Whether the authentication session can be refreshed
                        };
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);



                        //var identity = new ClaimsIdentity(new[]
                        //{
                        //    new Claim(ClaimTypes.Role, role),

                        //});

                        //var principal = new ClaimsPrincipal(identity);

                        //var logins = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        var token = GenerateJwtToken(user, roles);

                        Console.WriteLine(token);

                        if (token != null)
                        {

                            var cookieOptions = new CookieOptions
                            {
                                HttpOnly = true,
                                SameSite = SameSiteMode.Strict,
                                Secure = true,  // Set to true if using HTTPS
                                Expires = DateTime.UtcNow.AddMinutes(30)
                            };

                            // Assuming 'Response' is an instance of HttpResponse in your ASP.NET Core controller
                            Response.Cookies.Append("JwtToken", token, cookieOptions);


                            var isValidToken = IsValidToken(token);

                            if (isValidToken)
                            {
                                Console.WriteLine("---------------" + isValidToken + "-------------------");
                                HttpContext.Session.SetString("JwtToken", token);

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

                    return View(model);
                }


            }
                ViewData["ValidateMessage"] = "User Not Found";
                return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ProfileAndChangePasswordViewModel viewModel)
        {
            // Ensure that the ModelState is valid for the ChangePasswordViewModel
            

            var loggedInUser = HttpContext.Session.GetString("UserName");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Retrieve the user from the database
            var user = await _userRepository.GetByUsernameAsync(loggedInUser);
            if (user == null)
            {
                // Handle the scenario if the user is not found
                return RedirectToAction("Login", "Account");
            }

            var decryptedPassword = BCrypt.Net.BCrypt.EnhancedVerify(viewModel.ChangePasswordViewModel.OldPassword, user.PasswordHash);
            // Compare the hashed old password with the hashed password stored in the database
            if (!decryptedPassword)
            {
                ModelState.AddModelError("ChangePassword.OldPassword", "The old password is incorrect.");
                return View(viewModel);
            }

            // Hash the new password before updating it in the database
            var newPasswordHash = _userService.HashPassword(viewModel.ChangePasswordViewModel.NewPassword);

            // Update the password in the database
            user.PasswordHash = newPasswordHash;
            await _userRepository.UpdateAsync(user);

            // Redirect to the profile view or any other appropriate page
            return RedirectToAction("ViewProfile", "Profile");
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

        public IActionResult Register()
        {
            return View();
        }

        

        public async Task<IActionResult> Logout()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
              await  HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
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
