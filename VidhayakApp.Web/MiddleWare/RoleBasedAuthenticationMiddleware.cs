using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VidhayakApp.Application.Services;
using VidhayakApp.Core.Entities;
using VidhayakApp.Infrastructure.Data;
using VidhayakApp.Infrastructure.Repositories;

namespace VidhayakApp.Web.MiddleWare
{
    public class RoleBasedAuthenticationMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;


        public RoleBasedAuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
             _next = next;
            _configuration = configuration;

        }

        public async Task Invoke(HttpContext context)
        {
            // Check if the request is for a Web API endpoint
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                // Web API logic
                var apiKey = context.Request.Headers["Api-Key"].FirstOrDefault();
                if (!IsValidApiKey(apiKey))
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    return;
                }

                // Continue processing the Web API request
                await _next(context); // Invoke the next middleware in the pipeline
            }
            else
            {
                // MVC logic
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (!IsValidToken(token))
                {
                    context.Response.Redirect("/login"); // Redirect to login for MVC requests
                    return;
                }

                // Continue processing the MVC request
                await _next(context); // Invoke the next middleware in the pipeline
            }
        }

        // ... rest of the middleware code ...

        private bool IsValidApiKey(string apiKey)
        {
            // Replace with your API key validation logic
            return !string.IsNullOrEmpty(apiKey) && apiKey == "your-api-key";
        }

        private bool IsValidToken(string token)
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
    public string GenerateJwtToken(User user,IEnumerable<Role> role)
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
    }
}
