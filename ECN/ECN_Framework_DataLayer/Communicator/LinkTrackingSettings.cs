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
    public class LinkTrackingSettings
    {
        public static ECN_Framework_Entities.Communicator.LinkTrackingSettings Get_CustomerID_LTID(int CustomerID,int LTID)
        {
            LinkTrackingSettings lts = new LinkTrackingSettings();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingSettings_Select_CustomerIDLTID";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@LTID", LTID);

            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.LinkTrackingSettings Get_BaseChannelID_LTID(int BaseChannelID, int LTID)
        {
            LinkTrackingSettings lts = new LinkTrackingSettings();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingSettings_Select_BaseChannelIDLTID";
            cmd.Parameters.AddWithValue("@BaseChannelID", BaseChannelID);
            cmd.Parameters.AddWithValue("@LTID", LTID);

            return Get(cmd);
        }

        public static int Insert(ECN_Framework_Entities.Communicator.LinkTrackingSettings lts)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingSettings_Insert";
            cmd.Parameters.AddWithValue("@LTID", lts.LTID);
            cmd.Parameters.AddWithValue("@CustomerID", (object)lts.CustomerID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BaseChannelID", (object)lts.BaseChannelID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@XMLConfig", lts.XMLConfig);
            cmd.Parameters.AddWithValue("@CreatedUserID", lts.CreatedUserID);
            cmd.Parameters.AddWithValue("@CreatedDate", lts.CreatedDate);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()));
        }

        public static void Update(ECN_Framework_Entities.Communicator.LinkTrackingSettings lts)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingSettings_Update";
            cmd.Parameters.AddWithValue("@LTSID", lts.LTSID);
            cmd.Parameters.AddWithValue("@LTID", lts.LTID);
            cmd.Parameters.AddWithValue("@CustomerID", (object)lts.CustomerID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BaseChannelID", (object)lts.BaseChannelID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@XMLConfig", lts.XMLConfig);
            cmd.Parameters.AddWithValue("@UpdatedUserID", lts.UpdatedUserID);
            cmd.Parameters.AddWithValue("@UpdatedDate", lts.UpdatedDate);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void UpdateCustomerOmnitureOverride(int baseChannelID, bool allowOverride, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingSettings_UpdateCustomerOmnitureOverride";
           
            cmd.Parameters.AddWithValue("@BaseChannelID", baseChannelID);
            cmd.Parameters.AddWithValue("@AllowOverride", allowOverride);
            cmd.Parameters.AddWithValue("@UserID", UserID);
           

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static ECN_Framework_Entities.Communicator.LinkTrackingSettings Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.LinkTrackingSettings lts = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
                {
                    if (rdr != null && rdr.HasRows)
                    {
                        try
                        {
                            lts = new ECN_Framework_Entities.Communicator.LinkTrackingSettings();
                            var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LinkTrackingSettings>.CreateBuilder(rdr);
                            while (rdr.Read())
                            {
                                lts = builder.Build(rdr);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            if (rdr != null)
                            {
                                rdr.Close();
                                rdr.Dispose();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
            }
            return lts;
        }
    }
}
