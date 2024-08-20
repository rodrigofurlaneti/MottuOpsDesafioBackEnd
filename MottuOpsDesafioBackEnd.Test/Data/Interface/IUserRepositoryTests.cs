using Moq;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Data.Interface
{
    public class IUserRepositoryTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserModel _validUserModel;
        private readonly List<UserModel> _userList;

        public IUserRepositoryTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();

            // Criando um exemplo de UserModel válido
            _validUserModel = new UserModel
            {
                Id = 1,
                Username = "testuser",
                PasswordHash = "password123",
                ProfileName = "testuser",
                ProfileId = 1
            };

            // Criando uma lista de exemplo
            _userList = new List<UserModel> { _validUserModel };
        }

        [Fact(DisplayName = "Deve inserir um novo usuário e retornar seu ID")]
        public async Task PostAsync_ShouldReturnUserId_WhenUserModelIsValid()
        {
            // Arrange
            _mockUserRepository
                .Setup(repo => repo.PostAsync(_validUserModel))
                .ReturnsAsync(_validUserModel.Id);

            // Act
            var result = await _mockUserRepository.Object.PostAsync(_validUserModel);

            // Assert
            _mockUserRepository.Verify(repo => repo.PostAsync(_validUserModel), Times.Once);
            Assert.Equal(_validUserModel.Id, result);
        }

        [Fact(DisplayName = "Deve retornar todos os usuários")]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            _mockUserRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(_userList);

            // Act
            var result = await _mockUserRepository.Object.GetAllAsync();

            // Assert
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            Assert.Equal(_userList, result);
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
            var result = await _mockUserRepository.Object.GetByIdAsync(userId);

            // Assert
            _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
            Assert.Equal(_validUserModel, result);
        }

        [Fact(DisplayName = "Deve atualizar os dados de um usuário")]
        public async Task PutAsync_ShouldUpdateUser_WhenUserModelIsValid()
        {
            // Arrange
            _mockUserRepository
                .Setup(repo => repo.PutAsync(_validUserModel))
                .Returns(Task.CompletedTask);

            // Act
            await _mockUserRepository.Object.PutAsync(_validUserModel);

            // Assert
            _mockUserRepository.Verify(repo => repo.PutAsync(_validUserModel), Times.Once);
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
            await _mockUserRepository.Object.DeleteAsync(userId);

            // Assert
            _mockUserRepository.Verify(repo => repo.DeleteAsync(userId), Times.Once);
        }
    }
}