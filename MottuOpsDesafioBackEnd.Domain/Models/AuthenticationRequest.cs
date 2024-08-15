using System.ComponentModel.DataAnnotations;

namespace MottuOpsDesafioBackEnd.Domain.Models
{
    public class AuthenticationRequest
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password { get; set; } = string.Empty ;
    }
}
