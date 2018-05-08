using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using ECN_Framework_Common.Functions;
using KM.Common.Functions;
using CommonStringFunctions = KM.Common.StringFunctions;

namespace ecn.collector.main.Content
{
    public partial class ContentEditor :Page
    {
        private bool createAsNew
        {
            get { return Convert.ToBoolean(ViewState["createAsNew"]); }
            set { ViewState["createAsNew"] = value; }
        }

        private int getSurveyID()
        {
            if (Request.QueryString["SurveyID"] != null)
                return Convert.ToInt32(Request.QueryString["SurveyID"].ToString());
            else
                return 0;
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

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Collector.Enums.MenuCode.SURVEY; 
            phError.Visible = false;
            int customerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            int userID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;

            //FCKeditor1.ImagePreviewText.ViewPaths = new string[] { "/ecn.images/Customers/" + Master.UserSession.CurrentUser.CustomerID.ToString() + "/images" };
            //FCKeditor2.ImageManager.ViewPaths = new string[] { "/ecn.images/Customers/" + Master.UserSession.CurrentUser.CustomerID.ToString() + "/images" };
            if (!Page.IsPostBack)
            {
                LoadFoldersDR();
                LoadUsersDR();
                FCKeditor1.Text = "";
                FCKeditor2.Text = "";

                loadContent();
            }
            else
            {
                setShowPane();
            }
        }

        #region Form Prep
        private void presetdrpUserID()
        {
            drpUserID.ClearSelection();
            drpUserID.Items.FindByValue(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID.ToString()).Selected = true;
        }

        public void setShowPane()
        {
            panelContentSource.Visible = true;
            panelContentText.Visible = true;
            panelContentURL.Visible = false;
            panelContentFilePointer.Visible = false;
            GetTextButton.Visible = true;
        }

        public void LoadFoldersDR()
        {
            List<ECN_Framework_Entities.Communicator.Folder> folderList =
            ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.CNT.ToString(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            var result = (from src in folderList
                          orderby src.FolderName
                          select src).ToList();
            folderID.DataSource = result;
            folderID.DataBind();
            folderID.Items.Insert(0, "root");
            folderID.Items.FindByValue("root").Value = "0";
        }

        public void LoadUsersDR()
        {
            List<KMPlatform.Entity.User> userList =
            KMPlatform.BusinessLogic.User.GetByCustomerID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID);
        
            if (KM.Platform.User.IsSystemAdministrator(Master.UserSession.CurrentUser))
                userList.Add(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            
            var result = (from src in userList
                          orderby src.UserName
                          select src).ToList();
            drpUserID.DataSource = result;
            drpUserID.DataBind();
        }
        #endregion

        #region Data Load

        public void loadContent()
        {
            string SurveyLink = "http://www.ecn5.com/ecn.collector/front/default.aspx?sid=" + getSurveyID() + "&bid=%%blastid%%&uid=%%EmailAddress%%";
            ECN_Framework_Entities.Collector.Survey objSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(getSurveyID(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            string SurveyTitle = objSurvey.SurveyTitle + DateTime.Now.ToString();
            string csource = ReplaceAnchor(GetContent(SurveyTitle, SurveyLink));
            string ctext = ReplaceAnchor(GetTEXTContent(SurveyTitle, SurveyLink));

            string htmlSrc = csource;
            string mobileSrc = csource;
            htmlSrc = RegexUtilities.GetCleanUrlContent(htmlSrc);
            mobileSrc = mobileSrc.Replace("%23", "#");

            ContentTitle.Text = SurveyTitle;
            drpUserID.ClearSelection();

           // if (!ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.IsPlatformAdministrator)
            //{
                drpUserID.Items.FindByValue(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID.ToString()).Selected = true;
            //}
            
            FCKeditor1.Visible = true;
            FCKeditor2.Visible = true;
            pnlTxtMobile.Visible = false;
            pnlTxtSource.Visible = false;
            FCKeditor1.Text = htmlSrc;
            FCKeditor2.Text = mobileSrc;

            panelContentText.Visible = true;
            ContentText.Text = ctext + DateTime.Now.ToString();
            ContentURL.Text = "";
        }
        #endregion

        protected void btnConvertoInlineCSS_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder csource = new StringBuilder();
            if (FCKeditor1.Text.ToString().Length > 0)
                {
                    csource.Append(ECN_Framework_Common.Functions.StringFunctions.CleanString(FCKeditor1.Text.ToString()));
                }
                else
                {
                    csource.Append(ECN_Framework_Common.Functions.StringFunctions.CleanString(ContentText.Text));
                }
            this.FCKeditor1.Text = ECN_Framework_Common.Functions.StringFunctions.ConvertToInlineCSS(csource);
            
        }

        public void GetTextFromHTML(object sender, System.EventArgs e)
        {
            ContentText.Text = HtmlFunctions.StripTextFromHtml(FCKeditor1.Text);
        }

        public void Reset()
        {
            FCKeditor1.Text = "";
            FCKeditor2.Text = "";
            ContentText.Text = "";
            ContentTitle.Text = "";
            folderID.SelectedValue = "0";
            ContentURL.Text = "";

        }

        public void CreateSurveyBlast(object sender, System.EventArgs e)
        {
            try
            {
                ECN_Framework_Entities.Collector.Survey objSurvey = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(getSurveyID(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                ECN_Framework_Entities.Communicator.CampaignItem ci = new ECN_Framework_Entities.Communicator.CampaignItem();
                ECN_Framework_Entities.Communicator.Template Template = ECN_Framework_BusinessLayer.Communicator.Template.GetByNumberOfSlots(1, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                ECN_Framework_Entities.Communicator.Layout layout = new ECN_Framework_Entities.Communicator.Layout();
                layout.LayoutName = objSurvey.SurveyTitle + " - Blast Content " + DateTime.Now.ToString();
                layout.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
                layout.FolderID = 0;
                layout.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                layout.TemplateID = Template.TemplateID;
                layout.ContentSlot1 = CreateContent();
                ECN_Framework_BusinessLayer.Communicator.Layout.Save(layout, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

                ECN_Framework_Entities.Communicator.Campaign c = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignName("Marketing Campaign", ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                if (c == null || c.CampaignID <= 0)
                {
                    c = new ECN_Framework_Entities.Communicator.Campaign();
                    c.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID;
                    c.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                    c.CampaignName = "Marketing Campaign";
                    ECN_Framework_BusinessLayer.Communicator.Campaign.Save(c, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                }
                ci.CampaignID = c.CampaignID;
                ci.CampaignItemName = objSurvey.SurveyTitle + " - Campaign " + DateTime.Now.ToString();
                ci.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID;
                ci.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                ci.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
                ci.CampaignItemNameOriginal = objSurvey.SurveyTitle + " - Campaign " + DateTime.Now.ToString();
                ci.IsHidden = false;
                ci.CampaignItemFormatType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemFormatType.HTML.ToString();
                ci.CompletedStep = 1;
                ci.CampaignItemType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Regular.ToString();
                if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.ReportingBlastFields))
                    ci.BlastField1 = objSurvey.SurveyTitle + " - Campaign ";
                ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save(ci, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                Response.Redirect("/ecn.communicator/main/ecnwizard/wizardSetup.aspx?campaignItemid=" + ci.CampaignItemID + "&CampaignItemType=Regular&PrePopLayoutID=" + layout.LayoutID, true);
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
        }

        public int CreateContent()
        {
            string ctitle = ECN_Framework_Common.Functions.StringFunctions.CleanString(ContentTitle.Text) + " - Blast Content";
            string csource = string.Empty;
            string cmobile = string.Empty;
            csource = ECN_Framework_Common.Functions.StringFunctions.CleanString(FCKeditor1.Text.ToString());
            cmobile = ECN_Framework_Common.Functions.StringFunctions.CleanString(FCKeditor2.Text.ToString());

            csource = csource.Replace("<tbody>", "");
            csource = csource.Replace("</tbody>", "");
            csource = csource.Replace("<TBODY>", "");
            csource = csource.Replace("</TBODY>", "");
            csource = csource.Replace("<tbody>", "");
            csource = csource.Replace("</tbody>", "");
            cmobile = cmobile.Replace("<tbody>", "");
            cmobile = cmobile.Replace("</tbody>", "");
            cmobile = cmobile.Replace("<TBODY>", "");
            cmobile = cmobile.Replace("</TBODY>", "");
            cmobile = cmobile.Replace("<tbody>", "");
            cmobile = cmobile.Replace("</tbody>", "");
            csource = CommonStringFunctions.ReplaceAnchor(csource);
            cmobile = CommonStringFunctions.ReplaceAnchor(cmobile);

            string ctext = ECN_Framework_Common.Functions.StringFunctions.CleanString(ContentText.Text);
            ctext = CommonStringFunctions.ReplaceAnchor(ctext);

            if (ctext == string.Empty || ctext.Length == 0)
            {
                var cleanText = StringFunctions.CleanString(FCKeditor1.Text.ToString());
                ctext = HtmlFunctions.StripTextFromHtml(cleanText);
            }

            ECN_Framework_Entities.Communicator.Content content = new ECN_Framework_Entities.Communicator.Content();
            content.ContentTitle = ctitle;
            content.ContentTypeCode = "HTML";
            content.LockedFlag = "N";
            content.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            content.FolderID = 0;
            content.ContentSource = csource;
            content.ContentMobile = cmobile;
            content.ContentText = ctext;
            content.ContentURL = "";
            content.ContentFilePointer = "";
            content.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;
            content.Sharing = "N";
            content.IsValidated = ECN_Framework_BusinessLayer.Communicator.Content.ValidateHTMLContent(content.ContentSource);
            int contentID = ECN_Framework_BusinessLayer.Communicator.Content.Save(content,ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            return contentID;
        }


        private string GetContent(string SurveyTitle, string SurveyURL)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ConfigurationManager.AppSettings["SurveyBlast_HTMLContent"].ToString());

            sb = sb.Replace("%%SurveyTitle%%", SurveyTitle);
            sb = sb.Replace("%%SurveyLink%%", SurveyURL);

            return sb.ToString();
        }

        private string GetTEXTContent(string SurveyTitle, string SurveyURL)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ConfigurationManager.AppSettings["SurveyBlast_TEXTContent"].ToString());

            sb = sb.Replace("%%SurveyTitle%%", SurveyTitle);
            sb = sb.Replace("%%SurveyLink%%", SurveyURL);

            return sb.ToString();
        }


        private string ReplaceAnchor(string str)
        {
            Regex regExp_href_tags = new Regex("\\w+:\\/\\/[\\w@][\\w.:@]+\\/?[\\w\\.?=%&=\\-@/$#,]*");
            MatchCollection link_Collection = regExp_href_tags.Matches(str);
            foreach (Match m in link_Collection)
            {
                foreach (Group g in m.Groups)
                    if (g.Value.Length > 0 && g.Value.IndexOf("#") > 0)
                        str = str.Replace(g.Value, g.Value.Replace("#", "%23"));
            }
            return str;
        }
        
        protected void btnTxtMobilePreview_Click(object sender, EventArgs e)
        {
            if (!txtEditorMobile.Text.Equals(string.Empty))
            {
                lblPreview.Text = txtEditorMobile.Text;
                this.modalPopupPreview.Show();
            }
        }

        protected void btnTxtSourcePreview_Click(object sender, EventArgs e)
        {
            if (!txtEditorSource.Text.Equals(string.Empty))
            {
                lblPreview.Text = txtEditorSource.Text;
                this.modalPopupPreview.Show();
            }
        }

        protected void btnClosePreview_Hide(object sender, EventArgs e)
        {
            this.modalPopupPreview.Hide();
        }
    }
}