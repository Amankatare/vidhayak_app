using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;

namespace VidhayakApp.Application.Services
{
    public class RoleService:IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly string _connectionstring;
        private readonly IConfiguration _configuration;
        public RoleService(IRoleRepository roleRepository, IConfiguration configuration)
        {
            _roleRepository = roleRepository;

            _connectionstring = configuration.GetConnectionString("VidhayakAppConnection");
        }

        public Task<Role> AuthenticateRoleAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
