using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator.Report
{
    [Serializable]
    public class ListSizeOvertimeReport
    {
        public static List<ECN_Framework_Entities.Communicator.Report.ListSizeOvertimeReport> Get(int groupID, DateTime startdate, DateTime enddate)
        {
            List<ECN_Framework_Entities.Communicator.Report.ListSizeOvertimeReport> retList = new List<ECN_Framework_Entities.Communicator.Report.ListSizeOvertimeReport>();
            string sqlQuery = "v_ListSizeOvertimeReport";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@groupID", groupID);
            cmd.Parameters.AddWithValue("@startdate", startdate);
            cmd.Parameters.AddWithValue("@enddate", enddate);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Report.ListSizeOvertimeReport>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Communicator.Report.ListSizeOvertimeReport x = builder.Build(rdr);
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
