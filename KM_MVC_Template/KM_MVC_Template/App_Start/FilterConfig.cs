using KMSite;
using System.Web;
using System.Web.Mvc;

namespace KM_MVC_Template
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
