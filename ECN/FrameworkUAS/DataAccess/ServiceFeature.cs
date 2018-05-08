using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KM.Common.Data;

namespace KMPlatform.DataAccess
{
    public class ServiceFeature
    {
        public static List<Entity.ServiceFeature> Select()
        {
            List<Entity.ServiceFeature> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ServiceFeature_Select";
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ServiceFeature> Select(int serviceID)
        {
            List<Entity.ServiceFeature> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ServiceFeature_Select_ServiceID";
            cmd.Parameters.AddWithValue("@ServiceID", serviceID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ServiceFeature> SelectOnlyEnabled(int serviceID)
        {
            List<Entity.ServiceFeature> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ServiceFeature_SelectOnlyEnabled_ServiceID";
            cmd.Parameters.AddWithValue("@ServiceID", serviceID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ServiceFeature> SelectOnlyEnabledClientGroupID(int serviceID, int clientGroupID)
        {
            List<Entity.ServiceFeature> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ServiceFeature_ClientGroup_Service";
            cmd.Parameters.AddWithValue("@ServiceID", serviceID);
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ServiceFeature> SelectOnlyEnabledClientID(int serviceID, int clientID)
        {
            List<Entity.ServiceFeature> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ServiceFeature_Client_Service";
            cmd.Parameters.AddWithValue("@ServiceID", serviceID);
            cmd.Parameters.AddWithValue("@clientID", clientID);
            retItem = GetList(cmd);
            return retItem;
        }

        
        public static Entity.ServiceFeature SelectServiceFeature(int serviceFeatureID)
        {
            Entity.ServiceFeature retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ServiceFeature_Select_ServiceFeatureID";
            cmd.Parameters.AddWithValue("@ServiceFeatureID", serviceFeatureID);
            retItem = Get(cmd);
            return retItem;
        }
        
        public static List<Entity.ServiceFeature.SecurityGroupTreeListRow> GetEmptySecurityGroupTreeList(int clientGroupID, int clientID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_ServiceFeature_GetEmptySecurityGroupTreeList";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            return GetList<Entity.ServiceFeature.SecurityGroupTreeListRow>(cmd);
        }

        public static List<Entity.ServiceFeature.SecurityGroupTreeListRow> GetSecurityGroupTreeList(int securityGroupID, int clientGroupID, int clientID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_ServiceFeature_GetSecurityGroupTreeList";
            cmd.Parameters.AddWithValue("@SecurityGroupID", securityGroupID);
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            return GetList<Entity.ServiceFeature.SecurityGroupTreeListRow>(cmd);
        }
        
        public static List<Entity.ServiceFeature.ClientGroupTreeListRow> GetClientGroupTreeList(int clientGroupID, bool isAdditionalCost)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_ServiceFeature_GetClientGroupTreeList";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);
            cmd.Parameters.AddWithValue("@IsAdditionalCost", isAdditionalCost);
            return GetList<Entity.ServiceFeature.ClientGroupTreeListRow>(cmd);
        }

        public static List<Entity.ServiceFeature.ClientGroupTreeListRow> GetClientTreeList(int clientGroupID, int clientID, bool isAdditionalCost)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_ServiceFeature_GetClientTreeList";
            cmd.Parameters.AddWithValue("@ClientGroupID", clientGroupID);
            cmd.Parameters.AddWithValue("@ClientID", clientID);
            cmd.Parameters.AddWithValue("@IsAdditionalCost", isAdditionalCost);
            return GetList<Entity.ServiceFeature.ClientGroupTreeListRow>(cmd);
        }

        public static bool HasAccess(int userID, int clientID, KMPlatform.Enums.Services serviceCode, KMPlatform.Enums.ServiceFeatures sfCode, KMPlatform.Enums.Access accessCode)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ServiceFeature_HasAccess";
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            cmd.Parameters.Add(new SqlParameter("@ClientID", clientID));
            cmd.Parameters.Add(new SqlParameter("@ServiceCode", serviceCode.ToString()));
            cmd.Parameters.Add(new SqlParameter("@SFCode", sfCode.ToString()));
            cmd.Parameters.Add(new SqlParameter("@AccessCode", accessCode.ToString()));
            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString())) > 0 ? true : false;
        }

        private static Entity.ServiceFeature Get(SqlCommand cmd)
        {
            Entity.ServiceFeature retItem = null;
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ServiceFeature();
                        var builder = DynamicBuilder<Entity.ServiceFeature>.CreateBuilder(rdr);
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

        private static List<T> GetList<T>(SqlCommand cmd)
        {
            List<T> retList = new List<T>();
            try
            {
                using (SqlDataReader rdr = KM.Common.DataFunctions.ExecuteReader(cmd, ConnectionString.KMPlatform.ToString()))
                {
                    if (rdr != null)
                    {
                        T retItem = default(T);
                        var builder = DynamicBuilder<T>.CreateBuilder(rdr);
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
        private static List<Entity.ServiceFeature> GetList(SqlCommand cmd)
        {
            return GetList<Entity.ServiceFeature>(cmd);
        }
        
        public static bool Save(Entity.ServiceFeature x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ServiceFeature_Save";
            cmd.Parameters.Add(new SqlParameter("@ServiceFeatureID", x.ServiceFeatureID));
            cmd.Parameters.Add(new SqlParameter("@ServiceID", x.ServiceID));
            cmd.Parameters.Add(new SqlParameter("@SFName", x.SFName));
            cmd.Parameters.Add(new SqlParameter("@Description", x.Description));
            cmd.Parameters.Add(new SqlParameter("@SFCode", x.SFCode));
            cmd.Parameters.Add(new SqlParameter("@DisplayOrder", x.DisplayOrder));
            cmd.Parameters.Add(new SqlParameter("@IsEnabled", x.IsEnabled));
            cmd.Parameters.Add(new SqlParameter("@IsAdditionalCost", x.IsAdditionalCost));
            cmd.Parameters.Add(new SqlParameter("@DefaultRate", x.DefaultRate));
            cmd.Parameters.Add(new SqlParameter("@DefaultDurationInMonths", x.DefaultDurationInMonths));
            cmd.Parameters.Add(new SqlParameter("@KMAdminOnly", x.KMAdminOnly));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return KM.Common.DataFunctions.ExecuteNonQuery(cmd, ConnectionString.KMPlatform.ToString());
        }
        public static int SaveReturnId(Entity.ServiceFeature x)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ServiceFeature_Save";
            cmd.Parameters.Add(new SqlParameter("@ServiceFeatureID", x.ServiceFeatureID));
            cmd.Parameters.Add(new SqlParameter("@ServiceID", x.ServiceID));
            cmd.Parameters.Add(new SqlParameter("@SFName", x.SFName));
            cmd.Parameters.Add(new SqlParameter("@Description", x.Description));
            cmd.Parameters.Add(new SqlParameter("@SFCode", x.SFCode));
            cmd.Parameters.Add(new SqlParameter("@DisplayOrder", x.DisplayOrder));
            cmd.Parameters.Add(new SqlParameter("@IsEnabled", x.IsEnabled));
            cmd.Parameters.Add(new SqlParameter("@IsAdditionalCost", x.IsAdditionalCost));
            cmd.Parameters.Add(new SqlParameter("@DefaultRate", x.DefaultRate));
            cmd.Parameters.Add(new SqlParameter("@DefaultDurationInMonths", x.DefaultDurationInMonths));
            cmd.Parameters.Add(new SqlParameter("@KMAdminOnly", x.KMAdminOnly));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(KM.Common.DataFunctions.ExecuteScalar(cmd, ConnectionString.KMPlatform.ToString()));
        }
    }
}
