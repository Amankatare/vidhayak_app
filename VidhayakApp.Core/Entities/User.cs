using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VidhayakApp.Core.Entities
{
    public class User
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
      
        public string PasswordHash { get; set; } // Store hashed passwords

        // Add other fields 
        
        //optional fields

       

    }
}