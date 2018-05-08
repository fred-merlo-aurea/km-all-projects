using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FrameworkUAD.DataAccess
{
    public class CodeSheet
    {
        public static bool ExistsByResponseGroupIDValue(int codesheetID, int responseGroupID, string responseValue, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Codesheet_Exists_ByResponseGroupIDValue";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@ResponseGroupID", responseGroupID);
            cmd.Parameters.AddWithValue("@ResponseValue", responseValue);
            cmd.Parameters.AddWithValue("@CodeSheetID", codesheetID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }

        public static List<Entity.CodeSheet> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.CodeSheet> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CodeSheet_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static List<Entity.CodeSheet> Select(int pubID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.CodeSheet> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CodeSheet_Select_PubID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@PubID", pubID);
            retItem = GetList(cmd);
            return retItem;
        }
        public static Entity.CodeSheet Get(SqlCommand cmd)
        {
            Entity.CodeSheet retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.CodeSheet();
                        DynamicBuilder<Entity.CodeSheet> builder = DynamicBuilder<Entity.CodeSheet>.CreateBuilder(rdr);
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
        public static Entity.CodeSheet SelectByID(int codeSheetID, KMPlatform.Object.ClientConnections client)
        {
            
               SqlCommand cmd = new SqlCommand();
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.CommandText = "e_CodeSheet_Select_ID";
               cmd.Connection = DataFunctions.GetClientSqlConnection(client);
               cmd.Parameters.AddWithValue("@CodeSheetID", codeSheetID);
               return Get(cmd);
            
        }
        public static List<Entity.CodeSheet> SelectByResponseGroupID(int responseGroupID, KMPlatform.Object.ClientConnections client)
        {
           
              SqlCommand cmd = new SqlCommand();
              cmd.CommandType = CommandType.StoredProcedure;
              cmd.CommandText = "e_CodeSheet_Select_ResponseGroupID";
              cmd.Connection = DataFunctions.GetClientSqlConnection(client);
              cmd.Parameters.AddWithValue("@ResponseGroupID", responseGroupID);
              return GetList(cmd);
           
        }
        public static DataSet SelectBySearch(int ResponseGroupID, string Name, string SearchCriteria, int CurrentPage, int PageSize, string SortDirection, string SortColumn, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CodeSheet_Select_Search";
            cmd.Parameters.Add(new SqlParameter("@ResponseGroupID", ResponseGroupID));
            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            cmd.Parameters.Add(new SqlParameter("@SearchCriteria", SearchCriteria));
            cmd.Parameters.Add(new SqlParameter("@CurrentPage", CurrentPage));
            cmd.Parameters.Add(new SqlParameter("@PageSize", PageSize));
            cmd.Parameters.Add(new SqlParameter("@SortDirection", SortDirection));
            cmd.Parameters.Add(new SqlParameter("@SortColumn", SortColumn));
            return KM.Common.DataFunctions.GetDataSet(cmd, DataFunctions.GetClientSqlConnection(client).ConnectionString.ToString());
        }
        public static List<Entity.CodeSheet> GetList(SqlCommand cmd)
        {
            List<Entity.CodeSheet> retList = new List<Entity.CodeSheet>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.CodeSheet retItem = new Entity.CodeSheet();
                        DynamicBuilder<Entity.CodeSheet> builder = DynamicBuilder<Entity.CodeSheet>.CreateBuilder(rdr);
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

        public static int Save(Entity.CodeSheet x, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CodeSheet_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@CodeSheetID", x.CodeSheetID);
            cmd.Parameters.AddWithValue("@PubID", (object)x.PubID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ResponseGroupID", (object)x.ResponseGroupID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ResponseGroup", (object)x.ResponseGroup ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ResponseValue", (object)x.ResponseValue ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ResponseDesc", (object)x.ResponseDesc ?? DBNull.Value);
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.AddWithValue("@DisplayOrder", (object)x.DisplayOrder ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ReportGroupID", (object)x.ReportGroupID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive", (object)x.IsActive ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@WQT_ResponseID", (object)x.WQT_ResponseID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsOther", (object)x.IsOther ?? DBNull.Value);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool Delete(KMPlatform.Object.ClientConnections client, int codeSheetID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CodeSheet_Delete";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@CodeSheetID", codeSheetID);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool CodeSheetValidation(int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_CodesheetValidation";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static void CreateNoValueRespones(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_CodeSheet_AddNoValue";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        public static bool CodeSheetValidation_Delete(int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_CodesheetValidation_Delete";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        #region File Validator
        public static bool FileValidator_CodeSheetValidation(int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "job_FileValidator_CodesheetValidation";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@SourceFileID", sourceFileID);
            cmd.Parameters.AddWithValue("@ProcessCode", processCode);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }
        #endregion
    }
}
