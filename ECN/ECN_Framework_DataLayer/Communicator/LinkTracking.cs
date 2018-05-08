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
    public class LinkTracking
    {
        public static ECN_Framework_Entities.Communicator.LinkTracking GetByLTID(int LTID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTracking_Select_LTID";
            cmd.Parameters.AddWithValue("@LTID", LTID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.LinkTracking> GetByCampaignItemID(int campaignItemID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTracking_Select_CampaignItemID";
            cmd.Parameters.AddWithValue("@CampaignItemID", campaignItemID);
            return GetList(cmd);
        }

        public static bool CreateLinkTrackingParams(int customerID, string domain, int LTID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTracking_Select_Domain";
            cmd.Parameters.AddWithValue("@LTID", LTID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@Domain", domain);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Communicator.LinkTracking> GetAll()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTracking_Select_All";
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.LinkTracking> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.LinkTracking> retList = new List<ECN_Framework_Entities.Communicator.LinkTracking>();

            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
                {
                    if (rdr != null && rdr.HasRows)
                    {
                        try
                        {
                            ECN_Framework_Entities.Communicator.LinkTracking retItem = new ECN_Framework_Entities.Communicator.LinkTracking();
                            var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LinkTracking>.CreateBuilder(rdr);
                            while (rdr.Read())
                            {
                                retItem = builder.Build(rdr);
                                if (retItem != null)
                                {
                                    retList.Add(retItem);
                                }
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
            return retList;
        }

        private static ECN_Framework_Entities.Communicator.LinkTracking Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.LinkTracking retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.LinkTracking();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LinkTracking>.CreateBuilder(rdr);
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
