using System.Threading.Tasks;
using VidhayakApp.Core.Entities;

public interface IUserDetailService
{
    Task<UserDetail> GetByIdAsync(int id);
    Task UpdateAsync(UserDetail userDetail);
     Task<UserDetail> GetByUserIdAsync(int userId);

    Task<UserDetail> GetUserDetailsByUserIdAsync(int userId);
    
}
