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
using System.IO;

using SecurityAccess = ECN_Framework.Common.SecurityAccess;

namespace ecn.accounts.main.Digital
{			
	public partial class _default : ECN_Framework.WebPageHelper
	{		
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.DIGITALEDITION;
            
            if(KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
			{
				if (!IsPostBack) 
				{
					loadChannels();
					loadCustomers();
					loadEditions();
				}
			}
			else 
			{
				Response.Redirect("../default.aspx");				
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
		
//		this.drpStatus.SelectedIndexChanged += new System.EventHandler(this.drpType_SelectedIndexChanged);
//		this.drpChannels.SelectedIndexChanged += new System.EventHandler(this.drpChannels_SelectedIndexChanged);
//		this.drpCustomers.SelectedIndexChanged += new System.EventHandler(this.drpCustomers_SelectedIndexChanged);
//		this.EditionsPager.IndexChanged += new System.EventHandler(this.EditionsPager_IndexChanged);

		private void InitializeComponent()
		{    

		}
		#endregion

		private void loadChannels()
		{
            drpChannels.DataSource = ECN_Framework_DataLayer.DataFunctions.GetDataTable("select BaseChannelID, BaseChannelName from [basechannel] where IsDeleted = 0 and Accesspublisher = 1 order by BaseChannelName", ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());
			drpChannels.DataBind();
			drpChannels.Items.Insert(0, new ListItem("--- ALL ---","0"));
		}

		private void loadCustomers()
		{
			string sqlquery = string.Empty;

			if (drpChannels.SelectedIndex > -1)
				if (Convert.ToInt32(drpChannels.SelectedValue) != 0)
					sqlquery += " and basechannelID = " + drpChannels.SelectedValue ;

            drpCustomers.DataSource = ECN_Framework_DataLayer.DataFunctions.GetDataTable("select CustomerID, CustomerName from [customer] where isnull(PublisherChannelID,0) <> 0 and IsDeleted = 0 " + sqlquery + " order by CustomerName", ECN_Framework_DataLayer.DataFunctions.ConnectionString.Accounts.ToString());
			drpCustomers.DataBind();
			drpCustomers.Items.Insert(0, new ListItem("--- ALL ---","0"));
			drpCustomers.ClearSelection();

			drpCustomers.Items.FindByValue("0").Selected = true;
			//loadEditions();

		}

		public void dgEditions_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			ecn.publisher.classes.Edition objEdition = new ecn.publisher.classes.Edition();

            objEdition = ecn.publisher.classes.Edition.GetEditionbyID(Convert.ToInt32(e.CommandArgument.ToString()));

            string Edition_Image_Path = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + objEdition.CustomerID + "/Publisher/" + e.CommandArgument.ToString() + "/");

            if (System.IO.Directory.Exists(Edition_Image_Path))
            {
                try
                {
                    Directory.Delete(Edition_Image_Path, true);
                }
                catch(Exception ex)
                {
                    Response.Write(Edition_Image_Path + " / " + ex.Message);
                }
            }
            objEdition.Delete(Convert.ToInt32(e.CommandArgument.ToString()));

			ResetPager();
			loadEditions();
		}

		public void dgEditions_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			ImageButton btnDelete = new ImageButton();
			if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
			{
				if (e.Item.Cells[7].Text.ToLower() == "pending")
				{
					e.Item.Cells[8].Text = "";
					e.Item.Cells[9].Text = "";
					e.Item.Cells[10].Text = "";
				}
				else
				{
					e.Item.Cells[11].Text = "";
				}
				btnDelete = (ImageButton) e.Item.FindControl("btnDelete");
				btnDelete.Attributes.Add("onclick", "return confirm('Are You Sure You want to delete?')");
			}
		}

		private void loadEditions()
		{
			string sqlQuery = "select editionID, b.basechannelname, c.customername, EditionName, m.Publicationname, EnableDate, DisableDate, Pages, e.Status, " +
				" Case when IsSearchable=1 then 'Yes' else 'No' end as IsSearchable, e.CreateDate from " +
				" editions e join Publications m on e.PublicationID = m.PublicationID join ecn5_accounts..Customer c on c.customerID = m.customerID join " + 
				" ecn5_accounts..basechannel b on c.basechannelID = b.basechannelID ";
			
			if (drpStatus.SelectedValue == string.Empty)
				sqlQuery += "where e.status in ('Active','InActive','Archieve','Pending') ";
			else
				sqlQuery += "where e.status = '" + drpStatus.SelectedValue + "'";	
	
			if (drpChannels.SelectedIndex > -1)
				if (Convert.ToInt32(drpChannels.SelectedValue) != 0)
					sqlQuery += " and b.basechannelID = " + drpChannels.SelectedValue;

			if (drpCustomers.SelectedIndex > -1)
				if (Convert.ToInt32(drpCustomers.SelectedValue) != 0)
					sqlQuery += " and c.CustomerID = " + drpCustomers.SelectedValue;

			DataTable dtEditions = ECN_Framework_DataLayer.DataFunctions.GetDataTable(sqlQuery, ConfigurationManager.AppSettings["pub"]);
			dgEditions.DataSource= dtEditions;
			dgEditions.DataBind();

			EditionsPager.RecordCount = dtEditions.Rows.Count;
		}

		protected void BaseChannelList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			ResetPager();
			loadCustomers();
			loadEditions();
		}

		protected void CustomerList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			ResetPager();
			loadEditions();
		}

		protected void drpType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			ResetPager();
			loadEditions();
		}

		private void ResetPager()
		{
			dgEditions.CurrentPageIndex = 0; 
			EditionsPager.CurrentPage = 1;
			EditionsPager.CurrentIndex = 0;	
		}

		protected void EditionsPager_IndexChanged(object sender, System.EventArgs e)
		{
			loadEditions();
		}

	}
}
