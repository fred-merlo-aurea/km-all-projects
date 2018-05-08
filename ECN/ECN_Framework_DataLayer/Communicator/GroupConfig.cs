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
    public class GroupConfig
    {
        public static List<ECN_Framework_Entities.Communicator.GroupConfig> GetByCustomerID(int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GroupConfig_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return GetList(cmd);
        }

        public static int Save(ECN_Framework_Entities.Communicator.GroupConfig GroupConfig, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GroupConfig_Save";
            cmd.Parameters.Add(new SqlParameter("@GroupConfigID", GroupConfig.GroupConfigID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", GroupConfig.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@ShortName", GroupConfig.ShortName));
            cmd.Parameters.Add(new SqlParameter("@IsPublic", GroupConfig.IsPublic));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int GroupConfigID, int CustomerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_GroupConfig_Delete";
            cmd.Parameters.Add(new SqlParameter("@GroupConfigID", GroupConfigID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", CustomerID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        private static List<ECN_Framework_Entities.Communicator.GroupConfig> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.GroupConfig> retList = new List<ECN_Framework_Entities.Communicator.GroupConfig>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.GroupConfig retItem = new ECN_Framework_Entities.Communicator.GroupConfig();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.GroupConfig>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.GroupConfig Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.GroupConfig retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.GroupConfig();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.GroupConfig>.CreateBuilder(rdr);
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
