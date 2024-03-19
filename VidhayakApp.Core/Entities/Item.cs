using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VidhayakApp.Core.Entities
{
    public class Item
    {
        [Key]
        [Column("ItemId")]
        public int ItemId { get; set; }
        public SubCategoryType SubCategoryTypeId { get; set; }
       
        public string Title { get; set; }
        public string Description { get; set; }
        public StatusType Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;  
       
        public DateTime? UpdatedAt{ get; set; }
        public string? ImagePath { get; set; }

        public int? AppUserId { get; set; }

        public string? Note { get; set; }

        // Enum for Item Type/Category
        public ItemType Type { get; set; }

        //ForeignKey
        public int UserId { get; set; }
        public User User{ get; set; }

       
    }

}
