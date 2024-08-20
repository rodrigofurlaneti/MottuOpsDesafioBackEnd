using MottuOpsDesafioBackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Domain.Models
{
    public class CourierModelTests
    {
        [Fact]
        public void Id_Should_SetAndGet_Value()
        {
            // Arrange
            var courier = new CourierModel();
            int expectedId = 1;

            // Act
            courier.Id = expectedId;

            // Assert
            Assert.Equal(expectedId, courier.Id);
        }

        [Fact]
        public void Identifier_Should_BeInitializedAsEmptyString()
        {
            // Arrange
            var courier = new CourierModel();

            // Act
            var identifier = courier.Identifier;

            // Assert
            Assert.NotNull(identifier);
            Assert.Equal(string.Empty, identifier);
        }

        [Fact]
        public void Name_Should_BeInitializedAsEmptyString()
        {
            // Arrange
            var courier = new CourierModel();

            // Act
            var name = courier.Name;

            // Assert
            Assert.NotNull(name);
            Assert.Equal(string.Empty, name);
        }

        [Fact]
        public void CNPJ_Should_BeInitializedAsEmptyString()
        {
            // Arrange
            var courier = new CourierModel();

            // Act
            var cnpj = courier.CNPJ;

            // Assert
            Assert.NotNull(cnpj);
            Assert.Equal(string.Empty, cnpj);
        }

        [Fact]
        public void BirthDate_Should_SetAndGet_Value()
        {
            // Arrange
            var courier = new CourierModel();
            DateTime expectedBirthDate = new DateTime(1990, 1, 1);

            // Act
            courier.BirthDate = expectedBirthDate;

            // Assert
            Assert.Equal(expectedBirthDate, courier.BirthDate);
        }

        [Fact]
        public void CNHNumber_Should_BeInitializedAsEmptyString()
        {
            // Arrange
            var courier = new CourierModel();

            // Act
            var cnhNumber = courier.CNHNumber;

            // Assert
            Assert.NotNull(cnhNumber);
            Assert.Equal(string.Empty, cnhNumber);
        }

        [Fact]
        public void CNHType_Should_BeInitializedAsEmptyString()
        {
            // Arrange
            var courier = new CourierModel();

            // Act
            var cnhType = courier.CNHType;

            // Assert
            Assert.NotNull(cnhType);
            Assert.Equal(string.Empty, cnhType);
        }

        [Fact]
        public void CNHImagePath_Should_BeInitializedAsEmptyString()
        {
            // Arrange
            var courier = new CourierModel();

            // Act
            var cnhImagePath = courier.CNHImagePath;

            // Assert
            Assert.NotNull(cnhImagePath);
            Assert.Equal(string.Empty, cnhImagePath);
        }

        [Fact]
        public void CNHImagePathFormFile_Should_BeNull_ByDefault()
        {
            // Arrange
            var courier = new CourierModel();

            // Act
            var cnhImagePathFormFile = courier.CNHImagePathFormFile;

            // Assert
            Assert.Null(cnhImagePathFormFile);
        }

        [Fact]
        public void RegistrationDate_Should_SetAndGet_Value()
        {
            // Arrange
            var courier = new CourierModel();
            DateTime expectedRegistrationDate = DateTime.Now;

            // Act
            courier.RegistrationDate = expectedRegistrationDate;

            // Assert
            Assert.Equal(expectedRegistrationDate, courier.RegistrationDate);
        }
    }
}
