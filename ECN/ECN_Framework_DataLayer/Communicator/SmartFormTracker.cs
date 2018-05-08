using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class SmartFormTracker
    {

       
        public static void Insert(ECN_Framework_Entities.Communicator.SmartFormTracker sft)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_SmartFormTracking_Insert";
            cmd.Parameters.Add(new SqlParameter("@BlastID", sft.BlastID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", sft.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@SmartFormID", sft.SFID));
            cmd.Parameters.Add(new SqlParameter("@GroupID", sft.GroupID));
            cmd.Parameters.Add(new SqlParameter("@ReferringURL", sft.ReferringUrl));
            cmd.Parameters.Add(new SqlParameter("@ActivityDate", sft.ActivityDate));

             DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

    }
}
