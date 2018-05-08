using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ecn.creator.classes;
using ecn.common.classes;

namespace ecn.creator.pages
{



    public partial class pagelist : System.Web.UI.Page
    {
        //protected System.Web.UI.WebControls.DropDownList ContentFolderID;
        //protected System.Web.UI.WebControls.DropDownList PageFolderID;

        //ECN_Framework.Common.SecurityCheck sc = new ECN_Framework.Common.SecurityCheck();
        ECN_Framework_BusinessLayer.Application.ECNSession es;
        public static string communicatordb = ConfigurationManager.AppSettings["communicatordb"];
        public static string accountsdb = ConfigurationManager.AppSettings["accountsdb"];
        public static string creatorConn = ConfigurationManager.AppSettings["cre"];

        string CntFolderID = "0";
        string PgFolderID = "0";

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.SubMenu = "content and page list";
            Master.Heading = "Your Content and Campaigns";

            Master.HelpTitle = "Your Content and Campaigns";
            es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();
            ContentFolderID.FolderEvent += new EventHandler(ContentFolderEvent);
            PageFolderID.FolderEvent += new EventHandler(PageFolderEvent);
            if (KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.View))
            {
                int requestContentID = 0;
                int requestPageID = 0;
                String CustomerID = Master.UserSession.CurrentCustomer.CustomerID.ToString();

                if (!Page.IsPostBack)
                {
                    requestContentID = getContentID();
                    requestPageID = getPageID();

                    if (requestContentID > 0)
                    {
                        ECN_Framework.Common.SecurityAccess.canI("Content", requestContentID.ToString());
                        DeleteContent(requestContentID);
                    }
                    if (requestPageID > 0)
                    {
                        ECN_Framework.Common.SecurityAccess.canI("Pages", requestPageID.ToString());
                        DeletePage(requestPageID);
                    }

                    LoadUserDD("%", "%");
                    loadContentFoldersDD(CustomerID, "CNT");
                    loadPageFoldersDD(CustomerID, "CNT");

                    loadContentGrid(CustomerID, "0");
                    loadPagesGrid(CustomerID, "0");
                }
                else
                {
                    ContentUserID.SelectedIndexChanged += new EventHandler(ContentUserID_SelectedIndexChanged);
                    PageUserID.SelectedIndexChanged += new EventHandler(PageUserID_SelectedIndexChanged);
                    string contentuserID = "%";
                    string pageuserID = "%";
                    try
                    {
                        contentuserID = ContentUserID.SelectedItem.Value.ToString();
                        pageuserID = PageUserID.SelectedItem.Value.ToString();
                        LoadUserDD(contentuserID, pageuserID);
                    }
                    catch (System.NullReferenceException ne)
                    {
                        string devnul = ne.ToString();
                        LoadUserDD("%", "%");
                    }
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

        private int getPageID()
        {
            int thePageID = 0;
            try
            {
                thePageID = Convert.ToInt32(Request.QueryString["PageID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return thePageID;
        }

        private void setError(string errorMessage)
        {
            ErrorLabel.Visible = true;
            ErrorLabel.Text = errorMessage;

            ErrorLabel.CssClass = "errormsg";
        }

        #region Data Load
        public void LoadUserDD(string contentusermatch, string layoutusermatch)
        {
            ContentUserID.DataSource = new KMPlatform.BusinessLogic.User().SelectByClientID(es.CurrentCustomer.PlatformClientID, false);// DataLists.GetUsersDR("%", sc.CustomerID().ToString());
            ContentUserID.DataBind();

            PageUserID.DataSource = new KMPlatform.BusinessLogic.User().SelectByClientID(es.CurrentCustomer.PlatformClientID, false);// DataLists.GetUsersDR("%", sc.CustomerID().ToString());
            PageUserID.DataBind();

            if (KM.Platform.User.IsSystemAdministrator(es.CurrentUser))
            {
                ContentUserID.Items.Insert(0, (new ListItem() { Text = es.CurrentUser.UserName, Value = es.CurrentUser.UserID.ToString() }));
                PageUserID.Items.Insert(0, (new ListItem() { Text = es.CurrentUser.UserName, Value = es.CurrentUser.UserID.ToString() }));
            }
            if ((contentusermatch.Equals("%")) && (layoutusermatch.Equals("%")))
            {
                string userID = Master.UserSession.CurrentUser.UserID.ToString();
                ContentUserID.Items.FindByValue(userID).Selected = true;
                PageUserID.Items.FindByValue(userID).Selected = true;
            }
            else
            {
                ContentUserID.Items.FindByValue(contentusermatch).Selected = true;
                PageUserID.Items.FindByValue(layoutusermatch).Selected = true;
            }
            if (ContentUserID.Items.Count == 1)
            {
                ContentUserID.Visible = false;
            }
            if (PageUserID.Items.Count == 1)
            {
                PageUserID.Visible = false;
            }
        }

        private void loadPagesGrid(String CustomerID, string folderID)
        {
            string sqlquery =
                /*" SELECT p.PageID, p.PageName, p.QueryValue, t.SlotsTotal, u.UserName "+
                " FROM Pages p, "+communicatordb+".dbo.Templates t, "+accountsdb+".dbo.Users u "+
                " WHERE p.CustomerID="+CustomerID+
                " AND p.TemplateID=t.TemplateID "+
                " AND p.UserID=u.UserID "+
                " AND u.UserID="+PageUserID.SelectedItem.Value+
                " AND p.FolderID = "+folderID + 
                " ORDER BY p.PageName ";*/
            " SELECT p.PageID, p.PageName, p.QueryValue, CONVERT(VARCHAR,p.Pagesize)+' KB' as PageSize, t.SlotsTotal " +
                " FROM Page p " +
                " JOIN " + communicatordb + ".dbo.Template t ON p.TemplateID=t.TemplateID " +
                " WHERE p.CustomerID=" + CustomerID +
                " AND p.UserID=" + PageUserID.SelectedItem.Value +
                " AND p.FolderID = " + folderID +
                " ORDER BY p.PageName ";
            DataTable dt = DataFunctions.GetDataTable(sqlquery, creatorConn);
            PagesGrid.DataSource = dt.DefaultView;
            PagesGrid.CurrentPageIndex = 0;
            PagesGrid.DataBind();
            PagesPager.RecordCount = dt.Rows.Count;
            //PagesPager.PagerStyle.CssClass = 
            upPages.Update();
        }

        private void loadContentGrid(string CustomerID, string folderID)
        {
            string sqlquery = "";
            sqlquery =
                " SELECT ContentID as ContentID, (convert(varchar(255),ContentID)+'&chID=" + es.CurrentBaseChannel.BaseChannelID.ToString() + "&cuID=" + CustomerID + "') as ContentIDplus, " +
                " REPLACE (c.ContentTitle, '[CORPORATE]', '<font color=''FF0000''>[CORPORATE]</FONT> ') as 'ContentTitle', " +
                " ContentTypeCode,LockedFlag " +
                " FROM " + communicatordb + ".dbo.Content c" +
                " WHERE c.CustomerID=" + CustomerID +
                " AND c.folderID = " + folderID +
                " AND c.IsDeleted = 0" +
                " AND c.ContentTypeCode = 'html' " +
                " AND ( c.CreatedUserID =" + ContentUserID.SelectedItem.Value + " OR c.UpdatedUserID = " + ContentUserID.SelectedItem.Value + ")" +
                " ORDER BY ContentTitle ";
            DataTable dt = DataFunctions.GetDataTable(sqlquery, creatorConn);
            ContentGrid.DataSource = dt.DefaultView;
            ContentGrid.CurrentPageIndex = 0;
            ContentGrid.DataBind();
            
            ContentPager.RecordCount = dt.Rows.Count;
            upContent.Update();
        
        }

        private void loadContentFoldersDD(string CustomerID, string selectedFolderType)
        {
            /*ContentFolderID.DataSource=DataLists.GetFoldersDR(CustomerID, "CNT");
            ContentFolderID.DataBind();
            ContentFolderID.Items.Insert(0,"root");
            ContentFolderID.Items.FindByValue("root").Value = "0";
            ContentFolderID.Items.FindByValue(FolderID).Selected = true; 
            */
            ContentFolderID.ID = "ContentFolderID";
            ContentFolderID.CustomerID = Convert.ToInt32(CustomerID);
            ContentFolderID.FolderType = selectedFolderType;
            ContentFolderID.ConnString = ecn.common.classes.DataFunctions.connStr;
            ContentFolderID.NodesExpanded = true;
            ContentFolderID.ChildrenExpanded = false;
            ContentFolderID.LoadFolderTree();
        }

        private void loadPageFoldersDD(string CustomerID, string selectedFolderType)
        {
            /*PageFolderID.DataSource=DataLists.GetFoldersDR(CustomerID, "CNT");
            PageFolderID.DataBind();
            PageFolderID.Items.Insert(0,"root");
            PageFolderID.Items.FindByValue("root").Value = "0";
            PageFolderID.Items.FindByValue(FolderID).Selected = true;*/
            PageFolderID.ID = "PageFolderID";
            PageFolderID.CustomerID = Convert.ToInt32(CustomerID);
            PageFolderID.FolderType = selectedFolderType;
            PageFolderID.ConnString = ecn.common.classes.DataFunctions.connStr;
            PageFolderID.NodesExpanded = true;
            PageFolderID.ChildrenExpanded = false;
            PageFolderID.LoadFolderTree();
        }
        #endregion

        #region Data Handlers

        public void DeleteContent(int theContentID)
        {
            try
            {
                ECN_Framework_BusinessLayer.Communicator.Content.Delete(theContentID, es.CurrentUser);
                loadContentGrid(Master.UserSession.CurrentCustomer.CustomerID.ToString(), ContentFolderID.SelectedFolderID.ToString());
            }
            catch (ECN_Framework_Common.Objects.ECNException ecn)
            {
                string errorMessage = "";
                foreach (ECN_Framework_Common.Objects.ECNError e in ecn.ErrorList)
                {
                    errorMessage += e.ErrorMessage + "<br />";
                }
                setError(errorMessage);
                return;
            }

        }

        public void DeletePage(int thePageID)
        {

            string sqlpages =
                " DELETE FROM Page " +
                " WHERE PageID=" + thePageID;
            DataFunctions.Execute(sqlpages);
            Response.Redirect("default.aspx");
        }

        #endregion

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

        private void ContentFolderEvent(object sender, EventArgs e)
        {
            TreeView tn = (TreeView)sender;
            
            ContentGrid.CurrentPageIndex = 0;

            ContentPager.CurrentPage = 0;
            loadContentGrid(es.CustomerID.ToString(), tn.SelectedNode.Value);
            
        }

        private void PageFolderEvent(object sender, EventArgs e)
        {
            TreeView tn = (TreeView)sender;
            
            PagesGrid.CurrentPageIndex = 0;
            PagesPager.CurrentPage = 0;
            loadPagesGrid(es.CustomerID.ToString(), tn.SelectedNode.Value);
            
        }

        private void ContentUserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContentPager.CurrentPage = 1;
            ContentPager.CurrentIndex = 0;
            //loadContentFoldersDD(sc.CustomerID(), "0");
            loadContentGrid(es.CustomerID.ToString(), ContentFolderID.SelectedFolderID.ToString());
            
        }

        private void PageUserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            PagesPager.CurrentPage = 1;
            PagesPager.CurrentIndex = 0;
            loadPagesGrid(es.CustomerID.ToString(), PageFolderID.SelectedFolderID.ToString());
        
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            //string CntFolderID = "0";
            //string PgFolderID = "0";
            //try { CntFolderID = ContentFolderID.SelectedFolderID.ToString(); }
            //catch { }

            //try { PgFolderID = PageFolderID.SelectedFolderID.ToString(); }
            //catch { }

            //string customerID = Master.UserSession.CurrentCustomer.CustomerID.ToString();
            //loadContentGrid(customerID, CntFolderID);
            //loadPagesGrid(customerID, PgFolderID);
        }


        protected void imgbtnDeleteContent_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            DeleteContent(Convert.ToInt32(imgbtn.CommandArgument.ToString()));
           
            loadPagesGrid(es.CustomerID.ToString(), PageFolderID.SelectedFolderID.ToString());
        }

        protected void ContentGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = (DataRowView)e.Item.DataItem;

                ImageButton imgbtnDeleteContent = (ImageButton)e.Item.FindControl("imgbtnDeleteContent");
                imgbtnDeleteContent.CommandArgument = dr["ContentID"].ToString();
            }
        }

        protected void ContentPager_IndexChanged(object sender, EventArgs e)
        {
            loadContentGrid(es.CustomerID.ToString(), ContentFolderID.SelectedFolderID.ToString());
            
        }

        protected void PagesPager_IndexChanged(object sender, EventArgs e)
        {
            loadPagesGrid(es.CustomerID.ToString(), PageFolderID.SelectedFolderID.ToString());
            
        }

        protected void PageUserID_SelectedIndexChanged1(object sender, EventArgs e)
        {
            loadPagesGrid(es.CustomerID.ToString(), PageFolderID.SelectedFolderID.ToString());
        }

        protected void ContentUserID_SelectedIndexChanged1(object sender, EventArgs e)
        {
            loadContentGrid(es.CustomerID.ToString(), ContentFolderID.SelectedFolderID.ToString());
        }


    }
}

