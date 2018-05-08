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
using System.Text.RegularExpressions;
using ecn.communicator.classes;

namespace KMPS_JF.Forms
{
    public partial class ThankYou : System.Web.UI.Page
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

        public string DemoOption
        {
            get
            {
                try { return ViewState["demo7"].ToString(); }
                catch { return string.Empty; }
            }
            set
            {
                ViewState["demo7"] = value;
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

        public int EmailID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["ei"].ToString());   
                }
                catch { return 0; }  
            }              
        }

        public int GroupID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["g"].ToString());
                }
                catch { return 0; }
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

        public bool IsPaid
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(Convert.ToInt32(Request.QueryString["ispaid"].ToString()));  
                }
                catch { return false; }     
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

                try
                {
                    PFID = Convert.ToInt32(Request.QueryString["PFID"].ToString());
                }
                catch { }

                try
                {
                    DemoOption = Request.QueryString["demo7"] != null ? Request.QueryString["demo7"].ToString().Trim() : "";
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

        private string GetThankYouPageHTML()
        {
            string thankYouHTML = string.Empty;
                        
            try
            {
                if (IsPaid)
                {
                    thankYouHTML = ReplaceCodeSnippets(pub.PaidThankYouPageHTML); 
                }                                
                else if (!String.IsNullOrEmpty(Convert.ToString(PFID)) && DemoOption != null)
                {
                    SqlCommand cmddtThankYou = new SqlCommand("select pfl.PrintThankYouPageHTML,pfl.DigitalThankYouPageHTML,pfl.DefaultThankYouPageHTML" +
                                                                        " from PubFormLandingPages pfl join PubForms pf on pfl.PFID=pf.PFID join Publications pub on pf.PubID=pub.PubID" +
                                                                        " where pub.PubCode=@PubCode and pfl.PFID=@PFID");
                    cmddtThankYou.CommandType = CommandType.Text;
                    cmddtThankYou.Parameters.Add(new SqlParameter("@PubCode", PubCode));
                    cmddtThankYou.Parameters.Add(new SqlParameter("@PFID", PFID));
                    DataTable dtThankYou = DataFunctions.GetDataTable(cmddtThankYou);

                    if (dtThankYou.Rows.Count > 0)
                    {
                        if (DemoOption.Equals("A", StringComparison.OrdinalIgnoreCase))
                        {
                            thankYouHTML = dtThankYou.Rows[0]["PrintThankYouPageHTML"].ToString();
                        }
                        else if (DemoOption.Equals("B", StringComparison.OrdinalIgnoreCase))
                        {
                            thankYouHTML = dtThankYou.Rows[0]["DigitalThankYouPageHTML"].ToString();
                        }
                        else if (DemoOption.Equals("", StringComparison.OrdinalIgnoreCase) || DemoOption.Equals("C", StringComparison.OrdinalIgnoreCase))
                        {
                            thankYouHTML = ReplaceCodeSnippets(dtThankYou.Rows[0]["DefaultThankYouPageHTML"].ToString());
                        }
                        else
                        {
                            thankYouHTML = pub.ThankYouPageHTML;
                        }
                    }
                    else
                    {
                        thankYouHTML = pub.ThankYouPageHTML;
                    }
                }
                else
                {
                    thankYouHTML = pub.ThankYouPageHTML;
                }

                if (string.IsNullOrEmpty(thankYouHTML))
                {
                    thankYouHTML = pub.ThankYouPageHTML;
                }

                return ReplaceCodeSnippets(thankYouHTML);

            }
            catch
            {
                return ReplaceCodeSnippets(pub.ThankYouPageHTML); 
            }
        }

        private void SetupPublicationPage()
        {
            PubName = pub.PubName;

            try { phHeader.Controls.Add(new LiteralControl(pub.HeaderHTML)); }
            catch { }

            try
            {

                lblPageDesc.Text = GetThankYouPageHTML();
            }
            catch { }

            try { phFooter.Controls.Add(new LiteralControl(pub.FooterHTML)); }
            catch { }

        }


        private string ReplaceCodeSnippets(string emailbody)
        {

           

            Dictionary<string, string> dProfile_codeSnippet= new Dictionary<string, string>();
            dProfile_codeSnippet.Add("emailAddress".ToLower(), "e");
            dProfile_codeSnippet.Add("Title".ToLower(), "t");
            dProfile_codeSnippet.Add("FirstName".ToLower(), "fn");
            dProfile_codeSnippet.Add("LastName".ToLower(), "ln");
            dProfile_codeSnippet.Add("FullName".ToLower(), "n");
            dProfile_codeSnippet.Add("Company".ToLower(), "compname");
            dProfile_codeSnippet.Add("Occupation".ToLower(), "t");
            dProfile_codeSnippet.Add("Address".ToLower(), "adr");
            dProfile_codeSnippet.Add("Address2".ToLower(), "adr2");
            dProfile_codeSnippet.Add("City".ToLower(), "city");
            dProfile_codeSnippet.Add("State".ToLower(), "state");
            dProfile_codeSnippet.Add("Zip".ToLower(), "zc");
            dProfile_codeSnippet.Add("Country".ToLower(), "ctry");
            dProfile_codeSnippet.Add("Voice".ToLower(), "ph");
            dProfile_codeSnippet.Add("Mobile".ToLower(), "mph");
            dProfile_codeSnippet.Add("Fax".ToLower(), "fax");
            dProfile_codeSnippet.Add("Website".ToLower(), "website");
            dProfile_codeSnippet.Add("Age".ToLower(), "age");
            dProfile_codeSnippet.Add("Income".ToLower(), "income");
            dProfile_codeSnippet.Add("Gender".ToLower(), "gndr");
            dProfile_codeSnippet.Add("BirthDate".ToLower(), "bdt");
            dProfile_codeSnippet.Add("PubCode".ToLower(), "pubcode"); 

            Dictionary<string, string> dUDF_codeSnippet = new Dictionary<string, string>();
            if ((GroupID > 0) && (EmailID > 0))
            {
                SqlCommand cmddt_UDF = new SqlCommand("select gdf.ShortName, edv.DataValue from ECN5_COMMUNICATOR..EmailDataValues edv   with (NOLOCK) " +
                                                          "join ECN5_COMMUNICATOR..GroupDatafields gdf  with (NOLOCK) " +
                                                          "on edv.GroupDatafieldsID=gdf.GroupDatafieldsID  " +
                                                          "where edv.EmailID=@emailID  " +
                                                          "and gdf.GroupID=@groupID  and gdf.IsDeleted=0 and isnull(gdf.DatafieldSetID,0) = 0" +
                                                          "order by 1");
                cmddt_UDF.CommandType = CommandType.Text;
                cmddt_UDF.Parameters.Add(new SqlParameter("@groupID", GroupID));
                cmddt_UDF.Parameters.Add(new SqlParameter("@emailID", EmailID));
                DataTable dt_UDF = DataFunctions.GetDataTable(cmddt_UDF);
                foreach (DataRow dr in dt_UDF.AsEnumerable())
                {
                    dUDF_codeSnippet.Add(dr["ShortName"].ToString().ToLower(), dr["DataValue"].ToString());
                    dUDF_codeSnippet.Add("user_" + dr["ShortName"].ToString().ToLower(), dr["DataValue"].ToString());
                }            
            }

            Regex r = new Regex("%%");
            Array BreakupHTMLMail = r.Split(emailbody);
            StringBuilder html_body = new StringBuilder();
            for (int i = 0; i < BreakupHTMLMail.Length; i++)
            {
                string line_data = BreakupHTMLMail.GetValue(i).ToString();
                if (i % 2 == 0)
                    html_body.Append(line_data);
                else
                {
                    line_data = line_data.ToLower();
                    string outValue = "";
                    if(dUDF_codeSnippet.TryGetValue(line_data, out outValue))
                    {
                        html_body.Append(outValue);
                    }
                    else if( dProfile_codeSnippet.TryGetValue(line_data, out outValue))
                    {
                        if (line_data.Equals("emailaddress"))
                        {
                            html_body.Append(Request.QueryString[outValue] != null ? Request.QueryString[outValue].ToString().Replace("+","%2B") : "");
                        }
                        else
                        {
                            html_body.Append(Request.QueryString[outValue] != null ? Request.QueryString[outValue].ToString() : "");
                        }

                    }
                    else if(line_data.Equals("emailid"))
                    {
                        html_body.Append(EmailID.ToString());
                    }
                    else if(line_data.Equals("paidlink"))
                    {
                        html_body.Append(Request.QueryString.ToString());
                    }
                    else if(line_data.Equals("magid"))
                    {
                        html_body.Append(MagazineID.ToString());
                    }
                    else if(line_data.Equals("issueid"))
                    {
                        html_body.Append(IssueID.ToString());
                    }                    
                }
            }
            return html_body.ToString(); 
        }
    }
}

