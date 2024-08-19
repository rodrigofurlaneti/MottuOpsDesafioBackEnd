using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Data.Interface
{
    public interface IMotorcycleRentalRepository
    {
        Task<int> PostAsync(MotorcycleRentalModel motorcycleRentalModel);
    }
}
