using ECN_Framework_Entities.Communicator;
using static ECN_Framework_Common.Objects.Communicator.Enums;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface ILayoutBusiness
    {
        Layout GetByLayoutIDNoAccessCheck(int layoutId, bool getChildren);

        int GetLayoutUserId(int layoutId);

        string GetPreviewNoAccessCheck(
            int layoutId,
            ContentTypeCode contentTypeCode,
            bool isMobile,
            int customerId,
            int? emailId,
            int? groupId,
            int? blastId);
    }
}
