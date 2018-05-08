using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using ecn.communicator.classes;
using ecn.common.classes;

namespace ecn.communicator.main.PageWatch
{
    public partial class PageWatchTagsEditor : ECN_Framework.WebPageHelper
    {
        int customerID = 0;
        int userID = 0;

        public static string accountsdb = ConfigurationManager.AppSettings["accountsdb"];
        public static string connString = ConfigurationManager.AppSettings["com"];

        private int getPageWatchID()
        {
            int thePageWatchID = 0;
            try
            {
                thePageWatchID = Convert.ToInt32(Request.QueryString["PageWatchID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return thePageWatchID;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.PAGEKNOWTICE ;
            Master.SubMenu = "";
            Master.Heading = "Page Watch Tags";
            Master.HelpContent = "";
            Master.HelpTitle = "Page Watch Tags Setup";	

            if ((KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser)) && ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(Master.UserSession.CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.PageKnowtice))
            {
                customerID = Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID.ToString());
                userID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);

                if (!IsPostBack)
                {
                    divError.Visible = false;
                    lblErrorMessage.Text = "";

                    ViewState["SortField"] = "Name";
                    ViewState["SortDirection"] = "ASC";

                    LoadGridView();
                    LoadActive();
                    SetDefaults();
                    btnSave.Text = "Add";
                }
            }
            else
            {
                Response.Redirect("default.aspx");
            }
        }

        private void SetDefaults()
        {
            try
            {                
                lblErrorMessage.Text = "";
                lblPageWatchTagID.Text = "";
                txtName.Text = "";
                txtTag.Text = "";
                ddlIsActive.SelectedIndex = -1;
                ddlIsActive.Items.FindByValue("1").Selected = true;
                FCKeditorPrevious.Text = "";
                FCKeditorCurrent.Text = "";
                pnlEdit.Visible = false;
                pnlCompare.Visible = false;
                divError.Visible = false;
            }
            catch (Exception)
            {
            }
        }
        
        private void LoadActive()
        {
            ddlIsActive.Items.Clear();
            ddlIsActive.Items.Add(new ListItem("YES", "1"));
            ddlIsActive.Items.Add(new ListItem("NO", "0"));
        }

        private void LoadGridView()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from PageWatchTag where PageWatchID = @PageWatchID";
            cmd.Parameters.AddWithValue("@PageWatchID", getPageWatchID());

            DataTable dtPageWatchTags = DataFunctions.GetDataTable(cmd);

            btnAccept.Enabled = false;
            btnIgnore.Enabled = false;
            foreach (DataRow row in dtPageWatchTags.Rows)
            {
                if (Convert.ToBoolean(row["IsChanged"].ToString()))
                {
                    btnAccept.Enabled = true;
                    btnIgnore.Enabled = true;
                    break;
                }
            }

            DataView dvPageWatchTags = dtPageWatchTags.DefaultView;

            dvPageWatchTags.Sort = ViewState["SortField"].ToString() + ' ' + ViewState["SortDirection"].ToString();

            gvPageWatchTags.DataSource = dvPageWatchTags;

            try
            {
                gvPageWatchTags.DataBind();
            }
            catch
            {
                gvPageWatchTags.PageIndex = 0;
                gvPageWatchTags.DataBind();
            }
        }

        private void LoadDetails(int PageWatchID)
        {
            //string sqlquery = "";
            //sqlquery = "SELECT * from PageWatch where PageWatchID = " + PageWatchID;

            //DataTable dtMessageTypes = DataFunctions.GetDataTable(sqlquery, ConfigurationManager.AppSettings["com"].ToString());

            //dvPageWatch.DataSource = dtMessageTypes.DefaultView;

            //try
            //{
            //    dvPageWatch.DataBind();
            //}
            //catch
            //{
            //}
        }

        public void ViewPageWatchTag(int pageWatchTagID)
        {            
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select pwt.*, pw.URL from pagewatchtag pwt join PageWatch pw on pwt.PageWatchID = pw.PageWatchID where pwt.pagewatchtagid = @PageWatchTagID";
            cmd.Parameters.AddWithValue("@PageWatchTagID", pageWatchTagID);

            try
            {
                DataTable dtPageWatch = DataFunctions.GetDataTable(cmd);

                string url = dtPageWatch.Rows[0]["URL"].ToString();
                string tag = dtPageWatch.Rows[0]["WatchTag"].ToString();
                lblView.Text = dtPageWatch.Rows[0]["Name"].ToString();
                FCKeditorPrevious.Text = dtPageWatch.Rows[0]["PreviousHTML"].ToString();

                string errorMessage = PageWatchTag.ValidateTag(tag, url);
                if (errorMessage == string.Empty)
                {
                    FCKeditorCurrent.Text = PageWatchTag.GetCurrentHTML(tag, url);
                    lblErrorMessage.Text = "";
                    divError.Visible = false;
                }
                else
                {
                    lblErrorMessage.Text = "ERROR : " + errorMessage + ". Tag has been deactivated.";
                    divError.Visible = true;
                    PageWatchTag.SetInactive(pageWatchTagID);
                    LoadGridView();
                }
            }
            catch (Exception)
            {
                lblErrorMessage.Text = "ECN experienced an error getting the Tag infomation";
                divError.Visible = true;
            }
            
            pnlEdit.Visible = false;
            pnlCompare.Visible = true;            
        }

        public void DeletePageWatchTag(int pageWatchTagID)
        {
            divError.Visible = false;
            lblErrorMessage.Text = "";

            string sqlquery =
                " DELETE FROM PageWatchHistory " +
                " WHERE PageWatchTagID =" + pageWatchTagID;
            DataFunctions.Execute(sqlquery);

            sqlquery =
                " DELETE FROM PageWatchTag " +
                " WHERE PageWatchTagID =" + pageWatchTagID;
            DataFunctions.Execute(sqlquery);

            LoadGridView();
        }

        public void EditPageWatchTag(int pageWatchTagID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select pwt.*, pw.URL from pagewatchtag pwt join PageWatch pw on pwt.PageWatchID = pw.PageWatchID where pwt.pagewatchtagid = @PageWatchTagID";
            cmd.Parameters.AddWithValue("@PageWatchTagID", pageWatchTagID);
            DataTable dtPageWatch = DataFunctions.GetDataTable(cmd);

            ddlIsActive.SelectedIndex = -1;

            lblPageWatchTagID.Text = pageWatchTagID.ToString();
            txtName.Text = dtPageWatch.Rows[0]["Name"].ToString();
            txtTag.Text = dtPageWatch.Rows[0]["WatchTag"].ToString();
            if (Convert.ToBoolean(dtPageWatch.Rows[0]["IsActive"].ToString()))
            {
                ddlIsActive.Items.FindByValue("1").Selected = true;
            }
            else
            {
                ddlIsActive.Items.FindByValue("0").Selected = true;
            }
            
            lblErrorMessage.Text = "";
            divError.Visible = false;
            pnlCompare.Visible = false;
            pnlEdit.Visible = true;
            
            btnSave.Text = "Update";
        }

        protected void gvPageWatchTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Text = "Save";
            LoadDetails(Convert.ToInt32(gvPageWatchTags.DataKeys[gvPageWatchTags.SelectedIndex].Value));
        }

        //protected void dvPageWatch_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        //{

        //}

        //protected void dvPageWatch_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        //{
        //string itemName = "";
        //string itemDesc = "";
        //string itemThreshold = "";
        //string itemPriority = "";
        //string itemIsActive = "";

        //foreach (DetailsViewRow dvRow in dvPageWatch.Rows)
        //{
        //    TextBox name = (TextBox)dvRow.FindControl("txtName");
        //    itemName = name.Text;
        //    TextBox desc = (TextBox)dvRow.FindControl("txtDescription");
        //    itemDesc = desc.Text;
        //    DropDownList threshold = (DropDownList)dvRow.FindControl("ddlThreshold");
        //    DropDownList priority = (DropDownList)dvRow.FindControl("ddlPriority");
        //    DropDownList isActive = (DropDownList)dvRow.FindControl("ddlIsActive");
        //    if (priority.SelectedValue == "True")
        //    {
        //        itemPriority = "1";
        //    }
        //    else
        //    {
        //        itemPriority = "0";
        //    }
        //    if (threshold.SelectedValue == "True")
        //    {
        //        itemThreshold = "1";
        //    }
        //    else
        //    {
        //        itemThreshold = "0";
        //        //no priority without threshold
        //        itemPriority = "0";
        //    }                
        //    if (isActive.SelectedValue == "True")
        //    {
        //        itemIsActive = "1";
        //    }
        //    else
        //    {
        //        itemIsActive = "0";
        //    }
        //    break;
        //}

        //int messagetypenamecount = Convert.ToInt32(DataFunctions.ExecuteScalar("select count(MessageTypeID) from MessageTypes where upper(LTRIM(RTRIM(Name))) = '" + itemName.Trim().ToUpper() + "' and ChannelID = " + channelID.ToString()));
        //if (messagetypenamecount == 0)
        //{
        //    string sqlquery = "";
        //    if (itemPriority == "0")
        //    {
        //        sqlquery = "INSERT INTO MessageTypes(Name, Description, Threshold, Priority, IsActive, ChannelID) VALUES ('" +
        //            itemName + "','" + itemDesc + "'," + itemThreshold + "," + itemPriority + "," + itemIsActive + "," + channelID + ");SELECT @@IDENTITY ";
        //    }
        //    else
        //    {
        //        //put in default sort order
        //        sqlquery = "INSERT INTO MessageTypes(Name, Description, Threshold, Priority, IsActive, SortOrder, ChannelID) VALUES ('" +
        //        itemName + "','" + itemDesc + "'," + itemThreshold + "," + itemPriority + "," + itemIsActive + ", (select MAX(SortOrder) + 1 from MessageTypes where Priority = 1 and IsActive = 1 and ChannelID = " + channelID + ")," + channelID + ");SELECT @@IDENTITY ";
        //    }

        //    try
        //    {
        //        string messageTypeID = DataFunctions.ExecuteScalar(sqlquery).ToString();
        //        dvPageWatch.ChangeMode(DetailsViewMode.Insert);
        //        dvPageWatch.HeaderText = "Add Page Watch";
        //        LoadGridView();
        //        LoadDetailsView(0);
        //        divError.Visible = false;
        //        lblErrorMessage.Text = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        divError.Visible = true;
        //        lblErrorMessage.Text = "ERROR: Error Occured when creating Page Watch<br>" + ex.ToString();
        //    }
        //}
        //else
        //{
        //    divError.Visible = true;
        //    lblErrorMessage.Text = "ERROR : Page Watch Name must be unique.";
        //}
        //}

        //protected void dvPageWatch_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        //{

        //}

        //protected void dvPageWatch_ModeChanging(object sender, DetailsViewModeEventArgs e)
        //{
        //    if (e.CancelingEdit)
        //    {
        //        dvPageWatch.ChangeMode(DetailsViewMode.Insert);
        //        dvPageWatch.HeaderText = "Add Page Watch";
        //        divError.Visible = false;
        //        lblErrorMessage.Text = "";
        //    }
        //}

        protected void gvPageWatchTags_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                gvPageWatchTags.PageIndex = e.NewPageIndex;
            }
            LoadGridView();
        }

        protected void gvPageWatchTags_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.ToString() == ViewState["SortField"].ToString())
            {
                switch (ViewState["SortDirection"].ToString())
                {
                    case "ASC":
                        ViewState["SortDirection"] = "DESC";
                        break;
                    case "DESC":
                        ViewState["SortDirection"] = "ASC";
                        break;
                }
            }
            else
            {
                ViewState["SortField"] = e.SortExpression;
                ViewState["SortDirection"] = "ASC";
            }
            LoadGridView();
        }

        //protected void gvPageWatchTags_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    try
        //    {
        //        int pageWatchTagID = Convert.ToInt32(gvPageWatchTags.DataKeys[e.RowIndex].Values[0]);
        //        DeletePageWatchTag(pageWatchTagID);
        //    }
        //    catch { }
        //}

        public void gvPageWatchTags_Command(Object sender, DataGridCommandEventArgs e)
        {
            //try
            //{
            //    int pageWatchTagID = Convert.ToInt32(e.CommandArgument.ToString());
            //    if (e.CommandName == "View")
            //    {
            //        string test = "test";
            //    }
            //    else if (e.CommandName == "Delete")
            //    {
            //    DeletePageWatchTag(pageWatchTagID);
            //    }
            //}
            //catch { }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string errorMessage = ecn.communicator.classes.PageWatchTag.ValidateTag(txtTag.Text, ecn.communicator.classes.PageWatch.GetPageWatchURL(getPageWatchID()));
            if (errorMessage == string.Empty && ddlIsActive.SelectedValue == "1")
            {
                if (btnSave.Text == "Add")
                {
                    //insert pagewatch
                    string insertSQL = "insert into pagewatchtag (" +
                                            "Name, WatchTag, IsChanged, IsActive, DateAdded, AddedBy, PageWatchID) values (" +
                                            "@Name, @WatchTag, @IsChanged, @IsActive, GETDATE(), @CurrentUserID, @PageWatchID)";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = insertSQL;
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@WatchTag", txtTag.Text);
                    cmd.Parameters.AddWithValue("@IsChanged", "0");
                    if (ddlIsActive.SelectedValue == "1")
                    {
                        cmd.Parameters.AddWithValue("@IsActive", "true");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@IsActive", "false");
                    }
                    cmd.Parameters.AddWithValue("@CurrentUserID", Master.UserSession.CurrentUser.UserID);
                    cmd.Parameters.AddWithValue("@PageWatchID", getPageWatchID());
                    try
                    {
                        DataFunctions.Execute(cmd);
                        LoadGridView();
                        LoadActive();
                        SetDefaults();
                        btnSave.Text = "Add";
                    }
                    catch (Exception)
                    {
                        divError.Visible = true;
                        lblErrorMessage.Text = "ERROR : Page Watch Tag not added.";
                    }
                }
                else
                {
                    //update pagewatch where pagewatchid = lblPageWatch.Text 
                    string updateSQL = "update pagewatchtag set " +
                                            "name = @Name, Watchtag = @WatchTag, isactive = @IsActive, dateupdated = GETDATE(), " +
                                            "updatedby = @CurrentUserID, ischanged = @IsChanged where pagewatchtagid = @PageWatchtagID";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = updateSQL;
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@WatchTag", txtTag.Text);
                    cmd.Parameters.AddWithValue("@IsChanged", "0");
                    if (ddlIsActive.SelectedValue == "1")
                    {
                        cmd.Parameters.AddWithValue("@IsActive", "true");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@IsActive", "false");
                    }
                    cmd.Parameters.AddWithValue("@CurrentUserID", Master.UserSession.CurrentUser.UserID);
                    cmd.Parameters.AddWithValue("@PageWatchTagID", lblPageWatchTagID.Text);
                    try
                    {
                        DataFunctions.Execute(cmd);
                        LoadGridView();
                        LoadActive();
                        SetDefaults();
                        btnSave.Text = "Add";
                    }
                    catch (Exception)
                    {
                        divError.Visible = true;
                        lblErrorMessage.Text = "ERROR : Page Watch Tag not updated.";
                    }
                }
            }
            else
            {
                divError.Visible = true;
                lblErrorMessage.Text = "ERROR : " + errorMessage;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            SetDefaults();
            btnSave.Text = "Add";
        }        

        protected void gvPageWatchTags_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //divError.Visible = false;
            //lblErrorMessage.Text = "";

            //int pageWatchTagID = Convert.ToInt32(gvPageWatchTags.DataKeys[e.NewEditIndex].Values["PageWatchTagID"].ToString());
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandType = CommandType.Text;
            //cmd.CommandText = "select pwt.*, pw.URL from pagewatchtag pwt join PageWatch pw on pwt.PageWatchID = pw.PageWatchID where pwt.pagewatchtagid = @PageWatchTagID";
            //cmd.Parameters.AddWithValue("@PageWatchTagID", pageWatchTagID);
            //DataTable dtPageWatch = DataFunctions.GetDataTable(cmd);

            //ddlIsActive.SelectedIndex = -1;

            //lblPageWatchTagID.Text = pageWatchTagID.ToString();
            //txtName.Text = dtPageWatch.Rows[0]["Name"].ToString();
            //txtTag.Text = dtPageWatch.Rows[0]["WatchTag"].ToString();
            //if (Convert.ToBoolean(dtPageWatch.Rows[0]["IsActive"].ToString()))
            //{
            //    ddlIsActive.Items.FindByValue("1").Selected = true;
            //}
            //else
            //{
            //    ddlIsActive.Items.FindByValue("0").Selected = true;
            //}
            //FCKeditorPrevious.Text = dtPageWatch.Rows[0]["PreviousHTML"].ToString();
            //FCKeditorCurrent.Text = PageWatchTag.GetCurrentHTML(dtPageWatch.Rows[0]["WatchTag"].ToString(), dtPageWatch.Rows[0]["URL"].ToString());
            //pnlCompare.Visible = true;
            //btnSave.Text = "Update";
        }        

        protected void btnValidate_Click(object sender, EventArgs e)
        {
            //string currentHTML = string.Empty;
            //string tag = txtTag.Text.Trim();
            //if (tag.Length > 0)
            //{
            //    currentHTML = PageWatchTag.GetCurrentHTML(tag);
            //}
            //if (currentHTML.Length > 0)
            //{
            //    FCKeditorCurrent.Text = currentHTML;
            //}
            //else
            //{
            //    divError.Visible = true;
            //    lblErrorMessage.Text = "ERROR : Invalid Tag.";
            //}
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            SetupBlast();             
        }

        protected void btnIgnore_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateRecords("ignore");
                Response.Redirect("PageWatchEditor.aspx");
            }
            catch (Exception)
            {
                divError.Visible = true;
                lblErrorMessage.Text = "ERROR : Page Watch Tag and/or History not added/updated.";
            }            
        }

        private void UpdateRecords(string statusCode)
        {
            DataTable dtPageWatchTags = GetAllTags();
            foreach (DataRow row in dtPageWatchTags.Rows)
            {
                string currentHTML = PageWatchTag.GetCurrentHTML(row["WatchTag"].ToString(), row["URL"].ToString());

                //insert history record
                string insertSQL = "insert into pagewatchhistory (" +
                                        "PageWatchTagID, PreviousHTML, CurrentHTML, StatusCode, DateAdded, AddedBy) values (" +
                                        "@PageWatchTagID, @PreviousHTML, @CurrentHTML, @StatusCode, GETDATE(), @CurrentUserID)";
                SqlCommand insertCMD = new SqlCommand();
                insertCMD.CommandType = CommandType.Text;
                insertCMD.CommandText = insertSQL;
                insertCMD.Parameters.AddWithValue("@PageWatchTagID", Convert.ToInt32(row["PageWatchTagID"].ToString()));
                insertCMD.Parameters.AddWithValue("@PreviousHTML", row["PreviousHTML"].ToString());
                insertCMD.Parameters.AddWithValue("@CurrentHTML", currentHTML);
                insertCMD.Parameters.AddWithValue("@CurrentUserID", Master.UserSession.CurrentUser.UserID);
                insertCMD.Parameters.AddWithValue("@StatusCode", statusCode);

                //update page watch tag                
                string updateSQL = "update pagewatchtag set previoushtml = @PreviousHTML, ischanged = 0, datechanged = null where pagewatchtagid = @PageWatchTagID";
                SqlCommand updateCMD = new SqlCommand();
                updateCMD.CommandType = CommandType.Text;
                updateCMD.CommandText = updateSQL;
                updateCMD.Parameters.AddWithValue("@PreviousHTML", currentHTML);
                updateCMD.Parameters.AddWithValue("@PageWatchTagID", Convert.ToInt32(row["PageWatchTagID"].ToString()));

                DataFunctions.Execute(insertCMD);
                DataFunctions.Execute(updateCMD);
            }
        }

        private void SetupBlast()
        {
            //Response.Redirect("../blasts/blasteditor.aspx?PageWatchID=" + getPageWatchID());

            string sqlQuery =
                " SELECT pw.*, g.GroupName, l.LayoutName, l.ContentSlot1" +
                " FROM PageWatch pw" +
                " join Groups g on pw.GroupID = g.GroupID" +
                " join Layouts l on pw.LayoutID = l.LayoutID" +
                " WHERE pw.PageWatchID=" + getPageWatchID() + " ";
            DataTable dt = DataFunctions.GetDataTable(sqlQuery);

            ecn.communicator.classes.Wizard w = new ecn.communicator.classes.Wizard();
            w.ID = 0;
            w.WizardName = dt.Rows[0]["LayoutName"].ToString();
            w.EmailSubject = dt.Rows[0]["LayoutName"].ToString();
            w.FromName = "";
            w.FromEmail = "";
            w.ReplyTo = "";
            w.UserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
            w.ContentID = Convert.ToInt32(dt.Rows[0]["ContentSlot1"].ToString());
            w.GroupID = Convert.ToInt32(dt.Rows[0]["GroupID"].ToString());
            w.LayoutID = Convert.ToInt32(dt.Rows[0]["LayoutID"].ToString());
            w.CompletedStep = 0;
            w.PageWatchID = getPageWatchID();
            int wizardID = w.Save();

            Response.Redirect("../Wizard/SetupCampaign.aspx?WizardID=" + wizardID);

        }

        private DataTable GetAllTags()
        {
            DataTable dtPageWatchTags = null;

            int pageWatchID = getPageWatchID();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select pwt.pagewatchtagid, pwt.watchtag, pwt.previoushtml, pw.url from pagewatchtag pwt join pagewatch pw on pwt.pagewatchid = pw.pagewatchid where pwt.pagewatchid = @PageWatchID";
            cmd.Parameters.AddWithValue("@PageWatchID", pageWatchID);
            dtPageWatchTags = DataFunctions.GetDataTable(cmd);

            return dtPageWatchTags;
        }

        protected void gvPageWatchTags_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int pageWatchTagID = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "View")
            {
                ViewPageWatchTag(pageWatchTagID);
            }
            else if (e.CommandName == "Delete")
            {
                DeletePageWatchTag(pageWatchTagID);
            }
            else if (e.CommandName == "Edit")
            {
                EditPageWatchTag(pageWatchTagID);
            }
        }

        protected void gvPageWatchTags_RowDeleting1(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void btnAddTag_Click(object sender, EventArgs e)
        {            
            SetDefaults();
            btnSave.Text = "Add";
            pnlCompare.Visible = false;
            pnlEdit.Visible = true;
        }

        //protected void btnValidateTag_Click(object sender, EventArgs e)
        //{
        //    string currentHTML = string.Empty;
        //    string tag = txtTag.Text.Trim();
        //    if (tag != string.Empty)
        //    {
        //        currentHTML = PageWatchTag.GetCurrentHTML(tag);
        //    }
        //    if (currentHTML != string.Empty)
        //    {

        //    }
        //}

        
    }
}