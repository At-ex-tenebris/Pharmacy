using System;
using System.Security.Cryptography;

namespace pharmacyApi.Utils {
    public static class PasswordHasher {
        public static string Hash(string password) {
            SHA512 hasher = new SHA512CryptoServiceProvider();
            byte[] srcBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] hash = hasher.ComputeHash(srcBytes);

            return Convert.ToBase64String(hash);
        }
    }
}
