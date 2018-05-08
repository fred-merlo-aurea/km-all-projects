using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Globalization;
using KM.Common;

namespace KMPS.MD.Objects
{
    [Serializable]
    [DataContract]
    public class BrandNewSubscriberTrend
    {
        #region Properties       
        [DataMember]
        public string BrandName { get; set; }
        public int monthdt { get; set; }
        [DataMember]
        public int yeardt { get; set; }
        [DataMember]
        public int weekdt { get; set; }
        [DataMember]
        public long Counts { get; set; }
        [DataMember]
        public string Month { get; set; }
        [DataMember]
        public string Week { get; set; }
        [DataMember]
        public DateTime MonthFirstDate { get; set; }
        #endregion

        public BrandNewSubscriberTrend()
        {
        }

        #region Data
        public static List<BrandNewSubscriberTrend> Get(KMPlatform.Object.ClientConnections clientconnection, DateTime? startdate, DateTime? enddate)
        {
                return GetData(clientconnection, startdate, enddate);
        }

        private static List<BrandNewSubscriberTrend> GetData(KMPlatform.Object.ClientConnections clientconnection, DateTime? startdate, DateTime? enddate)
        {
            BrandNewSubscriberTrend co = null;
            List<BrandNewSubscriberTrend> coList = new List<BrandNewSubscriberTrend>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("Dashboard_BrandNewSubscriberTrend_By_Range", conn);

            cmd.Parameters.AddWithValue("@startdate", startdate);
            cmd.Parameters.AddWithValue("@enddate", enddate);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<BrandNewSubscriberTrend> builder = DynamicBuilder<BrandNewSubscriberTrend>.CreateBuilder(rdr);

                while (rdr.Read())
                {
                    co = new BrandNewSubscriberTrend();
                    co = builder.Build(rdr);
                    co.Week = "Week " + rdr["weekdt"].ToString() + " " + rdr["yeardt"].ToString();
                    co.Month = DateTimeFormatInfo.CurrentInfo.GetMonthName(Int32.Parse(rdr["monthdt"].ToString())) + " " + rdr["yeardt"].ToString();
                    co.MonthFirstDate = new DateTime(Int32.Parse(rdr["yeardt"].ToString()), Int32.Parse(rdr["monthdt"].ToString()), 1);
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

        private static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);

            int daysOffset = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;

            DateTime firstMonday = jan1.AddDays(daysOffset);

            int firstWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(jan1, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);

            if (firstWeek <= 1)
            {
                weekOfYear -= 1;
            }

            return firstMonday.AddDays(weekOfYear * 7);
        }
        #endregion
    }
}