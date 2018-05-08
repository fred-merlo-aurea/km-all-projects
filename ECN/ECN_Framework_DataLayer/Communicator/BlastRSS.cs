using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    public class BlastRSS
    {
        public static int Save(ECN_Framework_Entities.Communicator.BlastRSS rss)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastRSS_Save";

            cmd.Parameters.AddWithValue("@BlastID", rss.BlastID);
            cmd.Parameters.AddWithValue("@Name", rss.Name);
            cmd.Parameters.AddWithValue("@FeedHTML", rss.FeedHTML);
            cmd.Parameters.AddWithValue("@FeedTEXT", rss.FeedTEXT);
            cmd.Parameters.AddWithValue("@FeedID", rss.FeedID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static List<ECN_Framework_Entities.Communicator.BlastRSS> GetByBlastID(int BlastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastRSS_Select_BlastID";

            cmd.Parameters.AddWithValue("@BlastID", BlastID);

            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Communicator.BlastRSS GetByBlastID_FeedID(int BlastID, int FeedID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastRSS_Select_BlastID_FeedID";
            cmd.Parameters.AddWithValue("@BlastID", BlastID);
            cmd.Parameters.AddWithValue("@FeedID", FeedID);

            return Get(cmd);
        }

        public static bool ExistsByBlastID(int BlastID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastRSS_Exists_BlastID";
            cmd.Parameters.AddWithValue("@BlastID", BlastID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()) > 0 ? true : false;
        }

        public static bool ExistsByBlastID_FeedID(int BlastID, int FeedID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_BlastRSS_Exists_BlastID_FeedID";
            cmd.Parameters.AddWithValue("@BlastID", BlastID);
            cmd.Parameters.AddWithValue("@FeedID", FeedID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()) > 0 ? true : false;
        }

        private static List<ECN_Framework_Entities.Communicator.BlastRSS> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.BlastRSS> retList = new List<ECN_Framework_Entities.Communicator.BlastRSS>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.BlastRSS retItem = new ECN_Framework_Entities.Communicator.BlastRSS();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastRSS>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.BlastRSS Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.BlastRSS retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.BlastRSS();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.BlastRSS>.CreateBuilder(rdr);
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
