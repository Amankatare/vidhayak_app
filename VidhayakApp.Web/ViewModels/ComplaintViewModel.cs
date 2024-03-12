using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VidhayakApp.Core.Entities;

namespace VidhayakApp.ViewModels
{
    public class ComplaintViewModel
    {
        
        [Required(ErrorMessage = "SubCategory is required")]
        public string SubCategory { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public string? Department { get; set; }

        // Item properties
        public int UserId { get; set; }

        // Complaint-specific properties
        [Required(ErrorMessage = "Category is required")]
        public ItemType Category { get; set; }

        // You can include other Item properties as needed

        // Constructor to initialize default values or perform additional setup
        public ComplaintViewModel()
        {
            // You can initialize default values or perform additional setup here
        }
    }
}
