using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using MottuOpsDesafioBackEnd.WebApplication.Controllers;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.WebApplication.Controllers
{
    public class MotorcycleRentalControllerTests
    {
        private readonly Mock<IMotorcycleRentalService> _mockMotorcycleRentalService;
        private readonly Mock<IMotorcycleService> _mockMotorcycleService;
        private readonly Mock<IPlanRentalService> _mockPlanRentalService;
        private readonly Mock<ICourierService> _mockCourierService;
        private readonly MotorcycleRentalController _motorcycleRentalController;

        public MotorcycleRentalControllerTests()
        {
            _mockMotorcycleRentalService = new Mock<IMotorcycleRentalService>();
            _mockMotorcycleService = new Mock<IMotorcycleService>();
            _mockPlanRentalService = new Mock<IPlanRentalService>();
            _mockCourierService = new Mock<ICourierService>();
            _motorcycleRentalController = new MotorcycleRentalController(
                _mockMotorcycleRentalService.Object,
                _mockMotorcycleService.Object,
                _mockPlanRentalService.Object,
                _mockCourierService.Object
            );

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _motorcycleRentalController.TempData = tempData.Object;
        }

        [Fact(DisplayName = "Index deve retornar uma View com MotorcycleRentalModel")]
        public async Task Index_ShouldReturnView_WithMotorcycleRentalModel()
        {
            // Arrange
            var motorcycles = new List<MotorcycleModel> { new MotorcycleModel { Id = 1, LicensePlate = "ABC1234" } };
            var plans = new List<PlanRentalModel> { new PlanRentalModel { Id = 1, Days = "1" } };

            _mockMotorcycleService.Setup(service => service.GetAllAsync()).ReturnsAsync(motorcycles);
            _mockPlanRentalService.Setup(service => service.GetAllAsync()).ReturnsAsync(plans);

            // Act
            var result = await _motorcycleRentalController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<MotorcycleRentalModel>(viewResult.Model);
            Assert.Single(model.Motorcycles);
            Assert.Single(model.PlansRental);
        }

        [Fact(DisplayName = "Post deve retornar BadRequest se MotorcycleRentalModel for nulo")]
        public async Task Post_ShouldReturnBadRequest_WhenMotorcycleRentalModelIsNull()
        {
            // Act
            var result = await _motorcycleRentalController.Post(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("A solicitação do moto é nula", badRequestResult.Value);
        }

        [Fact(DisplayName = "Post deve redirecionar para Index se a CNH não for válida")]
        public async Task Post_ShouldRedirectToIndex_WhenCNHIsNotValid()
        {
            // Arrange
            var motorcycleRentalModel = new MotorcycleRentalModel { CourierId = 1 };
            var courier = new CourierModel { CNHType = "B" };

            _mockCourierService.Setup(service => service.GetByIdAsync(motorcycleRentalModel.CourierId)).ReturnsAsync(courier);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _motorcycleRentalController.TempData = tempData.Object;

            // Act
            var result = await _motorcycleRentalController.Post(motorcycleRentalModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Authentication", redirectResult.ControllerName);

            // Verificando se TempData["MotorcycleRentalCNHTypeBErro"] foi configurado corretamente
            tempData.VerifySet(t => t["MotorcycleRentalCNHTypeBErro"] = "Invalido", Times.Once);
        }

        [Fact(DisplayName = "Post deve redirecionar para Index se a criação do aluguel falhar")]
        public async Task Post_ShouldRedirectToIndex_WhenRentalCreationFails()
        {
            // Arrange
            var motorcycleRentalModel = new MotorcycleRentalModel { CourierId = 1 };
            var courier = new CourierModel { CNHType = "A" };

            _mockCourierService.Setup(service => service.GetByIdAsync(motorcycleRentalModel.CourierId)).ReturnsAsync(courier);
            _mockMotorcycleRentalService.Setup(service => service.PostAsync(motorcycleRentalModel)).ReturnsAsync(0);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _motorcycleRentalController.TempData = tempData.Object;

            // Act
            var result = await _motorcycleRentalController.Post(motorcycleRentalModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Authentication", redirectResult.ControllerName);

            // Verificando se TempData["MotorcycleRentalErro"] foi configurado corretamente
            tempData.VerifySet(t => t["MotorcycleRentalErro"] = "Invalido", Times.Once);
        }

        [Fact(DisplayName = "Post deve redirecionar para Index com sucesso")]
        public async Task Post_ShouldRedirectToIndex_OnSuccess()
        {
            // Arrange
            var motorcycleRentalModel = new MotorcycleRentalModel { CourierId = 1 };
            var courier = new CourierModel { CNHType = "A" };

            _mockCourierService.Setup(service => service.GetByIdAsync(motorcycleRentalModel.CourierId)).ReturnsAsync(courier);
            _mockMotorcycleRentalService.Setup(service => service.PostAsync(motorcycleRentalModel)).ReturnsAsync(1);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _motorcycleRentalController.TempData = tempData.Object;

            // Act
            var result = await _motorcycleRentalController.Post(motorcycleRentalModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Authentication", redirectResult.ControllerName);

            // Verificando se TempData["MotorcycleRentalSuccess"] foi configurado corretamente
            tempData.VerifySet(t => t["MotorcycleRentalSuccess"] = "Valido", Times.Once);
        }
    }
}