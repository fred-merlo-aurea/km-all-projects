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
    public class RSSFeed
    {
        public static bool Exists(int RSSFeedID,string Name, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RSSFeed_Exists_Name";
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@FeedID", RSSFeedID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()) > 0 ? true : false;
        }

        public static bool UsedInContent(string rssFeedName, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RSSFeed_UsedInContent";
            cmd.Parameters.AddWithValue("@RSSFeedName", rssFeedName);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Communicator.RSSFeed GetByFeedID(int FeedID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RSSFeed_Select_ID";
            cmd.Parameters.AddWithValue("@FeedID", FeedID);

            return Get(cmd);
        }

        public static ECN_Framework_Entities.Communicator.RSSFeed GetByFeedName(string FeedName, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RSSFeed_Select_Name_CustomerID";
            cmd.Parameters.AddWithValue("@Name", FeedName);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);

            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.RSSFeed> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RSSFeed_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);

            return GetList(cmd);
        }

        public static int Save(ECN_Framework_Entities.Communicator.RSSFeed rssFeed, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RSSFeed_Save";
            if (rssFeed.FeedID > 0)
                cmd.Parameters.AddWithValue("@FeedID", rssFeed.FeedID);

            cmd.Parameters.AddWithValue("@Name", rssFeed.Name);
            cmd.Parameters.AddWithValue("@URL", rssFeed.URL);
            cmd.Parameters.AddWithValue("@CustomerID", rssFeed.CustomerID);
            cmd.Parameters.AddWithValue("@StoriesToShow", rssFeed.StoriesToShow);
            cmd.Parameters.AddWithValue("@IsDeleted", rssFeed.IsDeleted);
            cmd.Parameters.AddWithValue("@UserID", userID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());

        }

        public static void Delete(int FeedID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_RSSFeed_Delete";
            cmd.Parameters.AddWithValue("@RSSFeedID", FeedID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }


        private static ECN_Framework_Entities.Communicator.RSSFeed Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.RSSFeed retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.RSSFeed();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.RSSFeed>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.RSSFeed> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.RSSFeed> retList = new List<ECN_Framework_Entities.Communicator.RSSFeed>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.RSSFeed retItem = new ECN_Framework_Entities.Communicator.RSSFeed();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.RSSFeed>.CreateBuilder(rdr);
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
