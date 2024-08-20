using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using System.Data;

namespace MottuOpsDesafioBackEnd.Data.Repository
{
    public class MotorcycleRentalRepository : IMotorcycleRentalRepository
    {
        private readonly string _connectionString;

        public MotorcycleRentalRepository(IConfiguration configuration)
        {
            // Garante que _connectionString seja inicializado corretamente
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "A string de conexão não pode ser nula");
        }

        public async Task<MotorcycleRentalModel> GetByCourierIdAsync(int courierId)
        {
            string storedProcedureName = "Mottu_Procedure_MotorcycleRental_GetByCourierId";

            MotorcycleRentalModel motorcycleRentalModel = new MotorcycleRentalModel();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CourierId", courierId);

                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                motorcycleRentalModel = CreateMotorcycleRentalsFromReader(reader);
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.Error.WriteLine($"Erro de SQL: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro: {ex.Message}");
                throw;
            }

            return motorcycleRentalModel;
        }

        public async Task<int> PostAsync(MotorcycleRentalModel motorcycleRentalModel)
        {
            string storedProcedureName = "Mottu_Procedure_MotorcycleRentals_Insert";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CourierId", motorcycleRentalModel.CourierId);

                        command.Parameters.AddWithValue("@MotorcycleId", motorcycleRentalModel.MotorcycleId);

                        command.Parameters.AddWithValue("@StartDate", motorcycleRentalModel.StartDate);

                        command.Parameters.AddWithValue("@EndDate", motorcycleRentalModel.EndDate);

                        command.Parameters.AddWithValue("@ExpectedEndDate", motorcycleRentalModel.ExpectedEndDate);

                        command.Parameters.AddWithValue("@PlanType", motorcycleRentalModel.PlanType);

                        command.Parameters.AddWithValue("@DailyRate", motorcycleRentalModel.DailyRate);

                        await connection.OpenAsync();

                        var result = await command.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int motorcycleId))
                        {
                            return motorcycleId;
                        }
                        else
                        {
                            throw new Exception("Falha ao recuperar o novo ID da moto.");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.Error.WriteLine($"Erro de SQL: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro: {ex.Message}");
                throw;
            }
        }


        private MotorcycleRentalModel CreateMotorcycleRentalsFromReader(SqlDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
                Console.WriteLine(reader.GetName(i) + " - " + reader.GetFieldType(i));

            return new MotorcycleRentalModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                CourierId = reader.GetInt32(reader.GetOrdinal("CourierId")),
                MotorcycleId = reader.GetInt32(reader.GetOrdinal("MotorcycleId")),
                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                ExpectedEndDate = reader.GetDateTime(reader.GetOrdinal("ExpectedEndDate")),
                PlanType = reader.GetString(reader.GetOrdinal("PlanType")),
                DailyRate = reader.GetDecimal(reader.GetOrdinal("DailyRate"))
            };
        }
    }
}
