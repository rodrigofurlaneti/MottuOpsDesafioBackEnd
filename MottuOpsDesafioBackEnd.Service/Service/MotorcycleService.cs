using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Business.Service
{
    public class MotorcycleService : IMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;

        public MotorcycleService(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task DeleteAsync(int motorcycleId)
        {
            try
            {
                await _motorcycleRepository.DeleteAsync(motorcycleId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao deletar a moto com ID {motorcycleId}", ex.Message);

                throw new ApplicationException($"Erro ao deletar a moto com ID {motorcycleId}", ex);
            }
        }

        public Task<IEnumerable<MotorcycleModel>> GetAllAsync()
        {
            try
            {
                return _motorcycleRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao listar todas as motos", ex);
            }
        }

        public Task<MotorcycleModel> GetByIdAsync(int motorcycleId)
        {
            try
            {
                return _motorcycleRepository.GetByIdAsync(motorcycleId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao selecionar a moto com ID {motorcycleId}", ex.Message);

                throw new ApplicationException($"Erro ao selecionar a moto com ID {motorcycleId}", ex);
            }
        }

        public Task<int> PostAsync(MotorcycleModel motorcycleModel)
        {
            try
            {
                return _motorcycleRepository.PostAsync(motorcycleModel);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao inserir uma nova moto {motorcycleModel.Identifier}", ex.Message);

                throw new ApplicationException($"Erro ao inserir uma nova moto {motorcycleModel.Identifier}", ex);
            }
        }

        public Task PutAsync(MotorcycleModel motorcycleModel)
        {
            try
            {
                return _motorcycleRepository.PutAsync(motorcycleModel);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao atualizar uma moto com ID {motorcycleModel.Id}", ex.Message);

                throw new ApplicationException($"Erro ao atualizar uma moto com ID {motorcycleModel.Id}", ex);
            }
        }
    }
}
