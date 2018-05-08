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
    public class CustomerLinkTracking
    {
        public static ECN_Framework_Entities.Communicator.CustomerLinkTracking GetByCLTID(int CLTID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerLinkTracking_Select_CLTID";
            cmd.Parameters.AddWithValue("@CLTID", CLTID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.CustomerLinkTracking> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerLinkTracking_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.CustomerLinkTracking> GetByLTID(int LTID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerLinkTracking_Select_LTID";
            cmd.Parameters.AddWithValue("@LTID", LTID);
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.CustomerLinkTracking> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.CustomerLinkTracking> retList = new List<ECN_Framework_Entities.Communicator.CustomerLinkTracking>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.CustomerLinkTracking retItem = new ECN_Framework_Entities.Communicator.CustomerLinkTracking();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CustomerLinkTracking>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.CustomerLinkTracking Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.CustomerLinkTracking retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.CustomerLinkTracking();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.CustomerLinkTracking>.CreateBuilder(rdr);
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
