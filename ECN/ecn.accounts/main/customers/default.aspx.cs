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
using System.Linq;
using System.Collections.Generic; 
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Functions;


namespace ecn.accounts.customersmanager
{
    public partial class customers_main : WebPageHelper
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CUSTOMERS;
            Master.SubMenu = "customers list";
            Master.Heading = "";
            Master.HelpContent = "Customers Help";
            Master.HelpTitle = "CUSTOMERS MANAGER";

            if (!Page.IsPostBack)
            {

                if (false == KM.Platform.User.IsSystemAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
                {
                    Response.Redirect("~/main/securityAccessError.aspx");
                }

                if(!KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                    ddlBaseChannels.Enabled = false;

                loadBaseChannels();
                
                loadCustomers();
            }
        }

        public void loadBaseChannels()
        {
            List<ECN_Framework_Entities.Accounts.BaseChannel> bcList = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
            var result = (from src in bcList
                         orderby src.BaseChannelName
                         select new
                         {
                             BaseChannelID= src.BaseChannelID,
                             BaseChannelName = src.BaseChannelName
                         }).ToList() ;
            ddlBaseChannels.DataSource = result;
            ddlBaseChannels.DataBind();
            ddlBaseChannels.Items.FindByValue(Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString()).Selected = true;
        }

        


        public void ddlBaseChannels_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            
            loadCustomers();
        }

        public void ddlChannels_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            loadCustomers();
        }

        private void loadCustomers()
        {
            List<ECN_Framework_Entities.Accounts.Customer> lCustomers = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(int.Parse(ddlBaseChannels.SelectedItem.Value));
            List<KMPlatform.Entity.User> lUsers = KMPlatform.BusinessLogic.User.GetUsersByChannelID(int.Parse(ddlBaseChannels.SelectedItem.Value));

            var cust = from c in lCustomers
                    let cCount =
                    (
                        from u in lUsers
                        where c.CustomerID == u.CustomerID && u.IsActive
                        select u
                    ).Count()
                       /*where (c.CommunicatorChannelID == int.Parse(ddlChannels.SelectedItem.Value) ||
                                                    c.CollectorChannelID == int.Parse(ddlChannels.SelectedItem.Value) ||
                                                    c.CreatorChannelID == int.Parse(ddlChannels.SelectedItem.Value) ||
                                                    c.PublisherChannelID == int.Parse(ddlChannels.SelectedItem.Value) ||
                                                    c.CharityChannelID == int.Parse(ddlChannels.SelectedItem.Value)
                                                    )*/
                    orderby c.CustomerName
                    select new { c.CustomerID, c.CustomerName, c.CreatedDate, c.ActiveFlag, c.DemoFlag, UsersCount = cCount } 
                    ;
    
            grdCustomers.DataSource = cust;
            grdCustomers.DataBind();




            //string channelName = ddlChannels.SelectedItem.Text;
            //string sqlquery =
            //    " SELECT CustomerID, CustomerName, CreateDate, ActiveFlag, DemoFlag, " +
            //    " UsersCount = (SELECT COUNT(UserID) FROM Users WHERE CustomerID = c.CustomerID AND ActiveFlag ='Y') " +
            //    " FROM Customer c" +
            //    " where baseChannelID = " + ddlBaseChannels.SelectedItem.Value;

            //if (channelName.EndsWith("communicator)"))
            //{
            //    sqlquery += " and CommunicatorChannelID= " + ddlChannels.SelectedItem.Value.ToString();
            //}
            //else if (channelName.EndsWith("collector)"))
            //{
            //    sqlquery += " and CollectorChannelID= " + ddlChannels.SelectedItem.Value.ToString();
            //}
            //else if (channelName.EndsWith("creator)"))
            //{
            //    sqlquery += " and CreatorChannelID= " + ddlChannels.SelectedItem.Value.ToString();
            //}
            //else if (channelName.EndsWith("publisher)"))
            //{
            //    sqlquery += " and PublisherChannelID= " + ddlChannels.SelectedItem.Value.ToString();
            //}
            //else if (channelName.EndsWith("charity)"))
            //{
            //    sqlquery += " and CharityChannelID= " + ddlChannels.SelectedItem.Value.ToString();
            //}

            //sqlquery += " ORDER BY CustomerName ";

            if (!Master.UserSession.CurrentUser.IsKMStaff)
            {
                grdCustomers.Columns[7].Visible = false;
                grdCustomers.Columns[8].Visible = false;
            }
        }

        

        #region Gridview Item Commands

        protected void grdCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;

            // Make sure we aren't in header/footer rows
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string activeStatus = e.Row.Cells[2].Text.ToString();

                int  userCount = Convert.ToInt32(grdCustomers.DataKeys[e.Row.RowIndex].Values["UsersCount"]);

                LinkButton loginBtn = e.Row.FindControl("LoginBtn") as LinkButton;

                if (activeStatus.Equals("N"))
                {
                    //loginBtn.Enabled = false;
                    loginBtn.Text = "N/A";
                    loginBtn.Attributes.Add("style", "cursor:hand;padding:0;margin:0;");
                    loginBtn.Attributes.Add("onclick", "alert('Customer NOT ACTIVE.');return false;");
                }
                else if (userCount < 1)
                {
                    loginBtn.Text = "N/U";
                    loginBtn.Attributes.Add("style", "cursor:hand;padding:0;margin:0;");
                    loginBtn.Attributes.Add("onclick", "alert('No ACTIVE User.');return false;");
                }
                else
                {
                    loginBtn.Text = "<a href='customerlogin.aspx?customerID=" + grdCustomers.DataKeys[e.Row.RowIndex].Values["CustomerID"].ToString() + "'><center>Login</center></a>";
                    loginBtn.Attributes.Add("onclick", "window.location.href('customerlogin.aspx?customerID=" + grdCustomers.DataKeys[e.Row.RowIndex].Values["CustomerID"].ToString() + "')");
                }

                return;
            }
        }

        #endregion

        #region DataHandlers
        public void DeleteCustomer(int theCustomerID)
        {
            //ECN_Framework_BusinessLayer.Accounts.Customer.Delete(theCustomerID);
            ECN_Framework_BusinessLayer.Accounts.Customer.ClearCustomersCache_ByChannelID(Int32.Parse(ddlBaseChannels.SelectedValue));
            loadCustomers();
        }
        
        #endregion

        protected void grdCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

    }
}
