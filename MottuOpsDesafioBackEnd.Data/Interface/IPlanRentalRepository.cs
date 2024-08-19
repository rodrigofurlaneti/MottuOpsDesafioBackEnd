using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Data.Interface
{
    public interface IPlanRentalRepository
    {
        Task<IEnumerable<PlanRentalModel>> GetAllAsync();
    }
}
