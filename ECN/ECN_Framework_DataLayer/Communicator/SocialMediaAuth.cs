using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    [DataContract]
    public class SocialMediaAuth
    {
        /// <summary>
        /// Social Media Ids: 1-Facebook, 2-Twitter, 3-LinkedIn
        /// 
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="SocialMediaID"></param>
        /// <returns></returns>
        public static List<ECN_Framework_Entities.Communicator.SocialMediaAuth> GetByCustomer_SocialMediaID(int CustomerID, int SocialMediaID)
        {
            ECN_Framework_Entities.Communicator.SocialMediaAuth sma = new ECN_Framework_Entities.Communicator.SocialMediaAuth();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SocialMediaAuth_Select_CustomerID_SocialMediaID";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@SocialMediaID", SocialMediaID);
            return GetList(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.SocialMediaAuth> GetByCustomerID(int CustomerID)
        {
            List<ECN_Framework_Entities.Communicator.SocialMediaAuth> retList = new List<ECN_Framework_Entities.Communicator.SocialMediaAuth>();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SocialMediaAuth_Select_CustomerID";

            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);

            return GetList(cmd);
        }

        public static bool UsedInBlasts(int SocialMediaAuthID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SocialMediaAuth_UsedInBlasts";
            cmd.Parameters.AddWithValue("@SocialMediaAuthID", SocialMediaAuthID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()) > 0 ? true : false;
        }

        public static ECN_Framework_Entities.Communicator.SocialMediaAuth GetBySocialMediaAuthID(int authID)
        {
            ECN_Framework_Entities.Communicator.SocialMediaAuth sma = new ECN_Framework_Entities.Communicator.SocialMediaAuth();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SocialMediaAuth_Select_SocialMediaAuthID";

            cmd.Parameters.AddWithValue("@SocialMediaAuthID", authID);

            return Get(cmd);
        }

        public static bool UserIDExists(string userID, int customerID, int socialMediaID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SocialMediaAuth_Exists_UserID";
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@SocialMediaID", socialMediaID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString()) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Communicator.SocialMediaAuth> GetByUserID_CustomerID_SocialMediaID(string UserID, int CustomerID, int SocialMediaID)
        {
            
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SocialMediaAuth_Select_UserID_CustomerID_SocialMediaID";
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@SocialMediaID", SocialMediaID);
            return GetList(cmd);
        }

        public static int Save(ECN_Framework_Entities.Communicator.SocialMediaAuth sma, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "e_SocialMediaAuth_Save";
            cmd.Parameters.AddWithValue("@SocialMediaAuthID", sma.SocialMediaAuthID);
            cmd.Parameters.AddWithValue("@SocialMediaID", sma.SocialMediaID);
            cmd.Parameters.AddWithValue("@CustomerID", sma.CustomerID);
            cmd.Parameters.AddWithValue("@AccessToken", sma.Access_Token);
            cmd.Parameters.AddWithValue("@UserID", sma.UserID);
            cmd.Parameters.AddWithValue("@IsDeleted", sma.IsDeleted);
            cmd.Parameters.AddWithValue("@AccessSecret", sma.Access_Secret);
            cmd.Parameters.AddWithValue("@ProfileName", sma.ProfileName);

            if (sma.SocialMediaAuthID > 0)
            {
                cmd.Parameters.AddWithValue("@UpdatedUserID", userID);
            }
            else
                cmd.Parameters.AddWithValue("@CreatedUserID", userID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        private static List<ECN_Framework_Entities.Communicator.SocialMediaAuth> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.SocialMediaAuth> retList = new List<ECN_Framework_Entities.Communicator.SocialMediaAuth>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.SocialMediaAuth retItem = new ECN_Framework_Entities.Communicator.SocialMediaAuth();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.SocialMediaAuth>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Communicator.SocialMediaAuth Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.SocialMediaAuth retItem = new ECN_Framework_Entities.Communicator.SocialMediaAuth();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.SocialMediaAuth();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.SocialMediaAuth>.CreateBuilder(rdr);
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
