using KMSite;
using System.Web;
using System.Web.Mvc;

namespace KMWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomAuthorizeAttribute());
            filters.Add(new ValidateInputAttribute(false));
        }
    }
}
