using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VidhayakApp.Core.Entities
{
    public class GovtScheme
    {
        [Key]
        [Column("SchemeId")]
        public int SchemeId { get; set; }
        public string SchemeName { get; set; }
        public string? Description { get; set; }
    }
}