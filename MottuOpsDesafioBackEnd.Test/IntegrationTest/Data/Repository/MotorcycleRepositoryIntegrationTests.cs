using Microsoft.Extensions.Configuration;
using MottuOpsDesafioBackEnd.Data.Repository;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.IntegrationTest.Data.Repository
{
    public class MotorcycleRepositoryIntegrationTests
    {
        private readonly MotorcycleRepository _repository;
        private readonly string _connectionString;

        public MotorcycleRepositoryIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") 
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _repository = new MotorcycleRepository(configuration);
        }

        [Fact]
        public async Task PostAsync_ShouldReturnNewMotorcycleId_WhenSuccess()
        {
            // Arrange
            var motorcycleModel = new MotorcycleModel
            {
                Identifier = "Moto-123",
                LicensePlate = "XYZ-1234",
                Model = "Model X",
                Year = 2023
            };

            // Act
            var result = await _repository.PostAsync(motorcycleModel);

            // Assert
            Assert.True(result > 0); 
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfMotorcycles_WhenDataExists()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMotorcycleModel_WhenMotorcycleExists()
        {
            // Arrange
            var resultGetAll = await _repository.GetAllAsync();
            
            var listMotorcycle = resultGetAll.ToList();

            var motorcycleId = listMotorcycle.First().Id;

            // Act
            var result = await _repository.GetByIdAsync(motorcycleId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(motorcycleId, result.Id);
        }

        [Fact]
        public async Task GetByLicensePlateAsync_ShouldReturnTrue_WhenLicensePlateExists()
        {
            // Arrange
            string existingLicensePlate = "XYZ-1234"; // Certifique-se de que esta placa existe no banco de dados de teste

            // Act
            var result = await _repository.GetByLicensePlateAsync(existingLicensePlate);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetByLicensePlateAsync_ShouldReturnFalse_WhenLicensePlateDoesNotExist()
        {
            // Arrange
            string nonExistingLicensePlate = "ABC-9999"; // Certifique-se de que esta placa não existe no banco de dados de teste

            // Act
            var result = await _repository.GetByLicensePlateAsync(nonExistingLicensePlate);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task PutAsync_ShouldUpdateMotorcycle_WhenMotorcycleExists()
        {
            // Arrange
            var motorcycleModel = new MotorcycleModel
            {
                Id = 1, // Certifique-se de que este ID existe no banco de dados de teste
                Identifier = "Moto-456",
                LicensePlate = "XYZ-5678",
                Model = "Model Y",
                Year = 2024
            };

            // Act
            await _repository.PutAsync(motorcycleModel);

            // Assert
            var updatedMotorcycle = await _repository.GetByIdAsync(motorcycleModel.Id);
            Assert.Equal(motorcycleModel.LicensePlate, updatedMotorcycle.LicensePlate);
            Assert.Equal(motorcycleModel.Model, updatedMotorcycle.Model);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveMotorcycle_WhenMotorcycleExists()
        {
            // Arrange
            int motorcycleId = 1; // Certifique-se de que este ID existe no banco de dados de teste

            // Act
            await _repository.DeleteAsync(motorcycleId);

            // Assert
            var deletedMotorcycle = await _repository.GetByIdAsync(motorcycleId);
            Assert.Null(deletedMotorcycle);
        }
    }
}
