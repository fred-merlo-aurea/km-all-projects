using System;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using ecn.common.classes;

namespace ecn.accounts.webServicesKey
{
    public partial class AcessKey : System.Web.UI.Page
    {

        protected void Page_Load(object sender, System.EventArgs e) 
        {
            AccessKeyPanel.Visible = false;
            msglabel.Text = "";
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


        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {

        }
        #endregion

        protected void AccessKeyButton_Click(object sender, System.EventArgs e)
        {
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            try
            {
                string chID = es.CurrentBaseChannel.BaseChannelID.ToString();
                string custID = Master.UserSession.CurrentCustomer.CustomerID.ToString();
                string custName = DataFunctions.ExecuteScalar("Select CustomerName from [Customer] where CustomerID = " + custID + " and IsDeleted = 0").ToString();
                //string strToEncrypt	= chID+"|"+custID+"|"+custName; 
                string strToEncrypt = custID;
                string encryptedStr = KM.Common.Functions.DESEncryption.Encrypt(strToEncrypt, "ecn5WSVC");
                AccessKeyPanel.Visible = true;
                msglabel.Text = "" + encryptedStr + "<br>";
            }
            catch
            {
                AccessKeyPanel.Visible = true;
                msglabel.Text = "ERROR: WebServices Access Key couldnot be generated at this time. Please try again later [or] call (866) 844 6275 for assistance. ";
            }
        }//kIEUO3ZCjcG1xmD1WjgjcPwbVYsrcBO9Bc7+Mt87BDQ=
    }
}
