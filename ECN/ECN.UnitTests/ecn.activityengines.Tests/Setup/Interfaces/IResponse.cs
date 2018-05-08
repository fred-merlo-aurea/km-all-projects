namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IResponse
    {
        void Redirect(string url, bool endResponse);

        void Write(string content);
    }
}
