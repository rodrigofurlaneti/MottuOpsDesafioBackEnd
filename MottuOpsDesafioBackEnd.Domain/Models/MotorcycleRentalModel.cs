namespace MottuOpsDesafioBackEnd.Domain.Models
{
    public class MotorcycleRentalModel
    {
        public int Id { get; set; }
        public int CourierId { get; set; }
        public int MotorcycleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public string PlanType { get; set; }
        public decimal DailyRate { get; set; }
        public DateTime CreatedAt { get; set; }
        public CourierModel Courier { get; set; }
        public MotorcycleModel Motorcycle { get; set; } = new MotorcycleModel();
    }
}
