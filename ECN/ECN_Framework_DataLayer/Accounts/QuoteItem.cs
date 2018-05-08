using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using KM.Common;

namespace ECN_Framework_DataLayer.Accounts
{
    [Serializable]
    public class QuoteItem
    {
        public static bool Exists(int quoteID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_QuoteItem_Exists_ByQuoteID";
            cmd.Parameters.Add(new SqlParameter("@QuoteID", quoteID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Accounts.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Accounts.QuoteItem> GetByQuoteID(int quoteID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_QuoteItem_Select_QuoteID";
            cmd.Parameters.Add(new SqlParameter("@QuoteID", quoteID));
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Accounts.QuoteItem> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Accounts.QuoteItem> retList = new List<ECN_Framework_Entities.Accounts.QuoteItem>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Accounts.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Accounts.QuoteItem retItem = new ECN_Framework_Entities.Accounts.QuoteItem();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Accounts.QuoteItem>.CreateBuilder(rdr);
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

        public static void Delete(int quoteID, int customerID, int userID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_QuoteItem_Delete_All";
            cmd.Parameters.AddWithValue("@QuoteID", quoteID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Accounts.ToString());
        }
    }
}
