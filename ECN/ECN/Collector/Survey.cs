using System;
using System.Data;
using System.Data.SqlClient;
using ecn.common.classes;
using System.Configuration;

namespace ecn.collector.classes
{
    public class Survey
    {
        public static string communicatordb = ConfigurationManager.AppSettings["communicatordb"];
        public static string collectordb = ConfigurationManager.AppSettings["collectordb"];
        public static string con_collector = ConfigurationManager.AppSettings["col"];

        #region static methods

        public static DataTable GetSurveys(int customerID)
        {
            return DataFunctions.GetDataTable("SELECT s.*, (select COUNT(DISTINCT(EmailID)) FROM " + communicatordb + "..EmailDataValues WHERE GroupDatafieldsID = (select groupdatafieldsID from " + communicatordb + "..groupdatafields gdf where IsDeleted=0 and  shortname=Convert(varchar,s.SurveyID)+ '_completionDt' and gdf.surveyID = s.SurveyID and gdf.groupID = s.GroupID) AND DataValue <> '') as responsecount,  'surveyID=' + convert(varchar,s.SurveyID) + '&chID=' + convert(varchar,c.basechannelID) + '&cuID=" + customerID + "' as SurveyURL FROM survey s join ecn5_accounts..customer c on s.CustomerID = c.customerID WHERE s.CustomerID = " + customerID + " ORDER BY s.CreatedDate desc", con_collector);
        }

        public static DataTable GetOptionValues(int QuestionID)
        {
            return DataFunctions.GetDataTable("select ro.OptionID, ro.questionid, ro.score, ro.OptionValue, case when EndSurvey = 1 then -1 else Isnull(PageID,0) end as PageID from " + collectordb + "..response_options ro left join " + collectordb + "..SurveyBranching sb on ro.OptionID = sb.OptionID where ro.questionID= " + QuestionID, con_collector);
        }

        public static DataTable GetGridStatements(int QuestionID)
        {
            return DataFunctions.GetDataTable("SELECT * FROM " + collectordb + "..grid_statements WHERE questionID= " + QuestionID, con_collector);
        }

        public static bool IsExpired(int surveyID)
        {
            string sql = string.Format(@"select case when isnull(DisableDate, '') = '' then 'false' when datediff(d,getdate(), DisableDate)	>= 0 then 'false' else 'true' end from survey where SurveyID = {0}", surveyID);
            return Convert.ToBoolean(DataFunctions.ExecuteScalar("collector", sql));
        }

        public static bool IsCompleted(int surveyID, int ParticipantID)
        {
            string CompletionDate_gdfID = GetGroupDataFieldsID_CompletionDate(surveyID);
            string sql = string.Format(@"if exists(select EmailDataValuesID from {0}.dbo.emaildatavalues where emailID = {1} and groupdataFieldsID = {2} and ltrim(rtrim(datavalue)) <> '') select 'true' else select 'false'", communicatordb, ParticipantID, CompletionDate_gdfID);
            return Convert.ToBoolean(DataFunctions.ExecuteScalar(sql));
        }

        public static int AddUserToSurveyGroup(int surveyID, string username, string IPAddress)
        {
            if (username == string.Empty)
                username = String.Format("{0}-{1}@survey_{2}.com", System.Guid.NewGuid().ToString().Substring(1, 5), System.DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss"), surveyID);

            return Convert.ToInt32(DataFunctions.ExecuteScalar(string.Format("exec sp_AddUserToGroup {0}, '{1}', '{2}'", surveyID, username.Replace("'", "''"), IPAddress)));
        }

        /*
         * bool ValidUser(string userName)
         * see whether this participant's profile exists in the database
         * args: string userName - a string such as a user name or email address that identifies this participant
         */
        public static bool ValidUser(int surveyID, string userName)
        {
            return GetParticipantID(surveyID, userName) > 0;
        }

        /*
         * GetParticipantID(string username)
         * get the unique integer identifier for this participant
         * args: string username - this user's username
         * returns: the unique integer identifier for this participant
         */
        public static int GetParticipantID(int surveyID, string username)
        {
            string groupID = GetGroupID(surveyID);
            string query = string.Format(@"select e.EmailID from {0}.dbo.Emails e join {0}.dbo.EmailGroups eg on e.EmailID = eg.EmailID where
eg.GroupID = {1} and EmailAddress='{2}'", communicatordb, groupID, username);
            object email_id = DataFunctions.ExecuteScalar(query);
            if (null == email_id) return 0;
            return Convert.ToInt32(email_id);
        }

        /* GetGroupID()
         * returns the group ID that this survey acts on
         */
        public static string GetGroupID(int surveyID)
        {
            string Query = "SELECT GroupID FROM " + collectordb + "..survey WHERE SurveyID = " + surveyID;
            string groupid;
            try
            {
                groupid = DataFunctions.ExecuteScalar(Query).ToString();
            }
            catch
            {
                groupid = "";
            }
            return groupid;
        }

        /*
         * GetAllQuestions(int SurveyID, int PageID)
         * get all of the questions belonging to a particular survey  
         * args: int surveyID - a unique identifier for this survey 
         * returns: a DataTable containing all questions belonging to this survey
         */
        public static DataTable GetAllQuestions(int SurveyID, int PageID)
        {
            if (PageID != 0)
                return DataFunctions.GetDataTable("Select QuestionID, number, PageID, format, isnull(convert(varchar,grid_control_type),'') grid_control_type, QuestionText, isnull(convert(varchar,maxlength),0) as  maxlength, case when required = 1 then 'true' else 'false' end as Required, isnull(GridValidation,0) as GridValidation, case when ShowTextControl = 1 then 'true' else 'false' end as ShowTextControl from " + collectordb + "..question where PageID = " + PageID + " ORDER BY number;");
            else
                return DataFunctions.GetDataTable("Select QuestionID, number, PageID, format, isnull(convert(varchar,grid_control_type),'') grid_control_type, QuestionText, isnull(convert(varchar,maxlength),0) as  maxlength, case when required = 1 then 'true' else 'false' end as Required, isnull(GridValidation,0) as GridValidation, case when ShowTextControl = 1 then 'true' else 'false' end as ShowTextControl from " + collectordb + "..question where SurveyID = " + SurveyID + " ORDER BY number;");
        }


        /*
         * InsertResponse(int participantID, int surveyID, string responseText, int questionNumber)
         * store a participant response in the database
         * args: int participantID   - a unique identifier for this participant
         *		 int surveyID        - a unique identifier for this survey 
         *       string responseText - the text of the participant's response
         *       int questionNumber  - the number of the question to which participant is responding 
         */
        public static void InsertResponse(int participantID, int surveyID, string responseText, int questionNumber)
        {
            responseText = StringFunctions.Replace(responseText, "'", "''");
            string my_groupdatafieldsID = GetGroupDataFieldsID(surveyID, questionNumber);

            string responseInsert = "INSERT INTO " + communicatordb + "..EmailDataValues (EmailID, GroupDatafieldsID, DataValue, ModifiedDate) ";
            responseInsert += "VALUES (" + participantID + ", " + my_groupdatafieldsID + ", '" + responseText + "', GetDate() )";
            DataFunctions.Execute(responseInsert);
        }
        /*
         * InsertResponse(int participantID, int surveyID, string responseText, int questionNumber)
         * store a participant response in the database
         * args: int participantID   - a unique identifier for this participant
         *		 int surveyID        - a unique identifier for this survey 
         *       string responseText - the text of the participant's response
         *       int questionNumber  - the number of the question to which participant is responding 
         */
        public static void InsertResponse(int participantID, int surveyID, string responseText, string GroupDataFieldName)
        {
            responseText = StringFunctions.Replace(responseText, "'", "''");
            string my_groupdatafieldsID = GetGroupDataFieldsID(surveyID, GroupDataFieldName);

            string responseInsert = "INSERT INTO " + communicatordb + ".dbo.EmailDataValues (EmailID, GroupDatafieldsID, DataValue, ModifiedDate) ";
            responseInsert += "VALUES (" + participantID + ", " + my_groupdatafieldsID + ", '" + responseText + "', GetDate() )";
            DataFunctions.Execute(responseInsert);
        }
        /*
         * InsertResponse(int participantID, int surveyID, string responseText, int questionNumber, int gridStatementID)
         * store a participant response in the database
         * overloaded implementation of InsertResponse handles responses to questions which are formatted as grids
         * args: int participantID   - a unique identifier for this participant
         *		 int surveyID        - a unique identifier for this survey 
         *       string responseText - the text of the participant's response
         *       int questionNumber  - the number of the question to which participant is responding 
         *       int gridStatementID - a unique identifier for the grid statement to which participant is responding 
         */
        public static void InsertResponse(int participantID, int surveyID, string responseText, int questionNumber, int gridStatementID)
        {
            responseText = StringFunctions.Replace(responseText, "'", "''");

            string my_groupdatafieldsID = GetGroupDataFieldsID(surveyID, questionNumber);

            string responseInsert = "INSERT INTO " + communicatordb + ".dbo.EmailDataValues (EmailID, GroupDatafieldsID, DataValue, ModifiedDate, SurveyGridID) ";
            responseInsert += "VALUES (" + participantID + ", " + my_groupdatafieldsID + ", '" + responseText + "', GetDate(), " + gridStatementID + ")";
            DataFunctions.Execute(responseInsert);
        }
        /*
         * DeleteResponse(int surveyID, int questionNumber)
         * delete responses associated with a question from the database
         * args: int surveyID        - a unique identifier for this survey
         *		 int questionNumber  - the number of the question that is being deleted 
         */
        public static void DeleteResponse(int surveyID, int questionNumber)
        {
            string my_groupdatafieldsID = GetGroupDataFieldsID(surveyID, questionNumber);
            string deleteResponse = "DELETE FROM " + communicatordb + ".dbo.EmailDataValues WHERE GroupDatafieldsID = " + my_groupdatafieldsID;
            DataFunctions.Execute(deleteResponse);
        }
        /*
         * DeleteResponse(int participantID, int surveyID, int questionNumber)
         * overloaded implementation of DeleteResponse deletes a particular participant's 
         * response from the database.
         * args: int participantID   - a unique identifier for this participant
         *		 int surveyID        - a unique identifier for this survey 
         *       int questionNumber  - the number of the question to which participant is responding 
         */
        public static void DeleteResponse(int participantID, int surveyID, int questionNumber)
        {
            string my_groupdatafieldsID = GetGroupDataFieldsID(surveyID, questionNumber);
            string deleteResponse = "DELETE FROM " + communicatordb + ".dbo.EmailDataValues WHERE GroupDatafieldsID = " + my_groupdatafieldsID + " AND EmailID= " + participantID;

            DataFunctions.Execute(deleteResponse);
        }

        public static void DeleteResponse(int participantID, int surveyID, string shortname)
        {
            string my_groupdatafieldsID = GetGroupDataFieldsID(surveyID, shortname);
            string deleteResponse = "DELETE FROM " + communicatordb + ".dbo.EmailDataValues WHERE GroupDatafieldsID = " + my_groupdatafieldsID + " AND EmailID= " + participantID;

            DataFunctions.Execute(deleteResponse);
        }

        private static string GetGroupDataFieldsID(int surveyID, int questionNumber)
        {
            string sql = string.Format(@"
declare @groupID int
declare @groupDataFieldID int
set @groupID = (SELECT GroupID FROM survey WHERE SurveyID = {0})
set @groupDataFieldID = (SELECT GroupDatafieldsID from {2}.dbo.GroupDatafields where GroupID = @groupID AND ShortName='{0}_{1}' AND SurveyID={0})
if (@groupDataFieldID is null) 
BEGIN
	INSERT INTO {2}.dbo.GroupDataFields (GroupID, ShortName, LongName, SurveyID) Values (@groupID, '{0}_{1}', '{1}', {0});
	SELECT @@IDENTITY
END
ELSE
	select @groupDataFieldID", surveyID, questionNumber, communicatordb);
            return DataFunctions.ExecuteScalar(sql).ToString();
        }

        private static string GetGroupDataFieldsID(int surveyID, string shortName)
        {
            string sql = string.Format(@"
declare @groupID int
declare @groupDataFieldID int
set @groupID = (SELECT GroupID FROM survey WHERE SurveyID = {0})
set @groupDataFieldID = (SELECT GroupDatafieldsID from {2}.dbo.GroupDatafields where GroupID = @groupID AND ShortName='{1}' AND SurveyID={0})
if (@groupDataFieldID is null) 
BEGIN
	INSERT INTO {2}.dbo.GroupDataFields (GroupID, ShortName, LongName, SurveyID) Values (@groupID, '{1}', '{1}', {0});
	SELECT @@IDENTITY
END
ELSE
	select @groupDataFieldID", surveyID, shortName, communicatordb);
            return DataFunctions.ExecuteScalar(sql).ToString();
        }

        private static string GetGroupDataFieldsID_CompletionDate(int surveyID)
        {
            string sql = string.Format(@"
declare @groupID int
declare @groupDataFieldID int
set @groupID = (SELECT GroupID FROM survey WHERE SurveyID = {0})
set @groupDataFieldID = (SELECT GroupDatafieldsID from {2}.dbo.GroupDatafields where GroupID = @groupID AND ShortName='{0}_{1}' AND SurveyID={0})
if (@groupDataFieldID is null) 
BEGIN
	INSERT INTO {2}.dbo.GroupDataFields (GroupID, ShortName, LongName, SurveyID) Values (@groupID, '{0}_{1}', '{1}', {0});
	SELECT @@IDENTITY
END
ELSE
	select @groupDataFieldID", surveyID, "completionDt", communicatordb);
            return DataFunctions.ExecuteScalar(sql).ToString();
        }

        private static string GetGroupDataFieldsID_BlastID(int surveyID)
        {
            string sql = string.Format(@"
declare @groupID int
declare @groupDataFieldID int
set @groupID = (SELECT GroupID FROM survey WHERE SurveyID = {0})
set @groupDataFieldID = (SELECT GroupDatafieldsID from {2}.dbo.GroupDatafields where GroupID = @groupID AND ShortName='{0}_{1}' AND SurveyID={0})
if (@groupDataFieldID is null) 
BEGIN
	INSERT INTO {2}.dbo.GroupDataFields (GroupID, ShortName, LongName, SurveyID) Values (@groupID, '{0}_{1}', '{1}', {0});
	SELECT @@IDENTITY
END
ELSE
	select @groupDataFieldID", surveyID, "blastID", communicatordb);
            return DataFunctions.ExecuteScalar(sql).ToString();
        }


        public static DataTable GetResponses(int surveyID, int questionNumber, int participantID)
        {
            string groupID = GetGroupID(surveyID);
            string shortName = string.Format("{0}_{1}", surveyID, questionNumber);
            string sql = string.Format(@"select DataValue, SurveyGridID from {3}.dbo.EmailDataValues 
where GroupDatafieldsID in (SELECT GroupDatafieldsID from {3}.dbo.GroupDatafields where GroupID = {0} AND ShortName = '{1}') 
AND EmailID = {2}", groupID, shortName, participantID, communicatordb);
            return DataFunctions.GetDataTable(sql);
        }


        public static int GetCompletedResponseCount(int surveyID, string FilterStr)
        {
            SqlCommand cmd = new SqlCommand("sp_SurveyFilterResults");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@surveyID", SqlDbType.Int);
            cmd.Parameters["@surveyID"].Value = surveyID;
            cmd.Parameters.Add("@Filterstr", SqlDbType.VarChar);
            cmd.Parameters["@Filterstr"].Value = FilterStr;
            cmd.Parameters.Add("@OnlyCounts", SqlDbType.Int);
            cmd.Parameters["@OnlyCounts"].Value = 1;

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }

        public static int GetIncompleteResponseCount(int surveyID)
        {
            SqlCommand cmd = new SqlCommand("sp_getIncompleteSurveyCount");
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@surveyID", SqlDbType.Int);
            cmd.Parameters["@surveyID"].Value = surveyID;

            return Convert.ToInt32(DataFunctions.ExecuteScalar(cmd));
        }


        public static string GetTEXTResponses(int surveyID, int questionNumber, int participantID)
        {
            try
            {
                string gdfID = GetGroupDataFieldsID(surveyID, string.Format("{0}_{1}_TEXT", surveyID, questionNumber));
                return DataFunctions.ExecuteScalar(string.Format(@"select dataValue from {0}..emaildatavalues edv where groupdatafieldsID = {1} and emailID = {2}", communicatordb, gdfID, participantID)).ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /*
         * DeleteResponses(int surveyID)
         * delete all responses associated with a survey
         * args: int surveyID - a unique identifier for this survey
         */
        public static void DeleteResponses(int surveyID)
        {
            ECN_Framework.Common.SecurityAccess.canI("Survey", surveyID.ToString());
            string groupid = GetGroupID(surveyID);
            DataTable dt = DataFunctions.GetDataTable("SELECT GroupDatafieldsID from " + communicatordb + ".dbo.GroupDatafields where GroupID = " + groupid + " AND SurveyID=" + surveyID);
            foreach (DataRow dr in dt.Rows)
            {
                string deleteResponse = "DELETE FROM " + communicatordb + ".dbo.EmailDataValues WHERE GroupDatafieldsID = " + dr["GroupDatafieldsID"];
                DataFunctions.Execute(deleteResponse);
            }
        }
        public static void ToggleStatus(int surveyID)
        {
            ECN_Framework.Common.SecurityAccess.canI("Survey", surveyID.ToString());
            string sql_status = "select IsActive from survey where SurveyID=" + surveyID;
            bool active = Convert.ToBoolean(DataFunctions.ExecuteScalar(sql_status));
            if (active)
            {
                DataFunctions.Execute("update survey set isActive=0 where SurveyID=" + surveyID);
            }
            else
            {
                DataFunctions.Execute("update survey set isActive=1 where SurveyID=" + surveyID);
            }
        }

        /*
         * InsertCompletionDt(int participantID, int surveyID)
         * store a participant response in the database
         * args: int participantID   - a unique identifier for this participant
         *		 int surveyID        - a unique identifier for this survey 
         */
        public static void InsertCompletionDt(int participantID, int surveyID)
        {
            string my_groupdatafieldsID = GetGroupDataFieldsID_CompletionDate(surveyID);

            string responseInsert = string.Format(@"
declare @emailDataFieldsID_Count int
declare @lenEmailDataValue int
set @emailDataFieldsID_Count = (SELECT COUNT(EmailDataValuesID) FROM {0}.dbo.EmailDataValues WHERE GroupDataFieldsID = {1} AND EmailID = {2})
if(@emailDataFieldsID_Count < 1)
	BEGIN
		INSERT INTO {0}.dbo.EmailDataValues (EmailID, GroupDatafieldsID, DataValue, ModifiedDate) VALUES ({2}, {1}, getdate(), getdate())
	END
ELSE if(@emailDataFieldsID_Count = 1)
	BEGIN
		SET @lenEmailDataValue = (SELECT len(datavalue) FROM {0}.dbo.EmailDataValues 
		WHERE GroupDataFieldsID = {1} AND EmailID = {2})
		if(@lenEmailDataValue < 1)
			BEGIN 
				UPDATE {0}.dbo.EmailDataValues SET DataValue = getdate() WHERE EmailID = {2} 
				and GroupDatafieldsID = {1}
			END
	END
", communicatordb, my_groupdatafieldsID, participantID);
            DataFunctions.Execute(responseInsert);
        }

        /*
                 * InsertCompletionDt(int participantID, int surveyID)
                 * store a participant response in the database
                 * args: int participantID   - a unique identifier for this participant
                 *		 int surveyID        - a unique identifier for this survey 
                 */
        public static void InsertBlastID(int participantID, int surveyID, int BlastID)
        {
            string GDF_blastID = GetGroupDataFieldsID_BlastID(surveyID);

            string responseInsert = string.Format(@"
declare @emailDataFieldsID_Count int
declare @lenEmailDataValue int
set @emailDataFieldsID_Count = (SELECT COUNT(EmailDataValuesID) FROM {0}.dbo.EmailDataValues WHERE GroupDataFieldsID = {1} AND EmailID = {2})
if(@emailDataFieldsID_Count < 1)
	BEGIN
		INSERT INTO {0}.dbo.EmailDataValues (EmailID, GroupDatafieldsID, DataValue, ModifiedDate) VALUES ({2}, {1}, '{3}', getdate())
	END
ELSE if(@emailDataFieldsID_Count = 1)
	BEGIN
		SET @lenEmailDataValue = (SELECT len(datavalue) FROM {0}.dbo.EmailDataValues 
		WHERE GroupDataFieldsID = {1} AND EmailID = {2})
		if(@lenEmailDataValue < 1)
			BEGIN 
				UPDATE {0}.dbo.EmailDataValues SET DataValue = '{3}' WHERE EmailID = {2} 
				and GroupDatafieldsID = {1}
			END
	END
", communicatordb, GDF_blastID, participantID, BlastID.ToString());
            DataFunctions.Execute(responseInsert);
        }


        /*
         * bool HasResponses(int surveyID)
         * see if a survey has responses associated with it
         * args: int surveyID - a unique identifier for this survey
         * returns: true if survey has responses associated with it; otherwise false
         */
        public static bool HasResponses(int surveyID)
        {
            return Convert.ToBoolean(DataFunctions.ExecuteScalar(String.Format("if exists (select top 1 EmailDataValuesID from {0}..emaildatavalues edv join {0}..groupdatafields gdf on edv.groupdatafieldsID =  gdf.groupdatafieldsID  where surveyID = {1})  select 'true' else select 'false'", communicatordb, surveyID)));
        }

        public static DataTable GetGridResponseOptions(int surveyID, int questionID)
        {
            return DataFunctions.GetDataTable(string.Format("select * from response_options where QuestionID = {0}", questionID));
        }
        #endregion
    }
}

