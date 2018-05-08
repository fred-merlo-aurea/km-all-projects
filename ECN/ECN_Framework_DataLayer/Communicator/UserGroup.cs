using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class UserGroup
    {
        public static bool Exists(int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserGroups_Exists_ByUserID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Communicator.UserGroup> GetByUserID(int userID)
        {
            List<ECN_Framework_Entities.Communicator.UserGroup> retItemList = new List<ECN_Framework_Entities.Communicator.UserGroup>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserGroups_Select_ByUserID";
            cmd.Parameters.AddWithValue("@userID", userID);

            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.UserGroup GetSingle(int userID, int groupID)
        {
            List<ECN_Framework_Entities.Communicator.UserGroup> retItemList = new List<ECN_Framework_Entities.Communicator.UserGroup>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserGroups_Select_Single";
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@GroupID", groupID);

            return Get(cmd);
        }

        private static ECN_Framework_Entities.Communicator.UserGroup Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.UserGroup retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.UserGroup();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.UserGroup>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.UserGroup> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.UserGroup> retList = new List<ECN_Framework_Entities.Communicator.UserGroup>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.UserGroup retItem = new ECN_Framework_Entities.Communicator.UserGroup();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.UserGroup>.CreateBuilder(rdr);
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

        public static int Save(ECN_Framework_Entities.Communicator.UserGroup userGroup)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserGroups_Save";
            cmd.CommandTimeout = 0;
            cmd.Parameters.Add(new SqlParameter("@UGID", (object)userGroup.UGID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UserID", userGroup.UserID));
            cmd.Parameters.Add(new SqlParameter("@GroupID", userGroup.GroupID));
            if (userGroup.UGID > 0)
                cmd.Parameters.Add(new SqlParameter("@LoggingUserID", (object)userGroup.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@LoggingUserID", (object)userGroup.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void DeleteByUserID(int userID, int loggingUserID)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserGroups_Delete_UserID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@LoggingUserID", loggingUserID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void DeleteByUserID_CustomerID(int userID, int customerID, int loggingUserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserGroups_Delete_UserID_CustomerID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@LoggingUserID", loggingUserID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

    }
}
