using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Encryption
{
    public interface IEncryptor
    {
        byte[] Encrypt(string value, out byte[] salt);

        byte[] EncryptWithSalt(string value, byte[] salt);

        string Encrypt(string value, out string salt);

        string EncryptWithSalt(string value, string salt);
    }
}
