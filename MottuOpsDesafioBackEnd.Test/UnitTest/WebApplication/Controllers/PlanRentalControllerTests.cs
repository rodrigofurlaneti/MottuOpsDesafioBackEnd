using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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

namespace MottuOpsDesafioBackEnd.Test.UnitTest.WebApplication.Controllers
{
    public class PlanRentalControllerTests
    {
        private readonly Mock<IPlanRentalService> _planRentalServiceMock;
        private readonly PlanRentalController _controller;

        public PlanRentalControllerTests()
        {
            _planRentalServiceMock = new Mock<IPlanRentalService>();
            _controller = new PlanRentalController(_planRentalServiceMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task GetAll_ReturnsViewResult_WithPlanRentalModels()
        {
            // Arrange
            var planRentalModels = new List<PlanRentalModel> { new PlanRentalModel() };
            _planRentalServiceMock.Setup(service => service.GetAllAsync())
                .ReturnsAsync(planRentalModels);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<PlanRentalModel>>(viewResult.ViewData.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenPlanRentalModelIsNull()
        {
            // Act
            var result = await _controller.Post(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("A solicitação do plano é nula", badRequestResult.Value);
        }

        [Fact]
        public async Task Post_ReturnsRedirectToAction_WhenPlanRentalIsInvalid()
        {
            // Arrange
            var planRentalModel = new PlanRentalModel();
            _planRentalServiceMock.Setup(service => service.PostAsync(planRentalModel))
                .ReturnsAsync(0);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _controller.TempData = tempData.Object;

            // Act
            var result = await _controller.Post(planRentalModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Authentication", redirectToActionResult.ControllerName);

            // Verificando se TempData["PlanRentalErro"] foi configurado corretamente
            tempData.VerifySet(t => t["PlanRentalErro"] = "Invalido", Times.Once);
        }

        [Fact]
        public async Task Post_ReturnsRedirectToAction_WhenPlanRentalIsValid()
        {
            // Arrange
            var planRentalModel = new PlanRentalModel();
            _planRentalServiceMock.Setup(service => service.PostAsync(planRentalModel))
                .ReturnsAsync(1);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _controller.TempData = tempData.Object;

            // Act
            var result = await _controller.Post(planRentalModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Authentication", redirectToActionResult.ControllerName);

            // Verificando se TempData["PlanRentalSuccess"] foi configurado corretamente
            tempData.VerifySet(t => t["PlanRentalSuccess"] = "Valido", Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsBadRequest_WhenIdIsZero()
        {
            // Act
            var result = await _controller.GetById(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("A solicitação para atualizar o plano é nula", badRequestResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsViewResult_WhenPlanRentalIsValid()
        {
            // Arrange
            var planRentalModel = new PlanRentalModel();
            _planRentalServiceMock.Setup(service => service.GetByIdAsync(1))
                .ReturnsAsync(planRentalModel);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result.Result);
            var model = Assert.IsAssignableFrom<PlanRentalModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenPlanRentalModelIsNull()
        {
            // Act
            var result = await _controller.Update(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("A solicitação para atualizar o plano é nula", badRequestResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsRedirectToActionResult_WhenPlanRentalIsUpdated()
        {
            // Arrange
            var planRentalModel = new PlanRentalModel();

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _controller.TempData = tempData.Object;

            // Act
            var result = await _controller.Update(planRentalModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Authentication", redirectToActionResult.ControllerName);

            // Verificando se TempData["MotorcycleTypeSuccess"] foi configurado corretamente
            tempData.VerifySet(t => t["PlanRentalUpdateSuccess"] = "Valido", Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenIdIsZero()
        {
            // Act
            var result = await _controller.Delete(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("A solicitação para apagar o plano é nula", badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsRedirectToActionResult_WhenPlanRentalIsDeleted()
        {
            // Arrange

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _controller.TempData = tempData.Object;

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Authentication", redirectToActionResult.ControllerName);

            // Verificando se TempData["PlanRentalDeleteSuccess"] foi configurado corretamente
            tempData.VerifySet(t => t["PlanRentalDeleteSuccess"] = "Valido", Times.Once);
        }
    }
}