using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Data.Interface
{
    public interface IPlanRentalRepository
    {
        Task<IEnumerable<PlanRentalModel>> GetAllAsync();
        Task<int> PostAsync(PlanRentalModel planRentalModel);
        Task<PlanRentalModel> GetByIdAsync(int planRentalId);
        Task PutAsync(PlanRentalModel planRentalModel);
        Task DeleteAsync(int planRentalId);
    }
}
