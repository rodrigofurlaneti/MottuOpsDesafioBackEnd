﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using System.Data;

namespace MottuOpsDesafioBackEnd.Data.Repository
{
    public class CourierRepository : ICourierRepository
    {
        private readonly string _connectionString;

        public CourierRepository(IConfiguration configuration)
        {
            // Garante que _connectionString seja inicializado corretamente
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "A string de conexão não pode ser nula");
        }

        public async Task<int> PostAsync(CourierModel courierModel)
        {
            string storedProcedureName = "Mottu_Procedure_Couriers_Insert";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Identifier", courierModel.Identifier);

                        command.Parameters.AddWithValue("@Name", courierModel.Name);

                        command.Parameters.AddWithValue("@CNPJ", courierModel.CNPJ);

                        command.Parameters.AddWithValue("@BirthDate", courierModel.BirthDate);

                        command.Parameters.AddWithValue("@CNHNumber", courierModel.CNHNumber);

                        command.Parameters.AddWithValue("@CNHType", courierModel.CNHType);

                        command.Parameters.AddWithValue("@CNHImagePath", courierModel.CNHImagePath);

                        command.Parameters.AddWithValue("@Username", courierModel.Username);

                        command.Parameters.AddWithValue("@PasswordHash", courierModel.PasswordHash);

                        command.Parameters.AddWithValue("@ProfileId", courierModel.ProfileId);

                        await connection.OpenAsync();

                        var result = await command.ExecuteScalarAsync();

                        if (result != null && int.TryParse(result.ToString(), out int courierId))
                        {
                            return courierId;
                        }
                        else
                        {
                            throw new Exception("Falha ao recuperar o novo ID do entregador.");
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

        public async Task<CourierModel> GetByIdAsync(int courierId)
        {
            string storedProcedureName = "Mottu_Procedure_Couriers_GetById";

            CourierModel? courierModel = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CouriersId", courierId);

                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                courierModel = CreateCourierFromReader(reader);
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

            return courierModel;
        }
        public async Task PutCnhAsync(CourierModel courierModel)
        {
            string storedProcedureName = "Mottu_Procedure_Couriers_Update_Cnh";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        AddCourierCnhParameters(command, courierModel);

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


        public async Task<bool> GetByCnpjAsync(string cnpj)
        {
            string storedProcedureName = "Mottu_Procedure_Couriers_GetByCnpj";

            bool ret = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Cnpj", cnpj);

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

        public async Task<bool> GetByCnhAsync(string cnh)
        {
            string storedProcedureName = "Mottu_Procedure_Couriers_GetByCnh";

            bool ret = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Cnh", cnh);

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

        private void AddCourierParameters(SqlCommand command, CourierModel courierModel)
        {
            var parameters = new (string, object?)[]
            {
                    ("@CourierId", courierModel.Id),
                    ("@Identifier", courierModel.Identifier),
                    ("@Name", courierModel.Name),
                    ("@CNPJ", courierModel.CNPJ),
                    ("@BirthDate", courierModel.BirthDate),
                    ("@CNHNumber", courierModel.CNHNumber),
                    ("@CNHType", courierModel.CNHType),
                    ("@CNHImagePath", courierModel.CNHImagePath)
            };

            foreach (var (name, value) in parameters)
            {
                Console.WriteLine($"{name}: {value}");
                command.Parameters.AddWithValue(name, value);
            }
        }

        private void AddCourierCnhParameters(SqlCommand command, CourierModel courierModel)
        {
            var parameters = new (string, object?)[]
            {
                    ("@CourierId", courierModel.Id),
                    ("@CNHImagePath", courierModel.CNHImagePath)
            };

            foreach (var (name, value) in parameters)
            {
                Console.WriteLine($"{name}: {value}");
                command.Parameters.AddWithValue(name, value);
            }
        }

        private CourierModel CreateCourierFromReader(SqlDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
                Console.WriteLine(reader.GetName(i) + " - " + reader.GetFieldType(i));

            return new CourierModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Identifier = reader.GetString(reader.GetOrdinal("Identifier")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                CNPJ = reader.GetString(reader.GetOrdinal("CNPJ")),
                BirthDate = reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                CNHNumber = reader.GetString(reader.GetOrdinal("CNHNumber")),
                CNHType = reader.GetString(reader.GetOrdinal("CNHType")),
                RegistrationDate = reader.GetDateTime(reader.GetOrdinal("RegistrationDate"))
            };
        }
    }
}
