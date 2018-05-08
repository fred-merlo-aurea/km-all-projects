using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    [Serializable]
    [DataContract]
    public class SFSettings
    {
        public static ECN_Framework_Entities.Accounts.SFSettings GetOneToUse(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SFSettings_Select_GetOneToUse";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Get(cmd);
        }

        public static int Save(ECN_Framework_Entities.Accounts.SFSettings sfs, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SFSettings_Save";
            cmd.Parameters.Add(new SqlParameter("@SFSettingsID", sfs.SFSettingsID));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", (object)sfs.BaseChannelID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerCanOverride", (object)sfs.CustomerCanOverride ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerDoesOverride", (object)sfs.CustomerDoesOverride ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PushChannelMasterSuppression", (object)sfs.PushChannelMasterSuppression ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SandboxMode", (object)sfs.SandboxMode ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)sfs.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@RefreshToken", (object)sfs.RefreshToken ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ConsumerKey", (object)sfs.ConsumerKey ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ConsumerSecret", (object)sfs.ConsumerSecret ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()).ToString());
        }


        public static void RemoveBaseChannelOverrideForCustomer(int BaseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SFSettings_RemoveBaseChannelOverrideForCustomer";
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", BaseChannelID));
            DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }

        private static ECN_Framework_Entities.Accounts.SFSettings Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.SFSettings retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Accounts.SFSettings();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.SFSettings>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Accounts.SFSettings> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.SFSettings> retList = new List<ECN_Framework_Entities.Accounts.SFSettings>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.SFSettings retItem = new ECN_Framework_Entities.Accounts.SFSettings();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.SFSettings>.CreateBuilder(rdr);
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

        public static List<ECN_Framework_Entities.Accounts.SFSettings> GetCMSBaseChannels()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SFSettings_Select_CMSBaseChannels";
            return GetList(cmd);
        }
    }
}
