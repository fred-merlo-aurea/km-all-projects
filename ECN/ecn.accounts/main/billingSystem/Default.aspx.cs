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
using ecn.common.classes;
using ecn.common.classes.billing;

using CommonFunctions = ECN_Framework_Common.Functions;
using AccountEntity = ECN_Framework_Entities.Accounts;
using AccountBLL = ECN_Framework_BusinessLayer.Accounts;

namespace ecn.accounts.main.billingSystem
{
    public partial class Default : ECN_Framework.WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.BILLINGSYSTEM;
            Master.HelpContent = "Billing Systems Help";
            Master.HelpTitle = "BILLING SYSTEMS MANAGER";

            if (!IsPostBack)
            {
                if (Master.UserSession.CurrentUser.IsKMStaff)
                {
                    txtStart.Text = DateTime.Now.AddDays(-15).ToString("MM/dd/yyyy");
                    txtEnd.Text = DateTime.Now.AddDays(+1).ToString("MM/dd/yyyy");
                    ViewState["SortField"] = "custname";
                    ViewState["SortDirection"] = "ASC";
                    LoadCreatedBy();
                    LoadAccountManager();
                    QuotesDataBind();
                }
                else
                {
                    Response.Redirect("../securityAccessError.aspx");
                }
            }

        }

        private void QuotesDataBind()
        {
            string newWhereClauses = string.Empty;

            if (Convert.ToInt32(ddlCreatedBy.SelectedValue) > 0)
                newWhereClauses = string.Format("s.staffID = {0}", ddlCreatedBy.SelectedValue);

            //if (Convert.ToInt32(ddlAccountManager.SelectedValue) > 0)
            //	 newWhereClauses += string.Format((newWhereClauses!=string.Empty?" AND ":"") + "q.AccountManagerID = {0}", ddlAccountManager.SelectedValue);

            if (WhereClause != string.Empty)
                newWhereClauses += string.Format((newWhereClauses != string.Empty ? " AND " : "") + "{0}", WhereClause);

            if (newWhereClauses != string.Empty)
                newWhereClauses += " and q.IsDeleted = 0";
            else
                newWhereClauses = " q.IsDeleted = 0";

            string strQuery = " select QuoteID,  'Q' + (case when q.customerID = -1 then '-0001' else REPLICATE('0', 4 - LEN(c.basechannelID)) + Convert(varchar,c.basechannelID) end )  + '_' + " +
                " (case when q.customerID = -1 then '-000001' else REPLICATE('0', 6 - LEN(q.CustomerID)) + Convert(varchar,q.CustomerID) end )  + '_' + " +
                " REPLICATE('0', 2 - LEN(month(q.CreatedDate))) + Convert(varchar,month(q.CreatedDate)) + " +
                "	REPLICATE('0', 2 - LEN(day(q.CreatedDate))) + Convert(varchar,day(q.CreatedDate)) + Convert(varchar,year(q.CreatedDate)) + '_' + convert(varchar,QuoteID)as QuoteCode  , " +
                "	q.customerID, case when c.customerID is null then Company else customerName end as custname, " +
                "	q.FirstName, q.lastname, q.Phone, q.Email, q.CreatedDate, q.ApproveDate , " +
                "	case when status = 0 then 'Pending' when status=1 then 'Approved' when status=2 then 'Denied' end as Status " +
                "	from [quote] q left outer join [Customer] c on q.customerID = c.customerID and c.IsDeleted = 0 " + 
                "  left outer join [Staff] s on s.userID = q.createdUserID " + 
                " where ";

            strQuery += newWhereClauses;
            DataTable dtQuote = DataFunctions.GetDataTable(strQuery);

            DataView dvQuote = dtQuote.DefaultView;
            dvQuote.Sort = ViewState["SortField"].ToString() + ' ' + ViewState["SortDirection"].ToString();

            dgQuotes.DataSource = dvQuote;
            dgQuotes.DataBind();

            QuotesPager.RecordCount = dtQuote.Rows.Count;
        }

        protected void dgQuotes_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            if (e.CommandName.ToLower() != "sort")
            {
                int quoteID = Convert.ToInt32(e.CommandArgument);
                Quote q = new Quote(quoteID);
                switch (e.CommandName)
                {
                    case "Edit":
                        //HttpRequestProcessor processor = new HttpRequestProcessor("QuoteDetail.aspx");
                        //processor.Add("CustomerID", q.Customer.ID);
                        //processor.Add("QuoteID", quoteID);
                        //Server.Transfer(processor.EncryptedHttpRequest);
                        Response.Redirect(string.Format("QuoteDetail.aspx?CustomerID={0}&QuoteID={1}", q.Customer.ID, quoteID));
                        break;
                    case "Delete":
                        q.Delete();
                        dgQuotes.CurrentPageIndex = 0;
                        QuotesPager.CurrentPage = 1;
                        QuotesPager.CurrentIndex = 0;
                        QuotesDataBind();
                        break;
                }
            }
        }

        protected void dgQuotes_OnItemCreated(object source, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Footer)
            {
                return;
            }

            LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
            if (btnDelete != null)
            {
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            }
        }

        protected void dgQuotes_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.ToLower() == ViewState["SortField"].ToString().ToLower())
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

            QuotesDataBind();
        }
        protected void QuotesPager_IndexChanged(object sender, System.EventArgs e)
        {
            QuotesDataBind();
        }


        private string WhereClause
        {
            get
            {
                QuoteStatusEnum status = (QuoteStatusEnum)Convert.ToInt32(ddlQuoteStatus.SelectedValue);
                CustomerTypeEnum ctype = (CustomerTypeEnum)Convert.ToInt32(ddlCustomerType.SelectedValue);
                DateTime start, end;
                try
                {
                    start = Convert.ToDateTime(txtStart.Text);
                }
                catch (Exception)
                {
                    start = DateTimeInterpreter.MinValue;
                    txtStart.Text = "";
                }

                try
                {
                    end = Convert.ToDateTime(txtEnd.Text);
                }
                catch (Exception)
                {
                    end = DateTimeInterpreter.MaxValue;
                    txtEnd.Text = "";
                }

                return QuoteQueryBuilder.GetWhereClause(status, ctype, start, end);
            }
        }

        private void LoadCreatedBy()
        {
            ddlCreatedBy.DataSource = Staff.GetStaff();
            ddlCreatedBy.DataTextField = "FullName";
            ddlCreatedBy.DataValueField = "ID";
            ddlCreatedBy.DataBind();

            ddlCreatedBy.Items.Insert(0, new ListItem("ALL Staff", "0"));
            ddlCreatedBy.Items.FindByValue("0").Selected = true;

            if (Staff.CurrentStaff != null)
            {
                try
                {
                    ddlCreatedBy.ClearSelection();
                    ddlCreatedBy.Items.FindByValue(Staff.CurrentStaff.ID.ToString()).Selected = true;
                }
                catch
                { }
            }
        }

        private void LoadAccountManager()
        {
            ddlAccountManager.DataSource = Staff.GetStaffByRole(StaffRoleEnum.AccountManager);
            ddlAccountManager.DataTextField = "FullName";
            ddlAccountManager.DataValueField = "ID";
            ddlAccountManager.DataBind();

            ddlAccountManager.Items.Insert(0, new ListItem("ALL Account Manager", "0"));
            ddlAccountManager.Items.FindByValue("0").Selected = true;

            if (Staff.CurrentStaff != null)
            {
                if (Staff.CurrentStaff.IsRole(StaffRoleEnum.AccountExecutive))
                {
                    ddlAccountManager.Enabled = false;
                }
            }
        }

        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            lblQuote.Text = string.Format("{0} quotes for {1}", ddlQuoteStatus.SelectedItem.Text, ddlCustomerType.SelectedItem.Text.ToLower());
            dgQuotes.CurrentPageIndex = 0;
            QuotesPager.CurrentPage = 1;
            QuotesPager.CurrentIndex = 0;
            QuotesDataBind();
        }

        protected void ddlAccountManager_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            dgQuotes.CurrentPageIndex = 0;
            QuotesPager.CurrentPage = 1;
            QuotesPager.CurrentIndex = 0;
            QuotesDataBind();
        }

        protected void ddlCreatedBy_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            dgQuotes.CurrentPageIndex = 0;
            QuotesPager.CurrentPage = 1;
            QuotesPager.CurrentIndex = 0;
            QuotesDataBind();
        }
    }
}
