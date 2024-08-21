using Microsoft.Extensions.Configuration;
using MottuOpsDesafioBackEnd.Data.Repository;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.IntegrationTest.Data.Repository
{
    public class ProfileRepositoryIntegrationTests
    {
        private readonly ProfileRepository _repository;
        private readonly string _connectionString;

        public ProfileRepositoryIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Arquivo de configuração específico para testes
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _repository = new ProfileRepository(configuration);
        }

        [Fact]
        public async Task PostAsync_ShouldReturnNewUserProfileId_WhenSuccess()
        {
            // Arrange
            var userProfileModel = new UserProfileModel
            {
                ProfileName = "Admin"
            };

            // Act
            var result = await _repository.PostAsync(userProfileModel);

            // Assert
            Assert.True(result > 0); // Verifica se um ID válido foi retornado
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfUserProfiles_WhenDataExists()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUserProfileModel_WhenUserProfileExists()
        {
            // Arrange
            var resultGetAll = await _repository.GetAllAsync();
            var listUserProfile = resultGetAll.ToList();

            // Certifique-se de que a lista não está vazia
            Assert.NotEmpty(listUserProfile);

            // Seleciona o ID do primeiro perfil de usuário na lista
            var userProfileId = listUserProfile.First().Id;

            // Act
            var result = await _repository.GetByIdAsync(userProfileId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userProfileId, result.Id);
        }

        [Fact]
        public async Task PutAsync_ShouldUpdateUserProfile_WhenUserProfileExists()
        {
            // Arrange
            var resultGetAll = await _repository.GetAllAsync();

            var listUserProfile = resultGetAll.ToList();

            var userProfileId = listUserProfile.First().Id;

            var userProfileModel = new UserProfileModel
            {
                Id = userProfileId,
                ProfileName = "Updated Profile"
            };

            // Act
            await _repository.PutAsync(userProfileModel);

            // Assert
            var updatedUserProfile = await _repository.GetByIdAsync(userProfileModel.Id);
            Assert.Equal(userProfileModel.ProfileName, updatedUserProfile.ProfileName);
        }
    }
}
