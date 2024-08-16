using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Business.Interface
{
    public interface IMotorcycleTypeService
    {
        Task<int> PostAsync(MotorcycleTypeModel motorcycleTypeModel);
        Task<IEnumerable<MotorcycleTypeModel>> GetAllAsync();
        Task<MotorcycleTypeModel> GetByIdAsync(int motorcycleTypeId);
        Task PutAsync(MotorcycleTypeModel motorcycleTypeModel);
        Task DeleteAsync(int motorcycleTypeId);
    }
}
