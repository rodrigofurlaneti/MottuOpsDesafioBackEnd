using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using MottuOpsDesafioBackEnd.WebApplication.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.WebApplication.Controllers
{
    public class MotorcycleTypeControllerTests
    {
        private readonly Mock<IMotorcycleTypeService> _mockMotorcycleTypeService;
        private readonly MotorcycleTypeController _motorcycleTypeController;

        public MotorcycleTypeControllerTests()
        {
            _mockMotorcycleTypeService = new Mock<IMotorcycleTypeService>();
            _motorcycleTypeController = new MotorcycleTypeController(_mockMotorcycleTypeService.Object);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _motorcycleTypeController.TempData = tempData.Object;
        }

        [Fact(DisplayName = "Index deve retornar uma View")]
        public void Index_ShouldReturnView()
        {
            // Act
            var result = _motorcycleTypeController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact(DisplayName = "GetAll deve retornar uma View com todos os MotorcycleTypeModel")]
        public async Task GetAll_ShouldReturnView_WithAllMotorcycleTypeModels()
        {
            // Arrange
            var motorcycleTypes = new List<MotorcycleTypeModel> { new MotorcycleTypeModel { Id = 1, TypeName = "Type1" } };
            _mockMotorcycleTypeService.Setup(service => service.GetAllAsync()).ReturnsAsync(motorcycleTypes);

            // Act
            var result = await _motorcycleTypeController.GetAll();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<MotorcycleTypeModel>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact(DisplayName = "Post deve retornar BadRequest se MotorcycleTypeModel for nulo")]
        public async Task Post_ShouldReturnBadRequest_WhenMotorcycleTypeModelIsNull()
        {
            // Act
            var result = await _motorcycleTypeController.Post(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("A solicitação do tipo da moto é nula", badRequestResult.Value);
        }

        [Fact(DisplayName = "Post deve redirecionar para Index com sucesso")]
        public async Task Post_ShouldRedirectToIndex_OnSuccess()
        {
            // Arrange
            var motorcycleTypeModel = new MotorcycleTypeModel { TypeName = "Type1" };
            _mockMotorcycleTypeService.Setup(service => service.PostAsync(motorcycleTypeModel)).ReturnsAsync(1);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _motorcycleTypeController.TempData = tempData.Object;

            // Act
            var result = await _motorcycleTypeController.Post(motorcycleTypeModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Authentication", redirectResult.ControllerName);

            // Verificando se TempData["MotorcycleTypeSuccess"] foi configurado corretamente
            tempData.VerifySet(t => t["MotorcycleTypeSuccess"] = "Valido", Times.Once);
        }

        [Fact(DisplayName = "GetById deve retornar BadRequest se o ID for 0")]
        public async Task GetById_ShouldReturnBadRequest_WhenIdIsZero()
        {
            // Act
            var result = await _motorcycleTypeController.GetById(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("A solicitação para atualizar a moto é nula", badRequestResult.Value);
        }

        [Fact(DisplayName = "GetById deve redirecionar para Index se o tipo de moto não for encontrado")]
        public async Task GetById_ShouldRedirectToIndex_WhenMotorcycleTypeNotFound()
        {
            // Arrange
            var motorcycleTypeId = 1;
            _mockMotorcycleTypeService.Setup(service => service.GetByIdAsync(motorcycleTypeId)).ReturnsAsync((MotorcycleTypeModel)null);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _motorcycleTypeController.TempData = tempData.Object;

            // Act
            var result = await _motorcycleTypeController.GetById(motorcycleTypeId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Authentication", redirectResult.ControllerName);

            // Verificando se TempData["MotorcycleTypeErro"] foi configurado corretamente
            tempData.VerifySet(t => t["MotorcycleTypeErro"] = "Invalido", Times.Once);
        }

        [Fact(DisplayName = "Update deve redirecionar para Index com sucesso")]
        public async Task Update_ShouldRedirectToIndex_OnSuccess()
        {
            // Arrange
            var motorcycleTypeModel = new MotorcycleTypeModel { Id = 1, TypeName = "Type1" };

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _motorcycleTypeController.TempData = tempData.Object;

            // Act
            var result = await _motorcycleTypeController.Update(motorcycleTypeModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Authentication", redirectResult.ControllerName);

            // Verificando se TempData["MotorcycleTypeUpdateSuccess"] foi configurado corretamente
            tempData.VerifySet(t => t["MotorcycleTypeUpdateSuccess"] = "Valido", Times.Once);
        }

        [Fact(DisplayName = "Delete deve retornar BadRequest se o ID for 0")]
        public async Task Delete_ShouldReturnBadRequest_WhenIdIsZero()
        {
            // Act
            var result = await _motorcycleTypeController.Delete(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("A solicitação para apagar o usuário é nula", badRequestResult.Value);
        }

        [Fact(DisplayName = "Delete deve redirecionar para Index com sucesso")]
        public async Task Delete_ShouldRedirectToIndex_OnSuccess()
        {
            // Arrange
            var motorcycleTypeId = 1;

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _motorcycleTypeController.TempData = tempData.Object;

            // Act
            var result = await _motorcycleTypeController.Delete(motorcycleTypeId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Authentication", redirectResult.ControllerName);

            // Verificando se TempData["MotorcycleTypeDeleteSuccess"] foi configurado corretamente
            tempData.VerifySet(t => t["MotorcycleTypeDeleteSuccess"] = "Valido", Times.Once);
        }
    }
}