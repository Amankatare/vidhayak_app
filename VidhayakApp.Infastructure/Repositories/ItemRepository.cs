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
    public class ItemRepository: Repository<Item>,IItemRepository
    {
        public ItemRepository(VidhayakAppContext context) : base(context) { }

        //public async Task<List<Item?>> GetItemDetailsByUserIdAsync(int id)
        //{
        //    return await _context.Items
        //             .Include(ud => ud.User) // Include User navigation property if needed
        //             .FirstOrDefaultAsync(ud => ud.UserId == id);

        //}

        //public async Task<List<Item>> GetItemsByUserIdAsync(int userId)
        //{
        //    return await _context.Items
        //        .Where(item => item.UserId == userId)
        //        .ToListAsync();
        //}
    }
}
