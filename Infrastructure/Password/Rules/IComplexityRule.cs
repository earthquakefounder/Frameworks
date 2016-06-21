using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Password.Rules
{
    public interface IComplexityRule
    {
        string Message { get; }
        bool Validate(string password);
        char[] GenerateCharacters(Random rand);
    }
}
