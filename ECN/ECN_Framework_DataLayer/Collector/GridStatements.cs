using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Collector
{
    [Serializable]
    public class GridStatements
    {
        public static List<ECN_Framework_Entities.Collector.GridStatements> GetByQuestionID(int QuestionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Grid_Statements with (NOLOCK) where QuestionID=@QuestionID";
            cmd.Parameters.AddWithValue("@QuestionID", QuestionID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Collector.GridStatements Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Collector.GridStatements retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Collector.GridStatements();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Collector.GridStatements>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }

        private static List<ECN_Framework_Entities.Collector.GridStatements> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Collector.GridStatements> retList = new List<ECN_Framework_Entities.Collector.GridStatements>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Collector.GridStatements retItem = new ECN_Framework_Entities.Collector.GridStatements();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Collector.GridStatements>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        retList.Add(retItem);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }
    }
}
