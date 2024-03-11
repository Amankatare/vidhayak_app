using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhayakApp.Core.Entities;
using VidhayakApp.Infrastructure.Data;

public class UserDetailRepository : IUserDetailRepository
{
    private readonly VidhayakAppContext _context; // Replace YourDbContext with your actual DbContext

    public UserDetailRepository(VidhayakAppContext context)
    {
        _context = context;
    }

    public async Task<UserDetail> GetByIdAsync(int id)
    {
        return await _context.UserDetails.FindAsync(id);
    }

    public async Task UpdateAsync(UserDetail userDetail)
    {
        _context.Entry(userDetail).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

 
}
