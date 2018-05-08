using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using KMPS_JF_Objects.Objects;

namespace KMPS_JF.Forms
{
    public partial class FAQ : System.Web.UI.Page
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

        private string PubCode
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

        protected void Page_Load(object sender, EventArgs e)
        {
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
            }

            pub = Publication.GetPublicationbyID(PubID, PubCode);

            if (pub == null)
                Response.Redirect("Error.asx");

            bindCSSStyle(pub);
            SetupPublicationPage();
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
            sb = sb.Replace("%%FormWidth%%", (String.IsNullOrEmpty(pub.Width) ? "750px" : pub.Width));              

            divcss.InnerHtml = sb.ToString();
        }

        private void SetupPublicationPage()
        {
            try { phHeader.Controls.Add(new LiteralControl(pub.HeaderHTML)); }
            catch { }

            try { lblPageDesc.Text = pub.FAQPageHTML; }
            catch { }

            try { phFooter.Controls.Add(new LiteralControl(pub.FooterHTML)); }
            catch { }

        }
    }
}
