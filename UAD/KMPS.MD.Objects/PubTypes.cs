using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class PubTypes
    {
        #region Properties
        [DataMember]
        public int PubTypeID { get; set; }
        [DataMember]
        public string PubTypeDisplayName { get; set; }
        [DataMember]
        public string ColumnReference { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        #endregion

        #region Data
        public static List<PubTypes> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<PubTypes> pubtypes = (List<PubTypes>)CacheUtil.GetFromCache("PUBTYPES", DatabaseName);

                if (pubtypes == null)
                {
                    pubtypes = GetData(clientconnection);

                    CacheUtil.AddToCache("PUBTYPES", pubtypes, DatabaseName);
                }

                return pubtypes;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        private static List<PubTypes> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<PubTypes> retList = new List<PubTypes>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("Select * From PubTypes with (nolock) order by SortOrder ASC", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<PubTypes> builder = DynamicBuilder<PubTypes>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    PubTypes x = builder.Build(rdr);
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

        public static List<PubTypes> GetActive(KMPlatform.Object.ClientConnections clientconnection)
        {
            return GetAll(clientconnection).FindAll(x => x.IsActive == true);
        }

        public static PubTypes GetByID(KMPlatform.Object.ClientConnections clientconnection, int pubtypeID)
        {
            return GetAll(clientconnection).Find(x => x.PubTypeID == pubtypeID);
        }

        public static PubTypes GetByColumnReference(KMPlatform.Object.ClientConnections clientconnection, string ColumnReference)
        {
            return GetAll(clientconnection).Find(x => x.ColumnReference == ColumnReference);
        }

        public static List<PubTypes> GetActiveByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<PubTypes> pubtypes = (List<PubTypes>)CacheUtil.GetFromCache("PUBTYPES_" + brandID, DatabaseName);

                if (pubtypes == null)
                {
                    pubtypes = GetDataByBrandID(clientconnection, brandID);

                    CacheUtil.AddToCache("PUBTYPES_" + brandID, pubtypes, DatabaseName);
                }

                return pubtypes;
            }
            else
            {
                return GetDataByBrandID(clientconnection, brandID);
            }
        }

        private static List<PubTypes> GetDataByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            List<PubTypes> retList = new List<PubTypes>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select distinct pt.PubTypeID, ColumnReference, PubTypeDisplayName,  pt.IsActive, pt.SortOrder FROM  Brand b with (nolock) join branddetails bd with (nolock) on b.BrandID = bd.BrandID join pubs p with (nolock) ON bd.pubid = p.pubid join PubTypes pt  with (nolock) on pt.PubTypeID = p.PubTypeID where b.IsDeleted = 0 and pt.IsActive = 1 and b.BrandID = @brandID order by SortOrder", conn);
            cmd.Parameters.Add(new SqlParameter("@brandID", brandID));
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<PubTypes> builder = DynamicBuilder<PubTypes>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    PubTypes x = builder.Build(rdr);
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

        public static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("PUBTYPES", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("PUBTYPES", DatabaseName);
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

                if (CacheUtil.GetFromCache("PUBTYPES_" + brandID, DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("PUBTYPES_" + brandID, DatabaseName);
                }
            }
        }

        public static bool ExistsByPubTypeDisplayName(KMPlatform.Object.ClientConnections clientconnection, string PubTypeDisplayName, int PubTypeID)
        {
            SqlCommand cmd = new SqlCommand("Select * from PubTypes with (nolock) where PubTypeDisplayName = @PubTypeDisplayName and PubTypeID <> @PubTypeID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@PubTypeID", PubTypeID));
            cmd.Parameters.Add(new SqlParameter("@PubTypeDisplayName", PubTypeDisplayName));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }
        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, PubTypes pubtypes)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_PubTypes_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PubTypeID", pubtypes.PubTypeID));
            cmd.Parameters.Add(new SqlParameter("@PubTypeDisplayName", pubtypes.PubTypeDisplayName));
            cmd.Parameters.Add(new SqlParameter("@ColumnReference", pubtypes.ColumnReference));
            cmd.Parameters.Add(new SqlParameter("@IsActive", pubtypes.IsActive));
            cmd.Parameters.Add(new SqlParameter("@SortOrder", pubtypes.SortOrder));

            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int PubTypeID)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_PubTypes_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@PubTypeID", PubTypeID));
            DataFunctions.execute(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion

    }
}