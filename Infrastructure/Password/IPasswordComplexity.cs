using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Password
{
    public interface IPasswordComplexity
    {
        string ComplexityMessage { get; }
        bool Validate(string password);
        string Generate(int? length = null);
    }
}
