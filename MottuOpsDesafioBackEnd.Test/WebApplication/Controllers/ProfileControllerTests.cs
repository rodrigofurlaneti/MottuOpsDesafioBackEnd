using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using MottuOpsDesafioBackEnd.WebApplication.Controllers;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.WebApplication.Controllers
{
    public class ProfileControllerTests
    {

        private readonly Mock<IProfileRepository> _profileRepositoryMock;
        private readonly ProfileController _controller;

        public ProfileControllerTests()
        {
            _profileRepositoryMock = new Mock<IProfileRepository>();
            _controller = new ProfileController(_profileRepositoryMock.Object);
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task GetAll_ReturnsViewResult_WithUserProfiles()
        {
            // Arrange
            var userProfiles = new List<UserProfileModel> { new UserProfileModel() };
            _profileRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(userProfiles);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<UserProfileModel>>(viewResult.ViewData.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenUserProfileModelIsNull()
        {
            // Act
            var result = await _controller.Post(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("A solicitação do perfil de usuário é nula", badRequestResult.Value);
        }

        [Fact]
        public async Task Post_ReturnsRedirectToAction_WhenUserProfileIsInvalid()
        {
            // Arrange
            var userProfileModel = new UserProfileModel();
            _profileRepositoryMock.Setup(repo => repo.PostAsync(userProfileModel))
                .ReturnsAsync(0);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _controller.TempData = tempData.Object;

            // Act
            var result = await _controller.Post(userProfileModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Authentication", redirectToActionResult.ControllerName);

            tempData.VerifySet(t => t["UserProfileErro"] = "Invalido", Times.Once);
        }

        [Fact]
        public async Task Post_ReturnsRedirectToAction_WhenUserProfileIsValid()
        {
            // Arrange
            var userProfileModel = new UserProfileModel();
            _profileRepositoryMock.Setup(repo => repo.PostAsync(userProfileModel))
                .ReturnsAsync(1);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _controller.TempData = tempData.Object;

            // Act
            var result = await _controller.Post(userProfileModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Authentication", redirectToActionResult.ControllerName);

            tempData.VerifySet(t => t["UserProfileSuccess"] = "Valido", Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsBadRequest_WhenIdIsZero()
        {
            // Act
            var result = await _controller.GetById(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("A solicitação para atualizar o perfil do usuário é nula", badRequestResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsViewResult_WhenUserProfileIsValid()
        {
            // Arrange
            var userProfileModel = new UserProfileModel();
            _profileRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(userProfileModel);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result.Result);
            var model = Assert.IsAssignableFrom<UserProfileModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenUserProfileModelIsNull()
        {
            // Act
            var result = await _controller.Update(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("A solicitação do perfil do usuário é nula", badRequestResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsRedirectToActionResult_WhenUserProfileIsUpdated()
        {
            // Arrange
            var userProfileModel = new UserProfileModel();

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _controller.TempData = tempData.Object;

            // Act
            var result = await _controller.Update(userProfileModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Authentication", redirectToActionResult.ControllerName);

            tempData.VerifySet(t => t["UserProfileUpdateSuccess"] = "Valido", Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenIdIsZero()
        {
            // Act
            var result = await _controller.Delete(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("A solicitação para apagar o perfil do usuário é nula", badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsRedirectToActionResult_WhenUserProfileIsDeleted()
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

            tempData.VerifySet(t => t["UserProfileDeleteSuccess"] = "Valido", Times.Once);
        }
    }
}