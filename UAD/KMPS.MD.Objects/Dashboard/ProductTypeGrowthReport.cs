using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMPS.MD.Objects.Dashboard
{
    public class ProductTypeGrowthReport
    {
        public string PubtypeDisplayName { get; set; }
        public int Counts { get; set; }
        public ProductTypeGrowthReport(string producttypedisplayname, int counts)
        {
            PubtypeDisplayName = producttypedisplayname;
            Counts = counts;
        }


        public static List<ProductTypeGrowthReport> Get(KMPlatform.Object.ClientConnections clientconnection, DateTime? startdate, DateTime? enddate, int brandID)
        {
            SqlCommand cmd = new SqlCommand("Dashboard_GetProductTypeGrowth_By_Range");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@startdate", startdate);
            cmd.Parameters.AddWithValue("@enddate", enddate);
            cmd.Parameters.AddWithValue("@brandID", brandID);

            DataTable dt = DataFunctions.getDataTable(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
            List<ProductTypeGrowthReport> bg = new List<ProductTypeGrowthReport>();

            foreach (DataRow dr in dt.Rows)
            {
                bg.Add(new ProductTypeGrowthReport(dr["PubtypeDisplayName"].ToString(), int.Parse(dr["Counts"].ToString())));
            }

            return bg;
        }

    }

   
}

