using MottuOpsDesafioBackEnd.Domain.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Domain.Models
{
    public class UserProfileModelTests
    {
        [Fact]
        public void UserProfileModel_ProfileNameProperty_IsStringType()
        {
            // Arrange
            var model = new UserProfileModel();

            // Act & Assert
            Assert.IsType<string>(model.ProfileName);
        }

        [Fact]
        public void UserProfileModel_IdProperty_IsIntType()
        {
            // Arrange
            var model = new UserProfileModel();

            // Act & Assert
            Assert.IsType<int>(model.Id);
        }

        [Fact]
        public void UserProfileModel_ValidModel_NoValidationErrors()
        {
            // Arrange
            var model = new UserProfileModel
            {
                Id = 1,
                ProfileName = "Admin"
            };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.Empty(results);
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}