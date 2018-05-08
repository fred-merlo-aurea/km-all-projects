using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity.Report
{
    [Serializable]
    public class OnOffsByField
    {
        public static List<ECN_Framework_Entities.Activity.Report.OnOffsByField> Get(int groupID, string field, DateTime startdate, DateTime enddate, string reporttype)
        {
            List<ECN_Framework_Entities.Activity.Report.OnOffsByField> retList = new List<ECN_Framework_Entities.Activity.Report.OnOffsByField>();
            string sqlQuery = "sp_OnOffsByField";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupID", groupID);
            cmd.Parameters.AddWithValue("@field", field);
            cmd.Parameters.AddWithValue("@startdate", startdate);
            cmd.Parameters.AddWithValue("@enddate", enddate);
            cmd.Parameters.AddWithValue("@reporttype", reporttype);
            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.OnOffsByField>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Activity.Report.OnOffsByField x = builder.Build(rdr);
                        retList.Add(x);
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
