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

        public bool Compare(string value, string hash, string salt)
        {
            byte[] valueHashByte = EncryptWithSalt(value, Convert.FromBase64String(salt));
            byte[] hashByte = Convert.FromBase64String(hash);

            if (valueHashByte.Length != hashByte.Length)
                return false;

            uint diff = (uint)hashByte.Length ^ (uint)valueHashByte.Length;
            for (int i = 0; i < hashByte.Length; i++)
            {
                diff |= (uint)(hashByte[i] ^ valueHashByte[i]);
            }

            return diff == 0;
        }

        public string Encrypt(string value, out string salt)
        {
            byte[] saltBytes;
            byte[] hashBytes = null;
            using (var encrypted = new Rfc2898DeriveBytes(value, _keyLength, _iterations))
            {
                hashBytes = encrypted.GetBytes(_keyLength);
                saltBytes = encrypted.Salt;
                
            }

            salt = Convert.ToBase64String(saltBytes);

            return Convert.ToBase64String(hashBytes);
        }


        private byte[] EncryptWithSalt(string value, byte[] salt)
        {
            using (var encrypted = new Rfc2898DeriveBytes(value, salt))
            {
                return encrypted.GetBytes(_keyLength);
            }
        }
    }
}
