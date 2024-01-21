using System.Text;
using System.Security.Cryptography;
using System.Text;

namespace campground_api.Utils
{
    public class Encript
    {
        public static string GetSHA256Hash(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = SHA256.HashData(bytes);
            var sb = new StringBuilder();
            for(int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static string GenerateSalt()
        {
            byte[] salt = new byte[32];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }
    }
}
