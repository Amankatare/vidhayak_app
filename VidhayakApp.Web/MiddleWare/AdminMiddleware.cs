using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VidhayakApp.Web.MiddleWare
{
    public class AdminMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //var ifAdmin = false;
            var ifAdmin = context.Session.GetString("IsAuthenticated");
            // await context.Response.WriteAsync(ifAdmin);

            if (ifAdmin == "true")
            {
                Console.WriteLine("Admin Condition");
                // Check the user's role
                var role = GetRoleFromSession(context);
                Console.WriteLine(context);
                // await context.Response.WriteAsync(role);

                // Restrict access based on role

                if (role == "Admin")
                {
                    Console.WriteLine("Admin------ Condition");

                    var endpoint = DetermineEndpoint(context.Request.Path);

                    // Set the determined endpoint in the HttpContext
                    context.SetEndpoint(endpoint);


                    // Allow access to admin pages
                     next.DynamicInvoke(context);
                   
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
                Console.WriteLine("Not Admin Condition");
                // Redirect to login page if user is not authenticated
                context.Response.Redirect("/Account/Login");
                return;
            }
        }

        private string GetRoleFromSession(HttpContext context)
        {
            // Retrieve user's role from session or any other storage mechanism
            return context.Session.GetString("RoleName");
        }

        private Endpoint DetermineEndpoint(PathString requestPath)
        {
            // Convert requestPath to string for easier manipulation
            string fullPath = requestPath.Value;

            // Find the index of the last occurrence of '/'
            int lastIndex = fullPath.LastIndexOf('/');
            string endpointName;

            if (lastIndex != -1 && lastIndex < fullPath.Length - 1)
            {
                // Extract the substring after the last '/'
                endpointName = fullPath.Substring(lastIndex + 1);
            }
            else
            {
                // Use the full path if no '/' is found or it's the last character
                endpointName = fullPath;
            }

            // Your logic to determine the endpoint based on the modified request path
            // This could involve further processing or querying

            // For demonstration purposes, let's create a simple endpoint with the modified request path
            var endpoint = new Endpoint(
                requestDelegate: context => Task.CompletedTask,
                metadata: new EndpointMetadataCollection(),
                displayName: endpointName); // Use the modified request path as the display name of the endpoint

            return endpoint;
        }

        private Endpoint DetermineEndpointbychance(PathString requestPath)
        {
            // Convert requestPath to string for easier manipulation
            string fullPath = requestPath.Value;

            string endpointName;

                // Use the full path if no '/' is found or it's the last character
                endpointName = fullPath;
            

            // Your logic to determine the endpoint based on the modified request path
            // This could involve further processing or querying

            // For demonstration purposes, let's create a simple endpoint with the modified request path
            var endpoint = new Endpoint(
                requestDelegate: context => Task.CompletedTask,
                metadata: new EndpointMetadataCollection(),
                displayName: endpointName); // Use the modified request path as the display name of the endpoint

            return endpoint;
        }
    }
}

