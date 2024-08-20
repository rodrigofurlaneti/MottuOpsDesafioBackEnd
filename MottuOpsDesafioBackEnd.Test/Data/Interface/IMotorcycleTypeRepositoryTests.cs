using Moq;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Data.Interface
{
    public class IMotorcycleTypeRepositoryTests
    {
        private readonly Mock<IMotorcycleTypeRepository> _mockMotorcycleTypeRepository;
        private readonly MotorcycleTypeModel _validMotorcycleTypeModel;
        private readonly List<MotorcycleTypeModel> _motorcycleTypeList;

        public IMotorcycleTypeRepositoryTests()
        {
            _mockMotorcycleTypeRepository = new Mock<IMotorcycleTypeRepository>();

            // Criando um exemplo de MotorcycleTypeModel válido
            _validMotorcycleTypeModel = new MotorcycleTypeModel
            {
                Id = 1,
                TypeName = "Sport",
            };

            // Criando uma lista de exemplo
            _motorcycleTypeList = new List<MotorcycleTypeModel> { _validMotorcycleTypeModel };
        }

        [Fact(DisplayName = "Deve inserir um novo tipo de moto e retornar seu ID")]
        public async Task PostAsync_ShouldReturnMotorcycleTypeId_WhenMotorcycleTypeModelIsValid()
        {
            // Arrange
            _mockMotorcycleTypeRepository
                .Setup(repo => repo.PostAsync(_validMotorcycleTypeModel))
                .ReturnsAsync(_validMotorcycleTypeModel.Id);

            // Act
            var result = await _mockMotorcycleTypeRepository.Object.PostAsync(_validMotorcycleTypeModel);

            // Assert
            _mockMotorcycleTypeRepository.Verify(repo => repo.PostAsync(_validMotorcycleTypeModel), Times.Once);
            Assert.Equal(_validMotorcycleTypeModel.Id, result);
        }

        [Fact(DisplayName = "Deve retornar todos os tipos de motos")]
        public async Task GetAllAsync_ShouldReturnAllMotorcycleTypes()
        {
            // Arrange
            _mockMotorcycleTypeRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(_motorcycleTypeList);

            // Act
            var result = await _mockMotorcycleTypeRepository.Object.GetAllAsync();

            // Assert
            _mockMotorcycleTypeRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            Assert.Equal(_motorcycleTypeList, result);
        }

        [Fact(DisplayName = "Deve retornar um tipo de moto pelo ID")]
        public async Task GetByIdAsync_ShouldReturnMotorcycleTypeModel_WhenIdIsValid()
        {
            // Arrange
            var motorcycleTypeId = 1;
            _mockMotorcycleTypeRepository
                .Setup(repo => repo.GetByIdAsync(motorcycleTypeId))
                .ReturnsAsync(_validMotorcycleTypeModel);

            // Act
            var result = await _mockMotorcycleTypeRepository.Object.GetByIdAsync(motorcycleTypeId);

            // Assert
            _mockMotorcycleTypeRepository.Verify(repo => repo.GetByIdAsync(motorcycleTypeId), Times.Once);
            Assert.Equal(_validMotorcycleTypeModel, result);
        }

        [Fact(DisplayName = "Deve atualizar os dados de um tipo de moto")]
        public async Task PutAsync_ShouldUpdateMotorcycleType_WhenMotorcycleTypeModelIsValid()
        {
            // Arrange
            _mockMotorcycleTypeRepository
                .Setup(repo => repo.PutAsync(_validMotorcycleTypeModel))
                .Returns(Task.CompletedTask);

            // Act
            await _mockMotorcycleTypeRepository.Object.PutAsync(_validMotorcycleTypeModel);

            // Assert
            _mockMotorcycleTypeRepository.Verify(repo => repo.PutAsync(_validMotorcycleTypeModel), Times.Once);
        }

        [Fact(DisplayName = "Deve deletar um tipo de moto pelo ID")]
        public async Task DeleteAsync_ShouldDeleteMotorcycleType_WhenIdIsValid()
        {
            // Arrange
            var motorcycleTypeId = 1;
            _mockMotorcycleTypeRepository
                .Setup(repo => repo.DeleteAsync(motorcycleTypeId))
                .Returns(Task.CompletedTask);

            // Act
            await _mockMotorcycleTypeRepository.Object.DeleteAsync(motorcycleTypeId);

            // Assert
            _mockMotorcycleTypeRepository.Verify(repo => repo.DeleteAsync(motorcycleTypeId), Times.Once);
        }
    }
}