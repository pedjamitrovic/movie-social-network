using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieSocialNetworkApi.Helpers
{
    public static class PasswordHelper
    {
        public static string SHA256(string plainText, string secret)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText + secret);
            SHA256Managed sHA256ManagedString = new SHA256Managed();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
