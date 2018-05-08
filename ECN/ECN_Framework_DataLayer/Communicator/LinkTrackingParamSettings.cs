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
    public class LinkTrackingParamSettings
    {
        public static ECN_Framework_Entities.Communicator.LinkTrackingParamSettings Get_CustomerID_LTPID(int CustomerID, int LTPID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParamSettings_Select_CustomerID_LTPID";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@LTPID", LTPID);

            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.LinkTrackingParamSettings Get_BaseChannelID_LTPID(int BaseChannelID, int LTPID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParamSettings_Select_BaseChannelID_LTPID";
            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);
            cmd.Parameters.AddWithValue("@LTPID", LTPID);

            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.LinkTrackingParamSettings Get_LTPSID(int LTPSID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "";
            cmd.Parameters.AddWithValue("@LTPSID", LTPSID);

            return Get(cmd);
        }

        public static int Insert(ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParamSettings_Insert";
            cmd.Parameters.AddWithValue("@LTPID", ltps.LTPID);
            cmd.Parameters.AddWithValue("@CustomerID", (object)ltps.CustomerID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BaseChannelID", (object)ltps.BaseChannelID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DisplayName", ltps.DisplayName);
            cmd.Parameters.AddWithValue("@AllowCustom", ltps.AllowCustom);
            cmd.Parameters.AddWithValue("@IsRequired", ltps.IsRequired);
            cmd.Parameters.AddWithValue("@CreatedUserID", ltps.CreatedUserID);
            cmd.Parameters.AddWithValue("@CreatedDate", ltps.CreatedDate);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()));
        }

        public static void Update(ECN_Framework_Entities.Communicator.LinkTrackingParamSettings ltps)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParamSettings_Update";
            cmd.Parameters.AddWithValue("@LTPSID", ltps.LTPSID);
            cmd.Parameters.AddWithValue("@LTPID", ltps.LTPID);
            cmd.Parameters.AddWithValue("@CustomerID", (object)ltps.CustomerID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BaseChannelID", (object)ltps.BaseChannelID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DisplayName", ltps.DisplayName);
            cmd.Parameters.AddWithValue("@AllowCustom", ltps.AllowCustom);
            cmd.Parameters.AddWithValue("@IsRequired", ltps.IsRequired);
            cmd.Parameters.AddWithValue("@UpdatedUserID", ltps.UpdatedUserID);
            cmd.Parameters.AddWithValue("@UpdatedDate", ltps.UpdatedDate);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static ECN_Framework_Entities.Communicator.LinkTrackingParamSettings Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.LinkTrackingParamSettings lts = null;
            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    lts = new ECN_Framework_Entities.Communicator.LinkTrackingParamSettings();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LinkTrackingParamSettings>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        lts = builder.Build(rdr);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return lts;
        }

        private static List<ECN_Framework_Entities.Communicator.LinkTrackingParamSettings> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.LinkTrackingParamSettings> retList = new List<ECN_Framework_Entities.Communicator.LinkTrackingParamSettings>();
            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.LinkTrackingParamSettings lts = new ECN_Framework_Entities.Communicator.LinkTrackingParamSettings();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LinkTrackingParamSettings>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        lts = builder.Build(rdr);
                        if (lts != null)
                        {
                            retList.Add(lts);
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
    }
}
