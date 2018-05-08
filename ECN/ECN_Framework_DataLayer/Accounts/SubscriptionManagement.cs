using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Data.SqlClient;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    [Serializable]
    public class SubscriptionManagement
    {
        public static List<ECN_Framework_Entities.Accounts.SubscriptionManagement> GetByBaseChannelID(int BaseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagement_Select_BaseChannelID";
            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);

            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Accounts.SubscriptionManagement GetBySubscriptionManagementID(int SMID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagement_Select_SMID";
            cmd.Parameters.AddWithValue("@SMID", SMID);

            return Get(cmd);
        }

        public static void Delete(int SMID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagement_Delete";
            cmd.Parameters.AddWithValue("@SMID", SMID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }

        public static bool Exists(int SMID, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagement_Exists_SMID_CustomerID";
            cmd.Parameters.AddWithValue("@SMID", SMID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()).ToString()) == 1 ? true : false;
        }

        public static bool Exists(string PageName, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagement_Exists_PageName_CustomerID";
            cmd.Parameters.AddWithValue("@PageName", PageName);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()).ToString()) == 1 ? true : false;
        }

        public static int Save(ECN_Framework_Entities.Accounts.SubscriptionManagement sm)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagement_Save";
            if (sm.SubscriptionManagementID > 0)
            {
                cmd.Parameters.AddWithValue("@SMID", sm.SubscriptionManagementID);
                cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedUserID", sm.UpdatedUserID);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@CreatedUserID", sm.CreatedUserID);
            }
            cmd.Parameters.AddWithValue("@Name", sm.Name);
            cmd.Parameters.AddWithValue("@AdminEmail", sm.AdminEmail);
            cmd.Parameters.AddWithValue("@BaseChannelID", sm.BaseChannelID);
            cmd.Parameters.AddWithValue("@EmailFooter", sm.EmailFooter);
            cmd.Parameters.AddWithValue("@EmailHeader", sm.EmailHeader);
            cmd.Parameters.AddWithValue("@PageHeader", sm.Header);
            cmd.Parameters.AddWithValue("@PageFooter", sm.Footer);
            cmd.Parameters.AddWithValue("@MSMessage", sm.MSMessage);
            cmd.Parameters.AddWithValue("@IncludeMSGroups", sm.IncludeMSGroups);
            cmd.Parameters.AddWithValue("@IsDeleted", sm.IsDeleted);
            cmd.Parameters.AddWithValue("@UseReasonDropDown", sm.UseReasonDropDown.HasValue ? sm.UseReasonDropDown.Value : false);
            cmd.Parameters.AddWithValue("@ReasonVisible", sm.ReasonVisible.HasValue ? sm.ReasonVisible.Value : false);
            cmd.Parameters.AddWithValue("@ReasonLabel", sm.ReasonLabel);
            cmd.Parameters.AddWithValue("@UseThankYou", sm.UseThankYou.HasValue ? sm.UseThankYou.Value : false);
            cmd.Parameters.AddWithValue("@UseRedirect", sm.UseRedirect.HasValue ? sm.UseRedirect.Value : false);
            cmd.Parameters.AddWithValue("@ThankYouLabel", sm.ThankYouLabel);
            cmd.Parameters.AddWithValue("@RedirectURL", sm.RedirectURL);
            cmd.Parameters.AddWithValue("@RedirectDelay", sm.RedirectDelay);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()).ToString());
        }

        private static List<ECN_Framework_Entities.Accounts.SubscriptionManagement> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.SubscriptionManagement> retList = new List<ECN_Framework_Entities.Accounts.SubscriptionManagement>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.SubscriptionManagement retItem = new ECN_Framework_Entities.Accounts.SubscriptionManagement();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.SubscriptionManagement>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Accounts.SubscriptionManagement Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.SubscriptionManagement retItem = new ECN_Framework_Entities.Accounts.SubscriptionManagement();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.SubscriptionManagement>.CreateBuilder(rdr);
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
    }
}
