using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Web.Configuration;
using KMPS_JF_Objects.Objects;
using AuthorizeNet;
using System.Text.RegularExpressions;
using System.Configuration;
using ecn.communicator.classes;
using System.Web;

namespace PaidPub
{
    public partial class thankyou : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int formID = getFormID();
            DataTable dt = GetForm(formID);
            string thankYouHTML = ReplaceCodeSnippets(dt.Rows[0]["ThankYouHTML"].ToString());
            phHeader.Controls.Add(new LiteralControl(thankYouHTML));
        }

        private int getFormID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["FormID"].ToString());
            }
            catch { return 0; }
        }



        private DataTable GetForm(int PaidFormID)
        {
            SqlCommand cmdGetForm = new SqlCommand("select * from PaidForm where PaidFormID=@PaidFormID");
            cmdGetForm.CommandType = CommandType.Text;
            cmdGetForm.Parameters.Add(new SqlParameter("@PaidFormID", SqlDbType.Int)).Value = PaidFormID;
            DataTable dt =DataFunctions.GetDataTable(cmdGetForm);
            return dt;
        }

        private string ReplaceCodeSnippets(string html)
        {
            Dictionary<string, string> dProfile_codeSnippet = new Dictionary<string, string>();
            dProfile_codeSnippet.Add("emailaddress".ToLower(), "emailaddress");
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
            Regex r = new Regex("%%");
            Array BreakupHTMLMail = r.Split(html);
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
                    if (dUDF_codeSnippet.TryGetValue(line_data, out outValue))
                    {
                        html_body.Append(outValue);
                    }
                    else if (dProfile_codeSnippet.TryGetValue(line_data, out outValue))
                    {
                        if (line_data.Equals("emailaddress"))
                        {
                            if (Request.QueryString.AllKeys.Contains<string>("e"))
                            {
                                html_body.Append(Request.QueryString["e"] != null ? Request.QueryString["e"].ToString().Replace("+", "%2B") : "");
                            }
                            else
                            {
                                html_body.Append(Request.QueryString[outValue] != null ? Request.QueryString[outValue].ToString().Replace("+", "%2B") : "");
                            }
                        }
                        else
                        {
                            html_body.Append(Request.QueryString[outValue] != null ? Request.QueryString[outValue].ToString() : "");
                        }

                    }
                    
                    
                    
                }
            }


            return html_body.ToString();
        }
    }
}
