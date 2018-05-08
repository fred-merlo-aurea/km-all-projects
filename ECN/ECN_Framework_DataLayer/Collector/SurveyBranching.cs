using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ECN_Framework_Common.Objects;
using KM.Common;


namespace ECN_Framework_DataLayer.Collector
{
    [Serializable]
    public class SurveyBranching
    {
        public static bool BranchingExits(int QuestionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select QuestionID from SurveyBranching WITH (NOLOCK) where QuestionID = @QuestionID) select 1 else select 0";
            cmd.Parameters.AddWithValue("@QuestionID", QuestionID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Collector.ToString())) > 0 ? true : false;
        }

        public static void DeletebyPageID(int PageID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Delete from SurveyBranching where QuestionID in (select QuestionID from question where PageID = @PageID)";
            cmd.Parameters.AddWithValue("@PageID", PageID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Collector.ToString());
        }

        public static ECN_Framework_Entities.Collector.SurveyBranching GetByOptionID(int OptionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select SurveyID, QuestionID, OptionID, case when EndSurvey = 1 then -1 else isnull(PageID, 0) end as PageID, EndSurvey from SurveyBranching with (NOLOCK) where OptionID=@OptionID";
            cmd.Parameters.AddWithValue("@OptionID", OptionID);
            return Get(cmd);
        }

        private static ECN_Framework_Entities.Collector.SurveyBranching Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Collector.SurveyBranching retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Collector.SurveyBranching();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Collector.SurveyBranching>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Collector.SurveyBranching> GetList(SqlCommand cmd, int customerID)
        {
            List<ECN_Framework_Entities.Collector.SurveyBranching> retList = new List<ECN_Framework_Entities.Collector.SurveyBranching>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Collector.SurveyBranching retItem = new ECN_Framework_Entities.Collector.SurveyBranching();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Collector.SurveyBranching>.CreateBuilder(rdr);
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

        public static void Save(ECN_Framework_Entities.Collector.SurveyBranching item, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_SurveyBranching_Save";
            cmd.Parameters.Add(new SqlParameter("@SurveyID", (object)item.SurveyID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@QuestionID", (object)item.QuestionID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@OptionID", (object)item.OptionID ?? DBNull.Value));
            if(item.PageID > 0)
                cmd.Parameters.Add(new SqlParameter("@PageID", (object)item.PageID ?? DBNull.Value));
            
            cmd.Parameters.Add(new SqlParameter("@EndSurvey", (object)item.EndSurvey ?? DBNull.Value));
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Collector.ToString()).ToString();
        }
    }
}
