using Moq;
using MottuOpsDesafioBackEnd.Business.Service;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Business.Service
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;
        private readonly UserModel _validUserModel;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);

            // Criando um exemplo de UserModel válido
            _validUserModel = new UserModel
            {
                Id = 1,
                Username = "testuser",
                PasswordHash = "password123",
                ProfileId = 1
            };
        }

        [Fact(DisplayName = "Deve inserir um novo usuário e retornar seu ID")]
        public async Task PostAsync_ShouldReturnUserId_WhenUserModelIsValid()
        {
            // Arrange
            _mockUserRepository
                .Setup(repo => repo.PostAsync(_validUserModel))
                .ReturnsAsync(_validUserModel.Id);

            // Act
            var result = await _userService.PostAsync(_validUserModel);

            // Assert
            _mockUserRepository.Verify(repo => repo.PostAsync(_validUserModel), Times.Once);
            Assert.Equal(_validUserModel.Id, result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao inserir um novo usuário")]
        public async Task PostAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockUserRepository
                .Setup(repo => repo.PostAsync(It.IsAny<UserModel>()))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _userService.PostAsync(_validUserModel));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve retornar todos os usuários")]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var userList = new List<UserModel> { _validUserModel };
            _mockUserRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(userList);

            // Act
            var result = await _userService.GetAllAsync();

            // Assert
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            Assert.Equal(userList, result);
        }

        [Fact(DisplayName = "Deve retornar um usuário pelo ID")]
        public async Task GetByIdAsync_ShouldReturnUserModel_WhenIdIsValid()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository
                .Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(_validUserModel);

            // Act
            var result = await _userService.GetByIdAsync(userId);

            // Assert
            _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
            Assert.Equal(_validUserModel, result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao selecionar um usuário pelo ID")]
        public async Task GetByIdAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository
                .Setup(repo => repo.GetByIdAsync(userId))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _userService.GetByIdAsync(userId));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve atualizar um usuário")]
        public async Task PutAsync_ShouldUpdateUser_WhenUserModelIsValid()
        {
            // Arrange
            _mockUserRepository
                .Setup(repo => repo.PutAsync(_validUserModel))
                .Returns(Task.CompletedTask);

            // Act
            await _userService.PutAsync(_validUserModel);

            // Assert
            _mockUserRepository.Verify(repo => repo.PutAsync(_validUserModel), Times.Once);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao atualizar um usuário")]
        public async Task PutAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockUserRepository
                .Setup(repo => repo.PutAsync(It.IsAny<UserModel>()))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _userService.PutAsync(_validUserModel));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve deletar um usuário pelo ID")]
        public async Task DeleteAsync_ShouldDeleteUser_WhenIdIsValid()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository
                .Setup(repo => repo.DeleteAsync(userId))
                .Returns(Task.CompletedTask);

            // Act
            await _userService.DeleteAsync(userId);

            // Assert
            _mockUserRepository.Verify(repo => repo.DeleteAsync(userId), Times.Once);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao deletar um usuário")]
        public async Task DeleteAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository
                .Setup(repo => repo.DeleteAsync(userId))
                .ThrowsAsync(new Exception("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _userService.DeleteAsync(userId));

            Assert.Equal($"Erro ao deletar o usuário com ID {userId}", exception.Message);
        }

        [Fact(DisplayName = "Deve retornar um hash de senha válido")]
        public void HashPassword_ShouldReturnValidHash_WhenPasswordIsProvided()
        {
            // Arrange
            var password = "password123";

            // Act
            var result = _userService.HashPassword(password);

            // Assert
            Assert.NotNull(result);
            Assert.Contains(".", result);
        }

        [Fact(DisplayName = "Deve validar corretamente uma senha")]
        public void VerifyPassword_ShouldReturnTrue_WhenPasswordIsValid()
        {
            // Arrange
            var password = "password123";
            var hashedPassword = _userService.HashPassword(password);

            // Act
            var result = _userService.VerifyPassword(hashedPassword, password);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Deve falhar ao validar uma senha incorreta")]
        public void VerifyPassword_ShouldReturnFalse_WhenPasswordIsInvalid()
        {
            // Arrange
            var password = "password123";
            var hashedPassword = _userService.HashPassword(password);

            // Act
            var result = _userService.VerifyPassword(hashedPassword, "wrongpassword");

            // Assert
            Assert.False(result);
        }
    }
}