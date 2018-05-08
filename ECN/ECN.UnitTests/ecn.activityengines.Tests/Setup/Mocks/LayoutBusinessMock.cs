using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecn.activityengines.Tests.Setup.Interfaces;
using Shim = ECN_Framework_BusinessLayer.Communicator.Fakes.ShimLayout;
using Moq;
using LayoutEntity = ECN_Framework_Entities.Communicator.Layout;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using System.Diagnostics.CodeAnalysis;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class LayoutBusinessMock : Mock<ILayoutBusiness>
    {
        public LayoutBusinessMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.GetByLayoutID_NoAccessCheckInt32Boolean = GetByLayoutIdNoAccessCheck;
            Shim.GetLayoutUserIDInt32 = GetLayoutUserId;
            Shim.GetPreviewNoAccessCheckInt32EnumsContentTypeCodeBooleanInt32NullableOfInt32NullableOfInt32NullableOfInt32
                = GetPreviewNoAccessCheck;
        }

        private string GetPreviewNoAccessCheck(
            int layoutId,
            ContentTypeCode contentTypeCode,
            bool isMobile,
            int customerId,
            int? emailId,
            int? groupId,
            int? blastId)
        {
            return Object.GetPreviewNoAccessCheck(layoutId, contentTypeCode, isMobile, customerId, emailId,
                groupId, blastId);
        }

        private int GetLayoutUserId(int layoutId)
        {
            return Object.GetLayoutUserId(layoutId);
        }

        private LayoutEntity GetByLayoutIdNoAccessCheck(int layoutId, bool getChildren)
        {
            return Object.GetByLayoutIDNoAccessCheck(layoutId, getChildren);
        }
    }
}
