using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Business.Service
{
    public class CourierService : ICourierService
    {
        private readonly ICourierRepository _courierRepository;

        public CourierService(ICourierRepository courierRepository)
        {
            _courierRepository = courierRepository;
        }

        public Task<int> PostAsync(CourierModel courierModel)
        {
            try
            {
                return _courierRepository.PostAsync(courierModel);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao inserir um novo entregador {courierModel.Identifier}", ex.Message);

                throw new ApplicationException($"Erro ao inserir um novo entregador {courierModel.Identifier}", ex);
            }
        }

        public Task<bool> GetByCnpjAsync(string cnpj)
        {
            try
            {
                return _courierRepository.GetByCnpjAsync(cnpj);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao consultar se o CNPJ já existe na base de dados {cnpj}", ex.Message);

                throw new ApplicationException($"Erro ao consultar se o CNPJ já existe na base de dados {cnpj}", ex);
            }
        }

        public Task<bool> GetByCnhAsync(string cnh)
        {
            try
            {
                return _courierRepository.GetByCnhAsync(cnh);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao consultar se a cnh já existe na base de dados {cnh}", ex.Message);

                throw new ApplicationException($"Erro ao consultar se o CNPJ já existe na base de dados {cnh}", ex);
            }
        }
    }
}
