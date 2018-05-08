using System.Collections.Generic;
using System.Data;

namespace UAS.UnitTests.FrameworkSubGen.DataAccess
{
    public class SqlCommandData
    {
        public CommandType CommandType { get; set; }
        public string CommandText { get; set; }
        public string ConnectionString { get; set; }
        public IDictionary<string, object> Parameters { get; } = new Dictionary<string, object>();
    }
}