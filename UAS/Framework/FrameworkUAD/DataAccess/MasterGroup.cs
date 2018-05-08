using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class MasterGroup
    {
        public static bool ExistsByName(string displayName, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM mastergroups with (nolock) where Displayname = @DisplayName";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@DisplayName", displayName));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }

        public static bool ExistsByIDDisplayName(int masterGroupID, string displayName, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM mastergroups with (nolock) where mastergroupID <> @MasterGroupID and Displayname = @DisplayName";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@MasterGroupID", masterGroupID));
            cmd.Parameters.Add(new SqlParameter("@DisplayName", displayName));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }
        public static bool ExistsByIDName(int masterGroupID, string name, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM mastergroups with (nolock) where mastergroupID <> @MasterGroupID and Name = @Name";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@MasterGroupID", masterGroupID));
            cmd.Parameters.Add(new SqlParameter("@Name", name));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }

        public static NameValueCollection ValidationForDeleteorInActive(int MasterGroupID, KMPlatform.Object.ClientConnections client)
        {
            NameValueCollection nvReturn = new NameValueCollection();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "e_MasterGroup_Validate_DeleteorInActive";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@MasterGroupID", MasterGroupID));
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

        public static List<Entity.MasterGroup> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.MasterGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterGroup_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        public static List<Entity.MasterGroup> SelectByBrandID(int BrandID,  KMPlatform.Object.ClientConnections client)
        {
            List<Entity.MasterGroup> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterGroup_Select_ByBrandID";
            cmd.Parameters.Add(new SqlParameter("@BrandID", BrandID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }

        public static Entity.MasterGroup SelectByID(int MasterGroupID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterGroup_Select_ID";
            cmd.Parameters.AddWithValue("@MasterGroupID", MasterGroupID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Get(cmd);
        }

        public static DataSet SelectByMasterGroupsSearch(string Name, string SearchCriteria, int CurrentPage, int PageSize, string SortDirection, string SortColumn, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterGroup_Select_MasterGroupsBySearch";
            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            cmd.Parameters.Add(new SqlParameter("@SearchCriteria", SearchCriteria));
            cmd.Parameters.Add(new SqlParameter("@CurrentPage", CurrentPage));
            cmd.Parameters.Add(new SqlParameter("@PageSize", PageSize));
            cmd.Parameters.Add(new SqlParameter("@SortDirection", SortDirection));
            cmd.Parameters.Add(new SqlParameter("@SortColumn", SortColumn));
            return KM.Common.DataFunctions.GetDataSet(cmd, DataFunctions.GetClientSqlConnection(client).ConnectionString.ToString());
        }

        public static void UpdateSort(string mastergroupXml, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterGroup_Update_SortOrder";
            cmd.Parameters.Add(new SqlParameter("@MasterGroupXml", mastergroupXml));
            KM.Common.DataFunctions.GetDataSet(cmd, DataFunctions.GetClientSqlConnection(client).ConnectionString.ToString());
        }

        public static int Save(Entity.MasterGroup x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterGroup_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@MasterGroupID", x.MasterGroupID);
            cmd.Parameters.AddWithValue("@DisplayName", x.DisplayName);
            cmd.Parameters.AddWithValue("@Name", x.Name);
            cmd.Parameters.AddWithValue("@IsActive", x.IsActive);
            cmd.Parameters.AddWithValue("@EnableSubReporting", x.EnableSubReporting);
            cmd.Parameters.AddWithValue("@EnableSearching", x.EnableSearching);
            cmd.Parameters.AddWithValue("@EnableAdhocSearch", x.EnableAdhocSearch);
            cmd.Parameters.AddWithValue("@SortOrder", x.SortOrder);
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool Delete(KMPlatform.Object.ClientConnections client, int masterGroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterGroup_Delete";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@MasterGroupID", masterGroupID);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static Entity.MasterGroup Get(SqlCommand cmd)
        {
            Entity.MasterGroup retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.MasterGroup();
                        DynamicBuilder<Entity.MasterGroup> builder = DynamicBuilder<Entity.MasterGroup>.CreateBuilder(rdr);
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
        public static List<Entity.MasterGroup> GetList(SqlCommand cmd)
        {
            List<Entity.MasterGroup> retList = new List<Entity.MasterGroup>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.MasterGroup retItem = new Entity.MasterGroup();
                        DynamicBuilder<Entity.MasterGroup> builder = DynamicBuilder<Entity.MasterGroup>.CreateBuilder(rdr);
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
    }
}
