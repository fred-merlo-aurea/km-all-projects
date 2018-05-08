using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using DataFunctions = ECN_Framework_DataLayer.DataFunctions;
using KM.Common;

namespace ECN_Framework.Accounts.Report
{
    [Serializable]
    public abstract class ReportBase<T>
    {
        public int BaseChannelID { get; set; }

        public string BaseChannelName { get; set; }

        public string CustomerName { get; set; }

        public DateTime CreatedDate { get; set; }

        public int Usage { get; set; }

        public static IList<T> Get(int month, int year, bool testBlast, string sqlQuery)
        {
            var retList = new List<T>();
            var cmd = new SqlCommand(sqlQuery)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@testblast", testBlast == true ? "Y" : "N");

                using (var dataReader = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
                {
                    if (dataReader != null)
                    {
                        var builder = DynamicBuilder<T>.CreateBuilder(dataReader);

                        while (dataReader.Read())
                        {
                            var item = builder.Build(dataReader);
                            retList.Add(item);
                        }
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }

            return retList;
        }
    }
}