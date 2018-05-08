using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECN.Common.Helpers;

namespace ecn.publisher.main.Edition
{
    public partial class AddLinks : ECN_Framework_BusinessLayer.Application.WebPageHelper
    {
        private int CurrentPage
        {
            get
            {
                if (ViewState["CurrentPage"] == null)
                    return 1;
                else
                    return (int)ViewState["CurrentPage"];
            }

            set { ViewState["CurrentPage"] = value; }
        }

        private int Pages
        {
            get { return ViewStateHelper.GetFromViewState(ViewState, nameof(Pages), 1); }
            set { ViewStateHelper.SetViewState(ViewState, nameof(Pages), value); }
        }

        private int CustomerID
        {
            get { return ViewStateHelper.GetFromViewState(ViewState, nameof(CustomerID), 1); }
            set { ViewStateHelper.SetViewState(ViewState, nameof(CustomerID), value); }
        }

        private int getEditionID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["EditionID"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Publisher.Enums.MenuCode.EDITION;
            Master.SubMenu = "Manage Links";
            Master.Heading = "Add/Edit Links";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            lblErrorMessage.Visible = false;

            grdLinks.EmptyDataText = "No links exists in this page";
            //script for Marquee tool
            Page.ClientScript.RegisterClientScriptInclude("prototype", ResolveUrl("~/script/prototype.js"));
            Page.ClientScript.RegisterClientScriptInclude("prototypereduced", ResolveUrl("~/script/prototype_reduced.js"));
            Page.ClientScript.RegisterClientScriptInclude("scriptaculous", ResolveUrl("~/script/scriptaculous.js?load=effects"));
            Page.ClientScript.RegisterClientScriptInclude("scriptaculous", ResolveUrl("~/script/effects.js"));
            Page.ClientScript.RegisterClientScriptInclude("rectmarquee", ResolveUrl("~/script/rectmarquee.js"));
            Page.ClientScript.RegisterClientScriptInclude("previewtt", ResolveUrl("~/script/previewtt.js"));

            if (!IsPostBack)
            {
                ECN_Framework_Entities.Publisher.Edition ed = ECN_Framework_BusinessLayer.Publisher.Edition.GetByEditionID(getEditionID(), Master.UserSession.CurrentUser);
                ECN_Framework_Entities.Publisher.Publication pb = ECN_Framework_BusinessLayer.Publisher.Publication.GetByPublicationID(ed.PublicationID, Master.UserSession.CurrentUser);

                CustomerID = ed.CustomerID.Value;
                Pages = ed.Pages;

                for (int i = 1; i <= Pages; i++)
                {
                    ddlPageNo.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }

                CurrentPage = 1;

                LoadImage();               
            }
        }

        private void LoadImage()
        {
            Reset();
            lblCurrentPage.Text = CurrentPage.ToString();
            ddlPageNo.ClearSelection();
            ddlPageNo.Items.FindByValue(CurrentPage.ToString()).Selected = true;

            ImgDE.ImageUrl = ConfigurationManager.AppSettings["Image_DomainPath"] + "/Customers/" + CustomerID.ToString() + "/Publisher/" + getEditionID().ToString() + "/618/" + CurrentPage + ".jpg";

            List<ECN_Framework_Entities.Publisher.Page> page = ECN_Framework_BusinessLayer.Publisher.Page.GetByEditionID(getEditionID(), CurrentPage.ToString(), Master.UserSession.CurrentUser, true);

            var query = (from p in page
                            from l in p.LinkList
                           select new { l.LinkID, l.LinkURL, l.Alias, x = (618 * l.x1) / page[0].Height, y = (618 * l.y1) / page[0].Height, w = (618 * (l.x2 - l.x1)) / page[0].Height, h = (618 * (l.y2 - l.y1)) / page[0].Height });

            grdLinks.DataSource = query.ToList();
            grdLinks.DataBind();
        }
           
        private void Reset()
        {
            lblLinkID.Text = "0";
            tb_x.Text = "";
            tb_y.Text = "";
            tb_w.Text = "";
            tb_h.Text = "";
            tblinkAlias.Text = "";
            tblink.Text = "";
            btnSave.Text = "Add Link";
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            if (CurrentPage != 1)
                CurrentPage--;

            LoadImage();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Pages)
                CurrentPage++;

            LoadImage();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (tb_x.Text.Trim().Length > 0 && tb_y.Text.Trim().Length > 0 && tb_w.Text.Trim().Length > 0 && tb_h.Text.Trim().Length > 0 && tblink.Text.Trim().Length > 0)
            {

                List<ECN_Framework_Entities.Publisher.Page> page = ECN_Framework_BusinessLayer.Publisher.Page.GetByEditionID(getEditionID(), CurrentPage.ToString(), Master.UserSession.CurrentUser, true);

                ECN_Framework_Entities.Publisher.Link link = new ECN_Framework_Entities.Publisher.Link();

                double x1, y1, x2, y2;

                x1 = Convert.ToDouble(tb_x.Text);
                y1 = Convert.ToDouble(tb_y.Text);
                x2 = Convert.ToDouble(tb_x.Text) + Convert.ToDouble(tb_w.Text);
                y2 = Convert.ToDouble(tb_y.Text) + Convert.ToDouble(tb_h.Text);

                x1 = (Convert.ToDouble(page[0].Height) * x1) / 618.00;
                y1 = (Convert.ToDouble(page[0].Height) * y1) / 618.00;
                x2 = (Convert.ToDouble(page[0].Height) * x2) / 618.00;
                y2 = (Convert.ToDouble(page[0].Height) * y2) / 618.00;

                //Response.Write("x, y, w, h" + tb_x.Text + "," + tb_y.Text + "," + tb_w.Text + "," + tb_h.Text);

                link.LinkID = Convert.ToInt32(lblLinkID.Text);
                link.PageID = page[0].PageID;
                link.LinkType = "URI";
                link.LinkURL = tblink.Text.Replace("'", "''");
                link.x1 = Convert.ToInt32(x1);
                link.y1 = Convert.ToInt32(y1);
                link.x2 = Convert.ToInt32(x2);
                link.y2 = Convert.ToInt32(y2);
                link.Alias = tblinkAlias.Text.Replace("'", "''");
                link.CustomerID = page[0].CustomerID;

                if (link.LinkID > 0)
                    link.UpdatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
                else
                    link.CreatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);

                try
                {
                    ECN_Framework_BusinessLayer.Publisher.Link.Save(link, Master.UserSession.CurrentUser, true);
                }
                catch (ECN_Framework_Common.Objects.ECNException ecnex)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (ECN_Framework_Common.Objects.ECNError err in ecnex.ErrorList)
                    {
                        sb.Append(err.ErrorMessage + "<BR>");
                    }
                    lblErrorMessage.Text = sb.ToString();
                    return;
                }

                btnReset_Click(sender, e);

                LoadImage();
            }
        }

        protected void ibedit_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "editlink")
            {
                List<ECN_Framework_Entities.Publisher.Page> page = ECN_Framework_BusinessLayer.Publisher.Page.GetByEditionID(getEditionID(), CurrentPage.ToString(), Master.UserSession.CurrentUser, true);

                List<ECN_Framework_Entities.Publisher.Link> link = page[0].LinkList;

                var query = (from l in link
                             where l.LinkID == Convert.ToInt32(e.CommandArgument)
                             select new { l.LinkID, l.LinkURL, l.Alias, x = (618 * l.x1) / page[0].Height, y = (618 * l.y1) / page[0].Height, w = (618 * (l.x2 - l.x1)) / page[0].Height, h = (618 * (l.y2 - l.y1)) / page[0].Height });

                lblLinkID.Text = query.ToList()[0].LinkID.ToString();
                tblink.Text = query.ToList()[0].LinkURL.ToString();
                tblinkAlias.Text = query.ToList()[0].Alias.ToString();

                tb_x.Text = query.ToList()[0].x.ToString();
                tb_y.Text = query.ToList()[0].y.ToString();
                tb_w.Text = query.ToList()[0].w.ToString();
                tb_h.Text = query.ToList()[0].h.ToString();
                btnSave.Text = "Update Link";
            }
        }

        protected void ibdelete_Command(object sender, CommandEventArgs e)
        {
            try
            {
                ECN_Framework_BusinessLayer.Publisher.Link.Delete(Convert.ToInt32(e.CommandArgument.ToString()), Master.UserSession.CurrentUser);
                LoadImage();
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (ECN_Framework_Common.Objects.ECNError err in ecnex.ErrorList)
                {
                    sb.Append(err.ErrorMessage + "<BR>");
                }
                lblErrorMessage.Text = sb.ToString();
                lblErrorMessage.Visible = true;
            }
        }

        protected void ddlPageNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = Convert.ToInt32(ddlPageNo.SelectedItem.Value);
            LoadImage();
        }
    }
}
