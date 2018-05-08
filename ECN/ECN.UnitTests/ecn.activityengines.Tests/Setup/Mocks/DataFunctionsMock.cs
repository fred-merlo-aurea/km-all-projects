using System.Data;
using System.Diagnostics.CodeAnalysis;
using ecn.activityengines.Tests.Setup.Interfaces;
using Moq;
using Shim = ecn.common.classes.Fakes.ShimDataFunctions;

namespace ecn.activityengines.Tests.Setup.Mocks
{
    [ExcludeFromCodeCoverage]
    public class DataFunctionsMock : Mock<IDataFunctions>
    {
        public DataFunctionsMock()
        {
            SetupShims();
        }

        private void SetupShims()
        {
            Shim.GetDataTableString = GetDataTable;
            Shim.CleanStringString = CleanString;
        }

        private string CleanString(string input)
        {
            return Object.CleanString(input);
        }

        private DataTable GetDataTable(string query)
        {
            return Object.GetDataTable(query);
        }
    }
}
