using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;

namespace ecn.communicator.blastsmanager 
{
			
	public partial class conversionTracker : ECN_Framework.WebPageHelper
    {         
        protected System.Web.UI.WebControls.DataGrid ClicksGrid;
        protected ActiveUp.WebControls.PagerBuilder ClicksPager;
		protected System.Web.UI.WebControls.Button DownloadEmailsButton;
		protected System.Web.UI.WebControls.DropDownList DownloadType;
		protected System.Web.UI.WebControls.Panel DownloadPanel;
		
		ArrayList columnHeadings	= new ArrayList();
		ArrayList trackingLinks		= new ArrayList(); 
		ArrayList trackingLinksAlias	= new ArrayList();
		IEnumerator aListEnum		= null;
		DataTable emailstable;

		protected void Page_Load(object sender, System.EventArgs e) 
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.BLASTS; 
            Master.SubMenu = "";
            Master.Heading = "Conversion Tracking Reports";
            Master.HelpContent = "<p><b>Conversion Tracking</b><br />";
            Master.HelpTitle = "Blast Manager";	

            //if (KM.Platform.User.IsAdministratorOrHasUserPermission(Master.UserSession.CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ConversionTracking, KMPlatform.Enums.Access.View))	
            {
				int requestBlastID = getBlastID();
				Decimal totalRevenue = 0.0M;
				if (requestBlastID>0) 
                {
					loadClicksVisitConversionGrid(requestBlastID);
					totalRevenue = loadTotalRevenueConversion(requestBlastID);
					totalConversionRevenue.Text = "$"+String.Format("{0:#,###.##}",Convert.ToDecimal(totalRevenue));
					loadRevenueConversionGrid(requestBlastID);
				}
			}
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException();		
			}
		}

		private int getBlastID()
        {
			int theBlastID = 0;
			try {
				theBlastID = Convert.ToInt32(Request.QueryString["BlastID"].ToString());
			}
			catch(Exception E) {
				string devnull=E.ToString();
			}
			return theBlastID;
		}

		private void loadClicksVisitConversionGrid(int BlastID)
        {
            List<ECN_Framework_Entities.Communicator.ConversionLinks> cvList=
            ECN_Framework_BusinessLayer.Communicator.ConversionLinks.GetByBlastID(BlastID, Master.UserSession.CurrentUser);
			int j = 0;
            foreach (ECN_Framework_Entities.Communicator.ConversionLinks cv in cvList)
            {
                trackingLinks.Insert(j, cv.LinkURL);
				trackingLinksAlias.Insert(j, cv.LinkName); 
				j++;
			}		

			DataTable newDT = new DataTable();
			newDT.Columns.Add(new DataColumn("Conversion Page Name / Conversion URL "));
			newDT.Columns.Add(new DataColumn("<center>Total Conversions</center>"));
			newDT.Columns.Add(new DataColumn("<center>Unique Conversions</center>"));
			newDT.Columns.Add(new DataColumn("<center>Rate</center>"));
			
			DataRow newDR;
			
			for(int i=0; i<trackingLinks.Count; i++) 
            {
				int totalCount = 0, distinctCount = 0, clicksCount = 0;
                totalCount = ECN_Framework_BusinessLayer.Activity.BlastActivityConversion.GetCount(BlastID, Master.UserSession.CurrentUser.CustomerID, trackingLinks[i].ToString(), trackingLinks[i].ToString().Length + 1);
                distinctCount = ECN_Framework_BusinessLayer.Activity.BlastActivityConversion.GetDistinctCount(BlastID, Master.UserSession.CurrentUser.CustomerID, trackingLinks[i].ToString(), trackingLinks[i].ToString().Length + 1);
                clicksCount = ECN_Framework_BusinessLayer.Activity.BlastActivityClicks.CountByBlastID(BlastID);
	
				newDR			= newDT.NewRow();
				if(trackingLinksAlias[i].ToString().Length > 0)
                {
					newDR[0]		= "<b>["+trackingLinksAlias[i].ToString()+"]</b>&nbsp;"+trackingLinks[i].ToString();
				}
                else
                {
					newDR[0]		= trackingLinks[i].ToString();
				}
				newDR[1]		= "<center>"+totalCount+"</center>";
				newDR[2]		= "<center>"+distinctCount+"</center>";
                try
                {
                    newDR[3] = "<center>" + Decimal.Round(((Convert.ToDecimal(distinctCount) / Convert.ToDecimal(clicksCount)) * 100), 0).ToString() + "&nbsp;%</center>";
                }
                catch
                {
                    newDR[3] = "<center>0&nbsp;%</center>";
                }
				
				newDT.Rows.Add(newDR);
			}
			ClicksVisitConversionGrid.DataSource = new DataView(newDT);
			ClicksVisitConversionGrid.DataBind();

			conversionFormula.Text = "<u><b>Conversion Rate Calculation:</b></u><br>(# of Unique Tracking / # of Unique Clicks) X 100 ";
		}

        private string getLinkAlias(int BlastID, string link) 
        {
            ECN_Framework_Entities.Communicator.LinkAlias linkAlias =
            ECN_Framework_BusinessLayer.Communicator.LinkAlias.GetByBlastLink(BlastID, link, Master.UserSession.CurrentUser, false);
            return linkAlias.Alias;
		}

		private Decimal loadTotalRevenueConversion(int BlastID)
        {
			Decimal revenueTotal = 0.0M;
            DataTable dt=  ECN_Framework_BusinessLayer.Activity.BlastActivityConversion.GetRevenueData(BlastID, Master.UserSession.CurrentUser.CustomerID, "simple");
            if (dt.Rows.Count > 0)
                revenueTotal = Convert.ToDecimal(dt.Rows[0][0].ToString());
			return revenueTotal;
		}

		private void loadRevenueConversionGrid(int BlastID) 
        {
            RevenueConversionGrid.DataSource = ECN_Framework_BusinessLayer.Activity.BlastActivityConversion.GetRevenueData(Master.UserSession.CurrentUser.CustomerID, BlastID, "detailed");
            RevenueConversionGrid.DataBind();
		}

		public void downloadClickEmails(object sender, System.EventArgs e)
        {
			string newline	= "";

            string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentBaseChannel.BaseChannelID + "/downloads/");

			string downloadType	= DownloadType.SelectedItem.Value.ToString();
			DateTime date = DateTime.Now;
            String tfile = Master.UserSession.CurrentUser.CustomerID + "-" + getBlastID() + "-click-emails" + downloadType;
			string outfileName		= txtoutFilePath+tfile;					
			TextWriter txtfile= File.AppendText(outfileName);			
			columnHeadings.Insert(0,"ClickTime");
			columnHeadings.Insert(1,"EmailAddress");
			columnHeadings.Insert(2,"FullLink");
			aListEnum = columnHeadings.GetEnumerator();
			while(aListEnum.MoveNext()){
				newline += aListEnum.Current.ToString()+", ";
			}
			txtfile.WriteLine(newline);

            emailstable = ECN_Framework_BusinessLayer.Activity.BlastActivityClicks.GetByBlastID(Master.UserSession.CurrentUser.CustomerID, getBlastID(), Master.UserSession.CurrentUser);
			foreach ( DataRow dr in emailstable.Rows ) {
				newline = "";
				aListEnum.Reset();
				while(aListEnum.MoveNext()){
					newline += dr[aListEnum.Current.ToString()].ToString()+", ";
				}
				txtfile.WriteLine(newline);
			}
			txtfile.Close();

            if (downloadType == ".xls")
                Response.ContentType = "application/vnd.ms-excel";
            else
                Response.ContentType = "text/csv";

            Response.AddHeader("content-disposition", "attachment; filename=" + tfile);
            Response.WriteFile(outfileName);
            Response.Flush();
            Response.End();
		}
	}
}
