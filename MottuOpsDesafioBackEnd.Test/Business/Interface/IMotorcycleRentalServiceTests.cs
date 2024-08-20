using Moq;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Business.Interface
{
    public class IMotorcycleRentalServiceTests
    {
        private readonly Mock<IMotorcycleRentalService> _mockMotorcycleRentalService;
        private readonly MotorcycleRentalModel _validMotorcycleRentalModel;

        public IMotorcycleRentalServiceTests()
        {
            _mockMotorcycleRentalService = new Mock<IMotorcycleRentalService>();

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

        [Fact(DisplayName = "Deve inserir uma nova locação de moto e retornar seu ID")]
        public async Task PostAsync_ShouldReturnMotorcycleRentalId_WhenMotorcycleRentalModelIsValid()
        {
            // Arrange
            _mockMotorcycleRentalService
                .Setup(service => service.PostAsync(_validMotorcycleRentalModel))
                .ReturnsAsync(_validMotorcycleRentalModel.Id);

            // Act
            var result = await _mockMotorcycleRentalService.Object.PostAsync(_validMotorcycleRentalModel);

            // Assert
            _mockMotorcycleRentalService.Verify(service => service.PostAsync(_validMotorcycleRentalModel), Times.Once);
            Assert.Equal(_validMotorcycleRentalModel.Id, result);
        }

        [Fact(DisplayName = "Deve retornar uma locação de moto pelo CourierId")]
        public async Task GetByCourierIdAsync_ShouldReturnMotorcycleRentalModel_WhenCourierIdIsValid()
        {
            // Arrange
            var courierId = 1;
            _mockMotorcycleRentalService
                .Setup(service => service.GetByCourierIdAsync(courierId))
                .ReturnsAsync(_validMotorcycleRentalModel);

            // Act
            var result = await _mockMotorcycleRentalService.Object.GetByCourierIdAsync(courierId);

            // Assert
            _mockMotorcycleRentalService.Verify(service => service.GetByCourierIdAsync(courierId), Times.Once);
            Assert.Equal(_validMotorcycleRentalModel, result);
        }
    }
}