using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    [DataContract]
    public class SmartFormTracking
    {
        public static void Insert(ECN_Framework_Entities.Communicator.SmartFormTracking sft)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SmartFormTracking_Insert";
            if(sft.BlastID != null)
                cmd.Parameters.Add(new SqlParameter("@BlastID", sft.BlastID));
            if (sft.CustomerID != null)
                cmd.Parameters.Add(new SqlParameter("@CustomerID", sft.CustomerID));
            if (sft.SFID != null)
                cmd.Parameters.Add(new SqlParameter("@SmartFormID", sft.SFID));
            if (sft.GroupID != null)
                cmd.Parameters.Add(new SqlParameter("@GroupID", sft.GroupID));
            cmd.Parameters.Add(new SqlParameter("@ReferringURL", sft.ReferringUrl));

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString()); 
        }

    }
}
