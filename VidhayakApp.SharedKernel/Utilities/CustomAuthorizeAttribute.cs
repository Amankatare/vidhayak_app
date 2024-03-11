namespace VidhayakApp.SharedKernel.Utilities
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    //public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    //{
    //    private readonly IRoleService _roleService;

    //    public CustomAuthorizeAttribute(IRoleService roleService)
    //    {
    //        _roleService = roleService;
    //    }

    //    public void OnAuthorization(AuthorizationFilterContext context)
    //    {
    //        if (!context.HttpContext.User.Identity.IsAuthenticated)
    //        {
    //            // User is not authenticated, handle accordingly
    //            context.Result = new UnauthorizedResult();
    //            return;
    //        }

    //        // Get the user roles from the database
    //        var userRoles = _roleService.GetRolesForUser(context.HttpContext.User.Identity.Name);

    //        // Check if the user has the required role
    //        if (!string.IsNullOrEmpty(Roles) && !userRoles.Contains(Roles))
    //        {
    //            context.Result = new ForbidResult();
    //            return;
    //        }
    //    }
    //}

}

