using Moq;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.Data.Interface
{
    public class IProfileRepositoryTests
    {
        private readonly Mock<IProfileRepository> _mockProfileRepository;
        private readonly UserProfileModel _validUserProfileModel;
        private readonly List<UserProfileModel> _userProfileList;

        public IProfileRepositoryTests()
        {
            _mockProfileRepository = new Mock<IProfileRepository>();

            _validUserProfileModel = new UserProfileModel
            {
                Id = 1,
                ProfileName = "Admin"
            };

            _userProfileList = new List<UserProfileModel> { _validUserProfileModel };
        }

        [Fact(DisplayName = "Deve inserir um novo perfil de usuário e retornar seu ID")]
        public async Task PostAsync_ShouldReturnUserProfileId_WhenUserProfileModelIsValid()
        {
            // Arrange
            _mockProfileRepository
                .Setup(repo => repo.PostAsync(_validUserProfileModel))
                .ReturnsAsync(_validUserProfileModel.Id);

            // Act
            var result = await _mockProfileRepository.Object.PostAsync(_validUserProfileModel);

            // Assert
            _mockProfileRepository.Verify(repo => repo.PostAsync(_validUserProfileModel), Times.Once);
            Assert.Equal(_validUserProfileModel.Id, result);
        }

        [Fact(DisplayName = "Deve retornar todos os perfis de usuário")]
        public async Task GetAllAsync_ShouldReturnAllUserProfiles()
        {
            // Arrange
            _mockProfileRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(_userProfileList);

            // Act
            var result = await _mockProfileRepository.Object.GetAllAsync();

            // Assert
            _mockProfileRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            Assert.Equal(_userProfileList, result);
        }

        [Fact(DisplayName = "Deve retornar um perfil de usuário pelo ID")]
        public async Task GetByIdAsync_ShouldReturnUserProfileModel_WhenIdIsValid()
        {
            // Arrange
            var userProfileId = 1;
            _mockProfileRepository
                .Setup(repo => repo.GetByIdAsync(userProfileId))
                .ReturnsAsync(_validUserProfileModel);

            // Act
            var result = await _mockProfileRepository.Object.GetByIdAsync(userProfileId);

            // Assert
            _mockProfileRepository.Verify(repo => repo.GetByIdAsync(userProfileId), Times.Once);
            Assert.Equal(_validUserProfileModel, result);
        }

        [Fact(DisplayName = "Deve atualizar os dados de um perfil de usuário")]
        public async Task PutAsync_ShouldUpdateUserProfile_WhenUserProfileModelIsValid()
        {
            // Arrange
            _mockProfileRepository
                .Setup(repo => repo.PutAsync(_validUserProfileModel))
                .Returns(Task.CompletedTask);

            // Act
            await _mockProfileRepository.Object.PutAsync(_validUserProfileModel);

            // Assert
            _mockProfileRepository.Verify(repo => repo.PutAsync(_validUserProfileModel), Times.Once);
        }

        [Fact(DisplayName = "Deve deletar um perfil de usuário pelo ID")]
        public async Task DeleteAsync_ShouldDeleteUserProfile_WhenIdIsValid()
        {
            // Arrange
            var userProfileId = 1;
            _mockProfileRepository
                .Setup(repo => repo.DeleteAsync(userProfileId))
                .Returns(Task.CompletedTask);

            // Act
            await _mockProfileRepository.Object.DeleteAsync(userProfileId);

            // Assert
            _mockProfileRepository.Verify(repo => repo.DeleteAsync(userProfileId), Times.Once);
        }
    }
}