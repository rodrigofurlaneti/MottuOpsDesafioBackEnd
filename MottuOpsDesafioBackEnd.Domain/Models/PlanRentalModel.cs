namespace MottuOpsDesafioBackEnd.Domain.Models
{
    public class PlanRentalModel
    {
        public int Id { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public string Days { get; set; } = string.Empty;
        public decimal Value { get; set; } = 0.00m;
        public decimal TerminationFine { get; set; } = 0.00m;
        public DateTime RegistrationDate { get; set; }
    }
}
