using Moq;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.Business.Interface
{
    public class IPlanRentalServiceTests
    {
        private readonly Mock<IPlanRentalService> _mockPlanRentalService;
        private readonly PlanRentalModel _validPlanRentalModel;
        private readonly List<PlanRentalModel> _planRentalList;

        public IPlanRentalServiceTests()
        {
            _mockPlanRentalService = new Mock<IPlanRentalService>();

            _validPlanRentalModel = new PlanRentalModel
            {
                Id = 1,
                Days = "7",
                Identifier = "A",
                RegistrationDate = DateTime.Now,
                TerminationFine = "10 %",
                Value = "10 %"
            };

            _planRentalList = new List<PlanRentalModel> { _validPlanRentalModel };
        }

        [Fact(DisplayName = "Deve inserir um novo plano de locação e retornar seu ID")]
        public async Task PostAsync_ShouldReturnPlanRentalId_WhenPlanRentalModelIsValid()
        {
            // Arrange
            _mockPlanRentalService
                .Setup(service => service.PostAsync(_validPlanRentalModel))
                .ReturnsAsync(_validPlanRentalModel.Id);

            // Act
            var result = await _mockPlanRentalService.Object.PostAsync(_validPlanRentalModel);

            // Assert
            _mockPlanRentalService.Verify(service => service.PostAsync(_validPlanRentalModel), Times.Once);
            Assert.Equal(_validPlanRentalModel.Id, result);
        }

        [Fact(DisplayName = "Deve retornar todos os planos de locação")]
        public async Task GetAllAsync_ShouldReturnAllPlanRentals()
        {
            // Arrange
            _mockPlanRentalService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(_planRentalList);

            // Act
            var result = await _mockPlanRentalService.Object.GetAllAsync();

            // Assert
            _mockPlanRentalService.Verify(service => service.GetAllAsync(), Times.Once);
            Assert.Equal(_planRentalList, result);
        }

        [Fact(DisplayName = "Deve retornar um plano de locação pelo ID")]
        public async Task GetByIdAsync_ShouldReturnPlanRentalModel_WhenIdIsValid()
        {
            // Arrange
            var planRentalId = 1;
            _mockPlanRentalService
                .Setup(service => service.GetByIdAsync(planRentalId))
                .ReturnsAsync(_validPlanRentalModel);

            // Act
            var result = await _mockPlanRentalService.Object.GetByIdAsync(planRentalId);

            // Assert
            _mockPlanRentalService.Verify(service => service.GetByIdAsync(planRentalId), Times.Once);
            Assert.Equal(_validPlanRentalModel, result);
        }

        [Fact(DisplayName = "Deve atualizar os dados de um plano de locação")]
        public async Task PutAsync_ShouldUpdatePlanRental_WhenPlanRentalModelIsValid()
        {
            // Arrange
            _mockPlanRentalService
                .Setup(service => service.PutAsync(_validPlanRentalModel))
                .Returns(Task.CompletedTask);

            // Act
            await _mockPlanRentalService.Object.PutAsync(_validPlanRentalModel);

            // Assert
            _mockPlanRentalService.Verify(service => service.PutAsync(_validPlanRentalModel), Times.Once);
        }

        [Fact(DisplayName = "Deve deletar um plano de locação pelo ID")]
        public async Task DeleteAsync_ShouldDeletePlanRental_WhenIdIsValid()
        {
            // Arrange
            var planRentalId = 1;
            _mockPlanRentalService
                .Setup(service => service.DeleteAsync(planRentalId))
                .Returns(Task.CompletedTask);

            // Act
            await _mockPlanRentalService.Object.DeleteAsync(planRentalId);

            // Assert
            _mockPlanRentalService.Verify(service => service.DeleteAsync(planRentalId), Times.Once);
        }
    }
}