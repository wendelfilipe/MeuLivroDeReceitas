using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Application.Services.Criptography
{
    public class PasswordEncripter
    {
        private readonly string additionalKey;
        public PasswordEncripter(string additionalKey)
        {
            this.additionalKey = additionalKey;
        }
        public string Encrypt(string password)
        {
            var newPassword = $"{password}{additionalKey}";

            var bytes = Encoding.UTF8.GetBytes(newPassword);
            var hasBytes = SHA512.HashData(bytes);

            return StringBytes(hasBytes);
        }

        private static string StringBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }

            return sb.ToString();
        }
    }
}
