using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Business.Interface
{
    public interface IMotorcycleRentalService
    {
        Task<int> PostAsync(MotorcycleRentalModel motorcycleRentalModel);
    }
}
