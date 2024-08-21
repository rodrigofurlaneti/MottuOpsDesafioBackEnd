using Moq;
using MottuOpsDesafioBackEnd.Business.Service;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.Business.Service
{
    public class MotorcycleServiceTests
    {
        private readonly Mock<IMotorcycleRepository> _mockMotorcycleRepository;
        private readonly MotorcycleService _motorcycleService;
        private readonly MotorcycleModel _validMotorcycleModel;

        public MotorcycleServiceTests()
        {
            _mockMotorcycleRepository = new Mock<IMotorcycleRepository>();
            _motorcycleService = new MotorcycleService(_mockMotorcycleRepository.Object);

            _validMotorcycleModel = new MotorcycleModel
            {
                Id = 1,
                Identifier = "MOT123",
                Year = 2024,
                Model = "Model X",
                LicensePlate = "ABC1234"
            };
        }

        [Fact(DisplayName = "Deve inserir uma nova moto e retornar seu ID")]
        public async Task PostAsync_ShouldReturnMotorcycleId_WhenMotorcycleModelIsValid()
        {
            // Arrange
            _mockMotorcycleRepository
                .Setup(repo => repo.PostAsync(_validMotorcycleModel))
                .ReturnsAsync(_validMotorcycleModel.Id);

            // Act
            var result = await _motorcycleService.PostAsync(_validMotorcycleModel);

            // Assert
            _mockMotorcycleRepository.Verify(repo => repo.PostAsync(_validMotorcycleModel), Times.Once);
            Assert.Equal(_validMotorcycleModel.Id, result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao inserir uma nova moto")]
        public async Task PostAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockMotorcycleRepository
                .Setup(repo => repo.PostAsync(It.IsAny<MotorcycleModel>()))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _motorcycleService.PostAsync(_validMotorcycleModel));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve retornar todas as motos")]
        public async Task GetAllAsync_ShouldReturnAllMotorcycles()
        {
            // Arrange
            var motorcycleList = new List<MotorcycleModel> { _validMotorcycleModel };
            _mockMotorcycleRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(motorcycleList);

            // Act
            var result = await _motorcycleService.GetAllAsync();

            // Assert
            _mockMotorcycleRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            Assert.Equal(motorcycleList, result);
        }

        [Fact(DisplayName = "Deve retornar uma moto pelo ID")]
        public async Task GetByIdAsync_ShouldReturnMotorcycleModel_WhenIdIsValid()
        {
            // Arrange
            var motorcycleId = 1;
            _mockMotorcycleRepository
                .Setup(repo => repo.GetByIdAsync(motorcycleId))
                .ReturnsAsync(_validMotorcycleModel);

            // Act
            var result = await _motorcycleService.GetByIdAsync(motorcycleId);

            // Assert
            _mockMotorcycleRepository.Verify(repo => repo.GetByIdAsync(motorcycleId), Times.Once);
            Assert.Equal(_validMotorcycleModel, result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao selecionar uma moto pelo ID")]
        public async Task GetByIdAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            var motorcycleId = 1;
            _mockMotorcycleRepository
                .Setup(repo => repo.GetByIdAsync(motorcycleId))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _motorcycleService.GetByIdAsync(motorcycleId));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve retornar verdadeiro se a placa da moto já estiver cadastrada")]
        public async Task GetByLicensePlateAsync_ShouldReturnTrue_WhenLicensePlateExists()
        {
            // Arrange
            var licensePlate = "ABC1234";
            _mockMotorcycleRepository
                .Setup(repo => repo.GetByLicensePlateAsync(licensePlate))
                .ReturnsAsync(true);

            // Act
            var result = await _motorcycleService.GetByLicensePlateAsync(licensePlate);

            // Assert
            _mockMotorcycleRepository.Verify(repo => repo.GetByLicensePlateAsync(licensePlate), Times.Once);
            Assert.True(result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao consultar a placa da moto")]
        public async Task GetByLicensePlateAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            var licensePlate = "ABC1234";
            _mockMotorcycleRepository
                .Setup(repo => repo.GetByLicensePlateAsync(licensePlate))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _motorcycleService.GetByLicensePlateAsync(licensePlate));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve atualizar uma moto")]
        public async Task PutAsync_ShouldUpdateMotorcycle_WhenMotorcycleModelIsValid()
        {
            // Arrange
            _mockMotorcycleRepository
                .Setup(repo => repo.PutAsync(_validMotorcycleModel))
                .Returns(Task.CompletedTask);

            // Act
            await _motorcycleService.PutAsync(_validMotorcycleModel);

            // Assert
            _mockMotorcycleRepository.Verify(repo => repo.PutAsync(_validMotorcycleModel), Times.Once);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao atualizar uma moto")]
        public async Task PutAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockMotorcycleRepository
                .Setup(repo => repo.PutAsync(It.IsAny<MotorcycleModel>()))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _motorcycleService.PutAsync(_validMotorcycleModel));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve deletar uma moto pelo ID")]
        public async Task DeleteAsync_ShouldDeleteMotorcycle_WhenIdIsValid()
        {
            // Arrange
            var motorcycleId = 1;
            _mockMotorcycleRepository
                .Setup(repo => repo.DeleteAsync(motorcycleId))
                .Returns(Task.CompletedTask);

            // Act
            await _motorcycleService.DeleteAsync(motorcycleId);

            // Assert
            _mockMotorcycleRepository.Verify(repo => repo.DeleteAsync(motorcycleId), Times.Once);
        }
    }
}