using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using KM.Common;
using System.Xml.Linq;
using KMPS.MD.Objects;
using System.Collections.Specialized;

namespace KMPS.MD.Objects
{
    public class CodeSheet
    {
        #region Properties
        public int CodeSheetID { get; set; }
        public int PubID { get; set; }
        public string ResponseGroup { get; set; }
        public string ResponseValue { get; set; }
        public string ResponseDesc { get; set; }
        public int ResponseGroupID { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? CreatedByUserID { get; set; }
        public int? UpdatedByUserID { get; set; }
        public int DisplayOrder { get; set; }
        public int? ReportGroupID { get; set; }
        public bool IsActive { get; set; }
        public int WQT_ResponseID { get; set; }
        public bool IsOther { get; set; }
        #endregion

        #region Data
        public static List<CodeSheet> GetByResponseGroupID(KMPlatform.Object.ClientConnections clientconnection, int responseGroupID)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<CodeSheet> codeSheets = (List<CodeSheet>)CacheUtil.GetFromCache("CODESHEET_" + responseGroupID, DatabaseName);

                if (codeSheets == null)
                {
                    codeSheets = GetData(clientconnection, responseGroupID);

                    CacheUtil.AddToCache("CODESHEET_" + responseGroupID, codeSheets, DatabaseName);
                }

                return codeSheets;
            }
            else
            {
                return GetData(clientconnection, responseGroupID);
            }
        }

        private static List<CodeSheet> GetData(KMPlatform.Object.ClientConnections clientconnection, int responseGroupID)
        {
            List<CodeSheet> retList = new List<CodeSheet>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from CodeSheet where ResponseGroupID = @ResponseGroupID and isnull(IsActive, 1) = 1", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ResponseGroupID", responseGroupID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<CodeSheet> builder = DynamicBuilder<CodeSheet>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    CodeSheet x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return retList;
        }

        public static List<CodeSheet> GetByPubIDResponseGroupID(KMPlatform.Object.ClientConnections clientconnection, int pubID, int responseGroupID)
        {
            List<CodeSheet> retList = new List<CodeSheet>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from CodeSheet where pubID = @PubID and ResponseGroupID = @ResponseGroupID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@PubID", pubID);
            cmd.Parameters.AddWithValue("@ResponseGroupID", responseGroupID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<CodeSheet> builder = DynamicBuilder<CodeSheet>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    CodeSheet x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return retList;
        }

        public static CodeSheet GetByCodeSheetID(KMPlatform.Object.ClientConnections clientconnection, int codeSheetID)
        {
            CodeSheet retItem = new CodeSheet();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select CodeSheetID, PubID, ResponseGroup, Responsevalue, Responsedesc, ResponseGroupID, DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID, DisplayOrder, ReportGroupID, Isnull(IsActive, 1) as IsActive, WQT_ResponseID, IsOther from CodeSheet where CodeSheetID = @CodeSheetID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@CodeSheetID", codeSheetID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<CodeSheet> builder = DynamicBuilder<CodeSheet>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    retItem = builder.Build(rdr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return retItem;
        }

        public static List<CodeSheet> GetByPubID(KMPlatform.Object.ClientConnections clientconnection, int pubID)
        {
            List<CodeSheet> retList = new List<CodeSheet>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from CodeSheet where pubID = @PubID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@PubID", pubID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<CodeSheet> builder = DynamicBuilder<CodeSheet>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    CodeSheet x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return retList;
        }

        public static bool ExistsByResponseGroupIDResponseValue(KMPlatform.Object.ClientConnections clientconnection, int codesheetID, int responseGroupID, string responseValue)
        {
            SqlCommand cmd = new SqlCommand("e_Codesheet_Exists_ByResponseGroupIDValue");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ResponseGroupID", responseGroupID);
            cmd.Parameters.AddWithValue("@ResponseValue", responseValue);
            cmd.Parameters.AddWithValue("@CodeSheetID", codesheetID);
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }
        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, CodeSheet cs, StringBuilder sb)
        {
            DeleteCache(clientconnection, cs.ResponseGroupID);

            SqlCommand cmd = new SqlCommand("spSaveCodeSheet");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@CodeSheetID",cs.CodeSheetID));
            cmd.Parameters.Add(new SqlParameter("@PubID", cs.PubID));
            cmd.Parameters.Add(new SqlParameter("@ResponseGroupID", cs.ResponseGroupID));
            cmd.Parameters.Add(new SqlParameter("@ResponseGroup", cs.ResponseGroup));
            cmd.Parameters.Add(new SqlParameter("@ResponseValue", cs.ResponseValue));
            cmd.Parameters.Add(new SqlParameter("@ResponseDesc", cs.ResponseDesc));
            cmd.Parameters.Add(new SqlParameter("@xmlDocument", sb.ToString()));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", (object)cs.CreatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", (object)cs.DateCreated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)cs.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)cs.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsActive", (object)cs.IsActive ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsOther", (object)cs.IsOther ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ReportGroupID", (object)cs.ReportGroupID ?? DBNull.Value));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Update(KMPlatform.Object.ClientConnections clientconnection, int responseGroupID, string responseGroup)
        {
            DeleteCache(clientconnection, responseGroupID);

            SqlCommand cmd = new SqlCommand("Update CodeSheet set ResponseGroup = @ResponseGroup where ResponseGroupID = @ResponseGroupID");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@ResponseGroupID", responseGroupID));
            cmd.Parameters.Add(new SqlParameter("@ResponseGroup", responseGroup));
            DataFunctions.execute(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int CodeSheetID, int responseGroupID)
        {
            DeleteCache(clientconnection, responseGroupID);

            SqlCommand cmd = new SqlCommand("sp_CodeSheet_Delete");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@CodeSheetID", CodeSheetID));
            DataFunctions.execute(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }

        public static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection, int responseGroupID)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("CODESHEET_" + responseGroupID, DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("CODESHEET_" + responseGroupID, DatabaseName);
                }
            }
        }

        public static NameValueCollection Import(KMPlatform.Object.ClientConnections clientconnection, XDocument xDoc, int userID)
        {
            NameValueCollection nvReturn = new NameValueCollection();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_Import_CodeSheet", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@importXML", xDoc.ToString()));
            cmd.Parameters.Add(new SqlParameter("@userID", userID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    nvReturn.Add(rdr["Reference"].ToString() + " : ", rdr["ReferenceError"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            List<Pubs> lp = Pubs.GetAll(clientconnection);

            foreach (Pubs p in lp)
            {
                List<ResponseGroup> lrg = KMPS.MD.Objects.ResponseGroup.GetByPubID(clientconnection, p.PubID);

                foreach (ResponseGroup rg in lrg)
                {
                    CodeSheet.DeleteCache(clientconnection, rg.ResponseGroupID);
                }

                KMPS.MD.Objects.ResponseGroup.DeleteCache(clientconnection, p.PubID);
            }

            MasterGroup.DeleteCache(clientconnection);
            MasterCodeSheet.DeleteCache(clientconnection);

            return nvReturn;
        }
        #endregion
    }
}