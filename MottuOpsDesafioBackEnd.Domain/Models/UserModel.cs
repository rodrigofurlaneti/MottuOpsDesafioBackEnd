namespace MottuOpsDesafioBackEnd.Domain.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int ProfileId { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserProfileModel Profile { get; set; } 
    }
}
