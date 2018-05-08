using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.communicator.classes;
using ecn.common.classes;
using KM.Framework.Web.WebForms.EmailProfile;

namespace ecn.communicator.includes
{	
	public partial class emailProfile_UDF : EmailProfileBaseControl
    {
        private string _emailId = string.Empty;
        private string _groupId = string.Empty;

        protected override Label lblResultMessage
        {
            get
            {
                return this.MessageLabel;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _emailId = GetFromQueryString("eID", "EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received");
            _groupId = GetFromQueryString("gID", "EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received");

			LoadStandAloneUDFGrid(_emailId, _groupId);
		}

		#region StandAloneUDFGrid Edit / Cancel / Update Functions
		public void StandAloneUDFGrid_Edit(Object sender, DataGridCommandEventArgs e)  {

			// Set the EditItemIndex property to the index of the item clicked 
			// in the DataGrid control to enable editing for that item. Be sure
			// to rebind the DateGrid to the data source to refresh the control.
			string gdfID = Request.QueryString["GDFID"];
			StandAloneUDFGrid.EditItemIndex = e.Item.ItemIndex;
			LoadStandAloneUDFGrid(_emailId,_groupId);

		}
		public void StandAloneUDFGrid_Cancel(Object sender, DataGridCommandEventArgs e) {
			// Set the EditItemIndex property to -1 to exit editing mode. 
			// Be sure to rebind the DateGrid to the data source to refresh
			// the control.
			string gdfID = Request.QueryString["GDFID"];
			StandAloneUDFGrid.EditItemIndex = -1;
			LoadStandAloneUDFGrid(_emailId,_groupId);
		}
		public void StandAloneUDFGrid_Command(Object sender, DataGridCommandEventArgs e){
			switch(((LinkButton)e.CommandSource).CommandName){
				case "Delete":
					//                    DeleteItem(e);
					break;
					// Add other cases here, if there are multiple ButtonColumns in 
					// the DataGrid control.
				default:
					// Do nothing.
					break;
			}
		}
		public void StandAloneUDFGrid_Update(Object sender, DataGridCommandEventArgs e)  {
			// Get the correct value and update it from the field in value 0
			//         throw new System.Exception( " event = " + EmailsGrid.EditItemIndex.ToString());
			string email_data_id = StandAloneUDFGrid.DataKeys[StandAloneUDFGrid.EditItemIndex].ToString();
			TextBox valueText = (TextBox)e.Item.Cells[1].Controls[0];
			ECN_Framework.Common.SecurityAccess.canI("EmailDataValues",email_data_id);
			DataFunctions.Execute("update EmailDataValues set DataValue = '" + valueText.Text + "' where EmailDataValuesID= " + email_data_id);
			StandAloneUDFGrid.EditItemIndex = -1;
			LoadStandAloneUDFGrid(_emailId,_groupId);
		}
		#endregion

		private void LoadStandAloneUDFGrid(string EmailID, string group_id) {
			//ECN_Framework.Common.SecurityAccess.canI("Emails",EmailID);
			string sqlemailsquery=
				" SELECT e.EmailDataValuesID, g.ShortName, g.LongName, e.DataValue FROM "+
                " GroupDatafields g JOIN EmailDataValues e g.GroupDatafieldsID = e.GroupDatafieldsID " +
				" WHERE g.GroupID= "+group_id+" AND e.EmailID="+EmailID+
				" AND DataFieldSetID is null and surveyID is null ";
			DataTable emailstable = DataFunctions.GetDataTable(sqlemailsquery);
			if(emailstable.Rows.Count > 0){
				StandAloneUDFGrid.DataSource=emailstable.DefaultView;
				StandAloneUDFGrid.DataBind();
			}else{
				StandAloneUDFGrid.Visible = false;
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
