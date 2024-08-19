using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Business.Interface
{
    public interface IPlanRentalService
    {
        Task<IEnumerable<PlanRentalModel>> GetAllAsync();
    }
}
