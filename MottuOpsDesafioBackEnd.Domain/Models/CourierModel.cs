using System.ComponentModel.DataAnnotations;

namespace MottuOpsDesafioBackEnd.Domain.Models
{
    public class CourierModel : UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Identificador é obrigatório.")]
        public string Identifier { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo CNPJ é obrigatório.")]
        public string CNPJ { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Data de Nascimento é obrigatório.")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "O campo Número da CNH é obrigatório.")]
        public string CNHNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Tipo da CNH é obrigatório.")]
        public string CNHType { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Imagem da CNH é obrigatório.")]
        public string CNHImagePath { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; }
    }

}
