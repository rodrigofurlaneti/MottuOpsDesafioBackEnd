using MottuOpsDesafioBackEnd.Domain.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.Domain.Models
{
    public class PlanRentalModelTests
    {
        [Fact]
        public void PlanRentalModel_IdentifierProperty_IsStringType()
        {
            // Arrange
            var model = new PlanRentalModel();

            // Act & Assert
            Assert.IsType<string>(model.Identifier);
        }

        [Fact]
        public void PlanRentalModel_DaysProperty_IsStringType()
        {
            // Arrange
            var model = new PlanRentalModel();

            // Act & Assert
            Assert.IsType<string>(model.Days);
        }

        [Fact]
        public void PlanRentalModel_ValueProperty_IsStringType()
        {
            // Arrange
            var model = new PlanRentalModel();

            // Act & Assert
            Assert.IsType<string>(model.Value);
        }

        [Fact]
        public void PlanRentalModel_TerminationFineProperty_IsStringType()
        {
            // Arrange
            var model = new PlanRentalModel();

            // Act & Assert
            Assert.IsType<string>(model.TerminationFine);
        }

        [Fact]
        public void PlanRentalModel_RegistrationDateProperty_IsDateTimeType()
        {
            // Arrange
            var model = new PlanRentalModel();

            // Act & Assert
            Assert.IsType<DateTime>(model.RegistrationDate);
        }

        [Fact]
        public void PlanRentalModel_ValidModel_NoValidationErrors()
        {
            // Arrange
            var model = new PlanRentalModel
            {
                Id = 1,
                Identifier = "Plan123",
                Days = "30",
                Value = "100.00",
                TerminationFine = "20.00",
                RegistrationDate = new DateTime(2024, 8, 20)
            };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.Empty(results);
        }

        // Add other tests based on requirements

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}