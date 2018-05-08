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
    public class Survey
    {
        public static bool Exists(int surveyID, string surveyTitle, int customerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Survey_Exists_ByTitle";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@SurveyTitle", surveyTitle);
            cmd.Parameters.AddWithValue("@SurveyID", surveyID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Collector.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Collector.Survey> GetByCustomerID(int customerID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Survey_Select_CustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            return GetList(cmd, customerID);
        }

        public static bool HasSurvey(int groupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Survey_Exists_GroupID";
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Collector.ToString())) > 0 ? true : false;
        }

        public static bool HasResponses(int emailID, int surveyID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Survey_HasResponses_EmailID_SurveyID";
            cmd.Parameters.AddWithValue("@EmailID", emailID);
            cmd.Parameters.AddWithValue("@SurveyID", surveyID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Collector.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Collector.Survey> GetByGroupID(int groupid, int customerid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Survey_Select_GroupID_CustomerID";
            cmd.Parameters.AddWithValue("@GroupID", groupid);
            cmd.Parameters.AddWithValue("@CustomerID", customerid);
            return GetList(cmd, customerid);
        }

        private static ECN_Framework_Entities.Collector.Survey Get(SqlCommand cmd)
        {
            ECN_Framework_Entities.Collector.Survey retItem = null;

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    retItem = new ECN_Framework_Entities.Collector.Survey();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Collector.Survey>.CreateBuilder(rdr);
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

        private static List<ECN_Framework_Entities.Collector.Survey> GetList(SqlCommand cmd, int customerID)
        {
            List<ECN_Framework_Entities.Collector.Survey> retList = new List<ECN_Framework_Entities.Collector.Survey>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Collector.Survey retItem = new ECN_Framework_Entities.Collector.Survey();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Collector.Survey>.CreateBuilder(rdr);
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


        public static void Delete(int surveyID, int userID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_DeleteSurvey";
            cmd.Parameters.AddWithValue("@SurveyID", surveyID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Collector.ToString());
        }

        public static void Copy(int NewsurveyID, int OldsurveyID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_copysurvey";
            cmd.Parameters.AddWithValue("@NewsurveyID", NewsurveyID);
            cmd.Parameters.AddWithValue("@OldsurveyID", OldsurveyID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Collector.ToString());
        }

        public static void DeleteResponses(int surveyID, int p)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_DeleteSurveyResponses";
            cmd.Parameters.AddWithValue("@SurveyID", surveyID);
            DataFunctions.ExecuteNonQuery(cmd, DataFunctions.ConnectionString.Collector.ToString());
        }

        //public static bool Exists(int SkipSurveyID, string SurveyName, int customerID)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.Text;
        //    cmd.CommandText = "if exists (select top 1 SurveyID FROM Survey WITH (NOLOCK) " +
        //                      "WHERE CustomerID = @CustomerID and SurveyName=@SurveyName and SurveyID<> @SurveyID) select 1 else select 0";
        //    cmd.Parameters.AddWithValue("@CustomerID", customerID);
        //    cmd.Parameters.AddWithValue("@SurveyName", SurveyName);
        //    cmd.Parameters.AddWithValue("@SurveyID", SkipSurveyID);
        //    return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Collector.ToString())) > 0 ? true : false;
        //}
    
        public static ECN_Framework_Entities.Collector.Survey GetBySurveyID(int surveyID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * FROM Survey WITH (NOLOCK) where SurveyID= @SurveyID";
            cmd.Parameters.AddWithValue("@SurveyID", surveyID);
            return Get(cmd);
        }

        public static int Save(ECN_Framework_Entities.Collector.Survey item, int UserID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Survey_Save";
            cmd.Parameters.Add(new SqlParameter("@SurveyID", (object)item.SurveyID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@SurveyTitle", (object)item.SurveyTitle ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Description", (object)item.Description ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CustomerID", item.CustomerID));
            cmd.Parameters.Add(new SqlParameter("@GroupID", item.GroupID));
            cmd.Parameters.Add(new SqlParameter("@EnableDate", (object)item.EnableDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DisableDate", (object)item.DisableDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IntroHTML", (object)item.IntroHTML ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@ThankYouHTML", (object)item.ThankYouHTML ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsActive", (object)item.IsActive ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CompletedStep", (object)item.CompletedStep ?? DBNull.Value));
            if (item.SurveyID > 0)
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)item.UpdatedUserID ?? DBNull.Value));
            else
                cmd.Parameters.Add(new SqlParameter("@UserID", (object)item.CreatedUserID ?? DBNull.Value));

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Collector.ToString()).ToString());
        }



        public static bool IsSurveyGroup(int GroupID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "if exists (select top 1 SurveyID FROM Survey WITH (NOLOCK) " +
                              "WHERE GroupID = @GroupID) select 1 else select 0";
            cmd.Parameters.AddWithValue("@GroupID", GroupID);
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Collector.ToString())) > 0 ? true : false;
        }

        public static List<ECN_Framework_Entities.Communicator.Email> GetSurveyRespondents(int SurveyID, string Filters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "e_Survey_GetRespondents";
            cmd.Parameters.AddWithValue("@SurveyID", SurveyID);
            cmd.Parameters.AddWithValue("@filterstr", Filters);
            return GetEmailList(cmd);
        }

        private static List<ECN_Framework_Entities.Communicator.Email> GetEmailList(SqlCommand cmd)
        {
            List<ECN_Framework_Entities.Communicator.Email> retList = new List<ECN_Framework_Entities.Communicator.Email>();

            using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, DataFunctions.ConnectionString.Collector.ToString()))
            {
                if (rdr != null)
                {
                    ECN_Framework_Entities.Communicator.Email retItem = new ECN_Framework_Entities.Communicator.Email();
                    var builder = DynamicBuilder<ECN_Framework_Entities.Communicator.Email>.CreateBuilder(rdr);
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

        public static DataTable ExportParticipants(int GroupID, int CustomerID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_ExportSurveyRespondents";
            cmd.Parameters.AddWithValue("@GroupID", GroupID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Communicator.ToString());
      
        }

        public static DataTable GetQuestionResponse(int ParticipantID, int QuestionID, string Filters, int count)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_getQuestionResponse";
            cmd.Parameters.AddWithValue("@EmailID", ParticipantID);
            cmd.Parameters.AddWithValue("@QuestionID", QuestionID);
            cmd.Parameters.AddWithValue("@filterstr", Filters);
            cmd.Parameters.AddWithValue("@count", count);
            return DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.Collector.ToString());
        }

        public static int GetCompletedResponseCount(int SurveyID, string Filters)
        {
            SqlCommand cmd = new SqlCommand("sp_SurveyFilterResults");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@surveyID", SqlDbType.Int);
            cmd.Parameters["@surveyID"].Value = SurveyID;
            cmd.Parameters.Add("@Filterstr", SqlDbType.VarChar);
            cmd.Parameters["@Filterstr"].Value = Filters;
            cmd.Parameters.Add("@OnlyCounts", SqlDbType.Int);
            cmd.Parameters["@OnlyCounts"].Value = 1;
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Collector.ToString()));
        }

        public static int GetIncompleteResponseCount(int SurveyID)
        {
            SqlCommand cmd = new SqlCommand("sp_getIncompleteSurveyCount");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@surveyID", SqlDbType.Int);
            cmd.Parameters["@surveyID"].Value = SurveyID;
            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd, DataFunctions.ConnectionString.Collector.ToString()));
        }
    }


}
