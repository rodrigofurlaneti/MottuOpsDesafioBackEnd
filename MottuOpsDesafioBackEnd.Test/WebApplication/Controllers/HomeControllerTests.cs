using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MottuOpsDesafioBackEnd.Domain.Models;
using MottuOpsDesafioBackEnd.WebApplication.Controllers;
using System.Diagnostics;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.WebApplication.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly HomeController _homeController;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _homeController = new HomeController(_mockLogger.Object);
        }

        [Fact(DisplayName = "Index deve retornar uma View")]
        public void Index_ShouldReturnView()
        {
            // Act
            var result = _homeController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact(DisplayName = "Privacy deve retornar uma View")]
        public void Privacy_ShouldReturnView()
        {
            // Act
            var result = _homeController.Privacy();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact(DisplayName = "Error deve retornar uma View com ErrorViewModel")]
        public void Error_ShouldReturnView_WithErrorViewModel()
        {
            // Arrange
            var requestId = "123";

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.TraceIdentifier).Returns(requestId);

            var controllerContext = new ControllerContext
            {
                HttpContext = httpContextMock.Object
            };

            _homeController.ControllerContext = controllerContext;

            // Act
            var result = _homeController.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.Equal(requestId, model.RequestId);
        }
    }
}