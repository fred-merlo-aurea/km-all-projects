using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class BlastScheduleDays
    {
        public static List<ECN_Framework_Entities.Communicator.BlastScheduleDays> GetByBlastScheduleID(int blastScheduleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastScheduleDays where BlastScheduleID = @BlastScheduleID";
            cmd.Parameters.AddWithValue("@blastScheduleID", blastScheduleID);
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.BlastScheduleDays> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.BlastScheduleDays> retList = new List<ECN_Framework_Entities.Communicator.BlastScheduleDays>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.BlastScheduleDays retItem = new ECN_Framework_Entities.Communicator.BlastScheduleDays();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastScheduleDays>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            retList.Add(retItem);
                        }
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static ECN_Framework_Entities.Communicator.BlastScheduleDays GetByBlastScheduleDaysID(int blastScheduleDaysID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from BlastScheduleDays where BlastScheduleDaysID = @BlastScheduleDaysID";
            cmd.Parameters.AddWithValue("@BlastScheduleDaysID", blastScheduleDaysID);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Communicator.BlastScheduleDays Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.BlastScheduleDays retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.BlastScheduleDays();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastScheduleDays>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retItem;
        }
    }
}
