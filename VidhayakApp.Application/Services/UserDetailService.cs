using VidhayakApp.Core.Entities;

public class UserDetailService : IUserDetailService
{
    private readonly IUserDetailRepository _userDetailRepository; 

    public UserDetailService(IUserDetailRepository userDetailRepository)
    {
        _userDetailRepository = userDetailRepository;
    }

    public async Task<UserDetail> GetByIdAsync(int id)
    {
        return await _userDetailRepository.GetByIdAsync(id);
    }

    public async Task UpdateAsync(UserDetail userDetail)
    {
        await _userDetailRepository.UpdateAsync(userDetail);
   
    }

    // Add other methods as needed
}
