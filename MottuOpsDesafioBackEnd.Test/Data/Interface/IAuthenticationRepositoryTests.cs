using Moq;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Data.Interface
{
    public class IAuthenticationRepositoryTests
    {
        private readonly Mock<IAuthenticationRepository> _mockAuthenticationRepository;
        private readonly AuthenticationRequest _validAuthenticationRequest;
        private readonly AuthenticationResponse _validAuthenticationResponse;

        public IAuthenticationRepositoryTests()
        {
            _mockAuthenticationRepository = new Mock<IAuthenticationRepository>();

            // Criando exemplos de dados válidos
            _validAuthenticationRequest = new AuthenticationRequest
            {
                Username = "validUser",
                Password = "validPassword"
            };

            _validAuthenticationResponse = new AuthenticationResponse
            {
                Id = 1,
                Username = "Username",
                PasswordHash = "PasswordHash"
            };
        }

        [Fact(DisplayName = "Deve chamar PostAsync com um AuthenticationRequest válido")]
        public async Task PostAsync_ShouldBeCalled_WithValidAuthenticationRequest()
        {
            // Arrange
            _mockAuthenticationRepository
                .Setup(repo => repo.PostAsync(_validAuthenticationRequest))
                .ReturnsAsync(_validAuthenticationResponse);

            // Act
            var result = await _mockAuthenticationRepository.Object.PostAsync(_validAuthenticationRequest);

            // Assert
            _mockAuthenticationRepository.Verify(repo => repo.PostAsync(_validAuthenticationRequest), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(_validAuthenticationResponse, result);
        }

        [Fact(DisplayName = "Deve retornar null quando AuthenticationRequest for inválido")]
        public async Task PostAsync_ShouldReturnNull_WithInvalidAuthenticationRequest()
        {
            // Arrange
            var invalidAuthenticationRequest = new AuthenticationRequest
            {
                Username = "invalidUser",
                Password = "invalidPassword"
            };

            _mockAuthenticationRepository
                .Setup(repo => repo.PostAsync(invalidAuthenticationRequest))
                .ReturnsAsync((AuthenticationResponse?)null);

            // Act
            var result = await _mockAuthenticationRepository.Object.PostAsync(invalidAuthenticationRequest);

            // Assert
            _mockAuthenticationRepository.Verify(repo => repo.PostAsync(invalidAuthenticationRequest), Times.Once);
            Assert.Null(result);
        }
    }
}