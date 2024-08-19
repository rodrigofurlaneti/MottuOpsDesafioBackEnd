using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Business.Interface
{
    public interface ICourierService
    {
        Task<int> PostAsync(CourierModel courierModel);
        Task<bool> GetByCnpjAsync(string cnpj);
        Task<bool> GetByCnhAsync(string cnh);
    }
}
