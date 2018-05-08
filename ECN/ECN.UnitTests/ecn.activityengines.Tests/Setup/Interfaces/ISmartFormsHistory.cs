using ECN_Framework_Entities.Communicator;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface ISmartFormsHistory
    {
        SmartFormsHistory GetBySmartFormID_NoAccessCheck(int formId, int groupId);
    }
}
