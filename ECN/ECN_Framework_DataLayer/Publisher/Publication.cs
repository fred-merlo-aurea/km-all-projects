using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.Publisher
{
    [Serializable]
    public class Publication
    {
        public static List<ECN_Framework_Entities.Publisher.Publication> GetByCustomerID(int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Publication_Select_CustomerID";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Publisher.Publication GetByPublicationID(int publicationID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Publication_Select_PublicationID";
            cmd.Parameters.Add(new SqlParameter("@PublicationID", publicationID));
            return Get(cmd);
        }

        private static List<ECN_Framework_Entities.Publisher.Publication> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Publisher.Publication> retList = new List<ECN_Framework_Entities.Publisher.Publication>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Publisher.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Publisher.Publication retItem = new ECN_Framework_Entities.Publisher.Publication();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Publisher.Publication>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.Publisher.Publication Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Publisher.Publication retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Publisher.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Publisher.Publication();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Publisher.Publication>.CreateBuilder(rdr);
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

        public static bool Exists(int publicationID, string publicationName, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Publication_Exists_BYName";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            cmd.Parameters.Add(new SqlParameter("@PublicationName", publicationName));
            cmd.Parameters.Add(new SqlParameter("@PublicationID", publicationID));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString())) > 0 ? true : false;
        }

        public static bool Exists(int publicationID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Publication_Exists_BYID";
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            cmd.Parameters.Add(new SqlParameter("@PublicationID", publicationID));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString())) > 0 ? true : false;
        }

        public static bool ExistsAlias(int publicationID, string publicationCode)
        {
            if (!string.IsNullOrWhiteSpace(publicationCode))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "e_Publication_Exists_BYCode";
                cmd.Parameters.Add(new SqlParameter("@PublicationCode", publicationCode));
                cmd.Parameters.Add(new SqlParameter("@PublicationID", publicationID));
                return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString())) > 0 ? true : false;
            }
            return false;
        }

        public static int Save(ECN_Framework_Entities.Publisher.Publication publication)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Publication_Save";
            cmd.Parameters.Add(new SqlParameter("@PublicationID", publication.PublicationID));
            cmd.Parameters.Add(new SqlParameter("@PublicationName", publication.PublicationName));
            cmd.Parameters.Add(new SqlParameter("@PublicationType", publication.PublicationType));
            cmd.Parameters.Add(new SqlParameter("@CategoryID", publication.CategoryID));
            cmd.Parameters.Add(new SqlParameter("@Circulation", publication.Circulation));
            cmd.Parameters.Add(new SqlParameter("@FrequencyID", publication.FrequencyID));
            cmd.Parameters.Add(new SqlParameter("@PublicationCode", publication.PublicationCode));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", publication.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@GroupID", publication.GroupID));
            cmd.Parameters.Add(new SqlParameter("@Active", publication.Active));
            cmd.Parameters.Add(new SqlParameter("@ContactAddress1", publication.ContactAddress1));
            cmd.Parameters.Add(new SqlParameter("@ContactAddress2", publication.ContactAddress2));
            cmd.Parameters.Add(new SqlParameter("@ContactEmail", publication.ContactEmail));
            cmd.Parameters.Add(new SqlParameter("@ContactPhone", publication.ContactPhone));
            cmd.Parameters.Add(new SqlParameter("@ContactFormLink", publication.ContactFormLink));
            cmd.Parameters.Add(new SqlParameter("@EnableSubscription", publication.EnableSubscription));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionOption", publication.SubscriptionOption));
            cmd.Parameters.Add(new SqlParameter("@SubscriptionFormLink", publication.SubscriptionFormLink));
            cmd.Parameters.Add(new SqlParameter("@LogoURL", publication.LogoURL));
            cmd.Parameters.Add(new SqlParameter("@LogoLink", publication.LogoLink));
            if (publication.PublicationID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)publication.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)publication.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString()));
        }

        public static void Delete(int publicationID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Publication_Delete";
            cmd.Parameters.Add(new SqlParameter("@PublicationID", publicationID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Publisher.ToString());
        }
    }
}
