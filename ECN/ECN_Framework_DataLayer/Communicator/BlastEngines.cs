using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class BlastEngines
    {
        public static DataTable GetBlastEngineStatus()
        {
            SqlCommand cmd = new SqlCommand("rpt_BlastEngine_Status");
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = null;
            dt = DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
            return dt;
        }
    }
}
