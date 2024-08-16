using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Domain.Models;
using MottuOpsDesafioBackEnd.Business.Interface;
using System.Security.Cryptography;

namespace MottuOpsDesafioBackEnd.Business.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task DeleteAsync(int userId)
        {
            try
            {
                await _userRepository.DeleteAsync(userId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao deletar o usuário com ID {userId}", ex.Message);

                throw new ApplicationException($"Erro ao deletar o usuário com ID {userId}", ex);
            }
        }

        public Task<IEnumerable<UserModel>> GetAllAsync()
        {
            try
            {
                return _userRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao listar todos os usuários", ex);
            }
        }

        public Task<UserModel> GetByIdAsync(int userId)
        {
            try
            {
                return _userRepository.GetByIdAsync(userId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao selecionar o usuário com ID {userId}", ex.Message);

                throw new ApplicationException($"Erro ao selecionar o usuário com ID {userId}", ex);
            }
        }

        public Task<int> PostAsync(UserModel userModel)
        {
            try
            {
                return _userRepository.PostAsync(userModel);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao inserir um novo usuário com ID {userModel.Username}", ex.Message);

                throw new ApplicationException($"Erro ao inserir um novo usuário com ID {userModel.Username}", ex);
            }
        }

        public Task PutAsync(UserModel userModel)
        {
            try
            {
                return _userRepository.PutAsync(userModel);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao atualizar um usuário com ID {userModel.Username}", ex.Message);

                throw new ApplicationException($"Erro ao atualizar um usuário com ID {userModel.Username}", ex);
            }
        }

        public string HashPassword(string password)
        {
            // Exemplo usando PBKDF2
            using (var rfc2898 = new Rfc2898DeriveBytes(password, 16, 10000))
            {
                byte[] salt = rfc2898.Salt;
                byte[] hash = rfc2898.GetBytes(32);
                return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hash);
            }
        }

        public bool VerifyPassword(string storedHash, string password)
        {
            var parts = storedHash.Split('.');
            var salt = Convert.FromBase64String(parts[0]);
            var storedPasswordHash = Convert.FromBase64String(parts[1]);

            using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = rfc2898.GetBytes(32);
                return hash.SequenceEqual(storedPasswordHash);
            }
        }
    }
}
