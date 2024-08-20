using Moq;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Business.Interface
{
    public class IMotorcycleServiceTests
    {
        private readonly Mock<IMotorcycleService> _mockMotorcycleService;
        private readonly MotorcycleModel _validMotorcycleModel;
        private readonly List<MotorcycleModel> _motorcycleList;

        public IMotorcycleServiceTests()
        {
            _mockMotorcycleService = new Mock<IMotorcycleService>();

            _validMotorcycleModel = new MotorcycleModel
            {
                Id = 1,
                Year = 2024,
                Model = "Model X",
                LicensePlate = "ABC1234"
            };

            _motorcycleList = new List<MotorcycleModel> { _validMotorcycleModel };
        }

        [Fact(DisplayName = "Deve inserir uma nova moto e retornar seu ID")]
        public async Task PostAsync_ShouldReturnMotorcycleId_WhenMotorcycleModelIsValid()
        {
            // Arrange
            _mockMotorcycleService
                .Setup(service => service.PostAsync(_validMotorcycleModel))
                .ReturnsAsync(_validMotorcycleModel.Id);

            // Act
            var result = await _mockMotorcycleService.Object.PostAsync(_validMotorcycleModel);

            // Assert
            _mockMotorcycleService.Verify(service => service.PostAsync(_validMotorcycleModel), Times.Once);
            Assert.Equal(_validMotorcycleModel.Id, result);
        }

        [Fact(DisplayName = "Deve retornar todas as motos")]
        public async Task GetAllAsync_ShouldReturnAllMotorcycles()
        {
            // Arrange
            _mockMotorcycleService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(_motorcycleList);

            // Act
            var result = await _mockMotorcycleService.Object.GetAllAsync();

            // Assert
            _mockMotorcycleService.Verify(service => service.GetAllAsync(), Times.Once);
            Assert.Equal(_motorcycleList, result);
        }

        [Fact(DisplayName = "Deve retornar uma moto pelo ID")]
        public async Task GetByIdAsync_ShouldReturnMotorcycleModel_WhenIdIsValid()
        {
            // Arrange
            var motorcycleId = 1;
            _mockMotorcycleService
                .Setup(service => service.GetByIdAsync(motorcycleId))
                .ReturnsAsync(_validMotorcycleModel);

            // Act
            var result = await _mockMotorcycleService.Object.GetByIdAsync(motorcycleId);

            // Assert
            _mockMotorcycleService.Verify(service => service.GetByIdAsync(motorcycleId), Times.Once);
            Assert.Equal(_validMotorcycleModel, result);
        }

        [Fact(DisplayName = "Deve retornar verdadeiro se a placa da moto já estiver cadastrada")]
        public async Task GetByLicensePlateAsync_ShouldReturnTrue_WhenLicensePlateExists()
        {
            // Arrange
            var licensePlate = "ABC1234";
            _mockMotorcycleService
                .Setup(service => service.GetByLicensePlateAsync(licensePlate))
                .ReturnsAsync(true);

            // Act
            var result = await _mockMotorcycleService.Object.GetByLicensePlateAsync(licensePlate);

            // Assert
            _mockMotorcycleService.Verify(service => service.GetByLicensePlateAsync(licensePlate), Times.Once);
            Assert.True(result);
        }

        [Fact(DisplayName = "Deve atualizar os dados de uma moto")]
        public async Task PutAsync_ShouldUpdateMotorcycle_WhenMotorcycleModelIsValid()
        {
            // Arrange
            _mockMotorcycleService
                .Setup(service => service.PutAsync(_validMotorcycleModel))
                .Returns(Task.CompletedTask);

            // Act
            await _mockMotorcycleService.Object.PutAsync(_validMotorcycleModel);

            // Assert
            _mockMotorcycleService.Verify(service => service.PutAsync(_validMotorcycleModel), Times.Once);
        }

        [Fact(DisplayName = "Deve deletar uma moto pelo ID")]
        public async Task DeleteAsync_ShouldDeleteMotorcycle_WhenIdIsValid()
        {
            // Arrange
            var motorcycleId = 1;
            _mockMotorcycleService
                .Setup(service => service.DeleteAsync(motorcycleId))
                .Returns(Task.CompletedTask);

            // Act
            await _mockMotorcycleService.Object.DeleteAsync(motorcycleId);

            // Assert
            _mockMotorcycleService.Verify(service => service.DeleteAsync(motorcycleId), Times.Once);
        }
    }
}