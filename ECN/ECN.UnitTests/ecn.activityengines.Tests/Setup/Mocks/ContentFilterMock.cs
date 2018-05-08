using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.activityengines.Tests.Setup.Interfaces;
using Shim = ECN_Framework_BusinessLayer.Communicator.Fakes.ShimContentFilter;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class ContentFilterMock : Mock<IContentFilter>
    {
        public ContentFilterMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.HasDynamicContentInt32 = HasDynamicContent;
        }

        private bool HasDynamicContent(int layoutId)
        {
            return Object.HasDynamicContent(layoutId);
        }
    }
}
