using System;
using System.Collections.Generic;
using System.Linq;

namespace KM.Common
{
    public abstract class QueryString<TEnum, TQueryStringParam>
        where TEnum : struct
        where TQueryStringParam : QueryStringParameter<TEnum>
    {
        private const string CommaChar = ",";
        private const char AmpersandChar = '&';

        public string StringToParse { get; set; }

        public List<TQueryStringParam> ParameterList { get; set; }

        protected void ProcessQueryString(
            string stringToParse, 
            IReadOnlyDictionary<string, TEnum> mappings,
            Action<string, TEnum> queryStringAction)
        {
            if (string.IsNullOrWhiteSpace(stringToParse) ||
                mappings == null || 
                !mappings.Any())
            {
                return;
            }            

            var queryStringPairs = stringToParse.Split(AmpersandChar);
            foreach (var queryStringPair in queryStringPairs)
            {
                foreach (var mapping in mappings.Where(x => queryStringPair.Length > x.Key.Length))
                {
                    if (string.IsNullOrWhiteSpace(mapping.Key))
                    {
                        continue;
                    }

                    if (!queryStringPair.StartsWith(mapping.Key, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var queryStringValue = queryStringPair.Substring(mapping.Key.Length);
                    var commaIndex = queryStringValue.IndexOf(CommaChar, StringComparison.OrdinalIgnoreCase);
                    if (commaIndex > 0)
                    {
                        queryStringValue = queryStringValue.Substring(0, commaIndex);
                    }

                    queryStringAction(queryStringValue, mapping.Value);
                    
                    break;
                }
            }
        }
    }
}