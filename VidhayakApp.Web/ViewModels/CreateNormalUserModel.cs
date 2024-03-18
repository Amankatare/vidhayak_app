using System.ComponentModel.DataAnnotations;

namespace VidhayakApp.Web.ViewModels
{
    public class CreateNormalUserModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Ward is required")]
        public int WardId { get; set; }
        public string Dob { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }


        public string? Education { get; set; }
        public string? AadharNumber { get; set; }
        public string? SamagraID { get; set; }
        public string? VoterID { get; set; }
        public string? Caste { get; set; }
    }
}
