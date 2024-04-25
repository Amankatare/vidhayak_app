//using Microsoft.AspNetCore.Authorization;
//using Microsoft.EntityFrameworkCore;
//using VidhayakApp.Infrastructure.Data;

//namespace VidhayakApp.Web.Utilities
//{
//    public static class AuthorizationPolicyConfiguration
//    {
//        public static void AddRolesPolicies(this IServiceCollection services)
//        {
//            services.AddAuthorization(options =>
//            {
//                options.AddPolicy("AdminOnly", policy =>
//                {
//                    policy.RequireRole("Admin");
//                });

//                options.AddPolicy("AppUserOnly", policy =>
//                {
//                    policy.RequireRole("AppUser");
//                });

//                options.AddPolicy("UserOnly", policy =>
//                {
//                    policy.RequireRole("User");
//                });

//                options.AddPolicy("DynamicRolePolicy", policy =>
//                {
//                    policy.Requirements.Add(new DynamicRoleRequirement());
//                });
//            });

//            services.AddSingleton<IAuthorizationHandler, DynamicRoleHandler>();
//        }

//        public class DynamicRoleRequirement : IAuthorizationRequirement { }

//        public class DynamicRoleHandler : AuthorizationHandler<DynamicRoleRequirement>
//        {
//            private readonly VidhayakAppContext _dbContext;

//            public DynamicRoleHandler(VidhayakAppContext dbContext)
//            {
//                _dbContext = dbContext;
//            }

//            protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DynamicRoleRequirement requirement)
//            {
//                var userName = context.User.Identity.Name;
//                var userRole = await GetUserRoleFromDatabase(userName);

//                if (userRole != null)
//                {
//                    context.Succeed(requirement);
//                }
//                else
//                {
//                    context.Fail();
//                }
//            }

//            private async Task<string> GetUserRoleFromDatabase(string userName)
//            {
//                using (var dbContext = new VidhayakAppContext())
//                {
//                    var user = await dbContext.Users
//                        .Include(u => u.Role) // Assuming there's a navigation property named Role
//                        .FirstOrDefaultAsync(u => u.UserName == userName);

//                    return user?.Role?.RoleName;
//                }
//            }
//        }
//    }
//}
