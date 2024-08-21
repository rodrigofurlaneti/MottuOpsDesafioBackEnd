using Moq;
using MottuOpsDesafioBackEnd.Business.Service;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.Business.Service
{
    public class CourierServiceTests
    {
        private readonly Mock<ICourierRepository> _mockCourierRepository;
        private readonly CourierService _courierService;
        private readonly CourierModel _validCourierModel;

        public CourierServiceTests()
        {
            _mockCourierRepository = new Mock<ICourierRepository>();
            _courierService = new CourierService(_mockCourierRepository.Object);

            _validCourierModel = new CourierModel
            {
                Id = 1,
                Identifier = "ENT123",
                Name = "Entregador Exemplo",
                CNPJ = "12345678901234",
                CNHNumber = "12345678901",
                CNHType = "A",
                BirthDate = new DateTime(1980, 1, 1)
            };
        }

        [Fact(DisplayName = "Deve inserir um novo entregador e retornar seu ID")]
        public async Task PostAsync_ShouldReturnCourierId_WhenCourierModelIsValid()
        {
            // Arrange
            _mockCourierRepository
                .Setup(repo => repo.PostAsync(_validCourierModel))
                .ReturnsAsync(_validCourierModel.Id);

            // Act
            var result = await _courierService.PostAsync(_validCourierModel);

            // Assert
            _mockCourierRepository.Verify(repo => repo.PostAsync(_validCourierModel), Times.Once);
            Assert.Equal(_validCourierModel.Id, result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao inserir um novo entregador")]
        public async Task PostAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockCourierRepository
                .Setup(repo => repo.PostAsync(It.IsAny<CourierModel>()))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _courierService.PostAsync(_validCourierModel));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve retornar verdadeiro se o CNPJ já estiver cadastrado")]
        public async Task GetByCnpjAsync_ShouldReturnTrue_WhenCnpjExists()
        {
            // Arrange
            var cnpj = "12345678901234";
            _mockCourierRepository
                .Setup(repo => repo.GetByCnpjAsync(cnpj))
                .ReturnsAsync(true);

            // Act
            var result = await _courierService.GetByCnpjAsync(cnpj);

            // Assert
            _mockCourierRepository.Verify(repo => repo.GetByCnpjAsync(cnpj), Times.Once);
            Assert.True(result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao consultar o CNPJ")]
        public async Task GetByCnpjAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            var cnpj = "12345678901234";
            _mockCourierRepository
                .Setup(repo => repo.GetByCnpjAsync(cnpj))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _courierService.GetByCnpjAsync(cnpj));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve retornar verdadeiro se a CNH já estiver cadastrada")]
        public async Task GetByCnhAsync_ShouldReturnTrue_WhenCnhExists()
        {
            // Arrange
            var cnh = "12345678901";
            _mockCourierRepository
                .Setup(repo => repo.GetByCnhAsync(cnh))
                .ReturnsAsync(true);

            // Act
            var result = await _courierService.GetByCnhAsync(cnh);

            // Assert
            _mockCourierRepository.Verify(repo => repo.GetByCnhAsync(cnh), Times.Once);
            Assert.True(result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao consultar a CNH")]
        public async Task GetByCnhAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            var cnh = "12345678901";
            _mockCourierRepository
                .Setup(repo => repo.GetByCnhAsync(cnh))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _courierService.GetByCnhAsync(cnh));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve retornar um entregador pelo ID")]
        public async Task GetByIdAsync_ShouldReturnCourierModel_WhenIdIsValid()
        {
            // Arrange
            var courierId = 1;
            _mockCourierRepository
                .Setup(repo => repo.GetByIdAsync(courierId))
                .ReturnsAsync(_validCourierModel);

            // Act
            var result = await _courierService.GetByIdAsync(courierId);

            // Assert
            _mockCourierRepository.Verify(repo => repo.GetByIdAsync(courierId), Times.Once);
            Assert.Equal(_validCourierModel, result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao selecionar um entregador")]
        public async Task GetByIdAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            var courierId = 1;
            _mockCourierRepository
                .Setup(repo => repo.GetByIdAsync(courierId))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _courierService.GetByIdAsync(courierId));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve atualizar a CNH de um entregador")]
        public async Task PutCnhAsync_ShouldUpdateCourierCnh_WhenCourierModelIsValid()
        {
            // Arrange
            _mockCourierRepository
                .Setup(repo => repo.PutCnhAsync(_validCourierModel))
                .Returns(Task.CompletedTask);

            // Act
            await _courierService.PutCnhAsync(_validCourierModel);

            // Assert
            _mockCourierRepository.Verify(repo => repo.PutCnhAsync(_validCourierModel), Times.Once);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao atualizar a CNH de um entregador")]
        public async Task PutCnhAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockCourierRepository
                .Setup(repo => repo.PutCnhAsync(It.IsAny<CourierModel>()))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _courierService.PutCnhAsync(_validCourierModel));

            Assert.Equal("Erro no repositório", exception.Message);
        }
    }
}