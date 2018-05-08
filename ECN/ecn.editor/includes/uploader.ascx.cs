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
using System.Xml;
using ecn.collector.classes;
using ecn.common.classes;


namespace ecn.editor.includes
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

        public string getCustomerID()
        {
            try
            {
                if (Convert.ToInt32(sc.CustomerID()) > 0)
                    return sc.CustomerID().ToString();
                else
                    return Request.QueryString["cuID"].ToString();
            }
            catch
            {
                return Request.QueryString["cuID"].ToString();
            }
        }

        public string getChannelID()
        {
            try
            {
                if (Convert.ToInt32(sc.BasechannelID()) > 0)
                    return sc.BasechannelID().ToString();
                else
                    return Request.QueryString["chID"].ToString();
            }
            catch
            {
                return Request.QueryString["chID"].ToString();
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
                aFiles = (ArrayList)Session["sFiles"];
                if (aFiles == null)
                    aFiles = new ArrayList();
            }

            MessageLabel.Text = "";
            System.Web.UI.Control currentParentControl = this.Parent;
            _currentParent = currentParentControl.ID;

            if (currentParent.Equals("ImageManagerForm") || currentParent.Equals("PanelUpload"))
            {
                HelpPanel.Visible = true;
                FoldersPanel.Visible = true;
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
            string dirname = "";
            System.IO.DirectoryInfo dir = null;

            dirs = System.IO.Directory.GetDirectories(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + getCustomerID() + "/images"));

            dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "ROOT Folder";
            dt.Rows.Add(dr);

            for (int i = 0; i <= dirs.Length - 1; i++)
            {
                //Create a new DirectoryInfo object for this Dir
                dir = new System.IO.DirectoryInfo(dirs[i]);
                dirname = dir.Name.ToString();
                dr = dt.NewRow();
                dr[0] = dirname;
                dr[1] = " - - - " + dirname;

                dt.Rows.Add(dr);
            }

            DataView dvFolders = new DataView(dt);
            ImgFoldersDR.DataSource = dvFolders;
            ImgFoldersDR.DataBind();
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
                    if (FindFile.PostedFile.FileName.ToLower().EndsWith(".jpg") || FindFile.PostedFile.FileName.ToLower().EndsWith(".gif") || FindFile.PostedFile.FileName.ToLower().EndsWith(".png") ||
                        FindFile.PostedFile.FileName.ToLower().EndsWith(".xls") || FindFile.PostedFile.FileName.ToLower().EndsWith(".xml") ||
                        FindFile.PostedFile.FileName.ToLower().EndsWith(".txt") || FindFile.PostedFile.FileName.ToLower().EndsWith(".csv"))
                    {
                        aFiles.Add(FindFile);
                        FilesListBox.Items.Add(FindFile.PostedFile.FileName);
                        MessageLabel.Text = "";
                    }
                    else
                    {
                        MessageLabel.Text = "ERROR: Cannot Upload File: <br><br>Only files with following extensions are supported in ECN:<br><u>Image Files</u>: JPG, GIF, PNG<br><u>Other Files</u>: XLS, XML, TXT, CSV";
                    }
                    Session["sFiles"] = aFiles;
                }
                else
                {

                }
            }
            catch
            {
                MessageLabel.Text = "ERROR: Cannot Upload File: <br><br>Only Files with less than 25 MB is supported in ECN";
            }
        }


        /// RemvFile will remove the currently selected file from the listbox.

        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RemvFile_Click(object sender, System.EventArgs e)
        {
            if (FilesListBox.Items.Count != 0)
            {

                aFiles.RemoveAt(FilesListBox.SelectedIndex);
                FilesListBox.Items.Remove(FilesListBox.SelectedItem.Text);
            }
            Session["sFiles"] = aFiles;
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
                redirectURL = "filemanager.aspx?chID=" + getChannelID() + "&cuID=" + getCustomerID();
                if (ImgFoldersDR.SelectedValue.Length > 0)
                {
                    baseLocation += @"\" + ImgFoldersDR.SelectedValue.ToString();
                    redirectURL += "&folder=/" + ImgFoldersDR.SelectedValue.ToString();
                }
            }
            else
            {
                redirectURL = Request.Url.PathAndQuery.ToString();
            }
            string status = "";


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
                        fn = StringFunctions.Replace(System.IO.Path.GetFileName(f.PostedFile.FileName), " ", "_");
                        fn = StringFunctions.Replace(fn, "\'", "_");
                        f.PostedFile.SaveAs(baseLocation + "\\" + fn);
                        filesUploaded++;
                        status += fn + "<br>";

                        try
                        {
                            DataFunctions.Execute("insert into Uploadlog (UserID,CustomerID,Path,FileName,uploaddate,PageSource) values (0," + getCustomerID() + ",'" + baseLocation + "','" + fn.Replace("'", "''") + "',getdate(),'" + Request.ServerVariables["PATH_INFO"].ToString() + "')");
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
                    Session["sFiles"] = "";
                    aFiles.Clear();
                    FilesListBox.Items.Clear();

                    Response.Redirect(redirectURL);
                }
            }

        }

    }
}
