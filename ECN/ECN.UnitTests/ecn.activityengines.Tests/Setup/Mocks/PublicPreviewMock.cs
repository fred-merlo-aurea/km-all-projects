using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using ecn.activityengines.Fakes;
using Moq;
using ecn.activityengines.Tests.Setup.Interfaces;
using Shim = ecn.activityengines.Fakes.ShimpublicPreview;



namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class PublicPreviewMock : Mock<IPublicPreview>
    {
        public PublicPreviewMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.AllInstances.WriteToLogString = WriteToLog;
            Shim.IsMobileBrowser = IsMobileBrowser;
            ShimHtmlBuilder.DoDynamicTagsStringIEnumerableOfStringDataRowInt32 = DoDynamicTags;
        }

        private string DoDynamicTags(string preview, IEnumerable<string> dynamicTags, DataRow row, int customerID)
        {
            return Object.DoDynamicTags(preview, dynamicTags.ToList(), row, customerID);
        }

        private bool IsMobileBrowser()
        {
            return Object.IsMobileBrowser();
        }

        private void WriteToLog(publicPreview publicPreview, string log)
        {
            Object.WriteToLog(log);
        }

        internal void VerifyLogs(string[] expectedLogs)
        {
            foreach (var log in expectedLogs)
            {
                Verify(preview => preview.WriteToLog(log));
            }
        }
    }
}
