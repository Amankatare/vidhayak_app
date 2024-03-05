using Microsoft.EntityFrameworkCore;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infastructure.Repositories;
using VidhayakApp.Infrastructure.Data;

namespace VidhayakApp.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(VidhayakAppContext context) : base(context) { }
    }
}