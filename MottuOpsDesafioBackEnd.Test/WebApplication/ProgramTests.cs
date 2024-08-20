using Microsoft.Extensions.Hosting;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.WebApplication
{
    public class ProgramTests
    {
        [Fact]
        public void CreateHostBuilder_ReturnsIHostBuilder()
        {
            // Arrange
            var args = new string[] { };

            // Act
            var hostBuilder = Program.CreateHostBuilder(args);

            // Assert
            Assert.NotNull(hostBuilder);
            Assert.IsType<HostBuilder>(hostBuilder);
        }
    }
}
