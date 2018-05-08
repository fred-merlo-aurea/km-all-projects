using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.common.classes;
using ecn.common.classes.billing;

namespace ecn.accounts.main.billingSystem
{
    public partial class PriceItemDetails : ECN_Framework.WebPageHelper
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.BILLINGSYSTEM;  

            if (!IsPostBack) {
				ddlChannels.DataSource = BaseChannel.GetBaseChannels();
				ddlChannels.DataTextField = "Name";
				ddlChannels.DataValueField = "ID";
				ddlChannels.DataBind();
				ddlChannels.SelectedIndex = 0;
				ddlChannels_SelectedIndexChanged(null, null);
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void ddlChannels_SelectedIndexChanged(object sender, System.EventArgs e) {
			if (ddlChannels.SelectedIndex == -1) {
				return;
			}

			TechAccessPriceItemEditor.QuoteOptionLicenseType = LicenseTypeEnum.AnnualTechAccess;
			TechAccessPriceItemEditor.QuoteOptions = QuoteOption.GetServiceLevelQuoteOptions(Convert.ToInt32(ddlChannels.SelectedValue));			
			TechAccessPriceItemEditor.BaseChannelID = Convert.ToInt32(ddlChannels.SelectedValue);

			EmailUsagePriceItemEditor.QuoteOptionLicenseType = LicenseTypeEnum.EmailBlock;
			EmailUsagePriceItemEditor.QuoteOptions = QuoteOption.GetEmailUsageQuoteOptions(Convert.ToInt32(ddlChannels.SelectedValue));			
			EmailUsagePriceItemEditor.BaseChannelID = Convert.ToInt32(ddlChannels.SelectedValue);

			OptionPriceItemEditor.QuoteOptionLicenseType = LicenseTypeEnum.Option;			
			OptionPriceItemEditor.QuoteOptions = QuoteOption.GetOptionQuoteOptions(Convert.ToInt32(ddlChannels.SelectedValue));			
			OptionPriceItemEditor.BaseChannelID = Convert.ToInt32(ddlChannels.SelectedValue);
		}
	}
}
