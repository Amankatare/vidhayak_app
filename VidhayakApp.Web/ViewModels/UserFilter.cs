using VidhayakApp.Core.Entities;
using X.PagedList;

namespace VidhayakApp.Web.ViewModels
{
    public class UserFilter
    {
        public IPagedList<Item> Items { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public StatusType? statusType { get; set; }
    }
}
