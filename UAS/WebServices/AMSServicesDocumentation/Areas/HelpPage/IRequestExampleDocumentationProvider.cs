using System;
using System.Linq;
using System.Web.Http.Controllers;

namespace AMSServicesDocumentation.Areas.HelpPage
{
    interface IExampleDocumentationProvider
    {
        string GetRequestExampleDocumentation(HttpActionDescriptor actionDescriptor);
        string GetResponseExampleDocumentation(HttpActionDescriptor actionDescriptor);

        string GetCExampleDocumentation(HttpActionDescriptor actionDescriptor);

        string GetVbExampleDocumentation(HttpActionDescriptor actionDescriptor);
    }
}
