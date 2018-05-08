using KM.Common;
using SourceMediaPaidPub.Objects;
using SourceMediaPaidPub.Process;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PaidPub
{
    public partial class thankyou : System.Web.UI.Page
    {
        public string PubCode
        {
            get
            {
                try
                {
                    if (Request.QueryString["pubcode"].ToString().Contains(','))
                        return Request.QueryString["pubcode"].ToString().Split(',')[0].ToString();
                    else
                        return Request.QueryString["pubcode"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
            set { ViewState["PubCode"] = value; }
        }

        private string getQueryString(string qs)
        {
            try { return Request.QueryString[qs].ToString(); }
            catch { return string.Empty; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                KMPS_JF_Objects.Objects.Publication pub = new KMPS_JF_Objects.Objects.Publication();

                try
                {
                    pub = getPublicationByPubCode();

                    if (pub == null || pub.PubCode.Trim().Length == 0)
                    {
                        //lblErrorMessage.Text = "Invalid Publication";
                        return;
                    }
                    else
                    {
                        phHeader.Controls.Add(new LiteralControl(pub.HeaderHTML));
                        phFooter.Controls.Add(new LiteralControl(pub.FooterHTML));

                        string FilePath = ConfigurationManager.AppSettings["MagazineJson"]; 

		                SubscriberProcess _subscriberProcess = new SubscriberProcess();

                        Magazine _magazine = _subscriberProcess.GetMagazineList(FilePath);

                        SourcemediaPubcode sourceMediaPubCode = _magazine.SourcemediaPubcodes.SingleOrDefault(x => string.Equals(x.PubCode, PubCode, StringComparison.OrdinalIgnoreCase));

                        if (sourceMediaPubCode != null)
                        {
                            phThankYouHTML.Controls.Add(new LiteralControl(ReplaceCodeSnippets(sourceMediaPubCode.ThankYouHTML)));
                        }
                    }

                }
                catch (Exception ex)
                {
                    //lblErrorMessage.Text = "Invalid Publication";
                    return;
                }
            }
        }

        private string ReplaceCodeSnippets(string html)
        {
            html = Regex.Replace(html, "%%emailaddress%%", getQueryString("emailaddress"), RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "%%Address%%", getQueryString("Address"), RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "%%City%%", getQueryString("City"), RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "%%State%%", getQueryString("State"), RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "%%Zip%%", getQueryString("Zip"), RegexOptions.IgnoreCase);

            return html;
        }

        private KMPS_JF_Objects.Objects.Publication getPublicationByPubCode()
        {
            KMPS_JF_Objects.Objects.Publication pub = new KMPS_JF_Objects.Objects.Publication();
            if (IsCacheClear())
            {
                if (CacheUtil.IsCacheEnabled())
                {
                    if (CacheUtil.GetFromCache("Pub_" + PubCode.ToUpper(), "JOINTFORMS") != null)
                    {
                        CacheUtil.RemoveFromCache("Pub_" + PubCode.ToUpper(), "JOINTFORMS");
                    }
                }

                UpdateCacheClear();
            }
            pub = KMPS_JF_Objects.Objects.Publication.GetPublicationbyID(0, PubCode);
            return pub;
        }

        private bool IsCacheClear()
        {
            try
            {
                SqlCommand cmdGetCacheClear = new SqlCommand("select IsCacheClear from Publications where PubCode = @PubCode");
                cmdGetCacheClear.Parameters.AddWithValue("@PubCode", PubCode);
                return Convert.ToBoolean(KMPS_JF_Objects.Objects.DataFunctions.ExecuteScalar("", cmdGetCacheClear).ToString());
            }
            catch
            {
                return false;
            }
        }

        private void UpdateCacheClear()
        {
            try
            {
                SqlCommand cmdGetCacheClear = new SqlCommand("update Publications set IsCacheClear = 0 where PubCode = @PubCode");
                cmdGetCacheClear.Parameters.AddWithValue("@PubCode", PubCode);
                KMPS_JF_Objects.Objects.DataFunctions.Execute(cmdGetCacheClear);
            }
            catch
            {
            }
        }
    }
}
