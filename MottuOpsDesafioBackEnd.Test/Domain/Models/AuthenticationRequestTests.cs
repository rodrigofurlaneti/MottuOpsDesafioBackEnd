using MottuOpsDesafioBackEnd.Domain.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Domain.Models
{
    public class AuthenticationRequestTests
    {
        [Fact]
        public void AuthenticationRequest_UsernameIsRequired()
        {
            // Arrange
            var model = new AuthenticationRequest { Password = "password123" };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.Contains(results, v => v.ErrorMessage == "O nome de usuário é obrigatório.");
        }

        [Fact]
        public void AuthenticationRequest_PasswordIsRequired()
        {
            // Arrange
            var model = new AuthenticationRequest { Username = "username" };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.Contains(results, v => v.ErrorMessage == "A senha é obrigatória.");
        }

        [Fact]
        public void AuthenticationRequest_ValidModel_NoValidationErrors()
        {
            // Arrange
            var model = new AuthenticationRequest { Username = "username", Password = "password123" };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void AuthenticationRequest_UsernameProperty_IsStringType()
        {
            // Arrange
            var model = new AuthenticationRequest();

            // Act & Assert
            Assert.IsType<string>(model.Username);
        }

        [Fact]
        public void AuthenticationRequest_PasswordProperty_IsStringType()
        {
            // Arrange
            var model = new AuthenticationRequest();

            // Act & Assert
            Assert.IsType<string>(model.Password);
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
