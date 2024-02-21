using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using VidhayakApp.Core.Entities;

namespace VidhayakApp.Web.MiddleWare
{
    public class RoleBasedAuthenticationMiddleware
    {

        private readonly RequestDelegate _next;
      

        public RoleBasedAuthenticationMiddleware(RequestDelegate next)
        {
             _next= next;
          
        }

        
        public async Task Invoke(HttpContent context)
        {
              //await _next.(context);
        }
        




    }
}
