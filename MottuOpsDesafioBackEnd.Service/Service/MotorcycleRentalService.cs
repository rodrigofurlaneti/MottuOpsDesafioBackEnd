using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Data.Repository;
using MottuOpsDesafioBackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuOpsDesafioBackEnd.Business.Service
{
    public class MotorcycleRentalService : IMotorcycleRentalService
    {
        private readonly IMotorcycleRentalRepository _motorcycleRentalRepository;

        public MotorcycleRentalService(IMotorcycleRentalRepository motorcycleRentalRepository)
        {
            _motorcycleRentalRepository = motorcycleRentalRepository;
        }

        public Task<int> PostAsync(MotorcycleRentalModel motorcycleRentalModel)
        {
            try
            {
                return _motorcycleRentalRepository.PostAsync(motorcycleRentalModel);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao inserir um novo aluguel de moto {motorcycleRentalModel.Courier}", ex.Message);

                throw new ApplicationException($"Erro ao inserir um novo aluguel de moto {motorcycleRentalModel.Courier}", ex);
            }
        }
    }
}
