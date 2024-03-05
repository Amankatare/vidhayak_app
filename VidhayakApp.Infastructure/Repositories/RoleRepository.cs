using Microsoft.EntityFrameworkCore;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infastructure.Repositories;
using VidhayakApp.Infrastructure.Data;

namespace VidhayakApp.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(VidhayakAppContext context) : base(context) { }
    }
}
