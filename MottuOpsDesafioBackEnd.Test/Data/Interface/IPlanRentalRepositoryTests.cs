using Moq;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Data.Interface
{
    public class IPlanRentalRepositoryTests
    {
        private readonly Mock<IPlanRentalRepository> _mockPlanRentalRepository;
        private readonly PlanRentalModel _validPlanRentalModel;
        private readonly List<PlanRentalModel> _planRentalList;

        public IPlanRentalRepositoryTests()
        {
            _mockPlanRentalRepository = new Mock<IPlanRentalRepository>();

            // Criando um exemplo de PlanRentalModel válido
            _validPlanRentalModel = new PlanRentalModel
            {
                Id = 1,
                Days = "7",
                Identifier = "A",
                RegistrationDate = DateTime.Now,
                TerminationFine = "40 %",
                Value = "R$ 19,00"
            };

            // Criando uma lista de exemplo
            _planRentalList = new List<PlanRentalModel> { _validPlanRentalModel };
        }

        [Fact(DisplayName = "Deve inserir um novo plano de locação e retornar seu ID")]
        public async Task PostAsync_ShouldReturnPlanRentalId_WhenPlanRentalModelIsValid()
        {
            // Arrange
            _mockPlanRentalRepository
                .Setup(repo => repo.PostAsync(_validPlanRentalModel))
                .ReturnsAsync(_validPlanRentalModel.Id);

            // Act
            var result = await _mockPlanRentalRepository.Object.PostAsync(_validPlanRentalModel);

            // Assert
            _mockPlanRentalRepository.Verify(repo => repo.PostAsync(_validPlanRentalModel), Times.Once);
            Assert.Equal(_validPlanRentalModel.Id, result);
        }

        [Fact(DisplayName = "Deve retornar todos os planos de locação")]
        public async Task GetAllAsync_ShouldReturnAllPlanRentals()
        {
            // Arrange
            _mockPlanRentalRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(_planRentalList);

            // Act
            var result = await _mockPlanRentalRepository.Object.GetAllAsync();

            // Assert
            _mockPlanRentalRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            Assert.Equal(_planRentalList, result);
        }

        [Fact(DisplayName = "Deve retornar um plano de locação pelo ID")]
        public async Task GetByIdAsync_ShouldReturnPlanRentalModel_WhenIdIsValid()
        {
            // Arrange
            var planRentalId = 1;
            _mockPlanRentalRepository
                .Setup(repo => repo.GetByIdAsync(planRentalId))
                .ReturnsAsync(_validPlanRentalModel);

            // Act
            var result = await _mockPlanRentalRepository.Object.GetByIdAsync(planRentalId);

            // Assert
            _mockPlanRentalRepository.Verify(repo => repo.GetByIdAsync(planRentalId), Times.Once);
            Assert.Equal(_validPlanRentalModel, result);
        }

        [Fact(DisplayName = "Deve atualizar os dados de um plano de locação")]
        public async Task PutAsync_ShouldUpdatePlanRental_WhenPlanRentalModelIsValid()
        {
            // Arrange
            _mockPlanRentalRepository
                .Setup(repo => repo.PutAsync(_validPlanRentalModel))
                .Returns(Task.CompletedTask);

            // Act
            await _mockPlanRentalRepository.Object.PutAsync(_validPlanRentalModel);

            // Assert
            _mockPlanRentalRepository.Verify(repo => repo.PutAsync(_validPlanRentalModel), Times.Once);
        }

        [Fact(DisplayName = "Deve deletar um plano de locação pelo ID")]
        public async Task DeleteAsync_ShouldDeletePlanRental_WhenIdIsValid()
        {
            // Arrange
            var planRentalId = 1;
            _mockPlanRentalRepository
                .Setup(repo => repo.DeleteAsync(planRentalId))
                .Returns(Task.CompletedTask);

            // Act
            await _mockPlanRentalRepository.Object.DeleteAsync(planRentalId);

            // Assert
            _mockPlanRentalRepository.Verify(repo => repo.DeleteAsync(planRentalId), Times.Once);
        }
    }
}