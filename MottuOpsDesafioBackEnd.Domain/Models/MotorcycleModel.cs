namespace MottuOpsDesafioBackEnd.Domain.Models
{
    public class MotorcycleModel
    {
        public int Id { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
    }
}
