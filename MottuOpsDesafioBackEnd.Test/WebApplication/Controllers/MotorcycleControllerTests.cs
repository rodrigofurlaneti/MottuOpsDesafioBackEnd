using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using MottuOpsDesafioBackEnd.WebApplication.Controllers;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.WebApplication.Controllers
{
    public class MotorcycleControllerTests
    {
        private readonly Mock<IMotorcycleService> _mockMotorcycleService;
        private readonly Mock<IMotorcycleTypeService> _mockMotorcycleTypeService;
        private readonly MotorcycleController _motorcycleController;

        public MotorcycleControllerTests()
        {
            _mockMotorcycleService = new Mock<IMotorcycleService>();
            _mockMotorcycleTypeService = new Mock<IMotorcycleTypeService>();
            _motorcycleController = new MotorcycleController(_mockMotorcycleService.Object, _mockMotorcycleTypeService.Object);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _motorcycleController.TempData = tempData.Object;
        }

        [Fact(DisplayName = "Index deve retornar uma View com MotorcycleModel")]
        public async Task Index_ShouldReturnView_WithMotorcycleModel()
        {
            // Arrange
            var motorcycleTypes = new List<MotorcycleTypeModel> { new MotorcycleTypeModel { Id = 1, TypeName = "Type1" } };
            _mockMotorcycleTypeService.Setup(service => service.GetAllAsync()).ReturnsAsync(motorcycleTypes);

            // Act
            var result = await _motorcycleController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<MotorcycleModel>(viewResult.Model);
            Assert.Equal(1, model.Models.Count);
            Assert.Equal("Type1", model.Models.First().TypeName);
        }

        [Fact(DisplayName = "GetAll deve retornar uma View com todos os MotorcycleModel")]
        public async Task GetAll_ShouldReturnView_WithAllMotorcycleModels()
        {
            // Arrange
            var motorcycles = new List<MotorcycleModel> { new MotorcycleModel { Id = 1, LicensePlate = "ABC1234" } };
            _mockMotorcycleService.Setup(service => service.GetAllAsync()).ReturnsAsync(motorcycles);

            // Act
            var result = await _motorcycleController.GetAll();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<MotorcycleModel>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact(DisplayName = "Post deve retornar BadRequest se MotorcycleModel for nulo")]
        public async Task Post_ShouldReturnBadRequest_WhenMotorcycleModelIsNull()
        {
            // Act
            var result = await _motorcycleController.Post(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("A solicitação do moto é nula", badRequestResult.Value);
        }

        [Fact(DisplayName = "Post deve redirecionar para Index se a placa já existir")]
        public async Task Post_ShouldRedirectToIndex_WhenLicensePlateExists()
        {
            // Arrange
            var motorcycleModel = new MotorcycleModel { LicensePlate = "ABC1234" };
            _mockMotorcycleService.Setup(service => service.GetByLicensePlateAsync(motorcycleModel.LicensePlate)).ReturnsAsync(true);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _motorcycleController.TempData = tempData.Object;

            // Act
            var result = await _motorcycleController.Post(motorcycleModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Authentication", redirectResult.ControllerName);

            // Verificando se TempData["MotorcycleLicensePlateExistErro"] foi configurado corretamente
            tempData.VerifySet(t => t["MotorcycleLicensePlateExistErro"] = "Invalido", Times.Once);
        }

        [Fact(DisplayName = "Post deve redirecionar para Index com sucesso")]
        public async Task Post_ShouldRedirectToIndex_OnSuccess()
        {
            // Arrange
            var motorcycleModel = new MotorcycleModel { LicensePlate = "ABC1234" };
            _mockMotorcycleService.Setup(service => service.GetByLicensePlateAsync(motorcycleModel.LicensePlate)).ReturnsAsync(false);
            _mockMotorcycleService.Setup(service => service.PostAsync(motorcycleModel)).ReturnsAsync(1);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _motorcycleController.TempData = tempData.Object;

            // Act
            var result = await _motorcycleController.Post(motorcycleModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Authentication", redirectResult.ControllerName);

            // Verificando se TempData["MotorcycleSuccess"] foi configurado corretamente
            tempData.VerifySet(t => t["MotorcycleSuccess"] = "Valido", Times.Once);
        }

        [Fact(DisplayName = "GetById deve retornar BadRequest se o ID for 0")]
        public async Task GetById_ShouldReturnBadRequest_WhenIdIsZero()
        {
            // Act
            var result = await _motorcycleController.GetById(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("A solicitação para atualizar a moto é nula", badRequestResult.Value);
        }

        [Fact(DisplayName = "GetById deve redirecionar para Index se a moto não for encontrada")]
        public async Task GetById_ShouldRedirectToIndex_WhenMotorcycleNotFound()
        {
            // Arrange
            var motorcycleId = 1;
            _mockMotorcycleService.Setup(service => service.GetByIdAsync(motorcycleId)).ReturnsAsync((MotorcycleModel)null);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _motorcycleController.TempData = tempData.Object;

            // Act
            var result = await _motorcycleController.GetById(motorcycleId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Authentication", redirectResult.ControllerName);

            // Verificando se TempData["MotorcycleErro"] foi configurado corretamente
            tempData.VerifySet(t => t["MotorcycleErro"] = "Invalido", Times.Once);
        }

        [Fact(DisplayName = "GetById deve retornar View com o modelo de moto encontrada")]
        public async Task GetById_ShouldReturnView_WithMotorcycleModel()
        {
            // Arrange
            var motorcycleId = 1;
            var motorcycle = new MotorcycleModel { Id = motorcycleId, LicensePlate = "ABC1234" };
            _mockMotorcycleService.Setup(service => service.GetByIdAsync(motorcycleId)).ReturnsAsync(motorcycle);

            var motorcycleTypes = new List<MotorcycleTypeModel> { new MotorcycleTypeModel { Id = 1, TypeName = "Type1" } };
            _mockMotorcycleTypeService.Setup(service => service.GetAllAsync()).ReturnsAsync(motorcycleTypes);

            // Act
            var result = await _motorcycleController.GetById(motorcycleId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result.Result);
            var model = Assert.IsType<MotorcycleModel>(viewResult.Model);
            Assert.Equal(motorcycleId, model.Id);
            Assert.Equal(1, model.Models.Count);
            Assert.Equal("Type1", model.Models.First().TypeName);
        }

        [Fact(DisplayName = "Update deve retornar BadRequest se MotorcycleModel for nulo")]
        public async Task Update_ShouldReturnBadRequest_WhenMotorcycleModelIsNull()
        {
            // Act
            var result = await _motorcycleController.Update(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("A solicitação do usuário é nula", badRequestResult.Value);
        }

        [Fact(DisplayName = "Update deve redirecionar para Index com sucesso")]
        public async Task Update_ShouldRedirectToIndex_OnSuccess()
        {
            // Arrange
            var motorcycleModel = new MotorcycleModel { Id = 1, LicensePlate = "ABC1234" };

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _motorcycleController.TempData = tempData.Object;

            // Act
            var result = await _motorcycleController.Update(motorcycleModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Authentication", redirectResult.ControllerName);

            // Verificando se TempData["MotorcycleUpdateSuccess"] foi configurado corretamente
            tempData.VerifySet(t => t["MotorcycleUpdateSuccess"] = "Valido", Times.Once);
        }

        [Fact(DisplayName = "Delete deve retornar BadRequest se o ID for 0")]
        public async Task Delete_ShouldReturnBadRequest_WhenIdIsZero()
        {
            // Act
            var result = await _motorcycleController.Delete(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("A solicitação para apagar o usuário é nula", badRequestResult.Value);
        }

        [Fact(DisplayName = "Delete deve redirecionar para Index com sucesso")]
        public async Task Delete_ShouldRedirectToIndex_OnSuccess()
        {
            // Arrange
            var motorcycleId = 1;

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _motorcycleController.TempData = tempData.Object;

            // Act
            var result = await _motorcycleController.Delete(motorcycleId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Authentication", redirectResult.ControllerName);

            // Verificando se TempData["MotorcycleDeleteSuccess"] foi configurado corretamente
            tempData.VerifySet(t => t["MotorcycleDeleteSuccess"] = "Valido", Times.Once);
        }
    }
}