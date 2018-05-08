using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ActiveUp.WebControls.HtmlTextBox;
using ActiveUp.WebControls.HtmlTextBox.Tools;
using ecn.creator.classes;
using ecn.common.classes;
using System.Collections.Generic;
using System.Linq;

namespace ecn.creator.pages
{



    public partial class contentdetail : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.DropDownList ddContentTypeCode;
        //protected ActiveUp.WebControls.HtmlTextBox.Editor ContentSource;
        protected System.Web.UI.WebControls.Button GetTextButton;

        //ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
        ECN_Framework_BusinessLayer.Application.ECNSession es;

        public static string communicatordb = ConfigurationManager.AppSettings["communicatordb"];

        public contentdetail()
        {
            Page.Init += new System.EventHandler(Page_Init);
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.SubMenu = "add new content";
            Master.Heading = "Add/Edit Content";

            Master.HelpTitle = "Add/Edit Content";
            es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
           // ContentSource.ImageManager.ViewPaths = new string[] { "/ecn.images/Customers/" + ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID.ToString() + "/images" };


            if (KM.Platform.User.HasAccess(es.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.View))
            {
                //ContentSource.EnsureToolsCreated();
                int requestContentID = getContentID();
                string customerID = Master.UserSession.CurrentCustomer.CustomerID.ToString();

                if (Page.IsPostBack == false)
                {
                    folderID.DataSource = DataLists.GetFoldersDR(customerID, "CNT");
                    folderID.DataBind();
                    folderID.Items.Insert(0, "root");
                    folderID.Items.FindByValue("root").Value = "0";

                    List<KMPlatform.Entity.User> userList = new KMPlatform.BusinessLogic.User().SelectByClientID(es.CurrentCustomer.PlatformClientID, false);// DataLists.GetUsersDR("%", sc.CustomerID().ToString());

                    UserID.DataSource = userList.OrderBy(x => x.UserName) ;
                    UserID.DataBind();

                    if(KM.Platform.User.IsSystemAdministrator(es.CurrentUser))
                    {
                        UserID.Items.Insert(0,(new ListItem() { Text = es.CurrentUser.UserName, Value = es.CurrentUser.UserID.ToString() }));
                    }
                    if (requestContentID > 0)
                    {
                        //change form for edit/update method
                        ECN_Framework.Common.SecurityAccess.canI("Content", requestContentID.ToString());
                        LoadFormData(requestContentID);
                        SetUpdateInfo(requestContentID);
                        setShowPane();
                    }
                    else
                    {
                        presetUserID();
                        setShowPane();
                    }
                    //set images for wysiwyg object
                    //InitializeEditor();
                }
                else
                {
                    //InitializeEditor();
                    setShowPane();
                }
            }
            else
            {
                Response.Redirect("../default.aspx");
            }
        }

        private int getContentID()
        {
            int theContentID = 0;
            try
            {
                theContentID = Convert.ToInt32(Request.QueryString["ContentID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theContentID;
        }

        #region Form Prep

        private void presetUserID()
        {
            string theUserID = Master.UserSession.CurrentUser.UserID.ToString();
            UserID.Items.FindByValue(theUserID).Selected = true;
        }

        private void SetUpdateInfo(int setContentID)
        {
            ContentID.Text = setContentID.ToString();
            SaveButton.Text = "Update";
            SaveButton.Visible = false;
            //check customerlevel for duplicating contents
            checkDuplicateContent();

            UpdateButton.Visible = true;
        }

        private void SetCreateasnewInfo(int setContentID)
        {
            ContentID.Text = setContentID.ToString();
            SaveButton.Visible = false;
            UpdateButton.Visible = false;
            CreateAsNewTopButton.Visible = false;
            //CreateAsNewDownButton.Visible=true;

        }

        public void setShowPane()
        {

            /*switch (ddContentTypeCode.SelectedItem.Value) {
                case "html":*/
            panelContentSource.Visible = true;
            panelContentURL.Visible = false;
            panelContentFilePointer.Visible = false;
            /*break;
        case "text":
            panelContentSource.Visible=false;
            panelContentText.Visible=true;
            panelContentURL.Visible=false;
            panelContentFilePointer.Visible=false;
            GetTextButton.Visible=false;
            break;
        case "file":
            panelContentSource.Visible=false;
            panelContentText.Visible=false;
            panelContentURL.Visible=false;
            panelContentFilePointer.Visible=true;
            break;
        case "feed":
            panelContentSource.Visible=false;
            panelContentText.Visible=false;
            panelContentURL.Visible=true;
            panelContentFilePointer.Visible=false;
            break;
        default:
            break;
    }*/

        }

        private bool checkContentExists(string contentTitle)
        {
            string customerID = Master.UserSession.CurrentCustomer.CustomerID.ToString();
            bool exists = false;
            String sqlQuery =
                " SELECT count(*) " +
                " FROM " + communicatordb + ".dbo.Content " +
                " WHERE customerID=" + customerID + " and ContentTitle = '" + contentTitle + "'";
            string countRows = DataFunctions.ExecuteScalar(sqlQuery).ToString();

            if (Convert.ToInt32(countRows) > 0)
            {
                exists = true;
            }

            return exists;
        }

        private void checkDuplicateContent()
        {
            //SUNIL --TODO - replace substitute for customerlevel.

            //string customerLevel = sc.CustomerLevel();
            //if (customerLevel.Equals("1"))
            //{
            //    CreateAsNewTopButton.Visible = true;
            //    CreateAsNewTopButton.Enabled = false;
            //    CreateAsNewTopButton.ToolTip = "Your customer level does not allow you to use this functionality";
            //}
            //else
            //{
                CreateAsNewTopButton.Visible = false;
            //}
        }
        #endregion

        #region Data Load
        private void LoadFormData(int setContentID)
        {
            String sqlQuery =
                " SELECT * " +
                " FROM " + communicatordb + ".dbo.Content " +
                " WHERE ContentID=" + setContentID + " and ContentTypeCode = 'html'";
            DataTable dt = DataFunctions.GetDataTable(sqlQuery);
            foreach (DataRow dr in dt.Rows)
            {
                ContentTitle.Text = dr["ContentTitle"].ToString();
                folderID.Items.FindByValue(dr["FolderID"].ToString()).Selected = true;
                ContentSource.Text = dr["ContentSource"].ToString();
                ContentURL.Text = dr["ContentURL"].ToString();
                ContentFilePointer.Text = dr["ContentFilePointer"].ToString();
                try
                {
                    UserID.Items.FindByValue(dr["CreatedUserID"].ToString()).Selected = true;
                }
                catch {
                    try
                    {
                        UserID.Items.FindByValue(dr["UpdatedUserID"].ToString()).Selected = true;
                    }
                    catch { }
                }
                if (dr["LockedFlag"].ToString() == "Y")
                {
                    LockedFlag.Checked = true;
                    //ContentSource.Editable=false;
                    if (dr["CreatedUserID"].ToString() == Master.UserSession.CurrentUser.UserID.ToString())
                        UpdateButton.Enabled = true;
                    else
                        UpdateButton.Enabled = false;
                    UpdateButton.Attributes.Add("alt", "This Content is Locked");
                }
            }
        }

        private void CreateasNewLoadFormData(int setContentID)
        {
            String sqlQuery =
                " SELECT * " +
                " FROM Content " +
                " WHERE ContentID=" + setContentID + " ";
            DataTable dt = DataFunctions.GetDataTable(sqlQuery);
            foreach (DataRow dr in dt.Rows)
            {
                ContentTitle.Text = "";
                ddContentTypeCode.Items.FindByValue(dr["ContentTypeCode"].ToString()).Selected = true;
                folderID.Items.FindByValue(dr["FolderID"].ToString()).Selected = true;
                ContentSource.Text = dr["ContentSource"].ToString();
                ContentURL.Text = dr["ContentURL"].ToString();
                ContentFilePointer.Text = dr["ContentFilePointer"].ToString();
                UserID.Items.FindByValue(dr["CreatedUserID"].ToString()).Selected = true;
                if (dr["LockedFlag"].ToString() == "Y")
                {
                    LockedFlag.Checked = true;
                    //ContentSource.Editable=false;
                    UpdateButton.Enabled = false;
                    UpdateButton.Attributes.Add("alt", "This Content is Locked");
                }
            }
        }
        #endregion

        #region Data Handlers

        public void CreateContent(object sender, System.EventArgs e)
        {

            string ctitle = DataFunctions.CleanString(ContentTitle.Text);

            if (!(checkContentExists(ctitle)))
            {
                string lockedFlagValue = "N";
                if (LockedFlag.Checked == true)
                {
                    lockedFlagValue = "Y";
                }

                string csource = "";
                if (ContentSource.Text.Length > 0)
                {
                    csource = DataFunctions.CleanString(ContentSource.Text);
                }

                string sqlquery =
                    " INSERT INTO " + communicatordb + ".dbo.Content ( " +
                    " ContentTitle, ContentTypeCode, LockedFlag, CreatedUserID, FolderID, " +
                    " ContentSource, ContentURL, ContentFilePointer, " +
                    " CustomerID, CreatedDate " +
                    " ) VALUES ( " +
                    " '" + ctitle + "', 'html', '" + lockedFlagValue + "', " + UserID.SelectedItem.Value + ", " + folderID.SelectedItem.Value + ", " +
                    " '" + csource + "', '" + ContentURL.Text + "', '" + ContentFilePointer.Text + "', " +
                    " " + Master.UserSession.CurrentCustomer.CustomerID.ToString() + ", '" + System.DateTime.Now + "' " +
                    " ) ";
                DataFunctions.Execute(sqlquery);
                Response.Redirect("default.aspx");
            }
            else
            {
                msglabel.Visible = true;
                msglabel.Text = "Content with the same title already exists. Please use a different Title name.";
            }
        }

        public void UpdateContent(object sender, System.EventArgs e)
        {

            string customerID = Master.UserSession.CurrentCustomer.CustomerID.ToString();
            string ctitle = DataFunctions.CleanString(ContentTitle.Text);
            bool exists = false;
            String sqlQuery =
                " SELECT * " +
                " FROM " + communicatordb + ".dbo.Content " +
                " WHERE customerID=" + customerID + " and ContentTitle = '" + ctitle + "'";
            DataTable dt = DataFunctions.GetDataTable(sqlQuery);
            foreach (DataRow dr in dt.Rows)
            {
                if (!(dr["ContentID"].ToString()).Equals(getContentID().ToString()))
                {
                    exists = true;
                    break;
                }
            }

            if (!(exists))
            {
                string lockedFlagValue = "N";
                if (LockedFlag.Checked == true)
                {
                    lockedFlagValue = "Y";
                }
                string csource = DataFunctions.CleanString(ContentSource.Text);
                //string ctext=DataFunctions.CleanString(ContentText.Text);
                string sqlquery =
                    " UPDATE " + communicatordb + ".dbo.Content SET " +
                    " ContentTitle='" + ctitle + "', " +
                    " ContentTypeCode='html', " +
                    " FolderID=" + folderID.SelectedItem.Value + ", " +
                    " ContentURL='" + ContentURL.Text + "', " +
                    " ContentFilePointer='" + ContentFilePointer.Text + "', " +
                    " ContentSource='" + csource + "', " +
                    " LockedFlag='" + lockedFlagValue + "', " +
                    " UpdatedUserID=" + UserID.SelectedItem.Value + ", " +
                    " UpdatedDate='" + System.DateTime.Now + "' " +
                    " WHERE ContentID=" + ContentID.Text;
                DataFunctions.Execute(sqlquery);
                Response.Redirect("default.aspx");
            }
            else
            {
                msglabel.Visible = true;
                msglabel.Text = "Content with the same title already exists. Please use a different Title name.";
            }
        }

        public void CreateAsNewInitialize(object sender, System.EventArgs e)
        {
            int requestContentID = getContentID();
            SetCreateasnewInfo(requestContentID);
            CreateasNewLoadFormData(requestContentID);
            //InitializeEditor();
        }

        /*public void CreateAsNewContent(object sender, System.EventArgs e){
            string ctitle=DataFunctions.CleanString(ContentTitle.Text);
            if(!(checkContentExists(ctitle))){
                string lockedFlagValue="N";
                if (LockedFlag.Checked==true) {
                    lockedFlagValue="Y";
                }

                string csource=DataFunctions.CleanString(ContentSource.Text);
                string ctext="";
                if (ContentText.Text.Length>0) {
                    ctext=DataFunctions.CleanString(ContentText.Text);
                } else {
                    ctext=DataFunctions.CleanString(ContentSource.TextStripped);
                }
                string sqlquery=
                    " INSERT INTO Content ( "+
                    " ContentTitle, ContentTypeCode, LockedFlag, UserID, FolderID, "+
                    " ContentSource, ContentText, ContentURL, ContentFilePointer, "+
                    " CustomerID, ModifyDate "+
                    " ) VALUES ( "+
                    " '"+ctitle+"', '"+ddContentTypeCode.SelectedItem.Value+"', '"+lockedFlagValue+"', "+UserID.SelectedItem.Value+", "+folderID.SelectedItem.Value+", "+
                    " '"+csource+"', '"+ctext+"', '"+ContentURL.Text+"', '"+ContentFilePointer.Text+"', "+
                    " "+sc.CustomerID()+", '"+System.DateTime.Now+"' "+
                    " ) ";
                DataFunctions.Execute(sqlquery);
                Response.Redirect("default.aspx");
            }else{
                msglabel.Visible = true;
                msglabel.Text = "Content with the same title already exists. Please use a different Title name.";
            }
        }*/
        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
        }

        #region Web Form Designer generated code

        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.

        private void InitializeComponent()
        {

        }
        #endregion


    }
}
