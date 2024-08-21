using Microsoft.Extensions.Configuration;
using MottuOpsDesafioBackEnd.Data.Repository;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.IntegrationTest.Data.Repository
{
    public class UserRepositoryIntegrationTests
    {
        private readonly UserRepository _repository;
        private readonly string _connectionString;

        public UserRepositoryIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Arquivo de configuração específico para testes
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _repository = new UserRepository(configuration);
        }

        [Fact]
        public async Task PostAsync_ShouldReturnNewUserId_WhenSuccess()
        {
            // Arrange
            var userModel = new UserModel
            {
                Username = "testuser",
                PasswordHash = "hashedpassword",
                ProfileId = 1 // Certifique-se de que este ID de perfil existe no banco de dados de teste
            };

            // Act
            var result = await _repository.PostAsync(userModel);

            // Assert
            Assert.True(result > 0); // Verifica se um ID válido foi retornado
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfUsers_WhenDataExists()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUserModel_WhenUserExists()
        {
            // Arrange
            var resultGetAll = await _repository.GetAllAsync();
            var listUsers = resultGetAll.ToList();

            // Certifique-se de que a lista não está vazia
            Assert.NotEmpty(listUsers);

            // Seleciona o ID do primeiro usuário na lista
            var userId = listUsers.First().Id;

            // Act
            var result = await _repository.GetByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }

        [Fact]
        public async Task PutAsync_ShouldUpdateUser_WhenUserExists()
        {
            // Arrange
            var resultGetAll = await _repository.GetAllAsync();
            var listUsers = resultGetAll.ToList();

            // Seleciona o ID do primeiro usuário na lista
            var userId = listUsers.First().Id;

            var userModel = new UserModel
            {
                Id = userId,
                Username = "updateduser",
                PasswordHash = "updatedhashedpassword",
                ProfileId = 1 // Certifique-se de que este ID de perfil existe no banco de dados de teste
            };

            // Act
            await _repository.PutAsync(userModel);

            // Assert
            var updatedUser = await _repository.GetByIdAsync(userModel.Id);
            Assert.Equal(userModel.Username, updatedUser.Username);
            Assert.Equal(userModel.PasswordHash, updatedUser.PasswordHash);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveUser_WhenUserExists()
        {
            // Arrange
            var resultGetAll = await _repository.GetAllAsync();
            var listUsers = resultGetAll.ToList();

            // Certifique-se de que a lista não está vazia
            Assert.NotEmpty(listUsers);

            // Seleciona o ID do primeiro usuário na lista para deletar
            var userId = listUsers.First().Id;

            // Act
            await _repository.DeleteAsync(userId);

            // Assert
            var deletedUser = await _repository.GetByIdAsync(userId);
            Assert.Null(deletedUser);
        }
    }
}
