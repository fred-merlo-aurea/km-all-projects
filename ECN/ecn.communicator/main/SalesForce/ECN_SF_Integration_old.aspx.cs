using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.communicator.main.Salesforce;

namespace ecn.communicator.main.Salesforce
{
    public partial class ECN_SF_Integration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // code   ---  auth code the consumer must use to obtain the access and refresh tokens
            // state  ---  the state value that was passed in as part of the initial request if applicable
            if (Page.IsPostBack == false)
            {

                if (Request.QueryString["code"] != null && !string.IsNullOrEmpty(Request.QueryString["code"]))
                {
                    Entity.SF_Authentication.AuthCode = Request.QueryString["code"].ToString();
                    Entity.SF_Authentication.SetSalesForceToken();
                    if (Entity.SF_Authentication.LoginAttempted == true && Entity.SF_Authentication.LoggedIn == true)
                    {

                        kmMsg.Show("Successfully authenticated with Salesforce", "Salesforce Login", Salesforce.Controls.Message.Message_Icon.info);
                    }
                    try
                    {
                        //List<SF_Lead> leadList = SF_Lead.GetAll(SF_Authentication.Token.access_token).ToList();
                        //List<SF_Account> acctList = SF_Account.GetAll(SF_Authentication.Token.access_token).ToList();
                        //List<SF_Contact> conList = SF_Contact.GetAll(SF_Authentication.Token.access_token).ToList();
                    }
                    catch (Exception ex)
                    {
                        Entity.SF_Utilities.LogException(ex);
                    }
                }
                else
                {
                    //show a message that user did not successfully authenticate to SalesForce
                    if (Entity.SF_Authentication.LoginAttempted == true && Entity.SF_Authentication.LoggedIn == false)
                        kmMsg.Show("Authentication with Salesforce was not successful", "Salesforce Login", Salesforce.Controls.Message.Message_Icon.error);
                    //else if (Entity.SF_Authentication.LoginAttempted == false)
                    //    kmMsg.Show("Click the Salesforce Log In button to access your Salesforce data", "Salesforce Login", Salesforce.Controls.Message.Message_Icon.info);
                }
            }

            pnlHome.Visible = true;
            pnlImport.Visible = false;
            pnlInstructions.Visible = false;
            pnlSync.Visible = false;
            lblImportHeading.Visible = false;
            lblSyncHeading.Visible = false;

        }
        protected void btnSF_Login_Click(object sender, EventArgs e)
        {
            //Response.Redirect(SalesForce.SF_Pages.SF_Authentication.SF_Login());
            Salesforce.Entity.SF_Authentication.SF_Login(Master.UserSession.CurrentBaseChannel.BaseChannelID);
        }

        #region Button Redirects
        protected void btnImportSFCampaign_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/main/Salesforce/SF_Pages/SF_CampaignGroup.aspx");
        }

        protected void btnExportECNActivity_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/main/Salesforce/SF_Pages/SF_CampaignActivity.aspx");
        }

        protected void btnSyncContacts_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/main/Salesforce/SF_Pages/SF_Contacts.aspx");
        }

        protected void btnSyncLeads_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/main/Salesforce/SF_Pages/SF_Leads.aspx");
        }

        protected void btnSyncAccounts_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/main/Salesforce/SF_Pages/SF_Accounts.aspx");
        }

        protected void btnSyncSuppression_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/main/Salesforce/SF_Pages/SF_Suppression.aspx");
        }

        protected void btnSyncOptOut_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/main/Salesforce/SF_Pages/SF_OptOut.aspx");
        }
        #endregion


        protected void btnSyncHome_Click(object sender, EventArgs e)
        {
            pnlImport.Visible = false;
            pnlInstructions.Visible = false;
            pnlSync.Visible = true;
            pnlHome.Visible = false;
            lblSyncHeading.Visible = true;
            lblImportHeading.Visible = false;
            if (Master.UserSession.CurrentUser.CustomerID != 1)
            {
                btnSyncAccounts.Visible = false;
                imgSyncAccounts.Visible = false;
            }
        }

        protected void btnImportHome_Click(object sender, EventArgs e)
        {
            pnlImport.Visible = true;
            pnlInstructions.Visible = false;
            pnlSync.Visible = false;
            pnlHome.Visible = false;
            lblSyncHeading.Visible = false;
            lblImportHeading.Visible = true;
        }
    }
}