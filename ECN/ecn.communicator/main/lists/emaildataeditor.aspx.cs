using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Text;
using MetaBuilders.WebControls;

namespace ecn.communicator.listsmanager
{	
	public partial class emaildataeditor : ECN_Framework.WebPageHelper
	{
		string customerID		= "";

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;			
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS; 
            Master.SubMenu = "";
            Master.HelpContent = "Email Data Manager";
            Master.HelpTitle = "<b>Edit Email Data</b> <p>Here you can edit your Defined Email Data</p>";	

            if(Request.QueryString["EmailID"] != null) {
                email_id.Text = Request.QueryString["EmailID"];
            }
            if(Request.QueryString["GroupID"] != null) {
                group_id.Text = Request.QueryString["GroupID"];
            }

            customerID = Master.UserSession.CurrentUser.CustomerID.ToString();
            string email_address = (ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(Convert.ToInt32(email_id.Text), Master.UserSession.CurrentUser)).EmailAddress;
            Master.Heading = "Groups > Manage Groups > Group Editor > Edit UDF Data for Email Address: " + email_address;

			if (!IsPostBack)
            {				
                LoadEmailData(Convert.ToInt32(email_id.Text), Convert.ToInt32(group_id.Text));
				LoadUDFGroupsGrid(Convert.ToInt32(email_id.Text), Convert.ToInt32(group_id.Text));
				
                EmailsGrid.Visible = true;
                if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.Edit))
                {
                    EmailsGrid.Columns[2].Visible = false;
                }
                else
                {
                    EmailsGrid.Columns[2].Visible = true;
                }
                
                string dfsID = "";
                loadUDF(Convert.ToInt32(group_id.Text));
				if(Request.QueryString["DFSID"]!=null)
                {
					dfsID = Request.QueryString["DFSID"].ToString();
                    LoadUDFHistoryData(email_id.Text,group_id.Text,dfsID);			
					UDFGroupsGrid.Visible = true;
				} 
                else 
                {
					TransactionHistoryPanel.Visible=false;
				}			
			}
        }     

		#region UDFGroupsGrid Edit / Cancel / Update Functions
		public void UDFGroupsGrid_Edit(Object sender, DataGridCommandEventArgs e) 
        {
			string dfsID = Request.QueryString["DFSID"];
			LoadUDFHistoryData(email_id.Text,group_id.Text,dfsID);	
		}
		#endregion

		#region EmailsGrid Edit / Cancel / Update Functions
        public void EmailsGrid_Edit(Object sender, DataGridCommandEventArgs e)  
        {
			string gdfID = Request.QueryString["GDFID"];
            EmailsGrid.EditItemIndex = e.Item.ItemIndex;
            LoadEmailData(Convert.ToInt32(email_id.Text),Convert.ToInt32(group_id.Text));
        }

        public void EmailsGrid_Cancel(Object sender, DataGridCommandEventArgs e)
        {
			string gdfID = Request.QueryString["GDFID"];
            EmailsGrid.EditItemIndex = -1;
            LoadEmailData(Convert.ToInt32(email_id.Text),Convert.ToInt32(group_id.Text));
        }
        
		public void EmailsGrid_Update(Object sender, DataGridCommandEventArgs e)  
        {
            int groupDataFieldsID =Convert.ToInt32(EmailsGrid.DataKeys[EmailsGrid.EditItemIndex].ToString());
            string value = ((TextBox)e.Item.Cells[1].Controls[0]).Text;
            updateUDFValue(groupDataFieldsID, value);
        }
		#endregion

        private void loadUDF(int groupID)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> gdfList=
            ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, Master.UserSession.CurrentUser);
            var result = (from src in gdfList
                          where src.DatafieldSetID==null
                         select src).ToList();
            drpUDF.DataValueField = "GroupDataFieldsID";
            drpUDF.DataTextField = "ShortName";
            drpUDF.DataSource = result;
            drpUDF.DataBind();
            drpUDF.Items.Insert(0, new ListItem("-Select-", "-Select-"));
        }
        
		private void LoadUDFGroupsGrid(int email_id, int group_id) 
        {
            List<ECN_Framework_Entities.Communicator.DataFieldSets> datafieldSets = ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByGroupID(group_id);
            var result = (from src in datafieldSets
                         select new
                         {
                             URLLink = "EmailID=" + email_id + "&GroupID=" + group_id + "&DFSID=" + src.DataFieldSetID,
                             Name= src.Name
                         }).ToList();
            if (result.Count > 0)
            {
                UDFGroupsGrid.DataSource = result;
				UDFGroupsGrid.DataBind();
				UDFGroupsGrid.Caption = "<div align=left class=tableHeader>Transactions</div>";
				UDFGroupsGrid.Visible = true;
			}
            else
            {
				UDFGroupsGrid.Visible = false;			
			}
		}

		private void LoadUDFHistoryData(string email_id, string group_id, string dfs_ID) 
        {
            DataTable historyData = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetTransUDFDataValues(Convert.ToInt32(customerID), Convert.ToInt32(group_id), email_id, Convert.ToInt32(dfs_ID), Master.UserSession.CurrentUser);
            historyData.DefaultView.Sort = "LastModifiedDate DESC";
		    historyData.Columns.Remove("EmailID");
            if (historyData.Columns.Count > 0)
            {
                historyData.Columns.Add("FixedSortDate", typeof(DateTime));
                foreach (DataRow row in historyData.Rows)
                {
                    string date = row["LastModifiedDate"].ToString();
                    if (!string.IsNullOrWhiteSpace(date))
                    {
                        row["FixedSortDate"] = Convert.ToDateTime(date);    
                    }
                }

                historyData.DefaultView.Sort = "FixedSortDate DESC";
                historyData.Columns.Remove("FixedSortDate");

                UDFHistoryGrid.DataSource = historyData.DefaultView.ToTable();
                UDFHistoryGrid.Caption = "";
                UDFHistoryGrid.DataBind();
                TransactionHistoryPanel.Visible = true;
                lblNoTransData.Visible = false;
            }
            else
            {
                TransactionHistoryPanel.Visible = false;
                lblNoTransData.Visible = true;
            }

		}

        private void LoadEmailData(int email_id, int group_id)
        {
            EmailsGrid.DataSource = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetStandaloneUDFDataValues(group_id, email_id, Master.UserSession.CurrentUser);
            EmailsGrid.DataBind();
        }

        protected void btnAddUDFValue_Click(object sender, EventArgs e)
        {
            if (!drpUDF.SelectedValue.Equals("-Select-"))
            {
                int groupDataFieldsID = Convert.ToInt32(drpUDF.SelectedValue);
                string value = txtUDFValue.Text;
                updateUDFValue(groupDataFieldsID, value);
                txtUDFValue.Text = "";
                drpUDF.SelectedValue = "-Select-";
            }
        }

        private void updateUDFValue(int groupDataFieldsID, string value)
        {
            ECN_Framework_Entities.Communicator.Email email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(Convert.ToInt32(Request.QueryString["EmailID"].ToString()), Master.UserSession.CurrentUser);
            ECN_Framework_Entities.Communicator.EmailGroup emailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup.GetByEmailIDGroupID(email.EmailID, Convert.ToInt32(Request.QueryString["GroupID"].ToString()), Master.UserSession.CurrentUser);
            StringBuilder xmlUDF = new StringBuilder("");
            xmlUDF.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML><row><ea>" + email.EmailAddress + "</ea><udf id=\"" + groupDataFieldsID.ToString() + "\"><v><![CDATA[" + value + "]]></v></udf></row></XML>");
            StringBuilder xmlProfile = new StringBuilder("");
            xmlProfile.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML><Emails>");
            xmlProfile.Append("<emailaddress>" + email.EmailAddress + "</emailaddress>");
            xmlProfile.Append("</Emails></XML>");
            ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(Master.UserSession.CurrentUser, Master.UserSession.CurrentUser.CustomerID, Convert.ToInt32(Request.QueryString["GroupID"].ToString()), xmlProfile.ToString(), xmlUDF.ToString(), emailGroup.FormatTypeCode, emailGroup.SubscribeTypeCode, false, "", "Ecn.communicator.main.lists.emaildataeditor.updateUDFValue");
            EmailsGrid.EditItemIndex = -1;
            string gdfID = Request.QueryString["GDFID"];
            LoadEmailData(Convert.ToInt32(email_id.Text), Convert.ToInt32(group_id.Text));
        }
	}
}
