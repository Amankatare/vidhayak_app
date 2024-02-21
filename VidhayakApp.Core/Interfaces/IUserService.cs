using System.Threading.Tasks;
using VidhayakApp.Core.Entities;

namespace VidhayakApp.Core.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(User user);
        // Task<User> GetUserByEmailAsync(string email);
        Task<User> AuthenticateAsync(string username, string password);
    }
}