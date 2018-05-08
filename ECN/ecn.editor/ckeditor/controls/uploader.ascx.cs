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


namespace ecn.editor.ckeditor.controls
{
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

        public int getCustomerID()
        {
            try
            {
                if (ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID > 0)
                    return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                else
                    return Convert.ToInt32(Request.QueryString["cuID"].ToString());
            }
            catch
            {
                return Convert.ToInt32(Request.QueryString["cuID"].ToString());
            }
        }

        public int getChannelID()
        {
            try
            {
                if (ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID > 0)
                    return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID;
                else
                    return Convert.ToInt32(Request.QueryString["chID"].ToString());
            }
            catch
            {
                return Convert.ToInt32(Request.QueryString["chID"].ToString());
            }
        }

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
                try
                {
                aFiles = (ArrayList)Session["sFiles"];
                }
              catch
                {
                  
                   aFiles = new ArrayList();
                   aFiles.Add(Session["sFiles"]);
                  
                }
                if (aFiles == null)
                    aFiles = new ArrayList();
            }

            MessageLabel.Text = "";
            System.Web.UI.Control currentParentControl = this.Parent;
            _currentParent = currentParentControl.ID;

            if (currentParent.Equals("ImageManagerForm") || currentParent.Equals("PanelUpload2"))
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

        public void LoadImgFoldersDR()
        {
            DataTable dt = ECN_Framework_BusinessLayer.Communicator.NewFile.GetDataTable_ImageFolder(getCustomerID().ToString());
            DataView dvFolders = new DataView(dt);
            ImgFoldersDR.DataSource = dvFolders;
            ImgFoldersDR.DataBind();
        }



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

        protected void RemvFile_Click(object sender, System.EventArgs e)
        {
            //if (FilesListBox.Items.Count != 0)
            //{

            //    aFiles.RemoveAt(FilesListBox.SelectedIndex);
            //    FilesListBox.Items.Remove(FilesListBox.SelectedItem.Text);
            //}
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
            }
                Session["sFiles"] = aFiles;
        }

        public void Upload_ServerClick(object sender, System.EventArgs e)
        {
            bool bError = false;
            string selectedFolder = System.Web.HttpUtility.UrlEncode(ImgFoldersDR.SelectedValue.ToString());
            string pathAndQuery = HttpUtility.UrlDecode(Request.Url.PathAndQuery.ToString());

            string baseLocation = ECN_Framework_BusinessLayer.Communicator.NewFile.GetBaseLocation(uploadDirectory, ImgFoldersDR.SelectedValue.ToString(), currentParent);
            // string baseLocation = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + uploadDirectory);
            string redirectURL = ECN_Framework_BusinessLayer.Communicator.NewFile.GetRedirectURL(currentParent, uploadDirectory, selectedFolder, pathAndQuery);

            //  string redirectURL = ECN_Framework_BusinessLayer.Communicator.NewFile.GetRedirectURL(currentParent, uploadDirectory, HttpUtility.UrlEncode(ImgFoldersDR.SelectedValue.ToString()), HttpUtility.UrlDecode(Request.Url.PathAndQuery.ToString()));
            bError = ECN_Framework_BusinessLayer.Communicator.NewFile.Upload(aFiles, baseLocation, getCustomerID().ToString(), "0", MessageLabel, FilesListBox, Request.ServerVariables["PATH_INFO"].ToString());
         
            if (!bError)
            {
               // Session["sFiles"] = "";
                Session["sFiles"] = new ArrayList();
                
               // FilesListBox.Items.Clear();
                ClearFileListBox();
                aFiles.Clear();

                Response.Redirect(redirectURL);
            }
        }

        public void ClearFileListBox()
        {
            if (FilesListBox.Items.Count > 0)
            {
                for (int a = FilesListBox.Items.Count - 1; a > 0; a--)
                {
                    FilesListBox.Items.RemoveAt(a);
                }
                
               // FilesListBox.Items.Clear();

            }
        }
    }
}
