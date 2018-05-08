using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class MasterCodeSheet
    {
        public static bool ExistsByIDMasterValueMasterGroupID(int masterID, int masterGroupID, string masterValue, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterCodeSheet_Exists_ByIDMasterValueMasterGroupID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@MasterID", masterID);
            cmd.Parameters.AddWithValue("@MasterValue", masterValue);
            cmd.Parameters.AddWithValue("@MasterGroupID", masterGroupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }

        public static List<Entity.MasterCodeSheet> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.MasterCodeSheet> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterCodeSheet_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.MasterCodeSheet SelectByID(int masterID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterCodeSheet_Select_ID";
            cmd.Parameters.AddWithValue("@MasterID", masterID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Get(cmd);
        }
        public static List<Entity.MasterCodeSheet> SelectMasterGroupID(KMPlatform.Object.ClientConnections client, int masterGroupID)
        {
            List<Entity.MasterCodeSheet> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterCodeSheet_Select_MasterGroupID";
            cmd.Parameters.AddWithValue("@MasterGroupID", masterGroupID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.MasterCodeSheet> SelectMasterGroupBrandID(KMPlatform.Object.ClientConnections client, int brandID)
        {
            List<Entity.MasterCodeSheet> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterCodeSheet_Select_ByBrandID";
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static DataSet SelectByMasterCodeSheetSearch(int masterGroupID, string Name, string SearchCriteria, int CurrentPage, int PageSize, string SortDirection, string SortColumn, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterCodeSheet_Select_Search";
            cmd.Parameters.Add(new SqlParameter("@MasterGroupID", masterGroupID));
            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            cmd.Parameters.Add(new SqlParameter("@SearchCriteria", SearchCriteria));
            cmd.Parameters.Add(new SqlParameter("@CurrentPage", CurrentPage));
            cmd.Parameters.Add(new SqlParameter("@PageSize", PageSize));
            cmd.Parameters.Add(new SqlParameter("@SortDirection", SortDirection));
            cmd.Parameters.Add(new SqlParameter("@SortColumn", SortColumn));
            return KM.Common.DataFunctions.GetDataSet(cmd, DataFunctions.GetClientSqlConnection(client).ConnectionString.ToString());
        }

        public static DataSet SelectByCodeSheetID(int codeSheetID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterCodeSheet_Select_CodeSheetID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@CodeSheetID", codeSheetID);
            return KM.Common.DataFunctions.GetDataSet(cmd, DataFunctions.GetClientSqlConnection(client).ConnectionString.ToString());
        }

        public static void UpdateSort(string mastercodesheetXml, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterCodeSheet_Update_SortOrder";
            cmd.Parameters.Add(new SqlParameter("@MasterCodeSheetXml", mastercodesheetXml));
            KM.Common.DataFunctions.GetDataSet(cmd, DataFunctions.GetClientSqlConnection(client).ConnectionString.ToString());
        }

        public static int Save(Entity.MasterCodeSheet x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterCodeSheet_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@MasterID", x.MasterID);
            cmd.Parameters.AddWithValue("@MasterGroupID", x.MasterGroupID);
            cmd.Parameters.AddWithValue("@MasterValue", x.MasterValue);
            cmd.Parameters.AddWithValue("@MasterDesc", x.MasterDesc);
            cmd.Parameters.AddWithValue("@MasterDesc1", (object)x.MasterDesc1 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EnableSearching", x.EnableSearching);
            cmd.Parameters.AddWithValue("@SortOrder", x.SortOrder);
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static int ImportSubscriber(int masterID, XDocument xDoc, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterCodeSheet_Import_Subscriber";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@MASTERGROUPID", masterID);
            cmd.Parameters.AddWithValue("@IMPORTXML", xDoc.ToString(SaveOptions.DisableFormatting));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool DeleteMasterID(KMPlatform.Object.ClientConnections client, int masterID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MasterCodeSheet_Delete_MasterID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@MasterID", masterID);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static void CreateNoValueRespones(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_MasterCodeSheet_AddNoValue";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static Entity.MasterCodeSheet Get(SqlCommand cmd)
        {
            Entity.MasterCodeSheet retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.MasterCodeSheet();
                        DynamicBuilder<Entity.MasterCodeSheet> builder = DynamicBuilder<Entity.MasterCodeSheet>.CreateBuilder(rdr);
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
        public static List<Entity.MasterCodeSheet> GetList(SqlCommand cmd)
        {
            List<Entity.MasterCodeSheet> retList = new List<Entity.MasterCodeSheet>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.MasterCodeSheet retItem = new Entity.MasterCodeSheet();
                        DynamicBuilder<Entity.MasterCodeSheet> builder = DynamicBuilder<Entity.MasterCodeSheet>.CreateBuilder(rdr);
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
