using System.ComponentModel.DataAnnotations;
using VidhayakApp.Core.Entities;

namespace VidhayakApp.Web.ViewModels
{
    public class CreateUserModel
    {
       
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Ward is required")]
        public string Ward { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        public List<User> AppUsers { get; set; }
    }
}
