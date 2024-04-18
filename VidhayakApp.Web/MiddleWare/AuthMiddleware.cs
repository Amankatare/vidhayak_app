//using Microsoft.AspNetCore.Http;
//using System;
//using System.Threading.Tasks;
//using VidhayakApp.Core.Interfaces;
//using VidhayakApp.Infrastructure.Repositories;

//public class AuthMiddleware : IMiddleware
//{
//    private readonly IUserRepository _user;
//    public AuthMiddleware(IUserRepository user)
//    {
//        _user = user;
//    }

    using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using VidhayakApp.Core.Interfaces;

public class AuthMiddleware : IMiddleware
{
    private readonly IUserRepository _userRepository;

    public AuthMiddleware(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var requestPath = context.Request.Path;

        // Check if the request path is the index page or login page
        if (requestPath == "/" || requestPath.StartsWithSegments("/Account/Login"))
        {
            // Allow access to the index page and login page without redirection
            await next(context);
            return;
        }



        var loggedInUser = context.Session.GetString("UserName");

        if (!string.IsNullOrEmpty(loggedInUser))
        {
            var userObject = await _userRepository.GetByUsernameAsync(loggedInUser);

            if (userObject != null)
            {
                var loggedInRoleId = context.Session.GetInt32("RoleId");

                if (loggedInRoleId == userObject.RoleId)
                {
                    // Authentication successful, proceed to next middleware
                    await next(context);
                    return;
                }
                else
                {
                    // User is authenticated but doesn't have the correct role
                    context.Response.StatusCode = 403; // Forbidden
                    return;
                }
            }
        }

        // User is not authenticated, redirect to login page
        context.Response.Redirect("/Account/Login");
    }
}

//    //public async Task InvokeAsync(HttpContext context, RequestDelegate next)
//    //{

//    //    var loggedInUser = context.Session.GetString("UserName");
//    //    if (loggedInUser != null)
//    //    {
//    //        var userObject = await _user.GetByUsernameAsync(loggedInUser);
//    //        if (userObject != null)
//    //        {
//    //            var loggedInRoleId = context.Session.GetInt32("RoleId");
//    //            if (loggedInRoleId == userObject.RoleId) 
//    //            {
//    //                var loggedInRoleName = context.Session.GetString("RoleName");


//    //                context.Response.Redirect("/"+loggedInRoleName+"/"+"Dashboard");

//    //            }

//    //        }
//    //    }   

//    //    await next(context);
//    //}


//}