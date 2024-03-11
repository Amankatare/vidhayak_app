using Microsoft.EntityFrameworkCore;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data;

public class UserDetailService : IUserDetailService
{
    private readonly IUserDetailRepository _userDetailRepository; 
    private readonly VidhayakAppContext _db; 

    public UserDetailService(IUserDetailRepository userDetailRepository,VidhayakAppContext db)
    {
        _userDetailRepository = userDetailRepository;
        _db = db;
    }
    public async Task<UserDetail> GetByUserIdAsync(int userId)
    {
        return await _db.UserDetails.FirstOrDefaultAsync(d => d.UserId == userId);
    }
    public async Task<UserDetail> GetByIdAsync(int id)
    {
        return await _userDetailRepository.GetByIdAsync(id);
    }

    public async Task UpdateAsync(UserDetail userDetail)
    {
        await _userDetailRepository.UpdateAsync(userDetail);
   
    }

    // Add other methods as needed
}
