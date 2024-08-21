using Microsoft.Extensions.Configuration;
using MottuOpsDesafioBackEnd.Data.Repository;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.IntegrationTest.Data.Repository
{
    public class MotorcycleTypeRepositoryIntegrationTests
    {
        private readonly MotorcycleTypeRepository _repository;
        private readonly string _connectionString;

        public MotorcycleTypeRepositoryIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") 
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _repository = new MotorcycleTypeRepository(configuration);
        }

        [Fact]
        public async Task PostAsync_ShouldReturnNewMotorcycleTypeId_WhenSuccess()
        {
            // Arrange
            var motorcycleTypeModel = new MotorcycleTypeModel
            {
                TypeName = "Off-Road"
            };

            // Act
            var result = await _repository.PostAsync(motorcycleTypeModel);

            // Assert
            Assert.True(result > 0); // Verifica se um ID válido foi retornado
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfMotorcycleTypes_WhenDataExists()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMotorcycleTypeModel_WhenMotorcycleTypeExists()
        {
            // Arrange
            var resultGetAll = await _repository.GetAllAsync();

            var listMotorcycleType = resultGetAll.ToList();

            Assert.NotEmpty(listMotorcycleType);

            var motorcycleTypeId = listMotorcycleType.First().Id;

            // Act
            var result = await _repository.GetByIdAsync(motorcycleTypeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(motorcycleTypeId, result.Id);
        }

        [Fact]
        public async Task PutAsync_ShouldUpdateMotorcycleType_WhenMotorcycleTypeExists()
        {
            // Arrange
            var resultGetAll = await _repository.GetAllAsync();

            var listMotorcycleType = resultGetAll.ToList();

            var motorcycleTypeId = listMotorcycleType.First().Id;

            var motorcycleTypeModel = new MotorcycleTypeModel
            {
                Id = motorcycleTypeId, 
                TypeName = "Updated Type Name"
            };

            // Act
            await _repository.PutAsync(motorcycleTypeModel);

            // Assert
            var updatedMotorcycleType = await _repository.GetByIdAsync(motorcycleTypeModel.Id);
            Assert.Equal(motorcycleTypeModel.TypeName, updatedMotorcycleType.TypeName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveMotorcycleType_WhenMotorcycleTypeExists()
        {
            // Arrange
            var resultGetAll = await _repository.GetAllAsync();
            var listMotorcycleType = resultGetAll.ToList();

            // Certifique-se de que a lista não está vazia
            Assert.NotEmpty(listMotorcycleType);

            // Seleciona o ID do primeiro tipo de moto na lista para deletar
            var motorcycleTypeId = listMotorcycleType.First().Id;

            // Act
            await _repository.DeleteAsync(motorcycleTypeId);

            // Assert
            var deletedMotorcycleType = await _repository.GetByIdAsync(motorcycleTypeId);
            Assert.Null(deletedMotorcycleType);
        }
    }
}
