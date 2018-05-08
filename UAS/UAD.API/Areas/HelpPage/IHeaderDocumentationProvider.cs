using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace UAD.API.Areas.HelpPage
{
    interface IHeaderDocumentationProvider
    {
        IEnumerable<HeaderDocumentation> GetRequestHeaderDocumentation(HttpActionDescriptor actionDescriptor);
        IEnumerable<HeaderDocumentation> GetResponseHeaderDocumentation(HttpActionDescriptor actionDescriptor);
    }
}
