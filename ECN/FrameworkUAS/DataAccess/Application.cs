using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class Application
    {
        private const string ParameterUserID = "@UserID";
        private const string ParameterServiceID = "@ServiceID";
        private const string ParameterSecurityGroupID = "@SecurityGroupID";

        public static List<Entity.Application> Select()
        {
            var sqlCommand = CreateStoredProcedureSqlCommand("e_Application_Select");
            return GetList(sqlCommand);
        }

        public static List<Entity.Application> SelectForUser(int userID)
        {
            var sqlCommand = CreateStoredProcedureSqlCommand("e_Application_Select_UserID");
            sqlCommand.Parameters.AddWithValue(ParameterUserID, userID);

            return GetList(sqlCommand);
        }

        public static List<Entity.Application> SelectForSecurityGroup(int securityGroupID)
        {
            var sqlCommand = CreateStoredProcedureSqlCommand("e_Application_Select_SecurityGroupID");
            sqlCommand.Parameters.AddWithValue(ParameterSecurityGroupID, securityGroupID);

            return GetList(sqlCommand);
        }

        public static List<Entity.Application> SelectForService(int serviceID)
        {
            var sqlCommand = CreateStoredProcedureSqlCommand("e_Application_Select_ServiceID");
            sqlCommand.Parameters.AddWithValue(ParameterServiceID, serviceID);

            return GetList(sqlCommand);
        }

        public static List<Entity.Application> SelectOnlyEnabledForService(int serviceID)
        {
            var sqlCommand = CreateStoredProcedureSqlCommand("e_Application_SelectOnlyEnabled_ServiceID");
            sqlCommand.Parameters.AddWithValue(ParameterServiceID, serviceID);

            return GetList(sqlCommand);
        }
        public static List<Entity.Application> SelectOnlyEnabledForService(int serviceID, int userID)
        {
            var sqlCommand = CreateStoredProcedureSqlCommand("e_Application_SelectOnlyEnabled_ServiceID_UserID");
            sqlCommand.Parameters.AddWithValue(ParameterServiceID, serviceID);
            sqlCommand.Parameters.AddWithValue(ParameterUserID, userID);

            return GetList(sqlCommand);
        }

        private static SqlCommand CreateStoredProcedureSqlCommand(string storedProcedureName)
        {
            return new SqlCommand()
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = storedProcedureName,
            };
        }

        private static Entity.Application Get(SqlCommand cmd)
        {
            Entity.Application retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Application();
                        var builder = DynamicBuilder<Entity.Application>.CreateBuilder(rdr);
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
        private static List<Entity.Application> GetList(SqlCommand cmd)
        {
            List<Entity.Application> retList = new List<Entity.Application>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Application retItem = new Entity.Application();
                        var builder = DynamicBuilder<Entity.Application>.CreateBuilder(rdr);
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

        public static int Save(Entity.Application x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Application_Save";
            cmd.Parameters.AddWithValue("@ApplicationID", x.ApplicationID);
            cmd.Parameters.AddWithValue("@ApplicationName", x.ApplicationName);
            cmd.Parameters.AddWithValue("@Description", x.Description);
            cmd.Parameters.AddWithValue("@ApplicationCode", x.ApplicationCode);
            cmd.Parameters.AddWithValue("@DefaultView", x.DefaultView);
            cmd.Parameters.AddWithValue("@IsActive", x.IsActive);
            cmd.Parameters.AddWithValue("@IconFullName", x.IconFullName);
            cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);
            cmd.Parameters.AddWithValue("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedByUserID", x.CreatedByUserID);
            cmd.Parameters.AddWithValue("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FromEmailAddress", x.FromEmailAddress);
            cmd.Parameters.AddWithValue("@ErrorEmailAddress", x.ErrorEmailAddress);

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }
    }
}
