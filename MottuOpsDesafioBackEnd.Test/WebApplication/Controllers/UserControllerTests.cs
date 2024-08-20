using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using MottuOpsDesafioBackEnd.WebApplication.Controllers;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.WebApplication.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IProfileService> _profileServiceMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _profileServiceMock = new Mock<IProfileService>();
            _controller = new UserController(_userServiceMock.Object, _profileServiceMock.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithUserModel()
        {
            // Arrange
            var profiles = new List<UserProfileModel> { new UserProfileModel() };
            _profileServiceMock.Setup(service => service.GetAllAsync())
                .ReturnsAsync(profiles);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<UserModel>(viewResult.ViewData.Model);
            Assert.Equal(profiles.Count, model.Profiles.Count);
        }

        [Fact]
        public async Task GetAll_ReturnsViewResult_WithUserModels()
        {
            // Arrange
            var users = new List<UserModel> { new UserModel() };
            _userServiceMock.Setup(service => service.GetAllAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<UserModel>>(viewResult.ViewData.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenUserModelIsNull()
        {
            // Act
            var result = await _controller.Post(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("A solicitação do usuário é nula", badRequestResult.Value);
        }

        [Fact]
        public async Task Post_ReturnsRedirectToAction_WhenUserProfileIsInvalid()
        {
            // Arrange
            var userModel = new UserModel();
            _userServiceMock.Setup(service => service.PostAsync(userModel))
                .ReturnsAsync(0);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _controller.TempData = tempData.Object;

            // Act
            var result = await _controller.Post(userModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Authentication", redirectToActionResult.ControllerName);

            tempData.VerifySet(t => t["UserErro"] = "Invalido", Times.Once);
        }

        [Fact]
        public async Task Post_ReturnsRedirectToAction_WhenUserProfileIsValid()
        {
            // Arrange
            var userModel = new UserModel();
            _userServiceMock.Setup(service => service.PostAsync(userModel))
                .ReturnsAsync(1);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _controller.TempData = tempData.Object;

            // Act
            var result = await _controller.Post(userModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Authentication", redirectToActionResult.ControllerName);

            tempData.VerifySet(t => t["UserSuccess"] = "Valido", Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsBadRequest_WhenIdIsZero()
        {
            // Act
            var result = await _controller.GetById(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("A solicitação para atualizar o usuário é nula", badRequestResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsRedirectToAction_WhenUserIsNull()
        {
            // Arrange
            _userServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((UserModel)null);

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _controller.TempData = tempData.Object;

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result.Result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Authentication", redirectToActionResult.ControllerName);

            tempData.VerifySet(t => t["UserErro"] = "Invalido", Times.Once);
        }

        [Fact]
        public async Task GetById_ReturnsViewResult_WithUserModel()
        {
            // Arrange
            var userModel = new UserModel();
            var profiles = new List<UserProfileModel> { new UserProfileModel() };

            _userServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(userModel);
            _profileServiceMock.Setup(service => service.GetAllAsync())
                .ReturnsAsync(profiles);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result.Result);
            var model = Assert.IsAssignableFrom<UserModel>(viewResult.ViewData.Model);
            Assert.Equal(userModel, model);
            Assert.Equal(profiles.Count, model.Profiles.Count);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenUserModelIsNull()
        {
            // Act
            var result = await _controller.Update(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("A solicitação do usuário é nula", badRequestResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsRedirectToActionResult_WhenUserProfileIsUpdated()
        {
            // Arrange
            var userModel = new UserModel();

            // Mock TempData
            var tempData = new Mock<ITempDataDictionary>();
            _controller.TempData = tempData.Object;

            // Act
            var result = await _controller.Update(userModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Authentication", redirectToActionResult.ControllerName);

            tempData.VerifySet(t => t["UserUpdateSuccess"] = "Valido", Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenIdIsZero()
        {
            // Act
            var result = await _controller.Delete(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("A solicitação para apagar o usuário é nula", badRequestResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsRedirectToActionResult_WhenUserIsDeleted()
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

            tempData.VerifySet(t => t["UserDeleteSuccess"] = "Valido", Times.Once);
        }
    }
}