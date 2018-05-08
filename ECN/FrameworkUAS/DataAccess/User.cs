using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class User
    {
        public static List<Entity.User> Select()
        {
            List<Entity.User> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.User> Select(int clientID)
        {
            List<Entity.User> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.User> Select(int clientID, int securityGroupID, bool includeObjects = false)
        {
            List<Entity.User> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Select_ClientID_SecurityGroupID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.User> Select(int clientID, string securityGroupName, bool includeObjects = false)
        {
            List<Entity.User> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Select_ClientID_SecurityGroupName";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@SecurityGroupName", securityGroupName);

            retItem = GetList(cmd);
            return retItem;
        }


        public static List<Entity.User> SelectByClientGroupID(int clientgroupID)
        {
            List<Entity.User> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Select_ClientGroupID";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientgroupID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.User> SelectbyClientGroupIDServiceCode(int clientgroupID, KMPlatform.Enums.Services serviceCode)
        {
            List<Entity.User> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Select_ClientGroupID_ServiceCode";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientgroupID);
            cmd.Parameters.AddWithValue("@ServiceCode", serviceCode.ToString());

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.User> SelectbyClientIDServiceCode(int clientID, KMPlatform.Enums.Services serviceCode)
        {
            List<Entity.User> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Select_ClientID_ServiceCode";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@ServiceCode", serviceCode.ToString());

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.User> SelectByClientID(int clientID)
        {
            List<Entity.User> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.User LogIn(string userName, string password)
        {
            Entity.User retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_LogIn";
            cmd.Parameters.Add(new SqlParameter("@UserName", userName));
            cmd.Parameters.Add(new SqlParameter("@Password", password));

            retItem = Get(cmd);
            return retItem;
        }
        public static Entity.User LogIn(Guid accessKey)
        {
            Entity.User retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_LogIn_AccessKey";
            cmd.Parameters.Add(new SqlParameter("@AccessKey", accessKey));

            retItem = Get(cmd);
            return retItem;
        }
        public static Entity.User SearchUserName(string userName)
        {
            Entity.User retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Search_UserName";
            cmd.Parameters.Add(new SqlParameter("@UserName", userName));

            retItem = Get(cmd);
            return retItem;
        }
        public static Entity.User SearchEmail(string email)
        {
            Entity.User retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Search_Email";
            cmd.Parameters.Add(new SqlParameter("@Email", email));

            retItem = Get(cmd);
            return retItem;
        }
        public static Entity.User SelectUser(int userID)
        {
            Entity.User retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Select_UserID";
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));

            retItem = Get(cmd);
            return retItem;
        }

        public static DataTable SelectUserForGrid(int clientID, int? ClientGroupID, int pageSize, int pageIndex, bool IncludePlatformAdmins, bool UserIsCAdmin, bool IncludeAllClients, bool IncludeBCAdmins, bool IsKMstaff, string searchText, bool ShowDisabledUsers, bool ShowDisabledUserRoles)
        {
            List<Entity.User> retList = new List<Entity.User>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Select_UserGrid";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            if (ClientGroupID.HasValue)
                cmd.Parameters.AddWithValue("@ClientGroupID", ClientGroupID.Value);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
            cmd.Parameters.AddWithValue("@IncludePlatformAdmins", IncludePlatformAdmins);
            cmd.Parameters.AddWithValue("@UserIsCAdmin", UserIsCAdmin);
            cmd.Parameters.AddWithValue("@IncludeAllClients", IncludeAllClients);
            cmd.Parameters.AddWithValue("@IncludeBCAdmins", IncludeBCAdmins);
            cmd.Parameters.AddWithValue("@IsKMstaff", IsKMstaff);
            cmd.Parameters.AddWithValue("@searchText", searchText);
            cmd.Parameters.AddWithValue("@ShowDisabledUsers", ShowDisabledUsers);
            cmd.Parameters.AddWithValue("@ShowDisabledUserRoles", ShowDisabledUserRoles);            

            cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.KMPlatform.ToString());

            return KM.Common.DataFunctions.GetDataTable(cmd);
        }

        public static DataTable DownloadUserGrid(int clientID, int? ClientGroupID,  bool IncludePlatformAdmins, bool UserIsCAdmin, bool IncludeAllClients, bool IncludeBCAdmins, bool IsKMstaff, string searchText, bool ShowDisabledUsers, bool ShowDisabledUserRoles)
        {
            List<Entity.User> retList = new List<Entity.User>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Select_UserGrid_Download";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            if (ClientGroupID.HasValue)
                cmd.Parameters.AddWithValue("@ClientGroupID", ClientGroupID.Value);
            cmd.Parameters.AddWithValue("@IncludePlatformAdmins", IncludePlatformAdmins);
            cmd.Parameters.AddWithValue("@UserIsCAdmin", UserIsCAdmin);
            cmd.Parameters.AddWithValue("@IncludeAllClients", IncludeAllClients);
            cmd.Parameters.AddWithValue("@IncludeBCAdmins", IncludeBCAdmins);
            cmd.Parameters.AddWithValue("@IsKMstaff", IsKMstaff);
            cmd.Parameters.AddWithValue("@searchText", searchText);
            cmd.Parameters.AddWithValue("@ShowDisabledUsers", ShowDisabledUsers);
            cmd.Parameters.AddWithValue("@ShowDisabledUserRoles", ShowDisabledUserRoles);

            cmd.Connection = KM.Common.DataFunctions.GetSqlConnection(ConnectionString.KMPlatform.ToString());

            return KM.Common.DataFunctions.GetDataTable(cmd);
        }

        public static bool Validate_UserName(string userName, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_UserName_Exists";
            cmd.Parameters.AddWithValue("@UserName", userName);
            cmd.Parameters.AddWithValue("@UserID", userID);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()).ToString()) > 0 ? true : false;

        }

        public static void Delete(int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Delete";
            cmd.Parameters.AddWithValue("@UserID", userID);

            KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.KMPlatform.ToString());
        }
        private static Entity.User Get(SqlCommand cmd)
        {
            Entity.User retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.User();
                        var builder = DynamicBuilder<Entity.User>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (rdr["Status"].ToString().Equals(Enums.UserStatus.Active.ToString()))
                                retItem.Status = Enums.UserStatus.Active;
                            else if (rdr["Status"].ToString().Equals(Enums.UserStatus.Disabled.ToString()))
                                retItem.Status = Enums.UserStatus.Disabled;
                            else if (rdr["Status"].ToString().Equals(Enums.UserStatus.Invited.ToString()))
                                retItem.Status = Enums.UserStatus.Invited;
                            else if (rdr["Status"].ToString().Equals(Enums.UserStatus.Locked.ToString()))
                                retItem.Status = Enums.UserStatus.Locked;
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                if(cmd.Connection != null)
                    cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }

        private static List<Entity.User> GetList(SqlCommand cmd)
        {
            List<Entity.User> retList = new List<Entity.User>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.User retItem = new Entity.User();
                        var builder = DynamicBuilder<Entity.User>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                if (rdr["Status"].ToString().Equals(Enums.UserStatus.Active.ToString()))
                                    retItem.Status = Enums.UserStatus.Active;
                                else if (rdr["Status"].ToString().Equals(Enums.UserStatus.Disabled.ToString()))
                                    retItem.Status = Enums.UserStatus.Disabled;
                                else if (rdr["Status"].ToString().Equals(Enums.UserStatus.Invited.ToString()))
                                    retItem.Status = Enums.UserStatus.Invited;
                                else if (rdr["Status"].ToString().Equals(Enums.UserStatus.Locked.ToString()))
                                    retItem.Status = Enums.UserStatus.Locked;
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



        public static int Save(Entity.User x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_User_Save";
            cmd.Parameters.Add(new SqlParameter("@UserID", x.UserID));
            cmd.Parameters.Add(new SqlParameter("@DefaultClientGroupID", x.DefaultClientGroupID));
            cmd.Parameters.Add(new SqlParameter("@DefaultClientID", x.DefaultClientID));
            cmd.Parameters.Add(new SqlParameter("@FirstName", x.FirstName));
            cmd.Parameters.Add(new SqlParameter("@LastName", x.LastName));
            cmd.Parameters.Add(new SqlParameter("@UserName", x.UserName));
            cmd.Parameters.Add(new SqlParameter("@Password", x.Password));
            cmd.Parameters.Add(new SqlParameter("@Salt", x.Salt));
            cmd.Parameters.Add(new SqlParameter("@EmailAddress", x.EmailAddress));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@AccessKey", x.AccessKey.ToString()));
            cmd.Parameters.Add(new SqlParameter("@IsAccessKeyValid", x.IsAccessKeyValid));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsKMStaff", x.IsKMStaff));
            cmd.Parameters.Add(new SqlParameter("@IsPlatformAdministrator", x.IsPlatformAdministrator));
            cmd.Parameters.Add(new SqlParameter("@Phone", x.Phone));
            cmd.Parameters.Add(new SqlParameter("@Status", x.Status.ToString()));
            cmd.Parameters.Add(new SqlParameter("@IsReadOnly", x.IsReadOnly.ToString()));
            cmd.Parameters.Add(new SqlParameter("@RequirePasswordReset", x.RequirePasswordReset));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }

    }
}
