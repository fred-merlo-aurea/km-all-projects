using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using ecn.communicator.MasterPages;
using KMPlatform.Entity;
using Ecn.Communicator.Main.Lists.Interfaces;
using Ecn.Communicator.Main.Lists.Helpers;
using ecn.communicator.Constants;

namespace ecn.communicator.listsmanager
{
    public partial class groupsubscribe : ECN_Framework.WebPageHelper
    {
        private const string SoFormType = "SO";
        private const string DoFormType = "DO";
        private const string CreateNewSmartForm = "Create New smartForm";

        string selectedFields = string.Empty;
        string optinHTML = string.Empty;

        private IMasterCommunicator MasterCommunicator;
        private ISmartFormsHistory SmartFormsHistory;
        private IRequest HttpRequest;

        protected System.Web.UI.WebControls.Button OptinHTMLPreview;

        public int GroupId
        {
            get
            {
                return RequestQueryString(QueryStringKeys.GroupId, default(int));
            }
        }

        public int SFID
        {
            get
            {
                return RequestQueryString(QueryStringKeys.SFID, default(int));
            }
        }

        public string RequestedAction
        {
            get
            {
                return RequestQueryString(QueryStringKeys.Action, string.Empty);
            }
        }

        public groupsubscribe()
        {
            this.MasterCommunicator = new MasterCommunicatorAdapter(this.Master);
            this.SmartFormsHistory = new SmartFormsHistoryAdapter();
            this.HttpRequest = new RequestAdapter(this.Request);
        }

        public groupsubscribe(IMasterCommunicator masterCommunicator, ISmartFormsHistory smartFormsHistory, IRequest request)
        {
            MasterCommunicator = masterCommunicator;
            SmartFormsHistory = smartFormsHistory;
            HttpRequest = request;
        }

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
            Master.Heading = "Groups > Manage Groups > Smart Forms";
            Master.HelpContent = "<img align='right' src=/ecn.images/images/icogroups.gif><b>Group Subscribe</b><br />Just copy and paste the code that is in the Text Box in your Newsletter to enable the Subscribe and UnSubscribes from the users who receive your Newsletter.</p><p>Check Boxes agint ";
            Master.HelpTitle = "Groups Manager";
            //HTMLCode.Wrap = true;
            //SO_HTMLCode.Wrap = true; ;
            //HTMLCode.UseDivMode = false;
            //SO_HTMLCode.UseDivMode = false;
            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "grouppriv") || KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            if (KMPlatform.BusinessLogic.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                var requestGroupID = GroupId;
                var requestSFID = SFID;
                var requestAction = RequestedAction;

                //if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.PrePopsmartForms))
                if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                {
                    PP_SmartFormButton.Enabled = true;
                }
                else
                {
                    PP_SmartFormButton.Enabled = false;
                }

                if (!Page.IsPostBack)
                {
                    if (requestSFID > 0)
                    {
                        if (requestAction.Equals("delete"))
                        {
                            try
                            {
                                ECN_Framework_BusinessLayer.Communicator.SmartFormsHistory.Delete(requestSFID, Master.UserSession.CurrentUser);
                                SO_SmartFormButton_Click(sender, e);
                            }
                            catch (ECNException ex)
                            {
                                setECNError(ex);
                            }
                        }
                        else
                        {
                            if (!(Page.IsPostBack))
                            {
                                DO_panelFCKEditor.Enabled = false;
                                DO_panelFCKEditor.Visible = false;
                                DO_SmartFormButton.Enabled = true;
                                SO_SmartFormButton.Enabled = false;
                                SO_panelFCKEditor.Enabled = true;
                                SO_panelFCKEditor.Visible = true;
                                panelTexbox.Enabled = false;
                                panelTexbox.Visible = false;
                                LoadEmailFields(requestGroupID, SO_OptinFieldSelection);
                                LoadSmartFormData(requestSFID);
                                SO_RefreshHTML.Enabled = true;
                                SO_OptinHTMLSave.Enabled = true;
                                SO_OptinHTMLSaveNew.Text = "Create as a  New smartFrom";
                            }
                        }
                    }
                    else if (requestGroupID > 0)
                    {
                        if (!(Page.IsPostBack))
                        {
                            if (KMPlatform.BusinessLogic.Client.HasService(Master.UserSession.ClientID, KMPlatform.Enums.Services.EMAILMARKETING))
                            {
                                DO_panelFCKEditor.Enabled = true;
                                DO_panelFCKEditor.Visible = true;
                                panelTexbox.Enabled = false;
                                panelTexbox.Visible = false;
                                LoadEmailFields(requestGroupID, OptinFieldSelection);
                                SetHTMLCode(requestGroupID, true, HTMLCode);
                            }
                            else
                            {
                                DO_SmartFormButton.Enabled = false;
                                SO_SmartFormButton.Enabled = false;

                                DO_panelFCKEditor.Enabled = false;
                                DO_panelFCKEditor.Visible = false;
                                panelTexbox.Enabled = true;
                                panelTexbox.Visible = true;
                                SetHTMLCode(requestGroupID, false, SO_HTMLCode);
                            }
                        }
                        else
                        {
                            if (!(SO_SmartFormButton.Enabled))
                            {
                                LoadSmartFormGrid(requestGroupID);
                            }
                        }
                    }
                }
                if ((requestSFID > 0) || (!(SO_SmartFormButton.Enabled)))
                {
                    LoadSmartFormGrid(requestGroupID);
                }
            }
            else
            {
                Response.Redirect("../default.aspx");
            }

            if (!Page.IsPostBack)
            {
                if (!KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                {
                    pnlEdit.Visible = false;
                    pnlEditor.Visible = false;
                }
                else
                {
                    pnlEdit.Visible = true;
                    pnlEditor.Visible = true;
                }
                //SO_HTMLCode.EditModes = new string[] { "HTML", "Design" };
                //HTMLCode.EditModes = new string[] { "HTML", "Design" };
            }
        }

        #region Form Prep
        //  private void SetHTMLCode(int setGroupID, bool smartForm, ecn.communicator.main.ECNWizard.Content.RadEditor.RadEditor HTMLCode)
        private void SetHTMLCode(int setGroupID, bool smartForm, CKEditor.NET.CKEditorControl HTMLCode)
        {
            ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(setGroupID, Master.UserSession.CurrentUser);
            if (smartForm)
            {

                string DBoptinCde = group.OptinHTML, DBoptinFields = group.OptinFields;
                if (DBoptinCde.ToString().Length == 0 || DBoptinFields.ToString().Length == 0)
                {
                    HTMLCode.Text = GetHTMLCode(setGroupID, "DO");
                }
                else
                {
                    this.OptinHTMLSave.Enabled = true;
                    optinHTML = DBoptinCde.ToString();
                    selectedFields = DBoptinFields.ToString();
                    StringTokenizer st = new StringTokenizer(selectedFields, '|');
                    while (st.HasMoreTokens())
                    {
                        string token = st.NextToken();
                        for (int i = 0; i < OptinFieldSelection.Items.Count; i++)
                        {
                            if (OptinFieldSelection.Items[i].Text.Equals(token))
                            {
                                OptinFieldSelection.Items[i].Selected = true;
                            }
                        }
                    }

                    HTMLCode.Text = optinHTML.ToString();
                }
            }
            else
            {
                string redirpage = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/subscribe.aspx";
                string thecode =
                    "<form action=" + redirpage + ">" +
                    "<INPUT id=EmailAddress type=text name=e size=25><br>" +
                    "<INPUT id=RadioSub type=radio value=S name=s checked>Subscribe&nbsp;" +
                    "<INPUT id=RadioUnSub type=radio value=U name=s>UnSubscribe&nbsp;<br>" +
                    "<INPUT id=RadioHTML type=radio value=html name=f checked>HTML&nbsp;" +
                    "<INPUT id=RadioText type=radio value=text name=f>Text&nbsp;<br>" +
                    "<input type=hidden name=g value='" + setGroupID + "'> <input type=hidden name=c value=" + Master.UserSession.CurrentUser.CustomerID + ">" +
                    "<INPUT id=Submit type=submit value=Submit name=submitBtn onclick=\"this.disabled=true;this.form.submit();\">" +
                    "</form>";
                TxtBx_HTMLCode.Text = thecode;
            }
        }

        private void Set_SOHTMLCode(string htmlCode, string selectedFields)
        {
            this.SO_OptinHTMLSave.Enabled = true;
            StringTokenizer st = new StringTokenizer(selectedFields, '|');
            while (st.HasMoreTokens())
            {
                string token = st.NextToken();
                for (int i = 0; i < SO_OptinFieldSelection.Items.Count; i++)
                {
                    if (SO_OptinFieldSelection.Items[i].Text.Equals(token))
                    {
                        SO_OptinFieldSelection.Items[i].Selected = true;
                    }
                }
            }

            SO_HTMLCode.Text = GetHTMLCode(GroupId, "SO").ToString();
        }

        private void LoadSmartFormData(int setSFID)
        {
            var smartFormHistory = ECN_Framework_BusinessLayer.Communicator.SmartFormsHistory.GetBySmartFormID(setSFID, GroupId, Master.UserSession.CurrentUser);
            if (smartFormHistory != null)
            {
                smartFormName.Text = smartFormHistory.SmartFormName;
                Set_SOHTMLCode("", smartFormHistory.SmartFormFields);
                Response_UserMsgSubject.Text = smartFormHistory.Response_UserMsgSubject;
                Response_UserMsgBody.Text = smartFormHistory.Response_UserMsgBody;
                Response_UserScreen.Text = smartFormHistory.Response_UserScreen;
                Response_FromEmail.Text = smartFormHistory.Response_FromEmail;
                Response_AdminEmail.Text = smartFormHistory.Response_AdminEmail;
                Response_AdminMsgSubject.Text = smartFormHistory.Response_AdminMsgSubject;
                Response_AdminMsgBody.Text = smartFormHistory.Response_AdminMsgBody;
            }
        }

        private void LoadSmartFormGrid(int groupID)
        {
            var chID = HttpRequest.GetQueryStringValue(QueryStringKeys.ChannelId);
            var cuID = HttpRequest.GetQueryStringValue(QueryStringKeys.CustomerId);
            var smartFormHistoryList = this.SmartFormsHistory.GetByGroupID(groupID, MasterCommunicator.GetCurrentUser());

            var result = (from src in smartFormHistoryList
                          select new
                          {
                              SmartFormID = "SFID=" + src.SmartFormID + "&GroupID=" + groupID + "&chID=" + chID + "&cuID=" + cuID,
                              DelSmartFormID = src.SmartFormID,
                              RespParams = "GroupID = " + groupID + " & chID = " + chID + " & cuID = " + cuID,
                              SmartFormName = src.SmartFormName
                          }).ToList();
            SmartFormGrid.DataSource = result;
            SmartFormGrid.DataBind();
            GridPager.RecordCount = result.Count;
        }

        private void LoadEmailFields(int groupID, ListBox smartFormType)
        {
            smartFormType.Items.Clear();
            smartFormType.Items.Add(new ListItem("Title", "t"));
            smartFormType.Items.Add(new ListItem("FirstName", "fn"));
            smartFormType.Items.Add(new ListItem("LastName", "ln"));
            smartFormType.Items.Add(new ListItem("FullName", "n"));
            smartFormType.Items.Add(new ListItem("Company", "compname"));
            smartFormType.Items.Add(new ListItem("Occupation", "occ"));
            smartFormType.Items.Add(new ListItem("Address", "adr"));
            smartFormType.Items.Add(new ListItem("Address2", "adr2"));
            smartFormType.Items.Add(new ListItem("City", "city"));
            smartFormType.Items.Add(new ListItem("State", "state"));
            smartFormType.Items.Add(new ListItem("Zip", "zc"));
            smartFormType.Items.Add(new ListItem("Country", "ctry"));
            smartFormType.Items.Add(new ListItem("Phone", "ph"));
            smartFormType.Items.Add(new ListItem("Mobile", "mph"));
            smartFormType.Items.Add(new ListItem("Fax", "fax"));
            smartFormType.Items.Add(new ListItem("Website", "website"));
            smartFormType.Items.Add(new ListItem("Age", "age"));
            smartFormType.Items.Add(new ListItem("Income", "income"));
            smartFormType.Items.Add(new ListItem("Gender", "gndr"));
            smartFormType.Items.Add(new ListItem("User1", "usr1"));
            smartFormType.Items.Add(new ListItem("User2", "usr2"));
            smartFormType.Items.Add(new ListItem("User3", "usr3"));
            smartFormType.Items.Add(new ListItem("User4", "usr4"));
            smartFormType.Items.Add(new ListItem("User5", "usr5"));
            smartFormType.Items.Add(new ListItem("User6", "usr6"));
            smartFormType.Items.Add(new ListItem("Birthdate", "bdt"));
            smartFormType.Items.Add(new ListItem("UserEvent1", "usrevt1"));
            smartFormType.Items.Add(new ListItem("UserEvent1Date", "usrevtdt1"));
            smartFormType.Items.Add(new ListItem("UserEvent2", "usrevt2"));
            smartFormType.Items.Add(new ListItem("UserEvent2Date", "usrevtdt2"));

            List<ECN_Framework_Entities.Communicator.GroupDataFields> groupDataFieldsList =
            ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, Master.UserSession.CurrentUser);

            foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields in groupDataFieldsList)
            {
                smartFormType.Items.Add(new ListItem(groupDataFields.ShortName, "user_" + groupDataFields.ShortName));
            }
        }

        private void ClearSO_Fields()
        {
            smartFormName.Text = "";
            SO_HTMLCode.Text = "";
            Response_FromEmail.Text = "";
            Response_UserMsgSubject.Text = "";
            Response_UserMsgBody.Text = "";
            Response_UserScreen.Text = "";
            Response_AdminEmail.Text = "";
            Response_AdminMsgSubject.Text = "";
            Response_AdminMsgBody.Text = "";
            SO_OptinFieldSelection.ClearSelection();
        }
        #endregion


        #region refresh & save Form button clicks
        protected void RefreshHTML_Click(object sender, System.EventArgs e)
        {
            if (DO_SmartFormButton.Enabled)
            {
                this.SO_OptinHTMLSave.Enabled = true;
                optinHTML = GetHTMLCode(GroupId, "SO").ToString();
                SO_HTMLCode.Text = optinHTML.ToString();
            }
            else if (SO_SmartFormButton.Enabled)
            {
                this.OptinHTMLSave.Enabled = true;
                optinHTML = GetHTMLCode(GroupId, "DO").ToString();
                HTMLCode.Text = optinHTML.ToString();
            }
        }

        protected void SaveOptinHTML_Click(object sender, System.EventArgs e)
        {
            if (SO_SmartFormButton.Enabled)
            {
                try
                {
                    var optinHTML = GetHTMLCode(GroupId, DoFormType).ToString();
                    var communicatorGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(GroupId, Master.UserSession.CurrentUser);
                    communicatorGroup.OptinFields = selectedFields.ToString();
                    communicatorGroup.OptinHTML = optinHTML.ToString();
                    communicatorGroup.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                    ECN_Framework_BusinessLayer.Communicator.Group.Save(communicatorGroup, Master.UserSession.CurrentUser);
                }
                catch (ECNException ex)
                {
                    setECNError(ex);
                }
            }
            else if (DO_SmartFormButton.Enabled)
            {
                try
                {
                    SaveSmartFormsHistory();
                }
                catch (ECNException ex)
                {
                    setECNError(ex);
                }
            }
            UpdateUIAfterSavingSmartFormsHistory();
            SO_OptinHTMLSaveNew.Text = CreateNewSmartForm;
        }

        private void UpdateUIAfterSavingSmartFormsHistory()
        {
            LoadSmartFormGrid(GroupId);
            SO_RefreshHTML.Enabled = false;
            SO_OptinHTMLSave.Enabled = false;
            ClearSO_Fields();
        }

        private void SaveSmartFormsHistory()
        {
            var optinHTML = GetHTMLCode(GroupId, SoFormType).ToString();
            var smartFormHistory = this.SmartFormsHistory.GetBySmartFormID(SFID, GroupId, MasterCommunicator.GetCurrentUser());

            smartFormHistory.GroupID = GroupId;
            smartFormHistory.SmartFormName = this.smartFormName.Text.ToString();
            smartFormHistory.SmartFormHTML = optinHTML.ToString();
            smartFormHistory.SmartFormFields = selectedFields.ToString();
            smartFormHistory.Response_UserMsgSubject = Response_UserMsgSubject.Text.ToString();
            smartFormHistory.Response_UserMsgBody = Response_UserMsgBody.Text.ToString();
            smartFormHistory.Response_UserScreen = Response_UserScreen.Text.ToString();
            smartFormHistory.Response_FromEmail = Response_FromEmail.Text.ToString();
            smartFormHistory.Response_AdminEmail = Response_AdminEmail.Text.ToString();
            smartFormHistory.Response_AdminMsgSubject = Response_AdminMsgSubject.Text.ToString();
            smartFormHistory.Response_AdminMsgBody = Response_AdminMsgBody.Text.ToString();
            smartFormHistory.CustomerID = MasterCommunicator.GetCustomerID();
            smartFormHistory.CreatedUserID = MasterCommunicator.GetUserID();
            smartFormHistory.UpdatedUserID = MasterCommunicator.GetUserID();
            this.SmartFormsHistory.Save(smartFormHistory, MasterCommunicator.GetCurrentUser());
        }

        private string GetHTMLCode(int groupID, string formType)
        {
            var thecode = string.Empty;
            if (formType.Equals("DO"))
            {
                int itemsCnt = OptinFieldSelection.Items.Count;
                string CustomerID = Master.UserSession.CurrentUser.CustomerID.ToString();
                string redirpage = System.Configuration.ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/subscribe.aspx";
                thecode =
                    "<form action=" + redirpage + ">" +
                    "<table border=1><tr>" +
                    "<td>Email Address:</td><td><INPUT id=EmailAddress type=text name=e size=25></td></tr>";

                if (itemsCnt > 0)
                {
                    for (int i = 0; i < itemsCnt; i++)
                    {
                        if (OptinFieldSelection.Items[i].Selected)
                        {
                            thecode += "<tr><td>" + OptinFieldSelection.Items[i].Text.ToString() + ": </td><td><INPUT id=" + OptinFieldSelection.Items[i].Text.ToString() + " type=text name=" + OptinFieldSelection.Items[i].Value + " size=15></td></tr>";
                            selectedFields += OptinFieldSelection.Items[i].Text.ToString() + "|";
                        }
                    }
                }
                thecode +=
                    "<tr><td colspan=2><INPUT id=RadioSub type=radio value=S name=s checked>Subscribe&nbsp;" +
                    "<INPUT id=RadioUnSub type=radio value=U name=s>UnSubscribe&nbsp;</td></tr>" +
                    "<tr><td colspan=2><INPUT id=RadioHTML type=radio value=html name=f checked>HTML&nbsp;" +
                    "<INPUT id=RadioText type=radio value=text name=f>Text&nbsp;</td></tr>" +
                    "<tr><td colspan=2 align=center><input type=hidden name=g value='" + groupID + "'><input type=hidden name=c value=" + CustomerID + ">" +
                    "<INPUT id=Submit type=submit value=Submit name=submitBtn onclick=\"this.disabled=true;this.form.submit();\"></td></tr></table></form>";
            }
            else if (formType.Equals(SoFormType))
            {
                var itemsCnt = SO_OptinFieldSelection.Items.Count;
                var CustomerID = MasterCommunicator.GetCustomerID().ToString();
                var redirpage = string.Format("{0}/engines/SO_subscribe.aspx", ConfigurationManager.AppSettings["Activity_DomainPath"]);
                thecode =
                    "<form action=" + redirpage + ">" +
                    "<table border=1><tr>" +
                    "<td>Email Address:</td><td><INPUT id=EmailAddress type=text name=e size=25></td></tr>";

                if (itemsCnt > 0)
                {
                    for (int i = 0; i < itemsCnt; i++)
                    {
                        if (SO_OptinFieldSelection.Items[i].Selected)
                        {
                            thecode += "<tr><td>" + SO_OptinFieldSelection.Items[i].Text.ToString() + ": </td><td><INPUT id=" + SO_OptinFieldSelection.Items[i].Text.ToString() + " type=text name=" + SO_OptinFieldSelection.Items[i].Value + " size=15></td></tr>";
                            selectedFields += SO_OptinFieldSelection.Items[i].Text.ToString() + "|";
                        }
                    }
                }
                thecode +=
                    "<tr><td colspan=2 align=center>" +
                    "	<INPUT type=hidden value=S name=s><INPUT type=hidden value=html name=f>" +
                    "	<input type=hidden name=g value='" + groupID + "'><input type=hidden name=c value=" + CustomerID + ">" +
                    "	<input type=hidden name=sfID value='" + SFID + "'>" +
                    "	<INPUT id=Submit type=submit value=Submit name=submitBtn onclick=\"this.disabled=true;this.form.submit();\">" +
                    "</td></tr></table></form>";
            }
            return thecode;
        }
        #endregion

        #region smartForm selection button Clicks
        protected void DO_SmartFormButton_Click(object sender, System.EventArgs e)
        {
            var requestGroupID = GroupId;
            SO_panelFCKEditor.Enabled = false;
            SO_panelFCKEditor.Visible = false;
            SO_SmartFormButton.Enabled = true;
            DO_SmartFormButton.Enabled = false;
            DO_panelFCKEditor.Enabled = true;
            DO_panelFCKEditor.Visible = true;
            panelTexbox.Enabled = false;
            panelTexbox.Visible = false;
            LoadEmailFields(requestGroupID, OptinFieldSelection);
            SetHTMLCode(requestGroupID, true, HTMLCode);
        }

        protected void SO_SmartFormButton_Click(object sender, System.EventArgs e)
        {
            var requestGroupID = GroupId;

            DO_panelFCKEditor.Enabled = false;
            DO_panelFCKEditor.Visible = false;
            DO_SmartFormButton.Enabled = true;
            SO_SmartFormButton.Enabled = false;
            SO_panelFCKEditor.Enabled = true;
            SO_panelFCKEditor.Visible = true;
            panelTexbox.Enabled = false;
            panelTexbox.Visible = false;
            LoadEmailFields(requestGroupID, SO_OptinFieldSelection);
            SO_HTMLCode.Text = "";
            LoadSmartFormGrid(requestGroupID);
            SO_RefreshHTML.Enabled = false;
            SO_OptinHTMLSave.Enabled = false;
            ClearSO_Fields();
        }

        protected void RP_SmartFormButton_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("referralprogram.aspx?GroupID=" + GroupId + "&chID=" + Master.UserSession.CurrentBaseChannel.BaseChannelID + "&cuID=" + Master.UserSession.CurrentUser.CustomerID);
        }

        protected void PP_SmartFormButton_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("groupsubscribePrePopSF.aspx?GroupID=" + GroupId + "&chID=" + Master.UserSession.CurrentBaseChannel.BaseChannelID + "&cuID=" + Master.UserSession.CurrentUser.CustomerID);
        }

        protected void SO_OptinHTMLSaveNew_Click(object sender, EventArgs e)
        {
            try
            {
                SaveSmartFormsHistory();
                UpdateUIAfterSavingSmartFormsHistory();
            }
            catch (ECNException ex)
            {
                setECNError(ex);
            }
        }
        #endregion

    }
}
