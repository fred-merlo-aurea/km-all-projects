using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using Ecn.Communicator.Main.Admin.Helpers;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.admin.SubscriptionManagement
{
    public partial class SubscriptionManagementEdit : System.Web.UI.Page
    {
        private static int SMID = -1;
        private static string currentSMGroupID = "";
        private static ECN_Framework_Entities.Accounts.SubscriptionManagement currentPage;
        private static List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup> listSMGroups;
        private static List<ECN_Framework_Entities.Accounts.SubsriptionManagementUDF> listSMGUDFs;
        private static List<TempSMGUDF> editSMGUDFs;
        private static List<TempSMG> dtSMGroups;
        private static List<TempSMGUDF> dtSMGUDFs;
        private static DataTable dtReason;
        private BaseChannelOperationsHandler _landingPagesOperations = null;

        public SubscriptionManagementEdit()
        {
            _landingPagesOperations = new BaseChannelOperationsHandler(null);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            if (!Page.IsPostBack)
            {
                dtReason = new DataTable();

                dtReason.Columns.Add("Reason", typeof(string));
                dtReason.Columns.Add("ID", typeof(string));
                dtReason.Columns.Add("SortOrder", typeof(int));
                dtReason.Columns.Add("IsDeleted", typeof(bool));

                dtSMGroups = new List<TempSMG>();
                dtSMGUDFs = new List<TempSMGUDF>();
                if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
                {
                    pnlNoAccess.Visible = false;
                    pnlSettings.Visible = true;
                    LoadData();

                }
                else
                {
                    pnlNoAccess.Visible = true;
                    pnlSettings.Visible = false;
                    Label1.Text = "You do not have access to this page because you are not a Basechannel Administrator.";
                }

            }
        }

        #region PageEvents

        protected void btnAddGroup_Click(object sender, EventArgs e)
        {
            if (ddlSelectCustomer.SelectedIndex > 0)
            {
                if (ddlSelectGroup.SelectedIndex > 0)
                {
                    if (!string.IsNullOrEmpty(txtDefineLabel.Text))
                    {
                        int newGroupID = -1;
                        int.TryParse(ddlSelectGroup.SelectedValue.ToString(), out newGroupID);

                        string guid = Guid.NewGuid().ToString();
                        TempSMG tempSMG = new TempSMG();
                        tempSMG.SMID = -1;
                        tempSMG.SMGID = guid.ToString();
                        tempSMG.GroupID = newGroupID;
                        tempSMG.Label = txtDefineLabel.Text.Trim();
                        tempSMG.IsDeleted = false;

                        dtSMGroups.Add(tempSMG);

                        LoadGroupData();


                    }
                    else
                    {
                        throwECNException("Please enter a label");
                        return;
                    }
                }
                else
                {
                    throwECNException("Please select a Group");
                    return;
                }
            }
            else
            {
                throwECNException("Please select a Customer and a Group");
                return;
            }
        }

        protected void btnSavePage_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentPage != null && currentPage.SubscriptionManagementID > 0)
                {
                    currentPage.Name = txtName.Text.Trim();
                    currentPage.AdminEmail = txtAdminEmail.Text.Trim();
                    currentPage.BaseChannelID = Master.UserSession.CurrentBaseChannel.BaseChannelID;
                    currentPage.CreatedDate = DateTime.Now;
                    currentPage.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                    currentPage.EmailFooter = txtEmailFooter.Text.Trim();
                    currentPage.EmailHeader = txtEmailHeader.Text.Trim();
                    currentPage.Footer = txtPageFooter.Text.Trim();
                    currentPage.Header = txtPageHeader.Text.Trim();
                    currentPage.IncludeMSGroups = chkIncludeMasterSuppressed.Checked;
                    if (currentPage.IncludeMSGroups.Value)
                    {
                        currentPage.MSMessage = txtMSMessage.Text.Trim();
                    }
                    else
                    {
                        currentPage.MSMessage = string.Empty;
                    }
                    currentPage.IsDeleted = false;
                    currentPage.ReasonVisible = rblVisibilityReason.SelectedValue.ToLower().Equals("no") ? false : true;

                    if (currentPage.ReasonVisible.Value)
                    {
                        currentPage.ReasonLabel = txtReasonLabel.Text.Trim();
                        currentPage.UseReasonDropDown = rblReasonControlType.SelectedValue.Equals("Drop Down") ? true : false;
                    }
                    else
                    {
                        currentPage.ReasonLabel = string.Empty;
                        currentPage.UseReasonDropDown = null;
                    }

                    if (rblRedirectThankYou.SelectedValue.ToLower().Equals("thankyou"))
                    {
                        currentPage.UseThankYou = true;
                        currentPage.UseRedirect = false;
                        currentPage.ThankYouLabel = txtThankYouMessage.Text.Trim();
                    }
                    else if (rblRedirectThankYou.SelectedValue.ToLower().Equals("redirect"))
                    {
                        currentPage.UseRedirect = true;
                        currentPage.UseThankYou = false;
                        currentPage.RedirectURL = txtRedirectURL.Text.Trim();
                    }
                    else if (rblRedirectThankYou.SelectedValue.ToLower().Equals("both"))
                    {
                        currentPage.UseRedirect = true;
                        currentPage.UseThankYou = true;
                        currentPage.ThankYouLabel = txtThankYouMessage.Text.Trim();
                        currentPage.RedirectURL = txtRedirectURL.Text.Trim();
                        currentPage.RedirectDelay = Convert.ToInt32(ddlRedirectDelay.SelectedValue.ToString());
                    }
                    else
                    {
                        currentPage.UseRedirect = false;
                        currentPage.UseThankYou = false;
                        currentPage.RedirectURL = string.Empty;
                        currentPage.ThankYouLabel = string.Empty;
                        currentPage.RedirectDelay = 0;
                    }
                    try
                    {
                        currentPage.SubscriptionManagementID = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagement.Save(currentPage, Master.UserSession.CurrentUser);
                    }
                    catch (ECNException ex)
                    {
                        setECNError(ex);
                        return;
                    }
                    
                    
                }
                else
                {
                    currentPage = new ECN_Framework_Entities.Accounts.SubscriptionManagement();
                    currentPage.Name = txtName.Text.Trim();
                    currentPage.AdminEmail = txtAdminEmail.Text.Trim();
                    currentPage.BaseChannelID = Master.UserSession.CurrentBaseChannel.BaseChannelID;
                    currentPage.UpdatedDate = DateTime.Now;
                    currentPage.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                    currentPage.EmailFooter = txtEmailFooter.Text.Trim();
                    currentPage.EmailHeader = txtEmailHeader.Text.Trim();
                    currentPage.Footer = txtPageFooter.Text.Trim();
                    currentPage.Header = txtPageHeader.Text.Trim();
                    currentPage.MSMessage = txtMSMessage.Text.Trim();
                    currentPage.IsDeleted = false;
                    currentPage.IncludeMSGroups = chkIncludeMasterSuppressed.Checked;
                    if (currentPage.IncludeMSGroups.Value)
                    {
                        currentPage.MSMessage = txtMSMessage.Text.Trim();
                    }
                    else
                    {
                        currentPage.MSMessage = string.Empty;
                    }
                    currentPage.IsDeleted = false;
                    currentPage.ReasonVisible = rblVisibilityReason.SelectedValue.ToLower().Equals("no") ? false : true;

                    if (currentPage.ReasonVisible.Value)
                    {
                        currentPage.ReasonLabel = txtReasonLabel.Text.Trim();
                        currentPage.UseReasonDropDown = rblReasonControlType.SelectedValue.Equals("Drop Down") ? true : false;
                    }
                    else
                    {
                        currentPage.ReasonLabel = string.Empty;
                        currentPage.UseReasonDropDown = null;
                    }

                    if(rblRedirectThankYou.SelectedValue.ToLower().Equals("thankyou") )
                    {
                        currentPage.UseThankYou = true;
                       currentPage.UseRedirect = false;
                       currentPage.ThankYouLabel = txtThankYouMessage.Text.Trim();
                    }
                    else if(rblRedirectThankYou.SelectedValue.ToLower().Equals("redirect"))
                    {
                        currentPage.UseRedirect = true;
                        currentPage.UseThankYou = false;
                        currentPage.RedirectURL = txtRedirectURL.Text.Trim();
                    }
                    else if(rblRedirectThankYou.SelectedValue.ToLower().Equals("both"))
                    {
                        currentPage.UseRedirect = true;
                        currentPage.UseThankYou = true;
                        currentPage.ThankYouLabel = txtThankYouMessage.Text.Trim();
                        currentPage.RedirectURL = txtRedirectURL.Text.Trim();
                        currentPage.RedirectDelay = Convert.ToInt32(ddlRedirectDelay.SelectedValue.ToString());
                    }
                    else
                    {
                        currentPage.UseRedirect = false;
                        currentPage.UseThankYou = false;
                        currentPage.RedirectURL = string.Empty;
                        currentPage.ThankYouLabel = string.Empty;
                        currentPage.RedirectDelay = 0;
                    }



                    try
                    {
                        currentPage.SubscriptionManagementID = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagement.Save(currentPage, Master.UserSession.CurrentUser);
                    }
                    catch (ECNException ex)
                    {
                        setECNError(ex);
                        return;
                    }
                }

                #region Reason section
                if (currentPage.ReasonVisible.Value)
                {

                    if (currentPage.UseReasonDropDown.Value)
                    {
                        if (dtReason.Select("IsDeleted = 'false'").Count() > 0)
                        {
                            foreach (DataRow dr in dtReason.Rows)
                            {
                                if (dr["ID"].ToString().Contains("-"))
                                {
                                    if (dr["IsDeleted"].ToString().ToLower().Equals("false"))
                                    {
                                        ECN_Framework_Entities.Accounts.SubscriptionManagementReason smr = new ECN_Framework_Entities.Accounts.SubscriptionManagementReason();
                                        smr.SubscriptionManagementID = currentPage.SubscriptionManagementID;

                                        smr.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                                        smr.Reason = dr["Reason"].ToString();
                                        smr.IsDeleted = false;
                                        smr.SortOrder = Convert.ToInt32(dr["SortOrder"].ToString());
                                        ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementReason.Save(smr);
                                    }
                                }
                                else
                                {
                                    if (dr["IsDeleted"].ToString().ToLower().Equals("true"))
                                    {
                                        int ReasonID = Convert.ToInt32(dr["ID"].ToString());
                                        ECN_Framework_Entities.Accounts.SubscriptionManagementReason smr = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementReason.GetBySMID(currentPage.SubscriptionManagementID).First(x => x.SubscriptionManagementReasonID == ReasonID);
                                        smr.IsDeleted = true;
                                        smr.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                                        ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementReason.Save(smr);
                                    }
                                    else
                                    {
                                        ECN_Framework_Entities.Accounts.SubscriptionManagementReason smr = new ECN_Framework_Entities.Accounts.SubscriptionManagementReason();
                                        smr.SubscriptionManagementReasonID = Convert.ToInt32(dr["ID"].ToString());
                                        smr.SubscriptionManagementID = currentPage.SubscriptionManagementID;
                                        smr.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                                        smr.Reason = dr["Reason"].ToString();
                                        smr.SortOrder = Convert.ToInt32(dr["SortOrder"].ToString());
                                        smr.IsDeleted = false;
                                        ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementReason.Save(smr);
                                    }
                                }
                            }
                        }
                        else
                        {
                            throwECNException("Please enter at least one Reason");
                            return;
                        }
                    }
                    else
                    {
                        List<ECN_Framework_Entities.Accounts.SubscriptionManagementReason> listReason = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementReason.GetBySMID(currentPage.SubscriptionManagementID);
                        foreach (ECN_Framework_Entities.Accounts.SubscriptionManagementReason smr in listReason)
                        {
                            smr.IsDeleted = true;
                            smr.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                            ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementReason.Save(smr);
                        }
                    }

                }

                #endregion

                #region Groups section
                if (currentPage.SubscriptionManagementID > 0)
                {
                    //SubscriptionManagementGroups section
                    foreach (TempSMG group in dtSMGroups)
                    {
                        if (group.SMGID.Contains("-"))
                        {
                            if (!group.IsDeleted)
                            {
                                ECN_Framework_Entities.Accounts.SubscriptionManagementGroup newGroup = new ECN_Framework_Entities.Accounts.SubscriptionManagementGroup();
                                newGroup.CreatedDate = DateTime.Now;
                                newGroup.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                                newGroup.GroupID = group.GroupID;
                                newGroup.IsDeleted = false;
                                newGroup.Label = group.Label;
                                newGroup.SubscriptionManagementPageID = currentPage.SubscriptionManagementID;

                                try
                                {
                                    group.DBSMGroupID = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementGroup.Save(newGroup);
                                }
                                catch (ECNException ex)
                                {
                                    setECNError(ex);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (group.IsDeleted)
                            {
                                try
                                {
                                    ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementGroup.Delete(group.SMID, Convert.ToInt32(group.SMGID), Master.UserSession.CurrentUser);
                                }
                                catch (ECNException ex)
                                {
                                    setECNError(ex);
                                    return;
                                }
                            }
                            else
                            {
                                ECN_Framework_Entities.Accounts.SubscriptionManagementGroup newGroup = listSMGroups.First(x => x.SubscriptionManagementGroupID == Convert.ToInt32(group.SMGID.ToString()));
                                newGroup.UpdatedDate = DateTime.Now;
                                newGroup.UpdatedUserID = Master.UserSession.CurrentUser.UserID;


                                newGroup.Label = group.Label;
                                ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementGroup.Save(newGroup);
                            }
                        }
                    }

                    //SubscriptionManagementUDF section
                    foreach (TempSMGUDF smgudf in dtSMGUDFs)
                    {
                        if (smgudf.SMUDFID.Contains("-"))
                        {
                            if (!smgudf.IsDeleted)
                            {
                                ECN_Framework_Entities.Accounts.SubsriptionManagementUDF newUDF = new ECN_Framework_Entities.Accounts.SubsriptionManagementUDF();
                                newUDF.CreatedDate = DateTime.Now;
                                newUDF.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                                newUDF.GroupDataFieldsID = smgudf.GroupDataFieldsID;
                                newUDF.IsDeleted = false;
                                newUDF.StaticValue = smgudf.StaticValue;
                                newUDF.SubscriptionManagementGroupID = dtSMGroups.First(x => x.SMGID == smgudf.SMGID).DBSMGroupID;
                                try
                                {
                                    ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementUDF.Save(newUDF);
                                }
                                catch (ECNException ex)
                                {
                                    setECNError(ex);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (smgudf.IsDeleted)
                            {
                                try
                                {
                                    ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementUDF.Delete(Convert.ToInt32(smgudf.SMGID), Master.UserSession.CurrentUser, Convert.ToInt32(smgudf.SMUDFID));
                                }
                                catch (ECNException ex)
                                {
                                    setECNError(ex);
                                    return;
                                }
                            }
                        }
                    }
                }
                #endregion
            }

            catch (ECNException ex)
            {
                LoadData();
                setECNError(ex);
                return;
            }
            Response.Redirect("SubscriptionManagementList.aspx");

        }

        protected void ddlSelectCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            int custID = -1;
            int.TryParse(ddlSelectCustomer.SelectedValue.ToString(), out custID);

            if (custID > 0)
            {
                List<ECN_Framework_Entities.Communicator.Group> listGroups = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(custID, Master.UserSession.CurrentUser);

                listGroups = listGroups.Where(x => x.MasterSupression.HasValue ? x.MasterSupression == 0 : !x.MasterSupression.HasValue).ToList();

                var FinalListGroups = listGroups.Where(x => !dtSMGroups.Any(x2 => x2.GroupID == x.GroupID && x2.IsDeleted == false));

                ddlSelectGroup.DataSource = FinalListGroups.OrderBy(x => x.GroupName);
                ddlSelectGroup.DataTextField = "GroupName";
                ddlSelectGroup.DataValueField = "GroupID";
                ddlSelectGroup.DataBind();
                ddlSelectGroup.Items.Insert(0, new ListItem { Text = "--SELECT--", Value = "-1" });
            }
            else
            {
                ddlSelectGroup.Items.Clear();
            }

        }

        protected void imgbtnAddUDF_Click(object sender, ImageClickEventArgs e)
        {
            lblErrorMessageUDF.Visible = false;
            if (!string.IsNullOrEmpty(txtStaticValue.Text.Trim()))
            {
                if (ddlUDF.SelectedIndex > 0)
                {

                    TempSMGUDF newUDF = new TempSMGUDF();
                    newUDF.SMGID = imgbtnAddUDF.CommandArgument.ToString();
                    newUDF.SMUDFID = Guid.NewGuid().ToString();
                    newUDF.IsDeleted = false;
                    newUDF.GroupID = dtSMGroups.First(x => x.SMGID == imgbtnAddUDF.CommandArgument).GroupID;
                    newUDF.StaticValue = txtStaticValue.Text.Trim();
                    newUDF.GroupDataFieldsID = Convert.ToInt32(ddlUDF.SelectedValue.ToString());
                    editSMGUDFs.Add(newUDF);

                    gvUDF.DataSource = editSMGUDFs.Where(x => x.IsDeleted == false);
                    gvUDF.DataBind();

                    txtStaticValue.Text = "";
                    List<ECN_Framework_Entities.Communicator.GroupDataFields> listUDFs = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(newUDF.GroupID, Master.UserSession.CurrentUser);
                    var finalList = from c in listUDFs
                                    where !(from p in editSMGUDFs.Where(x => x.IsDeleted == false)
                                            select p.GroupDataFieldsID).Contains(c.GroupDataFieldsID)
                                    select new { ShortName = c.ShortName, GroupDataFieldsID = c.GroupDataFieldsID };

                    ddlUDF.DataSource = finalList.OrderBy(x => x.ShortName);
                    ddlUDF.DataTextField = "ShortName";
                    ddlUDF.DataValueField = "GroupDataFieldsID";
                    ddlUDF.DataBind();
                    ddlUDF.Items.Insert(0, new ListItem { Text = "--SELECT--", Value = "-1" });
                }
                else
                {
                    lblErrorMessageUDF.Text = "Please select a UDF";
                    lblErrorMessageUDF.Visible = true;
                }
            }
            else
            {
                lblErrorMessageUDF.Text = "Please enter a static value";
                lblErrorMessageUDF.Visible = true;
            }
        }

        protected void btnSaveUDF_Click(object sender, EventArgs e)
        {
            dtSMGroups.First(x => x.SMGID == imgbtnAddUDF.CommandArgument).Label = txtGroupLabel.Text;
            foreach (TempSMGUDF udf in editSMGUDFs)
            {
                if (udf.SMUDFID.Contains("-"))
                {
                    if (!udf.IsDeleted && !dtSMGUDFs.Exists(x => x.SMUDFID == udf.SMUDFID))
                    {
                        dtSMGUDFs.Add(udf);
                    }
                }
                else
                {
                    if (udf.IsDeleted)
                    {
                        try
                        {
                            dtSMGUDFs.First(x => x.SMUDFID == udf.SMUDFID).IsDeleted = true;

                        }
                        catch (ECNException ex)
                        {
                            setECNError(ex);
                            return;
                        }
                    }
                }
            }
            LoadGroupData();
            mpeEditUDFs.Hide();
        }

        protected void rblReasonControlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblReasonControlType.SelectedValue.Equals("Text Box"))
            {
                pnlReasonDropDown.Visible = false;
            }
            else
            {
                pnlReasonDropDown.Visible = true;
                if (dtReason.Rows.Count == 0)
                {
                    _landingPagesOperations.AddDefaultReasons(dtReason);
                }
                loadGrid();
            }
        }

        protected void rblRedirectThankYou_SelectedIndexChanged(object sender, EventArgs e)
        {
            _landingPagesOperations.HandleRedirectThankYouSelectionChanged(
                rblRedirectThankYou,
                tblDelay,
                tblRedirect,
                tblThankYou,
                txtRedirectURL,
                txtThankYouMessage,
                ddlRedirectDelay);
        }

        protected void rlReasonDropDown_ItemReorder(object sender, ReorderListItemReorderEventArgs e)
        {
            _landingPagesOperations.HandleReasonReorder(
                dtReason,
                e.NewIndex,
                e.OldIndex,
                rlReasonDropDown,
                pnlReasonDropDown);
        }

        protected void rlReasonDropDown_ItemCommand(object sender, ReorderListCommandEventArgs e)
        {
            _landingPagesOperations.HandleReasonCommand(
                dtReason,
                e.CommandName,
                e.CommandArgument?.ToString(),
                txtReasonLabelEdit,
                btnSaveReason,
                mpeEditReason,
                rlReasonDropDown,
                pnlReasonDropDown);
        }
        protected void btnAddNewReason_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNewReason.Text))
            {
                if (dtReason.Select("Reason = '" + txtNewReason.Text.Replace("'","''") + "' and IsDeleted = 'false'").Count() == 0)
                {
                    DataRow dr = dtReason.NewRow();
                    dr["Reason"] = txtNewReason.Text.Trim();
                    dr["ID"] = Guid.NewGuid().ToString();
                    dr["SortOrder"] = dtReason.Select("IsDeleted = 'false'").Count() + 1;
                    dr["IsDeleted"] = false;

                    dtReason.Rows.Add(dr);
                    loadGrid();

                    txtNewReason.Text = string.Empty;
                }
                else
                {
                    throwECNException("Reason already exists");
                    return;
                }
            }
            else
            {
                throwECNException("Please enter a value for the new reason");
                return;
            }
        }
        protected void rblVisibilityReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblVisibilityReason.SelectedValue.ToLower().Equals("no"))
            {
                pnlReason.Visible = false;

            }
            else
            {
                pnlReason.Visible = true;
                if (rblReasonControlType.SelectedValue.ToLower().Equals("text box"))
                    pnlReasonDropDown.Visible = false;
                else
                    pnlReasonDropDown.Visible = true;
            }
        }

        protected void btnSaveReason_Click(object sender, EventArgs e)
        {
            _landingPagesOperations.HandleSaveReason(
                dtReason,
                txtReasonLabelEdit.Text.Trim(),
                btnSaveReason.CommandArgument?.ToString(),
                mpeEditReason,
                rlReasonDropDown,
                pnlReasonDropDown);
        }


        #endregion

        #region DataLoad
        private void LoadData()
        {
            if (Request.QueryString["smid"] != null)
            {
                int.TryParse(Request.QueryString["smid"].ToString(), out SMID);
                if (SMID > 0)
                {
                    currentPage = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagement.GetBySubscriptionManagementID(SMID);
                    listSMGroups = new List<ECN_Framework_Entities.Accounts.SubscriptionManagementGroup>();
                    listSMGUDFs = new List<ECN_Framework_Entities.Accounts.SubsriptionManagementUDF>();
                    if (currentPage != null)
                    {
                        txtName.Text = currentPage.Name;
                        txtName.Enabled = false;
                        txtPageHeader.Text = currentPage.Header;
                        txtPageFooter.Text = currentPage.Footer;
                        txtEmailFooter.Text = currentPage.EmailFooter;
                        txtEmailHeader.Text = currentPage.EmailHeader;
                        txtAdminEmail.Text = currentPage.AdminEmail;
                        txtMSMessage.Text = currentPage.MSMessage;
                        chkIncludeMasterSuppressed.Checked = currentPage.IncludeMSGroups.HasValue ? currentPage.IncludeMSGroups.Value : false;
                        lblURL.Visible = true;
                        txtURL.Text = ConfigurationManager.AppSettings["Activity_DomainPath"].ToString() + "/engines/subscriptionmanagement.aspx?smid=" + SMID.ToString() + "&e=%%EmailAddress%%";
                        if (currentPage.ReasonVisible.Value)
                        {
                            txtReasonLabel.Text = currentPage.ReasonLabel;
                            rblVisibilityReason.SelectedValue = "Yes";

                            if (currentPage.UseReasonDropDown.Value)
                            {
                                rblReasonControlType.SelectedValue = "Drop Down";
                                pnlReasonDropDown.Visible = true;
                                List<ECN_Framework_Entities.Accounts.SubscriptionManagementReason> listReason = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementReason.GetBySMID(currentPage.SubscriptionManagementID);
                                if (listReason.Count > 0)
                                {
                                    foreach (ECN_Framework_Entities.Accounts.SubscriptionManagementReason lpor in listReason)
                                    {
                                        DataRow dr = dtReason.NewRow();
                                        dr["Reason"] = lpor.Reason;
                                        dr["ID"] = lpor.SubscriptionManagementReasonID.ToString();
                                        dr["SortOrder"] = lpor.SortOrder.Value;
                                        dr["IsDeleted"] = lpor.IsDeleted;
                                        dtReason.Rows.Add(dr);
                                    }
                                }
                                else
                                {
                                    _landingPagesOperations.AddDefaultReasons(dtReason);
                                }
                                loadGrid();
                            }
                            else
                            {
                                rblReasonControlType.SelectedValue = "Text Box";
                                pnlReasonDropDown.Visible = false;

                            }
                        }
                        else
                        {
                            pnlReason.Visible = false;
                        }

                        if(currentPage.UseThankYou.HasValue && currentPage.UseThankYou.Value && (!currentPage.UseRedirect.HasValue || !currentPage.UseRedirect.Value))
                        {
                            tblDelay.Visible = false;
                            tblRedirect.Visible = false;
                            tblThankYou.Visible = true;
                            rblRedirectThankYou.SelectedValue = "thankyou";
                            txtThankYouMessage.Text = currentPage.ThankYouLabel;
                        }
                        else if (currentPage.UseRedirect.HasValue && currentPage.UseRedirect.Value &&( !currentPage.UseThankYou.HasValue || !currentPage.UseThankYou.Value))
                        {
                            tblDelay.Visible = false;
                            tblRedirect.Visible = true;
                            tblThankYou.Visible = false;
                            rblRedirectThankYou.SelectedValue = "redirect";
                            txtRedirectURL.Text = currentPage.RedirectURL;
                        }
                        else if(currentPage.UseRedirect.HasValue && currentPage.UseThankYou.HasValue && currentPage.UseRedirect.Value && currentPage.UseThankYou.Value)
                        {
                            tblDelay.Visible = true;
                            tblRedirect.Visible = true;
                            tblThankYou.Visible = true;
                            rblRedirectThankYou.SelectedValue = "both";
                            txtThankYouMessage.Text = currentPage.ThankYouLabel;
                            txtRedirectURL.Text = currentPage.RedirectURL;
                            if (currentPage.RedirectDelay == 0)
                                ddlRedirectDelay.SelectedValue = "5";
                            else
                                ddlRedirectDelay.SelectedValue = currentPage.RedirectDelay.ToString();
                        }
                        else
                        {
                            tblDelay.Visible = false;
                            tblRedirect.Visible = false;
                            tblThankYou.Visible = false;

                            rblRedirectThankYou.SelectedValue = "neither";
                            txtThankYouMessage.Text = string.Empty;
                            txtRedirectURL.Text = string.Empty;
                        }
                    }
                    else
                    {
                        lblURL.Visible = false;
                        txtName.Enabled = true;
                    }
                }
                LoadGroupData();
            }
            else
            {
                rblRedirectThankYou.SelectedValue = "neither";
                tblDelay.Visible = false;
                tblRedirect.Visible = false;
                tblThankYou.Visible = false;

                txtRedirectURL.Text = string.Empty;
                txtThankYouMessage.Text = string.Empty;
                ddlRedirectDelay.SelectedIndex = 0;

                rblVisibilityReason.SelectedValue = "no";
                rblReasonControlType.SelectedValue = "Text Box";
                pnlReason.Visible = false;

                currentPage = new ECN_Framework_Entities.Accounts.SubscriptionManagement();
                _landingPagesOperations.AddDefaultReasons(dtReason);
                loadGrid();
            }
            LoadSetupData();
        }

        private void loadGrid()
        {
            if (dtReason != null)
            {
                var result = (from src in dtReason.AsEnumerable()
                              orderby src.Field<int>("SortOrder")
                              where src.Field<bool>("IsDeleted") == false
                              select new
                              {
                                  ID = src.Field<string>("ID"),
                                  Reason = src.Field<string>("Reason"),
                                  SortOrder = src.Field<int>("SortOrder"),
                                  IsDeleted = src.Field<bool>("IsDeleted"),

                              }).ToList();
                rlReasonDropDown.DataSource = result;
                rlReasonDropDown.DataBind();
                pnlReasonDropDown.Visible = true;
            }
        }
        private void LoadGroupData()
        {
            if (currentPage != null)
            {
                listSMGroups = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementGroup.GetBySMID(currentPage.SubscriptionManagementID);
                foreach (ECN_Framework_Entities.Accounts.SubscriptionManagementGroup smg in listSMGroups)
                {
                    if (!dtSMGroups.Exists(x => x.SMGID == smg.SubscriptionManagementGroupID.ToString()))
                    {
                        TempSMG tempsmg = new TempSMG();
                        tempsmg.SMGID = smg.SubscriptionManagementGroupID.ToString();
                        tempsmg.SMID = smg.SubscriptionManagementPageID;
                        tempsmg.Label = smg.Label;
                        tempsmg.IsDeleted = smg.IsDeleted;
                        tempsmg.GroupID = smg.GroupID;
                        tempsmg.DBSMGroupID = smg.SubscriptionManagementGroupID;
                        dtSMGroups.Add(tempsmg);

                        listSMGUDFs = ECN_Framework_BusinessLayer.Accounts.SubscriptionManagementUDF.GetBySMGID(smg.SubscriptionManagementGroupID);
                        foreach (ECN_Framework_Entities.Accounts.SubsriptionManagementUDF sudf in listSMGUDFs)
                        {
                            TempSMGUDF tempUDF = new TempSMGUDF();
                            tempUDF.SMUDFID = sudf.SubscriptionManagementUDFID.ToString();
                            tempUDF.SMGID = sudf.SubscriptionManagementGroupID.ToString();
                            tempUDF.GroupID = smg.GroupID;
                            tempUDF.IsDeleted = sudf.IsDeleted;
                            tempUDF.GroupDataFieldsID = sudf.GroupDataFieldsID;
                            tempUDF.StaticValue = sudf.StaticValue;

                            dtSMGUDFs.Add(tempUDF);
                        }
                    }
                }

            }
            txtDefineLabel.Text = "";

            ddlSelectCustomer.ClearSelection();
            ddlSelectGroup.Items.Clear();

            gvGroups.DataSource = dtSMGroups?.Where(x => x.IsDeleted == false);
            gvGroups.DataBind();
        }

        private void LoadSetupData()
        {

            List<ECN_Framework_Entities.Accounts.Customer> listCust = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(Master.UserSession.CurrentBaseChannel.BaseChannelID);

            ddlSelectCustomer.DataSource = listCust.OrderBy(x => x.CustomerName);
            ddlSelectCustomer.DataTextField = "CustomerName";
            ddlSelectCustomer.DataValueField = "CustomerID";
            ddlSelectCustomer.DataBind();
            ddlSelectCustomer.ClearSelection();
            ddlSelectCustomer.Items.Insert(0, new ListItem { Text = "--SELECT--", Value = "-1", Selected = true });
        }

        private void LoadSMGData(string smgid)
        {
            editSMGUDFs = dtSMGUDFs.Where(x => x.SMGID == smgid && x.IsDeleted == false).ToList();
            gvUDF.DataSource = editSMGUDFs;
            gvUDF.DataBind();

            TempSMG groupid = dtSMGroups.First(x => x.SMGID == smgid);
            txtGroupLabel.Text = groupid.Label;

            List<ECN_Framework_Entities.Communicator.GroupDataFields> listUDFs = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupid.GroupID, Master.UserSession.CurrentUser);
            var finalList = from c in listUDFs
                            where !(from p in editSMGUDFs.Where(x => x.IsDeleted == false)
                                    select p.GroupDataFieldsID).Contains(c.GroupDataFieldsID)
                            select new { ShortName = c.ShortName, GroupDataFieldsID = c.GroupDataFieldsID };

            ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(groupid.GroupID, Master.UserSession.CurrentUser);
            GroupName.Text = g.GroupName;

            ddlUDF.DataSource = finalList.OrderBy(x => x.ShortName);
            ddlUDF.DataTextField = "ShortName";
            ddlUDF.DataValueField = "GroupDataFieldsID";
            ddlUDF.DataBind();
            ddlUDF.Items.Insert(0, new ListItem { Text = "--SELECT--", Value = "-1" });

            imgbtnAddUDF.CommandArgument = smgid;
        }

        #endregion

        #region GridView events

        protected void gvGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TempSMG currentRow = new TempSMG();
                currentRow = (TempSMG)e.Row.DataItem;

                ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(currentRow.GroupID, Master.UserSession.CurrentUser);
                Label lblGroupName = (Label)e.Row.FindControl("lblGroupName");
                lblGroupName.Text = g.GroupName;

                ImageButton imgbtnEditUDF = (ImageButton)e.Row.FindControl("imgbtnEditUDF");
                imgbtnEditUDF.CommandArgument = currentRow.SMGID.ToString();

                ImageButton _imgbtnDeleteGroup = (ImageButton)e.Row.FindControl("imgbtnDeleteGroup");
                _imgbtnDeleteGroup.CommandArgument = currentRow.SMGID.ToString();

            }
        }

        protected void gvUDF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TempSMGUDF tempUDF = (TempSMGUDF)e.Row.DataItem;
                ImageButton imgbtnDeleteUDF = (ImageButton)e.Row.FindControl("imgbtnDeleteUDF");
                imgbtnDeleteUDF.CommandArgument = tempUDF.SMUDFID;

                ECN_Framework_Entities.Communicator.GroupDataFields gdf = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByID(tempUDF.GroupDataFieldsID, tempUDF.GroupID, Master.UserSession.CurrentUser);
                Label lblShortName = (Label)e.Row.FindControl("lblShortName");
                lblShortName.Text = gdf.ShortName;
            }
        }

        protected void gvUDF_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Equals("deleteudf"))
            {
                editSMGUDFs.First(x => x.SMUDFID == e.CommandArgument.ToString()).IsDeleted = true;

                ddlUDF.Items.Clear();
                List<ECN_Framework_Entities.Communicator.GroupDataFields> listUDFs = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(dtSMGroups.First(x => x.SMGID == currentSMGroupID).GroupID, Master.UserSession.CurrentUser);
                var finalList = from c in listUDFs
                                where !(from p in editSMGUDFs.Where(x => x.IsDeleted == false)
                                        select p.GroupDataFieldsID).Contains(c.GroupDataFieldsID)
                                select new { ShortName = c.ShortName, GroupDataFieldsID = c.GroupDataFieldsID };

                ddlUDF.DataSource = finalList.OrderBy(x => x.ShortName);
                ddlUDF.DataTextField = "ShortName";
                ddlUDF.DataValueField = "GroupDataFieldsID";
                ddlUDF.DataBind();
                ddlUDF.Items.Insert(0, new ListItem { Text = "--SELECT--", Value = "-1" });

                gvUDF.DataSource = editSMGUDFs.Where(x => x.IsDeleted == false);
                gvUDF.DataBind();
            }

        }

        protected void gvGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("deletegroup"))
            {
                dtSMGroups[dtSMGroups.IndexOf(dtSMGroups.First(x => x.SMGID == e.CommandArgument.ToString()))].IsDeleted = true;
                foreach (TempSMGUDF udf in dtSMGUDFs)
                {
                    if (udf.SMGID == e.CommandArgument.ToString())
                    {
                        udf.IsDeleted = true;
                    }
                }
                LoadGroupData();
            }
            else if (e.CommandName.Equals("editudf"))
            {
                ImageButton btnEditUDF = (ImageButton)e.CommandSource;
                currentSMGroupID = btnEditUDF.CommandArgument.ToString();
                LoadSMGData(btnEditUDF.CommandArgument.ToString());
                mpeEditUDFs.Show();
            }

        }

        #endregion

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.ReportSchedule, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        protected void btnCancelEditReason_Click(object sender, EventArgs e)
        {
            mpeEditReason.Hide();
        }

    }
    public partial class TempSMG
    {
        public TempSMG() { SMGID = ""; SMID = -1; GroupID = -1; Label = ""; IsDeleted = false; DBSMGroupID = -1; }
        public string SMGID { get; set; }
        public int SMID { get; set; }
        public int GroupID { get; set; }
        public string Label { get; set; }
        public bool IsDeleted { get; set; }
        public int DBSMGroupID { get; set; }

    }

    public partial class TempSMGUDF
    {
        public TempSMGUDF() { SMUDFID = ""; SMGID = ""; GroupDataFieldsID = -1; IsDeleted = false; StaticValue = ""; GroupID = -1; }

        public string SMUDFID { get; set; }
        public string SMGID { get; set; }
        public int GroupDataFieldsID { get; set; }
        public bool IsDeleted { get; set; }
        public string StaticValue { get; set; }
        public int GroupID { get; set; }
    }
}