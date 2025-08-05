using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HotelRvDbContext.Infrastructure.Persistence.Extensions
{
    public class PasswordEncryptor
    {
        private const int SaltSize = 16;  // 16 byte salt
        private const int HashSize = 32;  // 32 byte hash
        private const int Iterations = 100000;  // 100,000 iterasyon

        public static string Encrpt(string password)
        {
            //TODO burada testler yapilacak hashing algoritm degistirildi 
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Şifre boş olamaz");

            // Rastgele salt oluştur
            byte[] salt = new byte[SaltSize];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            // PBKDF2 ile hashle
            byte[] hash = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256)
                .GetBytes(HashSize);

            // Format: {salt}{hash}
            byte[] saltAndHash = new byte[SaltSize + HashSize];
            Buffer.BlockCopy(salt, 0, saltAndHash, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, saltAndHash, SaltSize, HashSize);

            return Convert.ToBase64String(saltAndHash);
        }
    }
}
