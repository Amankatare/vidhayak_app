using System.ComponentModel.DataAnnotations;
using VidhayakApp.Core.Entities;

namespace VidhayakApp.Web.ViewModels
{
    public class SuggestionFormViewModel
    {
     
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public StatusType Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? AppUserId { get; set; }
        public string? Note { get; set; }
        // Complaint-specific properties
        [Required(ErrorMessage = "Category is required")]
        public ItemType Type { get; set; }
        public int UserId { get; set; }
        
    }
}
