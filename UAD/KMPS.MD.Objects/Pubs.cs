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
    public class Pubs
    {
        private const string PubsDeleteCommandText = "sp_Pubs_Delete";
        private const string PubsValidateDeleteOrInactiveCommandText = "e_Product_Validate_DeleteorInActive";
        private const string PubsIdKey = "@PubID";
        private const string ReferenceKey = "Reference";
        private const string ReferenceNameKey = "ReferenceName";
        private const string ParameterKeySuffix = " : ";

        #region Properties
        public int PubID { get; set; }
        public string PubName { get; set; }
        public string PubCode { get; set; }
        public int PubTypeID { get; set; }
        public int GroupID { get; set; }
        public bool istradeshow { get; set; }
        public bool? EnableSearching { get; set; }
        public int Score { get; set; }
        public string PubTypeDisplayName { get; set; }
        public int SortOrder { get; set; }
        public bool IsCirc { get; set; }
        public bool HasPaidRecords { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? CreatedByUserID { get; set; }
        public int? UpdatedByUserID { get; set; }
        public bool IsImported { get; set; }
        public bool IsActive { get; set; }
        public bool AllowDataEntry { get; set; }
        public bool KMImportAllowed { get; set; }
        public bool ClientImportAllowed { get; set; }
        public bool AddRemoveAllowed { get; set; }
        public bool IsUAD { get; set; }
        public bool IsOpenCloseLocked { get; set; }
        public bool UseSubGen { get; set; }
        public int? FrequencyID { get; set; }
        public string YearStartDate { get; set; }
        public string YearEndDate { get; set; }
        #endregion

        #region Data
        public static int ExistsByIDName(KMPlatform.Object.ClientConnections clientconnection, int PubID, string Name)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Pubs with (nolock) where PubID <> @PubID and PubName = @Name");
            cmd.Parameters.Add(new SqlParameter("@PubID", PubID));
            cmd.Parameters.Add(new SqlParameter("@Name", Name));
            cmd.CommandType = CommandType.Text;
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }
        public static int ExistsByIDCode(KMPlatform.Object.ClientConnections clientconnection, int PubID, string PubCode)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Pubs with (nolock) where PubID <> @PubID and PubCode = @PubCode");
            cmd.Parameters.Add(new SqlParameter("@PubID", PubID));
            cmd.Parameters.Add(new SqlParameter("@PubCode", PubCode));
            cmd.CommandType = CommandType.Text;
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static List<Pubs> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<Pubs> pubs = (List<Pubs>) CacheUtil.GetFromCache("PUBS", DatabaseName);

                if (pubs == null)
                {
                    pubs = GetData(clientconnection);

                    CacheUtil.AddToCache("PUBS", pubs, DatabaseName);
                }

                return pubs;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        private static List<Pubs> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Pubs> retList = new List<Pubs>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select isNull(Score,0) as Score, pubID, PubName, PubCode, pt.PubTypeID, GroupID, IsTradeshow, enablesearching, IsImported, isnull(p.IsActive,1)  as IsActive, AllowDataEntry, KMImportAllowed, ClientImportAllowed, AddRemoveAllowed, IsUAD, IsCirc, IsOpenCloseLocked, UseSubGen, PubTypeDisplayName, p.SortOrder, isnull(p.IsCirc, 0)  as IsCirc, HasPaidRecords, FrequencyID, YearStartDate, YearEndDate from Pubs p with (nolock) Inner Join PubTypes pt with (nolock) on p.PubTypeID = pt.PubTypeID order by pubname asc", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Pubs> builder = DynamicBuilder<Pubs>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Pubs x = builder.Build(rdr);
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

        public static List<Pubs> GetSearchEnabled(ClientConnections clientConnections)
        {
            return GetActiveSorted(clientConnections, pubs => pubs.EnableSearching == true).ToList();
        }

        public static List<Pubs> GetActive(ClientConnections clientConnections)
        {
            return GetActiveSorted(clientConnections).ToList();
        }

        public static Pubs GetByID(KMPlatform.Object.ClientConnections clientconnection, int PubID)
        {
            return GetAll(clientconnection).Find(x => x.PubID == PubID);
        }

        public static List<Pubs> GetByPubTypeID(KMPlatform.Object.ClientConnections clientconnection, int PubTypeID)
        {
            return GetAll(clientconnection).FindAll(x => x.PubTypeID == PubTypeID);
        }

        public static List<Pubs> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<Pubs> pubs = (List<Pubs>)CacheUtil.GetFromCache("PUBS_" + brandID, DatabaseName);

                if (pubs == null)
                {
                    pubs = GetDataByBrandID(clientconnection, brandID);
                    CacheUtil.AddToCache("PUBS_" + brandID, pubs, DatabaseName);
                }

                return pubs;
            }
            else
            {
                return GetDataByBrandID(clientconnection, brandID);
            }
        }

        private static List<Pubs> GetDataByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            List<Pubs> retList = new List<Pubs>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select  p.pubID, PubName, PubCode, pt.PubTypeID, GroupID, IsTradeshow, enablesearching, IsImported, isnull(p.IsActive,1) as IsActive, AllowDataEntry, KMImportAllowed, ClientImportAllowed, AddRemoveAllowed, IsUAD, IsCirc, IsOpenCloseLocked, UseSubGen, pt.PubTypeDisplayName, p.SortOrder, isnull(p.IsCirc, 0)  as IsCirc, HasPaidRecords, FrequencyID, YearStartDate, YearEndDate  from Pubs p  with (nolock) join pubtypes pt with (nolock) on p.PubTypeID = pt.PubTypeID join BrandDetails bd with (nolock) on p.PubID = bd.PubID join Brand b with (nolock) on b.BrandID = bd.BrandID where b.IsDeleted = 0 and bd.brandID = @brandID order by pubname asc", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@brandID", brandID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Pubs> builder = DynamicBuilder<Pubs>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Pubs x = builder.Build(rdr);
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

        public static List<Pubs> GetSearchEnabledByBrandID(ClientConnections clientConnections, int brandId)
        {
            return GetActiveByBrandIdSorted(clientConnections, brandId, pubs => pubs.EnableSearching == true).ToList();
        }

        public static List<Pubs> GetActiveByBrandID(ClientConnections clientConnections, int brandId)
        {
            return GetActiveByBrandIdSorted(clientConnections, brandId).ToList();
        }

        public static bool ExistsByPubTypeID(KMPlatform.Object.ClientConnections clientconnection, int PubTypeID)
        {
            List<Pubs> pubs = Pubs.GetAll(clientconnection).FindAll(x => x.PubTypeID == PubTypeID);
            return pubs.Any() ? true : false;
        }


        public static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("PUBS", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("PUBS", DatabaseName);
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

                if (CacheUtil.GetFromCache("PUBS_" + brandID, DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("PUBS_" + brandID, DatabaseName);
                }
            }
        }

        private static IEnumerable<Pubs> GetActiveSorted(ClientConnections clientConnections, Predicate<Pubs> filterPredicate = null)
        {
            return GetAll(clientConnections)
                .Where(pubs => pubs.IsActive && (filterPredicate == null || filterPredicate(pubs)))
                .OrderBy(pubs => pubs.SortOrder);
        }

        private static IOrderedEnumerable<Pubs> GetActiveByBrandIdSorted(
            ClientConnections clientConnections,
            int brandId,
            Predicate<Pubs> filterPredicate = null)
        {
            return GetByBrandID(clientConnections, brandId)
                .Where(pubs => pubs.IsActive && (filterPredicate == null || filterPredicate(pubs)))
                .OrderBy(pubs => pubs.SortOrder);
        }

        #endregion

        #region CRUD 
        public int Save(KMPlatform.Object.ClientConnections clientconnection)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_SavePubs");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PubID", PubID));
            cmd.Parameters.Add(new SqlParameter("@PubName", PubName));
            cmd.Parameters.Add(new SqlParameter("@PubCode", PubCode));
            cmd.Parameters.Add(new SqlParameter("@PubTypeID", PubTypeID));
            cmd.Parameters.Add(new SqlParameter("@EnableSearching", EnableSearching));
            cmd.Parameters.Add(new SqlParameter("@Score", Score));
            cmd.Parameters.Add(new SqlParameter("@HasPaidRecords", HasPaidRecords));
            cmd.Parameters.Add(new SqlParameter("@IsActive", IsActive));
            cmd.Parameters.Add(new SqlParameter("@IsUAD", IsUAD));
            cmd.Parameters.Add(new SqlParameter("@IsCirc", IsCirc));
            cmd.Parameters.Add(new SqlParameter("@UseSubGen", UseSubGen));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", (object)CreatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateCreated", (object)DateCreated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FrequencyID", (object)FrequencyID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@YearStartDate", (object)YearStartDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@YearEndDate", (object)YearEndDate ?? DBNull.Value));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void SaveSortOrder(KMPlatform.Object.ClientConnections clientconnection, Pubs p)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_Pubs_Save_SortOrder");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PubID", p.PubID));
            cmd.Parameters.Add(new SqlParameter("@SortOrder", p.SortOrder));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }

        public static NameValueCollection ValidationForDeleteorInActive(ClientConnections clientConnections, int productId)
        {
            var result = UADUtilities.ValidateDeleteOrInActive(
                clientConnections,
                PubsValidateDeleteOrInactiveCommandText,
                GetCommandParameter(productId),
                ProcessDataReader);

            return result;
        }

        public static void Delete(ClientConnections clientConnections, int productId)
        {
            DeleteCache(clientConnections);
            UADUtilities.Delete(clientConnections, PubsDeleteCommandText, GetCommandParameter(productId));
        }

        private static IEnumerable<IDbDataParameter> GetCommandParameter(int productId)
        {
            return new List<SqlParameter>()
            {
                new SqlParameter(PubsIdKey, productId)
            };
        }

        private static NameValueCollection ProcessDataReader(IDataReader dataReader)
        {
            var result = new NameValueCollection();
            while (dataReader.Read())
            {
                result.Add($"{dataReader[ReferenceKey]}{ParameterKeySuffix}", dataReader[ReferenceNameKey].ToString());
            }

            return result;
        }

        public static void Import(KMPlatform.Object.ClientConnections clientconnection, XDocument xDoc, int userID)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_Import_Product");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@importXML", xDoc.ToString()));
            cmd.Parameters.Add(new SqlParameter("@userID", userID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
       #endregion
    }
}