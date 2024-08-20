using MottuOpsDesafioBackEnd.Domain.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Domain.Models
{
    public class UserModelTests
    {
        [Fact]
        public void UserModel_UsernameProperty_IsStringType()
        {
            // Arrange
            var model = new UserModel();

            // Act & Assert
            Assert.IsType<string>(model.Username);
        }

        [Fact]
        public void UserModel_PasswordHashProperty_IsStringType()
        {
            // Arrange
            var model = new UserModel();

            // Act & Assert
            Assert.IsType<string>(model.PasswordHash);
        }

        [Fact]
        public void UserModel_ProfileIdProperty_IsIntType()
        {
            // Arrange
            var model = new UserModel();

            // Act & Assert
            Assert.IsType<int>(model.ProfileId);
        }

        [Fact]
        public void UserModel_ProfileNameProperty_IsStringType()
        {
            // Arrange
            var model = new UserModel();

            // Act & Assert
            Assert.IsType<string>(model.ProfileName);
        }

        [Fact]
        public void UserModel_CreatedAtProperty_IsDateTimeType()
        {
            // Arrange
            var model = new UserModel();

            // Act & Assert
            Assert.IsType<DateTime>(model.CreatedAt);
        }

        [Fact]
        public void UserModel_Username_IsRequired()
        {
            // Arrange
            var model = new UserModel { PasswordHash = "password123" };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.Contains(results, v => v.ErrorMessage == "O campo usuário é obrigatório.");
        }

        [Fact]
        public void UserModel_PasswordHash_IsRequired()
        {
            // Arrange
            var model = new UserModel { Username = "user123" };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.Contains(results, v => v.ErrorMessage == "O campo senha é obrigatório.");
        }

        [Fact]
        public void UserModel_ValidModel_NoValidationErrors()
        {
            // Arrange
            var model = new UserModel
            {
                Id = 1,
                Username = "user123",
                PasswordHash = "password123",
                ProfileId = 1,
                ProfileName = "Admin",
                CreatedAt = new DateTime(2024, 8, 20),
                Profiles = new List<UserProfileModel>()
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