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
    public class DomainSuppression
    {
        public static bool Exists(int? customerID, int? baseChannelID, int domainSuppressionID, string domain)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainSuppression_Exists_ByDomain";
            cmd.Parameters.Add(new SqlParameter("@DomainSuppressionID", (object)domainSuppressionID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", (object)baseChannelID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)customerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Domain", domain));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Communicator.DomainSuppression> GetByDomain(int? customerID, int? baseChannelID, string searchString)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainSuppression_Select_Domain";
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", (object)baseChannelID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)customerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SearchString", searchString));
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.DomainSuppression> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.DomainSuppression> retList = new List<ECN_Framework_Entities.Communicator.DomainSuppression>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.DomainSuppression retItem = new ECN_Framework_Entities.Communicator.DomainSuppression();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.DomainSuppression>.CreateBuilder(rdr);
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

        public static ECN_Framework_Entities.Communicator.DomainSuppression GetByDomainSuppressionID(int domainSuppressionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainSuppression_Select_DomainSuppressionID";
            cmd.Parameters.AddWithValue("@DomainSuppressionID", domainSuppressionID);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Communicator.DomainSuppression Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Communicator.DomainSuppression retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Communicator.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Communicator.DomainSuppression();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.DomainSuppression>.CreateBuilder(rdr);
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

        public static int Save(ECN_Framework_Entities.Communicator.DomainSuppression suppression)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainSuppression_Save";
            cmd.Parameters.Add(new SqlParameter("@DomainSuppressionID", suppression.DomainSuppressionID));
            cmd.Parameters.Add(new SqlParameter("@BaseChannelID", (object)suppression.BaseChannelID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", (object)suppression.CustomerID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Domain", suppression.Domain));
            cmd.Parameters.Add(new SqlParameter("@IsActive", (object)suppression.IsActive ?? DBNull.Value));
            if (suppression.DomainSuppressionID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)suppression.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)suppression.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Communicator.ToString()).ToString());
        }

        public static void Delete(int domainSuppressionID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_DomainSuppression_Delete_DomainSuppressionID";
            cmd.Parameters.AddWithValue("@DomainSuppressionID", domainSuppressionID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Communicator.ToString());
        }
    }
}
