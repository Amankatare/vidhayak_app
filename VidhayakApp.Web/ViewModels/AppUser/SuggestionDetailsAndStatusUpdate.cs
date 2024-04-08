using VidhayakApp.Core.Entities;
using X.PagedList;

namespace VidhayakApp.Web.ViewModels.AppUser
{
    public class SuggestionDetailsAndStatusUpdate
    {
        public IPagedList<UserDetailAndFormDetailOnAppUserDashboardViewModel> UserDetailAndFormDetailOnAppUserDashboardViewModel { get; set; }


        public int? AppUserId { get; set; }
        public int ItemId { get; set; }
        public StatusType Status { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.Now.Date;

        public StatusType? statusType { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
}
