using Microsoft.EntityFrameworkCore;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data;

namespace VidhayakApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly VidhayakAppContext _context;

        public UserRepository(VidhayakAppContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
       
        public async Task<IEnumerable<User>> ListAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task UpdateAsync(User entity)
        {
          
               _context.Users.Update(entity);
              await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            User record = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            Console.WriteLine(record+ " 4 ");
            if (record != null)
            {
                Console.WriteLine(record + " 4 ");
                return record;
            }
            Console.WriteLine(record + " 4 ");
            return record;
        }

        
    }
}