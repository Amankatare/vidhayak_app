namespace VidhayakApp.Web.MiddleWare
{
    public class RegisterMiddleware : IMiddleware
    {
        private readonly RequestDelegate _next;

      

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var requestPath = context.Request.Path;

            // Check if the request path is the index page or register page
            if (requestPath == "/" || requestPath.StartsWithSegments("/Register"))
            {
                // Allow access to the index page and register page without redirection
                await _next(context);
                return;
            }

            await _next(context);
        }
    }
}
 
