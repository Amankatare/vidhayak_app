using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

public class AuthMiddleware : IMiddleware
{
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!IsUserAuthenticated(context))
        {
            // User is not authenticated; redirect to the login page
            context.Response.Redirect("~/Account/login");
            return;
        }
      
        
        await next(context);
    }
        private bool IsUserAuthenticated(HttpContext context)
    {
        // Implement your custom logic to check if the user is authenticated.
        // For example, you can use session, cookies, or any other mechanism.

        // For demonstration purposes, using a simple session check:
        return context.Session.GetString("UserName") != null;
    }
}