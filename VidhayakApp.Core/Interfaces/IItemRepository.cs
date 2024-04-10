using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhayakApp.Core.Entities;

namespace VidhayakApp.Core.Interfaces
{
    public interface IItemRepository:IRepository<Item>
    {
        //public Task<List<Item>> GetItemDetailsByUserIdAsync(int id);

        //public  Task<List<Item>> GetItemsByUserIdAsync(int userId);
        
    }
}
