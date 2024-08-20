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
    public class IMotorcycleTypeServiceTests
    {
        private readonly Mock<IMotorcycleTypeService> _mockMotorcycleTypeService;
        private readonly MotorcycleTypeModel _validMotorcycleTypeModel;
        private readonly List<MotorcycleTypeModel> _motorcycleTypeList;

        public IMotorcycleTypeServiceTests()
        {
            _mockMotorcycleTypeService = new Mock<IMotorcycleTypeService>();

            // Criando um exemplo de MotorcycleTypeModel válido
            _validMotorcycleTypeModel = new MotorcycleTypeModel
            {
                Id = 1,
                TypeName = "Admin"
            };

            // Criando uma lista de exemplo
            _motorcycleTypeList = new List<MotorcycleTypeModel> { _validMotorcycleTypeModel };
        }

        [Fact(DisplayName = "Deve inserir um novo tipo de moto e retornar seu ID")]
        public async Task PostAsync_ShouldReturnMotorcycleTypeId_WhenMotorcycleTypeModelIsValid()
        {
            // Arrange
            _mockMotorcycleTypeService
                .Setup(service => service.PostAsync(_validMotorcycleTypeModel))
                .ReturnsAsync(_validMotorcycleTypeModel.Id);

            // Act
            var result = await _mockMotorcycleTypeService.Object.PostAsync(_validMotorcycleTypeModel);

            // Assert
            _mockMotorcycleTypeService.Verify(service => service.PostAsync(_validMotorcycleTypeModel), Times.Once);
            Assert.Equal(_validMotorcycleTypeModel.Id, result);
        }

        [Fact(DisplayName = "Deve retornar todos os tipos de motos")]
        public async Task GetAllAsync_ShouldReturnAllMotorcycleTypes()
        {
            // Arrange
            _mockMotorcycleTypeService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(_motorcycleTypeList);

            // Act
            var result = await _mockMotorcycleTypeService.Object.GetAllAsync();

            // Assert
            _mockMotorcycleTypeService.Verify(service => service.GetAllAsync(), Times.Once);
            Assert.Equal(_motorcycleTypeList, result);
        }

        [Fact(DisplayName = "Deve retornar um tipo de moto pelo ID")]
        public async Task GetByIdAsync_ShouldReturnMotorcycleTypeModel_WhenIdIsValid()
        {
            // Arrange
            var motorcycleTypeId = 1;
            _mockMotorcycleTypeService
                .Setup(service => service.GetByIdAsync(motorcycleTypeId))
                .ReturnsAsync(_validMotorcycleTypeModel);

            // Act
            var result = await _mockMotorcycleTypeService.Object.GetByIdAsync(motorcycleTypeId);

            // Assert
            _mockMotorcycleTypeService.Verify(service => service.GetByIdAsync(motorcycleTypeId), Times.Once);
            Assert.Equal(_validMotorcycleTypeModel, result);
        }

        [Fact(DisplayName = "Deve atualizar os dados de um tipo de moto")]
        public async Task PutAsync_ShouldUpdateMotorcycleType_WhenMotorcycleTypeModelIsValid()
        {
            // Arrange
            _mockMotorcycleTypeService
                .Setup(service => service.PutAsync(_validMotorcycleTypeModel))
                .Returns(Task.CompletedTask);

            // Act
            await _mockMotorcycleTypeService.Object.PutAsync(_validMotorcycleTypeModel);

            // Assert
            _mockMotorcycleTypeService.Verify(service => service.PutAsync(_validMotorcycleTypeModel), Times.Once);
        }

        [Fact(DisplayName = "Deve deletar um tipo de moto pelo ID")]
        public async Task DeleteAsync_ShouldDeleteMotorcycleType_WhenIdIsValid()
        {
            // Arrange
            var motorcycleTypeId = 1;
            _mockMotorcycleTypeService
                .Setup(service => service.DeleteAsync(motorcycleTypeId))
                .Returns(Task.CompletedTask);

            // Act
            await _mockMotorcycleTypeService.Object.DeleteAsync(motorcycleTypeId);

            // Assert
            _mockMotorcycleTypeService.Verify(service => service.DeleteAsync(motorcycleTypeId), Times.Once);
        }
    }
}