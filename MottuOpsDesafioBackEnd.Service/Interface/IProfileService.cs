using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Business.Interface
{
    public interface IProfileService
    {
        Task<int> PostAsync(UserProfileModel userModel);
        Task<IEnumerable<UserProfileModel>> GetAllAsync();
        Task<UserProfileModel> GetByIdAsync(int userProfileId);
        Task PutAsync(UserProfileModel userProfileModel);
        Task DeleteAsync(int userProfileId);
    }
}
