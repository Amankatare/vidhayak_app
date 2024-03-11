

    namespace VidhayakApp.Web.ViewModels
    {
        public class UserWithDetailsViewModel
        {
            // Properties from the User entity
            public int UserId { get; set; }
            public string Name { get; set; }
            public string UserName { get; set; }
            public DateTime Dob { get; set; }
            public string Address { get; set; }
            public string Ward { get; set; }
            public string MobileNumber { get; set; }
            public string PasswordHash { get; set; }
            public int RoleId { get; set; }
            // Add other fields from User entity

            // Properties from the UserDetail entity
            public int DetailId { get; set; }
            public string Education { get; set; }
            public string AadharNumber { get; set; }
            public string SamagraID { get; set; }
            public string VoterID { get; set; }
            public string Caste { get; set; }
            // Add other fields from UserDetail entity
        }
    }


