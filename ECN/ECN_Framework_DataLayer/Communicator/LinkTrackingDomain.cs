using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Communicator
{
    [Serializable]
    [DataContract]
    public class LinkTrackingDomain
    {
        public static int Save(ECN_Framework_Entities.Communicator.LinkTrackingDomain item)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingDomain_Save";
            cmd.Parameters.Add(new SqlParameter("@Domain", item.Domain));
            cmd.Parameters.Add(new SqlParameter("@LTID", item.LTID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", item.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@UserID", item.CreatedUserID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()));
        }

        public static void Delete(int linkTrackingDomainID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingDomain_Delete";
            cmd.Parameters.AddWithValue("@LinkTrackingDomainID", linkTrackingDomainID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static void DeleteAll(int customerID, int LTID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingDomain_DeleteAll";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@LTID", LTID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }

        public static bool Exists(string domain, int customerID, int LTID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " if exists (select top 1 dt.LinkTrackingDomainID FROM LinkTrackingDomain dt WITH (NOLOCK) " +
                              " WHERE dt.CustomerID = @CustomerID and dt.Domain = @domainName and dt.IsDeleted = 0 and dt.LTID=@LTID) select 1 else select 0";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@DomainName", domain);
            cmd.Parameters.AddWithValue("@LTID", LTID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }


        public static ECN_Framework_Entities.Communicator.LinkTrackingDomain GetByDomain(string domain, int customerID, int LTID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingDomain_Select_Domain";
            cmd.Parameters.AddWithValue("@Domain", domain);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@LTID", LTID);
            return Get(cmd);
        }

        public static List<ECN_Framework_Entities.Communicator.LinkTrackingDomain> GetByCustomerID(int customerID, int LTID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_LinkTrackingDomain_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@LTID", LTID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Communicator.LinkTrackingDomain Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.LinkTrackingDomain retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.LinkTrackingDomain();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LinkTrackingDomain>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Communicator.LinkTrackingDomain> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.LinkTrackingDomain> retList = new List<ECN_Framework_Entities.Communicator.LinkTrackingDomain>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.LinkTrackingDomain retItem = new ECN_Framework_Entities.Communicator.LinkTrackingDomain();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.LinkTrackingDomain>.CreateBuilder(rdr);
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
