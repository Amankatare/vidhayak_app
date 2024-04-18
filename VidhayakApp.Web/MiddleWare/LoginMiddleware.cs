
namespace VidhayakApp.Web.MiddleWare
{
    public class LoginMiddleware : IMiddleware

    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var requestPath = context.Request.Path;

            // Check if the request path is the index page or login page
            if (requestPath == "/" || requestPath.StartsWithSegments("/Login"))
            {
                // Allow access to the index page and login page without redirection
                await next(context);
                return;
            
            }
        await next(context);
        }
    }
}
