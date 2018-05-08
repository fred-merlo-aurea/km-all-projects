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
    public class FlashReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.FlashReport> GetList(int groupID, int CustomerID, string PromoCode, DateTime FromDate, DateTime ToDate)
        {
            List<ECN_Framework_Entities.Activity.Report.FlashReport> retList = new List<ECN_Framework_Entities.Activity.Report.FlashReport>();
            string sqlQuery = "sp_KMPS_FlashReporting";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@pubGroupID", SqlDbType.Int));
            cmd.Parameters["@pubGroupID"].Value = groupID;

            cmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
            cmd.Parameters["@CustomerID"].Value = CustomerID;

            cmd.Parameters.Add(new SqlParameter("@PromoCode", SqlDbType.VarChar, 25));
            cmd.Parameters["@PromoCode"].Value = PromoCode.Trim();

            cmd.Parameters.Add(new SqlParameter("@fromdt", SqlDbType.Date));
            cmd.Parameters["@fromdt"].Value = FromDate;

            cmd.Parameters.Add(new SqlParameter("@todt", SqlDbType.Date));
            cmd.Parameters["@todt"].Value = ToDate;


            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.FlashReport>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Activity.Report.FlashReport x = builder.Build(rdr);
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
