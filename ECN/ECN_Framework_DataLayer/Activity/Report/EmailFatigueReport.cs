using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework_DataLayer.Activity.Report
{
    public class EmailFatigueReport
    {
        public static List<ECN_Framework_Entities.Activity.Report.EmailFatigueReport> GetReport(int customerID, DateTime startDate, DateTime endDate, string filterField, int filterID)
        {
            List<ECN_Framework_Entities.Activity.Report.EmailFatigueReport> retList = new List<ECN_Framework_Entities.Activity.Report.EmailFatigueReport>();
            string sqlQuery = "rpt_EmailFatigueReport";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            if (filterField.Length > 0)
            {
                cmd.Parameters.AddWithValue("@FilterField", filterField);
            }
            if (filterID > 0)
            {
                cmd.Parameters.AddWithValue("@FilterValue", filterID.ToString());
            }

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.EmailFatigueReport>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Activity.Report.EmailFatigueReport x = builder.Build(rdr);
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
        public static DataTable Download(int customerID, DateTime startDate, DateTime endDate, string filterField, int filterID,string actionType, int grouping, int numberOfTouches)
        {
            List<ECN_Framework_Entities.Activity.Report.EmailFatigueReport> retList = new List<ECN_Framework_Entities.Activity.Report.EmailFatigueReport>();
            string sqlQuery = "rpt_EmailFatigueReport_Download";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            cmd.Parameters.AddWithValue("@ActionType", actionType);
            cmd.Parameters.AddWithValue("@Grouping", grouping);
            cmd.Parameters.AddWithValue("@NumberOfTouches", numberOfTouches);

            if (filterField.Length > 0)
            {
                cmd.Parameters.AddWithValue("@FilterField", filterField);
            }
            if (filterID > 0)
            {
                cmd.Parameters.AddWithValue("@FilterValue", filterID.ToString());
            }

            DataTable dt = DataFunctions.GetDataTable(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString());
            return dt;
        }
    }
}
