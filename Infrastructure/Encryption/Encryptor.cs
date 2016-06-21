using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Infrastructure.Encryption
{
    public class Encryptor : IEncryptor
    {
        private const int _iterations = 256000;
        private const int _keyLength = 32;

        public string Encrypt(string value, out string salt)
        {
            byte[] saltBytes;
            byte[] hashBytes = Encrypt(value, out saltBytes);

            salt = Convert.ToBase64String(saltBytes);

            return Convert.ToBase64String(hashBytes);
        }

        public byte[] Encrypt(string value, out byte[] salt)
        {
            byte[] hashBytes = null;
            using (var encrypted = new Rfc2898DeriveBytes(value, _keyLength, _iterations))
            {
                hashBytes = encrypted.GetBytes(_keyLength);
                salt = encrypted.Salt;

                return hashBytes;
            }
        }

        public string EncryptWithSalt(string value, string salt)
        {
            return Convert.ToBase64String(EncryptWithSalt(value, Convert.FromBase64String(salt)));
        }

        public byte[] EncryptWithSalt(string value, byte[] salt)
        {
            using (var encrypted = new Rfc2898DeriveBytes(value, salt))
            {
                return encrypted.GetBytes(_keyLength);
            }
        }
    }
}
