using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.activityengines.Tests.Setup.Interfaces;
using Shim = ECN_Framework_BusinessLayer.Communicator.ContentReplacement.Fakes.ShimRSSFeed;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class RSSFeedMock : Mock<IRSSFeed>
    {
        public RSSFeedMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.ReplaceStringRefInt32BooleanNullableOfInt32 = Replace;
        }

        private void Replace(ref string content, int customerID, bool isText, int? blastID)
        {
            Object.Replace(ref content, customerID, isText, blastID);
        }
    }
}
