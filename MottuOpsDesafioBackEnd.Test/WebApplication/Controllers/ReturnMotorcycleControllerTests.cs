using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using MottuOpsDesafioBackEnd.WebApplication.Controllers;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.WebApplication.Controllers
{
    public class ReturnMotorcycleControllerTests
    {
        private readonly Mock<IMotorcycleRentalService> _motorcycleRentalServiceMock;
        private readonly Mock<IPlanRentalService> _planRentalServiceMock;
        private readonly ReturnMotorcycleController _controller;

        public ReturnMotorcycleControllerTests()
        {
            _motorcycleRentalServiceMock = new Mock<IMotorcycleRentalService>();
            _planRentalServiceMock = new Mock<IPlanRentalService>();
            _controller = new ReturnMotorcycleController(_motorcycleRentalServiceMock.Object, _planRentalServiceMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsBadRequest_WhenIdIsZero()
        {
            // Act
            var result = await _controller.Index(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("A solicitação para atualizar a moto é nula", badRequestResult.Value);
        }

        [Fact]
        public async Task Index_ReturnsRedirectToAction_WhenMotorcycleRentalIsNull()
        {
            // Arrange
            _motorcycleRentalServiceMock.Setup(service => service.GetByCourierIdAsync(It.IsAny<int>()))
                .ReturnsAsync((MotorcycleRentalModel)null);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _controller.TempData = tempData.Object;

            // Act
            var result = await _controller.Index(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Authentication", redirectToActionResult.ControllerName);

            tempData.VerifySet(t => t["MotorcycleRentalErro"] = "Invalido", Times.Once);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithMotorcycleRentalModel()
        {
            // Arrange
            var motorcycleRental = new MotorcycleRentalModel
            {
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now,
                PlanType = "7"
            };
            var planRental = new List<PlanRentalModel>
            {
                new PlanRentalModel { Days = "7", Value = "R$ 100" }
            };

            _motorcycleRentalServiceMock.Setup(service => service.GetByCourierIdAsync(It.IsAny<int>()))
                .ReturnsAsync(motorcycleRental);
            _planRentalServiceMock.Setup(service => service.GetAllAsync())
                .ReturnsAsync(planRental);

            // Act
            var result = await _controller.Index(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<MotorcycleRentalModel>(viewResult.ViewData.Model);
            Assert.Equal(motorcycleRental, model);
            Assert.Equal(planRental.First().Value, model.Plans.Value);

            // Verifica o cálculo do valor total sem multa
            decimal expectedValue = 5 * 100; // 5 dias * R$100
            Assert.Equal(expectedValue, viewResult.ViewData["ValorTotal"]);
        }

        [Fact]
        public async Task Index_ReturnsServerError_OnException()
        {
            // Arrange
            _motorcycleRentalServiceMock.Setup(service => service.GetByCourierIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.Index(1);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Erro do Servidor Interno", statusCodeResult.Value);
        }
    }
}