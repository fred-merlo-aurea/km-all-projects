using System.Runtime.Serialization;

namespace KM.Common
{
    public class Enums
    {
        [DataContract]
        public enum ColumnDelimiter
        {
            [EnumMember]
            comma,
            [EnumMember]
            tab,
            [EnumMember]
            semicolon,
            [EnumMember]
            colon,
            [EnumMember]
            tild,
            [EnumMember]
            pipe
        }

        public static ColumnDelimiter GetColumnDelimiter(string delimiter)
        {
            try
            {
                return (ColumnDelimiter)System.Enum.Parse(typeof(ColumnDelimiter), delimiter, true);
            }
            catch { return ColumnDelimiter.comma; }
        }

        public static char? GetDelimiterSymbol(ColumnDelimiter delimiterName)
        {
            switch (delimiterName)
            {
                case ColumnDelimiter.colon:
                    return ':';
                case ColumnDelimiter.comma:
                    return ',';
                case ColumnDelimiter.semicolon:
                    return ';';
                case ColumnDelimiter.tab:
                    return '\t';
                case ColumnDelimiter.tild:
                    return '~';
                case ColumnDelimiter.pipe:
                    return '|';
                default:
                    return null;
            }
        }

        public static char? GetDelimiterSymbol(string delimiterName)
        {
            return GetDelimiterSymbol(GetColumnDelimiter(delimiterName));
        }
    }
}
