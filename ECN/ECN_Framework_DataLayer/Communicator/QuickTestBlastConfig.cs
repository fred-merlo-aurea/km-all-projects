using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    [DataContract]
    public class QuickTestBlastConfig
    {
        public static ECN_Framework_Entities.Communicator.QuickTestBlastConfig GetByBaseChannelID(int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_QuickTestBlastConfig_Select_BaseChannelID";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.QuickTestBlastConfig GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_QuickTestBlastConfig_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.QuickTestBlastConfig GetKMDefaultConfig()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_QuickTestBlastConfig_Select_KMDefaultConfig";
            return Get(cmd);
        }

        public static int Save(ECN_Framework_Entities.Communicator.QuickTestBlastConfig qtb, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_QuickTestBlastConfig_Save";
            cmd.Parameters.Add(new SqlParameter("@QTBCID", qtb.QTBCID));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", (object)qtb.BaseChannelID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerCanOverride", (object)qtb.CustomerCanOverride ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerDoesOverride", (object)qtb.CustomerDoesOverride ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelDoesOverride", (object)qtb.BaseChannelDoesOverride ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)qtb.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AllowAdhocEmails", (object)qtb.AllowAdhocEmails ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AutoCreateGroup", (object)qtb.AutoCreateGroup ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AutoArchiveGroup", (object)qtb.AutoArchiveGroup ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void RemoveBaseChannelOverrideForCustomer(int BaseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_QuickTestBlastConfig_RemoveBaseChannelOverrideForCustomer";
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", BaseChannelID));
            DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static ECN_Framework_Entities.Communicator.QuickTestBlastConfig Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.QuickTestBlastConfig retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.QuickTestBlastConfig();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.QuickTestBlastConfig>.CreateBuilder(rdr);
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
