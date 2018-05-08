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
using System.Data.OleDb;		
using System.IO;			
using System.Text;
using System.Collections.Generic;
using System.Threading;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;
using CommonStringFunctions = KM.Common.StringFunctions;

using Role = KM.Platform.User;

namespace ecn.communicator.listsmanager
{
    public partial class ATHBimportDatafromFile : ECN_Framework.WebPageHelper
    {
        private const int OneSecondTimeOut = 1000;
        private const string ColumnNameAction = "Action";
        private const string ColumnNameActionCode = "ActionCode";
        private const string ColumnNameSortOrder = "sortOrder";
        private const string ColumnNameTotals = "Totals";
        private const int BatchSize = 100;
        private const int MaxBatchSize = 5000;

        protected System.Web.UI.HtmlControls.HtmlSelect tableColumnHeadersSelectbox; 
        protected System.Web.UI.WebControls.Panel panel;

        string file = "";
        string ftc = "";
        string stc = "";
        string gid = "";
        string dupes = "";
        string fileType = "";
        string separator = "";
        string sheetName = "";
        string lineStart = "";
        string dl = string.Empty;

        ArrayList columnHeadings = new ArrayList();
        Hashtable hUpdatedRecords = new Hashtable();

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }
        KMPlatform.Entity.User SessionCurrentUser { get { return Master.UserSession.CurrentUser; } }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            if (!IsPostBack)
            {
                lblGUID.Text = Guid.NewGuid().ToString();
                ECN_Framework_Entities.Communicator.Group grp = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(Convert.ToInt32(Request.QueryString["gid"]), Master.UserSession.CurrentUser);
                lblGroupName.Text = "Group: " + grp.GroupName;
                lblFileName.Text = "File: " + Request.QueryString["file"];
            }
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS;
            Master.SubMenu = "";
            Master.Heading = "Import Data";
            Master.HelpContent = "<img align='right' src=/ecn.images/images/icogroups.gif><b>Import Emails from Files</b><br />You can also upload a large list of email addresses. Our system will except either text files or CSV files!<br /><br /><b>Adding Files to Library</b><br />Click <i>Browse</i> to select the file, or type the path to the file which has the email addresses.<br />Move the file to the box by clicking <i>Add</i> button.<br /><font size=1>(File Transfer times vary depending on your connection)</font><br />Highlight the added file and click on <i>Remove</i> to remove the file.<br />Click the <i>Upload</i> button to upload the file to ECNblaster server.<br /><br /><b>Import emails to Group from file</b><br />Select the group you want to import the emails.<br />Select the Subscriber type and the Format of email the subscriber wants to receive (html/plain Text).<br />Select the file which has the email collection.<br />Click on the Import button to import the emails.</P>";
            Master.HelpTitle = "Groups Manager";

            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "grouppriv") || KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            //if (Role.IsAdministratorOrHasUserPermission(SessionCurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Groups))
            if (Role.HasAccess(SessionCurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Groups, KMPlatform.Enums.Access.ImportEmails) )
            {
                getDatafromQueryString(); 
                errlabel.Text = "";
                msglabel.Text = "";

                if (Page.IsPostBack)
                {
                    return;
                }
                BuildEmailImportForm(GetDataTableByFileType(file, sheetName, 5));
            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }
        
        public void ImportData(object sender, EventArgs e)
        {
            var xmlUdf = new StringBuilder(string.Empty);
            var xmlProfile = new StringBuilder(string.Empty);
            var mapper = new ImportMapper();
            var colRemove = new ArrayList();
            var emailAddressOnly = true;

            var startDateTime = DateTime.Now;
            var dataTableFile = GetDataTableByFileType(file, sheetName, 1);
            if (!AddMappings(mapper, dataTableFile))
            {
                return;
            }

            try
            {
                dataTableFile = GetDataTableFile();

                var udfExists = ProcessColumns(dataTableFile, colRemove, ref emailAddressOnly);
                foreach (var column in colRemove)
                {
                    dataTableFile.Columns.Remove(column.ToString());
                }

                colRemove.Clear();
                var groupDataFields = GetGroupDataFields(Convert.ToInt32(gid));
                var totalRecords = dataTableFile.Rows.Count;
                var progressInc = totalRecords / 50;
                var progressCount = progressInc;
                var progressPercent = 0;

                for (var rowIndex = 0; rowIndex < dataTableFile.Rows.Count; rowIndex++)
                {
                    if (rowIndex == 0 
                        && dataTableFile.Rows.Count >= BatchSize)
                    {
                        initNotify("Starting Import!");
                    }

                    ProcessGroupDataFields(dataTableFile, rowIndex, xmlProfile, xmlUdf, udfExists, groupDataFields);

                    if ((rowIndex != 0 && rowIndex % MaxBatchSize == 0) || rowIndex == dataTableFile.Rows.Count - 1)
                    {
                        UpdateToDB(
                            Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID),
                            Convert.ToInt32(Request.QueryString["gid"]),
                            $"<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>{xmlProfile}</XML>",
                            $"<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>{xmlUdf}</XML>",
                            emailAddressOnly);

                        xmlProfile = new StringBuilder(string.Empty);
                        xmlUdf = new StringBuilder(string.Empty);
                        GC.Collect();
                    }

                    if ((rowIndex == progressCount || rowIndex == dataTableFile.Rows.Count - 1) && dataTableFile.Rows.Count >= BatchSize)
                    {
                        progressPercent = progressPercent + 2;

                        Notify(progressPercent <= BatchSize ? progressPercent.ToString() : "101", $"Importing ({progressCount} / {totalRecords})");

                        progressCount = progressCount + progressInc;

                        if (rowIndex == dataTableFile.Rows.Count - 1)
                        {
                            Thread.Sleep(OneSecondTimeOut);
                        }
                    }
                }

                groupDataFields.Clear();

                if (hUpdatedRecords.Count > 0)
                {
                    CreateRows(startDateTime);
                }
            }
            catch (Exception exception)
            {
                printerr($"ERROR: {exception.Message}");
                Reset(mapper);
            }
        }

        private DataTable GetDataTableFile()
        {
            DataTable dataTableFile = null;
            try
            {
                dataTableFile = GetDataTableByFileType(file, sheetName, 0);
            }
            catch (OleDbException oledbEx)
            {
                if (oledbEx.Message.Contains(
                    $"\'{sheetName}$\' is not a valid name.  Make sure that it does not include invalid characters or punctuation and that it is not too long"))
                {
                    Response.Redirect("importmanager.aspx?error=sheetname");
                }
                else
                {
                    throw;
                }
            }

            return dataTableFile;
        }

        private void ProcessGroupDataFields(
            DataTable dataTableFile,
            int rowIndex,
            StringBuilder xmlProfile,
            StringBuilder xmlUdf,
            bool udfExists,
            IDictionary groupDataFields)
        {
            var dataRow = dataTableFile.Rows[rowIndex];

            var firstname = string.Empty;
            var emailaddress = string.Empty;
            var lastname = string.Empty;
            var state = string.Empty;
            xmlProfile.Append("<Emails>");

            foreach (DataColumn dataColumn in dataTableFile.Columns)
            {
                if (dataColumn.ColumnName.Contains("user_")
                    || dataColumn.ColumnName.Contains("delete"))
                {
                    continue;
                }

                if (dataColumn.ColumnName.Equals("emailaddress", StringComparison.OrdinalIgnoreCase)
                    && dataRow[dataColumn.ColumnName]
                        .ToString()
                        .Equals(string.Empty))
                {
                    emailaddress = CleanXMLString($"{Guid.NewGuid()}@kmpsgroup.com");
                    xmlProfile.Append("<" + dataColumn.ColumnName + ">" + emailaddress + "</" + dataColumn.ColumnName + ">");
                }
                else
                {
                    if (dataColumn.ColumnName.Equals("emailaddress", StringComparison.OrdinalIgnoreCase))
                    {
                        emailaddress = CleanXMLString(
                            dataRow[dataColumn.ColumnName]
                                .ToString());
                    }

                    xmlProfile.Append(
                        $"<{dataColumn.ColumnName}>{CleanXMLString(dataRow[dataColumn.ColumnName] .ToString())}</{dataColumn.ColumnName}>");
                }

                if (dataColumn.ColumnName.Equals("firstname", StringComparison.OrdinalIgnoreCase))
                {
                    firstname = CleanXmlString(dataRow[dataColumn.ColumnName]);
                }

                if (dataColumn.ColumnName.Equals("lastname", StringComparison.OrdinalIgnoreCase))
                {
                    lastname = CleanXmlString(dataRow[dataColumn.ColumnName]);
                }

                if (dataColumn.ColumnName.Equals("state", StringComparison.OrdinalIgnoreCase))
                {
                    state = CleanXmlString(dataRow[dataColumn.ColumnName]);
                }
            }

            xmlUdf.Append("<row>");
            xmlUdf.Append($"<ea  kv=\"{firstname} {lastname} {state}\">{emailaddress}</ea>");
            foreach (DataColumn dataColumn in dataTableFile.Columns)
            {
                if (udfExists 
                    && groupDataFields.Count > 0 
                    && dataColumn.ColumnName.IndexOf("user_", StringComparison.Ordinal) > -1)
                {
                    if (dataRow[dataColumn.ColumnName]
                            .ToString()
                            .Trim()
                            .Length > 0)
                    {
                        xmlUdf.Append($"<udf id=\"{groupDataFields[dataColumn.ColumnName]}\">");
                        xmlUdf.Append($"<v>{CleanXMLString(dataRow[dataColumn.ColumnName] .ToString())}</v>");
                        xmlUdf.Append("</udf>");
                    }
                }
            }

            xmlUdf.Append("</row>");
            xmlProfile.Append("<user1>" + firstname + " " + lastname + " " + state + "</user1>");
            xmlProfile.Append("</Emails>");
        }

        private static string CleanXmlString(object dataValue)
        {
            return CommonStringFunctions.EscapeXmlString(dataValue.ToString());
        }

        private bool ProcessColumns(DataTable dataTableFile, ArrayList colRemove, ref bool emailAddressOnly)
        {
            var udfExists = false;
            for (var columnIndex = 0; columnIndex < dataTableFile.Columns.Count; columnIndex++)
            {
                var selectedColumnName = Request.Params.Get(
                    $"{Master.ClientID}$ContentPlaceHolder1$ColumnHeaderSelect{columnIndex}");

                if (selectedColumnName.Equals("ignore", StringComparison.OrdinalIgnoreCase))
                {
                    colRemove.Add("delete" + columnIndex);
                    dataTableFile.Columns[columnIndex]
                        .ColumnName = "delete" + columnIndex;
                }
                else
                {
                    if (selectedColumnName.IndexOf("user_", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        udfExists = true;
                    }
                    else if (!selectedColumnName.Equals("emailaddress", StringComparison.OrdinalIgnoreCase))
                    {
                        emailAddressOnly = false;
                    }

                    dataTableFile.Columns[columnIndex]
                        .ColumnName = selectedColumnName.ToLower();
                }
            }

            return udfExists;
        }

        private bool AddMappings(ImportMapper mapper, DataTable dataTableFile)
        {
            try
            {
                var duplicationColumnCount = 0;
                var duplicatedColumns = new StringBuilder();

                for (var columnIndex = 0; columnIndex < dataTableFile.Columns.Count; columnIndex++)
                {
                    var selectedColumnName =
                        Request.Params.Get($"{Master.ClientID}$ContentPlaceHolder1$ColumnHeaderSelect{columnIndex}");

                    if (selectedColumnName.Equals("ignore", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (!mapper.AddMapping(columnIndex, selectedColumnName))
                    {
                        duplicationColumnCount++;
                        var columnPrefix = duplicatedColumns.Length > 0 
                                                ? "/" 
                                                : string.Empty;

                        duplicatedColumns.Append($"{columnPrefix}{selectedColumnName}");
                    }
                }

                if (duplicationColumnCount > 0)
                {
                    printerr($"ERROR - You have selected duplicate field names.<BR><BR>{duplicatedColumns}.");
                    Reset(mapper);
                    return false;
                }

                if (mapper.MappingCount == 0)
                {
                    printerr("ERROR - At lease one col is required to import data.");
                    Reset(mapper);
                    return false;
                }
            }
            catch (Exception exception)
            {
                printerr("ERROR :" + exception.Message);
                Reset(mapper);
                return false;
            }

            return true;
        }

        private void CreateRows(DateTime startDateTime)
        {
            var dataTableRecords = new DataTable();

            dataTableRecords.Columns.Add(ColumnNameAction);
            dataTableRecords.Columns.Add(ColumnNameActionCode);
            dataTableRecords.Columns.Add(ColumnNameTotals);
            dataTableRecords.Columns.Add(ColumnNameSortOrder);

            DataRow row;

            foreach (DictionaryEntry dictionaryEntry in hUpdatedRecords)
            {
                row = dataTableRecords.NewRow();

                switch (dictionaryEntry.Key.ToString())
                {
                    case "T":
                        row[ColumnNameAction] = "Total Records in the File";
                        row[ColumnNameActionCode] = string.Empty;
                        row[ColumnNameSortOrder] = 1;
                        break;
                    case "I":
                        row[ColumnNameAction] = "New";
                        row[ColumnNameActionCode] = "I";
                        row[ColumnNameSortOrder] = 2;
                        break;
                    case "U":
                        row[ColumnNameAction] = "Changed";
                        row[ColumnNameSortOrder] = 3;
                        row[ColumnNameActionCode] = "U";
                        break;
                    case "D":
                        row[ColumnNameAction] = "Duplicate(s)";
                        row[ColumnNameActionCode] = "D";
                        row[ColumnNameSortOrder] = 4;
                        break;
                    case "S":
                        row[ColumnNameAction] = "Skipped";
                        row[ColumnNameActionCode] = "S";
                        row[ColumnNameSortOrder] = 5;
                        break;
                    case "M":
                        row[ColumnNameAction] = "Skipped (Emails in Master Suppression)";
                        row[ColumnNameSortOrder] = 6;
                        row[ColumnNameActionCode] = "M";
                        break;
                }

                row[ColumnNameTotals] = dictionaryEntry.Value;
                dataTableRecords.Rows.Add(row);
            }

            row = dataTableRecords.NewRow();
            row[ColumnNameAction] = string.Empty;
            row[ColumnNameTotals] = " ";
            row[ColumnNameSortOrder] = 8;
            dataTableRecords.Rows.Add(row);

            var duration = DateTime.Now - startDateTime;

            row = dataTableRecords.NewRow();
            row[ColumnNameAction] = "Time to Import";
            row[ColumnNameTotals] = $"{duration.Hours}:{duration.Minutes}:{duration.Seconds}";
            row[ColumnNameSortOrder] = 9;
            dataTableRecords.Rows.Add(row);

            var dv = dataTableRecords.DefaultView;
            dv.Sort = "sortorder asc";

            gvImport.DataSource = dv;
            gvImport.DataBind();
            ImportButton.Visible = false;
        }

        private string CleanXMLString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text;
        }

        private void UpdateToDB(int CustomerID, int GroupID, string xmlProfile, string xmlUDF, bool EmailaddressOnly)
        {
            DataTable dtRecords = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmailsWithDupes(Master.UserSession.CurrentUser, GroupID, xmlProfile, xmlUDF, ftc, stc, false, "user1", false, "ecn.communicator.ATHBimportDatafromFile");
 
           if (dtRecords.Rows.Count > 0)
            {
                foreach (DataRow dr in dtRecords.Rows)
                {
                    if (!hUpdatedRecords.Contains(dr["Action"].ToString()))
                        hUpdatedRecords.Add(dr["Action"].ToString().ToUpper(), Convert.ToInt32(dr["Counts"]));
                    else
                    {
                        int eTotal = Convert.ToInt32(hUpdatedRecords[dr["Action"].ToString().ToUpper()]);
                        hUpdatedRecords[dr["Action"].ToString().ToUpper()] = eTotal + Convert.ToInt32(dr["Counts"]);
                    }
                }

            }
        }

        private void Reset(ImportMapper mapper)
        {
            BuildEmailImportForm(GetDataTableByFileType(file, sheetName, 5), mapper);
            ImportButton.Visible = true;
        }


        #region Method to build HTML table for import data
        private void BuildEmailImportForm(DataTable dt)
        {
            BuildEmailImportForm(dt, new ImportMapper());
        }

        private void BuildEmailImportForm(DataTable dt, ImportMapper mapper)
        {
            if (dt == null)
            {
                return;
            }

            HtmlTableRow tableRows = null;
            HtmlTableCell headerColumn = null;	// <td> to hold the header which is the emails table dropdown columns
            HtmlTableCell dataColumn = null;		// <td> to hold the data from the file
            int countColumns = dt.Columns.Count;


            for (int i = 0; i < countColumns; i++)
            {
                string columnname = string.Empty; //Grab data from 1st row ; match with fieldname if matches preselect column.
                string textData = "";
                int lineStartCheck = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    if (lineStartCheck == 0)
                        columnname = dr[i].ToString();

                    textData += dr[i].ToString() + ", ";
                    lineStartCheck++;
                }

                tableColumnHeadersSelectbox = buildColumnHeaderDropdowns("ColumnHeaderSelect" + i, columnname);
                if (tableColumnHeadersSelectbox.SelectedIndex < 0)
                    tableColumnHeadersSelectbox.SelectedIndex = tableColumnHeadersSelectbox.Items.IndexOf(tableColumnHeadersSelectbox.Items.FindByText(mapper.GetColumnName(i)));
                tableRows = new HtmlTableRow();		
                headerColumn = new HtmlTableCell();	
                headerColumn.Controls.Add(tableColumnHeadersSelectbox);	
                headerColumn.VAlign = "middle";
                headerColumn.Align = "middle";
                headerColumn.Height = "32px";
                headerColumn.Style.Add("background-color", "#D6DFFF");
                tableRows.Cells.Add(headerColumn);	

                dataColumn = new HtmlTableCell();
                dataColumn.Style.Add("font-family", "Verdana, Arial, Helvetica, sans-serif");
                dataColumn.Style.Add("font-size", "10px");
                dataColumn.Style.Add("background-color", "#D6DFFF");
                Label tableDataColumnLabel = new Label();


                tableDataColumnLabel.Text =
                string.Format("<font color='black'>{0}</font>",
                ECN_Framework_Common.Functions.StringFunctions.Left(textData, textData.Length > 100 ? 100 : textData.Length) + ((textData.Length > 100) ? "<font color=orange><b>&nbsp;&nbsp;&nbsp;more...</b></font>" : " "));
                dataColumn.Controls.Add(tableDataColumnLabel);
                tableRows.Cells.Add(dataColumn);

                dataCollectionTable.Rows.Add(tableRows);	
            }
        }

        protected void gvImport_Command(Object sender, GridViewCommandEventArgs e)
        {

            string actionCode = e.CommandArgument.ToString();
            if (actionCode.Equals("U") || actionCode.Equals("S") || actionCode.Equals("M") || actionCode.Equals("D") || actionCode.Equals("I"))
            {
                DataTable dt = (DataTable)ViewState["SupressionGroups_DataTable"];
                string downloadType = ".xls";
                ArrayList columnHeadings = new ArrayList();
                IEnumerator aListEnum = null;
                string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/downloads/");

                string newline = "";
                DateTime date = DateTime.Now;
                string filename = actionCode + "-" + file;
                String tfile = filename + downloadType;
                string outfileName = txtoutFilePath + tfile;

                if (!Directory.Exists(txtoutFilePath))
                {
                    Directory.CreateDirectory(txtoutFilePath);
                }

                if (File.Exists(outfileName))
                {
                    File.Delete(outfileName);
                }

                TextWriter txtfile = File.AppendText(outfileName);
                DataTable emailstable = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ExportFromImportEmails(Master.UserSession.CurrentUser, file +" "+ lblGUID.Text, actionCode);

                for (int i = 0; i < emailstable.Columns.Count; i++)
                {
                    columnHeadings.Add(emailstable.Columns[i].ColumnName.ToString());
                }

                aListEnum = columnHeadings.GetEnumerator();
                while (aListEnum.MoveNext())
                {
                    newline += aListEnum.Current.ToString() + (downloadType == ".xls" ? "\t" : ", ");
                }
                txtfile.WriteLine(newline);


                foreach (DataRow dr in emailstable.Rows)
                {
                    newline = "";
                    aListEnum.Reset();
                    while (aListEnum.MoveNext())
                    {
                        newline += dr[aListEnum.Current.ToString()].ToString() + (downloadType == ".xls" ? "\t" : ", ");
                    }
                    txtfile.WriteLine(newline);
                }
                txtfile.Close();

                if (downloadType == ".xls")
                {
                    Response.ContentType = "application/vnd.ms-excel";
                }
                else
                {
                    Response.ContentType = "text/csv";
                }

                Response.AddHeader("content-disposition", "attachment; filename=" + tfile);
                Response.WriteFile(outfileName);
                Response.Flush();
                Response.End();
            }
        }

        protected void gvImport_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string actionCode = ((Label)e.Row.FindControl("lblActionCode")).Text;
                if (actionCode.Equals("U") || actionCode.Equals("S") || actionCode.Equals("M") || actionCode.Equals("D") || actionCode.Equals("I"))
                {
                    ((LinkButton)e.Row.FindControl("lnkTotals")).Visible = true;
                    ((Label)e.Row.FindControl("lblTotals")).Visible = false;
                }
                else
                {
                    ((LinkButton)e.Row.FindControl("lnkTotals")).Visible=false;
                    ((Label)e.Row.FindControl("lblTotals")).Visible = true;
                }
            }
        }


        private HtmlSelect buildColumnHeaderDropdowns(string selectBoxName, string colName)
        {
            ArrayList columnHeaderSelect = new ArrayList();
            for (int i = 0; i < ColumnManager.ColumnCount; i++)
            {
                columnHeaderSelect.Insert(i, ColumnManager.GetColumnNameByIndex(i));
            }

            HtmlSelect selectbox = new HtmlSelect();
            selectbox.ID = selectBoxName;
            selectbox.Style.Add("font-family", "Verdana, Arial, Helvetica, sans-serif");
            selectbox.Style.Add("font-size", "11px");
            selectbox.Style.Add("background-color", "#FCF8E9");
            selectbox.DataSource = columnHeaderSelect;	
            selectbox.DataBind();

            try
            {
                foreach (ListItem li in selectbox.Items)
                {
                    if (li.Text.ToLower() == colName.ToLower() || li.Text.ToLower() == "user_" + colName.ToLower())
                    {
                        li.Selected = true;
                        break;
                    }
                }
            }
            catch
            {

            }
            return selectbox;
        }

        #endregion

        #region Import data helper methods/properties
        private DataTable GetDataTableByFileType(string fileName, string excelSheetName, int maxRecordsToRetrieve)
        {
            try
            {
                int startLine = (Convert.ToInt32(lineStart) - 1 >= 0) ? (Convert.ToInt32(lineStart) - 1) : 0;
                string physicalDataPath = getPhysicalPath();
                return FileImporter.GetDataTableByFileType(physicalDataPath, fileType, fileName, excelSheetName, startLine, maxRecordsToRetrieve, dl.ToUpper());
            }
            catch (System.Data.OleDb.OleDbException oledbEx)
            {
                if (oledbEx.Message.Contains("'" + sheetName + "$' is not a valid name.  Make sure that it does not include invalid characters or punctuation and that it is not too long"))
                {
                    Response.Redirect("importmanager.aspx?error=sheetname");
                }
                else
                    throw oledbEx;
            }
            return new DataTable();

        }
        private List<ECN_Framework_Entities.Communicator.GroupDataFields> _groupDataFields = null;
        protected List<ECN_Framework_Entities.Communicator.GroupDataFields> GroupDataFieldsList
        {
            get
            {
                if (_groupDataFields == null)
                {
                    _groupDataFields = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(Convert.ToInt32(gid), Master.UserSession.CurrentUser);
                }
                return (this._groupDataFields);
            }
        }

        private Hashtable GetGroupDataFields(int groupID)
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields>  groupDataFieldsList=ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, Master.UserSession.CurrentUser);
            Hashtable fields = new Hashtable();
            foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields in groupDataFieldsList)
                fields.Add("user_" + groupDataFields.ShortName.ToLower(), groupDataFields.GroupDataFieldsID);
            return fields;
        }

        private EmailTableColumnManager _columnManager = null;
        public EmailTableColumnManager ColumnManager
        {
            get
            {
                if (_columnManager == null)
                {
                    _columnManager = new EmailTableColumnManager();
                    foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields in GroupDataFieldsList)
                    {
                        ColumnManager.AddGroupDataFields(groupDataFields.ShortName);
                    }
                }
                return (this._columnManager);
            }
        }
        private string getPhysicalPath()
        {
            string DataPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/data");

            if (!Directory.Exists(DataPath))
            {
                Directory.CreateDirectory(DataPath);
            }
            return DataPath;
        }

        private string trimQuotes(string strToTrim)
        {
            string cleanString = "";
            if (strToTrim.Length > 0)
            {
                if (strToTrim.Equals("''"))
                {
                    cleanString = "";
                }
                else if (strToTrim.Equals("\"\""))
                {
                    cleanString = "";
                }
                else
                {
                    cleanString = ECN_Framework_Common.Functions.StringFunctions.TrimQuotes(strToTrim);
                }
            }
            return cleanString;
        }
        #endregion

        #region Other private methods
        private void getDatafromQueryString()
        {
            file = Request.QueryString["file"];
            ftc = Request.QueryString["ftc"];
            stc = Request.QueryString["stc"];
            gid = Request.QueryString["gid"];
            dupes = Request.QueryString["dups"];
            fileType = Request.QueryString["ft"];
            separator = Request.QueryString["sep"];
            sheetName = Request.QueryString["sheet"];
            lineStart = Request.QueryString["line"];
            dl = Request.QueryString["dl"];
        }

        private void print(string text)
        {
            msglabel.Text += text;
            msglabel.Visible = true;
        }
        private void printerr(string text)
        {
            errlabel.Text = text;
            errlabel.Visible = true;
            ImportButton.Visible = false;
        }
        #endregion

        #region ProgressBar
        public void initNotify(string StrSplash)
        {
            // Only do this on the first call to the page
            //Register loadingNotifier.js for showing the Progress Bar
            Response.Write(string.Format(@"<link href='/ecn.accounts/styles/progressbar.css' type='text/css' rel='stylesheet' />
				<body><script type='text/javascript' src='/ecn.accounts/scripts/ProgressBar.js'></script>
              <script language='javascript' type='text/javascript'>
              initLoader('{0}');
              </script></body>", StrSplash));
            // Send it to the client
            Response.Flush();


        }
        public void Notify(string strPercent, string strMessage)
        {
            //Update the Progress bar
            Response.Write(string.Format("<script language='javascript' type='text/javascript'>setProgress({0},'{1}'); </script>", strPercent, strMessage));
            Response.Flush();
        }
        #endregion
         
    }


}