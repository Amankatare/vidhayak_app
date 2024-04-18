using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data; // Assuming you have a User and UserRole entities

namespace VidhayakApp.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly VidhayakAppContext _context; // Replace YourDbContext with your actual DbContext class

        public RoleService(VidhayakAppContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetRolesForUser(string userName)
        {
            var userRoles = new List<string>();

            // Assuming you have a User entity with a UserName property
            var user = await _context.Users
                .Where(u => u.UserName == userName)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                // Assuming there's a direct navigation property from User to Role
                var role = user.Role;

                if (role != null)
                {
                    userRoles.Add(role.RoleName); // Assuming RoleName is the name of the role property
                }
            }

            return userRoles;
        }

        public async Task<Role> GetRoleByUserIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                // Assuming Role is eagerly loaded
                return user.Role;
            }
            return null; // User not found
        }
    }
}
