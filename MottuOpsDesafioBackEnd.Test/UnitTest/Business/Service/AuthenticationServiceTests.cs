using Moq;
using MottuOpsDesafioBackEnd.Business.Service;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.Business.Service
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IAuthenticationRepository> _mockAuthenticationRepository;
        private readonly AuthenticationService _authenticationService;
        private readonly AuthenticationRequest _validAuthenticationRequest;
        private readonly AuthenticationResponse _validAuthenticationResponse;

        public AuthenticationServiceTests()
        {
            _mockAuthenticationRepository = new Mock<IAuthenticationRepository>();
            _authenticationService = new AuthenticationService(_mockAuthenticationRepository.Object);

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
            _mockAuthenticationRepository
                .Setup(repo => repo.PostAsync(_validAuthenticationRequest))
                .ReturnsAsync(_validAuthenticationResponse);

            // Act
            var result = await _authenticationService.PostAsync(_validAuthenticationRequest);

            // Assert
            _mockAuthenticationRepository.Verify(repo => repo.PostAsync(_validAuthenticationRequest), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(_validAuthenticationResponse.Id, result?.Id);
            Assert.Equal(_validAuthenticationResponse.Username, result?.Username);
            Assert.Equal(_validAuthenticationResponse.PasswordHash, result?.PasswordHash);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro no repositório")]
        public async Task PostAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockAuthenticationRepository
                .Setup(repo => repo.PostAsync(It.IsAny<AuthenticationRequest>()))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _authenticationService.PostAsync(_validAuthenticationRequest));

            Assert.Equal("Erro no repositório", exception.Message);
        }
    }
}