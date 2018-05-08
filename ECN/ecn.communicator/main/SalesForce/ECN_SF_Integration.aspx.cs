using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.communicator.main.Salesforce
{
    public partial class basechannelsetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pnlActive.Visible = false;
            pnlInactive.Visible = true;

            if (!Page.IsPostBack)
            {
                loadData();
                if (Request.QueryString["code"] != null && !string.IsNullOrEmpty(Request.QueryString["code"]))
                {
                    ECN_Framework_Entities.Salesforce.SF_Authentication.AuthCode = Request.QueryString["code"].ToString();
                    ECN_Framework_Entities.Accounts.SFSettings sfs = ECN_Framework_BusinessLayer.Accounts.SFSettings.GetOneToUse(Master.UserSession.CurrentCustomer.CustomerID);
                    ECN_Framework_Entities.Salesforce.SF_Authentication.SetSalesForceToken(sfs.ConsumerKey, sfs.ConsumerSecret, sfs.SandboxMode.Value);
                    if (ECN_Framework_Entities.Salesforce.SF_Authentication.LoginAttempted == true && ECN_Framework_Entities.Salesforce.SF_Authentication.LoggedIn == true)
                    {
                        pnlActive.Visible = true;
                        pnlInactive.Visible = false;
                        Save();
                    }
                }                
            }
        }

        private void loadData()
        {
            ECN_Framework_Entities.Accounts.SFSettings sfs = ECN_Framework_BusinessLayer.Accounts.SFSettings.GetOneToUse(Master.UserSession.CurrentCustomer.CustomerID);
            if (sfs != null)
            {
                if (sfs.RefreshToken != string.Empty)
                {
                    pnlActive.Visible = true;
                    pnlInactive.Visible = false;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            ECN_Framework_Entities.Accounts.SFSettings sfs = ECN_Framework_BusinessLayer.Accounts.SFSettings.GetOneToUse(Master.UserSession.CurrentCustomer.CustomerID);
            if (sfs == null)
                sfs = new ECN_Framework_Entities.Accounts.SFSettings();
            sfs.CustomerCanOverride = false;
            sfs.ConsumerKey = txtConsumerKey.Text;
            sfs.ConsumerSecret = txtConsumerSecret.Text;
            sfs.PushChannelMasterSuppression = false;
            sfs.SandboxMode = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SF_UseSandbox"].ToString());
            sfs.BaseChannelID = Master.UserSession.CurrentCustomer.BaseChannelID;
            ECN_Framework_BusinessLayer.Accounts.SFSettings.Save(sfs, Master.UserSession.CurrentUser);
            ECN_Framework_Entities.Salesforce.SF_Authentication.SF_Login(sfs.ConsumerKey, sfs.ConsumerSecret, Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SF_UseSandbox"].ToString()));
        }

        private void Save()
        {         
            if(ECN_Framework_Entities.Salesforce.SF_Authentication.Token!=null)
            {
                ECN_Framework_Entities.Accounts.SFSettings sfs = ECN_Framework_BusinessLayer.Accounts.SFSettings.GetOneToUse(Master.UserSession.CurrentCustomer.CustomerID);
                sfs.RefreshToken = ECN_Framework_Entities.Salesforce.SF_Authentication.Token.refresh_token;
                sfs.BaseChannelID = Master.UserSession.CurrentCustomer.BaseChannelID;
                ECN_Framework_BusinessLayer.Accounts.SFSettings.Save(sfs, Master.UserSession.CurrentUser);
                loadData();
            }
        }

        protected void lnkRegular_Click(object sender, EventArgs e)
        {
            Response.Redirect("../main/ecnwizard/wizardsetup.aspx?campaignItemType=salesforce");
        }
    }
}