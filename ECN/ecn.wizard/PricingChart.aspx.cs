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
using System.Configuration;

namespace ecn.wizard
{
	/// <summary>
	/// Summary description for PricingChart.
	/// </summary>
	public partial class PricingChart : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			//delete EmailID from EmailGroups for that Group.	
			ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

			string sqlQuery	=	" select substring(convert(varchar,cast(EmailRangeStart as money),1),1,len(convert(varchar,cast(EmailRangeStart as money),1))-3) + ' - ' +  substring(convert(varchar,cast(EmailRangeEnd as money),1),1,len(convert(varchar,cast(EmailRangeEnd as money),1))-3) as EmailCount, isnull(BaseFee,0) as BaseFee, Isnull(EmailRate,0) as cost from wizard_EmailRates where baseChannelID = " + sc.ChannelID();

			try
			{
				dgPricing.DataSource = DataFunctions.GetDataTable(sqlQuery, ConfigurationManager.AppSettings["act"].ToString());
				dgPricing.DataBind();
			}
			catch
			{
				throw;
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
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
