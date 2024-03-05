using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data;

namespace VidhayakApp.Infastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public readonly VidhayakAppContext _context;
        public Repository(VidhayakAppContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            var addedEntity = _context.Set<T>().Add(entity).Entity;
            await _context.SaveChangesAsync(); 

            return addedEntity;
        }

        public async Task DeleteAsync(T entity)
        { 
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();

        }
        //public async Task DeletebyIdAsync(int entity)
        //{
        //    var existingEntity = await _context.Set<T>().FindAsync(entity);

        //    if (existingEntity != null)
        //    {
        //        _context.Set<T>().Remove(existingEntity);
        //        await _context.SaveChangesAsync();
        //    }
        //}

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByUsernameAsync(string username)
        {
            T record = await _context.Set<T>().FirstOrDefaultAsync(u => EF.Property<string>(u, "UserName") == username);

            return record;
        }

        public async Task<IEnumerable<T>> ListAllAsync()
        {
            return _context.Set<T>().ToList();
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
