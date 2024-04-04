using VidhayakApp.Core.Entities;

namespace VidhayakApp.Web.ViewModels.AppUser
{
    public class SuggestionDetailsAndStatusUpdate
    {
        public IEnumerable<UserDetailAndFormDetailOnAppUserDashboardViewModel> UserDetailAndFormDetailOnAppUserDashboardViewModel { get; set; }


        public int? AppUserId { get; set; }
        public int ItemId { get; set; }
        public StatusType Status { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.Now.Date;

    }
}
