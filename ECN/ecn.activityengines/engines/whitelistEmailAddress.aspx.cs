using System;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ecn.activityengines
{

    public partial class whitelistEmailAddress : System.Web.UI.Page
    {
        //http://localhost/ecn.activityengines/engines/whitelistemailaddress.aspx?p=1,bill.hipps@teamkm.com

        string FromEmailAddress = string.Empty;
        int CustomerID = 0;

        private KMPlatform.Entity.User User;
        private ECN_Framework_Entities.Accounts.LandingPageAssign LPA;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            getParams();
            SetHeaderFooter();
            if (FromEmailAddress.Length > 0)
            {
                lblFromEmail.Text = FromEmailAddress;
            }

        }

        private void SetHeaderFooter()
        {
            LPA = ECN_Framework_BusinessLayer.Accounts.LandingPageAssign.GetOneToUse(2, -1, true);

            Page.Title = "Whitelist Email Address";

            Label lblHeader = Master.FindControl("lblHeader") as Label;
            lblHeader.Text = LPA.Header;

            Label lblFooter = Master.FindControl("lblFooter") as Label;
            lblFooter.Text = LPA.Footer;
        }

        private void getParams()
        {
            string _params = Request.QueryString["p"].ToString();
            ECN_Framework_Common.Functions.StringTokenizer st = new ECN_Framework_Common.Functions.StringTokenizer(_params, ',');
            ArrayList paramsAL = new ArrayList();

            for (int i = 0; st.HasMoreTokens(); i++)
            {
                paramsAL.Insert(i, st.NextToken().ToString());
            }
            try { CustomerID = Convert.ToInt32(paramsAL[0].ToString()); }
            catch {}
            try { FromEmailAddress = paramsAL[1].ToString(); }
            catch {}
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
    }
}
