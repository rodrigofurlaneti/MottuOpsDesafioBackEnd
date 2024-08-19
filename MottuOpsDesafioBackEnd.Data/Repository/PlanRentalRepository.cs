using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using System.Data;

namespace MottuOpsDesafioBackEnd.Data.Repository
{
    public class PlanRentalRepository : IPlanRentalRepository
    {
        private readonly string _connectionString;

        public PlanRentalRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "A string de conexão não pode ser nula");
        }

        public async Task<IEnumerable<PlanRentalModel>> GetAllAsync()
        {
            List<PlanRentalModel> list = new List<PlanRentalModel>();

            string storedProcedureName = "Mottu_Procedure_PlanRental_GetAll";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                list.Add(CreateFromReader(reader));
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

            return list;
        }

        private PlanRentalModel CreateFromReader(SqlDataReader reader)
        {
            return new PlanRentalModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Identifier = reader.GetString(reader.GetOrdinal("Identifier")),
                Days = reader.GetString(reader.GetOrdinal("Days")),
                Value = reader.GetDecimal(reader.GetOrdinal("Value")),
                TerminationFine = reader.GetDecimal(reader.GetOrdinal("TerminationFine")),
                RegistrationDate = reader.GetDateTime(reader.GetOrdinal("RegistrationDate"))
            };
        }
    }
}
