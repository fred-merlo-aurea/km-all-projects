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
using ecn.accounts.classes;
using ecn.common.classes;


namespace ecn.accounts.includes
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

        public ArrayList files = new ArrayList();
        static public ArrayList hif = new ArrayList();
        public int filesUploaded = 0;

        protected void Page_Load(object sender, System.EventArgs e)
        {


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
            if (Page.IsPostBack == true)
            {
                hif.Add(FindFile);
                FilesListBox.Items.Add(FindFile.PostedFile.FileName);

            }
            else
            {

            }
        }


        /// RemvFile will remove the currently selected file from the listbox.

        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RemvFile_Click(object sender, System.EventArgs e)
        {
            if (FilesListBox.Items.Count != 0)
            {

                hif.RemoveAt(FilesListBox.SelectedIndex);
                FilesListBox.Items.Remove(FilesListBox.SelectedItem.Text);
            }

        }


        /// Upload_ServerClick is the server side script that will upload the files to the web server
        /// by looping through the files in the listbox.

        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Upload_ServerClick(object sender, System.EventArgs e)
        {
            ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
            string baseLocation = uploadDirectory;
            ECN_Framework_BusinessLayer.Communicator.NewFile.Upload(hif, baseLocation, sc.CustomerID().ToString(), sc.UserID(), MessageLabel, FilesListBox, Request.ServerVariables["PATH_INFO"].ToString());
            hif.Clear();
            FilesListBox.Items.Clear();
            Response.Redirect(Request.Url.PathAndQuery);
        }

    }
}