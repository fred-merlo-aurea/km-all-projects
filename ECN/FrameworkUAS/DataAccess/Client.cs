using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class Client
    {
        public static List<Entity.Client> Select()
        {
            List<Entity.Client> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Client> SelectAMS(bool isAms, bool isActive)
        {
            List<Entity.Client> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_Select_AMS";
            cmd.Parameters.AddWithValue("@IsAms", isAms);
            cmd.Parameters.AddWithValue("@IsActive", isActive);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Client> AMS_SelectPaid()
        {
            List<Entity.Client> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_Select_AMSPaid";

            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.Client Select(string clientName)
        {
            Entity.Client retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_Select_ClientName";
            cmd.Parameters.AddWithValue("@ClientName", clientName);

            retItem = Get(cmd);
            return retItem;
        }
        public static Entity.Client SelectFtpFolder(string ftpFolder)
        {
            Entity.Client retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_Select_FtpFolder";
            cmd.Parameters.AddWithValue("@FtpFolder", ftpFolder);

            retItem = Get(cmd);
            return retItem;
        }
        public static List<Object.Product> SelectProduct(KMPlatform.Entity.Client client)
        {
            List<Object.Product> retItem = new List<Object.Product>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_Product_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            if (!string.IsNullOrEmpty(client.ClientLiveDBConnectionString) && !string.IsNullOrEmpty(client.ClientTestDBConnectionString))
                retItem = GetProductList(cmd);
            return retItem;
        }

        public static Entity.Client SelectDefault(Guid accessKey)
        {
            Entity.Client retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_Select_DefaultClient_AccessKey";
            cmd.Parameters.AddWithValue("@AccessKey", accessKey);

            retItem = Get(cmd);
            return retItem;
        }
        public static List<Entity.Client> SelectForAccessKey(Guid accessKey)
        {
            List<Entity.Client> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_Select_AccessKey";
            cmd.Parameters.AddWithValue("@AccessKey", accessKey);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Client> SelectForClientGroup(int clientGroupID)
        {
            List<Entity.Client> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_Select_ClientGroupID";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Client> SelectActiveForClientGroup(int clientGroupID)
        {
            List<Entity.Client> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_SelectActive_ClientGroupID";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.Client> SelectbyUserID(int userID)
        {
            List<Entity.Client> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_Select_UserID";
            cmd.Parameters.AddWithValue("@userID", userID);

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.Client> SelectAllByUserID(int userID)
        {
            List<Entity.Client> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_SelectAll_UserID";
            cmd.Parameters.AddWithValue("@userID", userID);

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.Client> SelectbyUserIDclientgroupID(int userID, int clientgroupID)
        {
            List<Entity.Client> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_Select_UserID_ClientGroupID";
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@clientgroupID", clientgroupID);

            retItem = GetList(cmd);
            return retItem;
        }


        public static Entity.Client Select(int clientID)
        {
            Entity.Client retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);

            retItem = Get(cmd);
            return retItem;
        }
       
        public static Entity.Client Get(SqlCommand cmd)
        {
            Entity.Client retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Client();
                        var builder = DynamicBuilder<Entity.Client>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }
        public static List<Entity.Client> GetList(SqlCommand cmd)
        {
            List<Entity.Client> retList = new List<Entity.Client>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Client retItem = new Entity.Client();
                        var builder = DynamicBuilder<Entity.Client>.CreateBuilder(rdr);
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
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
        public static List<Object.Product> GetProductList(SqlCommand cmd)
        {
            List<Object.Product> retList = new List<Object.Product>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Object.Product retItem = new Object.Product();
                        var builder = DynamicBuilder<Object.Product>.CreateBuilder(rdr);
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
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
        public static int Save(Entity.Client x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_Save";
            cmd.Parameters.AddWithValue("@ClientID", x.ClientID);
            cmd.Parameters.AddWithValue("@ClientName", x.ClientName);
            cmd.Parameters.AddWithValue("@DisplayName", x.ClientName);
            cmd.Parameters.AddWithValue("@ClientCode", x.ClientCode);
            cmd.Parameters.AddWithValue("@ClientTestDBConnectionString", x.ClientTestDBConnectionString);
            cmd.Parameters.AddWithValue("@ClientLiveDBConnectionString", x.ClientLiveDBConnectionString);
            cmd.Parameters.AddWithValue("@IsActive", x.IsActive);
            cmd.Parameters.AddWithValue("@IgnoreUnknownFiles", x.IgnoreUnknownFiles);
            cmd.Parameters.AddWithValue("@AccountManagerEmails", x.AccountManagerEmails);
            cmd.Parameters.AddWithValue("@ClientEmails", x.ClientEmails);
            cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);
            cmd.Parameters.AddWithValue("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedByUserID", x.CreatedByUserID);
            cmd.Parameters.AddWithValue("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@HasPaid", x.HasPaid);
            cmd.Parameters.AddWithValue("@IsKMClient", x.IsKMClient);
            cmd.Parameters.AddWithValue("@ParentClientId", x.ParentClientId);
            cmd.Parameters.AddWithValue("@HasChildren", x.HasChildren);
            cmd.Parameters.AddWithValue("@AccessKey", x.AccessKey);
            cmd.Parameters.AddWithValue("@IsAMS", x.IsAMS);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        
        }

        public static void UpdateUsersForSecurityGroups(int clientID, int clientGroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_UserClientSecurityGroup_Insert_ClientGroupRoles";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);

            KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.KMPlatform.ToString());
        }
        public static bool HasService(int clientID, int clientGroupID, string serviceName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_HasService_ClientID_ServiceName";
            cmd.Parameters.Add(new SqlParameter("@ClientID", clientID));
            cmd.Parameters.Add(new SqlParameter("@ClientGroupID", clientGroupID));
            cmd.Parameters.Add(new SqlParameter("@ServiceName", serviceName));

            return Convert.ToBoolean(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }
        public static bool HasFeature(int clientID, int clientGroupID, string serviceName, string featureName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_HasServiceFeature_ClientID_ServiceName_FeatureName";
            cmd.Parameters.Add(new SqlParameter("@ClientID", clientID));
            cmd.Parameters.Add(new SqlParameter("@ServiceName", serviceName));
            cmd.Parameters.Add(new SqlParameter("@FeatureName", featureName));

            return Convert.ToBoolean(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }
        public static List<Entity.Client> SelectForAtLeastCustAdmin(int clientGroupID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_Select_User_CustAdmin";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            return GetList(cmd);
        }

        public static bool Exists(string clientName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_Exists_ClientName";
            cmd.Parameters.AddWithValue("@ClientName", clientName);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()).ToString()) > 0 ? true : false;
        }

        public static void Delete(int clientID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Client_Delete_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);

            KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.KMPlatform.ToString());
        }
    }
}
