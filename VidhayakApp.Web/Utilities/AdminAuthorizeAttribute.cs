//namespace VidhayakApp.SharedKernel.Utilities
//{
//    public class CustomAuthorizeAttribute : AuthorizeAttribute
//    {
//        private readonly string _roleName;

//        public CustomAuthorizeAttribute(string roleName)
//        {
//            _roleName = roleName;
//        }

//        protected override bool AuthorizeCore(HttpContextBase httpContext)
//        {
//            // Check if the user is authenticated
//            if (!httpContext.User.Identity.IsAuthenticated)
//                return false;

//            // Retrieve the user's role from the database
//            var user = GetUserFromDatabase(httpContext.User.Identity.Name);

//            // Check if the user has the required role
//            return user != null && user.Roles.Any(r => r.RoleName == _roleName);
//        }

//        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
//        {
//            // Redirect to a custom unauthorized page or return a custom HTTP status code
//            filterContext.Result = new HttpUnauthorizedResult("Unauthorized access");
//        }
//    }
//}
