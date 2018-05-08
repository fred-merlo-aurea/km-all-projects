using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECN_Framework_DataLayer.Activity.Report
{
    [Serializable]
    public class TopEvangelists
    {
        public static DataTable Get(int campaignItemID)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "rpt_TopEvangelists"
            };
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Activity.ToString());
        }
    }
}
