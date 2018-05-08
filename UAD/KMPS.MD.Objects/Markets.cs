using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class Markets
    {
        public Markets() { }

        #region Properties
        [DataMember]
        public int MarketID { get; set; }
        [DataMember]
        public string MarketName  { get; set; }
        [DataMember]
        public string MarketXML { get; set; }
        [DataMember]
        public int? BrandID { get; set; }
        [DataMember]
        public string  BrandName { get; set; }
        #endregion

        #region Data
        public static List<Markets> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<Markets> markets = (List<Markets>)CacheUtil.GetFromCache("MARKETS", DatabaseName);

                if (markets == null)
                {
                    markets = GetData(clientconnection);

                    CacheUtil.AddToCache("MARKETS", markets, DatabaseName);
                }

                return markets;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        public static List<Markets> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<Markets> retList = new List<Markets>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("Select m.*, b.brandID, b.BrandName from Markets m left join brand b on m.brandID = b.brandID where isnull(b.IsDeleted,0) = 0 order by Marketname asc", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Markets> builder = DynamicBuilder<Markets>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Markets x = builder.Build(rdr);
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

        public static Markets GetByID(KMPlatform.Object.ClientConnections clientconnection, int marketID)
        {
            return GetAll(clientconnection).Find(x => x.MarketID == marketID);
        }

        public static List<Markets> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == brandID);
        }

        public static List<Markets> GetNotInBrand(KMPlatform.Object.ClientConnections clientconnection)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == 0 || x.BrandID == null);
        }

        public static List<Markets> GetInBrandByUserID(KMPlatform.Object.ClientConnections clientconnection, int UserID)
        {
            List<Markets> retList = new List<Markets>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select m.*, b.BrandName from userbrand ub with (nolock) join  Brand b with (nolock) on ub.BrandID=b.BrandID join Markets m with (nolock) on m.BrandID = b.BrandID  where b.IsDeleted = 0 and userID = @UserID order by Marketname asc", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
            cmd.CommandTimeout = 0;
            try
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<Markets> builder = DynamicBuilder<Markets>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    Markets x = builder.Build(rdr);
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

        public static bool ExistsByName(KMPlatform.Object.ClientConnections clientconnection, int marketID, string marketName)
        {
            SqlCommand cmd = new SqlCommand("SELECT * from Markets with (nolock) where MarketName = @MarketName and MarketID <> @MarketID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@MarketName", marketName));
            cmd.Parameters.Add(new SqlParameter("@MarketID", marketID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("MARKETS", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("MARKETS", DatabaseName);
                }
            }
        }
        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, Markets m)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_SaveMarketsWithXML");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MarketID", SqlDbType.Int)).Value = m.MarketID;
            cmd.Parameters.Add(new SqlParameter("@MarketName", SqlDbType.VarChar)).Value = m.MarketName;
            cmd.Parameters.Add(new SqlParameter("@CurrentXML", SqlDbType.Xml)).Value = m.MarketXML;
            cmd.Parameters.Add(new SqlParameter("@BrandID", SqlDbType.Int)).Value = m.BrandID;
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }
        #endregion
   }
}
