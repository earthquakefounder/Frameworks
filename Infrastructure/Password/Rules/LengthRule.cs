using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Password.Rules
{
    public class LengthRule : IComplexityRule
    {
        public LengthRule(int minimumLength)
        {
            MinimumLength = minimumLength;
        }

        public int MinimumLength { get; private set; }

        public string Message => $"a length of {MinimumLength} characters";

        public char[] GenerateCharacters(Random rand)
        {
            throw new NotImplementedException();
        }

        public bool Validate(string password) => password != null && password.Length >= MinimumLength;
    }
}
