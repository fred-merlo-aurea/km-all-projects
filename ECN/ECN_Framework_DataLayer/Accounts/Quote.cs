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
    public class Quote
    {
        public static bool Exists(int quoteID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Quote_Exists_ByQuoteID";
            cmd.Parameters.Add(new SqlParameter("@QuoteID", quoteID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Accounts.Quote> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Quote_Select_CustomerID";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Accounts.Quote GetByQuoteID(int quoteID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Quote_Select_QuoteID";
            cmd.Parameters.Add(new SqlParameter("@QuoteID", quoteID));
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Accounts.Quote Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Accounts.Quote retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Accounts.Quote();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.Quote>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Accounts.Quote> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.Quote> retList = new List<ECN_Framework_Entities.Accounts.Quote>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.Quote retItem = new ECN_Framework_Entities.Accounts.Quote();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.Quote>.CreateBuilder(rdr);
                    while (rdr.Read())
                    {
                        retItem = builder.Build(rdr);
                        retList.Add(retItem);
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
            }
            cmd.Connection.Close();
            cmd.Dispose();
            return retList;
        }

        public static void Delete(int QuoteID, int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Quote_Delete";
            cmd.Parameters.Add(new SqlParameter("@QuoteID", QuoteID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }
    }
}
