using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMPS.MD.Objects.Dashboard
{
    public class CumulativeGrowth
    {
        public int Month { get; set; }
        public string MonthYearLabel { get; set; }
        public int Year { get; set; }
        public int Counts { get; set; }

        public CumulativeGrowth(int month, int year, int counts)
        {
            Month = month;
            Year = year;
            Counts = counts;
            MonthYearLabel = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month).ToUpper().Substring(0,3) + ", " + year.ToString().Substring(2,2) ;
        }

        public static List<CumulativeGrowth> Get(KMPlatform.Object.ClientConnections clientconnection, string Entityname, string Type, int StartMonth, int StartYear, int EndMonth, int EndYear, int brandID)
        {
            SqlCommand cmd = new SqlCommand("Dashboard_CumulativeGrowth");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@entityName", Entityname);
            cmd.Parameters.AddWithValue("@Type", Type);
            cmd.Parameters.AddWithValue("@startmonth", StartMonth);
            cmd.Parameters.AddWithValue("@startyear", StartYear);
            cmd.Parameters.AddWithValue("@endmonth", EndMonth);
            cmd.Parameters.AddWithValue("@endyear", EndYear);
            cmd.Parameters.AddWithValue("@brandID", brandID);

            DataTable dt = DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(clientconnection));

            List<CumulativeGrowth> lcgr = new List<CumulativeGrowth>();

            foreach (DataRow dr in dt.Rows)
            {
                lcgr.Add(new CumulativeGrowth(int.Parse(dr["Month"].ToString()), int.Parse(dr["Year"].ToString()), int.Parse(dr["Counts"].ToString())));
            }

            return lcgr;
        }
    }
}

