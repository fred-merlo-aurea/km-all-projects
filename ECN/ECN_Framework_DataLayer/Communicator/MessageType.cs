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
    public class MessageType
    {
        public static bool Exists(int messageTypeID, int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "IF EXISTS (SELECT TOP 1 MessageTypeID from MessageType where BaseChannelID = @BaseChannelID and MessageTypeID = @MessageTypeID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@MessageTypeID", messageTypeID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(string name, int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "IF EXISTS (SELECT TOP 1 MessageTypeID from MessageType where BaseChannelID = @BaseChannelID and Name = @Name AND IsDeleted = 0) SELECT 1 ELSE SELECT 0";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@Name", name);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int messageTypeID, string name, int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MessageType_Exists_ByName";
            cmd.Parameters.AddWithValue("@MessageTypeID", messageTypeID);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Communicator.MessageType GetByMessageTypeID(int messageTypeID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM MessageType WHERE MessageTypeID = @MessageTypeID AND IsDeleted = 0";
            cmd.Parameters.AddWithValue("@MessageTypeID", messageTypeID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.MessageType> GetActivePriority(bool isActive, bool isPriority, int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM MessageType WHERE BaseChannelID = @BaseChannelID AND Priority = @Priority AND IsActive = @IsActive AND IsDeleted = 0";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@Priority", isPriority);
            cmd.Parameters.AddWithValue("@IsActive", isActive);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.MessageType> GetByBaseChannelID(int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM MessageType WHERE BaseChannelID = @BaseChannelID AND IsDeleted = 0";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            return GetList(cmd);
        }
        
        //do not use, customer id is not valid for this object
        //public static List<ECN_Framework_Entities.Communicator.MessageType> GetByCustomerID(int customerID)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.Text;
        //    cmd.CommandText = "SELECT * FROM MessageType WHERE CustomerID = @CustomerID AND IsDeleted = 0";
        //    cmd.Parameters.AddWithValue("@CustomerID", customerID);
        //    return GetList(cmd);
        //}

        private static ECN_Framework_Entities.Communicator.MessageType Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.MessageType retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.MessageType();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.MessageType>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.MessageType> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.MessageType> messageTypeList = new List<ECN_Framework_Entities.Communicator.MessageType>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.MessageType retItem = new ECN_Framework_Entities.Communicator.MessageType();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.MessageType>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            messageTypeList.Add(retItem);
                        }
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return messageTypeList;
        }

        public static void UpdateSortOrder(int messageTypeID, int sortOrder, int baseChannelID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MessageType_UpdateSortOrder";
            cmd.Parameters.AddWithValue("@MessageTypeID", messageTypeID);
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@SortOrder", sortOrder);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static int GetMaxSortOrder(int baseChannelID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select MAX(SortOrder) + 1 from MessageType where Priority = 1 and IsActive = 1 and BaseChannelID = @BaseChannelID and IsDeleted = 0";
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", baseChannelID));
            string sortOrder=DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString();
            if (!sortOrder.Equals(""))
                return Convert.ToInt32(sortOrder);
            else
                return 1;
        }

        public static void Delete(int messageTypeID, int baseChannelID, int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MessageType_Delete_Single";
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@MessageTypeID", messageTypeID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        //customer id is not valid for this object
        public static int Save(ECN_Framework_Entities.Communicator.MessageType type)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MessageType_Save";
            cmd.Parameters.Add(new SqlParameter("@MessageTypeID", type.MessageTypeID));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", (object)type.BaseChannelID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Name", type.Name));
            cmd.Parameters.Add(new SqlParameter("@Description", type.Description));
            cmd.Parameters.Add(new SqlParameter("@Threshold", (object)type.Threshold ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Priority", (object)type.Priority ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PriorityNumber", (object)type.PriorityNumber ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SortOrder", (object)type.SortOrder ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsActive", (object)type.IsActive ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", DBNull.Value));
            if (type.MessageTypeID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)type.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)type.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }
    }
}
