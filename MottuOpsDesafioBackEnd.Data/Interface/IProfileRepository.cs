using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Data.Interface
{
    public interface IProfileRepository
    {
        Task<int> PostAsync(UserProfileModel userProfileModel);
        Task<IEnumerable<UserProfileModel>> GetAllAsync();
        Task<UserProfileModel> GetByIdAsync(int userProfileId);
        Task PutAsync(UserProfileModel userProfileModel);
    }
}
