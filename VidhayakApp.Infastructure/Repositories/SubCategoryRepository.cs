using VidhayakApp.Core.Entities;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Infrastructure.Data;

namespace VidhayakApp.Infastructure.Repositories
{
    public class SubCategoryRepository: Repository<SubCategory>,ISubCategoryRepository
    {
        public SubCategoryRepository(VidhayakAppContext context) : base(context) { }
    }
}
