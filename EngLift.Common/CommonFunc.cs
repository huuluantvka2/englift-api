using System.Security.Cryptography;

namespace EngLift.Common
{
    public static class CommonFunc
    {
        private const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+";
        public static string GenerateRandomPassword(int length)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[length];
                rng.GetBytes(buffer);

                var password = new string(buffer.Select(b => ValidChars[b % ValidChars.Length]).ToArray());
                return password;
            }
        }
    }
}
