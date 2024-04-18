using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VidhayakApp.Core.Interfaces;

namespace VidhayakApp.SharedKernel.Utilities
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AdminAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleService _roleService;

        public AdminAuthorizeAttribute(IHttpContextAccessor httpContextAccessor, IRoleService roleService)
        {
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
        }

        public async Task OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null && httpContext.Session != null)
            {
                var userRole = httpContext.Session.GetString("RoleId");
                var userId = httpContext.Session.GetInt32("UserId");

                if (userId.HasValue)
                {
                    var userRoleObject = await _roleService.GetRoleByUserIdAsync(userId.Value);

                    if (userRoleObject != null)
                    {
                        var userRoleName = userRoleObject.RoleName;

                        if (userRoleName == "Admin")
                        {
                            // User is authorized as Admin, allow access
                            return;
                        }
                    }
                    else
                    {
                        // Role not found for the user
                        context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                        return;
                    }
                }
                else
                {
                    // User is not authenticated
                    context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                    return;
                }
            }

            // Session or HttpContext is not available
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);

        }

        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
