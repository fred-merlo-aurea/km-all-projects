using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using ecn.communicator.classes;
using ecn.common.classes;
using ECN_Framework_Common.Objects;


namespace ecn.communicator.includes
{

    /// attachme allows for multiple files to be uploaded to your web server while using only
    /// one HttpInputFile control and a listbox.

    public partial class uploader : System.Web.UI.UserControl
    {

        public string uploadDirectory
        {
            set
            {
                uploadpath.Text = value;
            }
            get
            {
                return uploadpath.Text;
            }
        }

        string _currentParent;
        public string currentParent
        {
            set
            {
                _currentParent = value;
            }
            get
            {
                return _currentParent;
            }
        }


        ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();

        ArrayList aFiles = new ArrayList();
        public int filesUploaded = 0;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (FilesListBox.Items.Count == 0)
            {
                aFiles = new ArrayList();
            }
            else
            {
                aFiles = (ArrayList)Session["sFiles" + sc.UserID()];

                if (aFiles == null)
                    aFiles = new ArrayList();
            }

            MessageLabel.Text = "";
            System.Web.UI.Control currentParentControl = this.Parent;
            _currentParent = currentParentControl.ID;

            if (currentParent.Equals("ImageManagerForm") || currentParent.Equals("PanelUpload") || currentParent.Equals("SocialPanelUpload"))
            {
                HelpPanel.Visible = true;
                FoldersPanel.Visible = true;
            }
            else if (currentParent.Equals("gatewayUpload"))
            {
                HelpPanel.Visible = false;
                FoldersPanel.Visible = false;
            }
            else
            {
                HelpPanel.Visible = false;
                FoldersPanel.Visible = false;
            }

            if (!(Page.IsPostBack))
            {
                LoadImgFoldersDR();
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }


        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {

        }
        #endregion


        public void LoadImgFoldersDR()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DirID");
            dt.Columns.Add("DirName");

            DataRow dr = null;
            string[] dirs = null;
            System.IO.DirectoryInfo dir = null;

            if (!System.IO.Directory.Exists(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + sc.CustomerID() + "/images")))
                System.IO.Directory.CreateDirectory(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + sc.CustomerID() + "/images"));

            dirs = System.IO.Directory.GetDirectories(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + sc.CustomerID() + "/images"));

            dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "ROOT Folder";
            dt.Rows.Add(dr);

            //for (int i = 0; i <= dirs.Length - 1; i++)
            //{
            //    //Create a new DirectoryInfo object for this Dir
            //    dir = new System.IO.DirectoryInfo(dirs[i]);
            //    dirname = dir.Name.ToString();
            //    dr = dt.NewRow();
            //    dr[0] = dirname;
            //    dr[1] = " - - - " + dirname;

            //    dt.Rows.Add(dr);
            //}

            for (int i = 0; i <= dirs.Length - 1; i++)
            {
                dir = new System.IO.DirectoryInfo(dirs[i]);
                dt = GetRecursiveImageFolders(dt, dir, dir.Name);
            }

            //DataView dvFolders = new DataView(dt); ImgFoldersDR.DataSource = dvFolders;
            ImgFoldersDR.DataSource = from x in dt.AsEnumerable() select new { DirID = System.Web.HttpUtility.UrlEncode(x["DirID"].ToString()), DirName = x["DirName"] };
            ImgFoldersDR.DataBind();
        }

        private DataTable GetRecursiveImageFolders(DataTable dtFolders, System.IO.DirectoryInfo dir, string currentdirectory)
        {
            DataRow drFolders;
            drFolders = dtFolders.NewRow();

            string imageDirectory = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + sc.CustomerID() + "/images";

            string dirName = dir.Name.ToString();
            drFolders = dtFolders.NewRow();
            drFolders[0] = currentdirectory;
            drFolders[1] = " - - - " + currentdirectory;
            dtFolders.Rows.Add(drFolders);

            string[] dirs = null;
            dirs = System.IO.Directory.GetDirectories(Server.MapPath(imageDirectory + "/" + currentdirectory));

            for (int i = 0; i <= dirs.Length - 1; i++)
            {
                System.IO.DirectoryInfo subdir = new System.IO.DirectoryInfo(dirs[i]);
                dtFolders = GetRecursiveImageFolders(dtFolders, subdir, currentdirectory + "/" + subdir.Name);
            }

            return dtFolders;
        }



        /// AddFile will add the path of the client side file that is currently in the PostedFile
        /// property of the HttpInputFile control to the listbox.

        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddFile_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (Page.IsPostBack == true)
                {
                    if (!(FindFile.PostedFile.FileName == ""))
                    {


                    if (FindFile.PostedFile.FileName.ToLower().EndsWith(".jpg") || FindFile.PostedFile.FileName.ToLower().EndsWith(".gif") || FindFile.PostedFile.FileName.ToLower().EndsWith(".png") ||
                        FindFile.PostedFile.FileName.ToLower().EndsWith(".xls") || FindFile.PostedFile.FileName.ToLower().EndsWith(".xlsx") || FindFile.PostedFile.FileName.ToLower().EndsWith(".xml") ||
                        FindFile.PostedFile.FileName.ToLower().EndsWith(".txt") || FindFile.PostedFile.FileName.ToLower().EndsWith(".csv") || FindFile.PostedFile.FileName.ToLower().EndsWith(".css"))
                    {
                        aFiles.Add(FindFile);
                        FilesListBox.Items.Add(FindFile.PostedFile.FileName);
                        MessageLabel.Text = "";
                    }
                    else
                    {

                       MessageLabel.Text = "ERROR: Cannot Upload File: <br><br>Only files with following extensions are supported in ECN:<br><u>Image Files</u>: JPG, GIF, PNG<br><u>Other Files</u>: XLS, XML, TXT, CSV";

                       // const string errMsg = "ERROR: Cannot Upload File: <br><br>Only files with following extensions are supported in ECN:<br><u>Image Files</u>: JPG, GIF<br><u>Other Files</u>: XLS, XLSX, XML, TXT, CSV";
                       // throwECNException(errMsg, this.Parent.FindControl("phError") as PlaceHolder, this.Parent.FindControl("lblErrorMessage") as Label);
                    }

                    Session["sFiles" + sc.UserID()] = aFiles;

                    //if (sc.UserID() == "1568" || sc.UserID() == "2661")
                    //{
                    //    Response.Write(sc.UserID() + "<BR>");
                    //    aFiles = (ArrayList)Session["sFiles" + sc.UserID()];
                    //    foreach (System.Web.UI.HtmlControls.HtmlInputFile f in aFiles)
                    //    {
                    //        Response.Write(f.PostedFile.FileName + "<BR>");
                    //    }
                    //}
                }
            }
            }
            catch
            {
                const string errMsg = "ERROR: Cannot Upload File: <br><br>Only Files with less than 25 MB is supported in ECN";
                throwECNException(errMsg, this.Parent.FindControl("phError") as PlaceHolder, this.Parent.FindControl("lblErrorMessage") as Label);
            }
        }

        private void throwECNException(string message, PlaceHolder phError, Label lblErrorMessage)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Link, Enums.Method.Get, message);
            List<ECNError> errorList = new List<ECNError> { ecnError };
            setECNError(new ECNException(errorList, Enums.ExceptionLayer.WebSite), phError, lblErrorMessage);
        }

        private void setECNError(ECNException ecnException, PlaceHolder phError, Label lblErrorMessage)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }

        /// RemvFile will remove the currently selected file from the listbox.

        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RemvFile_Click(object sender, System.EventArgs e)
        {
            if (FilesListBox.Items.Count != 0)
            {
                if (FilesListBox.SelectedIndex > -1)
                {
                    aFiles.RemoveAt(FilesListBox.SelectedIndex);
                    FilesListBox.Items.Remove(FilesListBox.SelectedItem.Text);
                }
                else
                {
                    MessageLabel.Text = "ERROR: Remove Function needs at least 1 file to be selected from the list box.";
                }
                Session["sFiles" + sc.UserID()] = aFiles;
            }
        }

        /// Upload_ServerClick is the server side script that will upload the files to the web server
        /// by looping through the files in the listbox.

        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Upload_ServerClick(object sender, System.EventArgs e)
        {
            bool bError = false;
            string baseLocation = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + uploadDirectory);
            string redirectURL = "";

            if (currentParent.Equals("ImageManagerForm") || currentParent.Equals("PanelUpload"))
            {
                redirectURL = "filemanager.aspx";
                if (ImgFoldersDR.SelectedValue.Length > 0)
                {
                    baseLocation += @"\" + ImgFoldersDR.SelectedValue.ToString();
                    redirectURL += "?folder=/" + ImgFoldersDR.SelectedValue.ToString();
                }
            }
            else if (currentParent.Equals("SocialPanelUpload"))
            {
                redirectURL = Request.Url.PathAndQuery.ToString();
                if (ImgFoldersDR.SelectedValue.Length > 0)
                {
                    baseLocation += @"\" + ImgFoldersDR.SelectedValue.ToString();

                }
            }
            else if (currentParent.Equals("gatewayUpload"))
            {
                redirectURL = Request.Url.PathAndQuery.ToString();
                if (ImgFoldersDR.SelectedValue.Length > 0)
                {
                    baseLocation += @"\" + ImgFoldersDR.SelectedValue.ToString();

                }
            }
            else
            {
                redirectURL = Request.Url.PathAndQuery.ToString();
            }
            string status = "";

            if (false == string.IsNullOrWhiteSpace(baseLocation))
            {
                baseLocation = System.Web.HttpUtility.UrlDecode(baseLocation);
            }

            if ((FilesListBox.Items.Count == 0) && (filesUploaded == 0))
            {
                MessageLabel.Text = "ERROR: Upload Function needs at least 1 file added in the list box.";
                return;
            }
            else
            {
                foreach (System.Web.UI.HtmlControls.HtmlInputFile f in aFiles)
                {
                    string fn = string.Empty;
                    try
                    {
                        fn = System.IO.Path.GetFileNameWithoutExtension(f.PostedFile.FileName);
                        fn = ECN_Framework_Common.Functions.StringFunctions.ReplaceNonAlphaNumeric(fn, "_");
                        f.PostedFile.SaveAs(baseLocation + "\\" + fn + System.IO.Path.GetExtension(f.PostedFile.FileName));
                        filesUploaded++;
                        status += fn + "<br>";

                        try
                        {
                            DataFunctions.Execute("insert into Uploadlog (UserID,CustomerID,Path,FileName,uploaddate,PageSource) values (" + sc.UserID() + "," + sc.CustomerID() + ",'" + baseLocation + "','" + fn.Replace("'", "''") + "',getdate(),'" + Request.ServerVariables["PATH_INFO"].ToString() + "')");
                        }
                        catch
                        { }

                    }
                    catch (Exception err)
                    {
                        bError = true;
                        MessageLabel.Text = "Error File Save " + fn + "<br>" + err.Message.ToString();
                    }
                }
              if (!bError)
            {
                    if (filesUploaded == aFiles.Count)
                    {
                        MessageLabel.Text = "These " + filesUploaded + " file(s) were uploaded:<br>" + status;
                    }
                Session["sFiles" + sc.UserID()] = new ArrayList();
                aFiles.Clear();
                FilesListBox.Items.Clear();

                    if (!currentParent.Equals("gatewayUpload"))
                        Response.Redirect(redirectURL);
                    else
                    {
                        RaiseBubbleEvent("upload", new EventArgs());
                    }
                }
            }

        }

        //public void Upload_ServerClick(object sender, System.EventArgs e)
                //{
        //    bool bError = false;
        //    string baseLocation = ECN_Framework_BusinessLayer.Communicator.NewFile.GetBaseLocation(uploadDirectory, ImgFoldersDR.SelectedValue.ToString(), currentParent);
        //  string redirectURL = ECN_Framework_BusinessLayer.Communicator.NewFile.GetRedirectURL(currentParent, uploadDirectory, ImgFoldersDR.SelectedValue.ToString(), Request.Url.PathAndQuery.ToString());
        //    bError = ECN_Framework_BusinessLayer.Communicator.NewFile.Upload(aFiles, baseLocation, sc.CustomerID(), sc.UserID(), MessageLabel, FilesListBox, Request.ServerVariables["PATH_INFO"].ToString());
        //      if (!bError)
        //    {
        //        Session["sFiles" + sc.UserID()] = new ArrayList();
        //        aFiles.Clear();
        //        FilesListBox.Items.Clear();

        //        //if (!currentParent.Equals("gatewayUpload"))
        //        //    Response.Redirect(redirectURL);
        //        //else
        //        //{
        //        //    RaiseBubbleEvent("upload", new EventArgs());
        //        //}

        //        Response.Redirect(redirectURL);
        //      }
                //}

    }
}

