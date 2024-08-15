namespace MottuOpsDesafioBackEnd.Domain.Models
{
    public class RegisteredMotorcycleModel
    {
        public int Id { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public DateTime ReceivedDate { get; set; }
    }
}
