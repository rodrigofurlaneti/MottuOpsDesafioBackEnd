using Moq;
using MottuOpsDesafioBackEnd.Domain.Models;
using MottuOpsDesafioBackEnd.Business.Interface;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.Business.Interface
{
    public class IAuthenticationServiceTests
    {
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;
        private readonly AuthenticationRequest _validAuthenticationRequest;
        private readonly AuthenticationResponse _validAuthenticationResponse;

        public IAuthenticationServiceTests()
        {
            _mockAuthenticationService = new Mock<IAuthenticationService>();

            // Criando exemplos de dados válidos
            _validAuthenticationRequest = new AuthenticationRequest
            {
                Username = "validUser",
                Password = "validPassword"
            };

            _validAuthenticationResponse = new AuthenticationResponse
            {
                Id = 1,
                Username = "validUser",
                PasswordHash = "hashedPassword",
                ProfileId = 1,
                CourierId = null
            };
        }

        [Fact(DisplayName = "Deve retornar AuthenticationResponse quando as credenciais são válidas")]
        public async Task PostAsync_ShouldReturnAuthenticationResponse_WhenCredentialsAreValid()
        {
            // Arrange
            _mockAuthenticationService
                .Setup(service => service.PostAsync(_validAuthenticationRequest))
                .ReturnsAsync(_validAuthenticationResponse);

            // Act
            var result = await _mockAuthenticationService.Object.PostAsync(_validAuthenticationRequest);

            // Assert
            _mockAuthenticationService.Verify(service => service.PostAsync(_validAuthenticationRequest), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(_validAuthenticationResponse.Id, result.Id);
            Assert.Equal(_validAuthenticationResponse.Username, result.Username);
            Assert.Equal(_validAuthenticationResponse.PasswordHash, result.PasswordHash);
        }

        [Fact(DisplayName = "Deve retornar null quando as credenciais são inválidas")]
        public async Task PostAsync_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var invalidAuthenticationRequest = new AuthenticationRequest
            {
                Username = "invalidUser",
                Password = "invalidPassword"
            };

            _mockAuthenticationService
                .Setup(service => service.PostAsync(invalidAuthenticationRequest))
                .ReturnsAsync((AuthenticationResponse?)null);

            // Act
            var result = await _mockAuthenticationService.Object.PostAsync(invalidAuthenticationRequest);

            // Assert
            _mockAuthenticationService.Verify(service => service.PostAsync(invalidAuthenticationRequest), Times.Once);
            Assert.Null(result);
        }
    }
}