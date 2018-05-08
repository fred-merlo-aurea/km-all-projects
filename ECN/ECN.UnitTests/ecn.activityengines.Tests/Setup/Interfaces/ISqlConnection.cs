namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface ISqlConnection
    {
        void Constructor(string connectionString);
        void Close();
        void Open();
    }
}
