using System.Collections.Generic;
using System.Threading.Tasks;
using VidhayakApp.Core.Entities;

namespace VidhayakApp.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByUsernameAsync(string username);
    }
}