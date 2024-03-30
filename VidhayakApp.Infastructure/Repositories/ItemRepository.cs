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
        
    }
}
