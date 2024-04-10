using VidhayakApp.Core.Entities;
using VidhayakApp.ViewModels;

namespace VidhayakApp.Web.ViewModels
{
    public class UserComplaintsViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public Ward Ward { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }

        // List of complaints
        public List<ComplaintViewModel> Complaints { get; set; }
    }
}
