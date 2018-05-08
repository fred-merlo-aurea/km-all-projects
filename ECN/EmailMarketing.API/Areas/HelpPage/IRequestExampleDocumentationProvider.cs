using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace EmailMarketing.API.Areas.HelpPage
{
    interface IExampleDocumentationProvider
    {
        string GetRequestExampleDocumentation(HttpActionDescriptor actionDescriptor);
        string GetResponseExampleDocumentation(HttpActionDescriptor actionDescriptor);
    }
}
