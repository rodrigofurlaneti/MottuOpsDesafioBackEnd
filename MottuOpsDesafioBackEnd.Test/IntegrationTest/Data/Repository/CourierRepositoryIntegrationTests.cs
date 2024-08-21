using Microsoft.Extensions.Configuration;
using MottuOpsDesafioBackEnd.Data.Repository;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.IntegrationTest.Data.Repository
{
    public class CourierRepositoryIntegrationTests
    {
        private readonly CourierRepository _repository;
        private readonly string _connectionString;

        public CourierRepositoryIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _repository = new CourierRepository(configuration);
        }

        [Fact]
        public async Task PostAsync_ShouldReturnNewCourierId_WhenSuccess()
        {
            // Arrange
            var courierModel = new CourierModel
            {
                Identifier = "123456",
                Name = "Test Courier",
                CNPJ = "12.345.678/0001-95",
                BirthDate = new DateTime(1990, 1, 1),
                CNHNumber = "123456789",
                CNHType = "A",
                CNHImagePath = "/path/to/image",
                Username = "testusername",
                PasswordHash = "hashedpassword",
                ProfileId = 1
            };

            // Act
            var result = await _repository.PostAsync(courierModel);

            // Assert
            Assert.True(result > 0); // Verifica se um ID válido foi retornado
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCourierModel_WhenCourierExists()
        {
            // Arrange
            int courierId = 1; // Certifique-se de que este ID existe no banco de dados de teste

            // Act
            var result = await _repository.GetByIdAsync(courierId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(courierId, result.Id);
        }

        //[Fact]
        //public async Task PutCnhAsync_ShouldUpdateCnhImagePath_WhenSuccess()
        //{
        //    // Arrange
        //    var courierModel = new CourierModel
        //    {
        //        Id = 1, // Certifique-se de que este ID existe no banco de dados de teste
        //        CNHImagePath = "/new/path/to/image"
        //    };

        //    // Act
        //    await _repository.PutCnhAsync(courierModel);

        //    // Assert
        //    var updatedCourier = await _repository.GetByIdAsync(courierModel.Id);
        //    Assert.Equal(courierModel.CNHImagePath, updatedCourier.CNHImagePath);
        //}

        [Fact]
        public async Task GetByCnpjAsync_ShouldReturnTrue_WhenCnpjExists()
        {
            // Arrange
            string existingCnpj = "12.345.678/0001-95"; // Certifique-se de que este CNPJ existe no banco de dados de teste

            // Act
            var result = await _repository.GetByCnpjAsync(existingCnpj);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetByCnpjAsync_ShouldReturnFalse_WhenCnpjDoesNotExist()
        {
            // Arrange
            string nonExistingCnpj = "99.999.999/0001-99"; // Certifique-se de que este CNPJ não existe no banco de dados de teste

            // Act
            var result = await _repository.GetByCnpjAsync(nonExistingCnpj);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetByCnhAsync_ShouldReturnTrue_WhenCnhExists()
        {
            // Arrange
            string existingCnh = "123456789"; 

            // Act
            var result = await _repository.GetByCnhAsync(existingCnh);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetByCnhAsync_ShouldReturnFalse_WhenCnhDoesNotExist()
        {
            // Arrange
            string nonExistingCnh = "987654321"; 

            // Act
            var result = await _repository.GetByCnhAsync(nonExistingCnh);

            // Assert
            Assert.False(result);
        }
    }
}
