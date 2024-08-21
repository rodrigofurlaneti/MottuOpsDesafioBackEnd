using MottuOpsDesafioBackEnd.Domain.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.Domain.Models
{
    public class RegisteredMotorcycleModelTests
    {
        [Fact]
        public void RegisteredMotorcycleModel_IdentifierProperty_IsStringType()
        {
            // Arrange
            var model = new RegisteredMotorcycleModel();

            // Act & Assert
            Assert.IsType<string>(model.Identifier);
        }

        [Fact]
        public void RegisteredMotorcycleModel_ModelProperty_IsStringType()
        {
            // Arrange
            var model = new RegisteredMotorcycleModel();

            // Act & Assert
            Assert.IsType<string>(model.Model);
        }

        [Fact]
        public void RegisteredMotorcycleModel_LicensePlateProperty_IsStringType()
        {
            // Arrange
            var model = new RegisteredMotorcycleModel();

            // Act & Assert
            Assert.IsType<string>(model.LicensePlate);
        }

        [Fact]
        public void RegisteredMotorcycleModel_YearProperty_IsIntType()
        {
            // Arrange
            var model = new RegisteredMotorcycleModel();

            // Act & Assert
            Assert.IsType<int>(model.Year);
        }

        [Fact]
        public void RegisteredMotorcycleModel_ReceivedDateProperty_IsDateTimeType()
        {
            // Arrange
            var model = new RegisteredMotorcycleModel();

            // Act & Assert
            Assert.IsType<DateTime>(model.ReceivedDate);
        }

        [Fact]
        public void RegisteredMotorcycleModel_ValidModel_NoValidationErrors()
        {
            // Arrange
            var model = new RegisteredMotorcycleModel
            {
                Id = 1,
                Identifier = "Motorcycle123",
                Year = 2024,
                Model = "ModelX",
                LicensePlate = "ABC1234",
                ReceivedDate = new DateTime(2024, 8, 20)
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