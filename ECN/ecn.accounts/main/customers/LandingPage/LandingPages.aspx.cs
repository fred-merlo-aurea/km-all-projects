//using System;
//using System.Collections;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Web;
//using System.Web.SessionState;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.HtmlControls;
//using System.IO;
//using System.Text;
//using System.Collections.Generic;

//using SecurityAccess = ECN_Framework.Common.SecurityAccess;

//namespace ecn.accounts.customersmanager
//{
//    public partial class LandingPages : ECN_Framework.WebPageHelper, ILandingPageWizard
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CUSTOMERS;
//            Master.SubMenu = "landing pages";
//            lblErrorMessage.Text = "";
//            phError.Visible = false;

//            //if (KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser))
//            //{
//                //int requestLPAID = getLPAID();

//                if (Page.IsPostBack == false)
//                {
//                    BindBaseChannels();
//                    //if (requestLPAID > 0)
//                    //{
//                    //    btnSave.Text = "Update";
//                    //}
//                }
//            //}
//            //else
//            //{
//            //    Response.Redirect(".../default.aspx");
//            //}
//        }

//        private void BindBaseChannels()
//        {
//            List<ECN_Framework_Entities.Accounts.BaseChannel> bcList = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
//            ddlBaseChannel.DataSource = bcList;
//            ddlBaseChannel.DataBind();
//            ddlBaseChannel.Items.FindByValue(Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString()).Selected = true;
//            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
//            {
//                ddlBaseChannel.Enabled = true;
//            }
//            else
//            {
//                ddlBaseChannel.Enabled = false;
//            }
//            BindCustomers();
//        }

//        private void BindCustomers()
//        {
//            List<ECN_Framework_Entities.Accounts.Customer> custList = ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(Convert.ToInt32(ddlBaseChannel.SelectedValue));
//            ddlCustomer.DataSource = custList;
//            ddlCustomer.DataBind();
//            if (KM.Platform.User.IsChannelAdministrator(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser))
//            {
//                ddlCustomer.SelectedIndex = 0;
//                ddlCustomer.Enabled = true;
//            }
//            else
//            {
//                ddlCustomer.Items.FindByValue(Master.UserSession.CurrentCustomer.CustomerID.ToString()).Selected = true;
//                ddlCustomer.Enabled = false;
//            }
//        }

//        private void BindPages()
//        {
//            List<ECN_Framework_Entities.Accounts.LandingPage> pageList = ECN_Framework_BusinessLayer.Accounts.LandingPage.GetAll();
//            ddlPage.DataSource = pageList;
//            ddlPage.DataBind();
//            ddlPage.SelectedIndex = 0;
//            LoadUserControl();
//        }

//        private void LoadUserControl()
//        {
//            switch (ddlPage.SelectedValue)
//            {
//                case "Unsubscribe":
//                    //load user control
//                    Unsubscribe1.Visible = true;
//                    break;
//                default:
//                    break;
//            }
//        }

//        protected void ddlBaseChannel_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            BindCustomers();
//        }

//        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            LoadUserControl();
//        }

//        protected void ddlPage_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            LoadUserControl();
//        }
//    }
//}