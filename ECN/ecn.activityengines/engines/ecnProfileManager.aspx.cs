using System;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using ecn.communicator.classes;
using ecn.common.classes;

namespace ecn.activityengines.engines {

    public partial class ecnProfileManager : System.Web.UI.Page {

        private string currentControl {
            get { return ViewState["currentControl"].ToString(); }
            set { ViewState["currentControl"] = value; }
        }

        protected void Page_Load( object sender, EventArgs e ) {
            messageLabel.Visible = false;
            headerLabel.Text = "";
            if(!(Page.IsPostBack)) {
                Control emailProfileControl = LoadControl("../includes/emailProfile_Base.ascx");
                ecnProfileControlPanel.Controls.Add(emailProfileControl);
                ecnProfileControlPanel.Visible = true;
                btnProfileDetails.CssClass = "selected";
                currentControl = "../includes/emailProfile_Base.ascx";

                loadProfilePageHeader();
            } else {
                Control emailProfileControl = LoadControl(currentControl);
                ecnProfileControlPanel.Controls.Add(emailProfileControl);
                ecnProfileControlPanel.Visible = true;

                if(Session["ECNPROFILE_INFO_HDR"].ToString().Length > 10) {
                    try {
                        headerLabel.Text = Session["ECNPROFILE_INFO_HDR"].ToString();
                    } catch {
                        loadProfilePageHeader();
                    }
                } else {
                    loadProfilePageHeader();
                }
            }
        }

        #region getEmailID, getEmailAddress, getGroupID, getAction, getPanelToShow
        private string getEmailID() {
            string theEmailID = "";
            try {
                theEmailID = Request.QueryString["eID"].ToString();
            } catch {
                messageLabel.Visible = true;
                messageLabel.Text = "<br>ERROR: EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received";
            }
            return theEmailID;
        }

        private string getEmailAddress() {
            string theEmailAddress = "";
            try {
                theEmailAddress = Request.QueryString["eAD"].ToString();
            } catch {
                messageLabel.Visible = true;
                messageLabel.Text = "<br>ERROR: EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received";
            }
            return theEmailAddress;
        }

        private string getPanelToShow() {
            string thePanel = "";
            try {
                thePanel = Request.QueryString["panel"].ToString();
            } catch {
            }
            return thePanel;
        }
        #endregion

        #region reset Styles on all the LinkButtons
        protected void resetStyles() { 
            btnProfileDetails.CssClass = "";
            btnListSubs.CssClass = "";
            btnUDF.CssClass = "";
            btnCampaigns.CssClass = "";
            btnSurveys.CssClass = "";
            btnDEs.CssClass = "";
            btnNotes.CssClass = "";
        }
        #endregion

        #region Load Profile Header info
        protected void loadProfilePageHeader() {
            string hdrStart = "<Table border=0 cellpadding=1>", hdrEnd="</Table>";
            headerLabel.Text += "<tr><td><b>Email Address:</b> " + getEmailAddress() + "</td><td>&nbsp;&nbsp;</td>";
            DataTable dt = DataFunctions.GetDataTable("SELECT (FirstName+' '+LastName) AS 'ProfileName', Company, Voice FROM Emails WHERE EmailID = " + getEmailID());
            if(dt.Rows.Count > 0) {
                headerLabel.Text += "<td><b>Name:</b> " + dt.Rows[0]["ProfileName"].ToString() + "</td></tr>";
                headerLabel.Text += "<tr><td><b>Company:</b> " + dt.Rows[0]["Company"].ToString() + "</td><td>&nbsp;&nbsp;</td>";
                headerLabel.Text += "<td><b>Phone#:</b> " + dt.Rows[0]["Voice"].ToString() + "</td></tr>";
                
            }

            headerLabel.Text = hdrStart + headerLabel.Text + hdrEnd;
            Session["ECNPROFILE_INFO_HDR"] = headerLabel.Text.ToString();
        }
        #endregion
        #region Link Button Click Events
        protected void btnProfileDetails_Click( object sender, EventArgs e ) {
            currentControl = "../includes/emailProfile_Base.ascx";
            resetControlPanel();
            Control emailProfileControl = (Control) LoadControl("../includes/emailProfile_Base.ascx");
            ecnProfileControlPanel.Controls.Add(emailProfileControl);
            ecnProfileControlPanel.Visible = true;
            btnProfileDetails.CssClass = "selected";
        }

        protected void btnListSubs_Click( object sender, EventArgs e ) {
            currentControl = "../includes/emailProfile_ListSubscriptions.ascx";
            resetControlPanel();
            Control emailProfileControl = LoadControl("../includes/emailProfile_ListSubscriptions.ascx");
            ecnProfileControlPanel.Controls.Add(emailProfileControl);

            ecnProfileControlPanel.Visible = true;
            btnListSubs.CssClass = "selected";
        }

        protected void btnUDF_Click( object sender, EventArgs e ) {
            currentControl = "../includes/emailProfile_UDF.ascx";
            resetControlPanel();
            Control emailProfileControl = LoadControl("../includes/emailProfile_UDF.ascx");
            ecnProfileControlPanel.Controls.Add(emailProfileControl);

            ecnProfileControlPanel.Visible = true;
            btnUDF.CssClass = "selected";
        }

        protected void btnCampaigns_Click( object sender, EventArgs e ) {
            currentControl = "../includes/emailProfile_EmailActivity.ascx";
            resetControlPanel();
            Control emailProfileControl = LoadControl("../includes/emailProfile_EmailActivity.ascx");
            ecnProfileControlPanel.Controls.Add(emailProfileControl);
            ecnProfileControlPanel.Visible = true;
            btnCampaigns.CssClass = "selected";
        }

        protected void btnSurveys_Click( object sender, EventArgs e ) {
            currentControl = "../includes/emailProfile_Survey.ascx";
            //currentControl = "webform1.ascx";
            resetControlPanel();
            Control emailProfileControl = LoadControl("../includes/emailProfile_Survey.ascx");
            //Control emailProfileControl = LoadControl("webform1.ascx");
            ecnProfileControlPanel.Controls.Add(emailProfileControl);
            ecnProfileControlPanel.Visible = true;
            btnSurveys.CssClass = "selected";
        }

        protected void btnDEs_Click( object sender, EventArgs e ) {
            currentControl = "../includes/emailProfile_DigitalEdition.ascx";
            resetControlPanel();
            Control emailProfileControl = LoadControl("../includes/emailProfile_DigitalEdition.ascx");
            ecnProfileControlPanel.Controls.Add(emailProfileControl);
            ecnProfileControlPanel.Visible = true;
            btnDEs.CssClass = "selected";
            
        }

        protected void btnNotes_Click( object sender, EventArgs e ) {
            currentControl = "../includes/emailProfile_Notes.ascx";
            resetControlPanel();
            Control emailProfileControl = LoadControl("../includes/emailProfile_Notes.ascx");
            ecnProfileControlPanel.Controls.Add(emailProfileControl);
            ecnProfileControlPanel.Visible = true;
            btnNotes.CssClass = "selected";
        }

        protected void resetControlPanel(){
            ecnProfileControlPanel.Controls.Clear();
            resetStyles();        
        }
        #endregion
    }
}
