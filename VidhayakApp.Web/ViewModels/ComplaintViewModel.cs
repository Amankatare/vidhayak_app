using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VidhayakApp.Core.Entities;

namespace VidhayakApp.ViewModels
{
    public class ComplaintViewModel
    {

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public int? DepartmentId { get; set; }

        // Item properties
        public int UserId { get; set; }

        public int SubCategoryId { get; set; }

        // Complaint-specific properties
        [Required(ErrorMessage = "Category is required")]
        public ItemType Category { get; set; }

     
    }
}
