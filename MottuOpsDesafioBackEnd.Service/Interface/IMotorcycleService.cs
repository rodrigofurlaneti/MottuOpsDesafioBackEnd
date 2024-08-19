using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Business.Interface
{
    public interface IMotorcycleService
    {
        Task<int> PostAsync(MotorcycleModel motorcycleModel);
        Task<IEnumerable<MotorcycleModel>> GetAllAsync();
        Task<MotorcycleModel> GetByIdAsync(int motorcycleId);
        Task<bool> GetByLicensePlateAsync(string licensePlate);
        Task PutAsync(MotorcycleModel motorcycleModel);
        Task DeleteAsync(int motorcycleId);
    }
}
