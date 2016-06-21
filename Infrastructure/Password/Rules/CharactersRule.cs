using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Password.Rules
{
    public class CharactersRule : IComplexityRule
    {
        public CharactersRule(char[] characters, int? minimumRequired = null)
        {
            Characters = characters ?? new char[0];

            MinimumRequired = (minimumRequired ?? 0) <= 0
                ? 1 
                : minimumRequired ?? 1;
        }

        public int MinimumRequired { get; private set; }

        public char[] Characters { get; private set; }

        public virtual char[] GenerateCharacters(Random rand)
        {
            int index = rand.Next(Characters.Length);

            char[] characters = new char[MinimumRequired];

            for(int i = 0; i < characters.Length; i++)
            {
                characters[i] = Characters[rand.Next(Characters.Length)];
            }

            return characters;
        }

        public virtual bool Validate(string password)
        {
            if (password == null || password.Length < MinimumRequired)
                return false;

            password = password ?? "";

            int count = 0;
            int index = 0;
            bool done = false;
            do
            {
                done = (index = password.IndexOfAny(Characters)) < 0;

                if (!done)
                {
                    count++;
                    password = password.Length > index + 1
                        ? password.Substring(index + 1)
                        : "";
                }
            } while (!done && count < MinimumRequired);

            return count >= MinimumRequired;
        }

        public virtual string Message => $"{MinimumRequired} of {new string(Characters)}";
    }
}
