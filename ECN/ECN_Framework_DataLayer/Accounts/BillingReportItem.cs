using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    public class BillingReportItem
    {
        public static List<ECN_Framework_Entities.Accounts.BillingReportItem> GetItemsByBillingReportID(int BillingReportID)
        {
            SqlCommand cmd = new SqlCommand("e_BillingReportItem_Select_BillingReportID");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BillingReportID", BillingReportID);

            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Accounts.BillingReportItem GetByBillingReportItemID(int BillingReportItemID)
        {
            SqlCommand cmd = new SqlCommand("e_BillingReportItem_Select_BillingReportItemID");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BillingReportItemID", BillingReportItemID);

            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Accounts.BillingReportItem> GetEmailUsageReport(string CustomerIDs, DateTime startDate, DateTime endDate, string fieldsToInclude, string columnSQL)
        {
            string query = "e_BillingReportItem_GetEmailUsage";
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerIDs", CustomerIDs);
            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            cmd.Parameters.AddWithValue("@FieldsToInclude", fieldsToInclude);
            cmd.Parameters.AddWithValue("@ColumnSQL", columnSQL);
            return GetList(cmd);
        }

        public static int Save(ECN_Framework_Entities.Accounts.BillingReportItem bri)
        {
            string query = "e_BillingReportItem_Save";
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BillingReportID", bri.BillingReportID);
            cmd.Parameters.AddWithValue("@Amount", bri.Amount);
            cmd.Parameters.AddWithValue("@IsFlatRateItem", bri.IsFlatRateItem);
            cmd.Parameters.AddWithValue("@ItemName", bri.InvoiceText);
            cmd.Parameters.AddWithValue("@BaseChannelName", bri.BaseChannelName);
            cmd.Parameters.AddWithValue("@BaseChannelID", bri.BaseChannelID);
            cmd.Parameters.AddWithValue("@CustomerName", bri.CustomerName);
            cmd.Parameters.AddWithValue("@CustomerID", bri.CustomerID);
            cmd.Parameters.AddWithValue("@IsDeleted", bri.IsDeleted);
            if (bri.BillingItemID > 0)
            {
                cmd.Parameters.AddWithValue("@BillingReportItemID", bri.BillingItemID);
                cmd.Parameters.AddWithValue("@UpdatedUserID", bri.UpdatedUserID);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CreatedUserID", bri.CreatedUserID);
            }
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()));
        }

        private static ECN_Framework_Entities.Accounts.BillingReportItem Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.BillingReportItem bri = new ECN_Framework_Entities.Accounts.BillingReportItem();
            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.BillingReportItem>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        bri = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }

            cmd.Connection.Close();
            cmd.Dispose();
            return bri;
        }

        private static List<ECN_Framework_Entities.Accounts.BillingReportItem> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.BillingReportItem> retList = new List<ECN_Framework_Entities.Accounts.BillingReportItem>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.BillingReportItem retItem = new ECN_Framework_Entities.Accounts.BillingReportItem();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.BillingReportItem>.CreateBuilder(rdr);
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
