using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Business.Interface
{
    public interface IUserService
    {
        Task<int> PostAsync(UserModel userModel);
        Task<IEnumerable<UserModel>> GetAllAsync();
        Task<UserModel> GetByIdAsync(int userId);
        Task PutAsync(UserModel userModel);
        Task DeleteAsync(int userId);
        string HashPassword(string password);
    }
}
