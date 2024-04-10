using VidhayakApp.Core.Entities;

namespace VidhayakApp.Web.ViewModels.AppUser
{
    public class UserDetailAndFormDetailOnAppUserDashboardViewModel
    {

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public Ward Ward { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string? ImagePath { get; set; }
        public ItemType Type { get; set; }
        public SubCategoryType SubCategory { get; set; }
        public int ItemId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public StatusType Status { get; set; }
        public string Note { get; set; }

        public string SchemeName { get; set; }
        public string DepartmentName { get; set; }

        public int SchemeId { get; set; }
        public int DepartmentId { get; set; }

    } 
}
