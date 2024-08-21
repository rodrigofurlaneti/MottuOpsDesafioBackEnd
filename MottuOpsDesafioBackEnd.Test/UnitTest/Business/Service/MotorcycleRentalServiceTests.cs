using Moq;
using MottuOpsDesafioBackEnd.Business.Service;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.Business.Service
{
    public class MotorcycleRentalServiceTests
    {
        private readonly Mock<IMotorcycleRentalRepository> _mockMotorcycleRentalRepository;
        private readonly MotorcycleRentalService _motorcycleRentalService;
        private readonly MotorcycleRentalModel _validMotorcycleRentalModel;

        public MotorcycleRentalServiceTests()
        {
            _mockMotorcycleRentalRepository = new Mock<IMotorcycleRentalRepository>();
            _motorcycleRentalService = new MotorcycleRentalService(_mockMotorcycleRentalRepository.Object);

            // Criando um exemplo de MotorcycleRentalModel válido
            _validMotorcycleRentalModel = new MotorcycleRentalModel
            {
                Id = 1,
                CourierId = 1,
                MotorcycleId = 100,
                StartDate = new DateTime(2024, 8, 1),
                EndDate = new DateTime(2024, 8, 10),
                ExpectedEndDate = new DateTime(2024, 8, 10),
                DailyRate = 15.00m
            };
        }

        [Fact(DisplayName = "Deve inserir um novo aluguel de moto e retornar seu ID")]
        public async Task PostAsync_ShouldReturnMotorcycleRentalId_WhenMotorcycleRentalModelIsValid()
        {
            // Arrange
            _mockMotorcycleRentalRepository
                .Setup(repo => repo.PostAsync(_validMotorcycleRentalModel))
                .ReturnsAsync(_validMotorcycleRentalModel.Id);

            // Act
            var result = await _motorcycleRentalService.PostAsync(_validMotorcycleRentalModel);

            // Assert
            _mockMotorcycleRentalRepository.Verify(repo => repo.PostAsync(_validMotorcycleRentalModel), Times.Once);
            Assert.Equal(_validMotorcycleRentalModel.Id, result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao inserir um novo aluguel de moto")]
        public async Task PostAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockMotorcycleRentalRepository
                .Setup(repo => repo.PostAsync(It.IsAny<MotorcycleRentalModel>()))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _motorcycleRentalService.PostAsync(_validMotorcycleRentalModel));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve retornar aluguel de moto pelo ID do entregador")]
        public async Task GetByCourierIdAsync_ShouldReturnMotorcycleRentalModel_WhenCourierIdIsValid()
        {
            // Arrange
            var courierId = 1;
            _mockMotorcycleRentalRepository
                .Setup(repo => repo.GetByCourierIdAsync(courierId))
                .ReturnsAsync(_validMotorcycleRentalModel);

            // Act
            var result = await _motorcycleRentalService.GetByCourierIdAsync(courierId);

            // Assert
            _mockMotorcycleRentalRepository.Verify(repo => repo.GetByCourierIdAsync(courierId), Times.Once);
            Assert.Equal(_validMotorcycleRentalModel, result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao consultar aluguel de moto pelo ID do entregador")]
        public async Task GetByCourierIdAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            var courierId = 1;
            _mockMotorcycleRentalRepository
                .Setup(repo => repo.GetByCourierIdAsync(courierId))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _motorcycleRentalService.GetByCourierIdAsync(courierId));

            Assert.Equal($"Erro no repositório", exception.Message);
        }
    }
}