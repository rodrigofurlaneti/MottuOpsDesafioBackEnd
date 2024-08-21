using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Data.Interface;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.UnitTest.WebApplication
{
    public class StartupTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public StartupTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Production"); // Simula o ambiente de Produção
            });
        }

        [Fact]
        public void ConfigureServices_RegistersAllDependencies()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;

                // Verifica se todos os serviços foram registrados corretamente
                Assert.NotNull(services.GetService<IAuthenticationService>());
                Assert.NotNull(services.GetService<ICourierService>());
                Assert.NotNull(services.GetService<IProfileService>());
                Assert.NotNull(services.GetService<IPlanRentalService>());
                Assert.NotNull(services.GetService<IMotorcycleService>());
                Assert.NotNull(services.GetService<IMotorcycleRentalService>());
                Assert.NotNull(services.GetService<IMotorcycleTypeService>());
                Assert.NotNull(services.GetService<IUserService>());

                Assert.NotNull(services.GetService<IAuthenticationRepository>());
                Assert.NotNull(services.GetService<ICourierRepository>());
                Assert.NotNull(services.GetService<IMotorcycleRepository>());
                Assert.NotNull(services.GetService<IMotorcycleRentalRepository>());
                Assert.NotNull(services.GetService<IMotorcycleTypeRepository>());
                Assert.NotNull(services.GetService<IProfileRepository>());
                Assert.NotNull(services.GetService<IPlanRentalRepository>());
                Assert.NotNull(services.GetService<IUserRepository>());
            }
        }
    }
}