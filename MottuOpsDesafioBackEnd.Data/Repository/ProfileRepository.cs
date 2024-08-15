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
    }
}
