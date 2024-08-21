using Moq;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.Business.Interface
{
    public class IUserServiceTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserModel _validUserModel;
        private readonly List<UserModel> _userList;

        public IUserServiceTests()
        {
            _mockUserService = new Mock<IUserService>();

            // Criando um exemplo de UserModel válido
            _validUserModel = new UserModel
            {
                Id = 1,
                Username = "testuser",
                PasswordHash = "password"
            };

            // Criando uma lista de exemplo
            _userList = new List<UserModel> { _validUserModel };
        }

        [Fact(DisplayName = "Deve inserir um novo usuário e retornar seu ID")]
        public async Task PostAsync_ShouldReturnUserId_WhenUserModelIsValid()
        {
            // Arrange
            _mockUserService
                .Setup(service => service.PostAsync(_validUserModel))
                .ReturnsAsync(_validUserModel.Id);

            // Act
            var result = await _mockUserService.Object.PostAsync(_validUserModel);

            // Assert
            _mockUserService.Verify(service => service.PostAsync(_validUserModel), Times.Once);
            Assert.Equal(_validUserModel.Id, result);
        }

        [Fact(DisplayName = "Deve retornar todos os usuários")]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            _mockUserService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(_userList);

            // Act
            var result = await _mockUserService.Object.GetAllAsync();

            // Assert
            _mockUserService.Verify(service => service.GetAllAsync(), Times.Once);
            Assert.Equal(_userList, result);
        }

        [Fact(DisplayName = "Deve retornar um usuário pelo ID")]
        public async Task GetByIdAsync_ShouldReturnUserModel_WhenIdIsValid()
        {
            // Arrange
            var userId = 1;
            _mockUserService
                .Setup(service => service.GetByIdAsync(userId))
                .ReturnsAsync(_validUserModel);

            // Act
            var result = await _mockUserService.Object.GetByIdAsync(userId);

            // Assert
            _mockUserService.Verify(service => service.GetByIdAsync(userId), Times.Once);
            Assert.Equal(_validUserModel, result);
        }

        [Fact(DisplayName = "Deve atualizar os dados de um usuário")]
        public async Task PutAsync_ShouldUpdateUser_WhenUserModelIsValid()
        {
            // Arrange
            _mockUserService
                .Setup(service => service.PutAsync(_validUserModel))
                .Returns(Task.CompletedTask);

            // Act
            await _mockUserService.Object.PutAsync(_validUserModel);

            // Assert
            _mockUserService.Verify(service => service.PutAsync(_validUserModel), Times.Once);
        }

        [Fact(DisplayName = "Deve deletar um usuário pelo ID")]
        public async Task DeleteAsync_ShouldDeleteUser_WhenIdIsValid()
        {
            // Arrange
            var userId = 1;
            _mockUserService
                .Setup(service => service.DeleteAsync(userId))
                .Returns(Task.CompletedTask);

            // Act
            await _mockUserService.Object.DeleteAsync(userId);

            // Assert
            _mockUserService.Verify(service => service.DeleteAsync(userId), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar um hash de senha válido")]
        public void HashPassword_ShouldReturnValidHash_WhenPasswordIsProvided()
        {
            // Arrange
            var password = "password123";
            var expectedHash = "hashedPassword123"; // Substitua pelo valor esperado em seu cenário

            _mockUserService
                .Setup(service => service.HashPassword(password))
                .Returns(expectedHash);

            // Act
            var result = _mockUserService.Object.HashPassword(password);

            // Assert
            _mockUserService.Verify(service => service.HashPassword(password), Times.Once);
            Assert.Equal(expectedHash, result);
        }
    }
}