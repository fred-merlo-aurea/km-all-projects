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
    public class MarketingAutomationHistory
    {
        public static List<ECN_Framework_Entities.Communicator.MarketingAutomationHistory> SelectByMarketingAutomationID(int MAID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MarketingAutomationHistory_Select_MAID";

            cmd.Parameters.AddWithValue("@MAID", MAID);
            return GetList(cmd);
        }

        public static void Insert(int MAID, int UserID, string Action)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_MarketingAutomationHistory_Insert";
            cmd.Parameters.AddWithValue("@MAID", MAID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@Action", Action);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static List<ECN_Framework_Entities.Communicator.MarketingAutomationHistory> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.MarketingAutomationHistory> retList = new List<ECN_Framework_Entities.Communicator.MarketingAutomationHistory>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.MarketingAutomationHistory retItem = new ECN_Framework_Entities.Communicator.MarketingAutomationHistory();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.MarketingAutomationHistory>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.MarketingAutomationHistory Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.MarketingAutomationHistory retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.MarketingAutomationHistory();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.MarketingAutomationHistory>.CreateBuilder(rdr);
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
