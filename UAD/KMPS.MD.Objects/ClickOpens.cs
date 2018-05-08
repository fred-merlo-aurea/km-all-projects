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
    public class ClickOpens
    {
        #region Properties       
        [DataMember]
        public int monthdt { get; set; }
        [DataMember]
        public int yeardt { get; set; }
        [DataMember]
        public int weekdt { get; set; }
        [DataMember]
        public long totalOpenCounts { get; set; }
        [DataMember]
        public long totalClickCounts { get; set; }
        [DataMember]
        public long totalVisitCounts { get; set; }
        [DataMember]
        public string Month { get; set; }
        [DataMember]
        public string Week { get; set; }
        [DataMember]
        public DateTime MonthFirstDate { get; set; }
        #endregion

        public ClickOpens()
        {
        }

        #region Data
        public static List<ClickOpens> Get(KMPlatform.Object.ClientConnections clientconnection)
        {
            return GetData(clientconnection);
        }

        private static List<ClickOpens> GetData(KMPlatform.Object.ClientConnections clientconnection)
        {
            ClickOpens co = null;
            List<ClickOpens> coList = new List<ClickOpens>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("Dashboard_OpensClicksVisits_By_Range", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<ClickOpens> builder = DynamicBuilder<ClickOpens>.CreateBuilder(rdr);
                
                while (rdr.Read())
                {
                    co = new ClickOpens();                    
                    co = builder.Build(rdr);
                    co.Week = "Week " + rdr[2].ToString() + " " + rdr[1].ToString();
                    co.Month = DateTimeFormatInfo.CurrentInfo.GetMonthName(Int32.Parse(rdr[0].ToString())) + " " + rdr[1].ToString();
                    co.MonthFirstDate = new DateTime( Int32.Parse(rdr[1].ToString()), Int32.Parse(rdr[0].ToString()), 1);
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