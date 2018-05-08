using System;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.ECNWizard.Group
{
    public partial class newGroup_Import : System.Web.UI.UserControl
    {
        protected System.Web.UI.HtmlControls.HtmlSelect tableColumnHeadersSelectbox;
        Hashtable hUpdatedRecords = new Hashtable();

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
            RaiseBubbleEvent("Upload", new EventArgs());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;

        }

        private void throwECNException(string message)
        {
            ECN_Framework_Common.Objects.ECNError ecnError = new ECNError(Enums.Entity.Group, Enums.Method.Save, message);
            List<ECNError> errorList= new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        protected void btnUpload_Click(object sender, System.EventArgs e)
        {

            reset();
            if (!FileUpload1.HasFile)
            {
                throwECNException("ERROR - A file must be specified to upload.");
                return;
            }
            else if (FileUpload1.PostedFile.FileName.ToLower().EndsWith(".xls") && txtSheetName.Text.Length == 0)
            {
                throwECNException("ERROR - Enter the Worksheet name for the Excel file.");
                return;
            }

            // check File Size - if > 50MB display message.
            if (FileUpload1.PostedFile.ContentLength > 52428800)
            {
                throwECNException("ERROR - Your File exceeds 50 mb or 100,000 records.");
                return;
            }

            if (FileUpload1.PostedFile.FileName.ToLower().EndsWith(".xls") || FileUpload1.PostedFile.FileName.ToLower().EndsWith(".xlsx") || FileUpload1.PostedFile.FileName.ToLower().EndsWith(".xml") ||
                FileUpload1.PostedFile.FileName.ToLower().EndsWith(".txt") || FileUpload1.PostedFile.FileName.ToLower().EndsWith(".csv"))
            {
                string filename = System.IO.Path.GetFileNameWithoutExtension(FileUpload1.PostedFile.FileName);
                filename = ECN_Framework_Common.Functions.StringFunctions.ReplaceNonAlphaNumeric(filename, "_");
                filename = filename + System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);

                lblImportFileName.Text = filename;
                string FileUploadPath = getPhysicalPath();

                if (!Directory.Exists(FileUploadPath))
                    Directory.CreateDirectory(FileUploadPath);

                FileUpload1.PostedFile.SaveAs(FileUploadPath + "\\" + filename);
                DataTable dtFile = new DataTable();

                // Check No of Records in File - If > 10000 - dislay message
                try
                {
                    dtFile = GetDataTableByFileType(filename, txtSheetName.Text, 100001);
                }
                catch (System.Data.OleDb.OleDbException oledbEx)
                {
                    if (oledbEx.Message.Contains("'"+txtSheetName.Text+"$' is not a valid name.  Make sure that it does not include invalid characters or punctuation and that it is not too long"))
                    {
                        throwECNException("ERROR - Please check the sheet name.");
                        return;
                    }
                    else
                        throw oledbEx;

                }
                if (dtFile == null)
                {
                    throwECNException("ERROR - No Records Found in the uploaded file.");
                    return;
                }
                if (dtFile.Rows.Count > 100000)
                {
                    throwECNException("ERROR - Your File exceeds 50 mb or 100,000 records.");
                    return;
                }
                else
                {
                    lblfilename.Text = filename;
                    BuildMappingGrid(filename);
                    plUpload.Visible = true;
                    btnImport.Visible = true;
                }
                dtFile.Dispose();
            }
            else
            {
                throwECNException("ERROR - Cannot Upload File: <br><br>Only files with following extensions (Excel, XML, TXT & CSV) are supported.");
            }
            RaiseBubbleEvent("Upload", e);
        }

        private bool isValidPhoneNumber(string phoneNumber)
        {

            Regex regex = new Regex(@"\d{10}");
            return regex.IsMatch(phoneNumber);
        }

        private Hashtable GetGroupDataFields()
        {
            List<ECN_Framework_Entities.Communicator.GroupDataFields> groupDataFieldsList=
            ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(GroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            Hashtable fields = new Hashtable();
            foreach (ECN_Framework_Entities.Communicator.GroupDataFields groupDataFields in groupDataFieldsList)
                fields.Add("user_" + groupDataFields.ShortName.ToLower(), groupDataFields.GroupDataFieldsID);

            return fields;
        }

        protected void btnImport_Click(object sender, System.EventArgs e)
        {
            lblGUID.Text = Guid.NewGuid().ToString();
            int duplicationColumnCount = 0;
            bool UDFExists = false;
            StringBuilder xmlUDF = new StringBuilder("");
            StringBuilder xmlProfile = new StringBuilder("");
            ImportMapper mapper = new ImportMapper();
            ArrayList colRemove = new ArrayList();
            DateTime startDateTime = DateTime.Now;
            bool EmailAddressOnly = true;
            bool noEmailAddress = false;
            DataTable dtFile;
            bool MobileNumbersOnly = false;
            string controlClientID = this.ClientID.Replace("_", "$") + "$ColumnHeaderSelect";
            try
            {
                btnImport.Visible = false;

                #region Validate Mapping
                dtFile = GetDataTableByFileType(lblfilename.Text, txtSheetName.Text, 1);

                StringBuilder duplicatedColumns = new StringBuilder();
              
                for (int i = 0; i < dtFile.Columns.Count; i++)
                {
                    string selectedColumnName = Request.Params.Get(controlClientID + i);
                    if (selectedColumnName.ToLower() == "ignore")
                    {
                        continue;
                    }
                    if (!mapper.AddMapping(i, selectedColumnName))
                    {
                        duplicationColumnCount++;
                        duplicatedColumns.Append(string.Format("{0}{1}", duplicatedColumns.Length > 0 ? "/" : "", selectedColumnName));
                    }
                }

                if (duplicationColumnCount > 0)
                {
                    Reset(mapper);
                    throwECNException(string.Format("ERROR - You have selected duplicate field names.<BR><BR>{0}.", duplicatedColumns.ToString()));
                    return;
                }

                if (mapper.MappingCount == 0)
                {
                    Reset(mapper);
                    if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SMS))
                        throwECNException("ERROR - Email Address or Phone Number is required to import data.");
                    else
                        throwECNException("ERROR - Email Address is required to import data.");
                    return;
                }
                if (!mapper.HasEmailAddress)
                {
                    noEmailAddress = true;
                }

                if (noEmailAddress==true && mapper.HasMobileNumber==false)
                {
                    Reset(mapper);
                    if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SMS))
                        throwECNException("ERROR - Email Address or Phone Number is required to import data.");
                    else
                        throwECNException("ERROR - Email Address is required to import data.");
                    return;
                }               
                #endregion          
            }
            catch
            {
                Reset(mapper);

                if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SMS))
                    throwECNException("ERROR - Email Address or Phone Number is required to import data.");
                else
                    throwECNException("ERROR - Email Address is required to import data.");
                return;
            }

            try
            {               
                dtFile = GetDataTableByFileType(lblfilename.Text, txtSheetName.Text, 0);
                for (int i = 0; i < dtFile.Columns.Count; i++)
                {
                    string selectedColumnName = Request.Params.Get(controlClientID + i);
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

                if (noEmailAddress)
                {
                    dtFile.Columns.Add("emailaddress", typeof(string));
                    for (int i = 0; i < dtFile.Rows.Count; i++)
                    {
                        if (isValidPhoneNumber(dtFile.Rows[i]["mobile"].ToString()))
                        {
                            dtFile.Rows[i]["emailaddress"] = dtFile.Rows[i]["mobile"].ToString() + "@KMautogenerated.com";
                        }
                    }
                    MobileNumbersOnly = true;
                }
                Hashtable hGDFFields = GetGroupDataFields();

                bool bRowCreated = false;

                int iTotalRecords = dtFile.Rows.Count;
                int iProgressInc = iTotalRecords / 10;
                int iProgressCount = iProgressInc;
                int iProgressPercent = 0;

                for (int cnt = 0; cnt < dtFile.Rows.Count; cnt++)
                {
                    DataRow drFile = dtFile.Rows[cnt];

                    bRowCreated = false;

                    xmlProfile.Append("<Emails>");

                    foreach (DataColumn dcFile in dtFile.Columns)
                    {
                        if (dcFile.ColumnName.IndexOf("user_") == -1 && dcFile.ColumnName.IndexOf("delete") == -1)
                        {
                            xmlProfile.Append("<" + dcFile.ColumnName + ">" + StringFunctions.CleanXMLString(drFile[dcFile.ColumnName].ToString()) + "</" + dcFile.ColumnName + ">");
                        }

                        if (UDFExists)
                        {
                            if (hGDFFields.Count > 0)
                            {
                                if (dcFile.ColumnName.IndexOf("user_") > -1)
                                {
                                    if (!bRowCreated)
                                    {
                                        xmlUDF.Append("<row>");
                                        xmlUDF.Append("<ea>" + StringFunctions.CleanXMLString(drFile["emailaddress"].ToString()) + "</ea>");
                                        bRowCreated = true;
                                    }

                                    xmlUDF.Append("<udf id=\"" + hGDFFields[dcFile.ColumnName].ToString() + "\">");

                                    xmlUDF.Append("<v><![CDATA[" + StringFunctions.CleanXMLString(drFile[dcFile.ColumnName].ToString()) + "]]></v>");

                                    xmlUDF.Append("</udf>");
                                }
                            }
                        }

                    }
                    xmlProfile.Append("</Emails>");

                    if (bRowCreated)
                        xmlUDF.Append("</row>");

                    if ((cnt != 0) && (cnt % 10000 == 0) || (cnt == dtFile.Rows.Count - 1))
                    {
                        UpdateToDB("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlProfile.ToString() + "</XML>", "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>" + xmlUDF.ToString() + "</XML>", EmailAddressOnly, MobileNumbersOnly);

                        xmlProfile = new StringBuilder("");
                        xmlUDF = new StringBuilder("");
                    }

                    if (((cnt == iProgressCount) || (cnt == dtFile.Rows.Count - 1)) && (dtFile.Rows.Count >= 100))
                    {
                        iProgressPercent = iProgressPercent + 10;
                        iProgressCount = iProgressCount + iProgressInc;

                        if (cnt == dtFile.Rows.Count - 1)
                        {
                            System.Threading.Thread.Sleep(1000);
                        }
                    }

                }
                hGDFFields.Clear();
                plUpload.Visible = false;
                plImportCompleted.Visible = true;

                if (hUpdatedRecords.Count > 0)
                {
                    DataTable dtRecords = new DataTable();

                    dtRecords.Columns.Add("Action");
                    dtRecords.Columns.Add("ActionCode");
                    dtRecords.Columns.Add("Totals");
                    dtRecords.Columns.Add("sortOrder");

                    DataRow row;

                    foreach (DictionaryEntry de in hUpdatedRecords)
                    {
                        row = dtRecords.NewRow();

                        if (de.Key.ToString() == "T")
                        {
                            row["Action"] = "Total Records in the File";
                            row["ActionCode"] = "";
                            row["sortOrder"] = 1;
                        }
                        else if (de.Key.ToString() == "I")
                        {
                            row["Action"] = "New";
                            row["ActionCode"] = "I";
                            row["sortOrder"] = 2;
                        }
                        else if (de.Key.ToString() == "U")
                        {
                            row["Action"] = "Changed";
                            row["sortOrder"] = 3;
                            row["ActionCode"] = "U";
                        }
                        else if (de.Key.ToString() == "D")
                        {
                            row["Action"] = "Duplicate(s)";
                            row["ActionCode"] = "D";
                            row["sortOrder"] = 4;
                        }
                        else if (de.Key.ToString() == "S")
                        {
                            row["Action"] = "Skipped";
                            row["ActionCode"] = "S";
                            row["sortOrder"] = 5;
                        }
                        else if (de.Key.ToString() == "M")
                        {
                            row["Action"] = "Skipped (Emails in Master Suppression)";
                            row["sortOrder"] = 6;
                            row["ActionCode"] = "M";
                        }
                        row["Totals"] = de.Value;
                        dtRecords.Rows.Add(row);
                    }

                    row = dtRecords.NewRow();
                    row["Action"] = "";
                    row["Totals"] = " ";
                    row["sortOrder"] = 8;
                    dtRecords.Rows.Add(row);

                    TimeSpan duration = DateTime.Now - startDateTime;

                    row = dtRecords.NewRow();
                    row["Action"] = "Time to Import";
                    row["Totals"] = duration.Hours + ":" + duration.Minutes + ":" + duration.Seconds;
                    row["sortOrder"] = 9;
                    dtRecords.Rows.Add(row);

                    DataView dv = dtRecords.DefaultView;
                    dv.Sort = "sortorder asc";

                    gvImport.DataSource = dv;
                    gvImport.DataBind();

                }
            }
            catch (Exception ex)
            {
                Reset(mapper);
                throwECNException("ERROR: " + ex.Message);
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
                string txtoutFilePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID + "/downloads/");

                string newline = "";
                DateTime date = DateTime.Now;
                string filename = actionCode + "-" + FileUpload1.FileName;
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
                DataTable emailstable = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ExportFromImportEmails(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, lblImportFileName.Text + " " + lblGUID.Text, actionCode);

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

        private void UpdateToDB(string xmlProfile, string xmlUDF, bool EmailaddressOnly, bool MobileNumbersOnly)
        {
             DataTable dtRecords = new DataTable();
            if(!MobileNumbersOnly)
                dtRecords = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, GroupID, xmlProfile, xmlUDF, "HTML", "S", false, lblImportFileName.Text + " " + lblGUID.Text, "Ecn.communicator.main.ecnwizard.group.importSubscibers.UpdateToDB - Not MobileNumbersOnly");
            else
                dtRecords = ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, GroupID, xmlProfile, xmlUDF, "HTML", "U", false, lblImportFileName.Text + " " + lblGUID.Text, "Ecn.communicator.main.ecnwizard.group.importSubscibers.UpdateToDB - MobileNumbersOnly");
         
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

        private void BuildMappingGrid(string file)
        {
            BuildEmailImportForm(GetDataTableByFileType(file, txtSheetName.Text, 6), new ImportMapper());
        }

        private void Reset(ImportMapper mapper)
        {
            BuildEmailImportForm(GetDataTableByFileType(lblfilename.Text, txtSheetName.Text, 6), mapper);
            btnImport.Visible = true;
        }

        private void BuildEmailImportForm(DataTable dtFile, ImportMapper mapper)
        {
            if (dtFile == null)
            {
                throwECNException("ERROR - No Records Found in the uploaded file.");
                return;
            }

            HtmlTableRow tableRows = null;
            HtmlTableCell headerColumn = null;	
            HtmlTableCell dataColumn = null;		

            for (int i = 0; i < dtFile.Columns.Count; i++)
            {
                if (i == 0)
                {
                    tableRows = new HtmlTableRow();
                    headerColumn = new HtmlTableCell();
                    headerColumn.Style.Add("background-color", "#cccccc");
                    headerColumn.Width = "25%";
                    headerColumn.Style.Add("padding-left", "5px");

                    dataColumn = new HtmlTableCell();
                    dataColumn.Style.Add("background-color", "#cccccc");
                    dataColumn.Width = "75%";
                    dataColumn.Style.Add("padding-left", "5px");

                    Label lblCol1Header = new Label();
                    Label lblCol2Header = new Label();

                    lblCol1Header.CssClass = "label10";
                    lblCol1Header.Text = "<strong>Field Name</strong>";

                    lblCol2Header.CssClass = "label10";
                    lblCol2Header.Text = "<strong>Data</strong>";

                    headerColumn.Controls.Add(lblCol1Header);
                    dataColumn.Controls.Add(lblCol2Header);

                    tableRows.Cells.Add(headerColumn);
                    tableRows.Cells.Add(dataColumn);
                    dataCollectionTable.Rows.Add(tableRows);
                }
                               
                tableColumnHeadersSelectbox = buildColumnHeaderDropdowns("ColumnHeaderSelect" + i);
                tableColumnHeadersSelectbox.SelectedIndex = tableColumnHeadersSelectbox.Items.IndexOf(tableColumnHeadersSelectbox.Items.FindByText(mapper.GetColumnName(i)));
                tableRows = new HtmlTableRow();		
                headerColumn = new HtmlTableCell();	
                headerColumn.Controls.Add(tableColumnHeadersSelectbox);	
                headerColumn.VAlign = "middle";
                headerColumn.Align = "center";

                tableRows.Cells.Add(headerColumn);	
                dataColumn = new HtmlTableCell();
                dataColumn.Style.Add("font-family", "Verdana, Arial, Helvetica, sans-serif");
                dataColumn.Style.Add("font-size", "10px");
                dataColumn.Style.Add("padding-left", "5px");

                Label tableDataColumnLabel = new Label();
                string textData = "";
                int lineStartCheck = 0;
                foreach (DataRow dr in dtFile.Rows)
                {
                    lineStartCheck++;
                    if (lineStartCheck > 5)
                    {
                        textData += "<font color=orange><b>&nbsp;&nbsp;&nbsp;more...</b></font>";
                        break;
                    }
                    textData += dr[i].ToString() + ", ";
                }

                tableDataColumnLabel.Text = textData;
                dataColumn.Controls.Add(tableDataColumnLabel);
                tableRows.Cells.Add(dataColumn);

                dataCollectionTable.Rows.Add(tableRows);	
            }
        }

        private HtmlSelect buildColumnHeaderDropdowns(string selectBoxName)
        {
            ArrayList columnHeaderSelect = new ArrayList();
            for (int i = 0; i < ColumnManager.ColumnCount; i++)
                columnHeaderSelect.Insert(i, ColumnManager.GetColumnNameByIndex(i));

            HtmlSelect selectbox = new HtmlSelect();
            selectbox.ID = selectBoxName;
            selectbox.Attributes.Add("class", "label10");
            selectbox.DataSource = columnHeaderSelect;	
            selectbox.DataBind();
            return selectbox;
        }

        private string getPhysicalPath()
        {
            string DataPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID + "/data");

            if (!Directory.Exists(DataPath))
            {
                Directory.CreateDirectory(DataPath);
            }
            return DataPath;
        }

        private DataTable GetDataTableByFileType(string fileName, string excelSheetName, int maxRecordsToRetrieve)
        {
            int startLine = 0;
            string physicalDataPath = getPhysicalPath();

            string fileType = string.Empty;

            if (fileName.ToLower().EndsWith(".xls") || fileName.ToLower().EndsWith(".xlsx"))
                fileType = "X";
            else if (fileName.ToLower().EndsWith(".txt"))
                fileType = "O";
            else if (fileName.ToLower().EndsWith(".csv"))
                fileType = "C";
            else if (fileName.ToLower().EndsWith(".xml"))
                fileType = "XML";

            return FileImporter.GetDataTableByFileType(physicalDataPath, fileType, fileName, excelSheetName, startLine, maxRecordsToRetrieve, "");
        }

        private List<ECN_Framework_Entities.Communicator.GroupDataFields> _groupDataFields = null;
        protected List<ECN_Framework_Entities.Communicator.GroupDataFields> GroupDataFieldsList
        {
            get
            {
                if (_groupDataFields == null)
                {
                    _groupDataFields = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(GroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                }
                return (this._groupDataFields);
            }
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

        protected List<ECN_Framework_Entities.Communicator.GroupDataFields> GroupDataFields
        {
            get
            {
                return ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(GroupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            }
        }

        public void reset()
        {
            plUpload.Visible = false;
            plImportCompleted.Visible = false;
        }
    }
}