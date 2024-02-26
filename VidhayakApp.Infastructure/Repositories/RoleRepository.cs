using Microsoft.EntityFrameworkCore;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data;

namespace VidhayakApp.Infastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly VidhayakAppContext _context;

        public RoleRepository(VidhayakAppContext context)
        {
            _context = context;
        }

        public async Task<Role> GetByIdAsync(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<Role> AddAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<IEnumerable<Role>> ListAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task UpdateAsync(Role entity)
        {

            _context.Roles.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Role entity)
        {
            _context.Roles.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Role> GetByUsernameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(u => u.RoleName == roleName);
        }
    }
}
