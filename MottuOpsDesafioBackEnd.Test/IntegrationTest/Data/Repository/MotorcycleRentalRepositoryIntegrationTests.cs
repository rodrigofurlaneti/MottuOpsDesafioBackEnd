using Microsoft.Extensions.Configuration;
using MottuOpsDesafioBackEnd.Data.Repository;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.IntegrationTest.Data.Repository
{
    public class MotorcycleRentalRepositoryIntegrationTests
    {
        private readonly MotorcycleRentalRepository _repository;
        private readonly string _connectionString;

        public MotorcycleRentalRepositoryIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") 
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _repository = new MotorcycleRentalRepository(configuration);
        }

        [Fact]
        public async Task GetByCourierIdAsync_ShouldReturnMotorcycleRentalModel_WhenRentalExists()
        {
            // Arrange
            int courierId = 1; // Certifique-se de que este ID existe no banco de dados de teste

            // Act
            var result = await _repository.GetByCourierIdAsync(courierId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(courierId, result.CourierId);
        }

    }
}
