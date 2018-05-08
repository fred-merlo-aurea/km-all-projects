using System.Collections.Generic;
using System.Data.SqlClient;

namespace ECN_Framework_DataLayer.Communicator.Helpers
{
    public static class SqlCommandCommunicator<TCommunicator> where TCommunicator : class, new()
    {
        public static TCommunicator Get(SqlCommand cmd)
        {
            TCommunicator retItem = null;

            try
            {
                using (var dataReader = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
                {
                    if (dataReader != null)
                    {
                        retItem = new TCommunicator();
                        var builder = DynamicBuilder<TCommunicator>.CreateBuilder(dataReader);
                        while (dataReader.Read())
                        {
                            retItem = builder.Build(dataReader);
                        }
                    }
                }
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return retItem;
        }

        public static List<TCommunicator> GetList(SqlCommand cmd)
        {
            var retList = new List<TCommunicator>();

            try
            {
                using (var dataReader = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
                {
                    if (dataReader != null)
                    {
                        var retItem = new TCommunicator();
                        var builder = DynamicBuilder<TCommunicator>.CreateBuilder(dataReader);
                        while (dataReader.Read())
                        {
                            retItem = builder.Build(dataReader);
                            if (retItem != null)
                            {
                                retList.Add(retItem);
                            }
                        }
                    }
                }
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }

            return retList;
        }
    }
}
