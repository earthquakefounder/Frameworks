using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Password.Rules
{
    public class UpperCaseRule : CharactersRule
    {
        public UpperCaseRule(int minimum = 1) : base("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(), minimum) { }

        public override string Message =>
            MinimumRequired == 1
            ? "1 upper case letter"
            : $"at least {MinimumRequired} upper case letters";
    }
}
