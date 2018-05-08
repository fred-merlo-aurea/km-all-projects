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
    public class UnsubscribeReasonDetail
    {
        //no paging for download
        public static List<ECN_Framework_Entities.Activity.Report.UnsubscribeReasonDetail> GetReport(string SearchField, string SearchCriteria, DateTime FromDate, DateTime ToDate, 
            int CustomerID, string Reason)
        {
            List<ECN_Framework_Entities.Activity.Report.UnsubscribeReasonDetail> retList = new List<ECN_Framework_Entities.Activity.Report.UnsubscribeReasonDetail>();
            string sqlQuery = "v_UnsubscribeReason_Detail_Report";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@FromDate", FromDate);
            cmd.Parameters.AddWithValue("@ToDate", ToDate);
            cmd.Parameters.AddWithValue("@SearchField", SearchField);
            cmd.Parameters.AddWithValue("@SearchCriteria", SearchCriteria);
            cmd.Parameters.AddWithValue("@Reason", Reason);

            using (SqlDataReader rdr = ECN_Framework_DataLayer.DataFunctions.ExecuteReader(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Activity.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Activity.Report.UnsubscribeReasonDetail>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        ECN_Framework_Entities.Activity.Report.UnsubscribeReasonDetail x = builder.Build(rdr);
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

        //paging for display
        public static DataSet GetPaging(string SearchField, string SearchCriteria, DateTime FromDate, DateTime ToDate, int CustomerID, string Reason, int PageSize, int CurrentPage)
        {
            DataSet dsReport = null;
            string sqlQuery = "v_UnsubscribeReason_Detail_Paging";

            using (SqlCommand cmd = new SqlCommand(sqlQuery))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                cmd.Parameters.AddWithValue("@SearchField", SearchField);
                cmd.Parameters.AddWithValue("@SearchCriteria", SearchCriteria);
                cmd.Parameters.AddWithValue("@Reason", Reason);
                cmd.Parameters.AddWithValue("@PageSize", PageSize);
                cmd.Parameters.AddWithValue("@CurrentPage", CurrentPage);

                dsReport = DataFunctions.GetDataSet(cmd, DataFunctions.ConnectionString.Activity.ToString());
            }

            return dsReport;
        }
    }
}

