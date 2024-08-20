using Moq;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Data.Interface
{
    public class ICourierRepositoryTests
    {
        private readonly Mock<ICourierRepository> _mockCourierRepository;
        private readonly CourierModel _validCourierModel;

        public ICourierRepositoryTests()
        {
            _mockCourierRepository = new Mock<ICourierRepository>();

            // Criando um exemplo de CourierModel válido
            _validCourierModel = new CourierModel
            {
                Id = 1,
                Name = "Entregador Exemplo",
                CNPJ = "12345678901234",
                CNHNumber = "12345678901",
                CNHType = "A",
                BirthDate = new DateTime(1980, 1, 1)
            };
        }

        [Fact(DisplayName = "Deve inserir um novo Courier e retornar seu ID")]
        public async Task PostAsync_ShouldReturnCourierId_WhenCourierModelIsValid()
        {
            // Arrange
            _mockCourierRepository
                .Setup(repo => repo.PostAsync(_validCourierModel))
                .ReturnsAsync(_validCourierModel.Id);

            // Act
            var result = await _mockCourierRepository.Object.PostAsync(_validCourierModel);

            // Assert
            _mockCourierRepository.Verify(repo => repo.PostAsync(_validCourierModel), Times.Once);
            Assert.Equal(_validCourierModel.Id, result);
        }

        [Fact(DisplayName = "Deve retornar um Courier pelo ID")]
        public async Task GetByIdAsync_ShouldReturnCourierModel_WhenIdIsValid()
        {
            // Arrange
            var courierId = 1;
            _mockCourierRepository
                .Setup(repo => repo.GetByIdAsync(courierId))
                .ReturnsAsync(_validCourierModel);

            // Act
            var result = await _mockCourierRepository.Object.GetByIdAsync(courierId);

            // Assert
            _mockCourierRepository.Verify(repo => repo.GetByIdAsync(courierId), Times.Once);
            Assert.Equal(_validCourierModel, result);
        }

        [Fact(DisplayName = "Deve atualizar a CNH de um Courier")]
        public async Task PutCnhAsync_ShouldUpdateCourierCnh_WhenCourierModelIsValid()
        {
            // Arrange
            _mockCourierRepository
                .Setup(repo => repo.PutCnhAsync(_validCourierModel))
                .Returns(Task.CompletedTask);

            // Act
            await _mockCourierRepository.Object.PutCnhAsync(_validCourierModel);

            // Assert
            _mockCourierRepository.Verify(repo => repo.PutCnhAsync(_validCourierModel), Times.Once);
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
            var result = await _mockCourierRepository.Object.GetByCnpjAsync(cnpj);

            // Assert
            _mockCourierRepository.Verify(repo => repo.GetByCnpjAsync(cnpj), Times.Once);
            Assert.True(result);
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
            var result = await _mockCourierRepository.Object.GetByCnhAsync(cnh);

            // Assert
            _mockCourierRepository.Verify(repo => repo.GetByCnhAsync(cnh), Times.Once);
            Assert.True(result);
        }
    }
}