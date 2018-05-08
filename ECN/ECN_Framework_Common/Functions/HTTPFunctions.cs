using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace ECN_Framework_Common.Functions
{
    public class HTTPFunctions
    {
        public static string GetWebFeed(string feedurl)
        {
            string body = "";
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(feedurl);
            HttpWebResponse ws = (HttpWebResponse)wr.GetResponse();
            Stream str = ws.GetResponseStream();
            StreamReader sr = new StreamReader(str);
            string line = " ";
            while (line != null)
            {
                body += line;
                line = sr.ReadLine();
            }
            return body;
        }
    }
}
