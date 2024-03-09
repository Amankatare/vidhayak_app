using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VidhayakApp.SharedKernel.Utilities
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,Inherited=true,AllowMultiple=true)]
    public class CustomAuthorizeAttribute: Attribute
    {
        private readonly string _allowUser;
        public CustomAuthorizeAttribute(string allowUser)
        {
            _allowUser = allowUser;
        }

        protected override bool AuthorizeCore(HttpContext context)
        {
            var user = context.User;
        }
    }
}
