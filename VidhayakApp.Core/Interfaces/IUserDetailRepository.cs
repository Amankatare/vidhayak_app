using System.Threading.Tasks;
using VidhayakApp.Core.Entities;

public interface IUserDetailRepository
{
    Task<UserDetail> GetByIdAsync(int id);
    Task UpdateAsync(UserDetail userDetail);
    // Add other methods as needed
}
