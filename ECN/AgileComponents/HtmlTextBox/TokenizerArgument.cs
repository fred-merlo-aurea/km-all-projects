using System;
using System.Collections.Specialized;

namespace ActiveUp.WebControls
{
    internal class TokenizerArgument
    {
        public string Input { get; }

        public StringCollection Output { get; }

        public int Index { get; }

        public ParseStatus Status { get; set; }
        
        public TokenizerArgument(string input, int index, ParseStatus status, StringCollection output)
        {
            if (input.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(nameof(input));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (output == null)
            {
                output = new StringCollection();
            }

            Input = input;
            Index = index;
            Status = status;
            Output = output;
        }
    }
}