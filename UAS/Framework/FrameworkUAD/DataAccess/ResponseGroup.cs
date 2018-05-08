using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class ResponseGroup
    {
        public static bool ExistsByName(string displayName, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ResponseGroup_Exists_ByDisplayName";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@DisplayName", displayName));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }
        public static bool ExistsByIDDisplayNamePubID(int pubID, int responseGroupID, string displayName, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ResponseGroup_Exists_ByIDDisplayNamePubID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@PubID", pubID));
            cmd.Parameters.Add(new SqlParameter("@ResponseGroupID", responseGroupID));
            cmd.Parameters.Add(new SqlParameter("@DisplayName", displayName));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }
        public static bool ExistsByIDResponseGroupNamePubID(int pubID, int responseGroupID, string responseGroupName, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ResponseGroup_Exists_ByIDResponseGroupNamePubID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@PubID", pubID));
            cmd.Parameters.Add(new SqlParameter("@ResponseGroupID", responseGroupID));
            cmd.Parameters.Add(new SqlParameter("@ResponseGroupName", responseGroupName));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }
        public static NameValueCollection ValidationForDeleteorInActive(int pubID, int responseGroupID, KMPlatform.Object.ClientConnections client)
        {
            NameValueCollection nvReturn = new NameValueCollection();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "e_ResponseGroup_Validate_DeleteorInActive";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@ResponseGroupID", responseGroupID));
            cmd.Parameters.Add(new SqlParameter("@PubID", pubID));
            cmd.CommandTimeout = 0;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        while (rdr.Read())
                        {
                            //nvReturn.Add(rdr["Reference"].ToString() + " : ", rdr["Reference"].ToString().ToUpper() == "FILTER EXPORT SCHEDULE" ? "<a href='../main/FilterExport.aspx?FilterScheduleId=" + rdr["ReferenceID2"].ToString() + "&FilterID=" + rdr["ReferenceID1"].ToString() + "'>" + rdr["ReferenceName"].ToString() + "</a>" : rdr["ReferenceName"].ToString());

                            if (nvReturn[rdr["Reference"].ToString() + " : "] != null)
                                nvReturn.Set(rdr["Reference"].ToString() + " : ", nvReturn.Get(rdr["Reference"].ToString() + " : ") + ", " + rdr["ReferenceName"].ToString());
                            else
                                nvReturn.Add(rdr["Reference"].ToString() + " : ", rdr["ReferenceName"].ToString());
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
            return nvReturn;
        }
        public static List<Entity.ResponseGroup> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ResponseGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ResponseGroup_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ResponseGroup> Select(int pubID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ResponseGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ResponseGroup_Select_PubID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@PubID", pubID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.ResponseGroup> Select(string pubCode, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ResponseGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ResponseGroup_Select_PubCode";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@PubCode", pubCode);
            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.ResponseGroup SelectByID(int responseGroupID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ResponseGroup_Select_ID";
            cmd.Parameters.AddWithValue("@ResponseGroupID", responseGroupID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Get(cmd);
        }
        public static DataSet SelectByResponseGroupsSearch(int PubID, string Name, string SearchCriteria, int CurrentPage, int PageSize, string SortDirection, string SortColumn, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ResponseGroup_Select_ResponseGroupsBySearch";
            cmd.Parameters.Add(new SqlParameter("@PubID", PubID));
            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            cmd.Parameters.Add(new SqlParameter("@SearchCriteria", SearchCriteria));
            cmd.Parameters.Add(new SqlParameter("@CurrentPage", CurrentPage));
            cmd.Parameters.Add(new SqlParameter("@PageSize", PageSize));
            cmd.Parameters.Add(new SqlParameter("@SortDirection", SortDirection));
            cmd.Parameters.Add(new SqlParameter("@SortColumn", SortColumn));
            return KM.Common.DataFunctions.GetDataSet(cmd, DataFunctions.GetClientSqlConnection(client).ConnectionString.ToString());
        }
        public static bool Copy(KMPlatform.Object.ClientConnections client, int responseGroupID, string destPubsXML)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ResponseGroup_Copy";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@srcResponseGroupID", responseGroupID));
            cmd.Parameters.Add(new SqlParameter("@destPubsXML", destPubsXML));
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool Delete(KMPlatform.Object.ClientConnections client, int responseGroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ResponseGroup_Delete";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@ResponseGroupID", responseGroupID));
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static Entity.ResponseGroup Get(SqlCommand cmd)
        {
            Entity.ResponseGroup retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.ResponseGroup();
                        DynamicBuilder<Entity.ResponseGroup> builder = DynamicBuilder<Entity.ResponseGroup>.CreateBuilder(rdr);
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
        public static List<Entity.ResponseGroup> GetList(SqlCommand cmd)
        {
            List<Entity.ResponseGroup> retList = new List<Entity.ResponseGroup>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.ResponseGroup retItem = new Entity.ResponseGroup();
                        DynamicBuilder<Entity.ResponseGroup> builder = DynamicBuilder<Entity.ResponseGroup>.CreateBuilder(rdr);
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

        public static int Save(Entity.ResponseGroup x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_ResponseGroup_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@ResponseGroupID", x.ResponseGroupID);
            cmd.Parameters.AddWithValue("@PubID", (object)x.PubID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ResponseGroupName", (object)x.ResponseGroupName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DisplayName", (object)x.DisplayName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DateCreated", x.DateCreated);
            cmd.Parameters.AddWithValue("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedByUserID", x.CreatedByUserID);
            cmd.Parameters.AddWithValue("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DisplayOrder", (object)x.DisplayOrder ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsMultipleValue", (object)x.IsMultipleValue ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsRequired", (object)x.IsRequired ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive", (object)x.IsActive ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@WQT_ResponseGroupID", (object)x.WQT_ResponseGroupID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ResponseGroupTypeId", x.ResponseGroupTypeId);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }
    }
}