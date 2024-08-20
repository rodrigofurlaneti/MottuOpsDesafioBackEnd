namespace MottuOpsDesafioBackEnd.Domain.Models
{
    public class PlanRentalModel
    {
        public int Id { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public string Days { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string TerminationFine { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
    }
}
