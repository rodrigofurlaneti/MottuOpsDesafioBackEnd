using MottuOpsDesafioBackEnd.Domain.Models;
using Xunit;

namespace MottuOpsDesafioBackEnd.Test.Domain.Models
{
    public class MotorcycleRentalModelTests
    {
        [Fact]
        public void MotorcycleRentalModel_Should_Have_Default_Values()
        {
            // Arrange
            var rental = new MotorcycleRentalModel();

            // Assert
            Assert.Equal(0, rental.Id);
            Assert.Equal(0, rental.CourierId);
            Assert.Equal(0, rental.MotorcycleId);
            Assert.Equal(default(DateTime), rental.StartDate);
            Assert.Equal(default(DateTime), rental.EndDate);
            Assert.Equal(default(DateTime), rental.ExpectedEndDate);
            Assert.Equal(0m, rental.DailyRate);
            Assert.Equal(default(DateTime), rental.CreatedAt);
            Assert.Null(rental.Courier);
            Assert.Null(rental.Plans);
            Assert.NotNull(rental.Motorcycles);
            Assert.Empty(rental.Motorcycles);
            Assert.NotNull(rental.PlansRental);
            Assert.Empty(rental.PlansRental);
        }

        [Fact]
        public void MotorcycleRentalModel_Should_SetAndGet_Properties()
        {
            // Arrange
            var rental = new MotorcycleRentalModel
            {
                Id = 1,
                CourierId = 2,
                MotorcycleId = 3,
                StartDate = new DateTime(2024, 8, 1),
                EndDate = new DateTime(2024, 8, 15),
                ExpectedEndDate = new DateTime(2024, 8, 14),
                PlanType = "Mensal",
                DailyRate = 100.50m,
                CreatedAt = new DateTime(2024, 8, 1),
                Courier = new CourierModel { Id = 2, Name = "Entregador 1" },
                Plans = new PlanRentalModel { Id = 1, Identifier = "Plano 1" },
                Motorcycles = new List<MotorcycleModel>
                {
                    new MotorcycleModel { Id = 3, Model = "Yamaha" }
                },
                PlansRental = new List<PlanRentalModel>
                {
                    new PlanRentalModel { Id = 1, Identifier = "Plano 1" }
                }
            };

            // Assert
            Assert.Equal(1, rental.Id);
            Assert.Equal(2, rental.CourierId);
            Assert.Equal(3, rental.MotorcycleId);
            Assert.Equal(new DateTime(2024, 8, 1), rental.StartDate);
            Assert.Equal(new DateTime(2024, 8, 15), rental.EndDate);
            Assert.Equal(new DateTime(2024, 8, 14), rental.ExpectedEndDate);
            Assert.Equal("Mensal", rental.PlanType);
            Assert.Equal(100.50m, rental.DailyRate);
            Assert.Equal(new DateTime(2024, 8, 1), rental.CreatedAt);
            Assert.NotNull(rental.Courier);
            Assert.Equal(2, rental.Courier.Id);
            Assert.NotNull(rental.Plans);
            Assert.Equal(1, rental.Plans.Id);
            Assert.NotNull(rental.Motorcycles);
            Assert.Single(rental.Motorcycles);
            Assert.NotNull(rental.PlansRental);
            Assert.Single(rental.PlansRental);
        }

        [Fact]
        public void MotorcycleRentalModel_Motorcycles_Should_BeEmpty_ByDefault()
        {
            // Arrange
            var rental = new MotorcycleRentalModel();

            // Assert
            Assert.NotNull(rental.Motorcycles);
            Assert.Empty(rental.Motorcycles);
        }

        [Fact]
        public void MotorcycleRentalModel_PlansRental_Should_BeEmpty_ByDefault()
        {
            // Arrange
            var rental = new MotorcycleRentalModel();

            // Assert
            Assert.NotNull(rental.PlansRental);
            Assert.Empty(rental.PlansRental);
        }

        [Fact]
        public void MotorcycleRentalModel_Should_Allow_Adding_Motorcycles()
        {
            // Arrange
            var rental = new MotorcycleRentalModel
            {
                Motorcycles = new List<MotorcycleModel>()
            };

            var listMotorcycle = new List<MotorcycleModel>
            {
                new MotorcycleModel()
                {
                    Id = 1,
                    Model = "Honda"
                }
            };

            rental.Motorcycles = listMotorcycle;

            // Act
            rental.Motorcycles.GetEnumerator().MoveNext(); 

            // Assert
            Assert.NotNull(rental.Motorcycles);
            Assert.Equal(listMotorcycle, rental.Motorcycles);
        }

        [Fact]
        public void MotorcycleRentalModel_Should_Allow_Adding_PlansRental()
        {
            // Arrange
            var rental = new MotorcycleRentalModel
            {
                PlansRental = new List<PlanRentalModel>()
            };

            var listPlanRental = new List<PlanRentalModel>()
            {
                new PlanRentalModel
                {
                    Id = 1,
                    Identifier = "Plano Semanal"
                }
            };

            // Act
            rental.PlansRental.GetEnumerator().MoveNext(); 
            rental.PlansRental = listPlanRental;

            // Assert
            Assert.NotNull(rental.PlansRental);
            Assert.Equal(listPlanRental, rental.PlansRental);
        }
    }
}