using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using ecn.communicator.Constants;

namespace ecn.communicator.main.lists
{
	public partial class groupsubscribePrePopScript : System.Web.UI.Page
    {
        protected Label Footer;

        public int SFID
        {
            get
            {
                var sfid = default(int);

                if (Request.QueryString.AllKeys.Contains(QueryStringKeys.SFID))
                {
                    int.TryParse(Request.QueryString[QueryStringKeys.SFID], out sfid);
                }

                return sfid;
            }
        }

        public int CustomerId
        {
            get
            {
                var customerId = default(int);

                if (Request.QueryString.AllKeys.Contains(QueryStringKeys.CustomerId))
                {
                    int.TryParse(Request.QueryString[QueryStringKeys.CustomerId], out customerId);
                }

                return customerId;
            }
        }

        protected void Page_Load(object sender, EventArgs e) 
        {
			LoadHeaderAndFooter(CustomerId);
			LoadScripts(SFID);
	    }

		#region Load Header & Footers
		private void LoadHeaderAndFooter(int customerID)
        {
            List<ECN_Framework_Entities.Accounts.CustomerTemplate> customerTemplateList = ECN_Framework_BusinessLayer.Accounts.CustomerTemplate.GetByCustomerID(customerID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            var result = (from src in customerTemplateList
                          where src.TemplateTypeCode == "ReportSPAM-PageHdr" && src.IsActive == true
                          select src.HeaderSource).ToList();
            if (result.Count != 0)
            {             
                lblHeader.Text = result.First();
            }
            else
            {
                customerTemplateList = ECN_Framework_BusinessLayer.Accounts.CustomerTemplate.GetByCustomerID(1, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                result =     (from src in customerTemplateList
                             where src.TemplateTypeCode == "ReportSPAM-PageHdr" && src.IsActive == true
                             select src.HeaderSource).ToList();
                lblHeader.Text = result.First();
            }
		}
		#endregion

		#region Load Scripts
		private void LoadScripts(int sfID){
			PrePopWebPageScriptTXT.Text = "<script language='Javascript' src='http://www.ecn5.com/ecn.communicator/scripts/validations.js'>\n</script>";
			PrePopBlastScriptTXT.Text = "HTTP://%%WEB_PAGE_URL_HOSTING_THE_ABOVE_SCRIPT%%?sfID="+sfID+"&eID=%%EmailID%%";
		}
		#endregion
        
	}
}
