using System;
using System.Web;
using System.Collections.Generic;

namespace Core_AMS.Utilities
{
    public class HTTPFunctions
    {
        public Dictionary<string, string> GetServerVariables()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.ServerVariables != null)
            {
                for (int i = 0; i < HttpContext.Current.Request.ServerVariables.Count; i++)
                {
                    dict.Add(HttpContext.Current.Request.ServerVariables.Keys[i].ToString(), HttpContext.Current.Request.ServerVariables[i].ToString());
                }
            }
            return dict;
        }
        //public static string GetWebFeed(string feedurl)
        //{
        //    string body = "";
        //    HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(feedurl);
        //    HttpWebResponse ws = (HttpWebResponse)wr.GetResponse();
        //    Stream str = ws.GetResponseStream();
        //    StreamReader sr = new StreamReader(str);
        //    string line = " ";
        //    while (line != null)
        //    {
        //        body += line;
        //        line = sr.ReadLine();
        //    }
        //    return body;
        //}

        //public static Control FindControlRecursive(Control controlToSearch, string id)
        //{
        //    if (null == controlToSearch)
        //        throw new ArgumentNullException("controlToSearch");

        //    if (string.IsNullOrEmpty("id"))
        //        throw new ArgumentNullException("id");

        //    Control result = controlToSearch.FindControl(id);
        //    if (null == result)
        //    {
        //        if (null != controlToSearch.Controls && controlToSearch.Controls.Count > 0)
        //        {
        //            foreach (Control c in controlToSearch.Controls)
        //            {
        //                result = FindControlRecursive(c, id);
        //                if (null != result)
        //                    break;
        //            }
        //        }
        //    }
        //    return result;
        //}
    }
}
