using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.activityengines.Tests.Setup.Interfaces;
using Shim = ECN_Framework_BusinessLayer.Communicator.Fakes.ShimContent;
using Moq;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class ContentMock : Mock<IContent>
    {
        public ContentMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.GetTagsListOfStringBoolean = GetTags;
            Shim.CheckForTransnippetString = CheckForTransnippet;
            Shim.ModifyHTMLStringDataTable = ModifyHTML;
        }

        private string ModifyHTML(string htmlOriginal, DataTable emailProfil)
        {
            return Object.ModifyHTML(htmlOriginal, emailProfil);
        }

        private int CheckForTransnippet(string html)
        {
            return Object.CheckForTransnippet(html);
        }

        private List<string> GetTags(List<string> toParse, bool fullTag)
        {
            return Object.GetTags(toParse, fullTag);
        }
    }
}
