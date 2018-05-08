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

namespace KMPS_JF_Setup.Publisher
{
    public partial class Pub_Events : System.Web.UI.Page
    {
        JFSession jfsess = new JFSession();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                    BoxPanel2.Title = "Manage Events for " + Request.QueryString["PubName"] + ":";
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void grdPublisherEvents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EventsEdit")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblEventID = row.FindControl("lblEventID") as Label;
                    hfldEventId.Value = lblEventID.Text;

                    txtDisplayName.Text = Server.HtmlDecode(row.Cells[1].Text);

                    txtDescription.Text = grdPublisherEvents.DataKeys[Convert.ToInt32(row.RowIndex)].Values["EventDesc"].ToString().Trim(); //locationId is invisible column

                    ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByText(row.Cells[3].Text));
                    txtURL.Text = grdPublisherEvents.DataKeys[Convert.ToInt32(row.RowIndex)].Values["EventURL"].ToString().Trim();
                    Label lblStartDate = row.FindControl("lblStartDate") as Label;
                    Label lblEndDate = row.FindControl("lblEndDate") as Label;
                    txtStartDate.Text = (lblStartDate.Text == "1/1/1753" ? "" : lblStartDate.Text);
                    txtEndDate.Text = (lblEndDate.Text == "1/1/1753" ? "" : lblEndDate.Text);
                    txtEventTime.Text = Server.HtmlDecode(row.Cells[6].Text);
                    txtLocation.Text = Server.HtmlDecode(row.Cells[2].Text);


                    BoxPanel1.Title = "Edit Event";
                    btnAddEvent.Text = "SAVE";


                    Label lblIsActive = (Label)row.FindControl("lblIsActive");
                    rbtlstIsActive.SelectedValue = (lblIsActive.Text.ToUpper() == "YES" ? "1" : "0");

                }
                else if (e.CommandName == "EventsDelete")
                {
                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    Label lblEventID = row.FindControl("lblEventID") as Label;
                    hfldEventId.Value = lblEventID.Text;
                    SqlDataSourcePEventConnect.SelectParameters["EventID"].DefaultValue = hfldEventId.Value;
                    SqlDataSourcePEventConnect.SelectParameters["iMod"].DefaultValue = "3";
                    SqlDataSourcePEventConnect.Select(DataSourceSelectArguments.Empty);
                                        
                    ClearData();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnAddEvent_Click(object sender, EventArgs e)
        {
            try
            {
                if (hfldEventId.Value == "")
                {
                    SqlDataSourcePEventConnect.SelectParameters["AddedBy"].DefaultValue = jfsess.UserName();
                    SqlDataSourcePEventConnect.SelectParameters["iMod"].DefaultValue = "1";
                    SqlDataSourcePEventConnect.Select(DataSourceSelectArguments.Empty);
                }
                else
                {
                    SqlDataSourcePEventConnect.SelectParameters["EventId"].DefaultValue = hfldEventId.Value;
                    SqlDataSourcePEventConnect.SelectParameters["ModifiedBy"].DefaultValue = jfsess.UserName();
                    SqlDataSourcePEventConnect.SelectParameters["iMod"].DefaultValue = "2";
                    SqlDataSourcePEventConnect.Select(DataSourceSelectArguments.Empty);
                }
                ClearData();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void ClearData()
        {
            hfldEventId.Value = "";
            txtDisplayName.Text = "";
            txtDescription.Text = "";
            ddlType.SelectedIndex = 0;
            rbtlstIsActive.SelectedIndex = 0;
            txtURL.Text = "";
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            txtEventTime.Text = "";            
            txtLocation.Text = "";

            SqlDataSourcePEventConnect.SelectParameters["EventId"].DefaultValue = "0";           
            SqlDataSourcePEventConnect.SelectParameters["iMod"].DefaultValue = "4";


            BoxPanel1.Title = "Add Event";
            btnAddEvent.Text = "SAVE";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
        }        
    }
}
