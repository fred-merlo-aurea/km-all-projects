using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace ecn.common.classes
{

    public class URLFunctions
    {
        public URLFunctions()
        {
            //default Constructor.
        }

        public static string getWebPageTitle(string strurl)
        {
            HttpWebRequest request = HttpWebRequest.Create(strurl) as HttpWebRequest;
            if (request == null)
            {
                return "[404] PAGE NOT FOUND";
            }

            // Regular expression for an HTML title
            string regex = @"(?<=<title.*>)([\s\S]*)(?=</title>)";

            try
            {
                // Download the page
                WebClient web = new WebClient();
                Stream data = web.OpenRead(strurl);
                StreamReader reader = new StreamReader(data);
                string pageData = reader.ReadToEnd();
                data.Close();
                reader.Close();

                // Extract the title
                Regex ex = new Regex(regex, RegexOptions.IgnoreCase);
                string returnvalue = ex.Match(pageData).Value.Trim();

                returnvalue = returnvalue.Substring(0, (returnvalue.Length>200?199:returnvalue.Length));

                returnvalue = returnvalue.Substring(0, (returnvalue.IndexOf("</title>",StringComparison.OrdinalIgnoreCase)>-1?returnvalue.IndexOf("</title>",StringComparison.OrdinalIgnoreCase):returnvalue.Length));

                return returnvalue;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return "[404] PAGE NOT FOUND";
            }
        }

    }
}