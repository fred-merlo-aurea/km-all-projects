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
    public class FilterPenetrationReports
    {
        public FilterPenetrationReports() { }

        #region Properties
        [DataMember]
        public int ReportID { get; set; }
        [DataMember]
        public string ReportName  { get; set; }
        [DataMember]
        public int CreatedUserID { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int  UpdatedUserID { get; set; }
        [DataMember]
        public DateTime UpdatedDate { get; set; }
        [DataMember]
        public int? BrandID { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        #endregion

        #region Data
        public static List<FilterPenetrationReports> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                List<FilterPenetrationReports> filterpenetrationreports = (List<FilterPenetrationReports>)CacheUtil.GetFromCache("FILTERPENETRATIONREPORTS", DatabaseName);

                if (filterpenetrationreports == null)
                {
                    filterpenetrationreports = GetData(clientconnection);

                    CacheUtil.AddToCache("FILTERPENETRATIONREPORTS", filterpenetrationreports, DatabaseName);
                }

                return filterpenetrationreports;
            }
            else
            {
                return GetData(clientconnection);
            }

        }

        public static List<FilterPenetrationReports> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<FilterPenetrationReports> retList = new List<FilterPenetrationReports>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from FilterPenetrationReports where IsDeleted = 0 order by ReportName asc", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(); 
                DynamicBuilder<FilterPenetrationReports> builder = DynamicBuilder<FilterPenetrationReports>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    FilterPenetrationReports x = builder.Build(rdr);
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

        public static List<FilterPenetrationReports> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == brandID);
        }

        public static List<FilterPenetrationReports> GetNotInBrand(KMPlatform.Object.ClientConnections clientconnection)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == 0 || x.BrandID == null);
        }

        public static FilterPenetrationReports GetByID(KMPlatform.Object.ClientConnections clientconnection, int reportID)
        {
            return GetAll(clientconnection).Find(x => x.ReportID == reportID);
        }

        public static bool ExistsByReportName(KMPlatform.Object.ClientConnections clientconnection, string reportName)
        {
            SqlCommand cmd = new SqlCommand("Select * from FilterPenetrationReports where IsDeleted = 0 and ReportName = @reportName");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@reportName", reportName));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static void DeleteCache(KMPlatform.Object.ClientConnections clientconnection)
        {
            if (CacheUtil.IsCacheEnabled())
            {
                string DatabaseName = DataFunctions.GetDBName(clientconnection);

                if (CacheUtil.GetFromCache("FILTERPENETRATIONREPORTS", DatabaseName) != null)
                {
                    CacheUtil.RemoveFromCache("FILTERPENETRATIONREPORTS", DatabaseName);
                }
            }
        }
        #endregion

        #region CRUD
        public static int Save(KMPlatform.Object.ClientConnections clientconnection, FilterPenetrationReports fpr)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterPenetrationReports_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ReportName", fpr.ReportName);
            cmd.Parameters.AddWithValue("@BrandID", fpr.BrandID);
            cmd.Parameters.AddWithValue("@UserID", fpr.CreatedUserID);
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int ReportID)
        {
            DeleteCache(clientconnection);
            SqlCommand cmd = new SqlCommand("e_FilterPenetrationReports_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ReportID", ReportID);
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}
