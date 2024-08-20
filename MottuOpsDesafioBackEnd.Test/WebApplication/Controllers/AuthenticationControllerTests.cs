using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using MottuOpsDesafioBackEnd.Test.Helpers;
using MottuOpsDesafioBackEnd.WebApplication.Controllers;
using System.Text;
using System.Text.Json;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.WebApplication.Controllers
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;
        private readonly AuthenticationController _authenticationController;

        public AuthenticationControllerTests()
        {
            _mockAuthenticationService = new Mock<IAuthenticationService>();
            _authenticationController = new AuthenticationController(_mockAuthenticationService.Object);
        }

        [Fact(DisplayName = "Index deve retornar uma View")]
        public void Index_ShouldReturnView()
        {
            // Act
            var result = _authenticationController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact(DisplayName = "Dashboard deve retornar BadRequest se AuthenticationRequest for nulo")]
        public async Task Dashboard_ShouldReturnBadRequest_WhenAuthenticationRequestIsNull()
        {
            // Act
            var result = await _authenticationController.Dashboard(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("A solicitação de autenticação é nula", badRequestResult.Value);
        }

        [Fact(DisplayName = "Dashboard deve redirecionar para Index se autenticação falhar")]
        public async Task Dashboard_ShouldRedirectToIndex_WhenAuthenticationFails()
        {
            // Arrange
            var authRequest = new AuthenticationRequest { Username = "user", Password = "password" };
            _mockAuthenticationService
                .Setup(service => service.PostAsync(authRequest))
                .ReturnsAsync((AuthenticationResponse)null);

            var tempData = new Mock<ITempDataDictionary>();
            tempData.Setup(t => t["AuthenticationErro"]).Returns("Invalido");
            _authenticationController.TempData = tempData.Object;

            // Act
            var result = await _authenticationController.Dashboard(authRequest);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Home", redirectResult.ControllerName);
            tempData.VerifySet(t => t["AuthenticationErro"] = "Invalido");
        }

        [Fact(DisplayName = "Dashboard deve salvar a resposta da autenticação na sessão e retornar a View")]
        public async Task Dashboard_ShouldSaveAuthResponseInSession_AndReturnView_WhenAuthenticationSucceeds()
        {
            // Arrange
            var authRequest = new AuthenticationRequest { Username = "user", Password = "password" };
            var authResponse = new AuthenticationResponse { Id = 1, Username = "user", ProfileId = 1 };

            _mockAuthenticationService
                .Setup(service => service.PostAsync(authRequest))
                .ReturnsAsync(authResponse);

            var session = new TestSession();

            var httpContext = new DefaultHttpContext
            {
                Session = session
            };

            _authenticationController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var tempData = new Mock<ITempDataDictionary>();
            _authenticationController.TempData = tempData.Object;

            // Act
            var result = await _authenticationController.Dashboard(authRequest);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result.Result);
            Assert.Null(viewResult.ViewName);

            // Verificando se o valor foi salvo na sessão
            session.TryGetValue("AuthResponse", out var sessionValue);
            var savedAuthResponse = JsonSerializer.Deserialize<AuthenticationResponse>(System.Text.Encoding.UTF8.GetString(sessionValue));
            Assert.Equal(authResponse.Id, savedAuthResponse.Id);

            // Verificando TempData
            tempData.VerifySet(t => t["AuthenticationSuccess"] = "Valido", Times.Once);
        }

        [Fact(DisplayName = "Dashboard deve retornar StatusCode 500 se ocorrer uma exceção")]
        public async Task Dashboard_ShouldReturnStatusCode500_WhenExceptionOccurs()
        {
            // Arrange
            var authRequest = new AuthenticationRequest { Username = "user", Password = "password" };
            _mockAuthenticationService
                .Setup(service => service.PostAsync(authRequest))
                .ThrowsAsync(new Exception("Erro durante a autenticação"));

            // Act
            var result = await _authenticationController.Dashboard(authRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Erro do Servidor Interno", statusCodeResult.Value);
        }
    }
}