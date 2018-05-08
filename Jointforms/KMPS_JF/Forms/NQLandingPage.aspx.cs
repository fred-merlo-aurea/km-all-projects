using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KMPS_JF_Objects.Objects;
using System.Configuration;
using System.Data.SqlClient;

namespace KMPS_JF.Forms
{
    public partial class NQLandingPage : System.Web.UI.Page
    {
        Publication pub = null;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.Theme = "";
        }

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

        public int PFID
        {
            get
            {
                try { return Convert.ToInt32(ViewState["PFID"]); }
                catch { return 0; }
            }
            set
            {
                ViewState["PFID"] = value;
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

        public string NQCountry
        {
            get
            {
                return ViewState["NQCountry"].ToString();
            }
            set
            {
                ViewState["NQCountry"] = value;
            }
        }

        public string QN
        {
            get
            {
                return ViewState["QN"].ToString();
            }
            set
            {
                ViewState["QN"] = value;
            }
        }

        public string QV
        {
            get
            {
                return ViewState["QV"].ToString();
            }
            set
            {
                ViewState["QV"] = value;
            }
        }

        public int EmailID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["ei"].ToString());
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int MagazineID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["btx_m"].ToString());
                }
                catch { return 0; }
            }
        }

        public int IssueID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["btx_i"].ToString());
                }
                catch { return 0; }
            }
        }  


        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetExpires(DateTime.Now.AddDays(-1)); 
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetValidUntilExpires(false);
            
            if (!IsPostBack)
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

                try
                {
                    PFID = Convert.ToInt32(Request.QueryString["PFID"].ToString());
                }
                catch { }

                try
                {
                    NQCountry = Request.QueryString["NQCountry"].ToString().Trim();
                }
                catch { }

                try
                {
                    QN = Request.QueryString["qn"].ToString().Trim();
                }
                catch { }


                try
                {
                    QV = Request.QueryString["qv"].ToString().Trim();
                }
                catch { }
            }

            pub = Publication.GetPublicationbyID(PubID, PubCode);

            if (pub == null)
                Response.Redirect("Error.asx");
            else
            {
                PubID = pub.PubID;
                PubCode = pub.PubCode;
            }


            bindCSSStyle(pub);
            SetupPublicationPage();
            LoadRelatedPubs();
        }

        private void bindCSSStyle(Publication pub)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append("<style type=\"text/css\"> ");
            //sb.Append("body { background-color:"+ pub.CSS[0].ToString() +";text-align:center;padding:20px 0;font-size:%%PageFontSize%%; font-family:%%PageFont%% }");
            //sb.Append("</style>");

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
            sb = sb.Replace("%%FormWidth%%", (String.IsNullOrEmpty(pub.Width) ? "750px" : pub.Width));      

            divcss.InnerHtml = sb.ToString();
        }

        private void LoadRelatedPubs()
        {
            SqlCommand cmdRelatedPubs = new SqlCommand("select LinkedToPubID, PubName, PubCode, p.pubLogo from relatedpubs r join publications p on r.linkedtoPubID = p.pubID where p.isActive = 1 and r.Isactive = 1 and r.pubID = @PubID");
            cmdRelatedPubs.CommandType = CommandType.Text;
            cmdRelatedPubs.Parameters.Add(new SqlParameter("@PubID", PubID.ToString()));
            DataTable dtRelatedPubs = DataFunctions.GetDataTable(cmdRelatedPubs);

            if (dtRelatedPubs.Rows.Count > 0)
            {
                gvRelatedPubs.DataSource = dtRelatedPubs;
                gvRelatedPubs.DataBind();
            }
        }

        private string GetNQLandingPageHTML()
        {
            string NQLandingPageHTML = string.Empty;
            DataTable dtNQThankYou = null;

            try
            {
                if (!String.IsNullOrEmpty(Convert.ToString(PFID)) && NQCountry != null)
                {

                    SqlCommand cmddtThankYou = new SqlCommand("select pf.PaidPageURL, pfl.NQResponsePageHTML, pfl.NQResponseCounPageHTML " +
                                                                        " from PubFormLandingPages pfl join PubForms pf on pfl.PFID=pf.PFID join Publications pub on pf.PubID=pub.PubID" +
                                                                        " where pub.PubCode=@PubCode and pfl.PFID=@PFID");
                    cmddtThankYou.CommandType = CommandType.Text;
                    cmddtThankYou.Parameters.Add(new SqlParameter("@PubCode", PubCode));
                    cmddtThankYou.Parameters.Add(new SqlParameter("@PFID", PFID));
                    dtNQThankYou = DataFunctions.GetDataTable(cmddtThankYou);

                    if (dtNQThankYou.Rows.Count > 0)
                    {
                        if (NQCountry.Equals("1", StringComparison.OrdinalIgnoreCase))
                        {
                            NQLandingPageHTML = dtNQThankYou.Rows[0]["NQResponseCounPageHTML"].ToString();    
                        }
                        else if (NQCountry.Equals("0", StringComparison.OrdinalIgnoreCase))
                        {
                            NQLandingPageHTML = dtNQThankYou.Rows[0]["NQResponsePageHTML"].ToString();   
                        }
                    }
                    else
                    {
                        NQLandingPageHTML = string.Empty;     
                    }
                }
                else
                {
                    NQLandingPageHTML = string.Empty;      
                }

                if (NQLandingPageHTML.Trim().Length > 0)
                {
                    string paidLink = string.Empty;
                    paidLink = dtNQThankYou.Rows[0]["PaidPageURL"].ToString();
                    if(!paidLink.Contains("?"))
                        paidLink= paidLink +  "?";
                    else
                        paidLink = paidLink + "&";
                    paidLink= paidLink + Server.UrlDecode(Request.QueryString.ToString());

                    NQLandingPageHTML = NQLandingPageHTML.Replace("%%paidlink%%", paidLink);
                    NQLandingPageHTML = NQLandingPageHTML.Replace("%%qn%%", QN.ToString());
                    NQLandingPageHTML = NQLandingPageHTML.Replace("%%qv%%", QV.ToString());
                    NQLandingPageHTML = NQLandingPageHTML.Replace("%%EmailID%%", EmailID.ToString());
                    NQLandingPageHTML = NQLandingPageHTML.Replace("%%MagID%%", MagazineID.ToString());        
                    NQLandingPageHTML = NQLandingPageHTML.Replace("%%IssueID%%", IssueID.ToString());      
                }

                return NQLandingPageHTML;
            }
            catch 
            {
                return string.Empty;
            }
        }

        private void SetupPublicationPage()
        {
            PubName = pub.PubName;

            try { phHeader.Controls.Add(new LiteralControl(pub.HeaderHTML)); }
            catch { }

            try
            {

                lblPageDesc.Text = GetNQLandingPageHTML();
            }
            catch { }

            try { phFooter.Controls.Add(new LiteralControl(pub.FooterHTML)); }
            catch { }

        }
    }
}
