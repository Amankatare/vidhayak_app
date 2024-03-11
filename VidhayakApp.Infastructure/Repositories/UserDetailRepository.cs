using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infastructure.Repositories;
using VidhayakApp.Infrastructure.Data;

public class UserDetailRepository : Repository<UserDetail>, IUserDetailRepository
{
    public UserDetailRepository(VidhayakAppContext context) : base(context) { }
    

 
}
