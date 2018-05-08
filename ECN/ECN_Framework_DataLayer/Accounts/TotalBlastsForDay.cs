using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    public class TotalBlastsForDay
    {
        public static List<ECN_Framework_Entities.Accounts.Report.TotalBlastsForDay> Get(DateTime startDate)
        {
            List<ECN_Framework_Entities.Accounts.Report.TotalBlastsForDay> retList = new List<ECN_Framework_Entities.Accounts.Report.TotalBlastsForDay>();
            
            string query = "rpt_TotalBlastsForDay";
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Date", startDate);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.Report.TotalBlastsForDay>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Accounts.Report.TotalBlastsForDay x = builder.Build(rdr);
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
