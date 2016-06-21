using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Password.Rules
{
    public class SpecialCharactersRule : CharactersRule
    {
        public SpecialCharactersRule(int minimum) : base("@%+\\/'!#$^?:.(){}{}~-_".ToCharArray(), minimum) { }
    }
}
 