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
    public class SmartFormsHistory
    {
        public static List<ECN_Framework_Entities.Communicator.SmartFormsHistory> GetByGroupID(int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SmartFormsHistory_Select_GroupID";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.SmartFormsHistory GetBySmartFormID(int smartFormID, int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SmartFormsHistory_Select_SmartFormID_GroupID";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@SmartFormID", smartFormID);
            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.SmartFormsHistory GetBySmartFormID(int smartFormID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SmartFormsHistory_Select_SmartFormID";
            cmd.Parameters.AddWithValue("@SmartFormID", smartFormID);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Communicator.SmartFormsHistory Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.SmartFormsHistory retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.SmartFormsHistory();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.SmartFormsHistory>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.SmartFormsHistory> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.SmartFormsHistory> retList = new List<ECN_Framework_Entities.Communicator.SmartFormsHistory>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.SmartFormsHistory retItem = new ECN_Framework_Entities.Communicator.SmartFormsHistory();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.SmartFormsHistory>.CreateBuilder(rdr);
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

        public static void Delete(int smartFormID, int customerID, int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SmartFormsHistory_Delete_Single";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@SmartFormID", smartFormID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static int Save(ECN_Framework_Entities.Communicator.SmartFormsHistory history)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SmartFormsHistory_Save";
            cmd.Parameters.Add(new SqlParameter("@SmartFormID", (object)history.SmartFormID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@GroupID", history.GroupID));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionGroupIDs", history.SubscriptionGroupIDs));
            cmd.Parameters.Add(new SqlParameter("@SmartFormName", history.SmartFormName));
            cmd.Parameters.Add(new SqlParameter("@SmartFormHtml", history.SmartFormHTML));
            cmd.Parameters.Add(new SqlParameter("@SmartFormFields", history.SmartFormFields));
            cmd.Parameters.Add(new SqlParameter("@Response_UserMsgSubject", history.Response_UserMsgSubject));
            cmd.Parameters.Add(new SqlParameter("@Response_UserMsgBody", history.Response_UserMsgBody));
            cmd.Parameters.Add(new SqlParameter("@Response_UserScreen", history.Response_UserScreen));
            cmd.Parameters.Add(new SqlParameter("@Response_FromEmail", history.Response_FromEmail));
            cmd.Parameters.Add(new SqlParameter("@Response_AdminEmail", history.Response_AdminEmail));
            cmd.Parameters.Add(new SqlParameter("@Response_AdminMsgSubject", history.Response_AdminMsgSubject));
            cmd.Parameters.Add(new SqlParameter("@Response_AdminMsgBody", history.Response_AdminMsgBody));
            cmd.Parameters.Add(new SqlParameter("@Type", history.Type));
            cmd.Parameters.Add(new SqlParameter("@DoubleOptIn", history.DoubleOptIn));
            if (history.SmartFormID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)history.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)history.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static bool Exists(int smartFormID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SmartFormsHistory_Exists_ByID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@SmartFormID", smartFormID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static int GetGroupID(int customerID, int smartFormID)
        {
            int groupID = 0;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "e_SmartFormsHistory_GetGroupID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters.AddWithValue("@SFID", smartFormID);
                groupID = Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()));
            }
            catch (Exception)
            {
            }

            return groupID;
        }
    }
}
