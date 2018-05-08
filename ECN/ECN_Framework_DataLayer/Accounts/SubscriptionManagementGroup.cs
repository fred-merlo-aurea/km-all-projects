using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    [Serializable]

    public class SubscriptionManagementGroup
    {
        public static List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup> GetBySMID(int SMID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagementGroup_Select_SMID";
            cmd.Parameters.AddWithValue("@SMID", SMID);

            return GetList(cmd);
        }

        public static void Delete(int SMID, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagementGroup_Delete_SMID";
            cmd.Parameters.AddWithValue("@SMID", SMID);
            cmd.Parameters.AddWithValue("@UserID", UserID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }

        public static void Delete(int SMID,int SMGID, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagementGroup_Delete_SMID_SMGID";
            cmd.Parameters.AddWithValue("@SMID", SMID);
            cmd.Parameters.AddWithValue("@SMGID", SMGID);
            cmd.Parameters.AddWithValue("@UserID", UserID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }

        public static bool Exists(int SMGID, int SMID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagementGroup_Exists_SMGID_SMID";
            cmd.Parameters.AddWithValue("@SMGID", SMGID);
            cmd.Parameters.AddWithValue("@SMID", SMID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static int Save(ECN_Framework_Entities.Accounts.SubscriptionManagementGroup SMGroup)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SubscriptionManagementGroup_Save";

            cmd.Parameters.AddWithValue("@SMGID", SMGroup.SubscriptionManagementGroupID);
            cmd.Parameters.AddWithValue("@SMID", SMGroup.SubscriptionManagementPageID);
            cmd.Parameters.AddWithValue("@GroupID", SMGroup.GroupID);
            cmd.Parameters.AddWithValue("@IsDeleted", SMGroup.IsDeleted);
            cmd.Parameters.AddWithValue("@Label", SMGroup.Label);

            if (SMGroup.SubscriptionManagementGroupID > 0)
            {
                cmd.Parameters.AddWithValue("@UpdatedUserID", SMGroup.UpdatedUserID);
                cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CreatedUserID", SMGroup.CreatedUserID);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            }

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()));
        }


        private static List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup> retList = new List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.SubscriptionManagementGroup retItem = new ECN_Framework_Entities.Accounts.SubscriptionManagementGroup();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup>.CreateBuilder(rdr);
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
