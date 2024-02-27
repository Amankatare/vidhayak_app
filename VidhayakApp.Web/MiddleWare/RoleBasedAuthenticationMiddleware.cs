using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VidhayakApp.Application.Services;
using VidhayakApp.Core.Entities;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.Infrastructure.Repositories;

namespace VidhayakApp.Web.MiddleWare
{
    public class RoleBasedAuthenticationMiddleware : IMiddleware
    {
        private readonly IConfiguration _configuration;
        public RoleBasedAuthenticationMiddleware(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate _next)
        {
            var userRoles = context.User.FindAll(ClaimTypes.Role).Select(c => c.Value);

            // Check for roles and perform role-specific logic
            if (userRoles.Contains("Admin"))
            {
                // Perform Admin-specific logic
                context.Response.WriteAsync("Welcome, Admin! This is the admin dashboard.");
                context.Response.Redirect("/Admin/Dashboard");
                return;
            }
            else if (userRoles.Contains("AppUser"))
            {
                // Perform Admin-specific logic
                context.Response.WriteAsync("Welcome, Admin! This is the admin dashboard.");
                context.Response.Redirect("/AppUser/Dashboard");
                return;
            }
            else if (userRoles.Contains("User"))
            {
                // Perform User-specific logic
                context.Response.WriteAsync("Welcome, User! This is the user dashboard.");
                context.Response.Redirect("/User/Dashboard");
                return;
            }
            // Add more role checks as needed...

            // If the user doesn't have a specific role, you can handle it here
          //  else {
           //   context.Response.WriteAsync("Unauthorized access.");
             //    return;
             //}

            // Continue processing the MVC request

            await _next(context); // Invoke the next middleware in the pipeline
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
                var key = Encoding.UTF8.GetBytes("your-secret-key"); // Replace with your actual secret key

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
    public string GenerateJwtToken(User user,IEnumerable<Role> roles)
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
