namespace MottuOpsDesafioBackEnd.Domain.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int ProfileId { get; set; }
        public string ProfileName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<UserProfileModel> Profiles { get; set; }

    }
}
