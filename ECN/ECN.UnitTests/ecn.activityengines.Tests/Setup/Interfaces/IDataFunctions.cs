using System.Data;

namespace ecn.activityengines.Tests.Setup.Interfaces
{
    public interface IDataFunctions
    {
        DataTable GetDataTable(string query);
        string CleanString(string input);
    }
}
