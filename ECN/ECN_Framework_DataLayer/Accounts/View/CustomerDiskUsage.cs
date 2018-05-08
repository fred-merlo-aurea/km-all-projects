using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts.View
{
    [Serializable]
    public class CustomerDiskUsage
    {
        public static ECN_Framework_Entities.Accounts.View.CustomerDiskUsage GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "v_CustomerDiskUsage_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Accounts.View.CustomerDiskUsage Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.View.CustomerDiskUsage retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Accounts.View.CustomerDiskUsage();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.View.CustomerDiskUsage>.CreateBuilder(rdr);
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
