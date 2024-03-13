using System.Threading.Tasks;
using VidhayakApp.Core.Entities;

namespace VidhayakApp.Core.Interfaces
{
    public interface IRoleService
    {
        //Task<bool> RegisterRoleAsync(Role role);
        //Task<User> GetRoleByUserNameAsync(User user);
        //Task<Role> AuthenticateRoleAsync(string username, string password);
        Task<IEnumerable<string>> GetRolesForUser(string userName);
    }
}