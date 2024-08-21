using Moq;
using MottuOpsDesafioBackEnd.Business.Service;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.Business.Service
{
    public class ProfileServiceTests
    {
        private readonly Mock<IProfileRepository> _mockProfileRepository;
        private readonly ProfileService _profileService;
        private readonly UserProfileModel _validUserProfileModel;

        public ProfileServiceTests()
        {
            _mockProfileRepository = new Mock<IProfileRepository>();
            _profileService = new ProfileService(_mockProfileRepository.Object);

            _validUserProfileModel = new UserProfileModel
            {
                Id = 1,
                ProfileName = "Administrador"
            };
        }

        [Fact(DisplayName = "Deve inserir um novo perfil de usuário e retornar seu ID")]
        public async Task PostAsync_ShouldReturnUserProfileId_WhenUserProfileModelIsValid()
        {
            // Arrange
            _mockProfileRepository
                .Setup(repo => repo.PostAsync(_validUserProfileModel))
                .ReturnsAsync(_validUserProfileModel.Id);

            // Act
            var result = await _profileService.PostAsync(_validUserProfileModel);

            // Assert
            _mockProfileRepository.Verify(repo => repo.PostAsync(_validUserProfileModel), Times.Once);
            Assert.Equal(_validUserProfileModel.Id, result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao inserir um novo perfil de usuário")]
        public async Task PostAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockProfileRepository
                .Setup(repo => repo.PostAsync(It.IsAny<UserProfileModel>()))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _profileService.PostAsync(_validUserProfileModel));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve retornar todos os perfis de usuário")]
        public async Task GetAllAsync_ShouldReturnAllUserProfiles()
        {
            // Arrange
            var userProfileList = new List<UserProfileModel> { _validUserProfileModel };
            _mockProfileRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(userProfileList);

            // Act
            var result = await _profileService.GetAllAsync();

            // Assert
            _mockProfileRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            Assert.Equal(userProfileList, result);
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
            var result = await _profileService.GetByIdAsync(userProfileId);

            // Assert
            _mockProfileRepository.Verify(repo => repo.GetByIdAsync(userProfileId), Times.Once);
            Assert.Equal(_validUserProfileModel, result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao selecionar um perfil de usuário pelo ID")]
        public async Task GetByIdAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            var userProfileId = 1;
            _mockProfileRepository
                .Setup(repo => repo.GetByIdAsync(userProfileId))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _profileService.GetByIdAsync(userProfileId));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve atualizar um perfil de usuário")]
        public async Task PutAsync_ShouldUpdateUserProfile_WhenUserProfileModelIsValid()
        {
            // Arrange
            _mockProfileRepository
                .Setup(repo => repo.PutAsync(_validUserProfileModel))
                .Returns(Task.CompletedTask);

            // Act
            await _profileService.PutAsync(_validUserProfileModel);

            // Assert
            _mockProfileRepository.Verify(repo => repo.PutAsync(_validUserProfileModel), Times.Once);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao atualizar um perfil de usuário")]
        public async Task PutAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockProfileRepository
                .Setup(repo => repo.PutAsync(It.IsAny<UserProfileModel>()))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _profileService.PutAsync(_validUserProfileModel));

            Assert.Equal("Erro no repositório", exception.Message);
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
            await _profileService.DeleteAsync(userProfileId);

            // Assert
            _mockProfileRepository.Verify(repo => repo.DeleteAsync(userProfileId), Times.Once);
        }

    }
}