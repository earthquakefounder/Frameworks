using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Encryption
{
    public interface IEncryptor
    {
        string Encrypt(string value, out string salt);

        bool Compare(string value, string hash, string salt);
    }
}
