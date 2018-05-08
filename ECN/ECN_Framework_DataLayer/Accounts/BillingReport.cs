using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    public class BillingReport
    {
        public static ECN_Framework_Entities.Accounts.BillingReport GetByBillingReportID(int BillingReportID)
        {
            SqlCommand cmd = new SqlCommand("e_BillingReport_Select_BillingReportID");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BillingReportID", BillingReportID);

            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Accounts.BillingReport> GetAll()
        {
            SqlCommand cmd = new SqlCommand("e_BillingReport_Select_All");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            return GetList(cmd);
        }

        public static int Save(ECN_Framework_Entities.Accounts.BillingReport br)
        {
            SqlCommand cmd = new SqlCommand("e_BillingReport_Save");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerIDs", br.CustomerIDs);
            cmd.Parameters.AddWithValue("@ReportName", br.BillingReportName);
            cmd.Parameters.AddWithValue("@IncludeFulfillment", br.IncludeFulfillment);
            cmd.Parameters.AddWithValue("@IncludeMasterFile", br.IncludeMasterFile);
            if(br.StartDate.HasValue)
                cmd.Parameters.AddWithValue("@StartDate", br.StartDate);
            if(br.EndDate.HasValue)
                cmd.Parameters.AddWithValue("@EndDate", br.EndDate);
            cmd.Parameters.AddWithValue("@IsRecurring", br.IsRecurring);
            cmd.Parameters.AddWithValue("@RecurrenceType", br.RecurrenceType);
            //cmd.Parameters.AddWithValue("@EmailBillingRate", br.EmailBillingRate);
            //cmd.Parameters.AddWithValue("@MasterFileRate", br.MasterFileRate);
            //cmd.Parameters.AddWithValue("@FulfillmentRate", br.FulfillmentRate);
            cmd.Parameters.AddWithValue("@FromEmail", br.FromEmail);
            cmd.Parameters.AddWithValue("@ToEmail", br.ToEmail);
            cmd.Parameters.AddWithValue("@FromName", br.FromName);
            cmd.Parameters.AddWithValue("@Subject", br.Subject);
            cmd.Parameters.AddWithValue("@IsDeleted", br.IsDeleted);
            cmd.Parameters.AddWithValue("@BaseChannelID", br.BaseChannelID);
            cmd.Parameters.AddWithValue("@BlastFields", br.BlastFields);
            cmd.Parameters.AddWithValue("@AllCustomers", br.AllCustomers);
            if (br.BillingReportID > 0)
            {
                cmd.Parameters.AddWithValue("@BillingReportID", br.BillingReportID);
                cmd.Parameters.AddWithValue("@UpdatedUserID", br.UpdatedUserID);

            }
            else
                cmd.Parameters.AddWithValue("@CreatedUserID", br.CreatedUserID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()));
        }

        public static void Update(int BillingReportID, string customerIDs, string reportName, bool includeFulfillment, bool includeMasterFile, DateTime StartDate, DateTime EndDate,bool isRecurring, string recurrenceType, double EmailBillingRate, double masterFileRate, double fulfillmentRate, string FromEmail, string toEmail, string FromName, string subject, bool isDeleted)
        {
            SqlCommand cmd = new SqlCommand("e_BillingReport_Update");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BillingReportID", BillingReportID);
            cmd.Parameters.AddWithValue("@CustomerIDs", customerIDs);
            cmd.Parameters.AddWithValue("@ReportName", reportName);
            cmd.Parameters.AddWithValue("@IncludeFulfillment", includeFulfillment);
            cmd.Parameters.AddWithValue("@IncludeMasterFile", includeMasterFile);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            cmd.Parameters.AddWithValue("@IsRecurring", isRecurring);
            cmd.Parameters.AddWithValue("@RecurrenceType", recurrenceType);
            cmd.Parameters.AddWithValue("@EmailBillingRate", EmailBillingRate);
            cmd.Parameters.AddWithValue("@MasterFileRate", masterFileRate);
            cmd.Parameters.AddWithValue("@FulfillmentRate", fulfillmentRate);
            cmd.Parameters.AddWithValue("@FromEmail", FromEmail);
            cmd.Parameters.AddWithValue("@ToEmail", toEmail);
            cmd.Parameters.AddWithValue("@FromName", FromName);
            cmd.Parameters.AddWithValue("@Subject", subject);
            cmd.Parameters.AddWithValue("@IsDeleted", isDeleted);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }

        private static ECN_Framework_Entities.Accounts.BillingReport Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.BillingReport retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Accounts.BillingReport();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.BillingReport>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Accounts.BillingReport> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.BillingReport> retList = new List<ECN_Framework_Entities.Accounts.BillingReport>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.BillingReport retItem = new ECN_Framework_Entities.Accounts.BillingReport();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.BillingReport>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            retList.Add(retItem);
                        }
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
