using System;
using System.Text;

namespace KM.Common.Functions
{
    public class NumberConverter
    {
        private const string MillionValue = "million";
        private const string ThousandValue = "thousand";
        private const string HundredValue = "hundred";
        private const string OneValue = "one";
        private const string TwoValue = "two";
        private const string ThreeValue = "three";
        private const string FourValue = "four";
        private const string FiveValue = "five";
        private const string SixValue = "six";
        private const string SevenValue = "seven";
        private const string EightValue = "eight";
        private const string NineValue = "nine";
        private const string TenValue = "ten";
        private const string TwentyValue = "twenty";
        private const string ThirtyValue = "thirty";
        private const string FourtyValue = "fourty";
        private const string FiftyValue = "fifty";
        private const string SixtyValue = "sixty";
        private const string SeventyValue = "seventy";
        private const string EightyValue = "eighty";
        private const string NinetyValue = "ninety";
        private const string ElevenValue = "eleven";
        private const string TwelveValue = "twelve";
        private const string ThirteenValue = "thirteen";
        private const string FourteenValue = "fourteen";
        private const string FifteenValue = "fifteen";
        private const string SixteenValue = "sixteen";
        private const string SeventeenValue = "seventeen";
        private const string EighteenValue = "eighteen";
        private const string NineteenValue = "nineteen";
        private const string AndValue = "and ";

        public string GetNumberInWord(int value)
        {
            var number = value.ToString();
            var numberInText = new StringBuilder();

            var andFlag = false;
            if (number.Length > 6)
            {
                var triplet = DoTriplet((value % 1000000000) / 1000000, andFlag);
                numberInText.AppendFormat("{0} {1} ", triplet, MillionValue);
                andFlag = true;
            }

            if (number.Length > 3)
            {
                var triplet = DoTriplet((value % 1000000) / 1000, andFlag);
                numberInText.AppendFormat("{0} {1} ", triplet, ThousandValue);
                andFlag = true;
            }
            else
            {
                andFlag = false;
            }

            numberInText.Append(DoTriplet(value % 1000, andFlag));
            return numberInText.ToString();
        }

        private string DoTriplet(int number, bool andflag)
        {
            var numberModuloTen = number % 10;
            var numberModuloHundred = (number % 100 / 10);
            var numberDividedByHundred = number / 100;

            var numberInText = new StringBuilder();
            if (numberDividedByHundred != 0)
            {
                var ones = GetOnes(numberDividedByHundred);
                numberInText.AppendFormat("{0} {1} ", ones, HundredValue);
            }

            if (numberModuloHundred != 0 || numberModuloTen != 0)
            {
                if (numberDividedByHundred != 0 || andflag)
                {
                    numberInText.Append(AndValue);
                }
            }

            if (numberModuloHundred == 1 && numberModuloTen != 0)
            {
                var first = GetFirst(numberModuloHundred * 10 + numberModuloTen);
                numberInText.Append(first);
            }
            else
            {
                var tens = GetTens(numberModuloHundred);
                var ones = GetOnes(numberModuloTen);
                numberInText.AppendFormat("{0} {1}", tens, ones);
            }

            return numberInText.ToString();
        }

        private string GetOnes(int number)
        {
            switch (number)
            {
                case 1:
                    return OneValue;
                case 2:
                    return TwoValue;
                case 3:
                    return ThreeValue;
                case 4:
                    return FourValue;
                case 5:
                    return FiveValue;
                case 6:
                    return SixValue;
                case 7:
                    return SevenValue;
                case 8:
                    return EightValue;
                case 9:
                    return NineValue;
                default:
                    return String.Empty;
            }
        }

        private string GetTens(int number)
        {
            switch (number)
            {
                case 1:
                    return TenValue;
                case 2:
                    return TwentyValue;
                case 3:
                    return ThirtyValue;
                case 4:
                    return FourtyValue;
                case 5:
                    return FiftyValue;
                case 6:
                    return SixtyValue;
                case 7:
                    return SeventyValue;
                case 8:
                    return EightyValue;
                case 9:
                    return NinetyValue;
                default:
                    return String.Empty;
            }
        }

        private string GetFirst(int number)
        {
            switch (number)
            {
                case 11:
                    return ElevenValue;
                case 12:
                    return TwelveValue;
                case 13:
                    return ThirteenValue;
                case 14:
                    return FourteenValue;
                case 15:
                    return FifteenValue;
                case 16:
                    return SixteenValue;
                case 17:
                    return SeventeenValue;
                case 18:
                    return EighteenValue;
                case 19:
                    return NineteenValue;
                default:
                    return String.Empty;
            }
        }
    }
}
