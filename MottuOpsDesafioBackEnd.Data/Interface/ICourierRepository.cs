﻿using MottuOpsDesafioBackEnd.Domain.Models;

namespace MottuOpsDesafioBackEnd.Data.Interface
{
    public interface ICourierRepository
    {
        Task<int> PostAsync(CourierModel courierModel);
        Task<CourierModel> GetByIdAsync(int courierId);
        Task PutCnhAsync(CourierModel courierModel);
        Task<bool> GetByCnpjAsync(string cnpj);
        Task<bool> GetByCnhAsync(string cnh);
    }
}
