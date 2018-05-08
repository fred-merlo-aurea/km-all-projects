﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Communicator;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.ECNWizard.Group
{
    public partial class newGroup_add : System.Web.UI.UserControl
    {
        private const string XmlUdf = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>";
        private const string SaveMethodSourcePath = "Ecn.communicator.main.ecnwizard.group.addSubscibers.Save";
        private const string XmlDeclarationFirstLine = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>";
        private const string CarriageReturnLineFeed = "\r\n";
        private const string LineFeed = "\n";
        private const string CarriageReturn = "\r";
        private const string Comma = ",";
        private const string Email = "Email";
        private const char DelimiterComma = ',';
        private const string KmAutogeneratedDomain = "@KMautogenerated.com";
        private const string XmlElementEndTag = "</XML>";
        private const string ActionColumnName = "Action";
        private const string CountsColumnName = "Counts";
        private const string ResultDataViewSortExpression = "sortorder asc";
        private const string TotalsColumnName = "Totals";
        private const string SortOrderColumnName = "sortOrder";
        private const string NonBreakingSpace = "&nbsp;";
        private const string Space = " ";
        private const string TimeToImport = "Time to Import";
        private const string Colon = ":";
        private const string TotalActionKey = "T";
        private const string TotalActionDescription = "Total Records in the File";
        private const string InsertActionKey = "I";
        private const string InsertActionDescription = "New";
        private const string UpdateActionKey = "U";
        private const string UpdateActionDescription = "Changed";
        private const string DuplicateActionKey = "D";
        private const string DuplicateActionDescription = "Duplicate(s)";
        private const string SkipActionKey = "S";
        private const string SkipActionDescription = "Skipped";
        private const string MasterSkipActionKey = "M";
        private const string MasterSkipActionDescription = "Skipped (Emails in Master Suppression)";
        private const int TotalActionSortOrder = 1;
        private const int InsertActionSortOrder = 2;
        private const int UpdateActionSortOrder = 3;
        private const int DuplicateActionSortOrder = 4;
        private const int SkipActionSortOrder = 5;
        private const int MasterSkipActionSortOrder = 6;

        public int GroupID
        {
            get
            {
                if (ViewState["groupID"] != null)
                    return Convert.ToInt32(ViewState["groupID"]);
                else
                    return 0;
            }
            set
            {
                ViewState["groupID"] = value;
            }
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
            if (Page.IsPostBack == false)
            {
                loadDropDowns(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID);
            }
        }

        public void loadDropDowns(int CustomerID)
        {
            if (KMPlatform.BusinessLogic.Client.HasServiceFeature(CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SMS))
            {
                pnlSubscriberType.Visible = true;
            }
            else
                pnlSubscriberType.Visible = false;
            List<ECN_Framework_Entities.Accounts.Code> codeList = ECN_Framework_BusinessLayer.Accounts.Code.GetAll();
            List<ECN_Framework_Entities.Accounts.Code> codeListSubscribeType = (from src in codeList
                                                                                where src.CodeType == "SubscribeType"
                                                                                select src).ToList();
            SubscribeTypeCode.DataSource = codeList;
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
                if (!(SubscribeTypeCode.Items[j].Value.Equals("S")))
                {
                    SubscribeTypeCode.Items.RemoveAt(j);
                }
                else
                {
                    j++;
                }
            }
        }

        public void Reset()
        {
            txtEmailAddress.Text = "";
            ResultsGrid.Visible = false;
        }

        public bool Save()
        {
            try
            {
                var emailAddressToAdd = txtEmailAddress.Text;
                if (string.IsNullOrWhiteSpace(emailAddressToAdd))
                {
                    return true;
                }

                var startDateTime = DateTime.Now;
                var xmlInsert = BuildInsertXml(emailAddressToAdd);

                var emailRecordsDt = EmailGroup.ImportEmails(
                    ECNSession.CurrentSession().CurrentUser,
                    ECNSession.CurrentSession().CurrentUser.CustomerID,
                    GroupID,
                    xmlInsert,
                    XmlUdf,
                    FormatTypeCode.SelectedItem.Value,
                    SubscribeTypeCode.SelectedItem.Value,
                    true,
                    string.Empty,
                    SaveMethodSourcePath);

                var updatedRecords = CreateActionCountsDictionary(emailRecordsDt);

                if (updatedRecords.Count > 0)
                {
                    UpdateResultGrid(updatedRecords, startDateTime);
                }

                return true;
            }
            catch (ECNException ex)
            {
                setECNError(ex);
                return false;
            }
        }

        protected void rblistType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblistType.SelectedValue.Equals("Email"))
            {
                lblType.Text = "Email Address";
                pnlTypeCode.Visible = true;

            }
            else if (rblistType.SelectedValue.Equals("Mobile"))
            {
                lblType.Text = "Mobile Number";
                pnlTypeCode.Visible = false;
            }
        }

        private string BuildInsertXml(string emailAddressToAdd)
        {
            var xmlInsert = new StringBuilder();
            xmlInsert.Append(XmlDeclarationFirstLine);

            var formattedEmailAddress = GetCommaAsSeparator(emailAddressToAdd);
            var tokenizer = new StringTokenizer(formattedEmailAddress, DelimiterComma);

            while (tokenizer.HasMoreTokens())
            {
                var emailAddress = tokenizer.NextToken().Trim();

                if (!rblistType.SelectedValue.Equals(Email))
                {
                    emailAddress += KmAutogeneratedDomain;
                }

                var emailXmlElement = GetEmailXmlElement(emailAddress);
                xmlInsert.Append(emailXmlElement);
            }

            xmlInsert.Append(XmlElementEndTag);
            return xmlInsert.ToString();
        }

        private static string GetCommaAsSeparator(string emailAddressToAdd)
        {
            var emailAddressBuilder = new StringBuilder(emailAddressToAdd);
            emailAddressBuilder.Replace(CarriageReturnLineFeed, Comma);
            emailAddressBuilder.Replace(LineFeed, Comma);
            emailAddressBuilder.Replace(CarriageReturn, Comma);
            return emailAddressBuilder.ToString();
        }

        private static string GetEmailXmlElement(string content)
        {
            return $"<Emails><emailaddress>{content}</emailaddress></Emails>";
        }

        private static Dictionary<string, int> CreateActionCountsDictionary(DataTable emailRecords)
        {
            var dictionaryActions = new Dictionary<string, int>();

            if (emailRecords.Rows.Count == 0)
            {
                return dictionaryActions;
            }

            foreach (DataRow dataRow in emailRecords.Rows)
            {
                var action = dataRow[ActionColumnName].ToString();
                var count = Convert.ToInt32(dataRow[CountsColumnName]);
                if (!dictionaryActions.ContainsKey(action))
                {
                    dictionaryActions.Add(action.ToUpper(), count);
                }
                else
                {
                    dictionaryActions[action.ToUpper()] += count;
                }
            }

            return dictionaryActions;
        }

        private void UpdateResultGrid(Dictionary<string, int> updatedRecords, DateTime startDateTime)
        {
            var dtRecords = CreateRecordsTable(updatedRecords, startDateTime);
            var dataView = dtRecords.DefaultView;
            dataView.Sort = ResultDataViewSortExpression;

            ResultsGrid.DataSource = dataView;
            ResultsGrid.DataBind();
            ResultsGrid.Visible = true;
        }

        private static DataTable CreateRecordsTable(Dictionary<string, int> updatedRecords, DateTime startDateTime)
        {
            var dtRecords = new DataTable();
            dtRecords.Columns.Add(ActionColumnName);
            dtRecords.Columns.Add(TotalsColumnName);
            dtRecords.Columns.Add(SortOrderColumnName);

            foreach (var countByActionPair in updatedRecords)
            {
                var row = dtRecords.NewRow();
                string action;
                int sortOrder;
                if (GetActionAndSortOrder(countByActionPair, out action, out sortOrder))
                {
                    row[ActionColumnName] = action;
                    row[SortOrderColumnName] = sortOrder;
                }
                row[TotalsColumnName] = countByActionPair.Value;
                dtRecords.Rows.Add(row);
            }

            AddEndRow(dtRecords);
            AddTimeToImportRow(dtRecords, startDateTime);

            return dtRecords;
        }

        private static bool GetActionAndSortOrder(KeyValuePair<string, int> countByActionPair, out string action, out int sortOrder)
        {
            action = string.Empty;
            sortOrder = 0;

            if (countByActionPair.Key == TotalActionKey)
            {
                action = TotalActionDescription;
                sortOrder = TotalActionSortOrder;
            }
            else if (countByActionPair.Key == InsertActionKey)
            {
                action = InsertActionDescription;
                sortOrder = InsertActionSortOrder;
            }
            else if (countByActionPair.Key == UpdateActionKey)
            {
                action = UpdateActionDescription;
                sortOrder = UpdateActionSortOrder;
            }
            else if (countByActionPair.Key == DuplicateActionKey)
            {
                action = DuplicateActionDescription;
                sortOrder = DuplicateActionSortOrder;
            }
            else if (countByActionPair.Key == SkipActionKey)
            {
                action = SkipActionDescription;
                sortOrder = SkipActionSortOrder;
            }
            else if (countByActionPair.Key == MasterSkipActionKey)
            {
                action = MasterSkipActionDescription;
                sortOrder = MasterSkipActionSortOrder;
            }
            else
            {
                return false;
            }

            return true;
        }

        private static void AddEndRow(DataTable records)
        {
            AddRow(records, NonBreakingSpace, Space, 8);
        }

        private static void AddTimeToImportRow(DataTable records, DateTime startDateTime)
        {
            var duration = DateTime.Now - startDateTime;
            var totals = $"{duration.Hours}{Colon}{duration.Minutes}{Colon}{duration.Seconds}";
            AddRow(records, TimeToImport, totals, 9);
        }

        private static void AddRow(DataTable records, string action, string totals, int sortOrderColumnName)
        {
            var row = records.NewRow();
            row[ActionColumnName] = action;
            row[TotalsColumnName] = totals;
            row[SortOrderColumnName] = sortOrderColumnName;
            records.Rows.Add(row);
        }
    }
}