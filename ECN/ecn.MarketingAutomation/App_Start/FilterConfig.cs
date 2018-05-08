using KMSite;
using System.Web;
using System.Web.Mvc;

namespace ecn.MarketingAutomation
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomAuthorizeAttribute());
        }
    }
}
