using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using AjaxControlToolkit;
using KMPS_JF_Objects.Objects;
using System.Text;

namespace KMPS_JF_Setup.Publisher
{
    public partial class Pub_Newsletters : System.Web.UI.Page
    {
        JFSession jfsess = new JFSession();
        Publication pub = null;



        private int PubID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.QueryString["PubId"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int GroupID
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ViewState["GroupID"].ToString());
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ViewState["GroupID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            try
            {
                pub = Publication.GetPublicationbyID(PubID, string.Empty);
            }
            catch { }

            try
            {
                if (!IsPostBack)
                {
                    BoxPanel2.Title = "Manage Newsletters for " + Request.QueryString["PubName"] + ":";
                    LoadCustomer();
                    LoadQSValues();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void LoadQSValues()
        {
            int groupid = Convert.ToInt32(hfldGroupID.Value);
            string strQuery = " Select shortname as shortname from (" +
                       " Select ('CustomValue') as ShortName" +
                       " UNION Select ('EmailAddress') as ShortName" +
                       " UNION Select ('FirstName') as ShortName" +
                       " UNION Select ('LastName') as ShortName" +
                       " UNION Select ('FullName') as ShortName" +
                       " UNION Select ('Password') as ShortName" +
                       " UNION Select ('Title') as ShortName" +
                       " UNION Select ('Company') as ShortName" +
                       " UNION Select ('Occupation') as ShortName" +
                       " UNION Select ('Address') as ShortName" +
                       " UNION Select ('Address2') as ShortName" +
                       " UNION Select ('City') as ShortName" +
                       " UNION Select ('State') as ShortName" +
                       " UNION Select ('Zip') as ShortName" +
                       " UNION Select ('ZipPlus') as ShortName" +
                       " UNION Select ('Country') as ShortName" +
                       " UNION Select ('Voice') as ShortName" +
                       " UNION Select ('Mobile') as ShortName" +
                       " UNION Select ('Fax') as ShortName" +
                       " UNION Select ('Website') as ShortName" +
                       " UNION Select ('Age') as ShortName" +
                       " UNION Select ('Income') as ShortName" +
                       " UNION Select ('Gender') as ShortName" +
                       " UNION Select ('Birthdate') as ShortName";
            if (groupid > 0)
                strQuery = strQuery + " UNION select ShortName from groupdatafields where groupID = " + groupid + " and isnull(datafieldSetID,0) = 0)AS ECNDATAFIELD ";
            else
                strQuery = strQuery + ") AS ECNDATAFIELD ";

            DataTable dtFormECNTextField = DataFunctions.GetDataTable(strQuery, ConfigurationManager.ConnectionStrings["ecn5_communicator"].ConnectionString);
            if (dtFormECNTextField.Rows.Count > 0)
            {
                drpQSValue.DataTextField = "ShortName";
                drpQSValue.DataValueField = "ShortName";
                drpQSValue.DataSource = dtFormECNTextField;
                drpQSValue.DataBind();
                drpQSValue.Items.Insert(0, new ListItem("-Select-", "-Select-"));
                drpQSValue.Items.FindByValue("-Select-").Selected = true;
            }

        }

        protected void drpQSValue_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (drpQSValue.SelectedValue.Equals("CustomValue"))
            {
                txtQSValue.Visible = true;
            }
            else
            {
                txtQSValue.Visible = false;
            }
        }

        private void LoadCustomer()
        {
            SqlCommand cmdCustomer = new SqlCommand("Select CustomerID, CustomerName from Customer where customerID in (select items from dbo.fn_split(@AllowedCustomerIDs,','))");
            cmdCustomer.Parameters.Add(new SqlParameter("@AllowedCustomerIDs", jfsess.AllowedCustoemerIDs()));
            DataTable dtCustomer = DataFunctions.GetDataTable("accounts", cmdCustomer);

            ddlCustomer.DataSource = dtCustomer;
            ddlCustomer.DataBind();

            ddlCustomer.Items.Insert(0, new ListItem("Select Customer", ""));
            ddlCustomer.Items[0].Selected = true;
        }

        private void LoadAvailableGroups(int newsletterID, int customerID)
        {
            SqlCommand cmdAvailableGroups = new SqlCommand("spGetAvailableGroupsForNewsletters");
            cmdAvailableGroups.CommandType = CommandType.StoredProcedure;

            cmdAvailableGroups.Parameters.AddWithValue("@NewsletterID", newsletterID.ToString());
            cmdAvailableGroups.Parameters.AddWithValue("@CustomerIDs", jfsess.AllowedCustoemerIDs().ToString());

            DataTable dtAvailableGroups = DataFunctions.GetDataTable(cmdAvailableGroups);
            
            lstAvailableGroups.DataSource = dtAvailableGroups;
            lstAvailableGroups.DataTextField = "GroupName";
            lstAvailableGroups.DataValueField = "GroupID";
            lstAvailableGroups.DataBind();
        }

        private void LoadSelectedGroups(int newsletterID, int customerID)
        {
            SqlCommand cmdSelectedGroups = new SqlCommand("spGetSelectedGroupsForNewsletters");
            cmdSelectedGroups.CommandType = CommandType.StoredProcedure;

            cmdSelectedGroups.Parameters.AddWithValue("@NewsLetterID", newsletterID.ToString());
            cmdSelectedGroups.Parameters.AddWithValue("@CustomerID", customerID.ToString());

            DataTable dtSelectedGroups = DataFunctions.GetDataTable(cmdSelectedGroups);
            lstSelectedGroups.DataSource = dtSelectedGroups;
            lstSelectedGroups.DataTextField = "GroupName";
            lstSelectedGroups.DataValueField = "GroupID";
            lstSelectedGroups.DataBind();
        }

        protected void btnAddSelectedGroups_Click(object sender, EventArgs e)
        {
            MoveSelectedItems(lstAvailableGroups, lstSelectedGroups, false);
        }

        protected void btnRemoveSelectedGroups_Click(object sender, EventArgs e)
        {
            MoveSelectedItems(lstSelectedGroups, lstAvailableGroups, false);
        }

        private void MoveSelectedItems(ListBox source, ListBox target, bool moveAllItems)
        {
            for (int i = source.Items.Count - 1; i >= 0; i--)
            {
                ListItem item = source.Items[i];
                if (item.Selected)
                {
                    target.Items.Add(item);
                    item.Selected = false;
                    source.Items.Remove(item);
                }
            }
        }

        private void SaveNewsletterAutoSubscriptions(int newsletterID, int customerID)
        {
            if (newsletterID > 0)
            {
                string groupIDs = string.Empty;
                foreach (ListItem li in lstSelectedGroups.Items)
                {
                    groupIDs += li.Value + ",";
                }

                SqlCommand cmdNLAutoSubs = new SqlCommand("spSavePubNewsletterAutoSubscription");
                cmdNLAutoSubs.CommandType = CommandType.StoredProcedure;

                cmdNLAutoSubs.Parameters.AddWithValue("@NewsletterID", newsletterID.ToString());
                cmdNLAutoSubs.Parameters.AddWithValue("@CustomerID", customerID.ToString());
                cmdNLAutoSubs.Parameters.AddWithValue("@pnlGroupIDs", groupIDs);

                DataFunctions.Execute(cmdNLAutoSubs);
            }
        }

        protected void grdPublisherNewsletters_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                ExtURLQS = null;
                if (e.CommandName == "NewsletterEdit")
                {
                    
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblNewslettersID = row.FindControl("lblNewslettersID") as Label;
                    Label lblCategoryID = row.FindControl("lblCategoryID") as Label;
                    Label lblCustomerID = row.FindControl("lblCustomerID") as Label;
                    Label lblGroupID = row.FindControl("lblGroupID") as Label;
                    hfldNewsletterID.Value = lblNewslettersID.Text;
                    hfldGroupID.Value = lblGroupID.Text;
                    txtDisplayName.Text = row.Cells[2].Text.Replace("&amp;", "&");
                    RadEditorDescription.Content = ((Label)row.FindControl("lblDescription")).Text;
                    ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue(lblCategoryID.Text));

                    Label lblIsActive = (Label)row.FindControl("lblIsActive");
                    rbtlstIsActive.SelectedValue = (lblIsActive.Text.ToUpper() == "YES" ? "1" : "0");
                    Label lblDisplayName = (Label)row.FindControl("lblDisplayName");
                    rblstDisplayName.SelectedValue = (lblDisplayName.Text.ToUpper().Trim() == "YES" ? "1" : "0");

                    try
                    {
                        ddlCustomer.ClearSelection();
                        ddlCustomer.Items.FindByValue(lblCustomerID.Text).Selected = true;
                    }
                    catch
                    {
                    }

                    ddlCustomer.Enabled = false;
                    BoxPanel1.Title = "Edit Newsletter";
                    btnAdd.Text = "SAVE";
                    LoadQSValues();
                    LoadAvailableGroups(Convert.ToInt32(lblNewslettersID.Text), Convert.ToInt32(lblCustomerID.Text));
                    LoadSelectedGroups(Convert.ToInt32(lblNewslettersID.Text), Convert.ToInt32(lblCustomerID.Text));
                    loadHttpPostData(Convert.ToInt32(lblNewslettersID.Text));

                }
                else if (e.CommandName == "NewsletterDelete")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblNewslettersID = row.FindControl("lblNewslettersID") as Label;
                    hfldNewsletterID.Value = lblNewslettersID.Text;
                    SqlDataSourcePNewslettersConnect.SelectParameters["NewsletterID"].DefaultValue = hfldNewsletterID.Value;
                    SqlDataSourcePNewslettersConnect.SelectParameters["iMod"].DefaultValue = "3";
                    SqlDataSourcePNewslettersConnect.Select(DataSourceSelectArguments.Empty);

                    ClearData();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private DataTable ExtURLQS
        {
            get
            {
                try
                {
                    return (DataTable)ViewState["ExtURLQS"];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                ViewState["ExtURLQS"] = value;
            }
        }

        protected void btnAddHttpPostURL_Click(object sender, EventArgs e)
        {
            DataTable dt = ExtURLQS;
            if (dt == null)
            {
                dt = new DataTable();
                DataColumn HttpPostParamsID = new DataColumn("HttpPostParamsID", typeof(string));
                dt.Columns.Add(HttpPostParamsID);

                DataColumn ParamName = new DataColumn("ParamName", typeof(string));
                dt.Columns.Add(ParamName);

                DataColumn ParamValue = new DataColumn("ParamValue", typeof(string));
                dt.Columns.Add(ParamValue);

                DataColumn CustomValue = new DataColumn("CustomValue", typeof(string));
                dt.Columns.Add(CustomValue);
            }
            DataRow dr = dt.NewRow();
            dr["HttpPostParamsID"] = Guid.NewGuid();
            dr["ParamName"] = txtQSName.Text;
            dr["ParamValue"] = drpQSValue.SelectedValue;
            dr["CustomValue"] = txtQSValue.Text;
            dt.Rows.Add(dr);
            txtQSName.Text = "";
            txtQSValue.Text="";
            ExtURLQS = dt;
            gvHttpPost.DataSource = dt;
            gvHttpPost.DataBind();
        }

        private void loadHttpPostData(int newsLetterID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = " select  CONVERT(varchar(255), hpp.HttpPostParamsID) as HttpPostParamsID , hpp.ParamName,hpp.ParamValue, hpp.CustomValue, hp.URL from HttpPostParams hpp " +
                               " join HttpPost hp " +
                               " on hpp.HttpPostID=hp.HttpPostID " +
                               " join PubNewsletters pbn " +
                               " on pbn.NewsletterID=hp.EntityID " +
                               " where pbn.NewsletterID=@newsLetterID and hp.IsNewsLetter=1";
            cmd.Parameters.AddWithValue("@newsLetterID", newsLetterID);
            DataTable dt=DataFunctions.GetDataTable(cmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                ExtURLQS = dt;
                txtPostURL.Text = dt.Rows[0]["URL"].ToString();
                gvHttpPost.DataSource = dt;
                gvHttpPost.DataBind();
            }
            else
            {
                txtPostURL.Text = "";
                gvHttpPost.DataSource = null;
                gvHttpPost.DataBind();
            }
        }

        protected void gvHttpPost_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string HttpPostParamsID = e.CommandArgument.ToString();
                if (e.CommandName == "ParamDelete")
                {
                    DataTable dt = ExtURLQS;
                    var result = (from src in dt.AsEnumerable()
                                  where src.Field<string>("HttpPostParamsID") == HttpPostParamsID
                                  select src).ToList();
                    dt.Rows.Remove(result[0]);
                    ExtURLQS = dt;
                    gvHttpPost.DataSource = ExtURLQS;
                    gvHttpPost.DataBind();

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string groupDesc = RadEditorDescription.Content;

            try
            {
                groupDesc = groupDesc.Substring(0, 495);
            }
            catch
            {
                groupDesc = groupDesc.Substring(0, groupDesc.Length);
            }

            try
            {
                StringBuilder sbxmldata = new StringBuilder();
                if (ExtURLQS != null)
                {
                    foreach (DataRow dr in ExtURLQS.AsEnumerable())
                    {
                        if (!dr["ParamValue"].ToString().Equals("-Select-"))
                            sbxmldata.Append(string.Format("<HttpPostID ParamName=\"{0}\" ParamValue=\"{1}\" CustomValue=\"{2}\"/>", dr["ParamName"].ToString(), dr["ParamValue"].ToString(), dr["CustomValue"].ToString()));
                    }
                }
                if (!sbxmldata.ToString().Equals(string.Empty) && txtPostURL.Text.Equals(string.Empty))
                    throw new Exception("External Post URL cannot be empty if the Query string values are specified");

                if (sbxmldata.ToString().Equals(string.Empty) && !txtPostURL.Text.Equals(string.Empty))
                    throw new Exception("Query string values cannot be empty if the External Post URL is specified");

                if (hfldNewsletterID.Value == "")
                {
                    if (GroupID == 0)
                        GroupID = ECNUpdate.SaveGroup(0, txtDisplayName.Text, groupDesc, Convert.ToInt32(ddlCustomer.SelectedItem.Value));

                    SqlDataSourcePNewslettersConnect.SelectParameters["ECNGroupID"].DefaultValue = GroupID.ToString();
                    SqlDataSourcePNewslettersConnect.SelectParameters["AddedBy"].DefaultValue = jfsess.UserName();
                    SqlDataSourcePNewslettersConnect.SelectParameters["URL"].DefaultValue = txtPostURL.Text == string.Empty ? "0" : txtPostURL.Text ;
                    SqlDataSourcePNewslettersConnect.SelectParameters["qsNameValue"].DefaultValue = sbxmldata.ToString() == string.Empty ? "<HttpPostID></HttpPostID>" : sbxmldata.ToString();
                    SqlDataSourcePNewslettersConnect.SelectParameters["iMod"].DefaultValue = "1";
                    SqlDataSourcePNewslettersConnect.Select(DataSourceSelectArguments.Empty);

                    GroupID = 0;
                }
                else
                {
                    SqlDataSourcePNewslettersConnect.SelectParameters["NewsletterID"].DefaultValue = hfldNewsletterID.Value;
                    SqlDataSourcePNewslettersConnect.SelectParameters["ModifiedBy"].DefaultValue = jfsess.UserName();
                    SqlDataSourcePNewslettersConnect.SelectParameters["URL"].DefaultValue = txtPostURL.Text == string.Empty ? "0" : txtPostURL.Text;
                    SqlDataSourcePNewslettersConnect.SelectParameters["qsNameValue"].DefaultValue = sbxmldata.ToString() == string.Empty ? "<HttpPostID></HttpPostID>" : sbxmldata.ToString();
                    SqlDataSourcePNewslettersConnect.SelectParameters["iMod"].DefaultValue = "2";
                    SqlDataSourcePNewslettersConnect.Select(DataSourceSelectArguments.Empty);
                }

                try
                {
                    SaveNewsletterAutoSubscriptions(Convert.ToInt32(hfldNewsletterID.Value), Convert.ToInt32(ddlCustomer.SelectedItem.Value));
                }
                catch
                {
                    SaveNewsletterAutoSubscriptions(0, Convert.ToInt32(ddlCustomer.SelectedItem.Value));
                }

               

                ClearData();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void ClearData()
        {
            hfldNewsletterID.Value = "";
            hfldGroupID.Value = "";
            txtDisplayName.Text = "";
            RadEditorDescription.Content = "";
            ddlCategory.SelectedIndex = 0;
            rbtlstIsActive.SelectedIndex = 0;
            ExtURLQS = null;
            txtPostURL.Text = "";
            gvHttpPost.DataSource = ExtURLQS;
            gvHttpPost.DataBind();

            SqlDataSourcePNewslettersConnect.SelectParameters["NewsletterID"].DefaultValue = "0";
            SqlDataSourcePNewslettersConnect.SelectParameters["iMod"].DefaultValue = "4";

            BoxPanel1.Title = "Add Newsletter";
            lstAvailableGroups.Items.Clear();
            lstSelectedGroups.Items.Clear();
        }
    }
}
