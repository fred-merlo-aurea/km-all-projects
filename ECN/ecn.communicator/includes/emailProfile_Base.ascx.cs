using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.common.classes;
using KM.Framework.Web.WebForms.EmailProfile;

namespace ecn.communicator.includes
{
    public partial class emailProfile_base : EmailProfileBaseBaseControl
    {
        private const string FullNameColumnName = "FullName";
        private const string FullNameParameterName = "@full_name";

        private string _action = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            _emailId = GetFromQueryString("eID", "EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received");
            _emailAddress= GetFromQueryString("eAD", "EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received");
            _groupId = GetFromQueryString("gID", "EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received");
            _action = GetAction();

			if(!(Page.IsPostBack))
            {
				if((_emailAddress.Length > 0) && (_groupId.Length > 0) && (_emailId.Length > 0))
                {
					if(_action.Equals("edit"))
                    {
						EditProfileButton.Enabled = true;
						EditProfileButton.Visible = true;
						FieldsValidationPanel.Visible = true;
					}
                    else
                    {
						FieldsValidationPanel.Visible = false;
						EditProfileButton.Enabled = false;
						EditProfileButton.Visible = false;
					}
					LoadProfileData(_emailId, _emailAddress, _groupId);
				}
			}
		}

		#region load States DR
		private void LoadUSStatesDR(){
			State.Items.Add(new ListItem("--", ""));			
			State.Items.Add(new ListItem("AL", "AL"));
			State.Items.Add(new ListItem("AK", "AK"));
			State.Items.Add(new ListItem("AZ", "AZ"));
			State.Items.Add(new ListItem("AR", "AR"));
			State.Items.Add(new ListItem("CA", "CA"));
			State.Items.Add(new ListItem("CO", "CO"));
			State.Items.Add(new ListItem("CT", "CT"));
			State.Items.Add(new ListItem("DE", "DE"));
			State.Items.Add(new ListItem("DC", "DC"));
			State.Items.Add(new ListItem("FL", "FL"));
			State.Items.Add(new ListItem("GA", "GA"));
			State.Items.Add(new ListItem("GU", "GU"));
			State.Items.Add(new ListItem("HI", "HI"));
			State.Items.Add(new ListItem("ID", "ID"));
			State.Items.Add(new ListItem("IL", "IL"));
			State.Items.Add(new ListItem("IN", "IN"));
			State.Items.Add(new ListItem("IA", "IA"));
			State.Items.Add(new ListItem("KS", "KS"));
			State.Items.Add(new ListItem("KY", "KY"));
			State.Items.Add(new ListItem("LA", "LA"));
			State.Items.Add(new ListItem("ME", "ME"));
			State.Items.Add(new ListItem("MD", "MD"));
			State.Items.Add(new ListItem("MA", "MA"));
			State.Items.Add(new ListItem("MI", "MI"));
			State.Items.Add(new ListItem("MN", "MN"));
			State.Items.Add(new ListItem("MS", "MS"));
			State.Items.Add(new ListItem("MO", "MO"));
			State.Items.Add(new ListItem("MT", "MT"));
			State.Items.Add(new ListItem("NE", "NE"));
			State.Items.Add(new ListItem("NV", "NV"));
			State.Items.Add(new ListItem("NH", "NH"));
			State.Items.Add(new ListItem("NJ", "NJ"));
			State.Items.Add(new ListItem("NM", "NM"));
			State.Items.Add(new ListItem("NY", "NY"));
			State.Items.Add(new ListItem("NC", "NC"));
			State.Items.Add(new ListItem("ND", "ND"));
			State.Items.Add(new ListItem("OH", "OH"));
			State.Items.Add(new ListItem("OK", "OK"));
			State.Items.Add(new ListItem("OR", "OR"));
			State.Items.Add(new ListItem("PA", "PA"));
			State.Items.Add(new ListItem("RI", "RI"));
			State.Items.Add(new ListItem("SC", "SC"));
			State.Items.Add(new ListItem("SD", "SD"));
			State.Items.Add(new ListItem("TN", "TN"));
			State.Items.Add(new ListItem("TX", "TX"));
			State.Items.Add(new ListItem("VI", "VI"));
			State.Items.Add(new ListItem("UT", "UT"));
			State.Items.Add(new ListItem("VT", "VT"));
			State.Items.Add(new ListItem("VA", "VA"));
			State.Items.Add(new ListItem("WA", "WA"));
			State.Items.Add(new ListItem("WV", "WV"));
			State.Items.Add(new ListItem("WI", "WI"));
			State.Items.Add(new ListItem("WY", "WY"));
		}
		#endregion

		private void LoadProfileData(string setEmailID, string setEmailAddress, string setGroupID) {
            var sqlQueryBuilder = new StringBuilder();
            sqlQueryBuilder.AppendFormat("SELECT e.*, eg.{0}, eg.{1} ", FormatTypeCodeColumnName, SubTypeCodeColumnName);
            sqlQueryBuilder.Append("FROM Emails e JOIN EmailGroups eg ON ");
            sqlQueryBuilder.AppendFormat("eg.{0}=e.{0} AND e.{0}={1} AND eg.{2}={3}", EmailIDColumnName, setEmailID, GroupIdColumnName, setGroupID);

            var sqlQuery = sqlQueryBuilder.ToString();
            var dataTable = DataFunctions.GetDataTable(sqlQuery);
			LoadUSStatesDR();
			foreach (DataRow dataRow in dataTable.Rows) {

                FillFormFromDataRow(dataRow);

				FullName.Text = dataRow[FullNameColumnName].ToString();
				Notes.Text = dataRow[NotesColumnName].ToString();
				FormatTypeCode.Items.FindByValue(dataRow[FormatTypeCodeColumnName].ToString()).Selected = true;
			}
		}

        protected override void AddColumnParameterPairsToUpdateEmailProfileCommand(IDictionary<string, string> additionalColumnParameterPairs)
        {
            base.AddColumnParameterPairsToUpdateEmailProfileCommand(additionalColumnParameterPairs);

            additionalColumnParameterPairs.Add(FullNameColumnName, FullNameParameterName);
        }

        protected override void SetUpdateEmailProfileCommandAdditionalParameters(SqlParameterCollection sqlParameters)
        {
            base.SetUpdateEmailProfileCommandAdditionalParameters(sqlParameters);

            var fullName = DataFunctions.CleanString(FullName.Text);
            var fullNameParameter =
                sqlParameters.Contains(FullNameParameterName) ? sqlParameters[FullNameParameterName] : sqlParameters.Add(FullNameParameterName, SqlDbType.VarChar, 50);

            fullNameParameter.Value = fullName;
        }

        protected override void SetUpdateEmailGroupCommandParameters(SqlParameter formatTypeCodeParameter, SqlParameter subTypeCodeParameter)
        {
            base.SetUpdateEmailGroupCommandParameters(formatTypeCodeParameter, subTypeCodeParameter);

            formatTypeCodeParameter.Value = FormatTypeCode.SelectedValue;
        }

        protected override void SetUpdateEmailProfileCommandNotesParameters(SqlParameter notesParameter)
        {
            var notesBuilder = new StringBuilder();
            notesBuilder.Append(Notes.Text);
            notesBuilder.AppendFormat("\n------------ NOTES UPDATED: {0}  -------------", DateTime.Now);

            notesParameter.Value = notesBuilder.ToString();
        }

        override protected void OnInit(EventArgs e) {
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
				
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent() {    

		}
	}
}