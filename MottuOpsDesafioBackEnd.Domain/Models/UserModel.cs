using System.ComponentModel.DataAnnotations;

namespace MottuOpsDesafioBackEnd.Domain.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo usuário é obrigatório.")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "O campo senha é obrigatório.")]
        public string PasswordHash { get; set; } = string.Empty;
        public int ProfileId { get; set; }
        public string ProfileName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<UserProfileModel> Profiles { get; set; }

    }
}
