using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FrameworkUAD.Entity;

namespace FrameworkUAD.DataAccess
{
    public class Product
    {
        public static bool ExistsByPubTypeID(int pubTypeID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Product_Exists_ByPubTypeID";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.Add(new SqlParameter("@PubTypeID", pubTypeID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd)) > 0 ? true : false;
        }
        public static List<Entity.Product> Select(KMPlatform.Object.ClientConnections client)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(client);
                List<Entity.Product> retItem = (List<Entity.Product>) CacheUtil.GetFromCache("PUBS", DatabaseName);
                if (retItem == null)
                {
                    retItem = GetData(client);
                    CacheUtil.AddToCache("PUBS", retItem, DatabaseName);
                }

                return retItem;
            }
            else
            {
                return GetData(client);
            }
            
        }
        

        public static Entity.Product Select(int pubID, KMPlatform.Object.ClientConnections client, bool GetLatestData=false)
        {
           
            if (CacheUtil.IsCacheEnabled() && !GetLatestData)
            {
                return Select(client).Find(x => x.PubID == pubID);
            }
            else
            {
                Entity.Product retItem = null;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Product_Select_PubID";
                cmd.Connection = DataFunctions.GetClientSqlConnection(client);
                cmd.Parameters.AddWithValue("@PubID", pubID);
                retItem = Get(cmd);
                return retItem;
            }
        }

        public static Entity.Product Select(string pubCode, KMPlatform.Object.ClientConnections client)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                return Select(client).Find(x => x.PubCode == pubCode);
            }
            else
            {
                Entity.Product retItem = null;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Product_Select_PubCode";
                cmd.Connection = DataFunctions.GetClientSqlConnection(client);
                cmd.Parameters.AddWithValue("@PubCode", pubCode);
                retItem = Get(cmd);
                return retItem;
            }
        }

        public static List<Entity.Product> SelectBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<Entity.Product> pubs = (List<Entity.Product>) CacheUtil.GetFromCache("PUBS_" + brandID, DatabaseName);

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

        private static List<Entity.Product> GetData(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Product_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            List<Entity.Product> retList = new List<Entity.Product>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.Product retItem = new Entity.Product();
                        DynamicBuilder<Entity.Product> builder = DynamicBuilder<Entity.Product>.CreateBuilder(rdr);
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
            catch(Exception ex) {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
        private static List<Entity.Product> GetDataByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            List<Entity.Product> retList = new List<Entity.Product>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select  p.PubID, PubName, PubCode, pt.PubTypeID, GroupID, istradeshow, EnableSearching, IsImported, isnull(p.IsActive,1) as IsActive, AllowDataEntry, KMImportAllowed, ClientImportAllowed, AddRemoveAllowed, IsUAD, IsCirc, IsOpenCloseLocked, UseSubGen, pt.PubTypeDisplayName, p.SortOrder, isnull(p.IsCirc, 0)  as IsCirc, HasPaidRecords, FrequencyID, YearStartDate, YearEndDate  from Pubs p  with (nolock) join pubtypes pt with (nolock) on p.PubTypeID = pt.PubTypeID join BrandDetails bd with (nolock) on p.PubID = bd.PubID join Brand b with (nolock) on b.BrandID = bd.BrandID where b.IsDeleted = 0 and bd.brandID = @brandID order by pubname asc", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@brandID", brandID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Entity.Product> builder = DynamicBuilder<Entity.Product>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Entity.Product x = builder.Build(rdr);
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

       
        public static Entity.Product Get(SqlCommand cmd)
        {
            Entity.Product retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Entity.Product();
                        DynamicBuilder<Entity.Product> builder = DynamicBuilder<Entity.Product>.CreateBuilder(rdr);
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

        public static int Save(Entity.Product x, KMPlatform.Object.ClientConnections client)
        {
            DeleteCache(client);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Product_Save";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@PubID", x.PubID);
            cmd.Parameters.AddWithValue("@PubName", x.PubName);
            cmd.Parameters.AddWithValue("@istradeshow", (object)x.istradeshow ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PubCode", (object)x.PubCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PubTypeID", (object)x.PubTypeID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GroupID", (object)x.GroupID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EnableSearching", (object)x.EnableSearching ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@score", (object)x.score ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SortOrder", (object)x.SortOrder ?? DBNull.Value);
            cmd.Parameters.Add(new SqlParameter("@DateCreated", x.DateCreated));
            cmd.Parameters.Add(new SqlParameter("@DateUpdated", (object)x.DateUpdated ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedByUserID", x.CreatedByUserID));
            cmd.Parameters.Add(new SqlParameter("@UpdatedByUserID", (object)x.UpdatedByUserID ?? DBNull.Value));
            cmd.Parameters.AddWithValue("@ClientID", (object)x.ClientID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@YearStartDate", (object)x.YearStartDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@YearEndDate", (object)x.YearEndDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IssueDate", (object)x.IssueDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsImported", (object)x.IsImported ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive", (object)x.IsActive ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AllowDataEntry", (object)x.AllowDataEntry ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FrequencyID", (object)x.FrequencyID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@KMImportAllowed", (object)x.KMImportAllowed ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ClientImportAllowed", (object)x.ClientImportAllowed ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AddRemoveAllowed", (object)x.AddRemoveAllowed ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AcsMailerInfoId", (object)x.AcsMailerInfoId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsUAD", (object)x.IsUAD ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsCirc", (object)x.IsCirc ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsOpenCloseLocked", (object)x.IsOpenCloseLocked ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@HasPaidRecords", (object)x.HasPaidRecords ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UseSubGen", (object)x.UseSubGen ?? DBNull.Value);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static bool Copy(KMPlatform.Object.ClientConnections client, int fromID, int toID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Product_Copy";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@FromPubID", fromID);
            cmd.Parameters.AddWithValue("@ToPubID", toID);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        public static bool UpdateLock(KMPlatform.Object.ClientConnections client, int userID)
        {
            DeleteCache(client);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Publication_UpdateLock";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            cmd.Parameters.AddWithValue("@UserID", userID);
            return KM.Common.DataFunctions.ExecuteNonQuery(cmd);
        }

        private static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("PUBS", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("PUBS", DatabaseName);
                }

                List<Entity.Brand> brand = Brand.Select(clientconnection);

                foreach (Entity.Brand b in brand)
                {
                    DeleteCacheByBrandID(clientconnection, b.BrandID);
                }
            }
        }
        private static void DeleteCacheByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
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

    }
}