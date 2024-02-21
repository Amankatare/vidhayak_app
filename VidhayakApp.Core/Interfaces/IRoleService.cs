using System.Threading.Tasks;
using VidhayakApp.Core.Entities;

namespace VidhayakApp.Core.Interfaces
{
    public interface IRoleService
    {
        //Task<bool> RegisterRoleAsync(Role role);
        // Task<User> GetRoleByUserNameAsync(string email);
        Task<Role> AuthenticateRoleAsync(string username, string password);
    }
}