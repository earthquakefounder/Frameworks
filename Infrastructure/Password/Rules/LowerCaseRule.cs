using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Password.Rules
{
    public class LowerCaseRule : CharactersRule
    {
        public LowerCaseRule(int minimum = 1) : base("abcdefghijklmnopqrstuvwxyz".ToCharArray(), minimum) { }

        public override string Message =>
            MinimumRequired == 1
            ? "1 lower case letter"
            : $"{MinimumRequired} lower case letters";
    }
}
