using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Domain.Models
{
    public class MotorcycleModelTests
    {
        [Fact]
        public void MotorcycleModel_Should_Have_Default_Values()
        {
            // Arrange
            var motorcycle = new MotorcycleModel();

            // Assert
            Assert.Equal(0, motorcycle.Id);
            Assert.Equal(string.Empty, motorcycle.Identifier);
            Assert.Equal(0, motorcycle.Year);
            Assert.Equal(string.Empty, motorcycle.Model);
            Assert.Equal(string.Empty, motorcycle.LicensePlate);
            Assert.Equal(default(DateTime), motorcycle.RegistrationDate);
            Assert.Null(motorcycle.Models);
        }

        [Fact]
        public void MotorcycleModel_Should_SetAndGet_Properties()
        {
            // Arrange
            var motorcycle = new MotorcycleModel
            {
                Id = 1,
                Identifier = "Moto123",
                Year = 2024,
                Model = "Yamaha",
                LicensePlate = "ABC1234",
                RegistrationDate = new DateTime(2024, 8, 20),
                Models = new List<MotorcycleTypeModel>
                {
                    new MotorcycleTypeModel { Id = 1, TypeName = "Esportiva" }
                }
            };

            // Assert
            Assert.Equal(1, motorcycle.Id);
            Assert.Equal("Moto123", motorcycle.Identifier);
            Assert.Equal(2024, motorcycle.Year);
            Assert.Equal("Yamaha", motorcycle.Model);
            Assert.Equal("ABC1234", motorcycle.LicensePlate);
            Assert.Equal(new DateTime(2024, 8, 20), motorcycle.RegistrationDate);
            Assert.NotNull(motorcycle.Models);
            Assert.Single(motorcycle.Models);
            Assert.Equal("Esportiva", motorcycle.Models[0].TypeName);
        }

        [Fact]
        public void MotorcycleModel_Models_Should_BeNull_ByDefault()
        {
            // Arrange
            var motorcycle = new MotorcycleModel();

            // Assert
            Assert.Null(motorcycle.Models);
        }

        [Fact]
        public void MotorcycleModel_Models_Should_Allow_Adding_MotorcycleTypeModel()
        {
            // Arrange
            var motorcycle = new MotorcycleModel
            {
                Models = new List<MotorcycleTypeModel>()
            };

            var motorcycleType = new MotorcycleTypeModel
            {
                Id = 1,
                TypeName = "Esportiva"
            };

            // Act
            motorcycle.Models.Add(motorcycleType);

            // Assert
            Assert.NotNull(motorcycle.Models);
            Assert.Single(motorcycle.Models);
            Assert.Equal(motorcycleType, motorcycle.Models[0]);
        }
    }
}