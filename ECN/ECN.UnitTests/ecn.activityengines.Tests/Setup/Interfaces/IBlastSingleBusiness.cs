namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IBlastSingleBusiness
    {
        int GetRefBlastID(int blastId, int emailId, int customerId, string blastType);
    }
}
