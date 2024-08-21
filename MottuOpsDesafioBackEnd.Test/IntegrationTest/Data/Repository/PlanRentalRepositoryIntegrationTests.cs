using Microsoft.Extensions.Configuration;
using MottuOpsDesafioBackEnd.Data.Repository;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.IntegrationTest.Data.Repository
{
    public class PlanRentalRepositoryIntegrationTests
    {
        private readonly PlanRentalRepository _repository;
        private readonly string _connectionString;

        public PlanRentalRepositoryIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Arquivo de configuração específico para testes
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _repository = new PlanRentalRepository(configuration);
        }

        [Fact]
        public async Task PostAsync_ShouldReturnNewPlanRentalId_WhenSuccess()
        {
            // Arrange
            var planRentalModel = new PlanRentalModel
            {
                Identifier = "Plan-123",
                Days = "7",
                Value = "150.00",
                TerminationFine = "20%"
            };

            // Act
            var result = await _repository.PostAsync(planRentalModel);

            // Assert
            Assert.True(result > 0); // Verifica se um ID válido foi retornado
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfPlanRentals_WhenDataExists()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPlanRentalModel_WhenPlanRentalExists()
        {
            // Arrange
            var resultGetAll = await _repository.GetAllAsync();
            var listPlanRental = resultGetAll.ToList();

            // Certifique-se de que a lista não está vazia
            Assert.NotEmpty(listPlanRental);

            // Seleciona o ID do primeiro plano de aluguel na lista
            var planRentalId = listPlanRental.First().Id;

            // Act
            var result = await _repository.GetByIdAsync(planRentalId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(planRentalId, result.Id);
        }

        [Fact]
        public async Task PutAsync_ShouldUpdatePlanRental_WhenPlanRentalExists()
        {
            // Arrange
            var resultGetAll = await _repository.GetAllAsync();
            var listPlanRental = resultGetAll.ToList();

            // Seleciona o ID do primeiro plano de aluguel na lista
            var planRentalId = listPlanRental.First().Id;

            var planRentalModel = new PlanRentalModel
            {
                Id = planRentalId,
                Identifier = "Updated Plan-123",
                Days = "14",
                Value = "300.00",
                TerminationFine = "30%"
            };

            // Act
            await _repository.PutAsync(planRentalModel);

            // Assert
            var updatedPlanRental = await _repository.GetByIdAsync(planRentalModel.Id);
            Assert.Equal(planRentalModel.Identifier, updatedPlanRental.Identifier);
            Assert.Equal(planRentalModel.Days, updatedPlanRental.Days);
            Assert.Equal(planRentalModel.Value, updatedPlanRental.Value);
            Assert.Equal(planRentalModel.TerminationFine, updatedPlanRental.TerminationFine);
        }
    }
}
