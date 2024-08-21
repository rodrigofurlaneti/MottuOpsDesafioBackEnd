using Moq;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.Data.Interface
{
    public class IMotorcycleRepositoryTests
    {
        private readonly Mock<IMotorcycleRepository> _mockMotorcycleRepository;
        private readonly MotorcycleModel _validMotorcycleModel;
        private readonly List<MotorcycleModel> _motorcycleList;

        public IMotorcycleRepositoryTests()
        {
            _mockMotorcycleRepository = new Mock<IMotorcycleRepository>();

            // Criando um exemplo de MotorcycleModel válido
            _validMotorcycleModel = new MotorcycleModel
            {
                Id = 1,
                Year = 2024,
                Model = "Model X",
                LicensePlate = "ABC1234"
            };

            // Criando uma lista de exemplo
            _motorcycleList = new List<MotorcycleModel> { _validMotorcycleModel };
        }

        [Fact(DisplayName = "Deve inserir uma nova moto e retornar seu ID")]
        public async Task PostAsync_ShouldReturnMotorcycleId_WhenMotorcycleModelIsValid()
        {
            // Arrange
            _mockMotorcycleRepository
                .Setup(repo => repo.PostAsync(_validMotorcycleModel))
                .ReturnsAsync(_validMotorcycleModel.Id);

            // Act
            var result = await _mockMotorcycleRepository.Object.PostAsync(_validMotorcycleModel);

            // Assert
            _mockMotorcycleRepository.Verify(repo => repo.PostAsync(_validMotorcycleModel), Times.Once);
            Assert.Equal(_validMotorcycleModel.Id, result);
        }

        [Fact(DisplayName = "Deve retornar todas as motos")]
        public async Task GetAllAsync_ShouldReturnAllMotorcycles()
        {
            // Arrange
            _mockMotorcycleRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(_motorcycleList);

            // Act
            var result = await _mockMotorcycleRepository.Object.GetAllAsync();

            // Assert
            _mockMotorcycleRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            Assert.Equal(_motorcycleList, result);
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
            var result = await _mockMotorcycleRepository.Object.GetByIdAsync(motorcycleId);

            // Assert
            _mockMotorcycleRepository.Verify(repo => repo.GetByIdAsync(motorcycleId), Times.Once);
            Assert.Equal(_validMotorcycleModel, result);
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
            var result = await _mockMotorcycleRepository.Object.GetByLicensePlateAsync(licensePlate);

            // Assert
            _mockMotorcycleRepository.Verify(repo => repo.GetByLicensePlateAsync(licensePlate), Times.Once);
            Assert.True(result);
        }

        [Fact(DisplayName = "Deve atualizar os dados de uma moto")]
        public async Task PutAsync_ShouldUpdateMotorcycle_WhenMotorcycleModelIsValid()
        {
            // Arrange
            _mockMotorcycleRepository
                .Setup(repo => repo.PutAsync(_validMotorcycleModel))
                .Returns(Task.CompletedTask);

            // Act
            await _mockMotorcycleRepository.Object.PutAsync(_validMotorcycleModel);

            // Assert
            _mockMotorcycleRepository.Verify(repo => repo.PutAsync(_validMotorcycleModel), Times.Once);
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
            await _mockMotorcycleRepository.Object.DeleteAsync(motorcycleId);

            // Assert
            _mockMotorcycleRepository.Verify(repo => repo.DeleteAsync(motorcycleId), Times.Once);
        }
    }
}