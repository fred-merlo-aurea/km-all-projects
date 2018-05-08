using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using KM.Common;

namespace ECN_Framework_DataLayer.FormDesigner
{
    [Serializable]
    public class Rule
    {
        public static List<ECN_Framework_Entities.FormDesigner.Rule> GetByFormID(int formID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Rule_Select_FormID";
            cmd.Parameters.AddWithValue("@FormID",formID);

            
            return GetList(cmd);
 
        }

        public static ECN_Framework_Entities.FormDesigner.Rule GetByRuleID(int RuleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Rule_Select_RuleID";
            cmd.Parameters.AddWithValue("@RuleID", RuleID);


            return Get(cmd);
        }

        public static void Delete(int RuleID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Rule_Delete_RuleID";
            cmd.Parameters.AddWithValue("@RuleID", RuleID);

            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.FormDesigner.ToString());
        }

        public static int Save(ECN_Framework_Entities.FormDesigner.Rule rule)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Rule_Save";
            cmd.Parameters.Add(new SqlParameter("@Rule_Seq_ID", rule.Rule_Seq_ID));
            cmd.Parameters.Add(new SqlParameter("@Form_Seq_ID", rule.Form_Seq_ID));
            cmd.Parameters.Add(new SqlParameter("@Control_ID", rule.Control_ID));
            cmd.Parameters.Add(new SqlParameter("@ConditionGroup_Seq_ID", rule.ConditionGroup_Seq_ID));
            cmd.Parameters.Add(new SqlParameter("@Type", rule.Type));
            cmd.Parameters.Add(new SqlParameter("@Action", rule.Action));
            cmd.Parameters.Add(new SqlParameter("@ActionJs", rule.ActionJs));
            cmd.Parameters.Add(new SqlParameter("@UrlToRedirect", rule.UrlToRedirect));
            cmd.Parameters.Add(new SqlParameter("@Order", rule.Order));
            cmd.Parameters.Add(new SqlParameter("@ResultType", rule.ResultType));
            cmd.Parameters.Add(new SqlParameter("@NonQualify", rule.NonQualify));
            cmd.Parameters.Add(new SqlParameter("@SuspendpostDB", rule.SuspendpostDB));
            cmd.Parameters.Add(new SqlParameter("@Overwritedatapost", rule.Overwritedatapost));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.FormDesigner.ToString()).ToString());
        }

        private static List<ECN_Framework_Entities.FormDesigner.Rule> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.FormDesigner.Rule > retList = new List<ECN_Framework_Entities.FormDesigner.Rule>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.FormDesigner.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.FormDesigner.Rule retItem = new ECN_Framework_Entities.FormDesigner.Rule();
                    var builder = DynamicBuilder<ECN_Framework_Entities.FormDesigner.Rule>.CreateBuilder(rdr);
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

        private static ECN_Framework_Entities.FormDesigner.Rule Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.FormDesigner.Rule retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.FormDesigner.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.FormDesigner.Rule();
                    var builder = DynamicBuilder<ECN_Framework_Entities.FormDesigner.Rule>.CreateBuilder(rdr);
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
