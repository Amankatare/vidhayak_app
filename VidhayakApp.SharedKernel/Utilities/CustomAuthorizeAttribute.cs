using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;
using VidhayakApp.Core.Interfaces;

namespace VidhayakApp.SharedKernel.Utilities
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    //public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    //{
    //    private readonly IHttpContextAccessor _httpContextAccessor;
    //    private readonly IRoleService _roleService;

    //    public CustomAuthorizeAttribute(IHttpContextAccessor httpContextAccessor, IRoleService roleService)
    //    {
    //        _httpContextAccessor = httpContextAccessor;
    //        _roleService = roleService;
    //    }

        //public void OnAuthorization(AuthorizationFilterContext context)
        //{
        //    var httpContext = _httpContextAccessor.HttpContext;

        //    if (httpContext != null && httpContext.Session != null)
        //    {
        //        var userRole = httpContext.Session.GetString("RoleId");

        //        if (string.IsNullOrEmpty(userRole))
        //        {
        //            // Role information not found in session, handle accordingly
        //            context.Result = new UnauthorizedResult();
        //            return;
        //        }

        //        // Get the user roles from the database
        //        var userRoles =  _roleService.GetRolesForUser(userRole);

        //        // Check if the user has the required role
        //        if (!string.IsNullOrEmpty(Roles) && !userRoles.Contains(Roles))
        //        {
        //            context.Result = new ForbidResult();
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        // Session not available, handle accordingly
        //        context.Result = new UnauthorizedResult();
        //        return;
        //    }
        //}

        
   // }
}
