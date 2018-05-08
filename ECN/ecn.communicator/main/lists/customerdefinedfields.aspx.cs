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
using System.Linq;
using System.Transactions;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.listsmanager
{
    public partial class customerdefinedfields : ECN_Framework.WebPageHelper
    {
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
            Master.Heading = "Groups > Manage Groups > User Defined Fields";
            Master.HelpContent = "<img align='right' src=/ecn.images/images/icogroups.gif><b>Edit Email Data Fields</b> <br />Here you can edit your Defined Email Data Fields";
            Master.HelpTitle = "Email Data Fields Manager";
            pnlAddTransNameMsg.Visible = false;
            lblAddTransNameMsg.Text = "";
            pnlCopyUDFMsg.Visible = false;
            lblCopyUDFMsg.Text = "";
            isPublicChkbox.Enabled = ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.EmailProfilePreferences);

            LoadUDFGrid();
            if (!Page.IsPostBack)
            {
                if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupUDFs, KMPlatform.Enums.Access.Edit))
                {
                    btnAddTransaction.Visible = true;
                    btnCopyUDF.Visible = true;
                    pnlAddUDF.Visible = true;
                    CustomDataGrid.Columns[6].Visible = true;
                    loadTransDropDown();
                }
                else
                {
                    btnAddTransaction.Visible = false;
                    btnCopyUDF.Visible = false;
                    pnlAddUDF.Visible = false;
                    CustomDataGrid.Columns[6].Visible = false;
                }
            }
        }

        protected void loadTransDropDown()
        {
            List<ECN_Framework_Entities.Communicator.DataFieldSets> dataFieldSetsList = ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByGroupID(Convert.ToInt32(Request.QueryString["GroupID"].ToString()));

            if (dataFieldSetsList.Count > 0)
            {
                transGroup.Visible = true;
                drpTransactionName.DataSource = dataFieldSetsList;
                drpTransactionName.DataBind();
                drpTransactionName.Items.Insert(0, new ListItem("--- Select Transaction Name ---", "0"));
            }
        }

        private void LoadUDFGrid()
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> groupDataFieldsList =
               ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(Convert.ToInt32(Request.QueryString["GroupID"].ToString()), Master.UserSession.CurrentUser);

            var resultSet = (from src in groupDataFieldsList
                             select new
                             {
                                 IsPublic = src.IsPublic,
                                 GroupDatafieldsID = src.GroupDataFieldsID,
                                 ShortName = src.ShortName,
                                 GroupID = src.GroupID,
                                 LongName = src.LongName,
                                 CodeSnippet = "%%" + src.ShortName + "%%",
                                 GroupingName = (src.DatafieldSetID == null) ? "" : (ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByDataFieldsetID(src.DatafieldSetID.Value, src.GroupID, false)).Name,
                                 Transactional = (src.DatafieldSetID == null) ? "N" : "Y"
                             }).ToList();

            CustomDataGrid.DataSource = resultSet;
            CustomDataGrid.DataBind();
        }

        public void CustomDataGrid_Command(Object sender, DataGridCommandEventArgs e)
        {
            int GroupDatafieldsID = (int)CustomDataGrid.DataKeys[(int)e.Item.ItemIndex];
            DeleteFieldData(GroupDatafieldsID);
            LoadUDFGrid();
        }

        public void ConfirmUDFDelete_ItemDataBound(Object sender, DataGridItemEventArgs e)
        {
            e.Item.Cells[7].Attributes.Add("onclick", "javascript:return confirm('Are you sure you want to delete this UDF and associated Data in all the Profiles?')");
        }

        private void DeleteFieldData(int groupDataFieldId)
        {
            try
            {
                ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Delete(groupDataFieldId, Convert.ToInt32(Request.QueryString["GroupID"].ToString()), Master.UserSession.CurrentUser);
                ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.Delete(groupDataFieldId);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }

        protected void add_button_Click(object sender, System.EventArgs e)
        {
            string shortNameTxt = short_name.Text;
            shortNameTxt = shortNameTxt.Replace(" ", "_");
            string isPublic = isPublicChkbox.Checked ? "Y" : "N";
            ECN_Framework_Entities.Communicator.GroupDataFields groupDataFeilds = new ECN_Framework_Entities.Communicator.GroupDataFields();
            groupDataFeilds.ShortName = StringFunctions.CleanString(shortNameTxt);
            groupDataFeilds.LongName = StringFunctions.CleanString(long_name.Text);
            groupDataFeilds.GroupID = Convert.ToInt32(Request.QueryString["GroupID"].ToString());
            groupDataFeilds.IsPublic = isPublic;
            groupDataFeilds.CreatedUserID = Master.UserSession.CurrentUser.UserID;
            groupDataFeilds.CustomerID = Master.UserSession.CurrentUser.CustomerID;
            if (transGroup.Visible && Convert.ToInt32(drpTransactionName.SelectedItem.Value) > 0)
            {
                groupDataFeilds.DatafieldSetID = Convert.ToInt32(drpTransactionName.SelectedItem.Value);
            }

            if (chkUseDefaultValue.Checked && ddlDefaultType.SelectedValue.ToString().ToLower().Equals("default") && txtDefaultValue.Text.Trim().Length == 0)
            {
                setECNError(new ECNException(new List<ECNError>() { new ECNError(Enums.Entity.GroupDataFieldsDefault, Enums.Method.Validate, "Please enter a value for Default Value") }));
                return;
            }

            try
            {
                groupDataFeilds.GroupDataFieldsID = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(groupDataFeilds, Master.UserSession.CurrentUser);

                if (chkUseDefaultValue.Checked)
                {
                    ECN_Framework_Entities.Communicator.GroupDataFieldsDefault gdfd = new ECN_Framework_Entities.Communicator.GroupDataFieldsDefault();
                    gdfd.GDFID = groupDataFeilds.GroupDataFieldsID;
                    if (ddlDefaultType.SelectedValue.ToString().ToLower().Equals("default"))
                    {
                        gdfd.DataValue = txtDefaultValue.Text.Trim();
                        gdfd.SystemValue = "";
                    }
                    else
                    {
                        gdfd.DataValue = "";
                        gdfd.SystemValue = ddlSystemValues.SelectedValue.ToString();
                    }
                    ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.Save(gdfd);
                }
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }


            short_name.Text = "";
            long_name.Text = "";
            pnlDefaultValue.Visible = false;
            ddlDefaultType.SelectedValue = "default";
            txtDefaultValue.Text = string.Empty;
            ddlSystemValues.SelectedIndex = 0;
            chkUseDefaultValue.Checked = false;

            if (transGroup.Visible)
                drpTransactionName.SelectedIndex = 0;

            LoadUDFGrid();

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ECN_Framework_Entities.Communicator.DataFieldSets dataFieldSets = new ECN_Framework_Entities.Communicator.DataFieldSets();
            dataFieldSets.GroupID = Convert.ToInt32(Request.QueryString["GroupID"]);
            dataFieldSets.MultivaluedYN = "Y";
            dataFieldSets.Name = txtTransactionName.Text;
            try
            {
                ECN_Framework_BusinessLayer.Communicator.DataFieldSets.Save(dataFieldSets, Master.UserSession.CurrentUser);
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
            mpeAddTransaction.Hide();
            loadTransDropDown();
            txtTransactionName.Text = "";
        }

        protected void btnAddTransaction_Click(object sender, EventArgs e)
        {
            txtTransactionName.Text = "";
            mpeAddTransaction.Show();
        }

        protected void btnCopyUDF_Click(object sender, EventArgs e)
        {
            drpSourceGroup.ClearSelection();
            gvUDF.DataSource = null;
            gvUDF.DataBind();
            List<ECN_Framework_Entities.Communicator.Group> groupList =
            ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser);
            var result = (from src in groupList
                          orderby src.GroupName
                          select src).ToList();
            drpSourceGroup.DataSource = result;
            drpSourceGroup.DataTextField = "GroupName";
            drpSourceGroup.DataValueField = "GroupID";
            drpSourceGroup.DataBind();
            drpSourceGroup.Items.Insert(0, new ListItem("--- Select Group Name ---", " "));
            mpeCopyUDF.Show();
        }

        protected void btnCopy_Click(object sender, EventArgs e)
        {

            List<ECN_Framework_Entities.Communicator.GroupDataFields> groupDataFieldsList =
            ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(Convert.ToInt32(drpSourceGroup.SelectedItem.Value), Master.UserSession.CurrentUser);

            foreach (GridViewRow row in gvUDF.Rows)
            {
                CheckBox chkbox = (CheckBox)row.FindControl("chkCopyUDF");
                if (chkbox.Checked)
                {
                    try
                    {
                        ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByID(Convert.ToInt32(gvUDF.DataKeys[row.RowIndex].Value.ToString()), Convert.ToInt32(drpSourceGroup.SelectedItem.Value), Master.UserSession.CurrentUser);
                        groupDataFields.GroupID = Convert.ToInt32(Request.QueryString["GroupID"].ToString());
                        groupDataFields.GroupDataFieldsID = -1;
                        groupDataFields.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                        if (groupDataFields.DatafieldSetID > 0)
                        {
                            Label lbl = (Label)row.FindControl("lblName");

                            ECN_Framework_Entities.Communicator.DataFieldSets dfs = ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByGroupIDName(Convert.ToInt32(Request.QueryString["GroupID"].ToString()), lbl.Text);

                            if (dfs == null)
                            {
                                ECN_Framework_Entities.Communicator.DataFieldSets dataFieldSets_source = ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByDataFieldsetID(groupDataFields.DatafieldSetID.Value, Convert.ToInt32(drpSourceGroup.SelectedItem.Value), false);

                                ECN_Framework_Entities.Communicator.DataFieldSets dataFieldSets = new ECN_Framework_Entities.Communicator.DataFieldSets();
                                dataFieldSets.Name = lbl.Text;
                                dataFieldSets.GroupID = Convert.ToInt32(Request.QueryString["GroupID"].ToString());
                                dataFieldSets.MultivaluedYN = dataFieldSets_source.MultivaluedYN;

                                ECN_Framework_BusinessLayer.Communicator.DataFieldSets.Save(dataFieldSets, Master.UserSession.CurrentUser);
                                groupDataFields.DatafieldSetID = dataFieldSets.DataFieldSetID;
                            }
                            else
                            {
                                groupDataFields.DatafieldSetID = dfs.DataFieldSetID;
                            }
                        }
                        groupDataFields.GroupDataFieldsID = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(groupDataFields, Master.UserSession.CurrentUser);
                        ECN_Framework_Entities.Communicator.GroupDataFieldsDefault gdfd = ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.GetByGDFID(Convert.ToInt32(gvUDF.DataKeys[row.RowIndex].Value.ToString()));
                        if (gdfd != null && gdfd.GDFID > 0)
                        {
                            gdfd.GDFID = groupDataFields.GroupDataFieldsID;
                            ECN_Framework_BusinessLayer.Communicator.GroupDataFieldsDefault.Save(gdfd);
                        }

                    }
                    catch (ECN_Framework_Common.Objects.ECNException ex)
                    {
                        setECNError(ex);
                    }
                }
            }

            LoadUDFGrid();
            loadTransDropDown();

        }

        protected void drpSourceGroup_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> GroupDataFieldsList_source =
            ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(Convert.ToInt32(drpSourceGroup.SelectedItem.Value), Master.UserSession.CurrentUser);

            List<ECN_Framework_Entities.Communicator.GroupDataFields> GroupDataFieldsList_destination =
            ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(Convert.ToInt32(Request.QueryString["GroupID"].ToString()), Master.UserSession.CurrentUser);

            List<ECN_Framework_Entities.Communicator.GroupDataFields> GroupDataFieldsList_sourceAvailable = new List<ECN_Framework_Entities.Communicator.GroupDataFields>();

            foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFieldSource in GroupDataFieldsList_source)
            {
                bool present = false;
                foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFieldDest in GroupDataFieldsList_destination)
                {
                    if (groupDataFieldSource.ShortName.ToLower().Equals(groupDataFieldDest.ShortName.ToLower()))
                    {
                        present = true;
                        break;
                    }
                }
                if (!present)
                    GroupDataFieldsList_sourceAvailable.Add(groupDataFieldSource);
            }

            var resultSet = from src in GroupDataFieldsList_sourceAvailable
                            select new
                            {
                                GroupDatafieldsID = src.GroupDataFieldsID,
                                ShortName = src.ShortName,
                                LongName = src.LongName,
                                Name = (src.DatafieldSetID == null) ? "" : (ECN_Framework_BusinessLayer.Communicator.DataFieldSets.GetByDataFieldsetID(src.DatafieldSetID.Value, src.GroupID, false)).Name
                            };

            gvUDF.DataSource = resultSet;
            gvUDF.DataBind();
            mpeCopyUDF.Show();
        }

        protected void chkUseDefaultValue_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseDefaultValue.Checked)
            {
                pnlDefaultValue.Visible = true;
                if (ddlDefaultType.SelectedValue.ToString().ToLower().Equals("default"))
                {
                    txtDefaultValue.Visible = true;
                    ddlSystemValues.Visible = false;
                }
                else
                {
                    txtDefaultValue.Visible = false;
                    ddlSystemValues.Visible = true;
                }
            }
            else
            {
                pnlDefaultValue.Visible = false;
            }
        }

        protected void ddlDefaultType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDefaultType.SelectedValue.ToString().ToLower().Equals("default"))
            {
                txtDefaultValue.Visible = true;
                ddlSystemValues.Visible = false;
            }
            else
            {
                txtDefaultValue.Visible = false;
                ddlSystemValues.Visible = true;
            }
        }

        protected void drpTransactionName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpTransactionName.SelectedIndex > 0)
            {
                pnlWholeDefaultValue.Visible = true;
            }
            else
            {
                pnlWholeDefaultValue.Visible = false;
            }
        }
    }

}
