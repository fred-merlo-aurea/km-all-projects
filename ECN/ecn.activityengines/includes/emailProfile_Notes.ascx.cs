using System;
using System.Data;
using System.Web.UI.WebControls;
using ecn.common.classes;
using KM.Framework.Web.WebForms.EmailProfile;

namespace ecn.activityengines.includes {
    public partial class emailProfile_Notes : EmailProfileBaseControl
    {
        private string _emailID = string.Empty;

        protected override Label lblResultMessage
        {
            get
            {
                return this.messageLabel;
            }
        }

        protected void Page_Load( object sender, EventArgs e ) {
            _emailID = GetFromQueryString("eID", "EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received");

            var query = string.Format("SELECT Notes FROM Emails WHERE EmailID = {0}", _emailID);
            profileNotes.Text = DataFunctions.ExecuteScalar(query).ToString();
        }
    }
}