using ecn.common.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace KM.Framework.Web.WebForms.EmailProfile
{
    public abstract partial class EmailProfileBaseBaseControl : EmailProfileBaseControl
    {
        private const int _minYearValue = 1900;
        private const int _maxYearValue = 9999;
        protected string _emailId = string.Empty;
        protected string _emailAddress = string.Empty;
        protected string _groupId = string.Empty;

        protected override Label lblResultMessage
        {
            get
            {
                return MessageLabel;
            }
        }

        protected Button btnUpdateEmailProfile
        {
            get
            {
                return EditProfileButton;
            }
        }

        protected override string GetFromQueryString(string key, string messageOnError)
        {
            var queryStringValue = string.Empty;

            if (Request.QueryString.AllKeys.Any(k => k == key))
            {
                queryStringValue = Request.QueryString[key].ToString();
            }
            else
            {
                btnUpdateEmailProfile.Enabled = false;
                ShowMessageLabel(string.Format("<br>ERROR: {0}", messageOnError));
            }

            return queryStringValue;
        }

        protected string GetAction()
        {
            var action = string.Empty;

            try
            {
                action = Request.QueryString["action"].ToString();
            }
            catch
            {
                lblResultMessage.Enabled = false;
                lblResultMessage.Visible = false;
                action = "view";
            }

            return action;
        }

        public void UpdateEmail(object sender, EventArgs e)
        {
            SqlCommand updateEmailProfileCommand, updateEmailGroupsCommand;

            try
            {
                updateEmailProfileCommand = BuildUpdateEmailProfileSqlCommand();
                updateEmailGroupsCommand = BuildUpdateEmailGroupsSqlCommand();
            }
            catch (Exception ex)
            {
                var formattedErrorMessage = "ERROR: Error reading your Profile Information{0}{1}";
                System.Diagnostics.Trace.TraceError(formattedErrorMessage, '\n', ex.Message);
                ShowMessageLabel(string.Format(formattedErrorMessage, string.Empty, string.Empty));
                return;
            }

            try
            {
                var dbConnection = new SqlConnection(DataFunctions.connStr);

                updateEmailProfileCommand.Connection = dbConnection;
                updateEmailProfileCommand.CommandTimeout = 0;
                updateEmailProfileCommand.Connection.Open();
                updateEmailProfileCommand.ExecuteNonQuery();
                updateEmailProfileCommand.Connection.Close();

                updateEmailGroupsCommand.Connection = dbConnection;
                updateEmailGroupsCommand.CommandTimeout = 0;
                updateEmailGroupsCommand.Connection.Open();
                updateEmailGroupsCommand.ExecuteNonQuery();
                updateEmailGroupsCommand.Connection.Close();

                ShowMessageLabel("Successfully Updated your Profile");
            }
            catch (Exception ex)
            {
                var formattedErrorMessage = "ERROR: Error Occured Updating your Profile Information.{0}{1}";
                System.Diagnostics.Trace.TraceError(formattedErrorMessage, '\n', ex.Message);
                ShowMessageLabel(string.Format(formattedErrorMessage, string.Empty, string.Empty));
            }
        }

        private void PrepareUpdateEmailProfileCommandColumnParameterPairs(IDictionary<string, string> emailsTableColumnParameterPairs)
        {
            emailsTableColumnParameterPairs.Add(EmailAddressColumnName, EmailAddressParameter);
            emailsTableColumnParameterPairs.Add(TitleColumnName, TitleParameter);
            emailsTableColumnParameterPairs.Add(FirstNameColumnName, FirstNameParameter);
            emailsTableColumnParameterPairs.Add(LastNameColumnName, LastNameParameter);
            emailsTableColumnParameterPairs.Add(CompanyNameColumnName, CompanyNameParameter);
            emailsTableColumnParameterPairs.Add(OccupationColumnName, OccupationParameter);
            emailsTableColumnParameterPairs.Add(AddressColumnName, AddressParameter);
            emailsTableColumnParameterPairs.Add(Address2ColumnName, Address2Parameter);
            emailsTableColumnParameterPairs.Add(CityColumnName, CityParameter);
            emailsTableColumnParameterPairs.Add(StateItemColumnName, StateItemParameter);
            emailsTableColumnParameterPairs.Add(ZipColumnName, ZipParameter);
            emailsTableColumnParameterPairs.Add(CountryColumnName, CountryParameter);
            emailsTableColumnParameterPairs.Add(VoiceColumnName, VoiceParameter);
            emailsTableColumnParameterPairs.Add(MobileColumnName, MobileParameter);
            emailsTableColumnParameterPairs.Add(FaxColumnName, FaxParameter);
            emailsTableColumnParameterPairs.Add(WebsiteColumnName, WebsiteParameter);
            emailsTableColumnParameterPairs.Add(BirthDateColumnName, BirthDateParameter);
            emailsTableColumnParameterPairs.Add(AgeColumnName, AgeParameter);
            emailsTableColumnParameterPairs.Add(IncomeColumnName, IncomeParameter);
            emailsTableColumnParameterPairs.Add(GenderColumnName, GenderParameter);
            emailsTableColumnParameterPairs.Add(User1ColumnName, User1Parameter);
            emailsTableColumnParameterPairs.Add(User2ColumnName, User2Parameter);
            emailsTableColumnParameterPairs.Add(User3ColumnName, User3Parameter);
            emailsTableColumnParameterPairs.Add(User4ColumnName, User4Parameter);
            emailsTableColumnParameterPairs.Add(User5ColumnName, User5Parameter);
            emailsTableColumnParameterPairs.Add(User6ColumnName, User6Parameter);
            emailsTableColumnParameterPairs.Add(UserEvent1ColumnName, UserEvent1Parameter);
            emailsTableColumnParameterPairs.Add(UserEvent2ColumnName, UserEvent2Parameter);
            emailsTableColumnParameterPairs.Add(UserEvent1DateColumnName, UserEvent1DateParameter);
            emailsTableColumnParameterPairs.Add(UserEvent2DateColumnName, UserEvent2DateParameter);
            emailsTableColumnParameterPairs.Add(NotesColumnName, NotesParameter);
        }

        /// <summary>
        /// This method is called while building the UpdateEmailProfile Sql Command
        /// This method can be overridden to add any Column - @Parameter pairs that will be appended to SET statement into the UpdateEmailProfile Sql Command
        /// KeyValuePair's key is used for "ColumnName" and the value is used for "@parameterName"
        /// </summary>
        /// <param name="additionalColumnParameterPairs">The dictionary that holds Column - @Parameter pairs</param>
        protected virtual void AddColumnParameterPairsToUpdateEmailProfileCommand(IDictionary<string, string> additionalColumnParameterPairs)
        {
            // This method block is left blank intentionally
        }

        protected string BuildUpdateEmailProfileSqlCommandText()
        {
            var columnParameterPairs = new Dictionary<string, string>();
            PrepareUpdateEmailProfileCommandColumnParameterPairs(columnParameterPairs);

            var additionalColumnParameterPairs = new Dictionary<string, string>();
            AddColumnParameterPairsToUpdateEmailProfileCommand(additionalColumnParameterPairs);

            var emailsTableColParamPairs = columnParameterPairs.Concat(additionalColumnParameterPairs);

            var columnsBuilder = new StringBuilder();
            foreach (var colParamPair in emailsTableColParamPairs)
            {
                if (columnsBuilder.Length > 0)
                {
                    columnsBuilder.Append(',');
                }
                columnsBuilder.AppendFormat("{0}={1} ", colParamPair.Key, colParamPair.Value);
            }

            var commandTextBuilder = new StringBuilder();
            commandTextBuilder.Append("UPDATE Emails SET ");
            commandTextBuilder.Append(columnsBuilder.ToString());
            commandTextBuilder.AppendFormat("WHERE {0}={1}", EmailIDColumnName, EmailIDParameter);

            return commandTextBuilder.ToString();
        }

        protected SqlCommand BuildUpdateEmailProfileSqlCommand()
        {
            var updateProfileCommand = new SqlCommand();
            updateProfileCommand.CommandText = BuildUpdateEmailProfileSqlCommandText();

            SetUpdateEmailProfileCommandAdditionalParameters(updateProfileCommand.Parameters);
            SetUpdateEmailProfileCommandPersonalInformationParameters(updateProfileCommand.Parameters);
            SetUpdateEmailProfileCommandJobInformationParameters(updateProfileCommand.Parameters);
            SetUpdateEmailProfileCommandAddressParameters(updateProfileCommand.Parameters);
            SetUpdateEmailProfileCommandCommunicationParameters(updateProfileCommand.Parameters);
            SetUpdateEmailProfileCommandUserParameters(updateProfileCommand.Parameters);
            SetUpdateEmailProfileCommandUserEventParameters(updateProfileCommand.Parameters);
            SetUpdateEmailProfileCommandNotesParameters(updateProfileCommand.Parameters);
            SetUpdateEmailProfileCommandWhereClauseParameters(updateProfileCommand.Parameters);

            return updateProfileCommand;
        }

        private void PrepareUpdateEmailsGroupsCommandColumnParameterPairs(IDictionary<string, string> emailGroupsTableColumnParameterPairs)
        {
            emailGroupsTableColumnParameterPairs.Add(FormatTypeCodeColumnName, FormatTypeCodeParameter);
            emailGroupsTableColumnParameterPairs.Add(SubTypeCodeColumnName, SubTypeCodeParameter);
            emailGroupsTableColumnParameterPairs.Add(DateUpdatedColumnName, DateUpdatedParameter);
        }

        protected string BuildUpdateEmailGroupsSqlCommandText()
        {
            var columnParameterPairs = new Dictionary<string, string>();
            PrepareUpdateEmailsGroupsCommandColumnParameterPairs(columnParameterPairs);

            var columnsBuilder = new StringBuilder();
            foreach (var colParamPair in columnParameterPairs)
            {
                if (columnsBuilder.Length > 0)
                {
                    columnsBuilder.Append(',');
                }
                columnsBuilder.AppendFormat("{0}={1} ", colParamPair.Key, colParamPair.Value);
            }

            var commandTextBuilder = new StringBuilder();
            commandTextBuilder
                .Append("UPDATE EmailGroups SET ")
                .Append(columnsBuilder.ToString())
                .AppendFormat("WHERE EmailID={0} and GroupID={1}", EmailIDParameter, GroupIdParameter);

            return commandTextBuilder.ToString();
        }

        protected SqlCommand BuildUpdateEmailGroupsSqlCommand()
        {
            var updateEmailGroupsCommand = new SqlCommand();
            updateEmailGroupsCommand.CommandText = BuildUpdateEmailGroupsSqlCommandText();

            SetUpdateEmailGroupCommandParameters(updateEmailGroupsCommand.Parameters);
            SetUpdateEmailGroupCommandWhereClauseParameters(updateEmailGroupsCommand.Parameters);

            return updateEmailGroupsCommand;
        }

        /// <summary>
        /// This method is called while building the UpdateEmailProfile Sql Command
        /// This method can be overridden to add extra parameters to the UpdateEmailGroups Sql Command and to set their values
        /// </summary>
        /// <param name="sqlParameters">Extra parameters to be added to UpdateEmailProfile Sql Command</param>
        protected virtual void SetUpdateEmailProfileCommandAdditionalParameters(SqlParameterCollection sqlParameters)
        {
            // This method block is left blank intentionally
        }

        private void SetUpdateEmailProfileCommandWhereClauseParameters(SqlParameterCollection sqlParameters)
        {
            int emailId;
            int.TryParse(_emailId, out emailId);

            var emailIdParameter = sqlParameters.Contains(EmailIDParameter) ? sqlParameters[EmailIDParameter] : sqlParameters.Add(EmailIDParameter, SqlDbType.Int, 4);

            emailIdParameter.Value = emailId;
        }

        private void SetUpdateEmailProfileCommandPersonalInformationParameters(SqlParameterCollection sqlParameters)
        {
            var title = DataFunctions.CleanString(Title.Text);
            var firstName = DataFunctions.CleanString(FirstName.Text);
            var lastName = DataFunctions.CleanString(LastName.Text);
            var gender = DataFunctions.CleanString(Gender.SelectedValue.ToString());
            var age = DataFunctions.CleanString(Age.Text);
            var birthDate = ParseToSqlDateTime(BirthDate.Text);

            var titleParameter = sqlParameters.Contains(TitleParameter) ? sqlParameters[TitleParameter] : sqlParameters.Add(TitleParameter, SqlDbType.VarChar, 50);
            var firstNameParameter = sqlParameters.Contains(FirstNameParameter) ? sqlParameters[FirstNameParameter] : sqlParameters.Add(FirstNameParameter, SqlDbType.VarChar, 50);
            var lastNameParameter = sqlParameters.Contains(LastNameParameter) ? sqlParameters[LastNameParameter] : sqlParameters.Add(LastNameParameter, SqlDbType.VarChar, 50);
            var genderParameter = sqlParameters.Contains(GenderParameter) ? sqlParameters[GenderParameter] : sqlParameters.Add(GenderParameter, SqlDbType.VarChar, 50);
            var ageParameter = sqlParameters.Contains(AgeParameter) ? sqlParameters[AgeParameter] : sqlParameters.Add(AgeParameter, SqlDbType.VarChar, 50);
            var birthDateParameter = sqlParameters.Contains(BirthDateParameter) ? sqlParameters[BirthDateParameter] : sqlParameters.Add(BirthDateParameter, SqlDbType.DateTime);

            titleParameter.Value = title;
            firstNameParameter.Value = firstName;
            lastNameParameter.Value = lastName;
            genderParameter.Value = gender;
            ageParameter.Value = age;
            birthDateParameter.Value = birthDate;

            SetUpdateEmailProfileCommandPersonalInformationParameters(titleParameter, firstNameParameter, lastNameParameter, genderParameter, ageParameter, birthDateParameter);
        }

        /// <summary>
        /// This method is called while building the UpdateEmailProfile Sql Command
        /// This method can be overridden to change values of the parameters that are passed into it.
        /// </summary>
        /// <param name="titleParameter">Title parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="firstNameParameter">FirstName parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="lastNameParameter">LastName parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="genderParameter">Gender parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="ageParameter">Age parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="birthDateParameter">BirthDate parameter of the UpdateProfileEmail SqlCommand</param>
        protected virtual void SetUpdateEmailProfileCommandPersonalInformationParameters(SqlParameter titleParameter, SqlParameter firstNameParameter, SqlParameter lastNameParameter, SqlParameter genderParameter, SqlParameter ageParameter, SqlParameter birthDateParameter)
        {
            // This method block is left blank intentionally
        }

        private void SetUpdateEmailProfileCommandJobInformationParameters(SqlParameterCollection sqlParameters)
        {
            var company = DataFunctions.CleanString(CompanyName.Text);
            var occupation = DataFunctions.CleanString(Occupation.Text);
            var income = DataFunctions.CleanString(Income.Text);

            var companyParameter = sqlParameters.Contains(CompanyNameParameter) ? sqlParameters[CompanyNameParameter] : sqlParameters.Add(CompanyNameParameter, SqlDbType.VarChar, 50);
            var occupationParameter = sqlParameters.Contains(OccupationParameter) ? sqlParameters[OccupationParameter] : sqlParameters.Add(OccupationParameter, SqlDbType.VarChar, 50);
            var incomeParameter = sqlParameters.Contains(IncomeParameter) ? sqlParameters[IncomeParameter] : sqlParameters.Add(IncomeParameter, SqlDbType.VarChar, 50);

            companyParameter.Value = company;
            occupationParameter.Value = occupation;
            incomeParameter.Value = income;

            SetUpdateEmailProfileCommandJobInformationParameters(companyParameter, occupationParameter, incomeParameter);
        }

        /// <summary>
        /// This method is called while building the UpdateEmailProfile Sql Command
        /// This method can be overridden to change values of the parameters that are passed into it.
        /// </summary>
        /// <param name="companyParameter">Company parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="occupationParameter">Occupation parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="incomeParameter">Income parameter of the UpdateProfileEmail SqlCommand</param>
        protected virtual void SetUpdateEmailProfileCommandJobInformationParameters(SqlParameter companyParameter, SqlParameter occupationParameter, SqlParameter incomeParameter)
        {
            // This method block is left blank intentionally
        }

        private void SetUpdateEmailProfileCommandAddressParameters(SqlParameterCollection sqlParameters)
        {
            var address = DataFunctions.CleanString(Address.Text);
            var address2 = DataFunctions.CleanString(Address2.Text);
            var city = DataFunctions.CleanString(City.Text);
            var state = DataFunctions.CleanString(State.Text);
            var zip = DataFunctions.CleanString(Zip.Text);
            var country = DataFunctions.CleanString(Country.Text);

            var addressParameter = sqlParameters.Contains(AddressParameter) ? sqlParameters[AddressParameter] : sqlParameters.Add(AddressParameter, SqlDbType.VarChar, 255);
            var address2Parameter = sqlParameters.Contains(Address2Parameter) ? sqlParameters[Address2Parameter] : sqlParameters.Add(Address2Parameter, SqlDbType.VarChar, 255);
            var cityParameter = sqlParameters.Contains(CityParameter) ? sqlParameters[CityParameter] : sqlParameters.Add(CityParameter, SqlDbType.VarChar, 50);
            var stateParameter = sqlParameters.Contains(StateItemParameter) ? sqlParameters[StateItemParameter] : sqlParameters.Add(StateItemParameter, SqlDbType.VarChar, 50);
            var zipParameter = sqlParameters.Contains(ZipParameter) ? sqlParameters[ZipParameter] : sqlParameters.Add(ZipParameter, SqlDbType.VarChar, 50);
            var countryParameter = sqlParameters.Contains(CountryParameter) ? sqlParameters[CountryParameter] : sqlParameters.Add(CountryParameter, SqlDbType.VarChar, 50);

            addressParameter.Value = address;
            address2Parameter.Value = address2;
            cityParameter.Value = city;
            stateParameter.Value = state;
            zipParameter.Value = zip;
            countryParameter.Value = country;

            SetUpdateEmailProfileCommandAddressParameters(addressParameter, address2Parameter, cityParameter, stateParameter, zipParameter);
        }

        /// <summary>
        /// This method is called while building the UpdateEmailProfile Sql Command
        /// This method can be overridden to change values of the parameters that are passed into it.
        /// </summary>
        /// <param name="addressParameter">Address parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="address2Parameter">Address2 parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="cityParameter">City parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="stateParameter">State parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="zipParameter">Zip parameter of the UpdateProfileEmail SqlCommand</param>
        protected virtual void SetUpdateEmailProfileCommandAddressParameters(SqlParameter addressParameter, SqlParameter address2Parameter, SqlParameter cityParameter, SqlParameter stateParameter, SqlParameter zipParameter)
        {
            // This method block is left blank intentionally
        }

        private void SetUpdateEmailProfileCommandCommunicationParameters(SqlParameterCollection sqlParameters)
        {
            var emailAddress = DataFunctions.CleanString(EmailAddress.Text);
            var voice = DataFunctions.CleanString(Voice.Text);
            var mobile = DataFunctions.CleanString(Mobile.Text);
            var fax = DataFunctions.CleanString(Fax.Text);
            var website = DataFunctions.CleanString(Website.Text);

            var emailAddressParameter = sqlParameters.Contains(EmailAddressParameter) ? sqlParameters[EmailAddressParameter] : sqlParameters.Add(EmailAddressParameter, SqlDbType.VarChar, 250);
            var voiceParameter = sqlParameters.Contains(VoiceParameter) ? sqlParameters[VoiceParameter] : sqlParameters.Add(VoiceParameter, SqlDbType.VarChar, 50);
            var mobileParameter = sqlParameters.Contains(MobileParameter) ? sqlParameters[MobileParameter] : sqlParameters.Add(MobileParameter, SqlDbType.VarChar, 50);
            var faxParameter = sqlParameters.Contains(FaxParameter) ? sqlParameters[FaxParameter] : sqlParameters.Add(FaxParameter, SqlDbType.VarChar, 50);
            var websiteParameter = sqlParameters.Contains(WebsiteParameter) ? sqlParameters[WebsiteParameter] : sqlParameters.Add(WebsiteParameter, SqlDbType.VarChar, 50);

            emailAddressParameter.Value = emailAddress;
            voiceParameter.Value = voice;
            mobileParameter.Value = mobile;
            faxParameter.Value = fax;
            websiteParameter.Value = website;

            SetUpdateEmailProfileCommandCommunicationParameters(emailAddressParameter, voiceParameter, mobileParameter, faxParameter, websiteParameter);
        }

        /// <summary>
        /// This method is called while building the UpdateEmailProfile Sql Command
        /// This method can be overridden to change values of the parameters that are passed into it.
        /// </summary>
        /// <param name="emailAddressParameter">EmailAddress parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="voiceParameter">Voice parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="mobileParameter">Mobile parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="faxParameter">Fax parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="websiteParameter">Website parameter of the UpdateProfileEmail SqlCommand</param>
        protected virtual void SetUpdateEmailProfileCommandCommunicationParameters(SqlParameter emailAddressParameter, SqlParameter voiceParameter, SqlParameter mobileParameter, SqlParameter faxParameter, SqlParameter websiteParameter)
        {
            // This method block is left blank intentionally
        }

        private void SetUpdateEmailProfileCommandUserParameters(SqlParameterCollection sqlParameters)
        {
            var user1 = DataFunctions.CleanString(User1.Text);
            var user2 = DataFunctions.CleanString(User2.Text);
            var user3 = DataFunctions.CleanString(User3.Text);
            var user4 = DataFunctions.CleanString(User4.Text);
            var user5 = DataFunctions.CleanString(User5.Text);
            var user6 = DataFunctions.CleanString(User6.Text);

            var user1Parameter = sqlParameters.Contains(User1Parameter) ? sqlParameters[User1Parameter] : sqlParameters.Add(User1Parameter, SqlDbType.VarChar, 255);
            var user2Parameter = sqlParameters.Contains(User2Parameter) ? sqlParameters[User2Parameter] : sqlParameters.Add(User2Parameter, SqlDbType.VarChar, 255);
            var user3Parameter = sqlParameters.Contains(User3Parameter) ? sqlParameters[User3Parameter] : sqlParameters.Add(User3Parameter, SqlDbType.VarChar, 255);
            var user4Parameter = sqlParameters.Contains(User4Parameter) ? sqlParameters[User4Parameter] : sqlParameters.Add(User4Parameter, SqlDbType.VarChar, 255);
            var user5Parameter = sqlParameters.Contains(User5Parameter) ? sqlParameters[User5Parameter] : sqlParameters.Add(User5Parameter, SqlDbType.VarChar, 255);
            var user6Parameter = sqlParameters.Contains(User6Parameter) ? sqlParameters[User6Parameter] : sqlParameters.Add(User6Parameter, SqlDbType.VarChar, 255);

            user1Parameter.Value = user1;
            user2Parameter.Value = user2;
            user3Parameter.Value = user3;
            user4Parameter.Value = user4;
            user5Parameter.Value = user5;
            user6Parameter.Value = user6;

            SetUpdateEmailProfileCommandUserParameters(user1Parameter, user2Parameter, user3Parameter, user4Parameter, user5Parameter, user6Parameter);
        }

        /// <summary>
        /// This method is called while building the UpdateEmailProfile Sql Command
        /// This method can be overridden to change values of the parameters that are passed into it.
        /// </summary>
        /// <param name="user6Parameter">User1 parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="user1Parameter">User2 parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="user2Parameter">User3 parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="user3Parameter">User4 parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="user4Parameter">User5 parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="user5Parameter">User6 parameter of the UpdateProfileEmail SqlCommand</param>
        protected virtual void SetUpdateEmailProfileCommandUserParameters(SqlParameter user1Parameter, SqlParameter user2Parameter, SqlParameter user3Parameter, SqlParameter user4Parameter, SqlParameter user5Parameter, SqlParameter user6Parameter)
        {
            // This method block is left blank intentionally
        }

        private void SetUpdateEmailProfileCommandUserEventParameters(SqlParameterCollection sqlParameters)
        {
            var userEvent1 = DataFunctions.CleanString(UserEvent1.Text);
            var userEvent2 = DataFunctions.CleanString(UserEvent2.Text);
            var userEvent1Date = ParseToSqlDateTime(UserEvent1Date.Text);
            var userEvent2Date = ParseToSqlDateTime(UserEvent2Date.Text);

            var userEvent1Parameter = sqlParameters.Contains(UserEvent1Parameter) ? sqlParameters[UserEvent1Parameter] : sqlParameters.Add(UserEvent1Parameter, SqlDbType.VarChar, 50);
            var userEvent2Parameter = sqlParameters.Contains(UserEvent2Parameter) ? sqlParameters[UserEvent2Parameter] : sqlParameters.Add(UserEvent2Parameter, SqlDbType.VarChar, 50);
            var userEvent1DateParameter = sqlParameters.Contains(UserEvent1DateParameter) ? sqlParameters[UserEvent1DateParameter] : sqlParameters.Add(UserEvent1DateParameter, SqlDbType.DateTime);
            var userEvent2DateParameter = sqlParameters.Contains(UserEvent2DateParameter) ? sqlParameters[UserEvent2DateParameter] : sqlParameters.Add(UserEvent2DateParameter, SqlDbType.DateTime);

            userEvent1Parameter.Value = userEvent1;
            userEvent2Parameter.Value = userEvent2;
            userEvent1DateParameter.Value = userEvent1Date;
            userEvent2DateParameter.Value = userEvent2Date;

            SetUpdateEmailProfileCommandUserEventParameters(userEvent1Parameter, userEvent1DateParameter, userEvent2Parameter, userEvent2DateParameter);
        }

        /// <summary>
        /// This method is called while building the UpdateEmailProfile Sql Command
        /// This method can be overridden to change values of the parameters that are passed into it.
        /// </summary>
        /// <param name="userEvent1Parameter">UserEvent1 parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="userEvent1DateParameter">UserEvent1Date parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="userEvent2Parameter">UserEvent2 parameter of the UpdateProfileEmail SqlCommand</param>
        /// <param name="userEvent2DateParameter">UserEvent2Date parameter of the UpdateProfileEmail SqlCommand</param>
        protected virtual void SetUpdateEmailProfileCommandUserEventParameters(SqlParameter userEvent1Parameter, SqlParameter userEvent1DateParameter, SqlParameter userEvent2Parameter, SqlParameter userEvent2DateParameter)
        {
            // This method block is left blank intentionally
        }

        private void SetUpdateEmailProfileCommandNotesParameters(SqlParameterCollection sqlParameters)
        {
            var notes = string.Format("\n------------ NOTES UPDATED: {0}  -------------", DateTime.Now);

            var notesParameter = sqlParameters.Contains(NotesParameter) ? sqlParameters[NotesParameter] : sqlParameters.Add(NotesParameter, SqlDbType.Text);
            notesParameter.Value = notes;

            SetUpdateEmailProfileCommandNotesParameters(notesParameter);
        }

        /// <summary>
        /// This method is called while building the UpdateEmailProfile Sql Command
        /// This method can be overridden to change values of the parameters that are passed into it.
        /// </summary>
        /// <param name="notesParameter">Notes parameter of the UpdateProfileEmail SqlCommand</param>
        protected virtual void SetUpdateEmailProfileCommandNotesParameters(SqlParameter notesParameter)
        {
            // This method block is left blank intentionally
        }

        private void SetUpdateEmailGroupCommandWhereClauseParameters(SqlParameterCollection sqlParameters)
        {
            int groupId;
            int.TryParse(_groupId, out groupId);
            int emailId;
            int.TryParse(_emailId, out emailId);

            var emailIdParameter = sqlParameters.Contains(EmailIDParameter) ? sqlParameters[EmailIDParameter] : sqlParameters.Add(EmailIDParameter, SqlDbType.Int, 4);
            var groupIdParameter = sqlParameters.Contains(GroupIdParameter) ? sqlParameters[GroupIdParameter] : sqlParameters.Add(GroupIdParameter, SqlDbType.Int, 4);

            emailIdParameter.Value = emailId;
            groupIdParameter.Value = groupId;
        }

        private void SetUpdateEmailGroupCommandParameters(SqlParameterCollection sqlParameters)
        {
            var subTypeCode = "S";
            var formatTypeCode = string.Empty;

            var formatTypeCodeParameter = sqlParameters.Contains(FormatTypeCodeParameter) ? sqlParameters[FormatTypeCodeParameter] : sqlParameters.Add(FormatTypeCodeParameter, SqlDbType.VarChar, 50);
            var subTypeCodeParameter = sqlParameters.Contains(SubTypeCodeParameter) ? sqlParameters[SubTypeCodeParameter] : sqlParameters.Add(SubTypeCodeParameter, SqlDbType.VarChar, 50);
            var dateUpdatedParameter = sqlParameters.Contains(DateUpdatedParameter) ? sqlParameters[DateUpdatedParameter] : sqlParameters.Add(DateUpdatedParameter, SqlDbType.DateTime);

            formatTypeCodeParameter.Value = formatTypeCode;
            subTypeCodeParameter.Value = subTypeCode;
            dateUpdatedParameter.Value = DateTime.Now;

            SetUpdateEmailGroupCommandParameters(formatTypeCodeParameter, subTypeCodeParameter);
        }

        /// <summary>
        /// This method is called while building the UpdateEmailGroups Sql Command
        /// This method can be overridden to change values of the parameters that are passed into it.
        /// </summary>
        /// <param name="formatTypeCodeParameter">FormatTypeCode parameter of the UpdateEmailGroups SqlCommand</param>
        /// <param name="subTypeCodeParameter">SubscribeTypeCode parameter of the UpdateEmailGroups SqlCommand</param>
        protected virtual void SetUpdateEmailGroupCommandParameters(SqlParameter formatTypeCodeParameter, SqlParameter subTypeCodeParameter)
        {
            // This method block is left blank intentionally
        }

        protected void FillFormFromDataRow(DataRow dataRow)
        {
            EmailAddress.Text = dataRow[EmailAddressColumnName].ToString();
            Title.Text = dataRow[TitleColumnName].ToString();
            FirstName.Text = dataRow[FirstNameColumnName].ToString();
            LastName.Text = dataRow[LastNameColumnName].ToString();
            CompanyName.Text = dataRow[CompanyNameColumnName].ToString();
            Occupation.Text = dataRow[OccupationColumnName].ToString();
            Address.Text = dataRow[AddressColumnName].ToString();
            Address2.Text = dataRow[Address2ColumnName].ToString();
            City.Text = dataRow[CityColumnName].ToString();

            try
            {
                var stateItem = dataRow[StateItemColumnName].ToString().ToUpper();
                State.Items.FindByValue(stateItem).Selected = true;
            }
            catch
            {
                State.Items[0].Selected = true;
            }

            Zip.Text = dataRow[ZipColumnName].ToString();
            Country.Text = dataRow[CountryColumnName].ToString();
            Voice.Text = dataRow[VoiceColumnName].ToString();
            Mobile.Text = dataRow[MobileColumnName].ToString();
            Fax.Text = dataRow[FaxColumnName].ToString();
            Website.Text = dataRow[WebsiteColumnName].ToString();
            Age.Text = dataRow[AgeColumnName].ToString();
            Income.Text = dataRow[IncomeColumnName].ToString();

            try
            {
                var gender = dataRow[GenderColumnName].ToString();
                Gender.Items.FindByValue(gender).Selected = true;
            }
            catch
            {
                Gender.Items[0].Selected = true;
            }

            User1.Text = dataRow[User1ColumnName].ToString();
            User2.Text = dataRow[User2ColumnName].ToString();
            User3.Text = dataRow[User3ColumnName].ToString();
            User4.Text = dataRow[User4ColumnName].ToString();
            User5.Text = dataRow[User5ColumnName].ToString();
            User6.Text = dataRow[User6ColumnName].ToString();
            UserEvent1.Text = dataRow[UserEvent1ColumnName].ToString();
            UserEvent2.Text = dataRow[UserEvent2ColumnName].ToString();
            UserEvent1Date.Text = ConvertToFormattedDateString(dataRow[UserEvent1DateColumnName]);
            UserEvent2Date.Text = ConvertToFormattedDateString(dataRow[UserEvent2DateColumnName]);
            BirthDate.Text = ConvertToFormattedDateString(dataRow[BirthDateColumnName]);
        }

        private string ConvertToFormattedDateString(object dataCellValue)
        {
            var formattedDateString = string.Empty;
            var dateTimeString = dataCellValue.ToString();

            if (!Convert.IsDBNull(dateTimeString) && dateTimeString.Length > 0)
            {
                DateTime dateTime;
                DateTime.TryParse(dateTimeString, out dateTime);

                if (dateTime.Year > _minYearValue && dateTime.Year < _maxYearValue)
                {
                    formattedDateString = dateTime.ToString("MM/dd/yyyy");
                }
            }

            return formattedDateString;
        }

        protected SqlDateTime ParseToSqlDateTime(string dateTimeString)
        {
            DateTime dateTime;
            return DateTime.TryParse(dateTimeString, out dateTime) ? new SqlDateTime(dateTime) : SqlDateTime.Null;
        }
    }
}