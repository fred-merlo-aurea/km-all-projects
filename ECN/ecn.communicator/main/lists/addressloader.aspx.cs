using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;
using System.IO;
using KM.Common;
using KM.Common.Extensions;
using Role = KM.Platform.User;
using BusinessGroup = ECN_Framework_BusinessLayer.Communicator.Group;
using BusinessEmailGroup = ECN_Framework_BusinessLayer.Communicator.EmailGroup;
using BusinessEmail = ECN_Framework_BusinessLayer.Communicator.Email;
using Enums = ECN_Framework_Common.Objects.Enums;

namespace ecn.communicator.listsmanager.addressloader
{
    public partial class addressloader : ECN_Framework.WebPageHelper
    {
        private const string ActionTotalRecords = "Total Records in the File";
        private const string ActionNew = "New";
        private const string ActionChanged = "Changed";
        private const string ActionDuplicates = "Duplicate(s)";
        private const string ActionSkipped = "Skipped";
        private const string ActionMasterSkipped = "Skipped (Emails in Master Suppression)";
        private const string ActionNbsp = "&nbsp;";
        private const string ActionTImeToImport = "Time to Import";
        private const string ActionCodeTime = "time";
        private const string CodeSubscribed = "S";
        private const string ColumnAction = "Action";
        private const string ColumnActionCode = "ActionCode";
        private const string ColumnTotals = "Totals";
        private const string ColumnSortOrder = "sortOrder";
        private const string CodeTotals = "T";
        private const string CodeInsert = "I";
        private const string CodeUpdated = "U";
        private const string CodeSkipped = "S";
        private const string CodeDuplicate = "D";
        private const string CodeMaster = "M";
        private const string CodeUnsibscribed = "U";
        private const string DelimComma = ",";
        private const char DelimCommaChar = ',';
        private const string DelimSpace = " ";
        private const string ErrorInvalidEmailTemplate = "Invalid email address: {0}";
        private const string EmailAddedTemplate =
            "<font face=verdana size=2 color=#000000>&nbsp;{0} rows updated/inserted </font>";
        private const string ErrorNoGroup = "Please select a group";
        private const string ErrorSelectedGroupDoesNotBelongs =
            "Selected Group does not belong to Current Customer. Customer was changed in a different browser window. Reload your screen to see the Current Customer.";
        private const string SortorderAsc = "sortorder asc";
        private const string SourceAddEmailsUnsubscribed =
            "Ecn.communicator.main.lists.addressloader.AddEmails - SubscribeType U";
        private const string SourceAddEmailsSubscribed =
            "Ecn.communicator.main.lists.addressloader.AddEmails - SubscribeType S";
        private const string TotalshiteSpace = " ";
        private const string TimeTemplate = "{0}:{1}:{2}";
        private const string WindowsEol = "\r\n";
        private const string XmlHeader = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>";
        private const string XmlEmailTemplate = "<Emails><emailaddress>{0}</emailaddress></Emails>";
        private const string XmlClosingTag = "</XML>";

        private static string fileName;
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
            Master.SubMenu = "add emails";
            Master.Heading = "Groups > Add Emails";
            Master.HelpContent = "<B>To Add Individual Emails:</B><br/><div id='par1'><ul><li>Choose the Group you would like to add the emails to</li><li>Choose the Subscribe Type (in most circumstances you will want to choose HTML; if your subscriber can only receive text emails, the system will automatically send them text emails. If you only want to send text messages, then you would select the Text subscribe type)</li><li>Choose the Format Type</li><li>In the “Addresses” box, type in each email address; <b>one email per line</b>. (Do not include separators such as a comma ( , ) or  ( ; ) between the addresses.)</li><li>Once all of your emails have been entered, click <em>Add</em>.</li><li>ECN will show you how many records were then successfully entered.</li>";
            Master.HelpTitle = "Groups Manager";

            groupExplorer1.enableSelectMode();
            Master.MasterRegisterButtonForPostBack(ResultsGrid);
            //KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "grouppriv") || KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser)
            //if (Role.IsAdministrator( Master.UserSession.CurrentUser) || Role.HasUserPermission(Master.UserSession.CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Groups) )
            if (Role.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.AddEmails))
            {
                if (Page.IsPostBack == false)
                {

                    loadDD(Master.UserSession.CurrentUser.CustomerID.ToString());
                }
            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }

        private void loadDD(String CustomerID)
        {
            List<ECN_Framework_Entities.Accounts.Code> codeList = ECN_Framework_BusinessLayer.Accounts.Code.GetAll();
            List<ECN_Framework_Entities.Accounts.Code> codeListSubscribeType = (from src in codeList
                                                                                where src.CodeType == "SubscribeType"
                                                                                select src).ToList();
            SubscribeTypeCode.DataSource = codeListSubscribeType;
            SubscribeTypeCode.DataBind();

            List<ECN_Framework_Entities.Accounts.Code> codeListFormatType = (from src in codeList
                                                                             where src.CodeType == "FormatType"
                                                                             select src).ToList();
            FormatTypeCode.DataSource = codeListFormatType;
            FormatTypeCode.DataBind();

            int j = 0;
            int count = SubscribeTypeCode.Items.Count;
            for (int i = 0; i < count; i++)
            {
                if (!(SubscribeTypeCode.Items[j].Value.Equals("S") || SubscribeTypeCode.Items[j].Value.Equals("U")))
                {
                    SubscribeTypeCode.Items.RemoveAt(j);
                }
                else
                {
                    j++;
                }
            }
        }

        private List<Group> GetSelectedGroups()
        {
            if (rbGroupChoice2.Checked && hfSelectGroupID.Value == "0")
            {
                //All groups
                List<ECN_Framework_Entities.Communicator.Group> groupList = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID), Master.UserSession.CurrentUser);
                var result = (from src in groupList
                              orderby src.GroupName
                              select src).ToList();

                ECN_Framework_Entities.Communicator.Group masterSuppressionGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID), Master.UserSession.CurrentUser);
                result.RemoveAll(x => x.GroupID == masterSuppressionGroup.GroupID);
                return result;
            }
            if (rbGroupChoice3.Checked)
            {
                //Master Suppression Group
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID), Master.UserSession.CurrentUser);
                List<ECN_Framework_Entities.Communicator.Group> groups = new List<Group>();
                if (group != null) { groups.Add(@group); }
                return groups;
            }
            else
            {
                //One group
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(Convert.ToInt32(hfSelectGroupID.Value), Master.UserSession.CurrentUser);
                List<ECN_Framework_Entities.Communicator.Group> groups = new List<Group>();
                if (group != null) { groups.Add(@group); }
                return groups;
            }
        }

        public void AddEmails(object sender, EventArgs e)
        {
            var groups = GetSelectedGroups();

            if (Page.IsValid && groups.Any())
            {
                if (Master.CutomerDDValue() != Master.UserSession.ClientID)
                {
                    throwECNException(ErrorSelectedGroupDoesNotBelongs);
                    return;
                }

                fileName = Guid.NewGuid().ToString();
                var emailsAdded = 0;
                var emailAddressToAdd = Addresses.Text;
                var startDateTime = DateTime.Now;
                var hUpdatedRecords = new Hashtable();

                var masterSuppressionGroup = GetMasterSuppressionGroup();

                var mergedTable = new DataTable();
                if (emailAddressToAdd.Length > 0)
                {
                    emailAddressToAdd = emailAddressToAdd.Replace(WindowsEol, DelimComma);
                    if (emailAddressToAdd.LastIndexOf(DelimComma) == (emailAddressToAdd.Length - 1))
                    {
                        emailAddressToAdd = emailAddressToAdd.Substring(0, emailAddressToAdd.Length - 1);
                    }

                    if (ProcessGroups(groups, emailAddressToAdd, masterSuppressionGroup, ref mergedTable))
                    {
                        return;
                    }

                    CalculateCounts(mergedTable, hUpdatedRecords);

                    if (hUpdatedRecords.Count > 0)
                    {
                        CreateActionTable(hUpdatedRecords, startDateTime);
                    }
                }
                else
                {
                    HideResultsGrid(emailsAdded);
                }
            }
            else
            {
                throwECNException(ErrorNoGroup);
            }
        }

        private void HideResultsGrid(int emailsAdded)
        {
            ResultsGrid.Visible = false;
            MessageLabel.Visible = true;
            MessageLabel.Text = string.Format(EmailAddedTemplate, emailsAdded);
        }

        private Group GetMasterSuppressionGroup()
        {
            var masterSuppressionGroup = BusinessGroup.GetMasterSuppressionGroup(
                Master.UserSession.CurrentCustomer.CustomerID,
                Master.UserSession.CurrentUser);
            return masterSuppressionGroup;
        }

        private void CreateActionTable(Hashtable hUpdatedRecords, DateTime startDateTime)
        {
            Guard.NotNull(hUpdatedRecords, nameof(hUpdatedRecords));

            var dtRecords = new DataTable();

            dtRecords.Columns.Add(ColumnAction);
            dtRecords.Columns.Add(ColumnActionCode);
            dtRecords.Columns.Add(ColumnTotals);
            dtRecords.Columns.Add(ColumnSortOrder);

            DataRow row;

            foreach (DictionaryEntry dictionaryEntry in hUpdatedRecords)
            {
                row = FillRowFromUpdatedRecord(dtRecords, dictionaryEntry);
                dtRecords.Rows.Add(row);
            }

            row = dtRecords.NewRow();
            row[ColumnAction] = ActionNbsp;
            row[ColumnTotals] = TotalshiteSpace;
            row[ColumnSortOrder] = 8;
            dtRecords.Rows.Add(row);

            var duration = DateTime.Now - startDateTime;

            row = dtRecords.NewRow();
            row[ColumnAction] = ActionTImeToImport;
            row[ColumnTotals] = string.Format(TimeTemplate, duration.Hours, duration.Minutes, duration.Seconds);
            row[ColumnActionCode] = ActionCodeTime;
            row[ColumnSortOrder] = 9;
            dtRecords.Rows.Add(row);

            var defaultView = dtRecords.DefaultView;
            defaultView.Sort = SortorderAsc;

            ResultsGrid.DataSource = defaultView;
            ResultsGrid.DataBind();
            ResultsGrid.Visible = true;
            MessageLabel.Visible = false;
        }

        private static DataRow FillRowFromUpdatedRecord(DataTable dtRecords, DictionaryEntry dictionaryEntry)
        {
            Guard.NotNull(dictionaryEntry, nameof(dictionaryEntry));

            var row = dtRecords.NewRow();

            if (dictionaryEntry.Key.ToString() == CodeTotals)
            {
                row[ColumnAction] = ActionTotalRecords;
                row[ColumnActionCode] = CodeTotals;
                row[ColumnSortOrder] = 1;
            }
            else if (dictionaryEntry.Key.ToString() == CodeInsert)
            {
                row[ColumnAction] = ActionNew;
                row[ColumnActionCode] = CodeInsert;
                row[ColumnSortOrder] = 2;
            }
            else if (dictionaryEntry.Key.ToString() == CodeUpdated)
            {
                row[ColumnAction] = ActionChanged;
                row[ColumnActionCode] = CodeUpdated;
                row[ColumnSortOrder] = 3;
            }
            else if (dictionaryEntry.Key.ToString() == CodeDuplicate)
            {
                row[ColumnAction] = ActionDuplicates;
                row[ColumnActionCode] = CodeDuplicate;
                row[ColumnSortOrder] = 4;
            }
            else if (dictionaryEntry.Key.ToString() == CodeSkipped)
            {
                row[ColumnAction] = ActionSkipped;
                row[ColumnActionCode] = CodeSkipped;
                row[ColumnSortOrder] = 5;
            }
            else if (dictionaryEntry.Key.ToString() == CodeMaster)
            {
                row[ColumnAction] = ActionMasterSkipped;
                row[ColumnActionCode] = CodeMaster;
                row[ColumnSortOrder] = 6;
            }

            row[ColumnTotals] = dictionaryEntry.Value;
            return row;
        }

        private bool ProcessGroups(
            IEnumerable<Group> groups, 
            string emailAddressToAdd, 
            Group masterSuppressionGroup,
            ref DataTable mergedTable)
        {
            //Subscribe/Unsubscribe from one or all Groups
            if (SubscribeTypeCode.SelectedValue.Equals(CodeUnsibscribed))
            {
                if (SubscribeToGroups(groups, emailAddressToAdd, masterSuppressionGroup, ref mergedTable))
                {
                    return true;
                }
            } //Subscribe to all groups they aren't currently unsubscribed from
            else if (CodeSubscribed.EqualsIgnoreCase(SubscribeTypeCode.SelectedValue))
            {
                if (SubscribeToUnsubscribedGroups(groups, emailAddressToAdd, ref mergedTable))
                {
                    return true;
                }
            }

            return false;
        }

        private bool SubscribeToGroups(
            IEnumerable<Group> groups, 
            string emailAddressToAdd, 
            Group masterSuppressionGroup,
            ref DataTable mergedTable)
        {
            Guard.NotNull(groups, nameof(groups));

            foreach (var currentGroup in groups)
            {
                var tokenizer = new StringTokenizer(emailAddressToAdd, DelimCommaChar);
                var currentUserGroup = BusinessGroup.GetByGroupID(
                    currentGroup.GroupID,
                    Master.UserSession.CurrentUser);

                if ((currentUserGroup.IsSeedList != true) &&
                    currentUserGroup.GroupID != masterSuppressionGroup.GroupID)
                {
                    var xmlInsert = new StringBuilder();
                    xmlInsert.Append(XmlHeader);
                    while (tokenizer.HasMoreTokens())
                    {
                        var email = tokenizer.NextToken().Trim();
                        var emailGroup = BusinessEmailGroup.GetByEmailAddressGroupID(
                            email,
                            currentUserGroup.GroupID,
                            Master.UserSession.CurrentUser);
                        if (BusinessEmail.IsValidEmailAddress(email))
                        {
                            if (emailGroup != null && emailGroup.SubscribeTypeCode != CodeUnsibscribed)
                            {
                                xmlInsert.AppendFormat(XmlEmailTemplate, email);
                            }
                        }
                        else
                        {
                            throwECNException(string.Format(ErrorInvalidEmailTemplate, email));
                            return true;
                        }
                    }

                    var resultsTable = BusinessEmailGroup.ImportEmails(
                        Master.UserSession.CurrentUser,
                        Master.UserSession.CurrentUser.CustomerID,
                        currentUserGroup.GroupID,
                        string.Concat(xmlInsert, XmlClosingTag),
                        XmlHeader,
                        FormatTypeCode.SelectedItem.Value,
                        SubscribeTypeCode.SelectedItem.Value,
                        true,
                        string.Join(DelimSpace, Master.UserSession.CurrentCustomer.CustomerID, fileName),
                        SourceAddEmailsUnsubscribed);
                    if (mergedTable.Columns.Count == 0)
                    {
                        mergedTable = resultsTable.Clone();
                    }

                    mergedTable.Merge(resultsTable);
                }
            }

            return false;
        }

        private bool SubscribeToUnsubscribedGroups(
            IEnumerable<Group> groups, 
            string emailAddressToAdd, 
            ref DataTable mergedTable)
        {
            Guard.NotNull(groups, nameof(groups));

            foreach (var currentGroup in groups)
            {
                var tokenizer = new StringTokenizer(emailAddressToAdd, DelimCommaChar);
                var xmlInsert = new StringBuilder();
                xmlInsert.Append(XmlHeader);

                var currentGroupId = currentGroup.GroupID;

                while (tokenizer.HasMoreTokens())
                {
                    var email = tokenizer.NextToken().Trim();

                    if (BusinessEmail.IsValidEmailAddress(email))
                    {
                        xmlInsert.AppendFormat(XmlEmailTemplate, email);
                    }
                    else
                    {
                        throwECNException(string.Format(ErrorInvalidEmailTemplate, email));
                        return true;
                    }
                }

                var resultsTable = BusinessEmailGroup.ImportEmails(
                    Master.UserSession.CurrentUser,
                    Master.UserSession.CurrentUser.CustomerID,
                    currentGroupId,
                    string.Concat(xmlInsert, XmlClosingTag),
                    XmlHeader,
                    FormatTypeCode.SelectedItem.Value,
                    SubscribeTypeCode.SelectedItem.Value,
                    true,
                    string.Join(DelimSpace, Master.UserSession.CurrentCustomer.CustomerID, fileName),
                    SourceAddEmailsSubscribed);
                if (mergedTable.Columns.Count == 0)
                {
                    mergedTable = resultsTable.Clone();
                }

                mergedTable.Merge(resultsTable);
            }

            return false;
        }

        private static void CalculateCounts(DataTable tempDT, Hashtable hUpdatedRecords)
        {
            if (tempDT.Rows.Count <= 0) return;
            foreach (DataRow dr in tempDT.Rows)
            {
                if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
                {
                    hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(), Convert.ToInt32(dr["Counts"]));
                }
                else
                {
                    int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
                    hUpdatedRecords[dr["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr["Counts"]);
                }
            }
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Email, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        protected void groupExplorer_Hide(object sender, EventArgs e)
        {

            this.modalPopupGroupExplorer.Hide();
        }
        protected void imgSelectGroup_Click(object sender, ImageClickEventArgs e)
        {
            hfGroupSelectionMode.Value = "SelectGroup";
            groupExplorer1.reset();
            this.modalPopupGroupExplorer.Show();
        }

        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                string source = sender.ToString();
                if (source.Equals("GroupSelected"))
                {
                    int groupID = groupExplorer1.selectedGroupID;
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(groupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    if (hfGroupSelectionMode.Value.Equals("SelectGroup"))
                    {
                        lblSelectGroupName.Text = group.GroupName;
                        hfSelectGroupID.Value = groupID.ToString();
                    }
                    else
                    {

                    }
                    groupExplorer1.reset();
                    this.modalPopupGroupExplorer.Hide();
                    upMain.Update();
                }
            }
            catch { }
            return true;
        }

        protected void GroupChoice_CheckedChanged(object sender, EventArgs e)
        {
            imgSelectGroup.Enabled = rbGroupChoice1.Checked;
            lblSelectGroupName.Text = "-No Group Selected-";
            hfSelectGroupID.Value = "0";
        }

        protected void ResultsGrid_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName.Equals("DownloadEmails"))
            {
                string actionCode = e.CommandArgument.ToString();
                if (actionCode.Equals("U") || actionCode.Equals("S") || actionCode.Equals("M") || actionCode.Equals("D") || actionCode.Equals("I"))
                {

                    string downloadType = ".xls";
                    ArrayList columnHeadings = new ArrayList();
                    IEnumerator aListEnum = null;
                    string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");

                    string newline = "";
                    DateTime date = DateTime.Now;
                    string friendlyFileName = Master.UserSession.CurrentCustomer.CustomerID + "-" + actionCode;
                    String tfile = friendlyFileName + downloadType;
                    string outfileName = txtoutFilePath + tfile;

                    if (!Directory.Exists(txtoutFilePath))
                    {
                        Directory.CreateDirectory(txtoutFilePath);
                    }

                    if (File.Exists(outfileName))
                    {
                        File.Delete(outfileName);
                    }

                    DataTable emailstable = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ExportFromImportEmails(Master.UserSession.CurrentUser, Master.UserSession.CurrentCustomer.CustomerID + " " + fileName, actionCode);

                    string export = ECN_Framework_Common.Functions.DataTableFunctions.ToTabDelimited(emailstable);
                    ECN_Framework_BusinessLayer.Activity.Report.ReportViewerExport.ExportToTab(export, tfile);

                }
            }
        }

        protected void ResultsGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string actionCode = drv["ActionCode"].ToString();
                if (actionCode.Equals("U") || actionCode.Equals("S") || actionCode.Equals("M") || actionCode.Equals("D") || actionCode.Equals("I"))
                {
                    ((LinkButton)e.Item.FindControl("lnkTotals")).Visible = true;

                }
                else if (actionCode.Equals("time") || actionCode.Equals("T"))
                {
                    LinkButton lbTotal = (LinkButton)e.Item.FindControl("lnkTotals");
                    lbTotal.Visible = true;
                    lbTotal.Enabled = false;
                }
                else
                {
                    ((LinkButton)e.Item.FindControl("lnkTotals")).Visible = false;

                }
            }
        }

    }
}
