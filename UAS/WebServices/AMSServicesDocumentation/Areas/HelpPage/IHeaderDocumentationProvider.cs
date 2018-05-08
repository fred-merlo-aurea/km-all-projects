using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;

namespace AMSServicesDocumentation.Areas.HelpPage
{
    interface IHeaderDocumentationProvider
    {
        IEnumerable<HeaderDocumentation> GetRequestHeaderDocumentation(HttpActionDescriptor actionDescriptor);
        IEnumerable<HeaderDocumentation> GetResponseHeaderDocumentation(HttpActionDescriptor actionDescriptor);
    }
}
