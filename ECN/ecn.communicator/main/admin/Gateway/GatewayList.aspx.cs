using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ecn.communicator.main.admin.Gateway
{
    public partial class GatewayList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            Master.SubMenu = "";
            Master.Heading = "";
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.CUSTOMER;
            
            if(!Page.IsPostBack)
            {
                //if (!KM.Platform.User.IsChannelAdministrator(Master.UserSession.CurrentUser) && !Master.UserSession.CurrentUser.IsAdmin)
                if (false == KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
                {
                    pnlNoAccess.Visible = true;
                    pnlMain.Visible = false;
                    Label1.Text = "You do not have access to this page because you are not an Administrator.";
                    return;
                }
                else
                {
                    pnlMain.Visible = true;
                    pnlNoAccess.Visible = false;
                    LoadData();
                }
            }
        }

        private void LoadData()
        {
            List<ECN_Framework_Entities.Communicator.Gateway> gateways = ECN_Framework_BusinessLayer.Communicator.Gateway.GetByCustomerID(Master.UserSession.CurrentCustomer.CustomerID);
            if (gateways != null && gateways.Count > 0)
            {
                gvGateways.Visible = true;
                gvGateways.DataSource = gateways.OrderBy(x => x.GatewayID).ToList();
                gvGateways.DataBind();
            }
            else
                gvGateways.Visible = false;
        }

        protected void gvGateways_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Equals("editgateway"))
            {
                Response.Redirect("GatewaySetup.aspx?GatewayID=" + e.CommandArgument);
            }
            else if(e.CommandName.ToLower().Equals("deletegateway"))
            {
                ECN_Framework_Entities.Communicator.Gateway gateway = ECN_Framework_BusinessLayer.Communicator.Gateway.GetByGatewayID(Convert.ToInt32(e.CommandArgument.ToString()));
                gateway.IsDeleted = true;
                ECN_Framework_BusinessLayer.Communicator.Gateway.Save(gateway, Master.UserSession.CurrentUser);
                LoadData();
            }
        }

        protected void gvGateways_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                ECN_Framework_Entities.Communicator.Gateway current = (ECN_Framework_Entities.Communicator.Gateway)e.Row.DataItem;

                ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEditGateway");
                ImageButton imgbtnDelete = (ImageButton)e.Row.FindControl("imgbtnDeleteGateway");
                Label lblURL = (Label)e.Row.FindControl("lblURLForGateway");

                imgbtnEdit.CommandArgument = current.GatewayID.ToString();
                imgbtnDelete.CommandArgument = current.GatewayID.ToString();
                if(current.RedirectURL.Contains("https://"))
                    lblURL.Text = "https://gateway.knowledgemarketing.com?PubCode=" + current.PubCode.ToUpper() + "&TypeCode=" + current.TypeCode.ToUpper();
                else
                    lblURL.Text = "http://gateway.knowledgemarketing.com?PubCode=" + current.PubCode.ToUpper() + "&TypeCode=" + current.TypeCode.ToUpper();
            }
        }

        protected void btnAddNewGateway_Click(object sender, EventArgs e)
        {
            Response.Redirect("GatewaySetup.aspx");
        }
    }
}