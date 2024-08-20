using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MottuOpsDesafioBackEnd.Domain.Models;
using MottuOpsDesafioBackEnd.Data.Interface;

namespace MottuOpsDesafioBackEnd.Data.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly string _connectionString;

        public AuthenticationRepository(IConfiguration configuration)
        {
            // Garante que _connectionString seja inicializado corretamente
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "A string de conexão não pode ser nula");
        }
        public async Task<AuthenticationResponse?> PostAsync(AuthenticationRequest authenticationRequest)
        {
            string storedProcedureName = "Mottu_Procedure_Authentication";

            AuthenticationResponse? authentication = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", authenticationRequest.Username);
                        command.Parameters.AddWithValue("@Password", authenticationRequest.Password);

                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                authentication = CreateFromReader(reader);
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

            return authentication;
        }

        private AuthenticationResponse CreateFromReader(SqlDataReader reader)
        {
            return new AuthenticationResponse
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                ProfileId = reader.GetInt32(reader.GetOrdinal("ProfileId")),
                CourierId = reader.IsDBNull(reader.GetOrdinal("CourierId"))
                    ? (int?)null
                    : reader.GetInt32(reader.GetOrdinal("CourierId"))
            };
        }
    }
}