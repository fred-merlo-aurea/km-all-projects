using System.Web;
using System.Web.Mvc;
using UAS.Web.Controllers;
using UAS.Web.Controllers.Common;

namespace UAS.Web
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
