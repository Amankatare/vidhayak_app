using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VidhayakApp.Core.Entities;

namespace VidhayakApp.Web.ViewModels
{
    public class UserViewModel
    {
        [Key]
        [Column("UserId")]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public string Ward { get; set; }
        public string MobileNumber { get; set; }

        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        // Additional properties for UserDetail
        public int DetailId { get; set; }
        public string Education { get; set; }
        public string AadharNumber { get; set; }
        public string SamagraID { get; set; }
        public string VoterID { get; set; }
        public string Caste { get; set; }
    }


}
