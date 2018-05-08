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

namespace wattpaidpub
{
    public partial class Nonqual : System.Web.UI.Page
    {
        KMPS_JF_Objects.Objects.Publication pub = null; 


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
            try
            {
                try
                {
                    PubID = Convert.ToInt32(Request.QueryString["PubID"].ToString()); 
                }
                catch { }

                try
                {
                    PubCode = Server.HtmlEncode(Request.QueryString["PubCode"].ToString());  
                }
                catch { }


                pub = Publication.GetPublicationbyID(PubID, PubCode);

                if (pub == null)
                    Response.Redirect("Error.aspx?msg=Invalid Publication Code"); 


                try
                {
                    if (Request.QueryString["NQCountry"] != null && Request.QueryString["NQCountry"].ToString() == "1")
                    {
                        lblCountryName.Text = Server.HtmlEncode(Request.QueryString["ctry"].ToString());
                        pnlNonQualCountry.Visible = true;
                    }
                    else
                    {
                        lblValue.Text = Server.HtmlEncode(Request.QueryString["qv"].ToString());
                        lblQuestion.Text = Server.HtmlEncode(Request.QueryString["qn"].ToString()); 
                        //btnBack.Visible = true;
                        pnlNonQualResponse.Visible = true;
                    }
                }
                catch { }


                PubID = pub.PubID;
                PubCode = pub.PubCode;
                lblMagazineName.Text = pub.PubName;
                btnHomePage.Text = pub.PubName + " Home";
                lblPubName.Text = pub.PubName;

                Page.Title = pub.PubName + " NON QUALIFICATION FORM.";

                bindCSSStyle(pub);
                SetupPublicationPage(); 
            }
            catch
            {
                Response.Redirect("Error.aspx?msg=Invalid Publication Code");
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

        protected void btnSubscribe_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://eforms.kmpsgroup.com/wattpub/subscribe.aspx?" + Request.QueryString.ToString());              
        }

        protected void btnHomePage_Click(object sender, EventArgs e)  
        {
            Response.Redirect("http://www.petfoodindustry-digital.com");   
        }
    }
}
