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
using System.Collections.Generic; 

namespace ecn.publisher.main.Publication
{
	
	
	
	public partial class Rules : System.Web.UI.Page
	{
        /*

		private int getPublicationID() 
		{
			try 
			{
				return Convert.ToInt32(Request.QueryString["PublicationID"].ToString());
			}
			catch
			{
				return 0;
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Publisher.Enums.MenuCode.PUBLICATION;
            Master.SubMenu = "";
            Master.Heading = "Rules List";
            Master.HelpContent = "";
            Master.HelpTitle = "";	

			if (!IsPostBack)
			{
				if (getPublicationID() > 0 ) //Update
				{

                    lblPublication.Text = ECN_Framework_BusinessLayer.Publisher.Publication.GetByPublicationID(getPublicationID(), Master.UserSession.CurrentUser).PublicationName;
                        
    				LoadRulesGrid();
				}
			}
		}

		private void LoadRulesGrid()
		{
			List<ECN_Framework_Entities.Publisher.Rule> r = ECN_Framework_BusinessLayer.Publisher.Rule.GetByEditionID(getPublicationID(), Master.UserSession.CurrentUser);
            

            DataTable dt = DataFunctions.GetDataTable("select r.RuleID, r.RuleName, e.EditionID, e.EditionName, convert(varchar(10),r.CreateDate,101) as CreateDate from rules r join edition e on r.editionID = e.editionID where r.publicationID = " + getPublicationID());

			dgRules.DataSource = dt.DefaultView;
			dgRules.DataBind();
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
			this.dgRules.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgRules_ItemCommand);
			this.dgRules.CancelCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgRules_CancelCommand);
			this.dgRules.EditCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgRules_EditCommand);
			this.dgRules.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgRules_UpdateCommand);
			this.dgRules.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgRules_DeleteCommand);
			this.dgRules.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgRules_ItemDataBound);

		}
		#endregion

		private void dgRules_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			dgRules.EditItemIndex = e.Item.ItemIndex;
			dgRules.ShowFooter = false;
			LoadRulesGrid();
		}

		private void dgRules_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			dgRules.EditItemIndex = - 1;
			dgRules.ShowFooter = true;
			LoadRulesGrid();
		}

		private void dgRules_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Footer)
			{
				DropDownList drpEditionListF = (DropDownList) e.Item.FindControl("drpEditionListF");

				DataTable dt = DataFunctions.GetDataTable("select editionID, editionName from Edition e where e.status = 'Active' and publicationID= " + getPublicationID() + " and editionID not in (select editionID from Rules where publicationID = " + getPublicationID() + ") order by EditionName ");

				drpEditionListF.DataTextField = "EditionName";
				drpEditionListF.DataValueField = "EditionID";
				drpEditionListF.DataSource = dt;
				drpEditionListF.DataBind();

			}
			
			if (e.Item.ItemType == ListItemType.EditItem)
			{
				DataRowView drv = (DataRowView) e.Item.DataItem;

				DropDownList drpEditionList = (DropDownList) e.Item.FindControl("drpEditionList");

				DataTable dt = DataFunctions.GetDataTable("select editionID, editionName from Edition e where e.status = 'Active' and publicationID= " + getPublicationID() + " and editionID not in (select editionID from Rules where publicationID = " + getPublicationID() + " and ruleID <> " + dgRules.DataKeys[e.Item.ItemIndex].ToString() + ") order by EditionName ");

				drpEditionList.DataTextField = "EditionName";
				drpEditionList.DataValueField = "EditionID";
				drpEditionList.DataSource = dt;
				drpEditionList.DataBind();		
	
				drpEditionList.Items.FindByValue(drv["EditionID"].ToString()).Selected = true;
			}
			else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				LinkButton lnkbutDelete =(LinkButton) e.Item.FindControl("lnkbutDelete");
				lnkbutDelete.Attributes.Add("onclick","javascript:return confirm('Are you sure you want to delete this rule?')");
			}

			

		}

		private void dgRules_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			DataFunctions.Execute("delete from RuleDetails where RuleID = " + dgRules.DataKeys[e.Item.ItemIndex].ToString() + ";delete from Rules where RuleID = " + dgRules.DataKeys[e.Item.ItemIndex].ToString());
			
			dgRules.EditItemIndex = -1;
			LoadRulesGrid();
		}

		private void dgRules_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if (e.CommandName == "Add")
			{
				TextBox txtrulename = (TextBox) e.Item.FindControl("txtRuleNameF");
				DropDownList drpEditionList = (DropDownList) e.Item.FindControl("drpEditionListF");

				DataFunctions.Execute("insert into rules (RuleName, PublicationID, EditionID) values ('" + txtrulename.Text.Replace("'", "''") + "'," + getPublicationID() + "," + drpEditionList.SelectedValue + ")");
				LoadRulesGrid();
			}
		}

		private void dgRules_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{

			TextBox txtrulename = (TextBox) e.Item.FindControl("txtRuleName");
			DropDownList drpEditionList = (DropDownList) e.Item.FindControl("drpEditionList");

			DataFunctions.Execute("update Rules Set RuleName = '" +  txtrulename.Text.Replace("'", "''") + "', editionID = " + drpEditionList.SelectedValue + " where RuleID = " + dgRules.DataKeys[e.Item.ItemIndex].ToString());
			
			dgRules.EditItemIndex = -1;
			LoadRulesGrid();
			dgRules.ShowFooter = true;

		}


         */
	}
}
