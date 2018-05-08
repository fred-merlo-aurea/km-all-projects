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
using System.Data.SqlClient;
using System.Data.SqlTypes;
using CrystalDecisions.Shared; 
using CrystalDecisions.CrystalReports.Engine; 
using ecn.common.classes;

namespace ecn.accounts.main.reports
{
    public partial class AccountIntensity : ECN_Framework.WebPageHelper
	{
        
		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.REPORTS;
            
            if (!IsPostBack)
			{                
                if(KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser) || Staff.CurrentStaff.Role == StaffRoleEnum.AccountManager) 
				{
					loadChannels();
					loadAccountExecutive();
					loadAccountManager();

					drpCustomer.Items.Insert(0,new ListItem("-- ALL --","0"));	
					drpCustomer.Items.FindByValue("0").Selected = true;

                    DataTable codeDT = ECN_Framework_DataLayer.DataFunctions.GetDataTable("SELECT * FROM [Code] WHERE CodeType='CustomerType' and IsDeleted = 0 order by codename;", ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());
					WebControlFunctions.PopulateDropDownList(drpCustomerType, codeDT, "CodeName", "CodeValue");
					drpCustomerType.Items.Insert(0, new ListItem("-- ALL --",""));
					drpCustomerType.Items.FindByValue("").Selected = true;

				}
				else
					Response.Redirect("/ecn.accounts/main/default.aspx");
			}
		}

        ReportDocument report = new ReportDocument();
        protected void Page_Unload(object sender, System.EventArgs e)
        {
            if (report != null)
            {
                report.Close();
                report.Dispose();
            }
        }

		private void loadChannels()
		{
            DataTable dtChannels = ECN_Framework_DataLayer.DataFunctions.GetDataTable("select BaseChannelID, BaseChannelName From [BaseChannel] where IsDeleted = 0 order by BaseChannelName", ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());

			drpChannel.DataSource = dtChannels;
			drpChannel.DataTextField = "BaseChannelName";
			drpChannel.DataValueField = "BaseChannelID";
			drpChannel.DataBind();

			drpChannel.Items.Insert(0,new ListItem("-- ALL --","0"));	

			drpChannel.Items.FindByValue("0").Selected = true;
		}

		protected void drpChannel_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            DataTable dtCustomers = ECN_Framework_DataLayer.DataFunctions.GetDataTable("select CustomerID, CustomerName From [Customer] Where basechannelID = " + drpChannel.SelectedItem.Value + " and IsDeleted = 0 order by CustomerName", ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());

			drpCustomer.DataSource = dtCustomers;
			drpCustomer.DataTextField = "CustomerName";
			drpCustomer.DataValueField = "CustomerID";
			drpCustomer.DataBind();
			drpCustomer.Items.Insert(0,new ListItem("-- ALL --","0"));	

			drpCustomer.Items.FindByValue("0").Selected = true;
			resetpager();
			Loadgrid();
		}

		private void loadAccountExecutive()
		{
			drpAE.DataSource = Staff.GetStaffByRole(StaffRoleEnum.AccountExecutive);;
			drpAE.DataTextField = "FullName";
			drpAE.DataValueField = "ID";
			drpAE.DataBind();

			drpAE.Items.Insert(0,new ListItem("-- ALL --","0"));	

			drpAE.Items.FindByValue("0").Selected = true;
		}

		private void loadAccountManager()
		{
			drpAM.DataSource = Staff.GetStaffByRole(StaffRoleEnum.AccountManager);;
			drpAM.DataTextField = "FullName";
			drpAM.DataValueField = "ID";
			drpAM.DataBind();

			drpAM.Items.Insert(0,new ListItem("-- ALL --","0"));	

			drpAM.Items.FindByValue("0").Selected = true;

		}


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
            this.Unload += new EventHandler(Page_Unload);
		}

//		this.lnkToPDF.Click += new System.Web.UI.ImageClickEventHandler(this.lnkToPDF_Click);
//		this.lnktoExl.Click += new System.Web.UI.ImageClickEventHandler(this.lnktoExl_Click);
//		this.drpChannel.SelectedIndexChanged += new System.EventHandler(this.drpChannel_SelectedIndexChanged);
//		this.CustomersPager.IndexChanged += new System.EventHandler(this.CustomersPager_IndexChanged);
//		this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
//

		private void InitializeComponent()
		{    
			this.lnkToPDF.Click += new System.Web.UI.ImageClickEventHandler(this.lnkToPDF_Click);
			this.lnktoExl.Click += new System.Web.UI.ImageClickEventHandler(this.lnktoExl_Click);

		}
		#endregion

		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{
			resetpager();
			Loadgrid();
		}

		private void Loadgrid()
		{
			SqlCommand cmd = new SqlCommand("sp_accountintensity");
			cmd.CommandTimeout = 0;
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@channelID", SqlDbType.Int);
			cmd.Parameters["@channelID"].Value = drpChannel.SelectedItem.Value;

			cmd.Parameters.Add("@customerID", SqlDbType.Int);
			cmd.Parameters["@customerID"].Value = drpCustomer.SelectedIndex>-1?drpCustomer.SelectedItem.Value:"0";

			cmd.Parameters.Add("@customerType", SqlDbType.VarChar);
			cmd.Parameters["@customerType"].Value = drpCustomerType.SelectedIndex>-1?drpCustomerType.SelectedItem.Value:"";


			cmd.Parameters.Add("@AccountExecutiveID", SqlDbType.Int);
			cmd.Parameters["@AccountExecutiveID"].Value = drpAE.SelectedItem.Value;

			cmd.Parameters.Add("@AccountManagerID", SqlDbType.Int);
			cmd.Parameters["@AccountManagerID"].Value = drpAM.SelectedItem.Value;

            DataTable dt = ECN_Framework_DataLayer.DataFunctions.GetDataTable(cmd, ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());

            if (dt != null && dt.Rows.Count <= (CustomersPager.PageSize * CustomersPager.CurrentPage))
                resetpager();

			dgCustomers.DataSource = dt;
			dgCustomers.DataBind();

			if (dt.Rows.Count > 0)
			{
				CustomersPager.RecordCount = dt.Rows.Count;
				CustomersPager.Visible = true;
			}
			else
				CustomersPager.Visible = false;
		}

		private void resetpager()
		{
			dgCustomers.CurrentPageIndex = 0; 
			CustomersPager.CurrentPage = 1;
			CustomersPager.CurrentIndex = 0;	
		}

		private void lnkToPDF_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			GenerateReport(CRExportEnum.PDF);
		}

		private void lnktoExl_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			GenerateReport(CRExportEnum.XLS);
		}

		private void GenerateReport(CRExportEnum ExportFormat)
		{

			Hashtable cParams = new Hashtable();

			cParams.Add("@channelID", drpChannel.SelectedItem.Value);
			cParams.Add("@customerID",drpCustomer.SelectedIndex>-1?drpCustomer.SelectedItem.Value:"0");
			cParams.Add("@AccountExecutiveID", drpAE.SelectedItem.Value);
			cParams.Add("@AccountManagerID", drpAM.SelectedItem.Value);
			cParams.Add("@customerType", drpCustomerType.SelectedItem.Value);
			
		
			report = CRReport.GetReport(Server.MapPath("crystalreport/rpt_AccountIntensity.rpt"), cParams);
			crv.ReportSource = report;
			crv.Visible = true;

			CRReport.Export(report, ExportFormat, "AccountIntensity." + ExportFormat.ToString());
		}

		protected void CustomersPager_IndexChanged(object sender, System.EventArgs e)
		{
			Loadgrid();
		}

	}
}
