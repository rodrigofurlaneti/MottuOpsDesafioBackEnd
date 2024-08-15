namespace MottuOpsDesafioBackEnd.Domain.Models
{
    public class AuthenticationResponse
    {
        public int Id { get; set; }
        public string? Username { get; set; } = string.Empty;
        public string? PasswordHash { get; set; } = string.Empty;
        public int? ProfileId { get; set; }
    }
}
