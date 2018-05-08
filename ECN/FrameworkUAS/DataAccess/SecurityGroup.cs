using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class SecurityGroup
    {
        public static List<Entity.SecurityGroup> Select()
        {
            List<Entity.SecurityGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroup_Select";

            retItem = GetList(cmd);
            return retItem;
        }

        public static Entity.SecurityGroup Select(int securityGroupID)
        {
            Entity.SecurityGroup retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroup_Select_SecurityGroupID";
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);
            retItem = Get(cmd);
            return retItem;
        }

        public static List<Entity.SecurityGroup> SelectForClientGroup(int clientGroupID)
        {
            List<Entity.SecurityGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroup_Select_ClientGroupID";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.SecurityGroup> SelectForClientGroup(int clientGroupID, KMPlatform.Enums.SecurityGroupAdministrativeLevel administrativeLevel)
        {
            List<Entity.SecurityGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroup_Select_ClientGroupID_AdministrativeLevel";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);
            cmd.Parameters.AddWithValue("@AdministrativeLevel", administrativeLevel.ToString());
            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.SecurityGroup> SelectActiveForClientGroup(int clientGroupID)
        {
            List<Entity.SecurityGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroup_SelectActive_ClientGroupID";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.SecurityGroup> SelectForClient(int clientID)
        {
            List<Entity.SecurityGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroup_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.SecurityGroup> SelectForClient(int clientID, KMPlatform.Enums.SecurityGroupAdministrativeLevel administrativeLevel)
        {
            List<Entity.SecurityGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroup_Select_ClientID_AdministrativeLevel";
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@AdministrativeLevel", administrativeLevel.ToString());

            retItem = GetList(cmd);
            return retItem;
        }

        public static Entity.SecurityGroup Select(int userID, int clientID)
        {
            Entity.SecurityGroup retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroup_Select_UserID_ClientID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@ClientID", clientID);

            retItem = Get(cmd);
            return retItem;
        }

        public static int CreateFromTemplateForClient(string securityGroupTemplateName, int clientGroupID, int clientID, string administrativeLevel, Entity.User user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroup_CreateFromTemplateForClient";
            cmd.Parameters.Add(new SqlParameter("@SecurityGroupTemplateName", securityGroupTemplateName));
            cmd.Parameters.Add(new SqlParameter("@ClientID", clientID));
            cmd.Parameters.Add(new SqlParameter("@AdministrativeLevel", administrativeLevel));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", user.UserID));
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }

        public static void UpdateAdministrators(int ClientID, int ClientGroupID, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroupPermission_UpdateAdministrators";
            cmd.Parameters.Add(new SqlParameter("@ClientID", ClientID));
            cmd.Parameters.Add(new SqlParameter("@ClientGroupID", ClientGroupID));
            cmd.Parameters.Add(new SqlParameter("@UserID", UserID));

            KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.KMPlatform.ToString());
        }

        public static int CreateFromTemplateForClientGroup(string securityGroupTemplateName, int clientGroupID, string administrativeLevel, Entity.User user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroup_CreateFromTemplateForClientGroup";
            cmd.Parameters.Add(new SqlParameter("@SecurityGroupTemplateName", securityGroupTemplateName));
            cmd.Parameters.Add(new SqlParameter("@ClientGroupID", clientGroupID));
            cmd.Parameters.Add(new SqlParameter("@AdministrativeLevel", administrativeLevel));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", user.UserID));
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }

        private static Entity.SecurityGroup Get(SqlCommand cmd)
        {
            Entity.SecurityGroup retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.SecurityGroup();
                        var builder = DynamicBuilder<Entity.SecurityGroup>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);

                            if (rdr["AdministrativeLevel"] == DBNull.Value)
                                retItem.AdministrativeLevel = KMPlatform.Enums.SecurityGroupAdministrativeLevel.None;
                            else if (rdr["AdministrativeLevel"].ToString().Equals(KMPlatform.Enums.SecurityGroupAdministrativeLevel.ChannelAdministrator.ToString()))
                                retItem.AdministrativeLevel = KMPlatform.Enums.SecurityGroupAdministrativeLevel.ChannelAdministrator;
                            else if (rdr["AdministrativeLevel"].ToString().Equals(KMPlatform.Enums.SecurityGroupAdministrativeLevel.Administrator.ToString()))
                                retItem.AdministrativeLevel = KMPlatform.Enums.SecurityGroupAdministrativeLevel.Administrator;

                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }
        private static List<Entity.SecurityGroup> GetList(SqlCommand cmd)
        {
            List<Entity.SecurityGroup> retList = new List<Entity.SecurityGroup>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.SecurityGroup retItem = new Entity.SecurityGroup();
                        var builder = DynamicBuilder<Entity.SecurityGroup>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                if (rdr["AdministrativeLevel"] == DBNull.Value)
                                    retItem.AdministrativeLevel = KMPlatform.Enums.SecurityGroupAdministrativeLevel.None;
                                else if (rdr["AdministrativeLevel"].ToString().Equals(KMPlatform.Enums.SecurityGroupAdministrativeLevel.ChannelAdministrator.ToString()))
                                    retItem.AdministrativeLevel = KMPlatform.Enums.SecurityGroupAdministrativeLevel.ChannelAdministrator;
                                else if (rdr["AdministrativeLevel"].ToString().Equals(KMPlatform.Enums.SecurityGroupAdministrativeLevel.Administrator.ToString()))
                                    retItem.AdministrativeLevel = KMPlatform.Enums.SecurityGroupAdministrativeLevel.Administrator;

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

        public static int Save(Entity.SecurityGroup x)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroup_Save";

            cmd.Parameters.Add(new SqlParameter("@SecurityGroupID", x.SecurityGroupID));
            cmd.Parameters.Add(new SqlParameter("@SecurityGroupName", x.SecurityGroupName));
            if (x.ClientID > 0)
                cmd.Parameters.Add(new SqlParameter("@ClientID", x.ClientID));
            else
                cmd.Parameters.Add(new SqlParameter("@ClientID", DBNull.Value));
            if (x.ClientGroupID > 0)
                cmd.Parameters.Add(new SqlParameter("@ClientGroupID", x.ClientGroupID));
            else
                cmd.Parameters.Add(new SqlParameter("@ClientGroupID", DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@AdministrativeLevel", x.AdministrativeLevel.ToString()));
            cmd.Parameters.Add(new SqlParameter("@Description", x.Description));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }

        public static bool ExistsByClient_ClientGroup(string name, int clientgroupID, int clientID, int securityGroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SecurityGroup_Exists_ClientGroup_Client";
            cmd.Parameters.AddWithValue("@SecurityGroupName", name);
            cmd.Parameters.AddWithValue("@ClientGroupID", clientgroupID);
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()).ToString()) > 0 ? true : false;
        }
    }
}
