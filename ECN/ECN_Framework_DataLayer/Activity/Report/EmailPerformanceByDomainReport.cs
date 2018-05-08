using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity.Report
{
    [Serializable]
    public class EmailPerformanceByDomainReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.EmailPerformanceByDomain> Get(int CustomerID, DateTime startdate, DateTime enddate, bool DrillDownOther)
        {
            List<ECN_Framework_Entities.Activity.Report.EmailPerformanceByDomain> retList = new List<ECN_Framework_Entities.Activity.Report.EmailPerformanceByDomain>();
            string sqlQuery = "v_EmailPerformanceByDomainReport";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@customerID", CustomerID);
            cmd.Parameters.AddWithValue("@startdate", startdate);
            cmd.Parameters.AddWithValue("@enddate", enddate);
            cmd.Parameters.AddWithValue("@DrillDownOther", DrillDownOther);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.EmailPerformanceByDomain>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Activity.Report.EmailPerformanceByDomain x = builder.Build(rdr);
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
