using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VidhayakApp.Web.MiddleWare
{
    public class AdminMiddleware : IMiddleware
    {
       

        

        public  async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var ifAdmin = context.Session.GetString("IsAuthenticated");
           // await context.Response.WriteAsync(ifAdmin);

            if (ifAdmin == "true")
            {

                // Check the user's role
                var role = GetRoleFromSession(context);

               // await context.Response.WriteAsync(role);

                // Restrict access based on role
                if (role == "Admin")
                {
                    // Allow access to admin pages
                    await next(context);
                    return;
                }
                else
                {
                    // Deny access to other roles
                    context.Response.StatusCode = 403; // Forbidden
                    await context.Response.WriteAsync("You do not have permission to access this resource.");
                    return;
                }
            }
            else
            {
                // Redirect to login page if user is not authenticated
                context.Response.Redirect("/Login");
                return;
            }
        }

        private string GetRoleFromSession(HttpContext context)
        {
            // Retrieve user's role from session or any other storage mechanism
            return context.Session.GetString("RoleName");
        }
    }
}

