using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using System.Data;

namespace MottuOpsDesafioBackEnd.Data.Repository
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly string _connectionString;

        public MotorcycleRepository(IConfiguration configuration)
        {
            // Garante que _connectionString seja inicializado corretamente
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "A string de conexão não pode ser nula");
        }

        public async Task<int> PostAsync(MotorcycleModel motorcycleModel)
        {
            string storedProcedureName = "Mottu_Procedure_Motorcycles_Insert";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Identifier", motorcycleModel.Identifier);

                        command.Parameters.AddWithValue("@LicensePlate", motorcycleModel.LicensePlate);

                        command.Parameters.AddWithValue("@Model", motorcycleModel.Model);

                        command.Parameters.AddWithValue("@Year", motorcycleModel.Year);

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

        public async Task<IEnumerable<MotorcycleModel>> GetAllAsync()
        {
            List<MotorcycleModel> list = new List<MotorcycleModel>();

            string storedProcedureName = "Mottu_Procedure_Motorcycles_GetAll";

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

        public async Task<bool> GetByLicensePlateAsync(string licensePlate)
        {
            string storedProcedureName = "Mottu_Procedure_Motorcycles_GetByLicensePlate";

            bool ret = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@LicensePlate", licensePlate);

                        SqlParameter existsParameter = new SqlParameter("@Exists", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(existsParameter);

                        await connection.OpenAsync();

                        await command.ExecuteNonQueryAsync();

                        ret = (bool)existsParameter.Value;
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

            return ret;
        }

        public async Task<MotorcycleModel> GetByIdAsync(int motorcycleId)
        {
            string storedProcedureName = "Mottu_Procedure_Motorcycles_GetById";

            MotorcycleModel? motorcycleModel = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@MotorcyclesId", motorcycleId);

                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                motorcycleModel = CreateMotorcycleFromReader(reader);
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

            return motorcycleModel;
        }

        public async Task PutAsync(MotorcycleModel motorcycleModel)
        {
            string storedProcedureName = "Mottu_Procedure_Motorcycles_Update";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Adicionar apenas os parâmetros necessários
                        AddUserMotorcyclesParameters(command, motorcycleModel);

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

        public async Task DeleteAsync(int id)
        {
            string storedProcedureName = "Mottu_Procedure_Motorcycles_Delete";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@MotorcycleId", id);

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

        private void AddUserMotorcyclesParameters(SqlCommand command, MotorcycleModel motorcycleModel)
        {
            var parameters = new (string, object?)[]
            {
                    ("@MotorcycleId", motorcycleModel.Id),
                    ("@Identifier", motorcycleModel.Identifier),
                    ("@LicensePlate", motorcycleModel.LicensePlate),
                    ("@Model", motorcycleModel.Model),
                    ("@Year", motorcycleModel.Year)
            };

            foreach (var (name, value) in parameters)
            {
                Console.WriteLine($"{name}: {value}");
                command.Parameters.AddWithValue(name, value);
            }
        }

        private MotorcycleModel CreateMotorcycleFromReader(SqlDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
                Console.WriteLine(reader.GetName(i) + " - " + reader.GetFieldType(i));

            return new MotorcycleModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Identifier = reader.GetString(reader.GetOrdinal("Identifier")),
                LicensePlate = reader.GetString(reader.GetOrdinal("LicensePlate")),
                Model = reader.GetString(reader.GetOrdinal("Model")),
                RegistrationDate = reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                Year = reader.GetInt32(reader.GetOrdinal("Year"))
            };
        }

        private MotorcycleModel CreateFromReader(SqlDataReader reader)
        {
            return new MotorcycleModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Identifier = reader.GetString(reader.GetOrdinal("Identifier")),
                LicensePlate = reader.GetString(reader.GetOrdinal("LicensePlate")),
                Model = reader.GetString(reader.GetOrdinal("Model")),
                RegistrationDate = reader.GetDateTime(reader.GetOrdinal("RegistrationDate")),
                Year = reader.GetInt32(reader.GetOrdinal("Year"))
            };
        }
    }
}
