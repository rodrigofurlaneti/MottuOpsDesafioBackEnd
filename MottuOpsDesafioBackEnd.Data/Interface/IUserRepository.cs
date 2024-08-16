using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Data.Interface
{
    public interface IUserRepository
    {
        Task<int> PostAsync(UserModel userModel);
        Task<IEnumerable<UserModel>> GetAllAsync();
        Task<UserModel> GetByIdAsync(int userId);
        Task PutAsync(UserModel userModel);
        Task DeleteAsync(int userId);
    }
}
