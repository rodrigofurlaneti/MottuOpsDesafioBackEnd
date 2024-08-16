using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using System.Data;

namespace MottuOpsDesafioBackEnd.Data.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly string _connectionString;

        public ProfileRepository(IConfiguration configuration)
        {
            // Garante que _connectionString seja inicializado corretamente
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "A string de conexão não pode ser nula");
        }

        public async Task<int> PostAsync(UserProfileModel userProfileModel)
        {
            string storedProcedureName = "Mottu_Procedure_UserProfiles_Insert";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ProfileName", userProfileModel.ProfileName);

                        await connection.OpenAsync();

                        var result = await command.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int userProfileId))
                        {
                            return userProfileId;
                        }
                        else
                        {
                            throw new Exception("Falha ao recuperar o novo ID do perfil do usuário.");
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

        public async Task<IEnumerable<UserProfileModel>> GetAllAsync()
        {
            List<UserProfileModel> list = new List<UserProfileModel>();

            string storedProcedureName = "Mottu_Procedure_UserProfiles_GetAll";

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

        public async Task<UserProfileModel> GetByIdAsync(int userProfileId)
        {
            string storedProcedureName = "Mottu_Procedure_UserProfiles_GetById";

            UserProfileModel? userProfileModel = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@UserProfilesId", userProfileId);

                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                userProfileModel = CreateUserProfileFromReader(reader);
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

            return userProfileModel;
        }

        public async Task PutAsync(UserProfileModel userProfileModel)
        {
            string storedProcedureName = "Mottu_Procedure_UserProfiles_Update";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Adicionar apenas os parâmetros necessários
                        AddUserProfilesParameters(command, userProfileModel);

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
            string storedProcedureName = "Mottu_Procedure_UserProfiles_Delete";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserProfileId", id);

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

        private void AddUserProfilesParameters(SqlCommand command, UserProfileModel userProfileModel)
        {
            var parameters = new (string, object?)[]
            {
                    ("@ProfileId", userProfileModel.Id),
                    ("@ProfileName", userProfileModel.ProfileName)
            };

            foreach (var (name, value) in parameters)
            {
                Console.WriteLine($"{name}: {value}");
                command.Parameters.AddWithValue(name, value);
            }
        }

        private UserProfileModel CreateUserProfileFromReader(SqlDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
                Console.WriteLine(reader.GetName(i) + " - " + reader.GetFieldType(i));

            return new UserProfileModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                ProfileName = reader.GetString(reader.GetOrdinal("ProfileName"))
            };
        }

        private UserProfileModel CreateFromReader(SqlDataReader reader)
        {
            return new UserProfileModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                ProfileName = reader.GetString(reader.GetOrdinal("ProfileName"))
            };
        }
    }
}
