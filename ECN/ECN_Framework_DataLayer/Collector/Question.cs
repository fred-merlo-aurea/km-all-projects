using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
using KM.Common;


namespace ECN_Framework_DataLayer.Collector
{

    [Serializable]
    [DataContract]
    public class Question
    {
        public static int Save(ECN_Framework_Entities.Collector.Question item, string position, int targetID, string options, string gridrow)
        {
            SqlCommand cmd = new SqlCommand("sp_SaveQuestion");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@QuestionID", SqlDbType.Int);
            cmd.Parameters["@QuestionID"].Value = item.QuestionID;

            cmd.Parameters.Add("@SurveyID", SqlDbType.Int);
            cmd.Parameters["@SurveyID"].Value = item.SurveyID;

            cmd.Parameters.Add("@PageID", SqlDbType.Int);
            cmd.Parameters["@PageID"].Value = item.PageID;

            cmd.Parameters.Add("@format", SqlDbType.VarChar);
            cmd.Parameters["@format"].Value = item.Format;

            cmd.Parameters.Add("@grid_control_type", SqlDbType.VarChar);
            cmd.Parameters["@grid_control_type"].Value = item.Grid_control_Type;

            cmd.Parameters.Add("@QuestionText", SqlDbType.VarChar);
            cmd.Parameters["@QuestionText"].Value = item.QuestionText;

            cmd.Parameters.Add("@maxlength", SqlDbType.Int);
            cmd.Parameters["@maxlength"].Value = item.MaxLength;

            cmd.Parameters.Add("@Required", SqlDbType.Bit);
            cmd.Parameters["@Required"].Value = item.Required;

            cmd.Parameters.Add("@GridValidation", SqlDbType.Int);
            cmd.Parameters["@GridValidation"].Value = item.GridValidation;

            cmd.Parameters.Add("@ShowTextControl", SqlDbType.Bit);
            cmd.Parameters["@ShowTextControl"].Value = item.ShowTextControl;

            cmd.Parameters.Add("@position", SqlDbType.VarChar);
            cmd.Parameters["@position"].Value = position;

            cmd.Parameters.Add("@targetID", SqlDbType.Int);
            cmd.Parameters["@targetID"].Value = targetID;

            cmd.Parameters.Add("@options", SqlDbType.Text);
            cmd.Parameters["@options"].Value = options;                

            cmd.Parameters.Add("@gridrow", SqlDbType.Text);
            cmd.Parameters["@gridrow"].Value = gridrow;
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Collector.ToString()).ToString());

        }

        public static List<ECN_Framework_Entities.Collector.Question> GetBySurveyID(int SurveyID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Question with (NOLOCK) where SurveyID=@SurveyID";
            cmd.Parameters.AddWithValue("@SurveyID", SurveyID);
            return GetList(cmd);
        }

        private static ECN_Framework_Entities.Collector.Question Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Collector.Question retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Collector.Question();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Collector.Question>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Collector.Question> GetList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Collector.Question> retList = new List<ECN_Framework_Entities.Collector.Question>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Collector.Question retItem = new ECN_Framework_Entities.Collector.Question();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Collector.Question>.CreateBuilder(rdr);
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

        public static void Reorder(int PageID, string Position, int QuestionID, int ToQuestionID)
        {
            SqlCommand cmd = new SqlCommand("sp_ReOrderQuestion");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PageID", SqlDbType.Int);
            cmd.Parameters["@PageID"].Value = PageID;

            cmd.Parameters.Add("@QuestionID", SqlDbType.Int);
            cmd.Parameters["@QuestionID"].Value = QuestionID;

            cmd.Parameters.Add("@Position", SqlDbType.Char);
            cmd.Parameters["@Position"].Value = Position;

            cmd.Parameters.Add("@ToQuestionID", SqlDbType.Int);
            cmd.Parameters["@ToQuestionID"].Value = ToQuestionID;
            DataFunctions.Execute(cmd, DataFunctions.ConnectionString.Collector.ToString());
        }

        public static List<ECN_Framework_Entities.Collector.Question> GetByPageID(int PageID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Question with (NOLOCK) where PageID=@PageID order by number asc";
            cmd.Parameters.AddWithValue("@PageID", PageID);
            return GetList(cmd);
        }

        public static void Delete(int QuestionID)
        {
            SqlCommand cmd = new SqlCommand("sp_DeleteQuestion");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@QuestionID", SqlDbType.Int);
            cmd.Parameters["@QuestionID"].Value = QuestionID;
            DataFunctions.Execute(cmd, DataFunctions.ConnectionString.Collector.ToString());
        }

        public static List<ECN_Framework_Entities.Collector.Question> GetBranchingQuestionsByPageID(int PageID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select q.* from SurveyBranching sb join question q on sb.QuestionID = q.QuestionID where q.PageID = @PageID";
            cmd.Parameters.AddWithValue("@PageID", PageID);
            return GetList(cmd);
        }

        public static ECN_Framework_Entities.Collector.Question GetByQuestionID(int QuestionID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Question  with (NOLOCK) where QuestionID=@QuestionID";
            cmd.Parameters.AddWithValue("@QuestionID", QuestionID);
            return Get(cmd);
        }

        public static DataTable GetQuestionsGridByPageID(int PageID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select *, case when format<>'textbox' then dbo.fn_getOptions(QuestionID) else '' end as options, case when format='grid' then dbo.fn_getGridoptions(QuestionID) else '' end as rows, (select case when count(*) > 0 then 1 else 0 end from SurveyBranching where QuestionID = Question.QuestionID) as BranchingExists from Question where PageID = " + PageID + " order by number";
            cmd.Parameters.AddWithValue("@PageID", PageID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Collector.ToString());
        }

        public static DataTable GetFilterValues(string Filters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_getFilterValues";
            cmd.Parameters.AddWithValue("@filterstr", Filters);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Collector.ToString());
        }

        public static DataTable GetQuestionsWithFilterCount(int SurveyID, string Filters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_GetQuestionsWithFilterCount";
            cmd.Parameters.AddWithValue("@Filterstr", Filters);
            cmd.Parameters.AddWithValue("@SurveyID", SurveyID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Collector.ToString());
        }

        public static DataTable GetTextResponses(int QuestionID, bool OtherText, string Filters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_getTextResponses";
            cmd.Parameters.AddWithValue("@Filterstr", Filters);
            cmd.Parameters.AddWithValue("@OtherText", OtherText);
            cmd.Parameters.AddWithValue("@QuestionID", QuestionID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Collector.ToString());
        }
    }
}
