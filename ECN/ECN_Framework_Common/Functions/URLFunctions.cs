using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECN_Framework_Common.Functions
{
    public class URLFunctions
    {
        public URLFunctions()
        {
            //default Constructor.
        }

        public static string getWebPageTitle(string strUrl)
        {
            return getWebPageTitleWithTimeout(strUrl).Result;
        }

        public static async Task<string> getWebPageTitleWithTimeout(string strUrl)
        {
            //Task.WaitAny()
            Task<string> task = getWebPageTitleAsync(strUrl);
            if (await Task.WhenAny(task, Task.Delay(10000)) == task)
            {
                return task.Result;
            }
            else
            {
                return "PAGE TITLE NOT FOUND";
            }
        }

        public static async Task<string> getWebPageTitleAsync(string strurl)
        {
            return await Task.Run(() =>
            {
                string returnResult = string.Empty;
                try
                {


                    HttpWebRequest request = HttpWebRequest.Create(strurl) as HttpWebRequest;
                    if (request == null)
                    {
                        returnResult = "[404] PAGE NOT FOUND";
                    }
                    else
                    {
                        string pageData = string.Empty;

                        using (WebClient web = new WebClient())
                        using (Stream data = web.OpenRead(strurl))
                        using (StreamReader reader = new StreamReader(data))
                        {
                            pageData = reader.ReadToEnd();//this is one place that can be slow or never complete - http://www.conairgroup.com/assets/npe2015/Conair 3D filament production.mp4
                        }


                        if (StringFunctions.HasValue(pageData))
                        {
                            // Regular expression for an HTML title
                            string regex = @"(?<=<title.*>)([\s\S]*)(?=</title>)";

                            // Extract the title
                            Regex ex = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
                            returnResult = ex.Match(pageData).Value.Trim();//this is one place that can be slow or never complete - http://econdev.burbankca.gov/Modules/ShowDocument.aspx?documentid=20987
                            returnResult = returnResult.Substring(0, (returnResult.Length > 200 ? 199 : returnResult.Length));
                            returnResult = returnResult.Substring(0, (returnResult.IndexOf("</title>", StringComparison.OrdinalIgnoreCase) > -1 ? returnResult.IndexOf("</title>", StringComparison.OrdinalIgnoreCase) : returnResult.Length));
                        }
                        //should be end of threaded////////////////////////////////////////////////

                        if (!StringFunctions.HasValue(returnResult))
                            returnResult = "PAGE TITLE NOT FOUND";
                    }
                }
                catch (Exception)
                {
                    returnResult = "[404] PAGE NOT FOUND";
                }
                return returnResult;
            });

        }
    }
}
