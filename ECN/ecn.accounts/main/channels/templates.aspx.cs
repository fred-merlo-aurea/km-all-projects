using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Linq;

namespace ecn.communicator.channelsmanager
{

    public partial class templates : ECN_Framework.WebPageHelper
    {
        public int thumbnailSize = 50;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CHANNELS;  

            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                int ChannelID = getChannelID();
                if (ChannelID > 0)
                {
                    loadTemplatesGrid();
                }
            }
            else
            {
                Response.Redirect("~/main/securityAccessError.aspx");
            }

            if (!IsPostBack)
            {
                phError.Visible = false;
                lblErrorMessage.Text = "";
                loadCategories();
            }
        }

        private void loadCategories()
        {
            ddlCategoryFilter.Items.Clear();
            List<ECN_Framework_Entities.Communicator.Template> templateList = ECN_Framework_BusinessLayer.Communicator.Template.GetByBaseChannelID(Convert.ToInt32(Master.UserSession.CurrentCustomer.BaseChannelID), Master.UserSession.CurrentUser).GroupBy(x => x.Category).Select(x => x.First()).ToList();

            templateList.RemoveAll(x => string.IsNullOrWhiteSpace(x.Category));

            ddlCategoryFilter.DataSource = templateList.OrderBy(x=>x.Category);
            ddlCategoryFilter.DataBind();
            ddlCategoryFilter.Items.Insert(0, "All");
        }

        public void ddlCategoryFilter_IndexChanged(object sender, EventArgs e)
        {
            DropDownList dllCategory = (DropDownList) sender;
            if (dllCategory.SelectedIndex != 0)
            {
                loadTemplatesGrid(dllCategory.SelectedItem.Text);
            }
            else
            {
                loadTemplatesGrid();
            }
        }

        #region get request vars
        private int getChannelID()
        {
            int theChannelID = 0;
            try
            {
                theChannelID = Convert.ToInt32(Request.QueryString["ChannelID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theChannelID;
        }

        private int getTemplateID()
        {
            int theTemplateID = 0;
            try
            {
                theTemplateID = Convert.ToInt32(Request.QueryString["TemplateID"].ToString());
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            return theTemplateID;
        }
        #endregion

        private void loadTemplatesGrid(string category = null)
        {
            List<ECN_Framework_Entities.Communicator.Template> template = ECN_Framework_BusinessLayer.Communicator.Template.GetByBaseChannelID(getChannelID(), Master.UserSession.CurrentUser);

            if (category != null)
            {
                template.RemoveAll(x => x.Category != category);
            }
            
            var query = from t in template
                        select new {t.TemplateID, t.TemplateStyleCode, t.SlotsTotal, t.IsActive,
                                    thumb = System.Configuration.ConfigurationManager.AppSettings["Accounts_VirtualPath"] + "/includes/thumbnail.aspx?size=" + thumbnailSize + "&image=" + t.TemplateImage.Replace("/ecn.images", ""), 
                                    Descr = "<b>" + t.TemplateName + "</b><br>" + t.TemplateDescription, ChannelID = t.BaseChannelID};
            
            grdTemplates.DataSource = query.ToList();
            grdTemplates.DataBind();

            //ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

            //string sqlquery =
            //    " SELECT TemplateID, TemplateImage, SlotsTotal, " +
            //    " '" + System.Configuration.ConfigurationManager.AppSettings["Communicator_VirtualPath"] + "/includes/thumbnail.aspx?size=" + thumbnailSize + "&image='+replace(TemplateImage,'/ecn.images','') AS Thumb, " +
            //    " ActiveFlag, LEFT('<b>'+TemplateName+'</b><br>'+TemplateDescription,80) AS Descr, TemplateStyleCode " +
            //    " FROM " + @ConfigurationManager.AppSettings["communicatordb"] + ".dbo.Templates " +
            //    " WHERE ChannelID=" + theChannelID;
            //TemplatesGrid.DataSource = ecn.common.classes.DataFunctions.GetDataTable(sqlquery);
            //TemplatesGrid.DataBind();
        }

        protected void grdTemplates_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void grdTemplates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "DELETE")
            {
                deleteTemplate(Convert.ToInt32(e.CommandArgument.ToString()));
            }
        }

        protected void grdTemplates_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTemplates.PageIndex = e.NewPageIndex;
            loadTemplatesGrid();
        }

        public void deleteTemplate(int templateID)
        {
            phError.Visible = false;
            lblErrorMessage.Text = "";
            try
            {
                ECN_Framework_BusinessLayer.Communicator.Template.Delete(templateID, Master.UserSession.CurrentUser);
                loadTemplatesGrid();
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnEX)
            {
                setECNError(ecnEX);
            }
            
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            phError.Visible = true;
            lblErrorMessage.Text = "";
            foreach (ECN_Framework_Common.Objects.ECNError ecnError in ecnException.ErrorList)
            {
                lblErrorMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.ErrorMessage;
            }
        }
    }
}
