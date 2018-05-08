using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMPS.MD.Objects.Dashboard
{
    public class BrandGrowthReport
    {
        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public int Counts { get; set; }
        public decimal CountsPercentage { get; set; }
        public BrandGrowthReport(int brandID, string brandname, int counts, decimal countspercentage)
        {
            BrandName = brandname;
            Counts = counts;
            BrandID = brandID;
            CountsPercentage = countspercentage;
        }


        public static List<BrandGrowthReport> Get(KMPlatform.Object.ClientConnections clientconnection, DateTime? startdate, DateTime? enddate)
        {
            SqlCommand cmd = new SqlCommand("Dashboard_GetBrandGrowth_By_Range");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@startdate", startdate);
            cmd.Parameters.AddWithValue("@enddate", enddate);

            DataTable dt = DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
            List<BrandGrowthReport> bg = new List<BrandGrowthReport>();

            foreach (DataRow dr in dt.Rows)
            {
                bg.Add(new BrandGrowthReport(int.Parse(dr["brandID"].ToString()), dr["brandname"].ToString(), int.Parse(dr["Counts"].ToString()),  decimal.Parse(dr["CountsPercentage"].ToString())));
            }

            return bg;
        }

    }

   
}

