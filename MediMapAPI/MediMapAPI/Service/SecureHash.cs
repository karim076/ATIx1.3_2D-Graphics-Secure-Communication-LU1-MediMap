using System.Security.Cryptography;

namespace MediMapAPI.Models
{
    public class SecureHash
    {
        private const int saltSize = 16;
        private const int keySize = 32;
        private const char segmentDelimiter = ':';
        private static readonly HashAlgorithmName algorithm = HashAlgorithmName.SHA512;

        public static string Hash(string password, int iterations = 10000)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                algorithm,
                keySize
            );

            return string.Join(
                segmentDelimiter.ToString(),
                iterations,
                Convert.ToBase64String(hash),
                Convert.ToBase64String(salt),
                algorithm
            );
        }

        public static bool Verify(string inputPassword, string hashedPassword)
        {
            string[] segments = hashedPassword.Split(segmentDelimiter);

            byte[] hash = Convert.FromBase64String(segments[1]);
            byte[] salt = Convert.FromBase64String(segments[2]);
            var iterations = int.Parse(segments[0]);
            HashAlgorithmName algorithm = new HashAlgorithmName(segments[3]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                inputPassword,
                salt,
                iterations,
                algorithm,
                hash.Length
            );

            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }
    }
}
