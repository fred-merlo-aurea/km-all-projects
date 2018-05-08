using System;
using System.Web;
using System.Web.UI;

namespace ecn.activityengines
{
    public abstract class PreviewBasePage : Page
    {
        private const string HttpUserAgentKey = "HTTP_USER_AGENT";

        protected static bool CheckMobileType(HttpContext context)
        {
            var mobileTypes = new string[]
            {
                "midp", "j2me", "avant", "docomo",
                "novarra", "palmos", "palmsource",
                "240x320", "opwv", "chtml",
                "pda", "windows ce", "mmp/",
                "blackberry", "mib/", "symbian",
                "wireless", "nokia", "hand", "mobi",
                "phone", "cdm", "up.b", "audio",
                "SIE-", "SEC-", "samsung", "HTC",
                "mot-", "mitsu", "sagem", "sony", "alcatel", "lg", "eric", "vx",
                "NEC", "philips", "mmm", "xx",
                "panasonic", "sharp", "wap", "sch",
                "rover", "pocket", "benq", "java",
                "pt", "pg", "vox", "amoi",
                "bird", "compal", "kg", "voda",
                "sany", "kdd", "dbt", "sendo",
                "sgh", "gradi", "jb", "dddi",
                "moto", "iphone"
            };

            // Loop through each item in the list created above 
            // and check if the header contains that text
            foreach (var mobileType in mobileTypes)
            {
                if (context.Request.ServerVariables[HttpUserAgentKey].
                        IndexOf(mobileType, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}