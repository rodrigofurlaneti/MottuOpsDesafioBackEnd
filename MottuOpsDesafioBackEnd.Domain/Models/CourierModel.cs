namespace MottuOpsDesafioBackEnd.Domain.Models
{
    public class CourierModel
    {
        public int Id { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string CNHNumber { get; set; } = string.Empty;
        public string CNHType { get; set; } = string.Empty;
        public string CNHImagePath { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
    }

}
