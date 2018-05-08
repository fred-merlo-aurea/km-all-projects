using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using ecn.common.classes;
using KM.Framework.Web.WebForms.EmailProfile;

namespace ecn.activityengines.includes
{
    public partial class emailProfile_UDFHistory : EmailProfileUDFHistoryBaseControl
    {
        protected override Label lblResultMessage
        {
            get
            {
                return MessageLabel;
            }
        }

        protected override DataGrid gridUDFHistoryData
        {
            get
            {
                return UDFHistoryGrid;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var emailId = GetFromQueryString("eID", "EmailID specified does not Exist. Please click on the 'Profile' link in the email message that you received");
            var groupId = GetFromQueryString("gID", "GroupID specified does not Exist. Please click on the 'Profile' link in the email message that you received");
            var dataFieldSetId = GetFromQueryString("dfsID", "DataFieldSetID specified does not Exist. Please click on the 'Profile' link in the email message that you received");
            var customerId = GetCustomerId(groupId);
            var dataFieldSetName = GetDataFieldSetName(dataFieldSetId);

            UDFHistoryNameLabel.Text = string.Format("{0} Details", dataFieldSetName);
            LoadUDFHistoryData(emailId, groupId, dataFieldSetId, customerId);
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
