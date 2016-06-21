using Infrastructure.Password.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Password
{
    public class PasswordComplexity : IPasswordComplexity
    {
        private LengthRule lengthRule;
        private IList<IComplexityRule> rules = new List<IComplexityRule>();
        public PasswordComplexity MinimumLength(int length)
        {
            this.lengthRule = new LengthRule(length);

            return this;
        }

        public PasswordComplexity SpecialCharacters(int minimum = 1)
        {
            return Rule(new SpecialCharactersRule(minimum));
        }

        public PasswordComplexity LowerCase(int minimum = 1)
        {
            return Rule(new LowerCaseRule());
        }

        public PasswordComplexity UpperCase(int minimum = 1)
        {
            return Rule(new UpperCaseRule());
        }

        public PasswordComplexity Numbers(int minimum = 1)
        {
            return Rule(new NumberRule(minimum));
        }

        public PasswordComplexity Rule(IComplexityRule rule)
        {
            rules.Add(rule);
            return this;
        }

        private string _message;
        public PasswordComplexity Message(string message)
        {
            _message = message;
            return this;
        }

        #region IPasswordComplexity
        public string Generate(int? length = null)
        {
            length = length ?? 10;
            if (length < lengthRule.MinimumLength)
                length = lengthRule.MinimumLength;

            var rand = new Random();

            List<char> characters = new List<char>();

            int iterations = 0;
            do
            {
                int generated_count = 0;
                iterations++;
                for (int i = 0; i < rules.Count; i++)
                {
                    char[] generated = rules[i].GenerateCharacters(rand);
                    generated_count += generated.Length;
                    characters.AddRange(generated);
                }

                if (generated_count == 0)
                    throw new InvalidOperationException($"Rules generated 0 new characters");
            } while (characters.Count < length);

            for(int i = characters.Count; i > 0; i--)
            {
                int index = rand.Next(i);
                char temp = characters[index];
                characters[index] = characters[i];
                characters[i] = temp;
            }

            return string.Concat(iterations > 1 ? characters.Take(length.Value) : characters);
        }

        public bool Validate(string password)
        {
            for(int i = 0; i < rules.Count; i++)
            {
                if (!rules[i].Validate(password))
                    return false;
            }

            return lengthRule == null || lengthRule.Validate(password);
        }

        public string ComplexityMessage => 
            _message != null
            ? _message
            : $"Password requires {string.Join(", ", new IComplexityRule[] { lengthRule }.Union(rules).Select(x => x.Message))}";
        #endregion
    }
}
