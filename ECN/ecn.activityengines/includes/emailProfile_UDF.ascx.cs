using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.communicator.classes;
using ecn.common.classes;
using System.Collections.Generic;
using KM.Framework.Web.WebForms.EmailProfile;

namespace ecn.activityengines.includes
{
    public partial class emailProfile_UDF : EmailProfileBaseControl
    {
        private const string HtmlAttributeClass = "class";
        private const string HtmlAttributeClassValue = "selected";

        private string _emailId = string.Empty;
        private string _customerId = string.Empty;
        private string _groupId = string.Empty;

        protected override Label lblResultMessage
        {
            get
            {
                return this.messageLabel;
            }
        }

        DataTable groupsSubscribedDT = null;
        DataTable groupsWithSimpleUDFValuesDT = null;
        private static KMPlatform.Entity.User user;

        protected void Page_Load(object sender, EventArgs e)
        {
            _emailId = GetFromQueryString("eID", "EmailAddress specified does not Exist. Please click on the 'Referral Program' link in the email message that you received");
            _groupId = GetFromQueryString("gid", "GroupID specified does not Exist. Please click on the 'Referral Program' link in the email message that you received");
            _customerId = GetFromQueryString("cid", "CustomerID specified does not Exist. Please click on the 'Referral Program' link in the email message that you received");

            user = KMPlatform.BusinessLogic.User.GetByAccessKey(ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);
            simpleUDFLink.Attributes.Add(HtmlAttributeClass, HtmlAttributeClassValue);
            transactionalUDFLink.Attributes.Add(HtmlAttributeClass, string.Empty);
        }

        #region simpleUDFGrid Edit / Cancel / Update Functions
        /*public void simpleUDFGrid_Edit(Object sender, DataGridCommandEventArgs e)  {

			// Set the EditItemIndex property to the index of the item clicked 
			// in the DataGrid control to enable editing for that item. Be sure
			// to rebind the DateGrid to the data source to refresh the control.
			string gdfID = Request.QueryString["GDFID"];
			simpleUDFGrid.EditItemIndex = e.Item.ItemIndex;
			loadSimpleUDFs(getEmailID(),GroupID);

		}
		public void simpleUDFGrid_Cancel(Object sender, DataGridCommandEventArgs e) {
			// Set the EditItemIndex property to -1 to exit editing mode. 
			// Be sure to rebind the DateGrid to the data source to refresh
			// the control.
			string gdfID = Request.QueryString["GDFID"];
			simpleUDFGrid.EditItemIndex = -1;
			loadSimpleUDFs(getEmailID(),GroupID);
		}
		public void simpleUDFGrid_Command(Object sender, DataGridCommandEventArgs e){
			switch(((LinkButton)e.CommandSource).CommandName){
				case "Delete":
					//                    DeleteItem(e);
					break;
					// Add other cases here, if there are multiple ButtonColumns in 
					// the DataGrid control.
				default:
					// Do nothing.
					break;
			}
		}
		public void simpleUDFGrid_Update(Object sender, DataGridCommandEventArgs e)  {
			// Get the correct value and update it from the field in value 0
			//         throw new System.Exception( " event = " + EmailsGrid.EditItemIndex.ToString());
			string email_data_id = simpleUDFGrid.DataKeys[simpleUDFGrid.EditItemIndex].ToString();
			TextBox valueText = (TextBox)e.Item.Cells[1].Controls[0];
			SecurityAccess.canI("EmailDataValues",email_data_id);
			DataFunctions.Execute("update EmailDataValues set DataValue = '" + valueText.Text + "' where EmailDataValuesID= " + email_data_id);
			simpleUDFGrid.EditItemIndex = -1;
			loadSimpleUDFs(getEmailID(),GroupID);
		}*/
        #endregion

        #region Load Simple UDF Grid
        private void loadSimpleUDFs()
        {
            if (_groupId != string.Empty && _emailId != string.Empty)
            {
                SqlConnection dbConn = new SqlConnection(ConfigurationManager.AppSettings["connString"].ToString());
                SqlCommand simpleUDFCmd = new SqlCommand("sp_GetEmailActivitySimpleUDFDataValues", dbConn);
                simpleUDFCmd.CommandType = CommandType.StoredProcedure;

                int groupId;
                int.TryParse(_groupId, out groupId);
                simpleUDFCmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
                simpleUDFCmd.Parameters["@GroupID"].Value = groupId;

                int emailId;
                int.TryParse(_emailId, out emailId);
                simpleUDFCmd.Parameters.Add(new SqlParameter("@EmailID", SqlDbType.Int));
                simpleUDFCmd.Parameters["@EmailID"].Value = emailId;

                SqlDataAdapter da = new SqlDataAdapter(simpleUDFCmd);
                DataSet ds = new DataSet();
                dbConn.Open();
                da.Fill(ds, "sp_GetEmailActivitySimpleUDFDataValues");
                dbConn.Close();
                groupsSubscribedDT = ds.Tables[0];
                groupsWithSimpleUDFValuesDT = ds.Tables[1];

                if (groupsSubscribedDT.Rows.Count > 0)
                {
                    simpleUDFDataList.DataSource = groupsSubscribedDT.DefaultView;
                    simpleUDFDataList.DataBind();
                }
                else
                {
                    messageLabel.Visible = true;
                    messageLabel.Text = "<img src=\"http://images.ecn5.com/images/small-alertIcon.gif\" />&nbsp;<sup>No Simple UDF's available at this time</sup>";
                    //simpleUDFGrid.Visible = false;
                }
            }
        }

        protected void simpleUDFDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataGrid simpleUDFGrid = e.Item.FindControl("simpleUDFGrid") as DataGrid;
                Label noSimpleUDFLabel = e.Item.FindControl("noSimpleUDFLabel") as Label;

                DataRow[] drs = groupsWithSimpleUDFValuesDT.Select("GroupID = " + (int)simpleUDFDataList.DataKeys[(int)e.Item.ItemIndex]);

                if (drs.Length > 0)
                {
                    noSimpleUDFLabel.Visible = false;
                    simpleUDFGrid.Visible = true;

                    DataTable dt = new DataTable();
                    dt.Columns.Add("GroupID");
                    dt.Columns.Add("GroupName");
                    dt.Columns.Add("ShortName");
                    dt.Columns.Add("LongName");
                    dt.Columns.Add("DataValue");

                    foreach (DataRow dr in drs)
                    {
                        dt.ImportRow(dr);
                    }
                    simpleUDFGrid.DataSource = dt;
                    simpleUDFGrid.DataBind();
                }
                else
                {
                    e.Item.Dispose();
                    noSimpleUDFLabel.Visible = true;
                    simpleUDFGrid.Visible = false;
                }
            }
        }
        #endregion

        #region Load Transactional UDF's
        private void loadTransactionaUDFs()
        {
            if (groupsSubscribedDT.Rows.Count > 0)
            {
                transUDFDataList.DataSource = groupsSubscribedDT.DefaultView;
                transUDFDataList.DataBind();

                foreach (DataListItem dl in transUDFDataList.Items)
                {
                    Repeater repeater = (Repeater)dl.FindControl("dataFieldsSetsRepeater");
                    if (repeater.Items.Count > 0)
                    {
                        var groupDataFieldSetsDT = DataFunctions.GetDataTable("SELECT df.DataFieldSetID AS 'DFSID', df.[Name] AS 'DFSName' FROM DataFieldSets df JOIN Groups g ON df.GroupID = g.GroupID AND g.customerID = " + _customerId + " AND g.GroupID = " + transUDFDataList.DataKeys[dl.ItemIndex].ToString());
                        DataRow[] dr = groupDataFieldSetsDT.Select();

                        for (int i = 0; i < repeater.Items.Count; i++)
                        {
                            DataGrid transUDFGrid = repeater.Items[i].FindControl("transUDFGrid") as DataGrid;

                            SqlConnection dbConn = new SqlConnection(ConfigurationManager.AppSettings["connString"].ToString());
                            SqlCommand UDFDataListCmd = new SqlCommand("sp_GetUDFDataValues", dbConn);
                            UDFDataListCmd.CommandTimeout = 100;
                            UDFDataListCmd.CommandType = CommandType.StoredProcedure;

                            //--GroupID
                            UDFDataListCmd.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
                            UDFDataListCmd.Parameters["@GroupID"].Value = Convert.ToInt32(transUDFDataList.DataKeys[dl.ItemIndex].ToString());
                            //--CustomerID
                            int customerId;
                            int.TryParse(_customerId, out customerId);
                            UDFDataListCmd.Parameters.Add(new SqlParameter("@CustomerID", SqlDbType.Int));
                            UDFDataListCmd.Parameters["@CustomerID"].Value = customerId;
                            //--UDF EmailID
                            UDFDataListCmd.Parameters.Add(new SqlParameter("@UDFEmailID", SqlDbType.VarChar));
                            UDFDataListCmd.Parameters["@UDFEmailID"].Value = _emailId;
                            //--DataFieldSetID
                            UDFDataListCmd.Parameters.Add(new SqlParameter("@DatafieldSetID", SqlDbType.Int));
                            UDFDataListCmd.Parameters["@DatafieldSetID"].Value = Convert.ToInt32(dr[i]["DFSID"].ToString());

                            SqlDataAdapter da = new SqlDataAdapter(UDFDataListCmd);
                            DataSet ds = new DataSet();
                            da.Fill(ds, "sp_GetUDFDataValues");
                            dbConn.Close();

                            try
                            {
                                DataTable historyData = ds.Tables[0];
                                historyData.Columns.Remove("EmailID");
                                transUDFGrid.DataSource = historyData.DefaultView;
                                transUDFGrid.DataBind();
                            }
                            catch { }
                        }
                    }
                }
            }
            else
            {
                messageLabel.Visible = true;
                messageLabel.Text = "<img src=\"http://images.ecn5.com/images/small-alertIcon.gif\" />&nbsp;<sup>No Transactional UDF's available at this time</sup>";
            }
        }

        protected void transUDFDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataGrid transUDFGrid = e.Item.FindControl("transUDFGrid") as DataGrid;
                Label noTransUDFLabel = e.Item.FindControl("noTransUDFLabel") as Label;
                Repeater dataFieldsSetsRepeater = e.Item.FindControl("dataFieldsSetsRepeater") as Repeater;

                var query = string.Format("SELECT df.DataFieldSetID AS 'DFSID', df.[Name] AS 'DFSName' FROM DataFieldSets df JOIN Groups g ON df.GroupID = g.GroupID AND g.customerID = {0} AND g.GroupID = {1}", _customerId, (int)transUDFDataList.DataKeys[(int)e.Item.ItemIndex]);
                var groupDataFieldSetsDT = DataFunctions.GetDataTable(query);

                if (groupDataFieldSetsDT.Rows.Count > 0)
                {
                    noTransUDFLabel.Visible = false;
                    dataFieldsSetsRepeater.Visible = true;
                    dataFieldsSetsRepeater.DataSource = groupDataFieldSetsDT.DefaultView;
                    dataFieldsSetsRepeater.DataBind();
                }
                else
                {
                    noTransUDFLabel.Visible = true;
                    dataFieldsSetsRepeater.Visible = false;
                }
            }
        }

        protected void dataFieldsSetsRepeater_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label dataFieldSetIDLabel = e.Item.FindControl("dataFieldSetIDLabel") as Label;
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


        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.

        private void InitializeComponent()
        {
            loadSimpleUDFs();
            //loadTransactionaUDFs();
        }
        #endregion


        protected void transactionalUDFLink_Click(object sender, EventArgs e)
        {
            simpleUDFLink.Attributes.Add("class", "");
            transactionalUDFLink.Attributes.Add("class", "selected");

            simpleUDFPanel.Visible = false;
            simpleUDFDataList.Dispose();

            this.transUDFPanel.Visible = true;
            loadTransactionaUDFs();
        }

        protected void simpleUDFLink_Click(object sender, EventArgs e)
        {
            simpleUDFLink.Attributes.Add("class", "selected");
            transactionalUDFLink.Attributes.Add("class", "");

            transUDFPanel.Visible = false;
            transUDFDataList.Dispose();

            simpleUDFPanel.Visible = true;
            loadSimpleUDFs();
        }
    }
}
