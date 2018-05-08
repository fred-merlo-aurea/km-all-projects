using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Globalization;
using KM.Common;

namespace KMPS.MD.Objects.Dashboard
{
    [Serializable]
    [DataContract]
    public class ClickOpenVisit
    {
        #region Properties       
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public int WeekPart { get; set; }
        [DataMember]
        public int Month { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public string ChartLabel { get; set; }

        [DataMember]
        public long totalOpenCounts { get; set; }
        [DataMember]
        public long totalClickCounts { get; set; }
        [DataMember]
        public long totalVisitCounts { get; set; }
      
        #endregion

        public ClickOpenVisit()
        {
        }

        #region Data
        public static List<ClickOpenVisit> GetMonthlySummary(KMPlatform.Object.ClientConnections clientconnection, int StartMonth, int StartYear, int EndMonth, int EndYear, int BrandID)
        {
                return GetMonthlySummaryData(clientconnection, StartMonth,  StartYear,  EndMonth,  EndYear, BrandID);
        }

        private static List<ClickOpenVisit> GetMonthlySummaryData(KMPlatform.Object.ClientConnections clientconnection,  int StartMonth, int StartYear, int EndMonth, int EndYear, int BrandID)
        {
            ClickOpenVisit co = null;
            List<ClickOpenVisit> coList = new List<ClickOpenVisit>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("Dashboard_OpensClicksVisits_By_Month", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            cmd.Parameters.AddWithValue("@startmonth", StartMonth);
            cmd.Parameters.AddWithValue("@startyear", StartYear);
            cmd.Parameters.AddWithValue("@endmonth", EndMonth);
            cmd.Parameters.AddWithValue("@endyear", EndYear);
            cmd.Parameters.AddWithValue("@BrandID", BrandID);

            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<ClickOpenVisit> builder = DynamicBuilder<ClickOpenVisit>.CreateBuilder(rdr);
                
                while (rdr.Read())
                {
                    co = new ClickOpenVisit();                    
                    co = builder.Build(rdr);
                    co.ChartLabel = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(co.Month).ToUpper().Substring(0, 3) + ", " + co.Year.ToString().Substring(2, 2);
                    coList.Add(co);
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

            return coList;
        }

        public static List<ClickOpenVisit> GetWeeklySummary(KMPlatform.Object.ClientConnections clientconnection, int Month, int Year, int BrandID)
        {
            return GetWeeklySummaryData(clientconnection, Month, Year, BrandID);
        }

        private static List<ClickOpenVisit> GetWeeklySummaryData(KMPlatform.Object.ClientConnections clientconnection, int Month, int Year, int BrandID)
        {
            ClickOpenVisit co = null;
            List<ClickOpenVisit> coList = new List<ClickOpenVisit>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("Dashboard_OpensClicksVisits_By_Week", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            cmd.Parameters.AddWithValue("@month", Month);
            cmd.Parameters.AddWithValue("@year", Year);
            cmd.Parameters.AddWithValue("@BrandID", BrandID);

            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<ClickOpenVisit> builder = DynamicBuilder<ClickOpenVisit>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    co = new ClickOpenVisit();
                    co = builder.Build(rdr);
                    co.ChartLabel = co.WeekPart.ToString() + ", " + co.Year;
                    coList.Add(co);
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

            return coList;
        }

        public static List<ClickOpenVisit> GetDailySummary(KMPlatform.Object.ClientConnections clientconnection, DateTime? startdate, DateTime? enddate, int BrandID)
        {
            return GetDailyData(clientconnection, startdate, enddate, BrandID);
        }

        private static List<ClickOpenVisit> GetDailyData(KMPlatform.Object.ClientConnections clientconnection, DateTime? startdate, DateTime? enddate, int BrandID)
        {
            ClickOpenVisit co = null;
            List<ClickOpenVisit> coList = new List<ClickOpenVisit>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("Dashboard_OpensClicksVisits_By_DateRange", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            cmd.Parameters.AddWithValue("@startdate", startdate);
            cmd.Parameters.AddWithValue("@enddate", enddate);
            cmd.Parameters.AddWithValue("@BrandID", BrandID);

            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<ClickOpenVisit> builder = DynamicBuilder<ClickOpenVisit>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    co = new ClickOpenVisit();
                    co = builder.Build(rdr);
                    co.ChartLabel = co.Date.ToString("MM/dd/yyyy");
                    coList.Add(co);
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

            return coList;
        }

        #endregion
    }
}