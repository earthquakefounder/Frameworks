using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Password.Rules
{
    public class NumberRule : CharactersRule
    {
        public NumberRule(int minimum = 1) : base("0123456789".ToCharArray(), minimum) { }

        public override string Message =>
            MinimumRequired == 1
            ? "1 number"
            : $"{MinimumRequired} numbers";
    }
}
