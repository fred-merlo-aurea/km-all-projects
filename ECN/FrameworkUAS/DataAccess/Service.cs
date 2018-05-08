using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class Service
    {
        private static string _CacheRegion = "Service";

        #region AMS
        public static List<Entity.Service> AMS_SelectForSecurityGroupAndUserID(int securityGroupID, int userID)
        {
            List<Entity.Service> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "e_Service_AMS_Select_SecurityGroup_UserID";
            cmd.CommandText = "e_Service_Select_SecurityGroup_UserID";
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Service> AMS_SelectForSecurityGroup(int securityGroupID)
        {
            List<Entity.Service> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "e_Service_AMS_Select_SecurityGroup";
            cmd.CommandText = "e_Service_Select_SecurityGroup";
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Service> AMS_SelectForSecurityGroupAndClientGroupID(int securityGroupID, int clientGroupID)
        {
            List<Entity.Service> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "e_Service_AMS_Select_SecurityGroup_ClientGroupID";
            cmd.CommandText = "e_Service_Select_SecurityGroup_ClientGroupID";
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);

            retItem = GetList(cmd);
            return retItem;
        }
        #endregion
        public static List<Entity.Service> Select()
        {
            List<Entity.Service> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Service_Select";

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Service> SelectForUser(int userID)
        {
            List<Entity.Service> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Service_Select_UserID";
            cmd.Parameters.AddWithValue("@UserID", userID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Service> SelectForUserAuthorization(int userID)
        {
            List<Entity.Service> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Service_SelectForUserAuthorization_UserID";
            cmd.Parameters.AddWithValue("@UserID", userID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Service> SelectForSecurityGroupAndUserID(int securityGroupID, int userID)
        {
            List<Entity.Service> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Service_Select_SecurityGroup_UserID";
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Service> SelectForSecurityGroupAndClientGroupID(int securityGroupID, int clientGroupID)
        {
            List<Entity.Service> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Service_Select_SecurityGroup_ClientGroupID";
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.Service> SelectForSecurityGroup(int securityGroupID)
        {
            List<Entity.Service> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Service_Select_SecurityGroup";
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);

            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.Service Select(int serviceID)
        {
            Entity.Service retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Service_Select_ServiceID";
            cmd.Parameters.AddWithValue("@ServiceID", serviceID);
            retItem = Get(cmd);
            return retItem;
        }

        public static Entity.Service Select(KMPlatform.Enums.Services service)
        {
            string serviceCode = service.ToString().Replace("_", " ");
            Entity.Service retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Service_Select_ServiceCode";
            cmd.Parameters.AddWithValue("@ServiceCode", serviceCode);
            retItem = Get(cmd);
            return retItem;
        }

        public static List<Entity.Service> SelectForSecurityGroupID(int securityGroupID)
        {
            List<Entity.Service> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Service_Select_SecurityGroup";
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.Service> SelectForClientGroupID( int clientGroupID)
        {
            List<Entity.Service> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Service_Select_ClientGroupID";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);

            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.Service> SelectForClientID(int clientID, bool includeObjects = false)
        {
            List<Entity.Service> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Service_Select_ClientID";
            cmd.Parameters.AddWithValue("@ClientID", clientID);


            if (KM.Common.CacheUtil.IsCacheEnabled())
            {
                retItem = (List<Entity.Service>)KM.Common.CacheUtil.GetFromCache(string.Format("{0}-{1}", clientID, includeObjects.ToString()), _CacheRegion);

                if (retItem == null)
                {
                    retItem = GetList(cmd);

                    if (includeObjects)
                    {
                        foreach (Entity.Service s in retItem)
                        {
                            ServiceFeature sfWorker = new ServiceFeature();

                            s.ServiceFeatures = ServiceFeature.SelectOnlyEnabledClientID(s.ServiceID, clientID);
                        }
                    }
                    KM.Common.CacheUtil.AddToCache(string.Format("{0}-{1}", clientID, includeObjects.ToString()), retItem, _CacheRegion);
                }
            }
            else
            {
                retItem = GetList(cmd);

                if (includeObjects)
                {
                    foreach (Entity.Service s in retItem)
                    {
                        ServiceFeature sfWorker = new ServiceFeature();

                        s.ServiceFeatures = ServiceFeature.SelectOnlyEnabledClientID(s.ServiceID, clientID);
                    }
                }
            }

            return retItem;
        }

        private static Entity.Service Get(SqlCommand cmd)
        {
            Entity.Service retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Service();
                        var builder = DynamicBuilder<Entity.Service>.CreateBuilder(rdr);
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
        private static List<Entity.Service> GetList(SqlCommand cmd)
        {
            List<Entity.Service> retList = new List<Entity.Service>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        Entity.Service retItem = new Entity.Service();
                        var builder = DynamicBuilder<Entity.Service>.CreateBuilder(rdr);
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
        public static int Save(Entity.Service x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Service_Save";
            cmd.Parameters.Add(new SqlParameter("@ServiceID", x.ServiceID));
            cmd.Parameters.Add(new SqlParameter("@ServiceName", x.ServiceName));
            cmd.Parameters.Add(new SqlParameter("@Description", x.Description));
            cmd.Parameters.Add(new SqlParameter("@ServiceCode", x.ServiceCode));
            cmd.Parameters.Add(new SqlParameter("@DisplayOrder", x.DisplayOrder));
            cmd.Parameters.Add(new SqlParameter("@IsEnabled", x.IsEnabled));
            cmd.Parameters.Add(new SqlParameter("@IsAdditionalCost", x.IsAdditionalCost));
            cmd.Parameters.Add(new SqlParameter("@HasFeatures", x.HasFeatures));
            cmd.Parameters.Add(new SqlParameter("@DefaultRate", x.DefaultRate));
            cmd.Parameters.Add(new SqlParameter("@DefaultDurationInMonths", x.DefaultDurationInMonths));
            cmd.Parameters.Add(new SqlParameter("@DefaultApplicationID", x.DefaultApplicationID));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }
    }
}
