using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhayakApp.Core.Entities
{
    public class Item
    {
        public int ItemId { get; set; }
       
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt{ get; set; }

        // Enum for Item Type/Category
        public ItemType Type { get; set; }

        //ForeignKey
        public int UserId { get; set; }
        public User User{ get; set; }

        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; }

        [ForeignKey("GovtDepartment")]
        public int? DepartmentId { get; set; }
        public virtual GovtDepartment GovtDepartment { get; set; }

        [ForeignKey("GovtScheme")]
        public int? SchemeId { get; set; }
        public virtual GovtScheme GovtScheme { get; set; }
    }

}
