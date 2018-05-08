using System.Web;
using System.Web.Mvc;

namespace EmailMarketing.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new Attributes.ExceptionsLoggedAttribute());

            //var exceptionConfig = new System.Collections.Generic.List<ExceptionConfiguration>();
            //exceptionConfig.Add(new ExceptionConfiguration(typeof(System.Web.Http.HttpResponseException),"An error occured",System.Net.HttpStatusCode.InternalServerError));

            //filters.Add(new DetailedExceptionFilterAttribute(null, true) { Order = 1 });
            //filters.Add(new DetailedExceptionFilterAttribute() { Order = 1 });
        }
    }
}