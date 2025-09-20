using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GermBox.Pathogens
{
    internal class NameGenerator
    {
        static Random random = new Random();

        // Roman numeral to integer map
        static readonly Dictionary<char, int> romanMap = new Dictionary<char, int>()
        {
            {'I', 1}, {'V', 5}, {'X', 10}, {'L', 50},
            {'C', 100}, {'D', 500}, {'M', 1000}
        };

            // Sample Latin-like genus roots and suffixes
        static string[] genusRoots = {
            "Toxi", "Morb", "Bacter", "Viro", "Patho", "Crypt", "Myco", "Necr", "Septic", "Infec"
        };

        static string[] genusSuffixes = {
            "coccus", "bacter", "virus", "myces", "plasma", "spora", "cella", "phage", "monas", "cella"
        };

            // Sample Latin-like species epithets
        static string[] speciesEpithets = {
            "virulenta", "mortalis", "pestifera", "contagiosa", "lethalis", "maligna", "infesta", "necrosa", "sepsica", "toxica"
        };

        public static string GenerateGenus()
        {
            string root = genusRoots[random.Next(genusRoots.Length)];
            string suffix = genusSuffixes[random.Next(genusSuffixes.Length)];
            string genus = root + suffix;

            // Capitalize the genus
            return char.ToUpper(genus[0]) + genus.Substring(1).ToLower();
        }

        public static string GenerateSpecies()
        {
            return speciesEpithets[random.Next(speciesEpithets.Length)].ToLower();
        }

        public static string AddNumeral(string numeral)
        {
            int number = RomanToInt(numeral) + 1;
            return IntToRoman(number);
        }

        public static string MutatedName(string name) { 
            throw new NotImplementedException();        
        }



        public static int RomanToInt(string roman)
        {
            int total = 0;
            int prev = 0;

            foreach (char c in roman.ToUpper())
            {
                int current = romanMap[c];

                if (current > prev)
                {
                    // Subtractive combination (e.g., IV = 5 - 1)
                    total += current - 2 * prev;
                }
                else
                {
                    total += current;
                }

                prev = current;
            }

            return total;
        }

        public static string IntToRoman(int number)
        {
            if (number < 1 || number > 3999)
                throw new ArgumentOutOfRangeException("number", "Valid range is 1-3999");

            var romanNumerals = new[]
            {
                (1000, "M"), (900, "CM"), (500, "D"), (400, "CD"),
                (100, "C"),  (90, "XC"),  (50, "L"),  (40, "XL"),
                (10, "X"),   (9, "IX"),   (5, "V"),   (4, "IV"),
                (1, "I")
            };

            var result = "";

            foreach (var (value, numeral) in romanNumerals)
            {
                while (number >= value)
                {
                    result += numeral;
                    number -= value;
                }
            }

            return result;
        }
    }
}
