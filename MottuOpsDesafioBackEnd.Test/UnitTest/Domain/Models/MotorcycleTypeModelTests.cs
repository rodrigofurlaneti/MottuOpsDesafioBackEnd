using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.Domain.Models
{
    public class MotorcycleTypeModelTests
    {
        [Fact]
        public void MotorcycleTypeModel_Should_Have_Default_Values()
        {
            // Arrange
            var motorcycleType = new MotorcycleTypeModel();

            // Assert
            Assert.Equal(0, motorcycleType.Id);
            Assert.Equal(string.Empty, motorcycleType.TypeName);
        }

        [Fact]
        public void MotorcycleTypeModel_Should_SetAndGet_Properties()
        {
            // Arrange
            var motorcycleType = new MotorcycleTypeModel
            {
                Id = 1,
                TypeName = "Sport"
            };

            // Assert
            Assert.Equal(1, motorcycleType.Id);
            Assert.Equal("Sport", motorcycleType.TypeName);
        }

        [Fact]
        public void MotorcycleTypeModel_Should_Allow_Empty_TypeName()
        {
            // Arrange
            var motorcycleType = new MotorcycleTypeModel
            {
                Id = 1,
                TypeName = string.Empty
            };

            // Assert
            Assert.Equal(1, motorcycleType.Id);
            Assert.Equal(string.Empty, motorcycleType.TypeName);
        }
    }
}