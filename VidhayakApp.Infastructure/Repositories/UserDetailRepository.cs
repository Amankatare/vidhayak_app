using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infastructure.Repositories;
using VidhayakApp.Infrastructure.Data;

public class UserDetailRepository : Repository<UserDetail>, IUserDetailRepository
{
    private readonly VidhayakAppContext _context;
    public UserDetailRepository(VidhayakAppContext context) : base(context) 
    { 
        _context = context;
    }

    public async Task<UserDetail?> GetUserDetailsByUserIdAsync(int id)
    {
        return await _context.UserDetails
             .Include(ud => ud.User) // Include User navigation property if needed
             .FirstOrDefaultAsync(ud => ud.UserId == id);
    }

}
