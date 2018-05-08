using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KM.Common;

namespace FrameworkUAD.DataAccess
{
    public class IssueSplitArchivePubSubscriptionMap
    {
        public static List<Entity.IssueSplitArchivePubSubscriptionMap> SelectIssueSplitPubsMapping(int issueSplitID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueSplitArchivePubSubscriptionMap> retItem = null;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueSplitArchivePubSubscriptionMap_Select_IssueSplitID";
            cmd.Parameters.AddWithValue("@IssueSplitID", issueSplitID);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            retItem = GetList(cmd);
            return retItem;
        }
        public static int MoveSplitRecords(int ToIssueSplitID,int FromIssueSplitID,int MovedRecordCount, DataTable dtIssueSplitPubs, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_IssueSplit_MoveRecords";
            cmd.Parameters.AddWithValue("@ToIssueSplitId", ToIssueSplitID);
            cmd.Parameters.AddWithValue("@FromIssueSplitId", FromIssueSplitID);
            cmd.Parameters.AddWithValue("@RecordMovedCount", MovedRecordCount);
            cmd.Parameters.AddWithValue("@TVP_IssueSplitIds", dtIssueSplitPubs);
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));

        }
      
        private static List<Entity.IssueSplitArchivePubSubscriptionMap> GetList(SqlCommand cmd)
        {
            List<Entity.IssueSplitArchivePubSubscriptionMap> retList = new List<Entity.IssueSplitArchivePubSubscriptionMap>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Entity.IssueSplitArchivePubSubscriptionMap retItem = new Entity.IssueSplitArchivePubSubscriptionMap();
                        DynamicBuilder<Entity.IssueSplitArchivePubSubscriptionMap> builder = DynamicBuilder<Entity.IssueSplitArchivePubSubscriptionMap>.CreateBuilder(rdr);
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
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }

    }
}
