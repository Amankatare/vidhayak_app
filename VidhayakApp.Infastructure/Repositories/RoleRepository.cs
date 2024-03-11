using Microsoft.EntityFrameworkCore;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infastructure.Repositories;
using VidhayakApp.Infrastructure.Data;

namespace VidhayakApp.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(VidhayakAppContext context) : base(context) { _dbContext = context; }

        private readonly VidhayakAppContext _dbContext;


        public IEnumerable<string> GetRolesForUser(string username)
        {
            var user = _dbContext.Users
                .Include(u => u.Role) // Include the Role navigation property
                .SingleOrDefault(u => u.UserName == username);

            return user?.Role != null ? new List<string> { user.Role.RoleName } : Enumerable.Empty<string>();
        }
    }
}
