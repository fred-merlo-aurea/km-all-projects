using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using KMPS_JF_Objects.Objects;
using System.Text.RegularExpressions;

namespace UPIPaidPub
{
    public partial class upi_thankyou : System.Web.UI.Page
    {
        Publication pub = null;

        public string PubCode
        {
            get
            {
                try
                {
                    return Request.QueryString["pubcode"].ToString();
                }
                catch { return string.Empty; }
            }
            set
            {
                ViewState["PubCode"] = value;
            }
        }

        public string ExpirationDate
        {
            get
            {
                try
                {
                    return Request.QueryString["expdt"].ToString();
                }
                catch { return string.Empty; }
            }
        }

        public string Password
        {
            get
            {
                try
                {
                    return Request.QueryString["pwd"].ToString();
                }
                catch { return string.Empty; }
            }
        }


        public string Email
        {
            get
            {
                try
                {
                    return Request.QueryString["E"].ToString();
                }
                catch { return string.Empty; }  
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(Server.MapPath("/upipaid/App_Themes/" + PubCode.ToUpper())))
                {
                    Page.Theme = PubCode;
                }
                else
                {
                    Page.Theme = "Default";
                }
            }
            catch 
            {
                Page.Theme = "Default";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dtMagList = GetMagList(PubCode);

            string header = dtMagList.Rows[0]["header"].ToString().Trim();
            if (PubCode != null && PubCode.Trim().Length > 0 && header.Length > 0)
                phHeader.Controls.Add(new LiteralControl(header));

            try
            {
                pub = Publication.GetPublicationbyID(0, PubCode);

                if (pub.PageTitle.Trim().Length > 0)
                    Page.Title = pub.PageTitle; 
                else
                    Page.Title = pub.PubName + " Registration";

                if (header.Length == 0)
                    phHeader.Controls.Add(new LiteralControl(pub.HeaderHTML));

                //phBody.Controls.Add(new LiteralControl(ReplaceCodeSnippets(pub.PaidThankYouPageHTML)));
                phBody.Controls.Add(new LiteralControl(pub.PaidThankYouPageHTML));
            }
            catch
            {
                Page.Title = PubCode + " Registration";
            }
        }

        private string ReplaceCodeSnippets(string emailBody)
        {
            string body = string.Empty;
            body = Regex.Replace(emailBody, "%%EXPIRATIONDATEYYYYMMDD%%", ExpirationDate, RegexOptions.IgnoreCase);                         
            body = Regex.Replace(body, "%%PASSWORD%%", Password, RegexOptions.IgnoreCase);
            body = Regex.Replace(body, "%%EMAILADDRESS%%", Email, RegexOptions.IgnoreCase); 
            return body;
        }

        private DataTable GetMagList(string pubCode)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Server.MapPath("xml/MagList.xml"));
            DataTable magList = ds.Tables[0];

            if (String.IsNullOrEmpty(pubCode))
            {
                return magList;
            }
            else
            {
                DataTable newMagList = magList.Clone();
                string expression = "pubcode = '" + pubCode + "'";
                DataRow[] results = magList.Select(expression);

                foreach (DataRow dr in results)
                {
                    newMagList.ImportRow(dr);
                }

                return newMagList;
            }
        }
    }
}
