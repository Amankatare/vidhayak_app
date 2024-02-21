using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //foreign key
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
