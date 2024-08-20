using Moq;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Business.Interface
{
    public class IProfileServiceTests
    {
        private readonly Mock<IProfileService> _mockProfileService;
        private readonly UserProfileModel _validUserProfileModel;
        private readonly List<UserProfileModel> _userProfileList;

        public IProfileServiceTests()
        {
            _mockProfileService = new Mock<IProfileService>();

            // Criando um exemplo de UserProfileModel válido
            _validUserProfileModel = new UserProfileModel
            {
                Id = 1,
                ProfileName = "Administrador"
            };

            // Criando uma lista de exemplo
            _userProfileList = new List<UserProfileModel> { _validUserProfileModel };
        }

        [Fact(DisplayName = "Deve inserir um novo perfil de usuário e retornar seu ID")]
        public async Task PostAsync_ShouldReturnUserProfileId_WhenUserProfileModelIsValid()
        {
            // Arrange
            _mockProfileService
                .Setup(service => service.PostAsync(_validUserProfileModel))
                .ReturnsAsync(_validUserProfileModel.Id);

            // Act
            var result = await _mockProfileService.Object.PostAsync(_validUserProfileModel);

            // Assert
            _mockProfileService.Verify(service => service.PostAsync(_validUserProfileModel), Times.Once);
            Assert.Equal(_validUserProfileModel.Id, result);
        }

        [Fact(DisplayName = "Deve retornar todos os perfis de usuário")]
        public async Task GetAllAsync_ShouldReturnAllUserProfiles()
        {
            // Arrange
            _mockProfileService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(_userProfileList);

            // Act
            var result = await _mockProfileService.Object.GetAllAsync();

            // Assert
            _mockProfileService.Verify(service => service.GetAllAsync(), Times.Once);
            Assert.Equal(_userProfileList, result);
        }

        [Fact(DisplayName = "Deve retornar um perfil de usuário pelo ID")]
        public async Task GetByIdAsync_ShouldReturnUserProfileModel_WhenIdIsValid()
        {
            // Arrange
            var userProfileId = 1;
            _mockProfileService
                .Setup(service => service.GetByIdAsync(userProfileId))
                .ReturnsAsync(_validUserProfileModel);

            // Act
            var result = await _mockProfileService.Object.GetByIdAsync(userProfileId);

            // Assert
            _mockProfileService.Verify(service => service.GetByIdAsync(userProfileId), Times.Once);
            Assert.Equal(_validUserProfileModel, result);
        }

        [Fact(DisplayName = "Deve atualizar os dados de um perfil de usuário")]
        public async Task PutAsync_ShouldUpdateUserProfile_WhenUserProfileModelIsValid()
        {
            // Arrange
            _mockProfileService
                .Setup(service => service.PutAsync(_validUserProfileModel))
                .Returns(Task.CompletedTask);

            // Act
            await _mockProfileService.Object.PutAsync(_validUserProfileModel);

            // Assert
            _mockProfileService.Verify(service => service.PutAsync(_validUserProfileModel), Times.Once);
        }

        [Fact(DisplayName = "Deve deletar um perfil de usuário pelo ID")]
        public async Task DeleteAsync_ShouldDeleteUserProfile_WhenIdIsValid()
        {
            // Arrange
            var userProfileId = 1;
            _mockProfileService
                .Setup(service => service.DeleteAsync(userProfileId))
                .Returns(Task.CompletedTask);

            // Act
            await _mockProfileService.Object.DeleteAsync(userProfileId);

            // Assert
            _mockProfileService.Verify(service => service.DeleteAsync(userProfileId), Times.Once);
        }
    }
}