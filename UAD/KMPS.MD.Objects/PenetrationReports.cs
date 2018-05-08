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
    public class PenetrationReports
    {
        public PenetrationReports() { }

        #region Properties
        [DataMember]
        public int ReportID { get; set; }
        [DataMember]
        public string ReportName  { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public DateTime CreateddtStamp { get; set; }
        [DataMember]
        public int  UpdatedBy { get; set; }
        [DataMember]
        public DateTime UpdateddtStamp { get; set; }
        [DataMember]
        public int? BrandID { get; set; }
        #endregion

        #region Data
        public static List<PenetrationReports> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<PenetrationReports> penetrationreports = (List<PenetrationReports>)CacheUtil.GetFromCache("PENETRATIONREPORTS", DatabaseName);

                if (penetrationreports == null)
                {
                    penetrationreports = GetData(clientconnection);

                    CacheUtil.AddToCache("PENETRATIONREPORTS", penetrationreports, DatabaseName);
                }

                return penetrationreports;
            }
            else
            {
                return GetData(clientconnection);
            }
        }

        public static List<PenetrationReports> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<PenetrationReports> retList = new List<PenetrationReports>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from PenetrationReports order by ReportName asc", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<PenetrationReports> builder = DynamicBuilder<PenetrationReports>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    PenetrationReports x = builder.Build(rdr);
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

        public static List<PenetrationReports> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == brandID);
        }

        public static List<PenetrationReports> GetNotInBrand(KMPlatform.Object.ClientConnections clientconnection)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == 0 || x.BrandID == null);
        }

        public static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("PENETRATIONREPORTS", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("PENETRATIONREPORTS", DatabaseName);
                }
            }
        }
        #endregion

        #region CRUD
        public static void SaveReport(KMPlatform.Object.ClientConnections clientconnection, int user, string marketID, string report_name, int brandID)
        {
            DeleteCache(clientconnection);
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_SaveMarketPenetrationReport", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MarketID", marketID);
            cmd.Parameters.AddWithValue("@report_name", report_name);
            cmd.Parameters.AddWithValue("@user", user);
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
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

        public static void DeleteReport(KMPlatform.Object.ClientConnections clientconnection, int ReportID)
        {
            DeleteCache(clientconnection);
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("sp_DelMarketPenetrationReport", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ReportID", ReportID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
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
        #endregion
    }
}
