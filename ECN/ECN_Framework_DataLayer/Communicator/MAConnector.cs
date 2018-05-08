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
    public class MAConnector
    {
        public static List<ECN_Framework_Entities.Communicator.MAConnector> GetByMarketingAutomationID(int MAID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MAConnector_Select_MarketingAutomationID";

            cmd.Parameters.AddWithValue("@MarketingAutomationID", MAID);
            return GetList(cmd);
        }

        public static int Save(ECN_Framework_Entities.Communicator.MAConnector mac)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MAConnector_Save";

            cmd.Parameters.AddWithValue("@MAConnectorID", mac.ConnectorID);
            cmd.Parameters.AddWithValue("@From", mac.From);
            cmd.Parameters.AddWithValue("@To", mac.To);
            cmd.Parameters.AddWithValue("@MarketingAutomationID", mac.MarketingAutomationID);
            cmd.Parameters.AddWithValue("@ControlID", mac.ControlID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int MACID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MAConnector_Delete";
            cmd.Parameters.AddWithValue("@MAConnectorID", MACID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static List<ECN_Framework_Entities.Communicator.MAConnector> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.MAConnector> retList = new List<ECN_Framework_Entities.Communicator.MAConnector>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.MAConnector retItem = new ECN_Framework_Entities.Communicator.MAConnector();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.MAConnector>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.MAConnector Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.MAConnector retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.MAConnector();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.MAConnector>.CreateBuilder(rdr);
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
