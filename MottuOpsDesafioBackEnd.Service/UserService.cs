using System.Security.Cryptography;

namespace MottuOpsDesafioBackEnd.Service
{
    public class UserService
    {
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
