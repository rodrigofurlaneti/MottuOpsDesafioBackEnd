using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Domain.Models
{
    public class ErrorViewModelTests
    {
        [Fact]
        public void RequestId_Should_BeNull_ByDefault()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel();

            // Act
            var requestId = errorViewModel.RequestId;

            // Assert
            Assert.Null(requestId);
        }

        [Fact]
        public void RequestId_Should_SetAndGet_Value()
        {
            // Arrange
            var errorViewModel = new ErrorViewModel();
            string expectedRequestId = "12345";

            // Act
            errorViewModel.RequestId = expectedRequestId;

            // Assert
            Assert.Equal(expectedRequestId, errorViewModel.RequestId);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("12345", true)]
        public void ShowRequestId_Should_Return_ExpectedValue(string requestId, bool expected)
        {
            // Arrange
            var errorViewModel = new ErrorViewModel
            {
                RequestId = requestId
            };

            // Act
            var result = errorViewModel.ShowRequestId;

            // Assert
            Assert.Equal(expected, result);
        }
    }
}