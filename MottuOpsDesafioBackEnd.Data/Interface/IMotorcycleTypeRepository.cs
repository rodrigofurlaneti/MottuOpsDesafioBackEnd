using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Data.Interface
{
    public interface IMotorcycleTypeRepository
    {
        Task<int> PostAsync(MotorcycleTypeModel motorcycleTypeModel);
        Task<IEnumerable<MotorcycleTypeModel>> GetAllAsync();
        Task<MotorcycleTypeModel> GetByIdAsync(int motorcycleTypeId);
        Task PutAsync(MotorcycleTypeModel motorcycleTypeModel);
        Task DeleteAsync(int motorcycleTypeId);
    }
}
