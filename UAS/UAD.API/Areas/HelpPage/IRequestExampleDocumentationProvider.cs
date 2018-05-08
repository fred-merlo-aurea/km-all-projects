using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;

namespace UAD.API.Areas.HelpPage
{
    interface IExampleDocumentationProvider
    {
        string GetRequestExampleDocumentation(HttpActionDescriptor actionDescriptor);
        string GetResponseExampleDocumentation(HttpActionDescriptor actionDescriptor);
    }
}