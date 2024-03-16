using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VidhayakApp.Core.Entities
{
    public class GovtDepartment
    {
        [Key]
        [Column("DepartmentId")]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string? Description { get; set; }
    }
}