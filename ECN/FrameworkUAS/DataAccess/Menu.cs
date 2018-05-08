using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class Menu
    {
        public static List<Entity.Menu> Select()
        {
            List<Entity.Menu> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Menu_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Menu> Select(int securityGroupID, bool hasAccess)
        {
            List<Entity.Menu> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Menu_Select_SecurityGroupID";
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);
            cmd.Parameters.AddWithValue("@HasAccess", hasAccess);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Menu> SelectForApplication(int applicationID, bool isActive)
        {
            List<Entity.Menu> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Menu_Select_ApplicationID";
            cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
            cmd.Parameters.AddWithValue("@IsActive", isActive);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Menu> SelectForApplication(int applicationID, int securityGroupID)
        {
            List<Entity.Menu> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Menu_Select_ApplicationID_SecurityGroupID";
            cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Menu> SelectForApplication(int applicationID, int userID, bool isActive)
        {
            List<Entity.Menu> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Menu_Select_ApplicationID_UserID";
            cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Menu> SelectForUser(int userID, int securityGroupID, bool isActive, bool hasAccess)
        {
            List<Entity.Menu> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Menu_Select_UserID_SecurityGroupID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);
            cmd.Parameters.AddWithValue("@IsActive", isActive);
            cmd.Parameters.AddWithValue("@HasAccess", hasAccess);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Menu> SelectForApplicationAndUser(string applicationName, int userID, int clientId, int securityGroupID, bool isActive, bool hasAccess)
        {
            List<Entity.Menu> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Menu_Select_ApplicationName_UserID_SecurityGroupID";
            cmd.Parameters.AddWithValue("@ApplicationName", applicationName);
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);
            cmd.Parameters.AddWithValue("@IsActive", isActive);
            cmd.Parameters.AddWithValue("@HasAccess", hasAccess);
            cmd.Parameters.AddWithValue("@clientID", clientId);
            retItem = GetList(cmd);
            return retItem;
        }

        private static Entity.Menu Get(SqlCommand cmd)
        {
            Entity.Menu retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Menu();
                        var builder = DynamicBuilder<Entity.Menu>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            //retItem.MenuFeatures = new List<Entity.MenuFeature>();
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
        private static List<Entity.Menu> GetList(SqlCommand cmd)
        {
            List<Entity.Menu> retList = new List<Entity.Menu>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Menu retItem = new Entity.Menu();
                        var builder = DynamicBuilder<Entity.Menu>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                //retItem.MenuFeatures = new List<Entity.MenuFeature>();
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

        public static int Save(Entity.Menu x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Menu_Save";
            cmd.Parameters.Add(new SqlParameter("@MenuID", x.MenuID));
            cmd.Parameters.Add(new SqlParameter("@ApplicationID", x.ApplicationID));
            cmd.Parameters.Add(new SqlParameter("@MenuName", x.MenuName));
            cmd.Parameters.Add(new SqlParameter("@Description", x.Description));
            cmd.Parameters.Add(new SqlParameter("@IsParent", x.IsParent));
            cmd.Parameters.Add(new SqlParameter("@ParentMenuID", x.ParentMenuID));
            cmd.Parameters.Add(new SqlParameter("@URL", x.URL));
            cmd.Parameters.Add(new SqlParameter("@IsActive", x.IsActive));
            cmd.Parameters.Add(new SqlParameter("@MenuOrder", x.MenuOrder));
            cmd.Parameters.Add(new SqlParameter("@HasFeatures", x.HasFeatures));
            cmd.Parameters.Add(new SqlParameter("@ImagePath", x.ImagePath));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }
    }
}
