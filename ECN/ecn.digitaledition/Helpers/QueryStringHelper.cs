using System;
using System.Web;

namespace Ecn.DigitalEdition.Helpers
{
    public static class QueryStringHelper
    {
        public static string GetStringValue(HttpRequestBase request, string key)
        {
            try
            {
                return request.QueryString[key].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static int GetIntValue(HttpRequestBase request, string key)
        {
            try
            {
                return Convert.ToInt32(request.QueryString[key].ToString());
            }
            catch
            {
                return 0;
            }
        }
    }
}
