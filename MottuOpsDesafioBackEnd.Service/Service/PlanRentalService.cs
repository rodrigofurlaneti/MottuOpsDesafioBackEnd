using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Business.Service
{
    public class PlanRentalService : IPlanRentalService
    {
        private readonly IPlanRentalRepository _planRentalRepository;

        public PlanRentalService(IPlanRentalRepository planRentalRepository)
        {
            _planRentalRepository = planRentalRepository;
        }

        public Task<IEnumerable<PlanRentalModel>> GetAllAsync()
        {
            try
            {
                return _planRentalRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao listar todos os planos", ex);
            }
        }
    }
}
