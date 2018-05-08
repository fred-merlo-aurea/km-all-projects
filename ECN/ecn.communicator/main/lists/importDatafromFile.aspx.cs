using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using KM.Common.UserControls;

namespace ecn.communicator.listsmanager
{
    public partial class importDatafromFile : ECN_Framework.WebPageHelper
    {
        private const string EmailAddressError = "ERROR - Email Address is required to import data.";
        private const string ErrorDuplicateField = "ERROR - You have selected duplicate field names.<BR><BR>{0}.";
        private const string Ignore = "ignore";
        private const string ContentPlaceHolderSelect = "$ContentPlaceHolder1$ColumnHeaderSelect";
        private const string Action = "Action";
        private const string SortOrder = "sortOrder";
        private const string ActionCode = "ActionCode";
        private const string Totals = "Totals";
        private const string SortAsc = "sortorder asc";
        private const string TimeToImport = "Time to Import";
        private const string StartingImport = "Starting Import!";
        private ImportMapper _importMapper = new ImportMapper();

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

            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "grouppriv") || KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.ImportEmails))
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

        public void ImportData(object sender, System.EventArgs e)
        {
            bool UDFExists = false;
            ArrayList colRemove = new ArrayList();
            bool EmailAddressOnly = true;
            var dtFile = new DataTable();
            DateTime startDateTime = DateTime.Now;

            if (!ValidateMapping(startDateTime))
            {
                return;
            }

            try
            {
                try
                {
                    dtFile = GetDataTableByFileType(file, sheetName, 0);
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
                for (int i = 0; i < dtFile.Columns.Count; i++)
                {
                    string selectedColumnName = Request.Params.Get(Master.ClientID + "$ContentPlaceHolder1$ColumnHeaderSelect" + i);

                    if (selectedColumnName.ToLower() == "ignore")
                    {
                        colRemove.Add("delete" + i);
                        dtFile.Columns[i].ColumnName = "delete" + i;
                    }
                    else
                    {
                        if (selectedColumnName.ToLower().ToString().IndexOf("user_") > -1)
                            UDFExists = true;
                        else if (selectedColumnName.ToLower() != "emailaddress")
                            EmailAddressOnly = false;

                        dtFile.Columns[i].ColumnName = selectedColumnName.ToLower().ToString();
                    }
                }

                for (int j = 0; j < colRemove.Count; j++)
                {
                    dtFile.Columns.Remove(colRemove[j].ToString());
                }
                colRemove.Clear();

                Hashtable hGDFFields = GetGroupDataFields(Convert.ToInt32(gid));
                Import(dtFile, UDFExists, hGDFFields, EmailAddressOnly);
                hGDFFields.Clear();

                if (hUpdatedRecords.Count > 0)
                {
                    gvImport.DataSource = GetDataView(startDateTime);
                    gvImport.DataBind();
                    ImportButton.Visible = false;

                }
            }
            catch (Exception ex)
            {
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(ex, "ImportDataFromFile.ImportData", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"].ToString()), CreateNote());
                printerr("An error occurred and was logged.  Please try again.  If it continues, please contact your digital services specialist.");
                Reset(_importMapper);
            }
        }

        private void Import(DataTable dataTableFile, bool udfExists, Hashtable hGDFFields, bool emailAddressOnly)
        {
            var bRowCreated = false;
            var xmlUDF = new StringBuilder("");
            var xmlProfile = new StringBuilder("");
            var iTotalRecords = dataTableFile.Rows.Count;
            var iProgressInc = iTotalRecords / 50;
            var iProgressCount = iProgressInc;
            var iProgressPercent = 0;

            for (var fileCount = 0; fileCount < dataTableFile.Rows.Count; fileCount++)
            {
                if (fileCount == 0 && dataTableFile.Rows.Count >= 100)
                {
                    initNotify(StartingImport);
                }

                var dataRowFile = dataTableFile.Rows[fileCount];
                bRowCreated = false;

                xmlProfile.Append("<Emails>");

                foreach (DataColumn dcFile in dataTableFile.Columns)
                {
                    if (dcFile.ColumnName.IndexOf("user_") == -1 && dcFile.ColumnName.IndexOf("delete") == -1)
                    {
                        if (dataRowFile[dcFile.ColumnName].ToString().Trim().Length > 0)
                        {
                            xmlProfile.Append("<" + dcFile.ColumnName + ">" + CleanXMLString(dataRowFile[dcFile.ColumnName].ToString()) + "</" + dcFile.ColumnName + ">");
                        }
                    }

                    if (udfExists)
                    {
                        if (hGDFFields.Count > 0)
                        {
                            if (dcFile.ColumnName.IndexOf("user_") > -1)
                            {
                                if (!bRowCreated)
                                {
                                    xmlUDF.Append("<row>")
                                          .Append("<ea>" + CleanXMLString(dataRowFile["emailaddress"].ToString()) + "</ea>");
                                    bRowCreated = true;
                                }

                                if (dataRowFile[dcFile.ColumnName].ToString().Trim().Length > 0)
                                {
                                    xmlUDF.Append("<udf id=\"" + hGDFFields[dcFile.ColumnName].ToString() + "\">")
                                          .Append("<v><![CDATA[" + CleanXMLString(dataRowFile[dcFile.ColumnName].ToString()) + "]]></v>")
                                          .Append("</udf>");
                                }
                            }
                        }
                    }
                }

                xmlProfile.Append("</Emails>");

                if (bRowCreated)
                {
                    xmlUDF.Append("</row>");
                }

                if (fileCount != 0 && fileCount % 5000 == 0 || fileCount == dataTableFile.Rows.Count - 1)
                {
                    UpdateToDB(Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID), Convert.ToInt32(gid), "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>", emailAddressOnly);
                    xmlProfile = new StringBuilder("");
                    xmlUDF = new StringBuilder("");
                    GC.Collect();
                }

                if ((fileCount == iProgressCount || fileCount == dataTableFile.Rows.Count - 1) && dataTableFile.Rows.Count >= 100)
                {
                    iProgressPercent = iProgressPercent + 2;

                    Notify(iProgressPercent <= 100 ? iProgressPercent.ToString() : "101", "Importing (" + iProgressCount.ToString() + " / " + iTotalRecords.ToString() + ")");

                    iProgressCount = iProgressCount + iProgressInc;

                    if (fileCount == dataTableFile.Rows.Count - 1)
                    {
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        private DataView GetDataView(DateTime startDateTime)
        {
            var dtRecords = new DataTable();
            dtRecords.Columns.Add(Action);
            dtRecords.Columns.Add(ActionCode);
            dtRecords.Columns.Add(Totals);
            dtRecords.Columns.Add(SortOrder);

            DataRow row;

            foreach (DictionaryEntry dictionaryEntry in hUpdatedRecords)
            {
                row = dtRecords.NewRow();

                if (dictionaryEntry.Key.ToString() == "T")
                {
                    row = SetDataRow("Total Records in the File", string.Empty, 1, row);
                }
                else if (dictionaryEntry.Key.ToString() == "I")
                {
                    row = SetDataRow("New", dictionaryEntry.Key.ToString(), 2, row);
                }
                else if (dictionaryEntry.Key.ToString() == "U")
                {
                    row = SetDataRow("Changed", dictionaryEntry.Key.ToString(), 3, row);
                }
                else if (dictionaryEntry.Key.ToString() == "D")
                {
                    row = SetDataRow("Duplicate(s)", dictionaryEntry.Key.ToString(), 4, row);
                }
                else if (dictionaryEntry.Key.ToString() == "S")
                {
                    row = SetDataRow("Skipped", dictionaryEntry.Key.ToString(), 5, row);
                }
                else if (dictionaryEntry.Key.ToString() == "M")
                {
                    row = SetDataRow("Skipped (Emails in Master Suppression)", dictionaryEntry.Key.ToString(), 6, row);
                }

                row[Totals] = dictionaryEntry.Value;
                dtRecords.Rows.Add(row);
            }

            row = dtRecords.NewRow();
            row[Action] = string.Empty;
            row[Totals] = " ";
            row[SortOrder] = 8;
            dtRecords.Rows.Add(row);

            var duration = DateTime.Now - startDateTime;
            row = dtRecords.NewRow();
            row[Action] = TimeToImport;
            row[Totals] = duration.Hours + ":" + duration.Minutes + ":" + duration.Seconds;
            row[SortOrder] = 9;
            dtRecords.Rows.Add(row);

            var dataView = dtRecords.DefaultView;
            dataView.Sort = SortAsc;

            return dataView;
        }

        private DataRow SetDataRow(string action, string actionCode, int sortOrder, DataRow dataRow)
        {
            dataRow[Action] = action;
            dataRow[SortOrder] = sortOrder;
            dataRow[ActionCode] = actionCode;

            return dataRow;
        }

        private bool ValidateMapping(DateTime startDateTime)
        {
            var duplicationColumnCount = 0;
            var dataTableFile = new DataTable();
            
            try
            {
                dataTableFile = GetDataTableByFileType(file, sheetName, 1);

                var duplicatedColumns = new StringBuilder();

                for (var index = 0; index < dataTableFile.Columns.Count; index++)
                {
                    var selectedColumnName = Request.Params.Get(Master.ClientID + ContentPlaceHolderSelect + index);
                    if (selectedColumnName.Equals(Ignore, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    if (!_importMapper.AddMapping(index, selectedColumnName))
                    {
                        duplicationColumnCount++;
                        duplicatedColumns.Append(string.Format("{0}{1}", duplicatedColumns.Length > 0 ? "/" : "", selectedColumnName));
                    }
                }

                if (duplicationColumnCount > 0 || _importMapper.MappingCount == 0 || !_importMapper.HasEmailAddress)
                {
                    if (duplicationColumnCount > 0)
                    {
                        printerr(string.Format(ErrorDuplicateField, duplicatedColumns.ToString()));
                    }
                    else
                    {
                        printerr(EmailAddressError);
                    }
                    Reset(_importMapper);
                    return false;
                }
            }
            catch
            {
                printerr(EmailAddressError);
                Reset(_importMapper);
                return false;
            }

            return true;
        }

        private string CreateNote()
        {
            StringBuilder sbNote = new StringBuilder();
            sbNote.AppendLine("Current Customer: " + Master.UserSession.CurrentCustomer.CustomerID.ToString());
            sbNote.AppendLine("Current User: " + Master.UserSession.CurrentUser.UserID.ToString());
            sbNote.AppendLine("Group ID: " + gid.ToString());
            sbNote.AppendLine("File Name: " + file);

            return sbNote.ToString();
        }

        private string CleanXMLString(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            text = text.Replace("\r", "");
            text = text.Replace("\n", "");
            return text;
        }

        private void UpdateToDB(int CustomerID, int GroupID, string xmlProfile, string xmlUDF, bool EmailaddressOnly)
        {
            DataTable dtRecords = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(Master.UserSession.CurrentUser, Master.UserSession.CurrentUser.CustomerID, GroupID, xmlProfile, xmlUDF, ftc, stc, EmailaddressOnly, file + " " + lblGUID.Text, "Ecn.communicator.main.lists.importDatafromFile.UpdateToDB");
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

            //Do the dupe column check
            List<string> columns = new List<string>();
            DataRow drColumns = dt.Rows[0];
            int columnIndex = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                string columnname = "";
                if (fileType.Equals("C"))
                    columnname = dc.ToString();// dr[i].ToString();
                else
                    columnname = drColumns[columnIndex].ToString();
                if (!columns.Contains(columnname.ToLower()))
                {
                    
                    columns.Add(columnname.ToLower());
                }
                else
                {
                    Response.Redirect("importmanager.aspx?error=duplicate");
                }
                columnIndex++;
            }

            for (int i = 0; i < countColumns; i++)
            {
                string columnname = string.Empty; //Grab data from 1st row ; match with fieldname if matches preselect column.
                string textData = "";
                int lineStartCheck = 0;
                
                foreach (DataRow dr in dt.Rows)
                {
                    if (lineStartCheck == 0)
                    {

                        if (fileType.Equals("C"))
                            columnname = dt.Columns[i].ToString();// dr[i].ToString();
                        else
                            columnname = dr[i].ToString();
                    }
                    textData += dr[i].ToString() + ", ";
                    lineStartCheck++;
                    if (lineStartCheck == 10)
                        break;
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
                DataTable emailstable = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ExportFromImportEmails(Master.UserSession.CurrentUser, file + " " + lblGUID.Text, actionCode);

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
                    ((LinkButton)e.Row.FindControl("lnkTotals")).Visible = false;
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
            columnHeaderSelect.Sort();

            return new SelectBuilder(selectBoxName)
                .Bind(columnHeaderSelect)
                .SelectItems(colName)
                .Build();
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
                {
                    Response.Redirect("importmanager.aspx?error=unknown");
                }
            }
            catch(ECNException ecn)
            {
                Response.Redirect("importmanager.aspx?error=duplicate");
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
            List<ECN_Framework_Entities.Communicator.GroupDataFields> groupDataFieldsList = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, Master.UserSession.CurrentUser);
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