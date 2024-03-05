using Microsoft.Extensions.Configuration;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infastructure.Repositories;

namespace VidhayakApp.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly string _connectionstring;
        private readonly IConfiguration _configuration;
        public RoleService(IRoleRepository roleRepository, IConfiguration configuration)
        {
            _roleRepository = roleRepository;

            _connectionstring = configuration.GetConnectionString("VidhayakAppConnection");
        }
        
        

    }
}
