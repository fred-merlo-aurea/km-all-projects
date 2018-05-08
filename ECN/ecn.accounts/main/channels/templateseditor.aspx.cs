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
using System.Text;
using ActiveUp.WebControls;


namespace ecn.communicator.channelsmanager
{
    public partial class templateseditor : ECN_Framework.WebPageHelper
    {
        private int ChannelID
        {
            get
            {
                if (Request.QueryString["ChannelID"] != null)
                    return Convert.ToInt32(Request.QueryString["ChannelID"].ToString());
                else
                    return -1;
            }
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Accounts.Enums.MenuCode.CHANNELS;
            lblErrorMessage.Text = "";
            phError.Visible = false;
           // TemplateSource.ImageManager.ViewPaths = new string[] { "/ecn.images/Customers/" + Master.UserSession.CurrentUser.CustomerID.ToString() + "/images" };
            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
            {
                ECN_Framework_BusinessLayer.Application.ECNSession es = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

                if (Page.IsPostBack == false)
                {

                    ddlBaseChannelID.Enabled = true;

                    int requestTemplateID = getTemplateID();

                    ddlTemplateStyleCode.DataSource = ECN_Framework_BusinessLayer.Accounts.Code.GetByCodeType(ECN_Framework_Common.Objects.Accounts.Enums.CodeType.TemplateStyle, Master.UserSession.CurrentUser);
                    ddlTemplateStyleCode.DataBind();

                    loadBaseChannelDropDown();

                    if (requestTemplateID > 0)
                    {
                        LoadFormData(requestTemplateID);
                        SetUpdateInfo(requestTemplateID);
                    }
                    else
                    {
                        ddlBaseChannelID.Items.FindByValue(Master.UserSession.CurrentBaseChannel.BaseChannelID.ToString()).Selected = true;
                    }
                    //set images for wysiwyg object
                    string ImagesWebPath = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentCustomer.CustomerID + "/images/";
                    string ImagesFilePath = Server.MapPath(ImagesWebPath);

                    //ActiveUp.WebControls.HtmlTextBox.Tools.Image imageLibrary = (ActiveUp.WebControls.HtmlTextBox.Tools.Image)TemplateSource.Toolbars[0].Tools["Image"];
                    //imageLibrary.AutoLoad = true;
                    //imageLibrary.Directories.Add("Images", ImagesFilePath, ImagesWebPath);

                    // Create a new code snippets list
                    ////ActiveUp.WebControls.HtmlTextBox.Tools.CodeSnippets snipLibrary = (ActiveUp.WebControls.HtmlTextBox.Tools.CodeSnippets)TemplateSource.Toolbars[0].Tools["CodeSnippets"];
                    //snipLibrary.Items.Add("Content Slot1", "%%slot1%%");
                    //snipLibrary.Items.Add("Content Slot2", "%%slot2%%");
                    //snipLibrary.Items.Add("Content Slot3", "%%slot3%%");
                    //snipLibrary.Items.Add("Content Slot4", "%%slot4%%");
                    //snipLibrary.Items.Add("Content Slot5", "%%slot5%%");
                    //snipLibrary.Items.Add("Content Slot6", "%%slot6%%");
                    //snipLibrary.Items.Add("Unsubscribe Link", "%%unsubscribelink%%");
                }
            }
            else
            {
                Response.Redirect("~/main/securityAccessError.aspx");
            }

            if (!IsPostBack)
            {
                LoadCategories();
            }
            else
            {
                modalPopupCreateCategory.Hide();
                modalPopupExistingCategory.Hide();
            }
            
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

        #region Form Prep

        public void loadBaseChannelDropDown()
        {
            ddlBaseChannelID.DataSource = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
            ddlBaseChannelID.DataBind();
        }

        private void SetUpdateInfo(int setTemplateID)
        {
            tbTemplateID.Text = setTemplateID.ToString();
            btnSave.Text = "Update";
            imgPreview.Visible = true;
        }

        #endregion

        #region Data Load
        private void LoadFormData(int setTemplateID)
        {
            ECN_Framework_Entities.Communicator.Template template = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID(setTemplateID, Master.UserSession.CurrentUser);

            if (!IsPostBack)
            {
                if (template != null && !string.IsNullOrWhiteSpace(template.Category))
                {
                    txtNewCategoy.Text = template.Category;
                }    
            }

            string tSource = "";

            tSource = template.TemplateSource;
            tSource = tSource.Replace("<TBODY>", "");
            tSource = tSource.Replace("</TBODY>", "");

            tbTemplateName.Text = template.TemplateName;
            ddlTemplateStyleCode.Items.FindByValue(template.TemplateStyleCode).Selected = true;
            
            TemplateSource.Text = tSource;
            tbTemplateText.Text = template.TemplateText;
            tbTemplateImage.Text = template.TemplateImage;
            imgPreview.ImageUrl = template.TemplateImage;
            tbTemplateDescription.Text = template.TemplateDescription;
            ddlSlotsTotal.Items.FindByValue(template.SlotsTotal.ToString()).Selected = true;
            ddlBaseChannelID.Items.FindByValue(template.BaseChannelID.ToString()).Selected = true;
            if (template.IsActive.Value)
            {
                cbActiveFlag.Checked = true;
            }
        }
        #endregion

        #region Data Handlers

        public void btnSave_click(object sender, System.EventArgs e)
        {
            ECN_Framework_Entities.Communicator.Template template = new ECN_Framework_Entities.Communicator.Template
            {
                TemplateID = getTemplateID(),
                IsActive = cbActiveFlag.Checked ? true : false,
                TemplateName = ECN_Framework_Common.Functions.StringFunctions.CleanString(tbTemplateName.Text),
                TemplateDescription = ECN_Framework_Common.Functions.StringFunctions.CleanString(tbTemplateDescription.Text),
                Category = !string.IsNullOrWhiteSpace(txtNewCategoy.Text) ? txtNewCategoy.Text : null
            };

            string tsource =  ECN_Framework_Common.Functions.StringFunctions.CleanString(TemplateSource.Text);

            tsource = tsource.Replace("<TBODY>", "");
            tsource = tsource.Replace("</TBODY>", "");
            template.TemplateSource = tsource;
            template.TemplateText = ECN_Framework_Common.Functions.StringFunctions.CleanString(tbTemplateText.Text);
            template.TemplateStyleCode = ddlTemplateStyleCode.SelectedItem.Value;
            template.TemplateImage = tbTemplateImage.Text;
            template.BaseChannelID = Convert.ToInt32(ddlBaseChannelID.SelectedItem.Value);
            template.SlotsTotal = Convert.ToInt32(ddlSlotsTotal.SelectedItem.Value);

            if (template.TemplateID > 0)
                template.UpdatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
            else
                template.CreatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);

            try
            {
                ECN_Framework_BusinessLayer.Communicator.Template.Save(template, Master.UserSession.CurrentUser);
            }
            catch (ECN_Framework_Common.Objects.ECNException ecnex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (ECN_Framework_Common.Objects.ECNError err in ecnex.ErrorList)
                {
                    sb.Append(err.ErrorMessage + "<BR>");
                }
                lblErrorMessage.Text = sb.ToString();
                phError.Visible = true;
                return;
            }

            Response.Redirect("templates.aspx?ChannelID=" + ddlBaseChannelID.SelectedItem.Value);
        }

        #endregion

        protected void btnNewCategory_Click(object sender, EventArgs e)
        {
            this.modalPopupCreateCategory.Show();
        }
        protected void btnExistingCategory_Click(object sender, EventArgs e)
        {
            this.modalPopupExistingCategory.Show();
        }
        protected void btnNewRule_Click(object sender, EventArgs e)
        {
            this.modalPopupCreateCategory.Show();
        }

        protected void btnCategoryEditorSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtInputCategory.Text))
            {
                txtNewCategoy.Text = txtInputCategory.Text;
                this.txtInputCategory.Text = string.Empty;
                this.modalPopupCreateCategory.Hide();
            }
        }

        protected void btnCategoryEditorClose_Click(object sender, EventArgs e)
        {
            this.txtInputCategory.Text = string.Empty;
            this.modalPopupCreateCategory.Hide();
        }

        protected void btnExitingCategoryOk_Click(object sender, EventArgs e)
        {
            txtNewCategoy.Text = drpCategory.SelectedItem.Text;
            this.modalPopupExistingCategory.Hide();
        }

        protected void btnExistingCategoryClose_Click(object sender, EventArgs e)
        {
            drpCategory.SelectedValue = "-Select-";
            this.modalPopupExistingCategory.Hide();
        }

        private void LoadCategories(int baseChannelID = -1)
        {
            drpCategory.Items.Clear();
            List<ECN_Framework_Entities.Communicator.Template> templateList = new List<ECN_Framework_Entities.Communicator.Template>();
            if (ChannelID > 0)
            {
                templateList = ECN_Framework_BusinessLayer.Communicator.Template.GetByBaseChannelID(ChannelID, Master.UserSession.CurrentUser).GroupBy(x => x.Category).Select(x => x.First()).ToList();
            }
            else
            {
                if(baseChannelID > 0)
                {
                    templateList = ECN_Framework_BusinessLayer.Communicator.Template.GetByBaseChannelID(baseChannelID, Master.UserSession.CurrentUser).GroupBy(x => x.Category).Select(x => x.First()).ToList();
                }
                else
                    templateList = ECN_Framework_BusinessLayer.Communicator.Template.GetByBaseChannelID(Convert.ToInt32(Master.UserSession.CurrentCustomer.BaseChannelID), Master.UserSession.CurrentUser).GroupBy(x => x.Category).Select(x => x.First()).ToList();
            }
            templateList.RemoveAll(x => string.IsNullOrWhiteSpace(x.Category));

            drpCategory.DataSource = templateList;
            drpCategory.DataBind();
            drpCategory.Items.Insert(0, "-Select-");
        }

        protected void ddlBaseChannelID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBaseChannelID.SelectedIndex > 0)
            {
                LoadCategories(Convert.ToInt32(ddlBaseChannelID.SelectedValue.ToString()));
            }
        }
    }
}
