using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using ecn.common.classes;

namespace ecn.accounts.main.channels
{
    public class DateRange
    {
        public DateRange(DateTime start, DateTime end)
        {
            _start = start;
            _end = end;
        }
        private DateTime _start;
        public DateTime Start
        {
            get
            {
                return (this._start);
            }
            set
            {
                this._start = value;
            }
        }
        private DateTime _end;
        public DateTime End
        {
            get
            {
                return (this._end);
            }
            set
            {
                this._end = value;
            }
        }
    }

    public partial class LicenseReport : ECN_Framework.WebPageHelper
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CHANNELS;  

            if (!IsPostBack) {
				txtStart.Text = DateTime.Now.AddMonths(-1).ToShortDateString();	
				LoadChannels();
			}
		}

        private void LoadChannels()
        {
            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                ddlChannels.DataSource = BaseChannel.GetBaseChannels();
            }
            else
            {
                ArrayList channels = new ArrayList();
                channels.Add(BaseChannel.GetBaseChannelByID(Master.UserSession.CurrentBaseChannel.BaseChannelID));
                ddlChannels.DataSource = channels;
            }
            ddlChannels.DataValueField = "ID";
            ddlChannels.DataTextField = "Name";
            ddlChannels.DataBind();

            if (ddlChannels.Items.Count > 0)
            {
                ddlChannels.SelectedIndex = 0;
            }
        }

		private void LoadAllCorpLicenses() {			
			DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(string.Format(@"select * from [CustomerLicense] 
where CustomerID in (select CustomerID from [Customer] where basechannelID = {0} and IsDeleted = 0) and IsDeleted = 0
and LicenseLevel = 'CORP' and AddDate > '{1}' and AddDate < '{2}';", ddlChannels.SelectedValue, FromDate.ToShortDateString(), ToDate.ToShortDateString()), ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());	
			dgdCorpLicenses.DataSource = dt;
			dgdCorpLicenses.DataBind();
		}

		#region Properties		
		DateTime FromDate {
			get {
				try {
					return Convert.ToDateTime(txtStart.Text);
				} catch (Exception) {					
					txtStart.Text = "";
					return DateTimeInterpreter.MinValue;						
				}				
			}
		}

		DateTime ToDate {
			get {
				try {
					return Convert.ToDateTime(txtEnd.Text);
				} catch( Exception) {
					txtEnd.Text = "";
					return DateTimeInterpreter.MaxValue;							
				}				
			}
		}
		#endregion

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
			this.dgdCorpLicenses.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgdCorpLicenses_ItemCommand);
			this.dgdCorpLicenses.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgdCorpLicenses_PageIndexChanged);

		}
		#endregion

		protected void btnRefine_Click(object sender, System.EventArgs e) {
			LoadAllCorpLicenses();	
			dgdDetailedUsage.Visible = false;
		}

		private void dgdCorpLicenses_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e) {
			dgdCorpLicenses.CurrentPageIndex = e.NewPageIndex;
			LoadAllCorpLicenses();
		}

		private void ShowDetails(DateTime startDate, DateTime endDate, int quantity) {
			dgdDetailedUsage.Visible = true;			
			ArrayList customers = Customer.GetAllCustomersByChannelID(Convert.ToInt32(ddlChannels.SelectedValue));
			ArrayList dateRanges = GetDateRanges(startDate, endDate);
			StringBuilder sql = new StringBuilder();

			/// Build sql state to retrive reporting data according to the start/end date of the license:
			/// DateRange1, DateRange2, ... ,DateRangeN, ID, Total
			for(int i=0; i<customers.Count;i++) {
				Customer c = customers[i] as Customer;
				sql.Append(string.Format("SELECT '{0}' as Name,", ECN_Framework_DataLayer.DataFunctions.CleanString(c.Name)));				
				sql.Append(Environment.NewLine);
				foreach(DateRange range in dateRanges) {
                    sql.Append(string.Format("(SELECT sum(SuccessTotal) from {1}.dbo.[Blast] where CustomerID={2} and StatusCode <> 'Deleted' and TestBlast='n' and sendTime>='{3}' and sendTime<='{4}') as [{0}],",
						GetMonthName(range.Start),ConfigurationManager.AppSettings["communicatordb"],c.ID, range.Start.ToShortDateString(), range.End.ToShortDateString()));
					sql.Append(Environment.NewLine);
				}
				sql.Append(string.Format("{0} as ID,", c.ID));
                sql.Append(string.Format("(SELECT sum(SuccessTotal) from {1}.dbo.[Blast] where CustomerID={2} and StatusCode <> 'Deleted' and TestBlast='n' and sendTime>='{3}' and sendTime<='{4}') as [{0}]",
					"Total",ConfigurationManager.AppSettings["communicatordb"],c.ID, startDate.ToShortDateString(), endDate.AddDays(1).ToShortDateString()));
				sql.Append(Environment.NewLine);
				sql.Append(Environment.NewLine);
				
				if (i<customers.Count-1) {
					sql.Append("UNION");
					sql.Append(Environment.NewLine);				
				}
			}			

			sql.Append("UNION ALL");
			sql.Append(Environment.NewLine);				
			sql.Append(string.Format("SELECT '{0}' as Name,", "Total"));				
			sql.Append(Environment.NewLine);
			/// Append one more sql to calculate the Monthly total across all customers in the channel.
			foreach(DateRange range in dateRanges) {
                sql.Append(string.Format("(SELECT sum(SuccessTotal) from {1}.dbo.[Blast] where CustomerID in (select CustomerID from [Customer] where IsDeleted = 0 and BaseChannelID = {2}) and StatusCode <> 'Deleted' and TestBlast='n' and sendTime>='{3}' and sendTime<='{4}') as [{0}],",
					GetMonthName(range.Start),ConfigurationManager.AppSettings["communicatordb"],ddlChannels.SelectedValue, range.Start.ToShortDateString(), range.End.AddDays(1).ToShortDateString()));
				sql.Append(Environment.NewLine);
			}
			// For the total line, set the customer ID to zero.
			sql.Append(string.Format("{0} as ID,", 0));
			sql.Append(string.Format("(SELECT sum(SuccessTotal) from {1}.dbo.[Blast] where CustomerID in (select CustomerID from [Customer] where IsDeleted = 0 and BaseChannelID = {2}) and StatusCode <> 'Deleted' and TestBlast='n' and sendTime>='{3}' and sendTime<='{4}') as [{0}]",
				"Total",ConfigurationManager.AppSettings["communicatordb"],ddlChannels.SelectedValue, startDate.ToShortDateString(), endDate.AddDays(1).ToShortDateString()));
			sql.Append(Environment.NewLine);			

			/// Build datagrid columns according to the start/end date of the license.
			for(int i=0;i<dateRanges.Count;i++) {
				DateRange range = dateRanges[i] as DateRange;
				BoundColumn column = new BoundColumn();				
				string header;
				if (i==0 || i==dateRanges.Count-1) {
					header = string.Format("{0}~{1}", range.Start.ToShortDateString(), range.End.ToShortDateString());								
				} else {
					header = GetMonthName(range.Start);
				}
				column.HeaderText = string.Format("<span title=\"{1}~{2}\">{0}</a>",header, range.Start.ToShortDateString(), range.End.ToShortDateString());								
				column.DataField = GetMonthName(range.Start);	
				column.DataFormatString="{0:d}";
				column.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
				column.FooterStyle.BackColor=Color.White;
				dgdDetailedUsage.Columns.Add(column);			
			}

			/// Add Total column:
			BoundColumn colTotal = new BoundColumn();
			colTotal.HeaderText = "Total";
			colTotal.DataField = "Total";
			colTotal.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
			colTotal.ItemStyle.BackColor=Color.White;			
			dgdDetailedUsage.Columns.Add(colTotal);

            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sql.ToString(), ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());			
			dgdDetailedUsage.DataSource = dt;
			dgdDetailedUsage.DataBind();

			int usedCount = 0;
			if (dt.Rows.Count >0) {
				try {
					usedCount = Convert.ToInt32(dt.Rows[dt.Rows.Count-1]["Total"]);				
				} catch  {
					usedCount = 0;
				}
			}

			dgdDetailedUsage.Caption = string.Format("Usage Detail -- Licensed: {0} Used: {1} Remaining: {2}<br/>", quantity, usedCount, quantity-usedCount);
		}

		private ArrayList GetDateRanges(DateTime start, DateTime end) {
			ArrayList dateRanges = new ArrayList();

			DateTime startDate = start;
			bool notReachTheEnd = true;
			do {
				DateTime endDate = GetLastDayOfTheMonth(startDate.Year, startDate.Month);
				if (endDate > end) {
					endDate = end;
					notReachTheEnd = false;					
				}
				dateRanges.Add(new DateRange(startDate, endDate));
				startDate = endDate.AddDays(1);
			} while (notReachTheEnd);
			return dateRanges;
		}

		private DateTime GetLastDayOfTheMonth(int year, int month) {
			return new DateTime(year, month, DateTime.DaysInMonth(year, month));						
		}

		private string GetMonthName(DateTime date) {
			return string.Format("{0:MMM,yy}", date);
		}

		private void dgdCorpLicenses_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e) {
			if (e.CommandName != "view") {
				return;
			}

			dgdCorpLicenses.SelectedIndex = e.Item.ItemIndex;
			LoadAllCorpLicenses();

			int clid = Convert.ToInt32(e.CommandArgument);
            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable("SELECT * from [CustomerLicense] where IsDeleted = 0 and CLID = " + clid, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());
			if (dt.Rows.Count == 0) {
				return;
			}

			DateTime start = Convert.ToDateTime(dt.Rows[0]["AddDate"]);
			DateTime end = Convert.ToDateTime(dt.Rows[0]["ExpirationDate"]);	
			int quantity = Convert.ToInt32(dt.Rows[0]["Quantity"]);
			ShowDetails(start, end, quantity);
		}
	}

	
	/// Used to represent the date range for licenses.
	/// eg. A corporation license is good from 4/6/2003~4/7/2004.
	/// The good time will be split into:
	/// 4/6/2003~4/30/2003
	/// 5/1/2003~5/31/2003
	/// ...
	/// 4/1/2004~4/7/2004
	
}
