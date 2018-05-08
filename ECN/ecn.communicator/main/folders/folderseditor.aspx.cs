using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;
using System.IO;


namespace ecn.communicator.foldermanager
{
    public partial class folderseditor : ECN_Framework.WebPageHelper
    {
        private const string Style = "style";
        private const string OnClick = "onclick";
        private const string DeleteStyle = "cursor:hand;padding:0;margin:0;";
        private const string FolderId = "Folder ID: ";

        #region Getters, Setters, declares
        protected System.Web.UI.WebControls.Label FolderError;
        protected System.Web.UI.WebControls.Panel GRP_CNTFolderButtonsPanel;
        protected System.Web.UI.WebControls.Panel IMGFolderButtonsPanel;

        DataTable FoldersDT = new DataTable();

        public static bool _grpPrevilageDelete = false;
        public static bool grpPrevilageDelete
        {
            get { return _grpPrevilageDelete; }
            set { _grpPrevilageDelete = value; }
        }

        public static bool _grpPrevilageEdit = false;
        public static bool grpPrevilageEdit
        {
            get { return _grpPrevilageEdit; }
            set { _grpPrevilageEdit = value; }
        }


        public static bool _cntPrevilageDelete = false;
        public static bool cntPrevilageDelete
        {
            get { return _cntPrevilageDelete; }
            set { _cntPrevilageDelete = value; }
        }

        public static bool _cntPrevilageEdit = false;
        public static bool cntPrevilageEdit
        {
            get { return _cntPrevilageEdit; }
            set { _cntPrevilageEdit = value; }
        }

        public static bool _imgPrevilageDelete = false;
        public static bool imgPrevilageDelete
        {
            get { return _imgPrevilageDelete; }
            set { _imgPrevilageDelete = value; }
        }

        public static bool _imgPrevilageEdit = false;
        public static bool imgPrevilageEdit
        {
            get { return _imgPrevilageEdit; }
            set { _imgPrevilageEdit = value; }
        }

        private string getFolderType()
        {
            if (Request.QueryString["fType"] != null)
                return Request.QueryString["fType"].ToString();
            else
                return string.Empty;
        }

        int customerID = 0;
        int baseChannelID = 0;
        int userID = 0;
        #endregion

        public folderseditor()
        {
            Page.Init += new System.EventHandler(Page_Init);
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

        private void setECNErrorMain(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phErrorMain.Visible = true;
            lblErrorMessageMain.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessageMain.Text = lblErrorMessageMain.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            phError.Visible = false;
            phErrorMain.Visible = false;
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.FOLDERS;
            Master.SubMenu = "";
            Master.Heading = "Folders > Manage Folders";
            Master.HelpContent = "<strong>Add / Edit Folders</strong><p>&#13;&#10;  To create a new Folder to store Contents &amp; Campaigns, enter the <i>Folder name</i> and <i>Folder Description</i> that you prefer, and &#13;&#10;  hit the <em>Create </em>button to create a new folder. </p>";
            Master.HelpTitle = "Folders Manager";

            //_grpPrevilage = KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "grouppriv");
            //_cntPrevilage = KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "contentpriv");
            KMPlatform.Entity.User currentUser = Master.UserSession.CurrentUser;

            _grpPrevilageEdit = KMPlatform.BusinessLogic.User.HasAccess(currentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupFolder, KMPlatform.Enums.Access.Edit);
            _cntPrevilageEdit = KMPlatform.BusinessLogic.User.HasAccess(currentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ContentFolder, KMPlatform.Enums.Access.Edit);
            _imgPrevilageEdit = KMPlatform.BusinessLogic.User.HasAccess(currentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ImagesFolder, KMPlatform.Enums.Access.Edit);

            _grpPrevilageDelete = KMPlatform.BusinessLogic.User.HasAccess(currentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.GroupFolder, KMPlatform.Enums.Access.Delete);
            _cntPrevilageDelete = KMPlatform.BusinessLogic.User.HasAccess(currentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ContentFolder, KMPlatform.Enums.Access.Delete);
            _imgPrevilageDelete = KMPlatform.BusinessLogic.User.HasAccess(currentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ImagesFolder, KMPlatform.Enums.Access.Delete);




            customerID = Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID);
            baseChannelID = Convert.ToInt32(Master.UserSession.CurrentBaseChannel.BaseChannelID);
            userID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);

            if (!(Page.IsPostBack))
            {
                LoadFolderTypes();
                if (getFolderType().Length > 0)
                {
                    FolderType.SelectedValue = getFolderType();
                }
                if (FolderType.SelectedValue.Equals("IMG"))
                {
                    FoldersList.DataKeyField = "ParentID";
                    LoadImageFolderData();
                }
                else
                {
                    FoldersList.DataKeyField = "FolderID";
                    LoadFoldersDT();
                    BuildFoldersGrid();
                }
            }

            if (FolderType.SelectedValue.ToUpper().Equals("GRP"))
            {
                FolderListHeading.Text = "Group Folders";
            }
            else if (FolderType.SelectedValue.ToUpper().Equals("CNT"))
            {
                FolderListHeading.Text = "Content / Message Folders";
            }
            else if (FolderType.SelectedValue.ToUpper().Equals("IMG"))
            {
                FolderListHeading.Text = "Image Folders";
            }

        }

        private void LoadFolderTypes()
        {

            FolderType.Items.Insert(0, new ListItem("&nbsp;&nbsp;<img src=\"/ecn.images/images/manageContentFolders.gif\" />Content / Messages", "CNT"));

            FolderType.Items.Insert(0, (new ListItem("&nbsp;&nbsp;<img src=\"/ecn.images/images/manageGroupFolders.gif\" />Group", "GRP")));

            FolderType.Items[0].Selected = true;
        }

        #region Folder List Data Load
        private void LoadFoldersDT()
        {
            FoldersDT = ECN_Framework_BusinessLayer.Communicator.Folder.GetFolderTree(Master.UserSession.CurrentUser.CustomerID, Master.UserSession.CurrentUser.UserID, FolderType.SelectedItem.Value.ToString(), Master.UserSession.CurrentUser);
        }

        private void BuildFoldersGrid()
        {
            FoldersList.DataSource = FoldersDT.DefaultView;
            FoldersList.DataBind();
        }

        private void LoadImageFolderData()
        {
            DataTable dtFolders = new DataTable();
            DataColumn dcFolderID = new DataColumn("FolderID", typeof(string));
            DataColumn dcFolderName = new DataColumn("FolderName", typeof(string));
            DataColumn dcFolderDescription = new DataColumn("FolderDescription", typeof(string));
            DataColumn dcFolderType = new DataColumn("FolderType", typeof(string));
            DataColumn dcParentID = new DataColumn("ParentID", typeof(string));
            DataColumn dcDateCreated = new DataColumn("DateCreated", typeof(string));
            DataColumn dcSystemFolder = new DataColumn("SystemFolder", typeof(string));
            DataColumn dcFiles = new DataColumn("Items", typeof(string));

            DataRow drFolders;
            dtFolders.Columns.Add(dcFolderID);
            dtFolders.Columns.Add(dcFolderName);
            dtFolders.Columns.Add(dcFolderDescription);
            dtFolders.Columns.Add(dcFolderType);
            dtFolders.Columns.Add(dcParentID);
            dtFolders.Columns.Add(dcDateCreated);
            dtFolders.Columns.Add(dcSystemFolder);
            dtFolders.Columns.Add(dcFiles);

            string imageDirectory = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + customerID + "/images";

            if (!System.IO.Directory.Exists(Server.MapPath(imageDirectory)))
                System.IO.Directory.CreateDirectory(Server.MapPath(imageDirectory));


            System.IO.DirectoryInfo dir = null;
            string[] dirs = null;
            dirs = System.IO.Directory.GetDirectories(Server.MapPath(imageDirectory));
            dir = new System.IO.DirectoryInfo(Server.MapPath(imageDirectory));
            string[] rootFolderFiles = System.IO.Directory.GetFiles(Server.MapPath(imageDirectory), "*.*");

            drFolders = dtFolders.NewRow();
            drFolders[0] = "0";
            drFolders[1] = "&nbsp;<Font color=#FF0000><b>ROOT FOLDER</b></font>";
            drFolders[2] = "<b>Images Root Folder</b><br><br>Number of Image Folders: " + System.IO.Directory.GetDirectories(Server.MapPath(imageDirectory)).Length + "<br>Number of Image Files: " + System.IO.Directory.GetFiles(Server.MapPath(imageDirectory), "*.*").Length;
            drFolders[3] = "IMGROOT";
            drFolders[4] = "";
            drFolders[5] = "";
            drFolders[6] = "Y";
            drFolders[7] = System.IO.Directory.GetFiles(Server.MapPath(imageDirectory), "*.*").Length;
            dtFolders.Rows.Add(drFolders);

            for (int i = 0; i <= dirs.Length - 1; i++)
            {
                dir = new System.IO.DirectoryInfo(dirs[i]);
                dtFolders = GetRecursiveImageFolders(dtFolders, dir, dir.Name, "");
            }

            DataView dvFolders = new DataView(dtFolders);
            FoldersList.DataSource = dvFolders;
            FoldersList.DataBind();
        }
        #endregion

        private DataTable GetRecursiveImageFolders(DataTable dtFolders, DirectoryInfo dir, string currentdirectory, string spacer)
        {
            DataRow drFolders;
            drFolders = dtFolders.NewRow();

            string imageDirectory = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + customerID + "/images";

            string dirName = dir.Name.ToString();
            drFolders = dtFolders.NewRow();
            drFolders[0] = "0";
            drFolders[1] = spacer + "<img src='/ecn.images/images/L.gif'>&nbsp;<img src='/ecn.images/images/Lg_folder_Yel.gif'>&nbsp;&nbsp;" + dirName;
            drFolders[2] = "<b>" + dirName + "</b><br>Number of Images: " + dir.GetFiles().Length;
            drFolders[3] = "IMG";
            drFolders[4] = currentdirectory;
            drFolders[5] = dir.CreationTime.ToString("MM/dd/yyyy");
            drFolders[6] = "N";
            drFolders[7] = System.IO.Directory.GetFiles(Server.MapPath(imageDirectory + "/" + currentdirectory)).Length;
            dtFolders.Rows.Add(drFolders);

            string[] dirs = null;
            dirs = System.IO.Directory.GetDirectories(Server.MapPath(imageDirectory + "/" + currentdirectory));

            for (int i = 0; i <= dirs.Length - 1; i++)
            {
                DirectoryInfo subdir = new System.IO.DirectoryInfo(dirs[i]);
                dtFolders = GetRecursiveImageFolders(dtFolders, subdir, currentdirectory + "/" + subdir.Name, spacer + "&nbsp;&nbsp;&nbsp;&nbsp;");
            }

            return dtFolders;
        }

        #region Delete Folder Check Handlers
        public void DeleteFolderChecks(string type, int folderID, int parentID, int items)
        {
            DeleteFolder(folderID);
        }

        public void DeleteImgFolder(string folderName)
        {
            System.IO.DirectoryInfo dir = null;
            string path = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + customerID + "/images/" + folderName + "/";
            try
            {
                dir = new System.IO.DirectoryInfo(Server.MapPath(path));
                if (dir.GetFiles().Length > 0)
                {
                    Response.Write("<script>alert('Images exist in this Folder. Delete is not allowed !!'); </script>");
                }
                else
                {
                    dir.Delete(true);
                }
            }
            catch
            {
                Response.Write("<script>alert('Error occured during Folder Deletion'); </script>");
            }
        }
        #endregion

        #region Folder Event Handlers (Add/ Delete)
        public void DeleteFolder(int folderID)
        {
            try
            {
                ECN_Framework_BusinessLayer.Communicator.Folder.Delete(folderID, Master.UserSession.CurrentUser);
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNErrorMain(ex);
            }
        }
        #endregion

        #region Folders DataList Events
        private void FoldersList_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRow[] FolderRows = FoldersDT.Select();
                string folderDesc = "";

                #region Delete button pre-validations
                LinkButton deleteBtn = e.Item.FindControl("FolderDelete") as LinkButton;
                Label fldrItemsLbl = e.Item.FindControl("FolderItemsLbl") as Label;
                int items = 0;
                try { items = Convert.ToInt32(fldrItemsLbl.Text.ToString()); }
                catch { items = 0; }

                if (FolderType.SelectedValue.Equals("GRP"))
                {
                    SetDeleteButtonAttributes(deleteBtn, items, e.Item.ItemIndex, "Groups exist", FolderId);
                }
                else if (FolderType.SelectedValue.Equals("CNT"))
                {
                    SetDeleteButtonAttributes(deleteBtn, items, e.Item.ItemIndex, "Content exists", FolderId);
                }
                else if (FolderType.SelectedValue.Equals("IMG"))
                {
                    string imageDirectory = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + customerID + "/images";
                    string currFolder = FoldersList.DataKeys[e.Item.ItemIndex].ToString();
                    if (!(currFolder.Equals("ROOT FOLDER", StringComparison.OrdinalIgnoreCase)))
                    {
                        int filesCount = System.IO.Directory.GetFiles(Server.MapPath(imageDirectory + "/" + currFolder)).Length;
                        SetDeleteButtonAttributes(deleteBtn, filesCount, e.Item.ItemIndex, "Image files exist", "Folder: ");
                    }
                }
                #endregion

                #region Description link populate
                Label descBtn = e.Item.FindControl("FolderDescLinkBtn") as Label;
                TextBox FolderDescTxtBx = e.Item.FindControl("HDNFolderDescTxtBx") as TextBox;
                string mouseAltSt = "<div style=\\'background-color:#FFFFFF;BORDER-TOP: #B6BCC6 1px solid;BORDER-LEFT: #B6BCC6 1px solid;BORDER-RIGHT: #B6BCC6 1px solid;	BORDER-BOTTOM: #B6BCC6 1px solid;position:absolute;\\'><table border=0 width=300 height=100 cellpadding=3><tr><TD valign=top style=\\'font-family:Arial Verdana; font-size:10px\\'>";
                string mouseAltEnd = "</TD></TR></TABLE></div>";
                if (FolderDescTxtBx.Text.ToString().Length > 0)
                {
                    folderDesc = FolderDescTxtBx.Text.ToString();
                }
                else
                {
                    folderDesc = "No Folder description available.";
                }
                folderDesc = folderDesc.Replace("'", "");
                descBtn.Attributes.Add("onmouseover", "return overlib('" + (mouseAltSt + folderDesc + mouseAltEnd) + "', FULLHTML,VAUTO,HAUTO,RIGHT,WIDTH,350,HEIGHT,130);");
                descBtn.Attributes.Add("onmouseout", "return nd();");
                #endregion
                return;
            }

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                if (FolderType.SelectedValue.Equals("IMG"))
                {
                    TextBox Edit_FolderName = e.Item.FindControl("Edit_FolderName") as TextBox;
                    string folderNm = FoldersList.DataKeys[e.Item.ItemIndex].ToString();
                    folderNm = folderNm.Replace("&nbsp;", "");
                    folderNm = folderNm.Replace("<sub><img src='/ecn.images/images/L.gif'></sub>", "");
                    folderNm = folderNm.Replace("<sub><img src='/ecn.images/images/Lg_folder_Yel.gif'></sub>", "");
                    folderNm = folderNm.Replace("<sub><img src='/ecn.images/images/Sm_folder_Yel.gif'></sub>", "");
                    Edit_FolderName.Text = folderNm;
                }
                else
                {
                    TextBox Edit_FolderName = e.Item.FindControl("Edit_FolderName") as TextBox;
                    TextBox Edit_FolderDesc = e.Item.FindControl("Edit_FolderDesc") as TextBox;
                    DataRow[] FolderRows = FoldersDT.Select("FolderID = " + (int)FoldersList.DataKeys[(int)e.Item.ItemIndex]);
                    string folderNm = FolderRows[0]["FolderName"].ToString();
                    folderNm = folderNm.Replace("&nbsp;", "");
                    folderNm = folderNm.Replace("<sub><img src='/ecn.images/images/L.gif'></sub>", "");
                    folderNm = folderNm.Replace("<sub><img src='/ecn.images/images/Lg_folder_Yel.gif'></sub>", "");
                    folderNm = folderNm.Replace("<sub><img src='/ecn.images/images/Sm_folder_Yel.gif'></sub>", "");
                    folderNm = folderNm.Replace("<sub><img src='/ecn.communicator/images/ecn-icon-folder.png'></sub>", "");

                    Edit_FolderName.Text = folderNm;
                    Edit_FolderDesc.Text = FolderRows[0]["FolderDescription"].ToString();
                }
            }
        }

        private void SetDeleteButtonAttributes(LinkButton deleteBtn, int items, int itemIndex, string type, string confirmPrefix)
        {
            if (items > 0)
            {
                deleteBtn.Enabled = false;
                deleteBtn.Attributes.Add(Style, DeleteStyle);
                deleteBtn.Attributes.Add(OnClick, $"alert('{type} in this Folder. Delete is not allowed !!');");
            }
            else
            {
                deleteBtn.Attributes.Add(OnClick, $"return confirm('{confirmPrefix}{FoldersList.DataKeys[itemIndex]} - Are you sure that you want to delete this Folder ?')");
            }
        }

        protected bool IsValidImageFolderName(string folderName)
        {
            //if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidObjectName(folderName))
            //    return false;

            string[] illegalChars = new string[] { "<", ">", ":", "\"", "/", "\\", "|", "?", "*" };
            //{ "*", ".", " /", "?","%","$","^","'","(",")","@","!","~","#","<", ">","[", "]", ":", ";", "|", "=", "\"" };

            foreach (string c in illegalChars)
            {
                if (folderName.Contains(c))
                {
                    return false;
                }
            }
            return true;
        }


        protected void btnAddFolder_Save(object sender, EventArgs e)
        {
            string ft = FolderType.SelectedValue.ToUpper();
            string fn = txtFolderNameSave.Text;
            string fd = txtFolderDescriptionSave.Text;
            string errorMessage = "";

            try
            {
                if (ft.Equals("IMG"))
                {
                    if (IsValidImageFolderName(fn))
                    {
                        fn = fn.Replace(" ", "_").ToString();
                        System.IO.DirectoryInfo dir = null;

                        string path = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentUser.CustomerID + "/images/" + lblCurrentDirectory.Text + "/" + fn;
                        if (!System.IO.Directory.Exists(Server.MapPath(path)))
                        {
                            dir = new System.IO.DirectoryInfo(Server.MapPath(path));
                            dir.Create();

                            txtFolderNameSave.Text = "";
                            txtFolderDescriptionSave.Text = "";
                            LoadImageFolderData();
                            modalPopupAddFolder.Hide();
                        }
                        else
                        {
                            errorMessage = "Folder Name already exists";

                        }

                    }
                    else
                    {
                        errorMessage = "FolderName has invalid characters";
                    }

                    if (errorMessage.Length > 0)
                    {
                        throwECNException(errorMessage);
                    }

                }
                else
                {
                    ECN_Framework_Entities.Communicator.Folder folder = new ECN_Framework_Entities.Communicator.Folder();
                    folder.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                    folder.FolderName = fn;
                    folder.FolderType = ft;
                    folder.FolderDescription = fd;
                    folder.IsSystem = false;
                    if (lblFolderID.Text.Length > 0)
                    folder.ParentID = Convert.ToInt32(lblFolderID.Text);
                    folder.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                    ECN_Framework_BusinessLayer.Communicator.Folder.Save(folder, Master.UserSession.CurrentUser);
                    txtFolderNameSave.Text = "";
                    txtFolderDescriptionSave.Text = "";
                    LoadFoldersDT();
                    BuildFoldersGrid();
                    modalPopupAddFolder.Hide();
                }
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }

        }

        protected void btnAddFolder_Close(object sender, EventArgs e)
        {
            txtFolderDescriptionSave.Text = "";
            txtFolderNameSave.Text = "";
            modalPopupAddFolder.Hide();
        }

        private void FoldersList_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            int folderID = 0;
            string[] Id = e.CommandArgument.ToString().Split('|');

            switch (e.CommandName)
            {
                case "Add":
                    lblFolderID.Text = Id[0];
                    lblCurrentDirectory.Text = Id[1];
                    modalPopupAddFolder.Show();
                    break;
                case "Edit":
                    if (!(FolderType.SelectedValue.Equals("IMG")))
                    {
                        FoldersList.EditItemIndex = e.Item.ItemIndex;
                    }
                    break;
                case "Update":
                    try
                    {
                        TextBox Edit_FolderName = e.Item.FindControl("Edit_FolderName") as TextBox;
                        TextBox Edit_FolderDesc = e.Item.FindControl("Edit_FolderDesc") as TextBox;
                        string fldrName = Edit_FolderName.Text.ToString().Replace("'", "");
                        string fldrDesc = Edit_FolderDesc.Text.ToString().Replace("'", "");
                        folderID = (int)FoldersList.DataKeys[(int)e.Item.ItemIndex];
                        ECN_Framework_Entities.Communicator.Folder folder = ECN_Framework_BusinessLayer.Communicator.Folder.GetByFolderID(folderID, Master.UserSession.CurrentUser);
                        folder.FolderName = fldrName;
                        folder.FolderDescription = fldrDesc;
                        folder.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
                        ECN_Framework_BusinessLayer.Communicator.Folder.Save(folder, Master.UserSession.CurrentUser);
                        FoldersList.EditItemIndex = -1;
                    }
                    catch (ECN_Framework_Common.Objects.ECNException ex)
                    {
                        setECNErrorMain(ex);
                    }
                    break;
                case "Cancel":
                    FoldersList.EditItemIndex = -1;
                    break;
                case "Delete":
                    if (FolderType.SelectedValue.Equals("IMG"))
                    {
                        string folderName = FoldersList.DataKeys[e.Item.ItemIndex].ToString();
                        DeleteImgFolder(folderName);
                    }
                    else
                    {
                        folderID = (int)FoldersList.DataKeys[(int)e.Item.ItemIndex];
                        TextBox HDNSystemFolder = e.Item.FindControl("HDNSystemFolder") as TextBox;
                        TextBox HDNFolderType = e.Item.FindControl("HDNFolderType") as TextBox;
                        TextBox HDNParentID = e.Item.FindControl("HDNParentID") as TextBox;
                        Label fldrItemsLbl = e.Item.FindControl("FolderItemsLbl") as Label;

                        DeleteFolderChecks(HDNFolderType.Text.Trim().ToString(), folderID, Convert.ToInt32(HDNParentID.Text.Trim().ToString()), Convert.ToInt32(fldrItemsLbl.Text.Trim().ToString()));
                    }
                    break;
            }
            if (FolderType.SelectedValue.Equals("IMG"))
            {
                FoldersList.DataKeyField = "ParentID";
                LoadImageFolderData();
            }
            else
            {
                LoadFoldersDT();
                BuildFoldersGrid();
            }
        }
        #endregion

        #region Web Form Designer generated code

        protected void Page_Init(object sender, EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.FoldersList.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.FoldersList_ItemCommand);
            this.FoldersList.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.FoldersList_ItemDataBound);
            this.FolderType.SelectedIndexChanged += new EventHandler(FolderType_SelectedIndexChanged);
        }
        #endregion

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Folder, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            throw new ECNException(errorList, Enums.ExceptionLayer.WebSite);
        }

        private void FolderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FoldersList.EditItemIndex = -1;
            if (FolderType.SelectedValue.Equals("IMG"))
            {
                FoldersList.DataKeyField = "ParentID";
                LoadImageFolderData();
            }
            else
            {
                FoldersList.DataKeyField = "FolderID";
                LoadFoldersDT();
                BuildFoldersGrid();
            }
        }

        protected void FoldersList_ItemDataBound1(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton AddSubFolderLinkBtn = (LinkButton)e.Item.FindControl("AddSubFolderLinkBtn");
                LinkButton FolderDelete = (LinkButton)e.Item.FindControl("FolderDelete");
                LinkButton FolderEdit = (LinkButton)e.Item.FindControl("FolderEdit");
                //Images
                if (FolderType.SelectedValue.Equals("IMG"))
                {
                    AddSubFolderLinkBtn.Visible = FolderEdit.Visible = _imgPrevilageEdit;
                    FolderDelete.Visible = _imgPrevilageDelete;
                }
                else if (FolderType.SelectedValue.Equals("GRP"))//groups
                {
                    AddSubFolderLinkBtn.Visible = FolderEdit.Visible = _grpPrevilageEdit;
                    FolderDelete.Visible = _grpPrevilageDelete;
                }
                else if (FolderType.SelectedValue.Equals("CNT"))//Content
                {
                    AddSubFolderLinkBtn.Visible = FolderEdit.Visible = _cntPrevilageEdit;
                    FolderDelete.Visible = _cntPrevilageDelete;
                }
            }
        }

    }
}
