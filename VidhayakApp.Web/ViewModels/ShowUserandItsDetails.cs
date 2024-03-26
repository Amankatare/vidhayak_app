using VidhayakApp.Core.Entities;

namespace VidhayakApp.Web.ViewModels
{
    public class ShowUserandItsDetails
    { 
            public IEnumerable<User> usersTable { get; set; }
            public IEnumerable<UserDetail> userDetailsTable { get; set; }

            public IEnumerable<Ward> userWardTable { get; set; }

            public User usersTableforView { get; set; }
            public UserDetail userDetailsTableforView { get; set; }

            public Ward userWardTableforView { get; set; }

    }
}



