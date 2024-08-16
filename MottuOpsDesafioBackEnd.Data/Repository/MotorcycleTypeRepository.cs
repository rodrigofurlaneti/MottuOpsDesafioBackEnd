using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using System.Data;

namespace MottuOpsDesafioBackEnd.Data.Repository
{
    public class MotorcycleTypeRepository : IMotorcycleTypeRepository
    {
        private readonly string _connectionString;

        public MotorcycleTypeRepository(IConfiguration configuration)
        {
            // Garante que _connectionString seja inicializado corretamente
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "A string de conexão não pode ser nula");
        }

        public async Task<int> PostAsync(MotorcycleTypeModel motorcycleTypeModel)
        {
            string storedProcedureName = "Mottu_Procedure_MotorcyclesTypes_Insert";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@TypeName", motorcycleTypeModel.TypeName);

                        await connection.OpenAsync();

                        var result = await command.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int userProfileId))
                        {
                            return userProfileId;
                        }
                        else
                        {
                            throw new Exception("Falha ao recuperar o novo ID do tipo da moto.");
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

        public async Task<IEnumerable<MotorcycleTypeModel>> GetAllAsync()
        {
            List<MotorcycleTypeModel> list = new List<MotorcycleTypeModel>();

            string storedProcedureName = "Mottu_Procedure_MotorcyclesTypes_GetAll";

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

        public async Task<MotorcycleTypeModel> GetByIdAsync(int motorcycleTypeId)
        {
            string storedProcedureName = "Mottu_Procedure_MotorcyclesTypes_GetById";

            MotorcycleTypeModel? motorcycleTypeModel = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@MotorcyclesTypesId", motorcycleTypeId);

                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                motorcycleTypeModel = CreateMotorcycleTypeFromReader(reader);
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

            return motorcycleTypeModel;
        }

        public async Task PutAsync(MotorcycleTypeModel motorcycleTypeModel)
        {
            string storedProcedureName = "Mottu_Procedure_MotorcyclesTypes_Update";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Adicionar apenas os parâmetros necessários
                        AddMotorcycleTypeParameters(command, motorcycleTypeModel);

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
            string storedProcedureName = "Mottu_Procedure_MotorcyclesTypes_Delete";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@MotorcycleTypeId", id);

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

        private void AddMotorcycleTypeParameters(SqlCommand command, MotorcycleTypeModel motorcycleTypeModel)
        {
            var parameters = new (string, object?)[]
            {
                    ("@Id", motorcycleTypeModel.Id),
                    ("@TypeName", motorcycleTypeModel.TypeName)
            };

            foreach (var (name, value) in parameters)
            {
                Console.WriteLine($"{name}: {value}");
                command.Parameters.AddWithValue(name, value);
            }
        }

        private MotorcycleTypeModel CreateMotorcycleTypeFromReader(SqlDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
                Console.WriteLine(reader.GetName(i) + " - " + reader.GetFieldType(i));

            return new MotorcycleTypeModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                TypeName = reader.GetString(reader.GetOrdinal("TypeName"))
            };
        }

        private MotorcycleTypeModel CreateFromReader(SqlDataReader reader)
        {
            return new MotorcycleTypeModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                TypeName = reader.GetString(reader.GetOrdinal("TypeName"))
            };
        }
    }
}
