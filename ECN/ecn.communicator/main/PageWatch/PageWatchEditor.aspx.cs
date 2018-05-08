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
using System.Collections.Generic;

namespace ecn.communicator.main.PageWatch
{
    public partial class PageWatchEditor : ECN_Framework.WebPageHelper
    {
        int customerID = 0;
        int userID = 0;

        public static string accountsdb = ConfigurationManager.AppSettings["accountsdb"];
        public static string connString = ConfigurationManager.AppSettings["com"];

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.PAGEKNOWTICE; 
            Master.SubMenu = "";
            Master.Heading = "Page Watches";
            Master.HelpContent = "";
            Master.HelpTitle = "Page Watches";	

            divError.Visible = false;
            lblErrorMessage.Text = "";

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
                    LoadDropDowns();
                    SetDefaults();
                    btnSave.Text = "Add";
                }
            }
            else
            {
                Response.Redirect("default.aspx");
            }
        }

        private void LoadDropDowns()
        {
            LoadUsers();
            LoadGroups();
            LoadContent();
            LoadActive();
            LoadFreqType();
        }

        private void SetDefaults()
        {
            try
            {
                //divError.Visible = false;
                //lblErrorMessage.Text = "";
                lblPageWatchID.Text = "";
                txtName.Text = "";
                txtURL.Text = "";
                ddlUser.SelectedIndex = -1;
                ddlGroup.SelectedIndex = -1;
                ddlContent.SelectedIndex = -1;
                ddlFrequencyType.SelectedIndex = -1;
                ddlFrequencyNo.SelectedIndex = -1;
                ddlIsActive.SelectedIndex = -1;

                ddlUser.Items.FindByValue(userID.ToString()).Selected = true;
                ddlGroup.Items[0].Selected = true;
                ddlContent.Items[0].Selected = true;
                ddlFrequencyType.Items[0].Selected = true;
                LoadFreqNo();
                ddlFrequencyNo.Items[0].Selected = true;
                ddlIsActive.Items.FindByValue("1").Selected = true;
            }
            catch (Exception)
            {
            }
        }

        private void LoadUsers()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select UserID, UserName from ecn5_accounts..users where CustomerID = @CustomerID order by UserName ASC";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);

            ddlUser.DataTextField = "UserName";
            ddlUser.DataValueField = "UserID";
            ddlUser.DataSource = DataFunctions.GetDataTable(cmd);
            ddlUser.DataBind();
        }

        private void LoadGroups()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select GroupID, GroupName from Groups where CustomerID = @CustomerID order by GroupName ASC";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);

            ddlGroup.DataTextField = "GroupName";
            ddlGroup.DataValueField = "GroupID";
            ddlGroup.DataSource = DataFunctions.GetDataTable(cmd);
            ddlGroup.DataBind();
        }

        private void LoadContent()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select LayoutID, LayoutName from Layouts where CustomerID = @CustomerID order by LayoutName ASC";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);

            ddlContent.DataTextField = "LayoutName";
            ddlContent.DataValueField = "LayoutID";
            ddlContent.DataSource = DataFunctions.GetDataTable(cmd);
            ddlContent.DataBind();
        }

        private void LoadActive()
        {
            ddlIsActive.Items.Clear();
            ddlIsActive.Items.Add(new ListItem("YES", "1"));
            ddlIsActive.Items.Add(new ListItem("NO", "0"));
        }

        private void LoadFreqType()
        {
            ddlFrequencyType.Items.Clear();
            ddlFrequencyType.Items.Add(new ListItem("HR", "HR"));
            ddlFrequencyType.Items.Add(new ListItem("DAY", "DAY"));
            ddlFrequencyType.Items.Add(new ListItem("WEEK", "WEEK"));
            ddlFrequencyType.Items.Add(new ListItem("MONTH", "MONTH"));
        }

        private void LoadFreqNo()
        {
            ddlFrequencyNo.Items.Clear();
            switch (ddlFrequencyType.SelectedItem.ToString())
            {
                case "HR":
                    ddlFrequencyNo.Items.Add(new ListItem("4", "4"));
                    ddlFrequencyNo.Items.Add(new ListItem("8", "8"));
                    ddlFrequencyNo.Items.Add(new ListItem("12", "12"));
                    ddlFrequencyNo.Items.Add(new ListItem("16", "16"));
                    ddlFrequencyNo.Items.Add(new ListItem("20", "20"));
                    break;
                case "DAY":
                    for (int i = 1; i < 32; i++)
                    {
                        ddlFrequencyNo.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }
                    break;
                case "WEEK":
                    for (int i = 1; i < 53; i++)
                    {
                        ddlFrequencyNo.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }
                    break;
                case "MONTH":
                    for (int i = 1; i < 13; i++)
                    {
                        ddlFrequencyNo.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }
                    break;
                default:
                    break;
            }
        }

        private void LoadGridView()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetPageWatches";
            cmd.Parameters.AddWithValue("@CustomerID", customerID);

            DataTable dtPageWatch = DataFunctions.GetDataTable(cmd);

            DataView dvPageWatch = dtPageWatch.DefaultView;

            dvPageWatch.Sort = ViewState["SortField"].ToString() + ' ' + ViewState["SortDirection"].ToString();

            gvPageWatch.DataSource = dvPageWatch;

            try
            {
                gvPageWatch.DataBind();
            }
            catch
            {
                gvPageWatch.PageIndex = 0;
                gvPageWatch.DataBind();				
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

        public void DeletePageWatch(int pageWatchID)
        {
            string sqlquery = "delete PageWatchHistory where PageWatchTagID in (select PageWatchTagID from PageWatchTag where PageWatchID = " + pageWatchID + ");delete PageWatchTag where PageWatchID = " + pageWatchID + ";delete PageWatch where PageWatchID = " + pageWatchID;
            //string sqlquery =
            //    " DELETE FROM PageWatch " +
            //    " WHERE PageWatchID =" + pageWatchID;
            DataFunctions.Execute(sqlquery);

            LoadGridView();
 
        }

        protected void gvPageWatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Text = "Save";
            LoadDetails(Convert.ToInt32(gvPageWatch.DataKeys[gvPageWatch.SelectedIndex].Value));
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

        protected void gvPageWatch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0)
            {
                gvPageWatch.PageIndex = e.NewPageIndex;
            }
            LoadGridView();
        }

        protected void gvPageWatch_Sorting(object sender, GridViewSortEventArgs e)
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

        protected void gvPageWatch_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int pageWatchID = Convert.ToInt32(gvPageWatch.DataKeys[e.RowIndex].Values[0]);
                DeletePageWatch(pageWatchID);
            }
            catch { }
        }

        public void gvPageWatch_Command(Object sender, DataGridCommandEventArgs e)
        {
            try
            {
                int pageWatchID = Convert.ToInt32(e.CommandArgument.ToString());
                DeletePageWatch(pageWatchID);
            }
            catch { }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string errorMessage = ecn.communicator.classes.PageWatch.ValidateURL(txtURL.Text);
            if (ddlIsActive.SelectedValue == "1" && errorMessage != string.Empty)
            {
                divError.Visible = true;
                lblErrorMessage.Text = "ERROR : " + errorMessage;
            }
            else
            {
                if (btnSave.Text == "Add")
                {
                    //insert pagewatch
                    string insertSQL = "insert into pagewatch (" +
                                            "Name, URL, AdminUserID, GroupID, LayoutID, FrequencyType, FrequencyNo, " +
                                            "IsActive, DateAdded, AddedBy, CustomerID) values (" +
                                            "@Name, @URL, @UserID, @GroupID, @LayoutID, @FrequencyType, @FrequencyNo, " +
                                            "@IsActive, GETDATE(), @CurrentUserID, @CustomerID)";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = insertSQL;
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@URL", txtURL.Text);
                    cmd.Parameters.AddWithValue("@UserID", ddlUser.SelectedValue);
                    cmd.Parameters.AddWithValue("@GroupID", ddlGroup.SelectedValue);
                    cmd.Parameters.AddWithValue("@LayoutID", ddlContent.SelectedValue);
                    cmd.Parameters.AddWithValue("@FrequencyType", ddlFrequencyType.SelectedValue);
                    cmd.Parameters.AddWithValue("@FrequencyNo", ddlFrequencyNo.SelectedValue);
                    if (ddlIsActive.SelectedValue == "1")
                    {
                        cmd.Parameters.AddWithValue("@IsActive", "true");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@IsActive", "false");
                    }
                    cmd.Parameters.AddWithValue("@CurrentUserID", Master.UserSession.CurrentUser.UserID);
                    cmd.Parameters.AddWithValue("@CustomerID", Master.UserSession.CurrentUser.CustomerID);
                    try
                    {
                        DataFunctions.Execute(cmd);
                        LoadGridView();
                        LoadDropDowns();
                        SetDefaults();
                        btnSave.Text = "Add";
                    }
                    catch (Exception)
                    {
                        divError.Visible = true;
                        lblErrorMessage.Text = "ERROR : Page Watch not added.";
                    }
                }
                else
                {
                    //update pagewatch where pagewatchid = lblPageWatch.Text 
                    string updateSQL = "update pagewatch set " +
                                            "name = @Name, url = @URL, adminuserid = @UserID, groupid = @GroupID, " +
                                            "layoutid = @LayoutID, frequencytype = @FrequencyType, frequencyno = " +
                                            "@FrequencyNo, isactive = @IsActive, dateupdated = GETDATE(), " +
                                            "updatedby = @CurrentUserID where pagewatchid = @PageWatchID";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = updateSQL;
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@URL", txtURL.Text);
                    cmd.Parameters.AddWithValue("@UserID", ddlUser.SelectedValue);
                    cmd.Parameters.AddWithValue("@GroupID", ddlGroup.SelectedValue);
                    cmd.Parameters.AddWithValue("@LayoutID", ddlContent.SelectedValue);
                    cmd.Parameters.AddWithValue("@FrequencyType", ddlFrequencyType.SelectedValue);
                    cmd.Parameters.AddWithValue("@FrequencyNo", ddlFrequencyNo.SelectedValue);
                    if (ddlIsActive.SelectedValue == "1")
                    {
                        cmd.Parameters.AddWithValue("@IsActive", "true");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@IsActive", "false");
                    }
                    cmd.Parameters.AddWithValue("@CurrentUserID", Master.UserSession.CurrentUser.UserID);
                    cmd.Parameters.AddWithValue("@PageWatchID", lblPageWatchID.Text);
                    try
                    {
                        DataFunctions.Execute(cmd);
                        LoadGridView();
                        LoadDropDowns();
                        SetDefaults();
                        btnSave.Text = "Add";
                    }
                    catch (Exception)
                    {
                        divError.Visible = true;
                        lblErrorMessage.Text = "ERROR : Page Watch not updated.";
                    }
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            SetDefaults();
            btnSave.Text = "Add";
        }

        protected void ddlFrequencyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFreqNo();
        }

        protected void gvPageWatch_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int pageWatchID = Convert.ToInt32(gvPageWatch.DataKeys[e.NewEditIndex].Values["PageWatchID"].ToString());
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from pagewatch where pagewatchid = @PageWatchID";
            cmd.Parameters.AddWithValue("@PageWatchID", pageWatchID);
            DataTable dtPageWatch = DataFunctions.GetDataTable(cmd);

            ddlUser.SelectedIndex = -1;
            ddlGroup.SelectedIndex = -1;
            ddlContent.SelectedIndex = -1;
            ddlFrequencyType.SelectedIndex = -1;
            ddlFrequencyNo.SelectedIndex = -1;
            ddlIsActive.SelectedIndex = -1;

            lblPageWatchID.Text = pageWatchID.ToString();
            txtName.Text = dtPageWatch.Rows[0]["Name"].ToString();
            txtURL.Text = dtPageWatch.Rows[0]["URL"].ToString();
            ddlUser.Items.FindByValue(dtPageWatch.Rows[0]["AdminUserID"].ToString()).Selected = true;
            ddlGroup.Items.FindByValue(dtPageWatch.Rows[0]["GroupID"].ToString()).Selected = true;
            ddlContent.Items.FindByValue(dtPageWatch.Rows[0]["LayoutID"].ToString()).Selected = true;
            ddlFrequencyType.Items.FindByValue(dtPageWatch.Rows[0]["FrequencyType"].ToString()).Selected = true;
            LoadFreqNo();
            ddlFrequencyNo.Items.FindByValue(dtPageWatch.Rows[0]["FrequencyNo"].ToString()).Selected = true;
            if (Convert.ToBoolean(dtPageWatch.Rows[0]["IsActive"].ToString()))
            {
                ddlIsActive.Items.FindByValue("1").Selected = true;
            }
            else
            {
                ddlIsActive.Items.FindByValue("0").Selected = true;
            }
            btnSave.Text = "Update";
        }

        protected void btnCheckNow_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = string.Empty;
            try
            {
                List<ecn.communicator.classes.PageWatch> pwList = ecn.communicator.classes.PageWatch.GetPageRecordsForCustomer(Convert.ToInt32(Master.UserSession.CurrentUser.CustomerID.ToString()), ecn.communicator.classes.PageWatch.ScheduleType.Both, PageWatchTag.ChangeType.Unchanged);
                if (pwList.Count > 0)
                {
                    foreach (ecn.communicator.classes.PageWatch pw in pwList)
                    {
                        string errorMessage = PageWatchTag.ValidateAllTags(pw.PWT, pw.URL);
                        if (errorMessage == string.Empty)
                        {
                            foreach (ecn.communicator.classes.PageWatchTag pwt in pw.PWT)
                            {
                                string currentHTML = "";
                                currentHTML = PageWatchTag.GetCurrentHTML(pwt.WatchTag, pw.URL);
                                string previousHTML = pwt.PreviousHTML;
                                if (!ecn.communicator.classes.PageWatchTag.CompareHTML(previousHTML, currentHTML))
                                {
                                    ecn.communicator.classes.PageWatchTag.UpdateTagRecord(pwt.PageWatchTagID);
                                }

                            }
                        }
                        else
                        {
                            divError.Visible = true;
                            lblErrorMessage.Text = lblErrorMessage.Text + "ERROR : " + errorMessage + ". Page has been deactivated. ";
                            ecn.communicator.classes.PageWatch.SetInactive(pw.PageWatchID);
                        }
                    }
                }
                LoadGridView();
                LoadDropDowns();
                SetDefaults();
                btnSave.Text = "Add";
            }
            catch (Exception)
            {
                divError.Visible = true;
                lblErrorMessage.Text = "ERROR : ECN Experienced an error when checking for content changes.";
            }
        }

        //protected void dvPageWatch_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        //{
            //string itemName = "";
            //string itemDesc = "";
            //string itemThreshold = "";
            //string itemPriority = "";
            //string messageTypeID = "";
            //string itemIsActive = "";

            //messageTypeID = dvPageWatch.DataKey[0].ToString();


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

            //int messagetypenamecount = Convert.ToInt32(DataFunctions.ExecuteScalar("select count(MessageTypeID) from MessageTypes where upper(LTRIM(RTRIM(Name))) = '" + itemName.Trim().ToUpper() + "' and ChannelID = " + channelID.ToString() + " and MessageTypeID != " + messageTypeID));
            //if (messagetypenamecount == 0)
            //{

            //    string sqlquery = "";
            //    sqlquery = "UPDATE MessageTypes Set Name = '" + itemName + "', Description = '" + itemDesc + "', Threshold = " + itemThreshold + ", Priority = " + itemPriority + ", IsActive = " + itemIsActive +
            //        "WHERE MessageTypeID = " + messageTypeID + ";SELECT @@IDENTITY";

            //    if (itemPriority == "0")
            //    {
            //        sqlquery = "UPDATE MessageTypes Set Name = '" + itemName + "', Description = '" + itemDesc + "', Threshold = " + itemThreshold + ", Priority = " + itemPriority + ", IsActive = " + itemIsActive +
            //            ", SortOrder = NULL WHERE MessageTypeID = " + messageTypeID + ";SELECT @@IDENTITY";
            //    }
            //    else
            //    {
            //        //put in default sort order
            //        sqlquery = "UPDATE MessageTypes Set Name = '" + itemName + "', Description = '" + itemDesc + "', Threshold = " + itemThreshold + ", Priority = " + itemPriority + ", IsActive = " + itemIsActive +
            //            ", SortOrder = (select MAX(SortOrder) + 1 from MessageTypes where Priority = 1 and IsActive = 1 and ChannelID = " + channelID + ") WHERE MessageTypeID = " + messageTypeID + ";SELECT @@IDENTITY";
            //    }

            //    try
            //    {
            //        messageTypeID = DataFunctions.ExecuteScalar(sqlquery).ToString();
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
            //        lblErrorMessage.Text = "ERROR: Error Occured when updating Page Watch<br>" + ex.ToString();
            //    }
            //}
            //else
            //{
            //    divError.Visible = true;
            //    lblErrorMessage.Text = "ERROR : Page Watch Name must be unique.";
            //}
        //}
    }
}