using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using KM.Common;
using KMPlatform.Object;
using UADUtilities = FrameworkUAD.DataAccess.Utilities;

namespace KMPS.MD.Objects
{
    [Serializable]
    public class MasterGroup
    {
        private const string MasterGroupDeleteCommandText = "sp_MasterGroup_Delete";
        private const string MasterGroupValidateDeleteOrInactiveCommandText = "e_MasterGroup_Validate_DeleteorInActive";
        private const string MasterGroupIdKey = "@MasterGroupID";

        #region Properties
        public int MasterGroupID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string ColumnReference { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public bool EnableSubReporting { get; set; }
        public bool EnableSearching { get; set; }
        public bool EnableAdhocSearch { get; set; }
	    public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
	    public int? CreatedByUserID { get; set; }
        public int? UpdatedByUserID { get; set; }

        #endregion

        #region Data
        public static int ExistsByName(KMPlatform.Object.ClientConnections clientconnection, string DisplayName)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM mastergroups with (nolock) where Displayname = @DisplayName");
            cmd.Parameters.Add(new SqlParameter("@DisplayName", DisplayName));
            cmd.CommandType = CommandType.Text;
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static int ExistsByIDDisplayName(KMPlatform.Object.ClientConnections clientconnection, int MasterGroupID, string DisplayName)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM mastergroups with (nolock) where mastergroupID <> @MasterGroupID and Displayname = @DisplayName");
            cmd.Parameters.Add(new SqlParameter("@MasterGroupID", MasterGroupID));
            cmd.Parameters.Add(new SqlParameter("@DisplayName", DisplayName));
            cmd.CommandType = CommandType.Text;
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }
        public static int ExistsByIDName(KMPlatform.Object.ClientConnections clientconnection, int MasterGroupID, string Name)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM mastergroups with (nolock) where mastergroupID <> @MasterGroupID and Name = @Name");
            cmd.Parameters.Add(new SqlParameter("@MasterGroupID", MasterGroupID));
            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            cmd.CommandType = CommandType.Text;
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

       
        public static List<MasterGroup> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<MasterGroup> mastergroups = (List<MasterGroup>)CacheUtil.GetFromCache("MASTERGROUPS", DatabaseName);

                if (mastergroups == null)
                {
                    mastergroups = GetData(clientconnection);

                    CacheUtil.AddToCache("MASTERGROUPS", mastergroups, DatabaseName);
                }

                return mastergroups;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        public static List<MasterGroup> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<MasterGroup> retList = new List<MasterGroup>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from MasterGroups with (nolock) order by SortOrder", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                retList = ExcRdrList(rdr);
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

        public static MasterGroup GetByID(KMPlatform.Object.ClientConnections clientconnection, int MasterGroupID)
        {
            MasterGroup mg = GetAll(clientconnection).Find(x => x.MasterGroupID == MasterGroupID);
            return mg;
        }

        public static List<MasterGroup> GetActiveMasterGroupsSorted(KMPlatform.Object.ClientConnections clientconnection)
        {
            var masterGroupList = GetAll(clientconnection);

            var MasterGroupQuery = (from m in masterGroupList
                                    where m.IsActive == true 
                                    orderby m.SortOrder ascending
                                    select m);

            return MasterGroupQuery.ToList();
        }

        public static List<MasterGroup> GetSearchEnabled(KMPlatform.Object.ClientConnections clientconnection)
        {
            var masterGroupList = GetAll(clientconnection);

            var MasterGroupQuery = (from m in masterGroupList
                                    where m.IsActive == true && m.EnableSearching == true
                                    orderby m.SortOrder ascending
                                    select m);

            return MasterGroupQuery.ToList();
        }

        public static List<MasterGroup> GetSubReportEnabled(KMPlatform.Object.ClientConnections clientconnection)
        {
            var masterGroupList = GetAll(clientconnection);

            var MasterGroupQuery = (from m in masterGroupList
                                    where m.IsActive == true && m.EnableSubReporting == true
                                    orderby m.SortOrder ascending
                                    select m);

            return MasterGroupQuery.ToList();
        }

        private static List<MasterGroup> ExcRdrList(SqlDataReader rdr)
        {
            List<MasterGroup> retList = new List<MasterGroup>();
            #region Reader
            while (rdr.Read())
            {
                MasterGroup retItem = new MasterGroup();
                int index;
                string name;

                #region Reader
                name = "MasterGroupID";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.MasterGroupID = Convert.ToInt32(rdr[index]);

                name = "Name";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.Name =  rdr[index].ToString();

                name = "Description";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.Description = rdr[index].ToString();

                name = "DisplayName";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.DisplayName = rdr[index].ToString();

                name = "ColumnReference";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.ColumnReference = rdr[index].ToString();

                name = "SortOrder";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.SortOrder = Convert.ToInt32(rdr[index]);

                name = "IsActive";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.IsActive = Convert.ToBoolean(rdr[index]);

                name = "EnableSubReporting";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.EnableSubReporting = Convert.ToBoolean(rdr[index]);

                name = "EnableSearching";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.EnableSearching = Convert.ToBoolean(rdr[index]);

                name = "EnableAdhocSearch";
                index = rdr.GetOrdinal(name);
                if (!rdr.IsDBNull(index))
                    retItem.EnableAdhocSearch = Convert.ToBoolean(rdr[index]);

                #endregion

                retList.Add(retItem);
            }
            #endregion

            return retList;
        }

        public static List<MasterGroup> getByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<MasterGroup> mastergroup = (List<MasterGroup>)CacheUtil.GetFromCache("MASTERGROUPS_" + brandID, DatabaseName);

                if (mastergroup == null)
                {
                    mastergroup = GetDataByBrandID(clientconnection, brandID);

                    CacheUtil.AddToCache("MASTERGROUPS_" + brandID, mastergroup, DatabaseName);
                }

                return mastergroup;
            }
            else
            {
                return GetDataByBrandID(clientconnection, brandID);
            }
        }

        private static List<MasterGroup> GetDataByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            List<MasterGroup> retList = new List<MasterGroup>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_MasterGroup_Select_ByBrandID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BrandID", brandID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<MasterGroup> builder = DynamicBuilder<MasterGroup>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    MasterGroup x = builder.Build(rdr);
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

        public static List<MasterGroup> GetActiveByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            var masterGroupList = getByBrandID(clientconnection, brandID);

            var MasterGroupQuery = (from m in masterGroupList
                                    where m.IsActive == true && m.EnableSubReporting == true
                                    orderby m.SortOrder ascending
                                    select m);

            return MasterGroupQuery.ToList();
        }

        public static List<MasterGroup> GetSearchEnabledByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            var masterGroupList = getByBrandID(clientconnection, brandID);

            var MasterGroupQuery = (from m in masterGroupList
                                    where m.IsActive == true && m.EnableSearching == true
                                    orderby m.SortOrder ascending
                                    select m);

            return MasterGroupQuery.ToList();
        }

        public static List<MasterGroup> GetSubReportEnabledByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            var masterGroupList = getByBrandID(clientconnection, brandID);

            var MasterGroupQuery = (from m in masterGroupList
                                    where m.IsActive == true && m.EnableSubReporting == true
                                    orderby m.SortOrder ascending
                                    select m);

            return MasterGroupQuery.ToList();
        }


        public static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("MASTERGROUPS", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("MASTERGROUPS", DatabaseName);
                }

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

                if (CacheUtil.GetFromCache("MASTERGROUPS_" + brandID, DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("MASTERGROUPS_" + brandID, DatabaseName);
                }
            }
        }
        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, MasterGroup mg)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_MasterGroup_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MasterGroupID", mg.MasterGroupID));
            cmd.Parameters.Add(new SqlParameter("@DisplayName", mg.DisplayName));
            cmd.Parameters.Add(new SqlParameter("@Name", mg.Name));
            cmd.Parameters.Add(new SqlParameter("@IsActive", mg.IsActive));
            cmd.Parameters.Add(new SqlParameter("@EnableSubReporting", mg.EnableSubReporting));
            cmd.Parameters.Add(new SqlParameter("@EnableSearching", mg.EnableSearching));
            cmd.Parameters.Add(new SqlParameter("@EnableAdhocSearch", mg.EnableAdhocSearch));
            cmd.Parameters.Add(new SqlParameter("@SortOrder", mg.SortOrder));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)mg.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)mg.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", (object)mg.DateCreated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", (object)mg.CreatedByUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static NameValueCollection ValidationForDeleteorInActive(ClientConnections clientConnections, int masterGroupId)
        {
            var result = UADUtilities.ValidateDeleteOrInActive(
                clientConnections,
                MasterGroupValidateDeleteOrInactiveCommandText,
                GetCommandParameter(masterGroupId),
                FilterExportScheduleProcessor.ProcessDataReader);

            return result;
        }

        public static void Delete(ClientConnections clientConnections, int masterGroupId)
        {
            DeleteCache(clientConnections);
            UADUtilities.Delete(clientConnections, MasterGroupDeleteCommandText, GetCommandParameter(masterGroupId));
        }

        public static NameValueCollection Import(KMPlatform.Object.ClientConnections clientconnection, XDocument xDoc, int userID)
        {
            DeleteCache(clientconnection);

            NameValueCollection nvReturn = new NameValueCollection();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_Import_MasterGroup", conn);
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
                    nvReturn.Add(rdr["Reference"].ToString() + " : ",  rdr["ReferenceError"].ToString());
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
            return nvReturn;
        }

        private static IEnumerable<SqlParameter> GetCommandParameter(int masterGroupId)
        {
            return new List<SqlParameter>()
            {
                new SqlParameter(MasterGroupIdKey, masterGroupId)
            };
        }

        #endregion
    }
}
