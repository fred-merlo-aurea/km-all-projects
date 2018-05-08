using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecn.SocialEngine.Processors
{
    internal static class ProcessorHelper
    {
        public static string ClearText(string text)
        {
            return text.Replace("&nbsp;", " ");
        }

        public static string GetStatusCodeByMap(string message, Dictionary<string, string> codeMap)
        {
            string result = null;
            foreach (var pair in codeMap)
            {
                if (message.Contains(pair.Key))
                {
                    result = pair.Value;
                }
            }

            return result;
        }
    }
}
