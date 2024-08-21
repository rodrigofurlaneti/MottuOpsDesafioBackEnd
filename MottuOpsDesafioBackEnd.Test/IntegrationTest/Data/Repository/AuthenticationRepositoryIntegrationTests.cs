using Microsoft.Extensions.Configuration;
using MottuOpsDesafioBackEnd.Data.Repository;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.IntegrationTest.Data.Repository
{
    public class AuthenticationRepositoryIntegrationTests
    {
        private readonly AuthenticationRepository _repository;
        private readonly string _connectionString;

        public AuthenticationRepositoryIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") 
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _repository = new AuthenticationRepository(configuration);
        }

        [Fact]
        public async Task PostAsync_ShouldReturnAuthenticationResponse_WhenCredentialsAreValid()
        {
            // Arrange
            var authenticationRequest = new AuthenticationRequest
            {
                Username = "admin", 
                Password = "admin"
            };

            // Act
            var result = await _repository.PostAsync(authenticationRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(authenticationRequest.Username, result.Username);
            Assert.NotEmpty(result.PasswordHash);
            Assert.True(result.ProfileId > 0);
        }

        [Fact]
        public async Task PostAsync_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var authenticationRequest = new AuthenticationRequest
            {
                Username = "invalidUsername",
                Password = "invalidPassword"
            };

            // Act
            var result = await _repository.PostAsync(authenticationRequest);

            // Assert
            Assert.Null(result);
        }
    }
}
