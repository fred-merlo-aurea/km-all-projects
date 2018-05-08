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
using System.Net.Mail;
using System.Text;
using ecn.communicator.classes;

namespace ecn.activityengines
{
	
	
	
	public partial class ManageSubscriptions : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lbEmailID;

		protected System.Web.UI.WebControls.RequiredFieldValidator rfv1;
		protected System.Web.UI.WebControls.RequiredFieldValidator rfv2;

		int EmailID = 0;
		string Emailaddress = string.Empty;
		int GroupID = 0;

		private string getQSPreference() 
		{
			string pref = string.Empty;
			try 
			{
				pref = Request.QueryString["prefrence"].ToString();
			}
			catch
			{}
			return pref;
		}
		
		private string getQSe() 
		{
			string e = string.Empty;
			try 
			{
				e = Request.QueryString["e"].ToString();
			}
			catch{}
			return e;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string accountsdb		= ConfigurationManager.AppSettings["accountsdb"];

			lbMessage.Text="";
			lbllistHeader.Text = "List Opt-in Preferences";
			lblemailHeader.Text = "Email Preferences";

            try
            {
                string[] Split = getQSe().Split(new Char[] { ',' });

                if (Split.Length > 1)
                {
                    try
                    {
                        Emailaddress = Convert.ToString(Split[0]);
                        EmailID = Convert.ToInt32(Split[1]);

                        if (getQSPreference().ToLower() == "email")
                        {
                            GroupID = Convert.ToInt32(Split[2]);
                        }
                    }
                    catch
                    {
                        EmailID = 0;
                        Emailaddress = string.Empty;
                    }

                }

                string header = string.Empty;
                string footer = string.Empty;
                string TemplateQuery = string.Empty;
                string CustomerID = string.Empty;

                if (!IsPostBack)
                {
                    if (EmailID > 0 && Emailaddress.Length > 0)
                    {
                        DataTable dt = DataFunctions.GetDataTable(" select  e.FirstName, e.Lastname, e.title, e.company, e.address, e.city, e.state, e.zip, e.country, convert(varchar(10),e.Birthdate,101) as birthdate, e.voice, e.mobile, e.gender, c.CustomerID, c.CustomerName from emails e join ecn5_accounts..customer c on e.customerID = c.customerID  where c.IsDeleted = 0 and e.EmailID = " + EmailID + " and emailaddress = '" + Emailaddress.Replace("'", "''").ToString() + "'");

                        if (dt.Rows.Count > 0)
                        {
                            lblEmail.Text = Emailaddress;
                            CustomerID = dt.Rows[0]["CustomerID"].ToString();
                            txtFirstname.Text = dt.Rows[0]["FirstName"].ToString();
                            txtLastName.Text = dt.Rows[0]["Lastname"].ToString();
                            txtJobTitle.Text = dt.Rows[0]["title"].ToString();
                            txtCompany.Text = dt.Rows[0]["company"].ToString();
                            txtAddress.Text = dt.Rows[0]["address"].ToString();
                            txtCity.Text = dt.Rows[0]["city"].ToString();
                            txtState.Text = dt.Rows[0]["state"].ToString();
                            txtZip.Text = dt.Rows[0]["zip"].ToString();
                            txtCountry.Text = dt.Rows[0]["country"].ToString();
                            txtMobile.Text = dt.Rows[0]["mobile"].ToString();
                            txtPhone.Text = dt.Rows[0]["voice"].ToString();
                            try
                            {
                                txtDOB.Text = dt.Rows[0]["Birthdate"].ToString();
                            }
                            catch
                            { }

                            try
                            {
                                TemplateQuery = " SELECT HeaderSource FROM " + accountsdb + ".dbo.CustomerTemplate WHERE CustomerID = " + CustomerID + " AND TemplateTypeCode='ManageSubHdr' and IsActive=1 and IsDeleted = 0";
                                header = DataFunctions.ExecuteScalar(TemplateQuery).ToString();
                            }
                            catch { }

                            try
                            {
                                TemplateQuery = " SELECT HeaderSource as FooterSource FROM " + accountsdb + ".dbo.CustomerTemplate WHERE CustomerID = " + CustomerID + " AND TemplateTypeCode='ManageSubftr' and IsActive=1 and IsDeleted = 0";
                                footer = DataFunctions.ExecuteScalar(TemplateQuery).ToString();
                            }
                            catch { }

                            try
                            {
                                rbGender.ClearSelection();
                                if (dt.Rows[0]["gender"].ToString().ToLower() == "male" || dt.Rows[0]["gender"].ToString().ToLower() == "m")
                                {
                                    rbGender.Items.FindByValue("Male").Selected = true;
                                }

                                if (dt.Rows[0]["gender"].ToString().ToLower() == "female" || dt.Rows[0]["gender"].ToString().ToLower() == "f")
                                {
                                    rbGender.Items.FindByValue("Female").Selected = true;
                                }
                            }
                            catch { }

                            pnlManange.Visible = true;

                            BindGrid();
                        }
                        else
                        {
                            pnlManange.Visible = false;
                            lbMessage.Text = "Invalid User Profile. Please click on the 'Manage my subscription' link in the email message that you received";
                        }
                    }

                    if (header == string.Empty)
                    {
                        TemplateQuery = " SELECT HeaderSource FROM " + accountsdb + ".dbo.CustomerTemplate WHERE CustomerID = 1 AND TemplateTypeCode='ManageSubHdr' and IsActive=1 and IsDeleted = 0";
                        header = DataFunctions.ExecuteScalar(TemplateQuery).ToString();
                        TemplateQuery = " SELECT HeaderSource as FooterSource FROM " + accountsdb + ".dbo.CustomerTemplate WHERE CustomerID = 1 AND TemplateTypeCode='ManageSubFtr' and IsActive=1 and IsDeleted = 0";
                        footer = DataFunctions.ExecuteScalar(TemplateQuery).ToString();

                        //pnlManange.Visible=false;
                        //lbMessage.Text = "Invalid User Profile. Please click on the 'Manage my subscription' link in the email message that you received";
                    }

                    MyHeader.Text = header;
                    MyFooter.Text = footer;
                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "ManageSubscriptions.Page_Load", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                //Helper.LogCriticalError(ex, "ManageSubscriptions.Page_Load");
                //NotifyAdmin(ex);
                pnlManange.Visible = false;
                lbMessage.Text = "Error loading Subscription Manager. Please click on the 'Manage my subscription' link in the email message that you received";
            }
		}

        private string CreateNote()
        {
            StringBuilder adminEmailVariables = new StringBuilder();
            //string admimEmailBody = string.Empty;

            try
            {
                adminEmailVariables.AppendLine("<br><b>Email ID:</b>&nbsp;" + EmailID);
            }
            catch (Exception)
            {
            }
            return adminEmailVariables.ToString();
        }

        //private void NotifyAdmin(Exception ex)
        //{
        //    StringBuilder adminEmailVariables = new StringBuilder();
        //    string admimEmailBody = string.Empty;

        //    adminEmailVariables.AppendLine("<br><b>Blast ID:</b>&nbsp;" + EmailID);

        //    admimEmailBody = ActivityError.CreateMessage(ex, Request, adminEmailVariables.ToString());

        //    Helper.SendMessage("Error in Activity Engine: Manage Subscriptions", admimEmailBody);
        //}
	
		private void BindGrid()
		{
			if (getQSPreference().ToLower() == "both")
			{
				plEmail.Visible=true;
				plList.Visible=true;

				DataTable dtemail = DataFunctions.GetDataTable("select distinct  eg.GroupID, GroupName from emailgroups eg join groups g on eg.groupID = g.groupID join groupdatafields gdf on gdf.groupID = g.groupID where subscribeTypeCode='S' and gdf.IsDeleted = 0 and emailID = " + EmailID + " and ispublic='y' order by GroupName");
				dtSubscriptionGrid.DataSource = dtemail;
				dtSubscriptionGrid.DataBind();

				DataTable dtlist = DataFunctions.GetDataTable("select  emailgroupID, GroupName, isnull(groupdescription,'') as description, subscribeTypeCode, case when FormatTypeCode = 'html' then 'true' else 'false' end as isHTML, case when FormatTypeCode = 'html' then 'false' else 'true' end as isText, case when subscribeTypeCode='S' then 'true' else 'false' end as IsSubscribed from emailgroups eg join groups g on eg.groupID = g.groupID where emailID = " + EmailID + " and PublicFolder  = 1 order by GroupName");
				dgSubscriptionGrid.DataSource = dtlist;
				dgSubscriptionGrid.DataBind();
			}
			else if (getQSPreference().ToLower() == "email")
			{
				plEmail.Visible=true;
				plList.Visible=false;

                DataTable dt = DataFunctions.GetDataTable("select distinct  eg.GroupID, GroupName from emailgroups eg join groups g on eg.groupID = g.groupID join groupdatafields gdf on gdf.groupID = g.groupID where g.GroupID = " + GroupID + " and emailID = " + EmailID + " and gdf.IsDeleted = 0 and ispublic='y' order by GroupName");
				dtSubscriptionGrid.DataSource = dt;
				dtSubscriptionGrid.DataBind();
			}
			else 
			{
				plEmail.Visible=false;
				plList.Visible=true;

				DataTable dt = DataFunctions.GetDataTable("select  emailgroupID, GroupName, isnull(groupdescription,'') as description, subscribeTypeCode, case when FormatTypeCode = 'html' then 'true' else 'false' end as isHTML, case when FormatTypeCode = 'html' then 'false' else 'true' end as isText, case when subscribeTypeCode='S' then 'true' else 'false' end as IsSubscribed from emailgroups eg join groups g on eg.groupID = g.groupID where emailID = " + EmailID + " and PublicFolder  = 1 order by GroupName");
				dgSubscriptionGrid.DataSource = dt;
				dgSubscriptionGrid.DataBind();
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
		
//		this.btnSubmit.Click+= new System.EventHandler(this.btnSubmit_Click);
//		this.dgSubscriptionGrid.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgSubscriptionGrid_ItemDataBound);
//		this.dtSubscriptionGrid.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dtSubscriptionGrid_ItemDataBound);

		private void InitializeComponent()
		{    
			this.dgSubscriptionGrid.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgSubscriptionGrid_ItemDataBound);
			this.dtSubscriptionGrid.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dtSubscriptionGrid_ItemDataBound);
		}
		#endregion

		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{

			string FormatTypeCode = "";
			string SubsribeTypeCode = "";

            try
            {
                if (EmailID > 0)
                {
                    DataFunctions.Execute("update emails set firstname='" + txtFirstname.Text.Replace("'", "''") + "', LastName='" + txtLastName.Text.Replace("'", "''") + "', title='" + txtJobTitle.Text.Replace("'", "''") + "', company='" + txtCompany.Text.Replace("'", "''") + "', voice='" + txtPhone.Text.Replace("'", "''") + "', mobile='" + txtMobile.Text.Replace("'", "''") + "', address='" + txtAddress.Text.Replace("'", "''") + "', city='" + txtCity.Text.Replace("'", "''") + "', state='" + txtState.Text.Replace("'", "''") + "', zip='" + txtZip.Text.Replace("'", "''") + "', country='" + txtCountry.Text.Replace("'", "''") + "'" + (txtDOB.Text.Trim() != "" ? ", Birthdate='" + txtDOB.Text + "'" : ", Birthdate=NULL") + " " + (rbGender.SelectedIndex > -1 ? ", Gender='" + rbGender.SelectedItem.Value + "'" : "") + " where emailID = " + EmailID);
                }

                if (getQSPreference().ToLower() == "email" || getQSPreference().ToLower() == "both")
                {
                    DataListItem dlItem = null;
                    CheckBoxList chkBoxlist = null;

                    for (int i = 0; i < dtSubscriptionGrid.Items.Count; i++)
                    {
                        dlItem = dtSubscriptionGrid.Items[i];

                        chkBoxlist = (CheckBoxList)dlItem.FindControl("cbList");

                        foreach (ListItem li in chkBoxlist.Items)
                        {
                            string datavalue = "N";
                            if (li.Selected)
                                datavalue = "Y";

                            //Response.Write(li.Text.ToString() + " / " + li.Value.ToString() + " / " + li.Selected + "<BR>");
                            DataFunctions.Execute("if exists (select EmailDataValuesID from emaildatavalues where EmailID = " + EmailID.ToString() + " and GroupDatafieldsID = " + li.Value.ToString() + ")" +
                                                    " update emaildatavalues set datavalue = '" + datavalue + "' where EmailID = " + EmailID.ToString() + " and GroupDatafieldsID = " + li.Value.ToString() +
                                                    " else " +
                                                    " insert into emaildatavalues values (" + EmailID.ToString() + "," + li.Value.ToString() + ",'" + datavalue + "',getdate(), null, null)");

                        }
                    }
                }

                if (getQSPreference().ToLower() == "list" || getQSPreference().ToLower() == "both" || getQSPreference().ToLower() == "")
                {
                    CheckBox chkBox = null;
                    RadioButton rb = null;
                    DataGridItem dgItem = null;

                    for (int i = 0; i < dgSubscriptionGrid.Items.Count; i++)
                    {
                        dgItem = dgSubscriptionGrid.Items[i];
                        chkBox = (CheckBox)dgItem.FindControl("chksubscription");
                        rb = (RadioButton)dgItem.FindControl("rbHTML");

                        if ((rb != null) && rb.Checked)
                            FormatTypeCode = "html";
                        else
                            FormatTypeCode = "text";

                        if ((chkBox != null) && chkBox.Checked)
                            SubsribeTypeCode = "S";
                        else
                            SubsribeTypeCode = "U";

                        DataFunctions.Execute("update emailgroups set subscribeTypeCode='" + SubsribeTypeCode + "', FormatTypeCode = '" + FormatTypeCode + "', LastChanged = getdate() where EmailGroupID = " + dgSubscriptionGrid.DataKeys[dgItem.ItemIndex].ToString());

                    }
                }
                lbMessage.Text = "Your subscription changes have been updated successfully.";
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogCriticalError(ex, "ManageSubscriptions.btnSubmit_Click", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), CreateNote());
                //Helper.LogCriticalError(ex, "ManageSubscriptions.btnSubmit_Click");
                //NotifyAdmin(ex);
                lbMessage.Text = "Error updating subscription. Please click on the 'Manage my subscription' link in the email message that you received";
            }
		}

		private void dgSubscriptionGrid_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e) 
		{
			RadioButton rb = null;
			string grpName = "";
			CheckBox chkBox = null;			

			if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer) 
			{
				chkBox = (CheckBox)e.Item.FindControl("chksubscription");
				grpName = "grp" + dgSubscriptionGrid.DataKeys[e.Item.ItemIndex].ToString();
				rb = (RadioButton) e.Item.FindControl("rbHTML");
				rb.GroupName = grpName;
				rb.Attributes.Add("onclick","javascript:document.getElementById('" + chkBox.ClientID + "').checked = true");
				rb = (RadioButton) e.Item.FindControl("rbText");
				rb.GroupName = grpName;
				rb.Attributes.Add("onclick","javascript:document.getElementById('" + chkBox.ClientID + "').checked = true");

			}	
		}

		
		private void dtSubscriptionGrid_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e) 
		{
            DataTable dt = DataFunctions.GetDataTable("select distinct gdf.groupdatafieldsID, shortname, longName, case when isnull(datavalue,'') = 'y' then 'true' else 'false' end as 'selected' from groupdatafields gdf left outer join emaildatavalues edv on gdf.groupdatafieldsID = edv.groupdatafieldsID and emailID=" + EmailID.ToString() + " where ispublic='y' and gdf.IsDeleted = 0 and  groupID = " + dtSubscriptionGrid.DataKeys[e.Item.ItemIndex].ToString() + " order by shortname");

			CheckBoxList cbl = (CheckBoxList) e.Item.FindControl("cbList")	;
			for(int i=0;i<dt.Rows.Count;i++)
			{
				ListItem li = new ListItem(dt.Rows[i]["longName"].ToString(),dt.Rows[i]["groupdatafieldsID"].ToString());
				li.Selected = Convert.ToBoolean(dt.Rows[i]["selected"]);
				cbl.Items.Add(li);
			}
		}
	}
}
