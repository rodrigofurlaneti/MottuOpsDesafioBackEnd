using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Data.Repository;
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

        public Task DeleteAsync(int planRentalId)
        {
            try
            {
                return _planRentalRepository.DeleteAsync(planRentalId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao deletar o plano com ID {planRentalId}", ex.Message);

                throw new ApplicationException($"Erro ao deletar o plano com ID {planRentalId}", ex);
            }
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

        public Task<PlanRentalModel> GetByIdAsync(int planRentalId)
        {
            try
            {
                return _planRentalRepository.GetByIdAsync(planRentalId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao selecionar o plano com ID {planRentalId}", ex.Message);

                throw new ApplicationException($"Erro ao selecionar o plano com ID {planRentalId}", ex);
            }
        }

        public Task<int> PostAsync(PlanRentalModel planRentalModel)
        {
            try
            {
                return _planRentalRepository.PostAsync(planRentalModel);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao inserir um novo plano {planRentalModel.Identifier}", ex.Message);

                throw new ApplicationException($"Erro ao inserir um novo plano {planRentalModel.Identifier}", ex);
            }
        }

        public Task PutAsync(PlanRentalModel planRentalModel)
        {
            try
            {
                return _planRentalRepository.PutAsync(planRentalModel);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao atualizar um plano com ID {planRentalModel.Id}", ex.Message);

                throw new ApplicationException($"Erro ao atualizar um plano com ID {planRentalModel.Id}", ex);
            }
        }
    }
}
