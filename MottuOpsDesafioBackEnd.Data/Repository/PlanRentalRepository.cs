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

        public async Task DeleteAsync(int planRentalId)
        {
            string storedProcedureName = "Mottu_Procedure_PlanRental_Delete";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PlanRentalId", planRentalId);

                        await connection.OpenAsync();

                        await command.ExecuteNonQueryAsync();
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

        public async Task<PlanRentalModel> GetByIdAsync(int planRentalId)
        {
            string storedProcedureName = "Mottu_Procedure_PlanRental_GetById";

            PlanRentalModel planRentalModel = new PlanRentalModel();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@PlanRentalId", planRentalId);

                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                planRentalModel.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                                planRentalModel.Identifier = reader.GetString(reader.GetOrdinal("Identifier"));
                                planRentalModel.Days = reader.GetString(reader.GetOrdinal("Days"));
                                planRentalModel.Value = reader.GetString(reader.GetOrdinal("Value"));
                                planRentalModel.TerminationFine = reader.GetString(reader.GetOrdinal("TerminationFine"));
                                planRentalModel.RegistrationDate = reader.GetDateTime(reader.GetOrdinal("RegistrationDate"));
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

            return planRentalModel;
        }

        public async Task<int> PostAsync(PlanRentalModel planRentalModel)
        {
            string storedProcedureName = "Mottu_Procedure_PlanRental_Insert";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Identifier", planRentalModel.Identifier);

                        command.Parameters.AddWithValue("@Days", planRentalModel.Days);

                        command.Parameters.AddWithValue("@Value", planRentalModel.Value);

                        command.Parameters.AddWithValue("@TerminationFine", planRentalModel.TerminationFine);

                        await connection.OpenAsync();

                        var result = await command.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int planRentalId))
                        {
                            return planRentalId;
                        }
                        else
                        {
                            throw new Exception("Falha ao recuperar o novo ID do plano.");
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

        public async Task PutAsync(PlanRentalModel planRentalModel)
        {
            string storedProcedureName = "Mottu_Procedure_PlanRental_Update";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        AddPlanRentalParameters(command, planRentalModel);

                        await connection.OpenAsync();

                        await command.ExecuteNonQueryAsync();
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

        private PlanRentalModel CreateFromReader(SqlDataReader reader)
        {
            return new PlanRentalModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Identifier = reader.GetString(reader.GetOrdinal("Identifier")),
                Days = reader.GetString(reader.GetOrdinal("Days")),
                Value = reader.GetString(reader.GetOrdinal("Value")),
                TerminationFine = reader.GetString(reader.GetOrdinal("TerminationFine")),
                RegistrationDate = reader.GetDateTime(reader.GetOrdinal("RegistrationDate"))
            };
        }

        private PlanRentalModel CreatePlanRentalFromReader(SqlDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
                Console.WriteLine(reader.GetName(i) + " - " + reader.GetFieldType(i));

            return new PlanRentalModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Identifier = reader.GetString(reader.GetOrdinal("Identifier")),
                Days = reader.GetString(reader.GetInt32("Days")),
                Value = reader.GetString(reader.GetOrdinal("Value")),
                TerminationFine = reader.GetString(reader.GetOrdinal("TerminatiopnFine")),
                RegistrationDate = reader.GetDateTime(reader.GetOrdinal("TerminatiopnFine"))
            };
        }

        private void AddPlanRentalParameters(SqlCommand command, PlanRentalModel planRentalModel)
        {
            var parameters = new (string, object?)[]
            {
                    ("@PlanRentalId", planRentalModel.Id),
                    ("@Identifier", planRentalModel.Identifier),
                    ("@Days", planRentalModel.Days),
                    ("@Value", planRentalModel.Value),
                    ("@TerminationFine", planRentalModel.TerminationFine)
            };

            foreach (var (name, value) in parameters)
            {
                Console.WriteLine($"{name}: {value}");
                command.Parameters.AddWithValue(name, value);
            }
        }
    }
}
