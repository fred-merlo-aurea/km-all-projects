namespace KM.Common.Functions
{
    public class KeyLineHelper
    {
        public char AlphaNum { get; set; }

        public int Digit { get; set; }

        public int Position { get; set; }

        public KeyLineHelper(char alphaNum, int digit, int position)
        {
            AlphaNum = alphaNum;
            Digit = digit;
            Position = position;
        }
    }
}
