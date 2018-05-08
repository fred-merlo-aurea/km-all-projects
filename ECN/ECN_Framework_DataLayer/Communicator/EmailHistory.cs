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
    public class EmailHistory
    {
        public static int FindMergedEmailID(int OldEmailID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_EmailHistory_Search_OldEmailID";

            cmd.Parameters.AddWithValue("@OldEmailID", OldEmailID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd,DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        private static ECN_Framework_Entities.Communicator.EmailHistory Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.EmailHistory retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.EmailHistory();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.EmailHistory>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.EmailHistory> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.EmailHistory> retList = new List<ECN_Framework_Entities.Communicator.EmailHistory>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.EmailHistory retItem = new ECN_Framework_Entities.Communicator.EmailHistory();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.EmailHistory>.CreateBuilder(rdr);
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
    }
}
