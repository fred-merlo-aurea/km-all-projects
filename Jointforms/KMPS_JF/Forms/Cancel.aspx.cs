using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using KMPS_JF_Objects.Objects;
using KMPS_JF_Objects.Controls;
using ecn.communicator.classes;
using ecn.common.classes;
using System.Text.RegularExpressions;

namespace KMPS_JF.Forms
{
    public partial class Cancel : System.Web.UI.Page
    {
        Publication pub = null;
        int PubID = -1;
        private string getQueryString(string qs)
        {
            try { return Request.QueryString[qs].ToString(); }
            catch { return string.Empty; }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    PubID = Convert.ToInt32(getQueryString("pubid"));
                }
                catch { }

                try
                {
                    PubCode = getQueryString("pubcode");

                    if (PubCode.Length > 15)
                    {
                        PubCode = PubCode.Substring(0, 15);
                    }

                    PubCode = PubCode.Replace("'", "''");
                }
                catch { }
                pub = Publication.GetPublicationbyID(PubID, PubCode);


                SetupPage();
            }
        }

        private void SetupPage()
        {
            if (pub != null)
            {
                if (pub.PageTitle.Trim().Length > 0)
                    Page.Title = pub.PageTitle;
                else
                    Page.Title = pub.PubName + " Subscription Form";

                divcss.InnerHtml = pub.GetCSS();

                phHeader.Controls.Add(new LiteralControl(pub.HeaderHTML));
                phFooter.Controls.Add(new LiteralControl(pub.FooterHTML));
                string basePath = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, string.Empty) + Request.ApplicationPath;
                hlBackToForm.NavigateUrl = basePath + "/Forms/Subscription.aspx" + Request.Url.Query.ToString() + "&step=form";
            }
        }
    }
}