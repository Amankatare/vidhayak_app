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
    public class GovtSchemeRepository : Repository<GovtScheme>, IGovtSchemeRepository
    {
        public GovtSchemeRepository(VidhayakAppContext context) : base(context) { }
    }
}
