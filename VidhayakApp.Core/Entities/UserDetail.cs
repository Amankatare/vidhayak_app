using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VidhayakApp.Core.Entities
{
    public class UserDetail
    {
        [Key]
        [Column("DetailId")]
        public int DetailId { get; set; }

        public string? Education { get; set; }
        public string? AadharNumber { get; set; }
        public string? SamagraID { get; set; }
        public string? VoterID { get; set; }
        public string? Caste { get; set; }

        // Foreign key
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
