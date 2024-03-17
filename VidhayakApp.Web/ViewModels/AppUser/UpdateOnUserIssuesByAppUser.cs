using VidhayakApp.Core.Entities;

namespace VidhayakApp.Web.ViewModels.AppUser
{
    public class UpdateOnUserIssuesByAppUser
    {
        public int UserId { get; set; }
        public int AppUserId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public StatusType Status { get; set; }
        public string Note { get; set; }
    }
}
