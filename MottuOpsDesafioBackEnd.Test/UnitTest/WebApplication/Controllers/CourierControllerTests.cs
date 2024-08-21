using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using MottuOpsDesafioBackEnd.WebApplication.Controllers;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.WebApplication.Controllers
{
    public class CourierControllerTests
    {
        private readonly Mock<ICourierService> _mockCourierService;
        private readonly CourierController _courierController;

        public CourierControllerTests()
        {
            _mockCourierService = new Mock<ICourierService>();
            _courierController = new CourierController(_mockCourierService.Object);

            // Simulando TempData
            var tempData = new Mock<ITempDataDictionary>();
            _courierController.TempData = tempData.Object;
        }

        [Fact(DisplayName = "Index deve retornar uma View")]
        public void Index_ShouldReturnView()
        {
            // Act
            var result = _courierController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact(DisplayName = "Post deve retornar BadRequest se CourierModel for nulo")]
        public async Task Post_ShouldReturnBadRequest_WhenCourierModelIsNull()
        {
            // Act
            var result = await _courierController.Post(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("A solicitação do entregador é nula", badRequestResult.Value);
        }

        [Fact(DisplayName = "Post deve redirecionar para Index se a criação do entregador falhar")]
        public async Task Post_ShouldRedirectToIndex_WhenCourierCreationFails()
        {
            // Arrange
            var courierModel = new CourierModel { Identifier = "C123" };

            _mockCourierService
                .Setup(service => service.PostAsync(courierModel))
                .ReturnsAsync(0);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _courierController.TempData = tempData.Object;

            // Act
            var result = await _courierController.Post(courierModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Home", redirectResult.ControllerName);

            // Verificando se TempData["CourierErro"] foi configurado corretamente
            tempData.VerifySet(t => t["CourierErro"] = "Ocorreu um erro ao tentar salvar o entregador.", Times.Once);
        }

        [Fact(DisplayName = "Post deve redirecionar para Index com sucesso")]
        public async Task Post_ShouldRedirectToIndex_OnSuccess()
        {
            // Arrange
            var courierModel = new CourierModel { Identifier = "C123" };

            _mockCourierService
                .Setup(service => service.PostAsync(courierModel))
                .ReturnsAsync(1);

            // Mock TempData com um dicionário real para capturar valores
            var tempData = new Mock<ITempDataDictionary>();
            _courierController.TempData = tempData.Object;

            // Act
            var result = await _courierController.Post(courierModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Home", redirectResult.ControllerName);

            // Verificando se TempData foi atualizado corretamente
            tempData.VerifySet(t => t["CourierSuccess"] = "Valido", Times.Once);
        }

        [Fact(DisplayName = "GetById deve retornar BadRequest se o ID for 0")]
        public async Task GetById_ShouldReturnBadRequest_WhenIdIsZero()
        {
            // Act
            var result = await _courierController.GetById(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("A solicitação para atualizar o entregador é nula", badRequestResult.Value);
        }

        [Fact(DisplayName = "GetById deve redirecionar para Index se o entregador não for encontrado")]
        public async Task GetById_ShouldRedirectToIndex_WhenCourierNotFound()
        {
            // Arrange
            var courierId = 1;

            _mockCourierService
                .Setup(service => service.GetByIdAsync(courierId))
                .ReturnsAsync((CourierModel)null);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _courierController.TempData = tempData.Object;

            // Act
            var result = await _courierController.GetById(courierId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Authentication", redirectResult.ControllerName);

            // Verifica se TempData["CourierErro"] foi configurado corretamente
            tempData.VerifySet(t => t["CourierErro"] = "Invalido", Times.Once);
        }

        [Fact(DisplayName = "GetById deve retornar View com o entregador encontrado")]
        public async Task GetById_ShouldReturnView_WithCourier()
        {
            // Arrange
            var courierId = 1;
            var courier = new CourierModel { Id = courierId, Identifier = "C123" };

            _mockCourierService
                .Setup(service => service.GetByIdAsync(courierId))
                .ReturnsAsync(courier);

            // Act
            var result = await _courierController.GetById(courierId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result.Result);
            Assert.Equal(courier, viewResult.Model);
        }

        [Fact(DisplayName = "UpdateCnh deve retornar BadRequest se CourierModel for nulo")]
        public async Task UpdateCnh_ShouldReturnBadRequest_WhenCourierModelIsNull()
        {
            // Act
            var result = await _courierController.UpdateCnh(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("A solicitação do usuário é nula", badRequestResult.Value);
        }

        [Fact(DisplayName = "UpdateCnh deve redirecionar para Index com sucesso")]
        public async Task UpdateCnh_ShouldRedirectToIndex_OnSuccess()
        {
            // Arrange
            var courierModel = new CourierModel { Identifier = "C123" };

            // Mock TempData com um dicionário real para capturar valores
            var tempData = new Mock<ITempDataDictionary>();
            _courierController.TempData = tempData.Object;

            // Act
            var result = await _courierController.UpdateCnh(courierModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Authentication", redirectResult.ControllerName);

            // Verificando se TempData foi atualizado corretamente
            tempData.VerifySet(t => t["CourierUpdateCnhSuccess"] = "Valido", Times.Once);
        }

        [Fact(DisplayName = "CnpjExist deve retornar JsonResult false se CNPJ for nulo ou vazio")]
        public async Task CnpjExist_ShouldReturnJsonFalse_WhenCnpjIsNullOrEmpty()
        {
            // Act
            var result = await _courierController.CnpjExist(null);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.False((bool)jsonResult.Value);
        }

        [Fact(DisplayName = "CnpjExist deve retornar JsonResult com status do CNPJ")]
        public async Task CnpjExist_ShouldReturnJsonResult_WithCnpjStatus()
        {
            // Arrange
            var cnpj = "12345678000195";
            _mockCourierService
                .Setup(service => service.GetByCnpjAsync(cnpj))
                .ReturnsAsync(true);

            // Act
            var result = await _courierController.CnpjExist(cnpj);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.True((bool)jsonResult.Value);
        }

        [Fact(DisplayName = "CnhExist deve retornar JsonResult false se CNH for nulo ou vazio")]
        public async Task CnhExist_ShouldReturnJsonFalse_WhenCnhIsNullOrEmpty()
        {
            // Act
            var result = await _courierController.CnhExist(null);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.False((bool)jsonResult.Value);
        }

        [Fact(DisplayName = "CnhExist deve retornar JsonResult com status da CNH")]
        public async Task CnhExist_ShouldReturnJsonResult_WithCnhStatus()
        {
            // Arrange
            var cnh = "1234567890";
            _mockCourierService
                .Setup(service => service.GetByCnhAsync(cnh))
                .ReturnsAsync(true);

            // Act
            var result = await _courierController.CnhExist(cnh);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.True((bool)jsonResult.Value);
        }
    }
}