using Microsoft.AspNetCore.Mvc;
using VidhayakApp.Core.Interfaces;
using VidhayakApp.Web.ViewModels;
using System;
using System.Threading.Tasks;
using VidhayakApp.Core.Entities;

namespace VidhayakApp.Web.Controllers.Profile
{
    public class ProfileController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserDetailRepository _userDetailRepository;
        private readonly IWardRepository _wardRepository;
        private readonly IRoleRepository _roleRepository;

        public ProfileController(IUserRepository userRepository, IUserDetailRepository userDetailRepository, IWardRepository wardRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _userDetailRepository = userDetailRepository;
            _wardRepository = wardRepository;
            _roleRepository = roleRepository;
        }

        public async Task<IActionResult> ViewProfile()
        {
            var loggedInUser = HttpContext.Session.GetString("UserName");
            var loggedInUserId = HttpContext.Session.GetInt32("UserId");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch user data
            var user = await _userRepository.GetByUsernameAsync(loggedInUser);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch user detail data
            var userDetails = await _userDetailRepository.GetByIdAsync(user.UserId);

            // Fetch ward data
            var ward = await _wardRepository.GetByIdAsync(user.WardId);
            var role = await _roleRepository.GetByIdAsync(user.RoleId);
            // Map data to view model
            var profileViewModel = new ProfileViewModel
            {
                UserId = user.UserId,
                Name = user.Name,
                UserName = user.UserName,
                Dob = user.Dob,
                Address = user.Address,
                Ward = ward?.WardName, // Ensure ward is not null
                MobileNumber = user.MobileNumber,
                PasswordHash = user.PasswordHash,
                RoleId = user.RoleId,
                RoleName = role?.RoleName,
                DetailId = userDetails?.DetailId ?? 0, // Ensure userDetails is not null
                Education = userDetails?.Education,
                AadharNumber = userDetails?.AadharNumber,
                SamagraID = userDetails?.SamagraID,
                VoterID = userDetails?.VoterID,
                Caste = userDetails?.Caste,
                VoterCount = userDetails?.VoterCount,
                WardId = user.WardId,
                WardName = ward?.WardName // Ensure ward is not null
            };

            return View(profileViewModel);
        }

        //public async Task<IActionResult> UpdateProfile()
        //{
        //    var loggedInUser = HttpContext.Session.GetString("UserName");
        //    if (loggedInUser == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    // Fetch user data
        //    var user = await _userRepository.GetByUsernameAsync(loggedInUser);
        //    if (user == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    // Fetch user detail data
        //    var userDetails = await _userDetailRepository.GetByIdAsync(user.UserId);

        //    // Fetch ward data
        //    var ward = await _wardRepository.GetByIdAsync(user.WardId);
        //    var role = await _roleRepository.GetByIdAsync(user.RoleId);
        //    // Map data to view model
        //    var profileViewModel = new ProfileViewModel
        //    {
        //        UserId = user.UserId,


        //        Dob = user.Dob,
        //        Address = user.Address,
        //        Ward = ward?.WardName, // Ensure ward is not null
        //        MobileNumber = user.MobileNumber,
        //        // Ensure userDetails is not null
        //        Education = userDetails?.Education,
        //        AadharNumber = userDetails?.AadharNumber,
        //        SamagraID = userDetails?.SamagraID,
        //        VoterID = userDetails?.VoterID,
        //        Caste = userDetails?.Caste,
        //        VoterCount = userDetails?.VoterCount,
        //        WardId = user.WardId,
        //        // Ensure ward is not null
        //    };

        //    return View(profileViewModel);
        //}

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            var loggedInUser = HttpContext.Session.GetString("UserName");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch user data
            var user = await _userRepository.GetByUsernameAsync(loggedInUser);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch user detail data for the given user ID
            var userDetails = await _userDetailRepository.GetByIdAsync(user.UserId);

            // If user details don't exist, create a new one
            if (userDetails == null)
            {
                userDetails = new UserDetail();
                userDetails.UserId = user.UserId; // Assign the correct user ID
            }

            // Update user detail data with model values
            userDetails.Education = model.Education;
            userDetails.AadharNumber = model.AadharNumber;
            userDetails.SamagraID = model.SamagraID;
            userDetails.VoterID = model.VoterID;
            userDetails.Caste = model.Caste;
            userDetails.VoterCount = model.VoterCount;

            // Save changes to the user detail entity
            await _userDetailRepository.UpdateAsync(userDetails);

            // Redirect to the updated profile view
            return RedirectToAction("ViewProfile");
        }


    }
}
