using Moq;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.Data.Interface
{
    public class IMotorcycleRentalRepositoryTests
    {
        private readonly Mock<IMotorcycleRentalRepository> _mockMotorcycleRentalRepository;
        private readonly MotorcycleRentalModel _validMotorcycleRentalModel;

        public IMotorcycleRentalRepositoryTests()
        {
            _mockMotorcycleRentalRepository = new Mock<IMotorcycleRentalRepository>();

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
        public async Task PostAsync_ShouldReturnRentalId_WhenMotorcycleRentalModelIsValid()
        {
            // Arrange
            _mockMotorcycleRentalRepository
                .Setup(repo => repo.PostAsync(_validMotorcycleRentalModel))
                .ReturnsAsync(_validMotorcycleRentalModel.Id);

            // Act
            var result = await _mockMotorcycleRentalRepository.Object.PostAsync(_validMotorcycleRentalModel);

            // Assert
            _mockMotorcycleRentalRepository.Verify(repo => repo.PostAsync(_validMotorcycleRentalModel), Times.Once);
            Assert.Equal(_validMotorcycleRentalModel.Id, result);
        }

        [Fact(DisplayName = "Deve retornar uma locação de moto pelo CourierId")]
        public async Task GetByCourierIdAsync_ShouldReturnMotorcycleRentalModel_WhenCourierIdIsValid()
        {
            // Arrange
            var courierId = 1;
            _mockMotorcycleRentalRepository
                .Setup(repo => repo.GetByCourierIdAsync(courierId))
                .ReturnsAsync(_validMotorcycleRentalModel);

            // Act
            var result = await _mockMotorcycleRentalRepository.Object.GetByCourierIdAsync(courierId);

            // Assert
            _mockMotorcycleRentalRepository.Verify(repo => repo.GetByCourierIdAsync(courierId), Times.Once);
            Assert.Equal(_validMotorcycleRentalModel, result);
        }
    }
}
