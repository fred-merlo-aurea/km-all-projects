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
using ecn.creator.classes;
using ecn.common.classes;


namespace ecn.creator.includes
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

        ArrayList aFiles = new ArrayList();
        public int filesUploaded = 0;

        //private string[] imgExt = { "jpg", "peg", "bmp", "tif", "tiff", "gif" };

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


        /// AddFile will add the path of the client side file that is currently in the PostedFile
        /// property of the HttpInputFile control to the listbox.

        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddFile_Click(object sender, System.EventArgs e)
        {
            if (Page.IsPostBack == true && FindFile.PostedFile.FileName != "")
            {
                aFiles.Add(FindFile);
                FilesListBox.Items.Add(FindFile.PostedFile.FileName);
            }

            Session["sFiles"] = aFiles;
        }


        /// RemvFile will remove the currently selected file from the listbox.

        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RemvFile_Click(object sender, System.EventArgs e)
        {
            if (FilesListBox.Items.Count != 0 && FilesListBox.SelectedIndex >= 0)
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
            string uploadDirectory2 = "";
            if (uploadDirectory.StartsWith("/ecn.images")) uploadDirectory2 = uploadDirectory.Substring(uploadDirectory.IndexOf("/", 3));
            else uploadDirectory2 = uploadDirectory;
            string baseLocation = ECN_Framework_BusinessLayer.Communicator.NewFile.GetBaseLocation(uploadDirectory2);
            ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            
            bool bError = ECN_Framework_BusinessLayer.Communicator.NewFile.Upload(aFiles, baseLocation, es.CustomerID.ToString(), es.CurrentUser.UserID.ToString(), MessageLabel, FilesListBox, Request.ServerVariables["PATH_INFO"].ToString());
            if (!bError)
                    {
                Session["sFiles"] = "";
                aFiles.Clear();
                FilesListBox.Items.Clear();
            }

        }


        //private bool ExtIsThere(string str)
        //{
        //    bool isImg = false;
        //    for (int i = 0; i < imgExt.Length; i++)
        //    {
        //        if (imgExt[i].CompareTo(str) == 0)
        //        {
        //            isImg = true;
        //            break;
        //        }
        //    }
        //    return isImg;
        //}
    }
}
