using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Domain.Models
{
    public class AuthenticationResponseTests
    {
        [Fact]
        public void Id_Should_SetAndGet_Value()
        {
            // Arrange
            var response = new AuthenticationResponse();
            int expectedId = 1;

            // Act
            response.Id = expectedId;

            // Assert
            Assert.Equal(expectedId, response.Id);
        }

        [Fact]
        public void Username_Should_BeInitializedAsEmptyString()
        {
            // Arrange
            var response = new AuthenticationResponse();

            // Act
            var username = response.Username;

            // Assert
            Assert.NotNull(username);
            Assert.Equal(string.Empty, username);
        }

        [Fact]
        public void PasswordHash_Should_BeInitializedAsEmptyString()
        {
            // Arrange
            var response = new AuthenticationResponse();

            // Act
            var passwordHash = response.PasswordHash;

            // Assert
            Assert.NotNull(passwordHash);
            Assert.Equal(string.Empty, passwordHash);
        }

        [Fact]
        public void ProfileId_Should_BeInitializedAsZero()
        {
            // Arrange
            var response = new AuthenticationResponse();

            // Act
            var profileId = response.ProfileId;

            // Assert
            Assert.Equal(0, profileId);
        }

        [Fact]
        public void CourierId_Should_BeInitializedAsZero()
        {
            // Arrange
            var response = new AuthenticationResponse();

            // Act
            var courierId = response.CourierId;

            // Assert
            Assert.Equal(0, courierId);
        }

        [Fact]
        public void Username_Should_SetAndGet_NullableValue()
        {
            // Arrange
            var response = new AuthenticationResponse();
            string? expectedUsername = "testUser";

            // Act
            response.Username = expectedUsername;

            // Assert
            Assert.Equal(expectedUsername, response.Username);
        }

        [Fact]
        public void PasswordHash_Should_SetAndGet_NullableValue()
        {
            // Arrange
            var response = new AuthenticationResponse();
            string? expectedPasswordHash = "hashedPassword";

            // Act
            response.PasswordHash = expectedPasswordHash;

            // Assert
            Assert.Equal(expectedPasswordHash, response.PasswordHash);
        }

        [Fact]
        public void ProfileId_Should_SetAndGet_NullableValue()
        {
            // Arrange
            var response = new AuthenticationResponse();
            int? expectedProfileId = 5;

            // Act
            response.ProfileId = expectedProfileId;

            // Assert
            Assert.Equal(expectedProfileId, response.ProfileId);
        }

        [Fact]
        public void CourierId_Should_SetAndGet_NullableValue()
        {
            // Arrange
            var response = new AuthenticationResponse();
            int? expectedCourierId = 3;

            // Act
            response.CourierId = expectedCourierId;

            // Assert
            Assert.Equal(expectedCourierId, response.CourierId);
        }
    }
}