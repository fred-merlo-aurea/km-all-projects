using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS_JF_Objects.Objects;
using System.Text;
using System.Configuration;
using System.Web.Configuration;
using System.Net;
using System.Data;
using System.Data.SqlClient;

namespace KMPS_JF.Forms
{
    public partial class PassAlong : System.Web.UI.Page
    {
        private Publication pub = null;

        public int PubID
        {
            get
            {
                try { return Convert.ToInt32(ViewState["PubID"]); }
                catch { return 0; }
            }
            set
            {
                ViewState["PubID"] = value;
            }
        }

        public string PubCode
        {
            get
            {
                try { return ViewState["PubCode"].ToString(); }
                catch { return string.Empty; }
            }
            set
            {
                ViewState["PubCode"] = value;
            }
        }

        public string PubName
        {
            get
            {
                return ViewState["PubName"].ToString();
            }
            set
            {
                ViewState["PubName"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["t"] != null)
            {
                try
                {
                    string title = Request.QueryString["t"].ToString();
                    user_PA1FUNCTION.Items.FindByValue(title).Selected = true;
                    user_PA2FUNCTION.Items.FindByValue(title).Selected = true;
                    user_PA3FUNCTION.Items.FindByValue(title).Selected = true;
                }
                catch { }
            }

            if (Request.QueryString["e"] == null || Request.QueryString["e"].ToString() == string.Empty || Request.QueryString["PubCode"] == null || Request.QueryString["PubCode"].ToString() == string.Empty)
            {
                container.Visible = false;
                ErrorContainer.Visible = true;
            }
            else
            {
                try
                {
                    PubID = Convert.ToInt32(Request.QueryString["PubID"].ToString());
                }
                catch { }

                try
                {
                    PubCode = Request.QueryString["PubCode"].ToString();

                    if (PubCode.Length > 15)
                    {
                        PubCode = PubCode.Substring(0, 15);
                    }

                    PubCode = PubCode.Replace("'", "''");

                }
                catch { }

                pub = Publication.GetPublicationbyID(PubID, PubCode);

                if (pub == null)
                    Response.Redirect("error.aspx?msg=invalid publication code");

                PubID = pub.PubID;
                PubCode = pub.PubCode;
                lblPubName.Text = pub.PubName;

                Page.Title = pub.PubName + " PASS ALONG FORM.";

                if (!IsPostBack)
                {
                    try
                    {
                        SqlCommand cmddt = new SqlCommand("select DataText, DataValue from PubSubscriptionFieldData PSFD JOIN PubSubscriptionFields PSF ON PSFD.PSFieldID = PSF.PSFieldID where PubID = @PubID AND ECNFieldName = 'PA1FUNCTION'");
                        cmddt.CommandType = CommandType.Text;
                        cmddt.Parameters.Add(new SqlParameter("@PubID", SqlDbType.Int)).Value = pub.PubID;
                        DataTable dt = DataFunctions.GetDataTable(cmddt);
                        LoadFunction(user_PA1FUNCTION, dt);
                        LoadFunction(user_PA2FUNCTION, dt);
                        LoadFunction(user_PA3FUNCTION, dt);
                    }
                    catch { }
                }

                bindCSSStyle(pub);
                SetupPublicationPage();
            }
        }

        private void bindCSSStyle(Publication pub)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(ConfigurationManager.AppSettings["Pub_CSS"].ToString());

            sb = sb.Replace("%%PBGColor%%", (pub.CSS[0] == string.Empty ? "#e0e0e0" : pub.CSS[0]));
            sb = sb.Replace("%%PageFontSize%%", (pub.CSS[4] == string.Empty ? "12px" : pub.CSS[4]));
            sb = sb.Replace("%%PageFont%%", (pub.CSS[3] == string.Empty ? "Arial, Helvetica, sans-serif" : pub.CSS[3]));    
            sb = sb.Replace("%%FBGColor%%", (pub.CSS[1] == string.Empty ? "#ffffff" : pub.CSS[1]));
            sb = sb.Replace("%%PageBorder%%", (pub.CSS[2] == string.Empty ? "2" : pub.CSS[2]));

            sb = sb.Replace("%%CBGColor%%", (pub.CSS[5] == string.Empty ? "#EDEDF5" : pub.CSS[5]));
            sb = sb.Replace("%%CatFontSize%%", (pub.CSS[6] == string.Empty ? "12px" : pub.CSS[6]));
            sb = sb.Replace("%%CFColor%%", (pub.CSS[7] == string.Empty ? "black" : pub.CSS[7]));

            sb = sb.Replace("%%QFSize%%", (pub.CSS[8] == string.Empty ? "12px" : pub.CSS[8]));
            sb = sb.Replace("%%QFColor%%", (pub.CSS[9] == string.Empty ? "#840000" : pub.CSS[9]));
            sb = sb.Replace("%%QFBold%%", (pub.CSS[10] == string.Empty ? "bold" : pub.CSS[10]));

            sb = sb.Replace("%%AFSize%%", (pub.CSS[11] == string.Empty ? "12px" : pub.CSS[11]));
            sb = sb.Replace("%%AFColor%%", (pub.CSS[12] == string.Empty ? "black" : pub.CSS[12]));
            sb = sb.Replace("%%AFBold%%", (pub.CSS[13] == string.Empty ? "normal" : pub.CSS[13]));

            sb = sb.Replace("%%FormWidth%%", pub.Width); 

            divcss.InnerHtml = sb.ToString();
        }


        private void SetupPublicationPage()
        {
            PubName = pub.PubName;

            try { phHeader.Controls.Add(new LiteralControl(pub.HeaderHTML)); }
            catch { }

            try { phFooter.Controls.Add(new LiteralControl(pub.FooterHTML)); }
            catch { }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("g=" + pub.ECNDefaultGroupID);
            sb.Append("&c=" + pub.ECNCustomerID);
            sb.Append("&e=" + Request.QueryString["e"].ToString());
            sb.Append("&user_PA1FNAME=" + Server.UrlEncode(user_PA1FNAME.Text));
            sb.Append("&user_PA1LNAME=" + Server.UrlEncode(user_PA1LNAME.Text));
            sb.Append("&user_PA1FUNCTION=" + Server.UrlEncode(user_PA1FUNCTION.SelectedValue));
            sb.Append("&user_PA1EMAIL=" + Server.UrlEncode(user_PA1EMAIL.Text));                       
            sb.Append("&user_PA2FNAME=" + Server.UrlEncode(user_PA2FNAME.Text));
            sb.Append("&user_PA2LNAME=" + Server.UrlEncode(user_PA2LNAME.Text));
            sb.Append("&user_PA2FUNCTION=" + Server.UrlEncode(user_PA2FUNCTION.SelectedValue));            
            sb.Append("&user_PA2EMAIL=" + Server.UrlEncode(user_PA2EMAIL.Text));
            sb.Append("&user_PA3FNAME=" + Server.UrlEncode(user_PA3FNAME.Text));
            sb.Append("&user_PA3LNAME=" + Server.UrlEncode(user_PA3LNAME.Text));
            sb.Append("&user_PA3EMAIL=" + Server.UrlEncode(user_PA3EMAIL.Text));
            sb.Append("&user_PA3FUNCTION=" + Server.UrlEncode(user_PA3FUNCTION.SelectedValue));   
            sb.Append("&user_PA1FUNCTXT=" + Server.UrlEncode(user_PA1FUNCTXT.Text));
            sb.Append("&user_PA2FUNCTXT=" + Server.UrlEncode(user_PA2FUNCTXT.Text)); 
            sb.Append("&user_PA3FUNCTXT=" + Server.UrlEncode(user_PA3FUNCTXT.Text));

            HttpBrowserCapabilities browser = Request.Browser;
            string browserInfo = String.Format("OS={0},Browser={1},Version={2},Major Version={3},MinorVersion={4},IsBeta={5},IsCrawler={6},IsAOL={7},IsWin16={8},IsWin32={9},Supports Tables={10},SupportsCookies={11},Supports VBScript={12},EcmaScriptVersion={13}", browser.Platform, browser.Browser, browser.Version, browser.MajorVersion, browser.MinorVersion, browser.Beta, browser.Crawler, browser.AOL, browser.Win16, browser.Win32, browser.Tables, browser.Cookies, browser.VBScript, browser.EcmaScriptVersion);

            bool status = ECNUtils.ECNHttpPost(Request.QueryString["e"].ToString(), PubCode, sb.ToString(), browserInfo);   

            if (status)
            {
                Response.Redirect(String.Format("PassAlongThankYou.aspx?pubcode={0}", pub.PubCode));
            }
            else
            {
                Response.Redirect("error.aspx?msg=Error in Request. Please try again.");
            }
        }

        protected void user_PA1FUNCTION_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (user_PA1FUNCTION.SelectedIndex == user_PA1FUNCTION.Items.Count - 1)
            {
                user_PA1FUNCTXT.Enabled = true;
            }
            else
            {
                user_PA1FUNCTXT.Enabled = false;
            }
        }

        protected void user_PA3FUNCTION_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (user_PA3FUNCTION.SelectedIndex == user_PA1FUNCTION.Items.Count - 1)
            {
                user_PA3FUNCTXT.Enabled = true;        
            }
            else
            {
                user_PA3FUNCTXT.Enabled = false;
            }
        }

        protected void user_PA2FUNCTION_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (user_PA2FUNCTION.SelectedIndex == user_PA1FUNCTION.Items.Count - 1)
            {
                user_PA2FUNCTXT.Enabled = true;
            }
            else
            {
                user_PA2FUNCTXT.Enabled = false;
            }
        }

        private void LoadFunction(DropDownList controlName, DataTable dt)
        {
            try
            {
                controlName.DataSource = dt;
                controlName.DataTextField = "DataText";
                controlName.DataValueField = "DataValue";
                controlName.DataBind();
                controlName.Items.Insert(0, new ListItem("Select One", ""));
            }
            catch { }
        }
       

    }
}
