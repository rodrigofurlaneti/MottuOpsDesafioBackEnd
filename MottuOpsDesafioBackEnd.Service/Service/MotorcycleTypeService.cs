using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Business.Service
{
    public class MotorcycleTypeService : IMotorcycleTypeService
    {
        private readonly IMotorcycleTypeRepository _motorcycleTypeRepository;

        public MotorcycleTypeService(IMotorcycleTypeRepository motorcycleTypeRepository)
        {
            _motorcycleTypeRepository = motorcycleTypeRepository;
        }

        public async Task DeleteAsync(int motorcycleId)
        {
            try
            {
                await _motorcycleTypeRepository.DeleteAsync(motorcycleId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao deletar o modelo da moto com ID {motorcycleId}", ex.Message);

                throw new ApplicationException($"Erro ao deletar o modelo moto com ID {motorcycleId}", ex);
            }
        }

        public Task<IEnumerable<MotorcycleTypeModel>> GetAllAsync()
        {
            try
            {
                return _motorcycleTypeRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao listar todos os modelos das motos", ex);
            }
        }

        public Task<MotorcycleTypeModel> GetByIdAsync(int motorcycleTypeId)
        {
            try
            {
                return _motorcycleTypeRepository.GetByIdAsync(motorcycleTypeId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao selecionar o modelo da moto com ID {motorcycleTypeId}", ex.Message);

                throw new ApplicationException($"Erro ao selecionar o modelo da moto com ID {motorcycleTypeId}", ex);
            }
        }

        public Task<int> PostAsync(MotorcycleTypeModel motorcycleTypeModel)
        {
            try
            {
                return _motorcycleTypeRepository.PostAsync(motorcycleTypeModel);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao inserir um novo modelo da moto {motorcycleTypeModel.TypeName}", ex.Message);

                throw new ApplicationException($"Erro ao inserir um novo modelo da moto {motorcycleTypeModel.TypeName}", ex);
            }
        }

        public Task PutAsync(MotorcycleTypeModel motorcycleTypeModel)
        {
            try
            {
                return _motorcycleTypeRepository.PutAsync(motorcycleTypeModel);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao atualizar um modelo da moto com ID {motorcycleTypeModel.Id}", ex.Message);

                throw new ApplicationException($"Erro ao atualizar um modelo da moto com ID {motorcycleTypeModel.Id}", ex);
            }
        }
    }
}
