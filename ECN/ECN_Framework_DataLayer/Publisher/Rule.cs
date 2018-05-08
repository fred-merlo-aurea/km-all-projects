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
    public class Rule
    {
        public static bool Exists(int ruleID, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Rule_Exists_ByID";
            cmd.Parameters.Add(new SqlParameter("@RuleID", ruleID));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Publisher.Rule> GetByPublicationID(int publicationID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Rule_Select_PublicationID";
            cmd.Parameters.Add(new SqlParameter("@PublicationID", publicationID));
            return GetList(cmd);
        }

        private static List<ECN_Framework_Entities.Publisher.Rule> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Publisher.Rule> retList = new List<ECN_Framework_Entities.Publisher.Rule>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Publisher.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Publisher.Rule retItem = new ECN_Framework_Entities.Publisher.Rule();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Publisher.Rule>.CreateBuilder(rdr);
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

        public static void Delete(int ruleID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Rule_Delete";
            cmd.Parameters.Add(new SqlParameter("@RuleID", ruleID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Publisher.ToString());
        }

        public static int Save(ECN_Framework_Entities.Publisher.Rule rule)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_rule_Save";
            cmd.Parameters.Add(new SqlParameter("@RuleID", rule.RuleID));
            cmd.Parameters.Add(new SqlParameter("@RuleName", rule.RuleName));
            cmd.Parameters.Add(new SqlParameter("@PublicationID", rule.PublicationID));
            cmd.Parameters.Add(new SqlParameter("@EditionID", rule.EditionID));
            if (rule.RuleID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)rule.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)rule.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Publisher.ToString()));
        }
    }
}
