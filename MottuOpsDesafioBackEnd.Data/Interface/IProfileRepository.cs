using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Data.Interface
{
    public interface IProfileRepository
    {
        Task<int> PostAsync(UserProfileModel userProfileModel);
    }
}
