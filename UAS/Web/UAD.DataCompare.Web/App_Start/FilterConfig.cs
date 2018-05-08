
using System.Web;
using System.Web.Mvc;
using UAD.DataCompare.Web.Controllers.Common;

namespace UAD.DataCompare.Web
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
