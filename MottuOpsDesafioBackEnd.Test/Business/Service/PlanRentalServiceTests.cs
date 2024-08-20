using Moq;
using MottuOpsDesafioBackEnd.Business.Service;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Business.Service
{
    public class PlanRentalServiceTests
    {
        private readonly Mock<IPlanRentalRepository> _mockPlanRentalRepository;
        private readonly PlanRentalService _planRentalService;
        private readonly PlanRentalModel _validPlanRentalModel;

        public PlanRentalServiceTests()
        {
            _mockPlanRentalRepository = new Mock<IPlanRentalRepository>();
            _planRentalService = new PlanRentalService(_mockPlanRentalRepository.Object);

            // Criando um exemplo de PlanRentalModel válido
            _validPlanRentalModel = new PlanRentalModel
            {
                Id = 1,
                Identifier = "PLAN123",
                Days = "7",
                RegistrationDate = DateTime.Now,
                TerminationFine = "20 %",
                Value = "R$ 24,00"
            };
        }

        [Fact(DisplayName = "Deve inserir um novo plano de locação e retornar seu ID")]
        public async Task PostAsync_ShouldReturnPlanRentalId_WhenPlanRentalModelIsValid()
        {
            // Arrange
            _mockPlanRentalRepository
                .Setup(repo => repo.PostAsync(_validPlanRentalModel))
                .ReturnsAsync(_validPlanRentalModel.Id);

            // Act
            var result = await _planRentalService.PostAsync(_validPlanRentalModel);

            // Assert
            _mockPlanRentalRepository.Verify(repo => repo.PostAsync(_validPlanRentalModel), Times.Once);
            Assert.Equal(_validPlanRentalModel.Id, result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao inserir um novo plano de locação")]
        public async Task PostAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockPlanRentalRepository
                .Setup(repo => repo.PostAsync(It.IsAny<PlanRentalModel>()))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _planRentalService.PostAsync(_validPlanRentalModel));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve retornar todos os planos de locação")]
        public async Task GetAllAsync_ShouldReturnAllPlanRentals()
        {
            // Arrange
            var planRentalList = new List<PlanRentalModel> { _validPlanRentalModel };
            _mockPlanRentalRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(planRentalList);

            // Act
            var result = await _planRentalService.GetAllAsync();

            // Assert
            _mockPlanRentalRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            Assert.Equal(planRentalList, result);
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
            var result = await _planRentalService.GetByIdAsync(planRentalId);

            // Assert
            _mockPlanRentalRepository.Verify(repo => repo.GetByIdAsync(planRentalId), Times.Once);
            Assert.Equal(_validPlanRentalModel, result);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao selecionar um plano de locação pelo ID")]
        public async Task GetByIdAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            var planRentalId = 1;
            _mockPlanRentalRepository
                .Setup(repo => repo.GetByIdAsync(planRentalId))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _planRentalService.GetByIdAsync(planRentalId));

            Assert.Equal("Erro no repositório", exception.Message);
        }

        [Fact(DisplayName = "Deve atualizar um plano de locação")]
        public async Task PutAsync_ShouldUpdatePlanRental_WhenPlanRentalModelIsValid()
        {
            // Arrange
            _mockPlanRentalRepository
                .Setup(repo => repo.PutAsync(_validPlanRentalModel))
                .Returns(Task.CompletedTask);

            // Act
            await _planRentalService.PutAsync(_validPlanRentalModel);

            // Assert
            _mockPlanRentalRepository.Verify(repo => repo.PutAsync(_validPlanRentalModel), Times.Once);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao atualizar um plano de locação")]
        public async Task PutAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockPlanRentalRepository
                .Setup(repo => repo.PutAsync(It.IsAny<PlanRentalModel>()))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _planRentalService.PutAsync(_validPlanRentalModel));

            Assert.Equal("Erro no repositório", exception.Message);
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
            await _planRentalService.DeleteAsync(planRentalId);

            // Assert
            _mockPlanRentalRepository.Verify(repo => repo.DeleteAsync(planRentalId), Times.Once);
        }

        [Fact(DisplayName = "Deve lançar ApplicationException quando ocorre um erro ao deletar um plano de locação")]
        public async Task DeleteAsync_ShouldThrowApplicationException_WhenRepositoryThrowsException()
        {
            // Arrange
            var planRentalId = 1;
            _mockPlanRentalRepository
                .Setup(repo => repo.DeleteAsync(planRentalId))
                .ThrowsAsync(new ApplicationException("Erro no repositório"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _planRentalService.DeleteAsync(planRentalId));

            Assert.Equal($"Erro no repositório", exception.Message);
        }
    }
}