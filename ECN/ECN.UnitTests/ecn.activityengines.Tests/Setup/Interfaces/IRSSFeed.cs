namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IRSSFeed
    {
        void Replace(ref string content, int customerID, bool isText, int? blastID);
    }
}
