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
    public class LinkTrackingParam
    {
        public static ECN_Framework_Entities.Communicator.LinkTrackingParam GetByLTPID(int LTPID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParam_Select_LTPID";
            cmd.Parameters.AddWithValue("@LTPID", LTPID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.LinkTrackingParam> GetByLTID(int LTID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingParam_Select_LTID";
            cmd.Parameters.AddWithValue("@LTID", LTID);
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.LinkTrackingParam> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.LinkTrackingParam> retList = new List<ECN_Framework_Entities.Communicator.LinkTrackingParam>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.LinkTrackingParam retItem = new ECN_Framework_Entities.Communicator.LinkTrackingParam();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LinkTrackingParam>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.LinkTrackingParam Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.LinkTrackingParam retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.LinkTrackingParam();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LinkTrackingParam>.CreateBuilder(rdr);
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
