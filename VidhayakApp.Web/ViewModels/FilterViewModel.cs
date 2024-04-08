using VidhayakApp.Core.Entities;
using VidhayakApp.Web.ViewModels.AppUser;
using X.PagedList;

namespace VidhayakApp.Web.ViewModels
{
    public class FilterViewModel
    {
        public IPagedList<UserDetailAndFormDetailOnAppUserDashboardViewModel> userDetailAndFormDetailOnAppUserDashboardViewModel { get; set; }

        public StatusType? statusType { get; set; }

        public DateTime? FromDate  { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
