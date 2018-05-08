using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KM.Common.Functions
{
    /**
     *  To compute KeyLine:
     *  1. Remove spaces from input - Fill length up to 8 with /'s
     *  2. Convert characters to pure numeric values by zeroing out all but the lower four bits. This yields values for each alpha character (you can see these in AlphaToNumeric method);
     *     numeric characters retain their value.
     *  3. Multiply the value at each odd-numbered position (first position, third position, etc) by 2
     *  4. Add the digits (not the actual value) in all positions. For example, if the second position of the keyline is "L", whose value is 12, add the digits 1 + 2 (not the value 12) to the sum.
     *     Special Case: The value of the letter "N" is an odd position, when weighted, becomes 14 x 2 = 28. The digits 2 and 8 are added, yielding 10. This sum is then further rendered as 1 + 0, not the integer 10.
     *  5. Subtract the right-most digit of the sum from 10, yielding the check digit.
     *     Note: If the right-most digit of the sum is 0, the check digit will be 0.
     */
    public class KeyLineComputation
    {
        private const int NotFoundValue = -1;
        private const char AlphaNumN = 'N';
        private const string Space = " ";
        private const string HashSign = "#";
        private const string ErrorResult = "error";
        private const string ErrorMessageFormatString = "A character or digit was not recognized in your input: {0}. This record will not be included in file.";
        private static readonly Dictionary<char, int> AlphaToNumericLookUp = new Dictionary<char, int>()
        {
            ['A'] = 1,
            ['B'] = 2,
            ['C'] = 3,
            ['D'] = 4,
            ['E'] = 5,
            ['F'] = 6,
            ['G'] = 7,
            ['H'] = 8,
            ['I'] = 9,
            ['J'] = 10,
            ['K'] = 11,
            ['L'] = 12,
            ['M'] = 13,
            ['N'] = 14,
            ['O'] = 15,
            ['P'] = 0,
            ['Q'] = 1,
            ['R'] = 2,
            ['S'] = 3,
            ['T'] = 4,
            ['U'] = 5,
            ['V'] = 6,
            ['W'] = 7,
            ['X'] = 8,
            ['Y'] = 9,
            ['Z'] = 10,
            ['/'] = 15
        };

        public static string Compute(string keyLine, Action<string> errorAction = null)
        {
            keyLine = keyLine.Replace(Space, string.Empty);
            var seqLength = keyLine.Length;
            var remainder = seqLength < 8
                ? 8 - seqLength
                : 0;

            keyLine = PadWithSlashes(keyLine, remainder);

            var keyLineHelpers = new List<KeyLineHelper>();

            for (var i = 0; i < keyLine.Length; i++)
            {
                var inputChar = keyLine[i];
                if (char.IsNumber(inputChar))
                {
                    var digit = int.Parse(inputChar.ToString());
                    keyLineHelpers.Add(new KeyLineHelper(inputChar, digit, i + 1));
                }
                else if (char.IsLetter(inputChar) || char.IsPunctuation(inputChar))
                {
                    var digit = AlphaToNumeric(inputChar);
                    if (digit == NotFoundValue)
                    {
                        errorAction?.Invoke(string.Format(ErrorMessageFormatString, keyLine));

                        return ErrorResult;
                    }

                    keyLineHelpers.Add(new KeyLineHelper(inputChar, digit, i + 1));
                }
            }

            var checkDigit = ComputeCheckDigit(keyLineHelpers);
            return  $"{HashSign}{keyLine}{checkDigit}{HashSign}";
        }

        private static string PadWithSlashes(string keyLine, int remainder)
        {
            if (remainder <= 0)
            {
                return keyLine;
            }

            var builder = new StringBuilder(keyLine);
            for (var i = 0; i < remainder; i++)
            {
                builder.Append("/");
            }

            keyLine = builder.ToString();

            return keyLine;
        }

        private static int ComputeCheckDigit(IList<KeyLineHelper> helpers)
        {
            for (var index = 0; index < helpers.Count; index++)
            {
                if ((index + 1) % 2 != 1)
                {
                    continue;
                }

                var digit = helpers[index].Digit;
                digit = digit * 2;
                helpers[index].Digit = digit;
            }

            var sum = helpers.Sum(keyLineHelper => ComputeAddition(keyLineHelper.AlphaNum, keyLineHelper.Digit, keyLineHelper.Position));
            var finalDigit = int.Parse(sum.ToString().Last().ToString());

            return finalDigit == 0
                ? 0
                : 10 - finalDigit;
        }

        private static int ComputeAddition(char alphaNum, int number, int position)
        {
            if (alphaNum == AlphaNumN && (position % 2 == 1))
            {
                return 1;
            }

            var digits = number
                .ToString()
                .Select(c => int.Parse(c.ToString()));

            return digits.Sum();
        }

        private static int AlphaToNumeric(char inputChar)
        {
            int intValue;

            return AlphaToNumericLookUp.TryGetValue(inputChar, out intValue)
                ? intValue
                : NotFoundValue;
        }
    }
}
