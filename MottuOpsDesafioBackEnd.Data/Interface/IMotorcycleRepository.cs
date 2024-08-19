using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Data.Interface
{
    public interface IMotorcycleRepository
    {
        Task<int> PostAsync(MotorcycleModel motorcycleModel);
        Task<IEnumerable<MotorcycleModel>> GetAllAsync();
        Task<MotorcycleModel> GetByIdAsync(int motorcycleId);
        Task<bool> GetByLicensePlateAsync(string licensePlate);
        Task PutAsync(MotorcycleModel motorcycleModel);
        Task DeleteAsync(int motorcycleId);
    }
}
