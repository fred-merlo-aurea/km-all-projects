using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace ECN_Framework_DataLayer.Publisher.Report
{
    [Serializable]
    public class ActivityDEReportDetails
    {
        public static DataSet GetList(int editionID, int blastID, string reportType, int pageNo, int pageSize)
        {
            string sqlQuery = "sp_GetDEreportDetails";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EditionID", editionID);
            cmd.Parameters.AddWithValue("@BlastID", blastID);
            cmd.Parameters.AddWithValue("@ReportType", reportType);
            cmd.Parameters.AddWithValue("@PageNo", pageNo);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            DataSet ds =  null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ds = DataFunctions.GetDataSet(cmd, DataFunctions.ConnectionString.Publisher.ToString());
                scope.Complete();
            }

            return ds;
        }
    }
}
