using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    public class MasterCodeSheet
    {
        #region Properties
        public int MasterID { get; set; }
        public string MasterValue { get; set; }
        public string MasterDesc { get; set; }
        public int MasterGroupID { get; set; }
        public string MasterDesc1 { get; set; }
        public bool? EnableSearching { get; set; }
        public int SortOrder { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? CreatedByUserID { get; set; }
        public int? UpdatedByUserID { get; set; }
        #endregion

        #region Data
        public static List<MasterCodeSheet> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {

            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<MasterCodeSheet> mcs = (List<MasterCodeSheet>)CacheUtil.GetFromCache("MASTERCODESHEET", DatabaseName);

                if (mcs == null)
                {
                    mcs = GetData(clientconnection);

                    CacheUtil.AddToCache("MASTERCODESHEET", mcs, DatabaseName);
                }

                return mcs;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        private static List<MasterCodeSheet> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<MasterCodeSheet> retList = new List<MasterCodeSheet>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from MasterCodeSheet with (nolock) order by SortOrder ASC", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<MasterCodeSheet> builder = DynamicBuilder<MasterCodeSheet>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    MasterCodeSheet x = builder.Build(rdr);
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

        public static List<MasterCodeSheet> GetSearchEnabled(KMPlatform.Object.ClientConnections clientconnection)
        {
            var mcsList = GetAll(clientconnection);

            var mcsQuery = (from m in mcsList
                                    where m.EnableSearching == null || m.EnableSearching == true
                                    select m);

            return mcsQuery.ToList();
        }


        public static List<MasterCodeSheet> GetByMasterGroupID(KMPlatform.Object.ClientConnections clientconnection, int MasterGroupID)
        {
            return GetAll(clientconnection).FindAll(x => x.MasterGroupID == MasterGroupID);
        }

        public static List<MasterCodeSheet> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<MasterCodeSheet> mastercodesheets = (List<MasterCodeSheet>)CacheUtil.GetFromCache("MASTERCODESHEET_" + brandID, DatabaseName);

                if (mastercodesheets == null)
                {
                    mastercodesheets = GetDataByBrandID(clientconnection, brandID);

                    CacheUtil.AddToCache("MASTERCODESHEET_" + brandID, mastercodesheets, DatabaseName);
                }

                return mastercodesheets;
            }
            else
            {
                return GetDataByBrandID(clientconnection, brandID);
            }
        }

        private static List<MasterCodeSheet> GetDataByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            List<MasterCodeSheet> retList = new List<MasterCodeSheet>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_MasterCodeSheet_Select_ByBrandID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BrandID", brandID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<MasterCodeSheet> builder = DynamicBuilder<MasterCodeSheet>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    MasterCodeSheet x = builder.Build(rdr);
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

        public static List<MasterCodeSheet> GetSearchEnabledByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            var mcsList = GetByBrandID(clientconnection, brandID);

            var mcsQuery = (from m in mcsList
                            where m.EnableSearching == null || m.EnableSearching == true
                            select m);

            return mcsQuery.ToList();
        }

        public static DataTable GetByCodeSheetID(KMPlatform.Object.ClientConnections clientconnection, int codeSheetID)
        {
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            string query = "Select mcs.MasterID, mg.DisplayName + ' - ' + mcs.MasterValue + ' - ' + mcs.MasterDesc as 'Display',  mg.DisplayName, mcs.MasterGroupID " +
                    " from Mastercodesheet mcs  with (nolock) " +
                    " inner join MasterGroups mg with (nolock) " +
                    " on mcs.MasterGroupID = mg.MasterGroupID " +
                    " inner join CodeSheet_Mastercodesheet_Bridge cmb with (nolock) " +
                    " on mcs.MasterID=cmb.MasterID where CodeSheetID = @CodeSheetID " +
                    " order by mg.DisplayName";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@CodeSheetID", codeSheetID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static MasterCodeSheet GetByMasterID(KMPlatform.Object.ClientConnections clientconnection, int masterID)
        {
            MasterCodeSheet retItem = new MasterCodeSheet();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from MasterCodeSheet mc with (nolock) where mc.MasterID = @MasterID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@MasterID", masterID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<MasterCodeSheet> builder = DynamicBuilder<MasterCodeSheet>.CreateBuilder(rdr);
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

        public static int ExistsByName(KMPlatform.Object.ClientConnections clientconnection, string MasterValue, int MasterGroupID)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM mastercodesheet with (nolock) where MasterValue = @MasterValue and mastergroupID = @MasterGroupID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@MasterValue", MasterValue));
            cmd.Parameters.Add(new SqlParameter("@MasterGroupID", MasterGroupID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static int ExistsByIDName(KMPlatform.Object.ClientConnections clientconnection, int MasterID, string MasterValue, int MasterGroupID)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM mastercodesheet with (nolock) where MasterID <> @MasterID and MasterValue = @MasterValue and mastergroupID = @MasterGroupID");
            cmd.CommandType = CommandType.Text;            
            cmd.Parameters.Add(new SqlParameter("@MasterID", MasterID));
            cmd.Parameters.Add(new SqlParameter("@MasterValue", MasterValue));
            cmd.Parameters.Add(new SqlParameter("@MasterGroupID", MasterGroupID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("MASTERCODESHEET", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("MASTERCODESHEET", DatabaseName);
                }

                //string DatabaseName = DataFunctions.GetDBName(clientconnection);

                //if (System.Web.HttpContext.Current.Cache[DatabaseName + "_MASTERCODESHEET"] != null)
                //{
                //    System.Web.HttpContext.Current.Cache.Remove(DatabaseName + "_MASTERCODESHEET");
                //}

                List<Brand> brand = Brand.GetAll(clientconnection);

                foreach (Brand b in brand)
                {
                    DeleteCacheByBrandID(clientconnection, b.BrandID);
                }
            }
        }

        public static void DeleteCacheByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("MASTERCODESHEET_" + brandID, DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("MASTERCODESHEET_" + brandID, DatabaseName);
                }
            }
        }
        #endregion

        #region CRUD

        public static int Save(KMPlatform.Object.ClientConnections clientconnection, MasterCodeSheet ms)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("spSaveMasterCodeSheet");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MasterID", ms.MasterID));
            cmd.Parameters.Add(new SqlParameter("@MasterGroupID", ms.MasterGroupID));
            cmd.Parameters.Add(new SqlParameter("@MasterValue", ms.MasterValue));
            cmd.Parameters.Add(new SqlParameter("@MasterDesc", ms.MasterDesc));
            cmd.Parameters.Add(new SqlParameter("@MasterDesc1", ms.MasterDesc1));
            cmd.Parameters.Add(new SqlParameter("@EnableSearching", ms.EnableSearching));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", (object)ms.CreatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", (object)ms.DateCreated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)ms.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)ms.DateUpdated ?? DBNull.Value));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void update(KMPlatform.Object.ClientConnections clientconnection, int SortOrder, int MasterID)
        {
            DeleteCache(clientconnection);     
            SqlCommand cmd = new SqlCommand("Update MasterCodeSheet set SortOrder = @SortOrder where MasterID = @MasterID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@SortOrder", SortOrder));
            cmd.Parameters.Add(new SqlParameter("@MasterID", MasterID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int MasterID)
        {
            DeleteCache(clientconnection); 
            SqlCommand cmd = new SqlCommand("sp_Mastercodesheet_Delete");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MasterID", MasterID));
            DataFunctions.execute(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }

        public static void Import(KMPlatform.Object.ClientConnections clientconnection, int masterGroupID, XDocument xDoc, int userID)
        {
            SqlCommand cmd = new SqlCommand("SP_IMPORT_SUBSCRIBER_MASTERCODESHEET");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MasterGroupID", masterGroupID));
            cmd.Parameters.Add(new SqlParameter("@ImportXML", xDoc.ToString(SaveOptions.DisableFormatting)));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));

            DeleteCache(clientconnection);
        }
        #endregion
    }
}