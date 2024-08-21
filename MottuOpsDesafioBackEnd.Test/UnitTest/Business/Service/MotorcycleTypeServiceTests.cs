using Moq;
using MottuOpsDesafioBackEnd.Business.Service;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.Business.Service
{
    public class MotorcycleTypeServiceTests
    {
        private readonly Mock<IMotorcycleTypeRepository> _mockMotorcycleTypeRepository;
        private readonly MotorcycleTypeService _motorcycleTypeService;
        private readonly MotorcycleTypeModel _validMotorcycleTypeModel;

        public MotorcycleTypeServiceTests()
        {
            _mockMotorcycleTypeRepository = new Mock<IMotorcycleTypeRepository>();
            _motorcycleTypeService = new MotorcycleTypeService(_mockMotorcycleTypeRepository.Object);

            _validMotorcycleTypeModel = new MotorcycleTypeModel
            {
                Id = 1,
                TypeName = "Sport"
            };
        }

        [Fact(DisplayName = "Deve inserir um novo modelo de moto e retornar seu ID")]
        public async Task PostAsync_ShouldReturnMotorcycleTypeId_WhenMotorcycleTypeModelIsValid()
        {
            // Arrange
            _mockMotorcycleTypeRepository
                .Setup(repo => repo.PostAsync(_validMotorcycleTypeModel))
                .ReturnsAsync(_validMotorcycleTypeModel.Id);

            // Act
            var result = await _motorcycleTypeService.PostAsync(_validMotorcycleTypeModel);

            // Assert
            _mockMotorcycleTypeRepository.Verify(repo => repo.PostAsync(_validMotorcycleTypeModel), Times.Once);
            Assert.Equal(_validMotorcycleTypeModel.Id, result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao inserir um novo modelo de moto")]
        public async Task PostAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockMotorcycleTypeRepository
                .Setup(repo => repo.PostAsync(It.IsAny<MotorcycleTypeModel>()))
                .ThrowsAsync(new Exception("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _motorcycleTypeService.PostAsync(_validMotorcycleTypeModel));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve retornar todos os modelos de motos")]
        public async Task GetAllAsync_ShouldReturnAllMotorcycleTypes()
        {
            // Arrange
            var motorcycleTypeList = new List<MotorcycleTypeModel> { _validMotorcycleTypeModel };
            _mockMotorcycleTypeRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(motorcycleTypeList);

            // Act
            var result = await _motorcycleTypeService.GetAllAsync();

            // Assert
            _mockMotorcycleTypeRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            Assert.Equal(motorcycleTypeList, result);
        }

        [Fact(DisplayName = "Deve retornar um modelo de moto pelo ID")]
        public async Task GetByIdAsync_ShouldReturnMotorcycleTypeModel_WhenIdIsValid()
        {
            // Arrange
            var motorcycleTypeId = 1;
            _mockMotorcycleTypeRepository
                .Setup(repo => repo.GetByIdAsync(motorcycleTypeId))
                .ReturnsAsync(_validMotorcycleTypeModel);

            // Act
            var result = await _motorcycleTypeService.GetByIdAsync(motorcycleTypeId);

            // Assert
            _mockMotorcycleTypeRepository.Verify(repo => repo.GetByIdAsync(motorcycleTypeId), Times.Once);
            Assert.Equal(_validMotorcycleTypeModel, result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao selecionar um modelo de moto pelo ID")]
        public async Task GetByIdAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            var motorcycleTypeId = 1;
            _mockMotorcycleTypeRepository
                .Setup(repo => repo.GetByIdAsync(motorcycleTypeId))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _motorcycleTypeService.GetByIdAsync(motorcycleTypeId));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve atualizar um modelo de moto")]
        public async Task PutAsync_ShouldUpdateMotorcycleType_WhenMotorcycleTypeModelIsValid()
        {
            // Arrange
            _mockMotorcycleTypeRepository
                .Setup(repo => repo.PutAsync(_validMotorcycleTypeModel))
                .Returns(Task.CompletedTask);

            // Act
            await _motorcycleTypeService.PutAsync(_validMotorcycleTypeModel);

            // Assert
            _mockMotorcycleTypeRepository.Verify(repo => repo.PutAsync(_validMotorcycleTypeModel), Times.Once);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao atualizar um modelo de moto")]
        public async Task PutAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockMotorcycleTypeRepository
                .Setup(repo => repo.PutAsync(It.IsAny<MotorcycleTypeModel>()))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _motorcycleTypeService.PutAsync(_validMotorcycleTypeModel));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve deletar um modelo de moto pelo ID")]
        public async Task DeleteAsync_ShouldDeleteMotorcycleType_WhenIdIsValid()
        {
            // Arrange
            var motorcycleTypeId = 1;
            _mockMotorcycleTypeRepository
                .Setup(repo => repo.DeleteAsync(motorcycleTypeId))
                .Returns(Task.CompletedTask);

            // Act
            await _motorcycleTypeService.DeleteAsync(motorcycleTypeId);

            // Assert
            _mockMotorcycleTypeRepository.Verify(repo => repo.DeleteAsync(motorcycleTypeId), Times.Once);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao deletar um modelo de moto")]
        public async Task DeleteAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            var motorcycleTypeId = 1;
            _mockMotorcycleTypeRepository
                .Setup(repo => repo.DeleteAsync(motorcycleTypeId))
                .ThrowsAsync(new Exception("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _motorcycleTypeService.DeleteAsync(motorcycleTypeId));

            Assert.Equal($"Erro ao deletar o modelo moto com ID {motorcycleTypeId}", exception.Message);
        }
    }
}