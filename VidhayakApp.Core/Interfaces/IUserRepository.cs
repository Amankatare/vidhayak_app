using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;

namespace VidhayakApp.Core.Interfaces
{
    public interface IUserRepository:IRepository<User>
    {
    }
}
