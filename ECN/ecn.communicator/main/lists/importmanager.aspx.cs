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
using System.IO;
using System.Collections.Generic;
using System.Linq;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.listsmanager
{
    public partial class importmanager : ECN_Framework.WebPageHelper
    {
        delegate void HidePopup();

        protected System.Web.UI.WebControls.Panel separatorValuePanel;

        string DataFilePath = "";

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        private int getGroupID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["GroupID"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private string getError()
        {
            try
            {
                return Request.QueryString["error"].ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private void throwECNException(string message)
        {
            ECN_Framework_Common.Objects.ECNError ecnError = new ECNError(Enums.Entity.Group, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite));
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            string errorMsg = getError();
            HidePopup delGroupsLookupPopup = new HidePopup(GroupsLookupPopupHide);
            this.ctrlgroupsLookup1.hideGroupsLookupPopup = delGroupsLookupPopup;

            if (errorMsg.Equals("sheetname") && IsPostBack==false)
            {
                throwECNException("ERROR - Please check the sheet name.");
            }
            else if (errorMsg.Equals("unknown") && IsPostBack == false)
            {
                throwECNException("ERROR - Issue importing file, please check format and contents.  If you are still having problems, please contact customer service.");
            }
            else if(errorMsg.Equals("duplicate") && IsPostBack == false)
            {
                throwECNException("ERROR - Issue importing file due to duplicate column names.");
            }
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.GROUPS;
            Master.SubMenu = "import data";
            Master.Heading = "Groups > Import Data";
            Master.HelpContent = "<b>To Add Emails from a File:</b><br/><div id='par1'><ul><li>Choose <em>Browse</em></li><li>Find the file you would like to import on your computer</li>&#13;&#10;<li>Select the file, click <em>Open</em></li><li>Click <em>Add</em></li><br/><em class='note'>Note: You can continue to Browse and add files, as needed, and upload them all at one time.</em></li>&#13;&#10;<li>Once all your files have been added, click <em>Upload</em> (Your files will appear in the Library)</li><li>To import the data into a group, choose the group you would like to import the data to.</li>&#13;&#10;<li>Choose the subscribe type.</li><li>Choose the format type.</li><li>Choose which file name the data will be imported from (the lists you have uploaded will appear in the dropdown list for your selection)</li>&#13;&#10;<li>Select the type of file (CSV, Excel)</li><li>Choose which line you would like to start on if different than what is listed.</li><li>Enter Sheet Name from excel workbook – must match exactly, and is case sensitive<br/>&#13;&#10;<em class='note'>Note: You can import data from only one sheet at a time.  If you have multiple sheets in your workbook, repeat this process until data from all sheets is added.</em></li>&#13;&#10;<li>Click <em>Start Import Process</em> (This will bring up the column association screen.</li><li>Here you can associate each data type in your file to a field in the ECN Email Profile.</li>&#13;&#10;<li>For each data type, you will see a sample to the left of a pull down menu.</li><li>From the pull down menu, choose the ECN Email Profile field you would like to import/match the data to.</li>&#13;&#10;<li>Repeat this for each data type.</li><li>Click on <em>Import Data</em> (or choose Ignore not to import the data).</li><li>Once the data has been entered, ECN will show you the results of your group import.</li>&#13;&#10;<li>You have now successfully entered your data into ECN from an external file.</li></ul></div>";
            Master.HelpTitle = "Groups Manager";
           
            //if (KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID, "grouppriv") || KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Email, KMPlatform.Enums.Access.ImportEmails))
            {
                string channelID = Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString();

                DataFilePath = "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/data";

                if (!Directory.Exists(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + DataFilePath)))
                    Directory.CreateDirectory(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + DataFilePath));

                uploadbox.uploadDirectory = DataFilePath;
                loadFilesTable(DataFilePath);

                if (Page.IsPostBack == false)
                {
                    loadDD(Master.UserSession.CurrentUser.CustomerID);
                    SubscribeTypeCode.Items.Clear();
                    SubscribeTypeCode.Items.Add(new ListItem("Subscribes", "S"));
                    SubscribeTypeCode.Items.Add(new ListItem("UnSubscribes", "U"));
                }

                string deletefile = getDeleteFile();
                if (deletefile != "")
                {
                    deleteFile(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + DataFilePath + "/" + deletefile));
                }
                
            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }

        public string getDeleteFile()
        {
            string theFile = "";
            if (Request.QueryString["deletefile"] != null)
            {
                theFile = Request.QueryString["deletefile"].ToString();
            }
            return theFile;
        }

        public void Grid_Change(Object sender, DataGridPageChangedEventArgs e)
        {           
            FileGrid.CurrentPageIndex = e.NewPageIndex;
            FileGrid.DataSource = createDataSource(DataFilePath);
            FileGrid.DataBind();
        }

        public DataView createDataSource(string datapath)
        {
            DataTable dtFiles = new DataTable();
            DataColumn dcFileName = new DataColumn("FileName", typeof(string));
            DataColumn dcSize = new DataColumn("Size", typeof(string));
            DataColumn dcDate = new DataColumn("Date", typeof(string));
            DataRow drFiles;
            dtFiles.Columns.Add(dcFileName);
            dtFiles.Columns.Add(dcSize);
            dtFiles.Columns.Add(dcDate);

            System.IO.FileInfo file = null;
            string[] files = null;
            string filename = "";
            files = System.IO.Directory.GetFiles(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + datapath), "*.*");

            for (int i = 0; i <= files.Length - 1; i++)
            {
                file = new System.IO.FileInfo(files[i]);
                filename = file.Name.ToString();
                if (filename.ToLower().EndsWith(".xml") ||
                    filename.ToLower().EndsWith(".txt") ||
                    filename.ToLower().EndsWith(".csv") ||
                    filename.ToLower().EndsWith(".xlsx") ||
                    filename.ToLower().EndsWith(".xls"))
                {
                    drFiles = dtFiles.NewRow();
                    drFiles[0] = file.Name;
                    drFiles[1] = (file.Length / 1000) + "kb";
                    drFiles[2] = file.LastWriteTime.ToShortDateString();
                    dtFiles.Rows.Add(drFiles);
                }
            }
            DataView dvFiles = new DataView(dtFiles);
            return dvFiles;
        }

        public void loadFilesTable(string datapath)
        {
            DataView files = createDataSource(datapath);
            FileGrid.DataSource = files;
            FileGrid.DataBind();
            if (Page.IsPostBack == false)
            {
                ImportFile.DataSource = files;
                ImportFile.DataBind();
                ImportFile.Items.Insert(0, new ListItem("- Select File-", ""));
            }
        }

        private void loadDD(int CustomerID)
        {
            
            int selectedFolderID = 0;
            int selectedGroupID = 0;
            if (!IsPostBack)
            {
                //List<ECN_Framework_Entities.Communicator.Folder> folderList = ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(Master.UserSession.CurrentUser.CustomerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.GRP.ToString(), Master.UserSession.CurrentUser);
                //drpFolder.DataSource = folderList;
                //drpFolder.DataBind();
                //drpFolder.Items.Insert(0, new ListItem("root", "0"));

                int groupID = getGroupID();
                if (groupID > 0)
                {
                    

                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(groupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    selectedGroupID = group.GroupID;
                    selectedFolderID = group.FolderID.Value;
                   
                }
                //LoadGroupsDR(selectedFolderID, selectedGroupID);
            }
            else
            {
                //selectedFolderID = Convert.ToInt32(drpFolder.SelectedValue.ToString());
                //selectedGroupID = Convert.ToInt32(GroupID.SelectedValue.ToString());
            }

            

            ECN_Framework_Entities.Communicator.Group groupDD = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(selectedGroupID, Master.UserSession.CurrentUser);
            if (groupDD != null)
            {
                if (groupDD.MasterSupression == null || groupDD.MasterSupression.Value == 0)
                {
                    SubscribeTypeCode.Items.Clear();
                    SubscribeTypeCode.Items.Add(new ListItem("Subscribes", "S"));
                    SubscribeTypeCode.Items.Add(new ListItem("UnSubscribes", "U"));
                }
                else
                {
                    SubscribeTypeCode.Items.Clear();
                    SubscribeTypeCode.Items.Add(new ListItem("Manual Upload", "M"));
                }
            }
            List<ECN_Framework_Entities.Accounts.Code> codeList= ECN_Framework_BusinessLayer.Accounts.Code.GetAll();
            //List<ECN_Framework_Entities.Accounts.Code> codeListSubscribeType = (from src in codeList
            //                                                                    where src.CodeType == "SubscribeType" && !src.CodeValue.Equals("M") && !src.CodeValue.Equals("B")
            //                                                                    select src).ToList();
            //SubscribeTypeCode.DataSource = codeListSubscribeType;
            //SubscribeTypeCode.DataBind();

            List<ECN_Framework_Entities.Accounts.Code> codeListFormatType = (from src in codeList
                                                                             where src.CodeType == "FormatType"
                                                                             select src).ToList();
            FormatTypeCode.DataSource = codeListFormatType;
            FormatTypeCode.DataBind();

            CSVTXTPanel.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
            CSVTXTPanel.BorderColor = System.Drawing.Color.Black;
        }

        //private void LoadGroupsDR(int FolderID, int selectedGroupID)
        //{
        //    DataTable groupView = ECN_Framework_BusinessLayer.Communicator.Group.GetSubscribers(FolderID, Master.UserSession.CurrentUser);
        //    ECN_Framework_Entities.Communicator.Group masterSupGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(Master.UserSession.CurrentCustomer.CustomerID, Master.UserSession.CurrentUser);
        //    if (FolderID == 0)
        //    {
        //        DataRow drMS = groupView.NewRow();
        //        drMS["GroupID"] = masterSupGroup.GroupID.ToString();
        //        drMS["GroupName"] = masterSupGroup.GroupName;
        //        drMS["IsSeedList"] = masterSupGroup.IsSeedList;
        //        drMS["FolderName"] = "";
        //        drMS["Archived"] = "false";
        //        groupView.Rows.Add(drMS);
        //    }
        //    groupView.DefaultView.Sort = "GroupName";
        //    groupView = groupView.DefaultView.ToTable();
        //    GroupID.DataSource = groupView;
        //    GroupID.DataTextField = "GroupName";
        //    GroupID.DataValueField = "GroupID";
        //    GroupID.DataBind();

        //    GroupID.ClearSelection();
        //    if (selectedGroupID > 0)
        //    {
        //        GroupID.Items.FindByValue(selectedGroupID.ToString()).Selected = true;
        //    }
        //    else
        //    {
        //        GroupID.Items.Insert(0, new ListItem() { Text = "Please select a group", Value = "0", Selected = true });
        //    }
            

        //}

        //public void drpFolder_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    LoadGroupsDR(Convert.ToInt32(drpFolder.SelectedValue), 0);
        //}

        public void ImportFile_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            errorlabel.Visible = false;

            if (ImportFile.SelectedIndex > -1)
            {
                string fileExtn = ECN_Framework_Common.Functions.StringFunctions.Right(ImportFile.SelectedItem.Value, (ImportFile.SelectedItem.Value.Length - (ImportFile.SelectedItem.Value.IndexOf(".") + 1))).ToUpper();

                switch (fileExtn)
                {
                    case "TXT":
                        CSVTXTPanel.Visible = true;
                        CSVTXTPanel.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
                        CSVTXTPanel.BorderColor = System.Drawing.Color.Black;
                        phTXTDelimiter.Visible = true;
                        ExcelPanel.Visible = false;
                        break;
                    case "CSV":
                        CSVTXTPanel.Visible = true;
                        CSVTXTPanel.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
                        CSVTXTPanel.BorderColor = System.Drawing.Color.Black;

                        phTXTDelimiter.Visible = false;
                        ExcelPanel.Visible = false;
                        break;
                    case "XLS":
                        CSVTXTPanel.Visible = false;
                        ExcelPanel.Visible = true;
                        ExcelPanel.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
                        ExcelPanel.BorderColor = System.Drawing.Color.Black;
                        phTXTDelimiter.Visible = false;
                        break;
                    case "XLSX":
                        CSVTXTPanel.Visible = false;
                        ExcelPanel.Visible = true;
                        ExcelPanel.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1);
                        ExcelPanel.BorderColor = System.Drawing.Color.Black;
                        phTXTDelimiter.Visible = false;
                        break;
                    case "XML":
                        CSVTXTPanel.Visible = false;
                        ExcelPanel.Visible = false;
                        phTXTDelimiter.Visible = false;
                        break;
                    default:
                        phTXTDelimiter.Visible = false;
                        CSVTXTPanel.Visible = false;
                        ExcelPanel.Visible = false;
                        break;
                }
            }
            else
            {
                phTXTDelimiter.Visible = false;
                CSVTXTPanel.Visible = false;
                ExcelPanel.Visible = false;
            }
        }

        public void deleteFile(string thefile)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(thefile);
            file.Delete();
            Response.Redirect(Request.Url.LocalPath);
        }

        public void ImportIt(object sender, System.EventArgs e)
        {

            string fileExtn = ECN_Framework_Common.Functions.StringFunctions.Right(ImportFile.SelectedItem.Value, (ImportFile.SelectedItem.Value.Length - (ImportFile.SelectedItem.Value.IndexOf(".") + 1))).ToLower();
            bool errorOccured = false;
            string redirectURL = "importDatafromFile.aspx?";

            if (fileExtn == string.Empty)
            {
                errorlabel.Visible = true;
                errorlabel.Text = "Error: Please select a File";
                return;
            }

            try
            {
                switch (fileExtn.ToUpper())
                {
                    case "XML":
                        errorlabel.Visible = false;
                        if (!(fileExtn == "xml"))
                        {
                            errorlabel.Visible = true;
                            errorlabel.Text = "Error: This FileType (XML) doesnot match the file selected";
                            errorOccured = true;
                        }
                        break;
                    case "TXT":
                        errorlabel.Visible = false;
                        if (!(fileExtn == "txt"))
                        {
                            CSVTXTPanel.Visible = true;
                            errorlabel.Visible = true;
                            errorlabel.Text = "Error: This FileType (txt) doesnot match the file selected";
                            errorOccured = true;
                        }
                        break;
                    case "CSV":
                        errorlabel.Visible = false;
                        if (!(fileExtn == "csv"))
                        {
                            CSVTXTPanel.Visible = true;
                            errorlabel.Visible = true;
                            errorlabel.Text = "Error: This FileType (csv) doesnot match the file selected";
                            errorOccured = true;
                        }
                        break;
                    case "XLS":
                        errorlabel.Visible = false;
                        if (!(fileExtn == "xls"))
                        {
                            ExcelPanel.Visible = true;
                            errorlabel.Visible = true;
                            errorlabel.Text = "Error: This FileType (excel) doesnot match the file selected";
                            errorOccured = true;
                        }
                        break;
                    case "XLSX":
                        errorlabel.Visible = false;
                        if (!(fileExtn == "xlsx"))
                        {
                            ExcelPanel.Visible = true;
                            errorlabel.Visible = true;
                            errorlabel.Text = "Error: This FileType (excel) doesnot match the file selected";
                            errorOccured = true;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                string devnull = ex.ToString();
                errorlabel.Visible = true;
                errorlabel.Text = "Error: Please select a File";
            }

            if (errorOccured == false)
            {

                string thefile = ImportFile.SelectedItem.Value;
                string ftc = FormatTypeCode.SelectedItem.Value;

                string gid = "";
                if (Convert.ToInt32(hfSelectGroupID.Value) > 0)
                {
                    gid = hfSelectGroupID.Value.ToString();
                }
                else
                {
                    throwECNException("Please select a group");
                    return;
                }

                string stc = "";
                if (SubscribeTypeCode.SelectedValue.ToString().Length > 0)
                {
                    stc = SubscribeTypeCode.SelectedValue.ToString();
                }
                else
                {
                    throwECNException("Please select a Subscribe type code");
                    return;
                }
                
                string dupes = HandleDuplicates.SelectedItem.Value;			
                string ft = fileExtn.ToUpper();		
                string dl = drpDelimiter.SelectedItem.Value;				
                string ln_nbr = "0";
                if (ft == "XLS" || ft == "XLSX")
                {
                    ft = "X";
                    ln_nbr = xlslinenumber.Text.ToString();
                }
                else if (ft == "CSV")
                {
                    ft = "C";
                    ln_nbr = linenumber.Text.ToString();
                }
                else if (ft == "TXT")
                {
                    ft = "O";
                    ln_nbr = linenumber.Text.ToString();
                }

                string shtnm = sheetName.Text.ToString();


                redirectURL += "file=" + thefile + "&ftc=" + ftc + "&stc=" + stc + "&gid=" + gid + "&dupes=" + dupes + "&ft=" + ft + "&line=" + ln_nbr + "&sheet=" + shtnm + "&dl=" + dl;
                Response.Redirect(redirectURL);
            }
            else
            {
                errorlabel.Visible = true;
                if (errorlabel.Text.Length == 0)
                {
                    errorlabel.Text = "Error: Unexpected Error Occured. Please <a href='mailto:support@teckman.com'>email</a> this to the admin";
                }
            }
        }

        protected void GroupID_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            loadDD(Master.UserSession.CurrentCustomer.CustomerID);
        }

        protected void imgSelectGroup_Click(object sender, ImageClickEventArgs e)
        {
            hfGroupSelectionMode.Value = "SelectGroup";
            ctrlgroupsLookup1.LoadControl();
            ctrlgroupsLookup1.Visible = true;
        }

        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                string source = sender.ToString();
                if (source.Equals("GroupSelected"))
                {
                    int groupID = ctrlgroupsLookup1.selectedGroupID;
                    ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(groupID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                    if (hfGroupSelectionMode.Value.Equals("SelectGroup"))
                    {
                        lblSelectGroupName.Text = group.GroupName;
                        hfSelectGroupID.Value = groupID.ToString();
                    }
                    else
                    {
                        //noop
                    }
                    ctrlgroupsLookup1.Visible = false;
                }
                
            }
            catch
            {
                //noop
            }
            return true;
        }

        private void GroupsLookupPopupHide()
        {
            ctrlgroupsLookup1.Visible = false;
        }

        protected void GroupChoice_CheckedChanged(object sender, EventArgs e)
        {
            imgSelectGroup.Enabled = rbGroupChoice1.Checked;
            lblSelectGroupName.Text = "-No Group Selected-";
            hfSelectGroupID.Value = "0";
            SubscribeTypeCode.Items.Clear();
            SubscribeTypeCode.Items.Add(new ListItem("Subscribes", "S"));
            SubscribeTypeCode.Items.Add(new ListItem("UnSubscribes", "U"));

            if (rbGroupChoice2.Checked)
            {
                ECN_Framework_Entities.Communicator.Group group = ECN_Framework_BusinessLayer.Communicator.Group.GetMasterSuppressionGroup(Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID), Master.UserSession.CurrentUser);
                hfSelectGroupID.Value = Convert.ToString(group.GroupID);
                SubscribeTypeCode.Items.Clear();
                SubscribeTypeCode.Items.Add(new ListItem("Manual Upload", "M"));
            }
        }
    }
}
