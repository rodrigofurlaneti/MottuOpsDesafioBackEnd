using Moq;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Business.Interface
{
    public class ICourierServiceTests
    {
        private readonly Mock<ICourierService> _mockCourierService;
        private readonly CourierModel _validCourierModel;

        public ICourierServiceTests()
        {
            _mockCourierService = new Mock<ICourierService>();

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

        [Fact(DisplayName = "Deve inserir um novo entregador e retornar seu ID")]
        public async Task PostAsync_ShouldReturnCourierId_WhenCourierModelIsValid()
        {
            // Arrange
            _mockCourierService
                .Setup(service => service.PostAsync(_validCourierModel))
                .ReturnsAsync(_validCourierModel.Id);

            // Act
            var result = await _mockCourierService.Object.PostAsync(_validCourierModel);

            // Assert
            _mockCourierService.Verify(service => service.PostAsync(_validCourierModel), Times.Once);
            Assert.Equal(_validCourierModel.Id, result);
        }

        [Fact(DisplayName = "Deve retornar um entregador pelo ID")]
        public async Task GetByIdAsync_ShouldReturnCourierModel_WhenIdIsValid()
        {
            // Arrange
            var courierId = 1;
            _mockCourierService
                .Setup(service => service.GetByIdAsync(courierId))
                .ReturnsAsync(_validCourierModel);

            // Act
            var result = await _mockCourierService.Object.GetByIdAsync(courierId);

            // Assert
            _mockCourierService.Verify(service => service.GetByIdAsync(courierId), Times.Once);
            Assert.Equal(_validCourierModel, result);
        }

        [Fact(DisplayName = "Deve atualizar a CNH de um entregador")]
        public async Task PutCnhAsync_ShouldUpdateCourierCnh_WhenCourierModelIsValid()
        {
            // Arrange
            _mockCourierService
                .Setup(service => service.PutCnhAsync(_validCourierModel))
                .Returns(Task.CompletedTask);

            // Act
            await _mockCourierService.Object.PutCnhAsync(_validCourierModel);

            // Assert
            _mockCourierService.Verify(service => service.PutCnhAsync(_validCourierModel), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar verdadeiro se o CNPJ já estiver cadastrado")]
        public async Task GetByCnpjAsync_ShouldReturnTrue_WhenCnpjExists()
        {
            // Arrange
            var cnpj = "12345678901234";
            _mockCourierService
                .Setup(service => service.GetByCnpjAsync(cnpj))
                .ReturnsAsync(true);

            // Act
            var result = await _mockCourierService.Object.GetByCnpjAsync(cnpj);

            // Assert
            _mockCourierService.Verify(service => service.GetByCnpjAsync(cnpj), Times.Once);
            Assert.True(result);
        }

        [Fact(DisplayName = "Deve retornar verdadeiro se a CNH já estiver cadastrada")]
        public async Task GetByCnhAsync_ShouldReturnTrue_WhenCnhExists()
        {
            // Arrange
            var cnh = "12345678901";
            _mockCourierService
                .Setup(service => service.GetByCnhAsync(cnh))
                .ReturnsAsync(true);

            // Act
            var result = await _mockCourierService.Object.GetByCnhAsync(cnh);

            // Assert
            _mockCourierService.Verify(service => service.GetByCnhAsync(cnh), Times.Once);
            Assert.True(result);
        }
    }
}