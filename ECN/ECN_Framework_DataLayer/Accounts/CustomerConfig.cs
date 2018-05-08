using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    [Serializable]
    public class CustomerConfig
    {

        public static bool Exists(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerConfig_Exists_ByCustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Accounts.CustomerConfig> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerConfig_Select_CustomerID";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Accounts.CustomerConfig> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.CustomerConfig> retList = new List<ECN_Framework_Entities.Accounts.CustomerConfig>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.CustomerConfig retItem = new ECN_Framework_Entities.Accounts.CustomerConfig();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.CustomerConfig>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        if (retItem != null)
                        {
                            retItem.ConfigNameID = GetConfigName(retItem.ConfigName);

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

        private static ECN_Framework_Common.Objects.Accounts.Enums.ConfigName GetConfigName(string ConfigName)
        {
            return (ECN_Framework_Common.Objects.Accounts.Enums.ConfigName)Enum.Parse(typeof(ECN_Framework_Common.Objects.Accounts.Enums.ConfigName), ConfigName);
        }

        public static int Save(ECN_Framework_Entities.Accounts.CustomerConfig customerconfig)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerConfig_Save";
            cmd.Parameters.Add(new SqlParameter("@CustomerConfigID", customerconfig.CustomerConfigID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerconfig.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@ProductID", customerconfig.ProductID));
            cmd.Parameters.Add(new SqlParameter("@ConfigName", customerconfig.ConfigName));
            cmd.Parameters.Add(new SqlParameter("@ConfigValue", customerconfig.ConfigValue));
            if (customerconfig.CustomerConfigID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)customerconfig.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)customerconfig.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString()));
        }

        public static void Delete(int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_CustomerConfig_Delete_ByCustomerID";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }

    }
}
