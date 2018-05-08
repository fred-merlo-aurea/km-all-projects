using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Objects;
using HtmlAgilityPack;
using CommonStringFunctions = KM.Common.StringFunctions;

namespace ecn.communicator.main.ECNWizard.Content
{
    public partial class layoutEditorNew : System.Web.UI.UserControl
    {
        bool createAsNew = false;
        protected System.Web.UI.WebControls.Button CreateAsNewDownButton;
        KMPlatform.Entity.User user = null;
        string defaultBorder = "border=1 bordercolor=black width=600 cellpadding=0 cellspacing=0";
        string CompanyAddress = "";
        int templateSlots = 0;
        int templateID = 0;

        public string TemplateID
        {
            set
            {
                int totaltemplates = templaterepeater.Items.Count;
                int theindex = 0;
                for (int i = 0; i < totaltemplates; i++)
                {
                    string currid = ((TextBox)templaterepeater.Items[i].FindControl("TemplateID")).Text;
                    if (currid == value)
                    {
                        theindex = i;
                    }
                }
                templaterepeater.SelectedIndex = theindex;
                loadTemplates();

                if (templaterepeater.SelectedItem != null)
                {
                    templateSlots = Convert.ToInt32(((TextBox)templaterepeater.SelectedItem.FindControl("SlotsTotal")).Text);    
                }
                
            }
            get
            {
                if (templaterepeater.SelectedIndex > 0)
                {
                    return ((TextBox)templaterepeater.SelectedItem.FindControl("TemplateID")).Text;
                }
                else
                {
                    if (templaterepeater.Items.Count > 0)
                        return ((TextBox)templaterepeater.Items[0].FindControl("TemplateID")).Text;
                    else
                        return null;
                }
            }
        }

        protected override bool OnBubbleEvent(object sender, EventArgs e)
        {
            try
            {
                string source = sender.ToString();
                if (source.Equals("ContentSelected"))
                {
                    int contentID = contentExplorer1.selectedContentID;
                    this.modalPopupExistingContent.Hide();
                    updateContentDetail(contentID);
                    contentExplorer1.Reset();
                    upMain.Update();
                }
            }
            catch { }
            return true;
        }

        private int getLayoutID()
        {
            if (Request.QueryString["LayoutID"] != null)
                return Convert.ToInt32(Request.QueryString["LayoutID"].ToString());
            else
                return 0;
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            //if (KM.Platform.User.IsAdministratorOrHasUserPermission(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.blastpriv, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.viewreport))
            //if (KMPlatform.BusinessLogic.User.HasPermission(
            //    ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, "blastpriv") 
            //|| KMPlatform.BusinessLogic.User.HasPermission(
            //    ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, "viewreport") 
            //|| ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentKM.Platform.User.IsChannelAdministrator(user) || ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentKM.Platform.User.IsSystemAdministrator(user) || ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.IsAdmin)
            //if (KM.Platform.User.IsAdministratorOrHasUserPermission(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Manage_Campaigns, ECN_Framework_Common.Objects.Accounts.Enums.UserPermission.Delivery_Report))
            if (KM.Platform.User.HasAccess(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.Edit))				
            {
                user = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
                if (!(Page.IsPostBack))
                {
                    checkDynamicContent();
                    contentExplorer1.enableSelectMode();
                    loadTemplates();
                    ToggleSlots();
                    lblMessageType.Visible = ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SetMessagePriority);
                    ddlMessageType.Visible = ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SetMessagePriority);
                    BindDropDowns();

                    templateSlots = 1;
                    ECN_Framework_Entities.Accounts.Customer customer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, false);
                    CompanyAddress = customer.Address + " " + customer.City + ", " + customer.State + " " + customer.Zip;
                    DisplayAddress.Text = CompanyAddress;

                    if (getLayoutID() > 0)
                    {
                        LoadFormData(getLayoutID());
                        checkBorder();
                    }
                    else
                    {
                        if (templaterepeater.Items.Count > 0)
                        {
                            templaterepeater.SelectedIndex = 0;
                            templateID = Convert.ToInt32(templaterepeater.SelectedValue);
                            TemplateID =templaterepeater.SelectedValue.ToString();
                            ShowPreviewTemplate(templateID);
                        }
                    }
                    LoadCategories();
                }
                getLayoutSize();
                checkAutoBorder();
            }
            else
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }
        }

        protected void ddlCategoryFilter_IndexChanged(object sender, EventArgs e)
        {
            DropDownList dllCategory = (DropDownList)sender;
            if (dllCategory.SelectedIndex != 0)
            {
                loadTemplates(dllCategory.SelectedItem.Text);
            }
            else
            {
                loadTemplates();
            }
            
        }

        private void LoadCategories()
        {
            ddlCategoryFilter.Items.Clear();
            List<ECN_Framework_Entities.Communicator.Template> templateList = ECN_Framework_BusinessLayer.Communicator.Template.GetByBaseChannelID(Convert.ToInt32(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser).GroupBy(x => x.Category).Select(x => x.First()).ToList();

            templateList.RemoveAll(x => string.IsNullOrWhiteSpace(x.Category));

            ddlCategoryFilter.DataSource = templateList;
            ddlCategoryFilter.DataBind();
            ddlCategoryFilter.Items.Insert(0, "All");
        }

        public void btnPreview_Click(object sender, EventArgs e)
        {
            lblPreview.Text = PopulatePreview();
            this.modalPopupPreview.Show();

        }

        public void checkDynamicContent()
        {
            if (!ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.DynamicContent))
            {
                dynamicContentAccess.Value = "false";
            }
            else
                dynamicContentAccess.Value = "true";
        }

        private void ShowPreviewLayout(int LayoutID)
        {
            string body = "";
            string TableOptions = "";
            string TemplateSource = "";
            int? Slot1;
            int? Slot2;
            int? Slot3;
            int? Slot4;
            int? Slot5;
            int? Slot6;
            int? Slot7;
            int? Slot8;
            int? Slot9;
            ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(LayoutID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
            TemplateSource = layout.Template.TemplateSource;
            TableOptions = layout.TableOptions;
            Slot1 = layout.ContentSlot1.Value;
            Slot2 = layout.ContentSlot2;
            Slot3 = layout.ContentSlot3;
            Slot4 = layout.ContentSlot4;
            Slot5 = layout.ContentSlot5;
            Slot6 = layout.ContentSlot6;
            Slot7 = layout.ContentSlot7;
            Slot8 = layout.ContentSlot8;
            Slot9 = layout.ContentSlot9;

            body = ECN_Framework_BusinessLayer.Communicator.Layout.HTMLEdit(TemplateSource, layout.Template.TemplateText, TableOptions, Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7, Slot8, Slot9, ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML, false, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            LabelPreview.Text = body;

            //List<ECN_Framework_Entities.Activity.BlastActivityClicks> clickList = ECN_Framework_BusinessLayer.Activity.BlastActivityClicks.GetByBlastID(getBlastID());
            //int totalClicks = clickList.Count;
            //if (totalClicks > 0)
            //{

            //    var result = (from src in clickList
            //                  group src by src.URL into gp
            //                  orderby gp.Count() descending
            //                  select new { URL = gp.Key, Clicks = (gp.Count()) }).ToList();

            //    HtmlDocument doc = new HtmlDocument();
            //    doc.LoadHtml(body);
            //    foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            //    {
            //        HtmlAttribute att = link.Attributes["href"];
            //        IEnumerable<int> clickCount = from r in result
            //                                      where r.URL == HttpUtility.HtmlDecode(att.Value)
            //                                      select r.Clicks;
            //        if (!clickCount.FirstOrDefault().ToString().Equals("0"))
            //        {
            //            int clicksCount = Convert.ToInt32(clickCount.FirstOrDefault().ToString());
            //            decimal clicksPercent = (clicksCount * 100) / totalClicks;
            //            //link.Attributes.Add("clickvalue", clickCount.FirstOrDefault().ToString() + " Clicks (" + Math.Round(clicksPercent,1)+"%)");
            //            link.Attributes.Add("clickvalue", clickCount.FirstOrDefault().ToString());
            //        }
            //    }
            //    LabelPreview.Text = doc.DocumentNode.InnerHtml;
            //}
            //else
            //{
            //    LabelPreview.Text = body;
            //}

        }

        private void ShowPreviewTemplate(int templateID)
        {
            string body = "";
            ECN_Framework_Entities.Communicator.Template template =
            ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID(templateID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            if (getLayoutID() == 0)
                body = loadHTMLLayout(template.TemplateSource, null);
            else
            {
                ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(getLayoutID(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
                body = loadHTMLLayout(template.TemplateSource, layout);
            }
            LabelPreview.Text = body;
        }

        private void ShowPreviewLayout(ECN_Framework_Entities.Communicator.Layout layout)
        {
            string body = "";
            ECN_Framework_Entities.Communicator.Template template =
            ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID(layout.TemplateID.Value, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            body = loadHTMLLayout(template.TemplateSource, layout);
            LabelPreview.Text = body;
        }

        protected void btnCreateContent1_Click(object sender, EventArgs e)
        {
            CreateContent_Click(1);
        }
        protected void btnCreateContent2_Click(object sender, EventArgs e)
        {
            CreateContent_Click(2);
        }
        protected void btnCreateContent3_Click(object sender, EventArgs e)
        {
            CreateContent_Click(3);
        }
        protected void btnCreateContent4_Click(object sender, EventArgs e)
        {
            CreateContent_Click(4);
        }
        protected void btnCreateContent5_Click(object sender, EventArgs e)
        {
            CreateContent_Click(5);
        }
        protected void btnCreateContent6_Click(object sender, EventArgs e)
        {
            CreateContent_Click(6);
        }
        protected void btnCreateContent7_Click(object sender, EventArgs e)
        {
            CreateContent_Click(7);
        }
        protected void btnCreateContent8_Click(object sender, EventArgs e)
        {
            CreateContent_Click(8);
        }
        protected void btnCreateContent9_Click(object sender, EventArgs e)
        {
            CreateContent_Click(9);
        }

        protected void btnExistingContent1_Click(object sender, EventArgs e)
        {
            ExistingContent_Click(1);
        }
        protected void btnExistingContent2_Click(object sender, EventArgs e)
        {
            ExistingContent_Click(2);
        }
        protected void btnExistingContent3_Click(object sender, EventArgs e)
        {
            ExistingContent_Click(3);
        }
        protected void btnExistingContent4_Click(object sender, EventArgs e)
        {
            ExistingContent_Click(4);
        }
        protected void btnExistingContent5_Click(object sender, EventArgs e)
        {
            ExistingContent_Click(5);
        }
        protected void btnExistingContent6_Click(object sender, EventArgs e)
        {
            ExistingContent_Click(6);
        }
        protected void btnExistingContent7_Click(object sender, EventArgs e)
        {
            ExistingContent_Click(7);
        }
        protected void btnExistingContent8_Click(object sender, EventArgs e)
        {
            ExistingContent_Click(8);
        }
        protected void btnExistingContent9_Click(object sender, EventArgs e)
        {
            ExistingContent_Click(9);
        }

        private void CreateContent_Click(int selectedSlot)
        {
            //Session.Remove("templaterepeater_LastSelectedValue");

            HiddenField_SelectedSlot.Value = selectedSlot.ToString();
            contentEditor1.LoadFoldersDR(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID);
            contentEditor1.LoadUsersDR(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID);

            HiddenField hfContent = (HiddenField)pnlHidden.FindControl("HiddenField_Content" + selectedSlot.ToString());
            if (!hfContent.Value.Equals("0"))
            {
                contentEditor1.selectedContentID = Convert.ToInt32(hfContent.Value);
                contentEditor1.loadContent(Convert.ToInt32(hfContent.Value), false);
            }
            this.modalPopupCreateContent.Show();
            upMain.Update();
            //System.Web.UI.ScriptManager.RegisterStartupScript(upContentEditor, upContentEditor.GetType(), "paint", "repaint();", true);
        }

        private void ExistingContent_Click(int selectedSlot)
        {
            HiddenField_SelectedSlot.Value = selectedSlot.ToString();
            this.modalPopupExistingContent.Show();
            //System.Web.UI.ScriptManager.RegisterStartupScript(upContentEditor, upContentEditor.GetType(), "paint", "repaint();", true);
            contentExplorer1.loadContentFoldersDD(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID);
        }

        public void DoItemSelect(object objSource, DataListCommandEventArgs objArgs)
        {
            templaterepeater.SelectedIndex = objArgs.Item.ItemIndex;

            ViewState["templaterepeater_LastSelectedValue"] = templaterepeater.SelectedValue;

            if (ddlCategoryFilter.SelectedIndex != 0)
            {
                loadTemplates(ddlCategoryFilter.SelectedItem.Text);
            }
            else
            {
                loadTemplates();    
            }
            templateSlots = Convert.ToInt32(((TextBox)templaterepeater.SelectedItem.FindControl("SlotsTotal")).Text);
            templateID = Convert.ToInt32(((TextBox)templaterepeater.SelectedItem.FindControl("TemplateID")).Text);

            lblTemplateName.Text = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID(templateID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser).TemplateName;

            ToggleSlots();
            ShowPreviewTemplate(templateID);
        }

        private void checkBorder()
        {
            if (TableOptions.Text.Equals(""))
            {
                TemplateBorder.Items.FindByValue("N").Selected = true;
            }
            else if (TableOptions.Text.Equals(defaultBorder))
            {
                TemplateBorder.Items.FindByValue("Y").Selected = true;
            }
            else
            {
                TemplateBorder.Items.FindByValue("C").Selected = true;
                TableOptions.Visible = true;
            }
        }

        private void LoadFormData(int layoutID)
        {
            ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(layoutID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
            LayoutName.Text = layout.LayoutName;
            string displayAdd = layout.DisplayAddress;
            if (displayAdd.Length <= 4)
            {
                DisplayAddress.Text = CompanyAddress;
            }
            else
            {
                DisplayAddress.Text = displayAdd;
            }

            folderID.Items.FindByValue(layout.FolderID.ToString()).Selected = true;
            if (ddlMessageType.Items.FindByValue(layout.MessageTypeID.ToString()) != null)
            {
                ddlMessageType.Items.FindByValue(layout.MessageTypeID.ToString()).Selected = true;
            }
            else
            {
                ddlMessageType.Items.FindByValue("0").Selected = true;
            }
            TableOptions.Text = layout.TableOptions.ToString();
            TemplateID = layout.TemplateID.ToString();
            int col_count = getValidColumnCount(Convert.ToInt32(TemplateID));

            HiddenField_Content1.Value = layout.ContentSlot1.ToString();
            if (col_count > 1)
            {
                HiddenField_Content2.Value = layout.ContentSlot2.ToString();
            }
            if (col_count > 2)
            {
                HiddenField_Content3.Value = layout.ContentSlot3.ToString();
            }
            if (col_count > 3)
            {
                HiddenField_Content4.Value = layout.ContentSlot4.ToString();
            }
            if (col_count > 4)
            {
                HiddenField_Content5.Value = layout.ContentSlot5.ToString();
            }
            if (col_count > 5)
            {
                HiddenField_Content6.Value = layout.ContentSlot6.ToString();
            }
            if (col_count > 6)
            {
                HiddenField_Content7.Value = layout.ContentSlot7.ToString();
            }
            if (col_count > 7)
            {
                HiddenField_Content8.Value = layout.ContentSlot8.ToString();
            }
            if (col_count > 8)
            {
                HiddenField_Content9.Value = layout.ContentSlot9.ToString();
            }
            ShowPreviewLayout(layout);
            ToggleSlots();

             lblTemplateName.Text = layout.Template.TemplateName;
        }

        private int getValidColumnCount(int template_id)
        {
            ECN_Framework_Entities.Communicator.Template template = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID(template_id, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            try
            {
                return template.SlotsTotal.Value;
            }
            catch (Exception)
            {
                return 0;
            }
                
        }

        private void checkAutoBorder()
        {
            string customerCommLevel = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CommunicatorLevel;
            if (customerCommLevel.Equals("1"))
            {
                TemplateBorder.Items.FindByValue("N").Selected = true;
                TemplateBorder.Enabled = false;
                TemplateBorder.ToolTip = "Your customer level does not allow you to use this functionality";
                TableOptions.Text = "";
            }
        }

        public void getLayoutSize()
        {
            string thesize = "";
            try
            {
                ECN_Framework_Entities.Communicator.Template template = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID(Convert.ToInt32(TemplateID), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                string TemplateSource = template.TemplateSource;
                string tableOptions = TableOptions.Text;

                if (tableOptions.Length < 1)
                {
                    tableOptions = " cellpadding=0 cellspacing=0 ";
                }

                string body = ECN_Framework_BusinessLayer.Communicator.Layout.EmailBody
                    ("",
                        TemplateSource,
                        tableOptions,
                        Convert.ToInt32(HiddenField_Content1.Value),
                        Convert.ToInt32(HiddenField_Content2.Value),
                        Convert.ToInt32(HiddenField_Content3.Value),
                        Convert.ToInt32(HiddenField_Content4.Value),
                        Convert.ToInt32(HiddenField_Content5.Value),
                        Convert.ToInt32(HiddenField_Content6.Value),
                        Convert.ToInt32(HiddenField_Content7.Value),
                        Convert.ToInt32(HiddenField_Content8.Value),
                        Convert.ToInt32(HiddenField_Content9.Value),
                        ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML,
                        false,
                        ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser
                    );
                var textbody = CommonStringFunctions.EscapeXmlString(body);
                int htmlsize = (body.Length / 1024);
                int textsize = (textbody.Length / 1024);
                thesize = " HTML:" + (htmlsize + textsize + 1) + " KB<BR>TEXT:" + textsize + " KB";
            }
            catch (Exception E)
            {
                string devnull = E.ToString();
            }
            SizeLabel.Text = thesize;
        }

        private string PopulatePreview()
        {
            string body = "";
            if (TemplateID != null)
            {
                string tableOptions = string.Empty;
                ECN_Framework_Entities.Communicator.Template template = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID(Convert.ToInt32(TemplateID), user);
                string TemplateSource = template.TemplateSource;

                tableOptions = TableOptions.Text;

                if (tableOptions.Length < 1)
                {
                    tableOptions = " cellpadding=0 cellspacing=0 ";
                }

                body = ECN_Framework_BusinessLayer.Communicator.Layout.EmailBody(
                    TemplateSource, "",
                    tableOptions,
                    Convert.ToInt32(HiddenField_Content1.Value),
                    Convert.ToInt32(HiddenField_Content2.Value),
                    Convert.ToInt32(HiddenField_Content3.Value),
                    Convert.ToInt32(HiddenField_Content4.Value),
                    Convert.ToInt32(HiddenField_Content5.Value),
                    Convert.ToInt32(HiddenField_Content6.Value),
                    Convert.ToInt32(HiddenField_Content7.Value),
                    Convert.ToInt32(HiddenField_Content8.Value),
                    Convert.ToInt32(HiddenField_Content9.Value),
                    ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML,
                    false,
                    ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser
                    );
                body = Regex.Replace(body, @"<link(.|\n)*?>", string.Empty);
                body = Regex.Replace(body, @"<meta(.|\n)*?>", string.Empty);
                body = Regex.Replace(body, @"<title(.|\n)*?>(.|\n)*?</title>", string.Empty);
            }
            return body;

        }

        public void CreateAsNewInitialize(object sender, System.EventArgs e)
        {
            createAsNew = true;
        }

        private void BindDropDowns()
        {
            List<ECN_Framework_Entities.Communicator.Folder> folderList = ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.CNT.ToString(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            folderID.DataSource = folderList;
            folderID.DataBind();
            folderID.Items.Insert(0, "root");
            folderID.Items.FindByValue("root").Value = "0";

            List<ECN_Framework_Entities.Communicator.MessageType> messageTypeList = ECN_Framework_BusinessLayer.Communicator.MessageType.GetByBaseChannelID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            var result = (from src in messageTypeList
                          where src.IsActive == true
                          orderby src.SortOrder
                          select src).ToList();
            ddlMessageType.DataSource = result.OrderBy(x => x.SortOrder);
            ddlMessageType.DataBind();
            ddlMessageType.Items.Insert(0, new ListItem("Select a Message Type", "0"));
        }

        private void ValidateSlotContents()
        {
            string message = "";

            int valid_column_count = getValidColumnCount(Convert.ToInt32(TemplateID));
            if (valid_column_count >= 1)
            {
                if (HiddenField_Content1.Value == "0")
                {
                    message = "1,";
                }
            }
            if (valid_column_count >= 2)
            {
                if (HiddenField_Content2.Value == "0")
                {
                    message += "2,";
                }
            }
            if (valid_column_count >= 3)
            {
                if (HiddenField_Content3.Value == "0")
                {
                    message += "3,";
                }
            }
            if (valid_column_count >= 4)
            {
                if (HiddenField_Content4.Value == "0")
                {
                    message += "4,";
                }
            }
            if (valid_column_count >= 5)
            {
                if (HiddenField_Content5.Value == "0")
                {
                    message += "5,";
                }
            }
            if (valid_column_count >= 6)
            {
                if (HiddenField_Content6.Value == "0")
                {
                    message += "6,";
                }
            }
            if (valid_column_count >= 7)
            {
                if (HiddenField_Content7.Value == "0")
                {
                    message += "7,";

                }
            }
            if (valid_column_count >= 8)
            {
                if (HiddenField_Content8.Value == "0")
                {
                    message += "8,";
                }
            }
            if (valid_column_count >= 9)
            {
                if (HiddenField_Content9.Value == "0")
                {
                    message += "9,";

                }
            }
            if (message.Length > 0)
            {
                message = message.Substring(0, message.Length - 1);
                throwECNException("Please enter content for slot(s) " + message);
            }
        }

        private static void throwECNWarning(string message, bool bShow = true)
        {
            ECNWarning ecnError = new ECNWarning(Enums.Entity.Layout, Enums.Method.Validate, message);
            List<ECNWarning> errorList = new List<ECNWarning>();
            errorList.Add(ecnError);
            throw new ECNWarning(errorList, Enums.ExceptionLayer.WebSite);
        }

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Layout, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            throw new ECNException(errorList, Enums.ExceptionLayer.WebSite);
        }

        private string[] getValidColumns(int template_id)
        {
            int valid_column_count = getValidColumnCount(template_id);

            string[] content_slot = { "''", "''", "''", "''", "''", "''", "''", "''", "''" };

            if (valid_column_count >= 1)
                content_slot[0] = HiddenField_Content1.Value;
            if (valid_column_count >= 2)
                content_slot[1] = HiddenField_Content2.Value;
            if (valid_column_count >= 3)
                content_slot[2] = HiddenField_Content3.Value;
            if (valid_column_count >= 4)
                content_slot[3] = HiddenField_Content4.Value;
            if (valid_column_count >= 5)
                content_slot[4] = HiddenField_Content5.Value;
            if (valid_column_count >= 6)
                content_slot[5] = HiddenField_Content6.Value;
            if (valid_column_count >= 7)
                content_slot[6] = HiddenField_Content7.Value;
            if (valid_column_count >= 8)
                content_slot[7] = HiddenField_Content8.Value;
            if (valid_column_count >= 9)
                content_slot[8] = HiddenField_Content9.Value;
            return content_slot;
        }

        private void ToggleSlots()
        {
            for (int i = 0; i < 9; i++)
            {
                if (i >= templateSlots)
                {
                    if (i == 0)
                    {
                        HiddenField_Content1.Value = "0";
                    }
                    else if (i == 1)
                    {
                        HiddenField_Content2.Value = "0";
                    }
                    else if (i == 2)
                    {
                        HiddenField_Content3.Value = "0";
                    }
                    else if (i == 3)
                    {
                        HiddenField_Content4.Value = "0";
                    }
                    else if (i == 4)
                    {
                        HiddenField_Content5.Value = "0";
                    }
                    else if (i == 5)
                    {
                        HiddenField_Content6.Value = "0";
                    }
                    else if (i == 6)
                    {
                        HiddenField_Content7.Value = "0";
                    }
                    else if (i == 7)
                    {
                        HiddenField_Content8.Value = "0";
                    }
                    else if (i == 8)
                    {
                        HiddenField_Content9.Value = "0";
                    }


                }
            }
        }

        public int SaveLayout()
        {
            ValidateSlotContents();
            string lname = ECN_Framework_Common.Functions.StringFunctions.CleanString(LayoutName.Text);
            string displayaddr = ECN_Framework_Common.Functions.StringFunctions.CleanString(DisplayAddress.Text);
            string[] content_slot = getValidColumns(Convert.ToInt32(TemplateID));
            string toptions = ECN_Framework_Common.Functions.StringFunctions.CleanString(TableOptions.Text);
            int layoutID = createAsNew ? 0 : getLayoutID();
            ECN_Framework_Entities.Communicator.Layout layout;
            if (layoutID > 0)
            {
                layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(layoutID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                layout.UpdatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            }
            else
            {
                layout = new ECN_Framework_Entities.Communicator.Layout();
                layout.CreatedUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;
            }
            layout.LayoutName = lname;
            layout.CustomerID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID;

            layout.TableOptions = toptions;
            layout.FolderID = Convert.ToInt32(folderID.SelectedItem.Value);
            if (ddlMessageType.SelectedValue != "0")
            {
                layout.MessageTypeID = Convert.ToInt32(ddlMessageType.SelectedValue);
            }
            int slotCount=getValidColumnCount(Convert.ToInt32(TemplateID));
            List<int> ContentList = new List<int>() ;
            if (!content_slot[0].Equals("''") && Convert.ToInt32(content_slot[0]) != 0)
            { 
                layout.ContentSlot1 = Convert.ToInt32(content_slot[0]);
                ContentList.Add((int)layout.ContentSlot1);
            }
            if (!content_slot[1].Equals("''") && Convert.ToInt32(content_slot[1]) != 0 && slotCount > 1)
            { 
                layout.ContentSlot2 = Convert.ToInt32(content_slot[1]);
                ContentList.Add((int)layout.ContentSlot2);
            }
            else
                layout.ContentSlot2 = null;
            if (!content_slot[2].Equals("''") && Convert.ToInt32(content_slot[2]) != 0 && slotCount > 2)
            { 
                layout.ContentSlot3 = Convert.ToInt32(content_slot[2]);
                ContentList.Add((int)layout.ContentSlot3);
            }
            else
                layout.ContentSlot3 = null;
            if (!content_slot[3].Equals("''") && Convert.ToInt32(content_slot[3]) != 0 && slotCount > 3)
            { 
                layout.ContentSlot4 = Convert.ToInt32(content_slot[3]);
                ContentList.Add((int)layout.ContentSlot4);
            }
            else
                layout.ContentSlot4 = null;
            if (!content_slot[4].Equals("''") && Convert.ToInt32(content_slot[4]) != 0 && slotCount > 4)
            { 
                layout.ContentSlot5 = Convert.ToInt32(content_slot[4]);
                ContentList.Add((int)layout.ContentSlot5);
            }
            else
                layout.ContentSlot5 = null;
            if (!content_slot[5].Equals("''") && Convert.ToInt32(content_slot[5]) != 0 && slotCount > 5)
            { 
                layout.ContentSlot6 = Convert.ToInt32(content_slot[5]);
                ContentList.Add((int)layout.ContentSlot6);
            }
            else
                layout.ContentSlot6 = null;
            if (!content_slot[6].Equals("''") && Convert.ToInt32(content_slot[6]) != 0 && slotCount > 6)
            { 
                layout.ContentSlot7 = Convert.ToInt32(content_slot[6]);
                ContentList.Add((int)layout.ContentSlot7);
            }
            else
                layout.ContentSlot7 = null;
            if (!content_slot[7].Equals("''") && Convert.ToInt32(content_slot[7]) != 0 && slotCount > 7)
            { 
                layout.ContentSlot8 = Convert.ToInt32(content_slot[7]);
                ContentList.Add((int)layout.ContentSlot8);
            }
            else
                layout.ContentSlot8 = null;
            if (!content_slot[8].Equals("''") && Convert.ToInt32(content_slot[8]) != 0 && slotCount > 8)
            { 
                layout.ContentSlot9 = Convert.ToInt32(content_slot[8]);
            }
            else
                layout.ContentSlot9 = null;
            layout.DisplayAddress = displayaddr;
            layout.TemplateID = Convert.ToInt32(TemplateID);
            ECN_Framework_BusinessLayer.Communicator.Layout.Save(layout, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            if(ContentList!=null)
            {
                bool bValidated=true ;
                foreach (int cl in ContentList)
                {
                    if(cl >0)
                    {
                        bValidated =  ECN_Framework_BusinessLayer.Communicator.Content.GetValidatedStatusByContentID_NoAccessCheck(cl);
                        if(!bValidated)
                              bValidated = false;
                      
                    }
                }
                if (!bValidated)
                    throwECNWarning("Content Not Validated", false);
            }
            return layout.LayoutID;
        }
       
        private void loadTemplates(string category = null)
        {
            List<ECN_Framework_Entities.Communicator.Template> templateList = ECN_Framework_BusinessLayer.Communicator.Template.GetByStyleCode(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, "newsletter", ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);

            if (category != null)
            {
                templateList.RemoveAll(x => x.Category != category);
            }
            
            if (!IsPostBack && templateList.Any())
            {
                lblTemplateName.Text = templateList.First().TemplateName;
            }

            templaterepeater.DataSource = templateList;

            int? lastSelectedValue = ViewState["templaterepeater_LastSelectedValue"] as int?;
            if (lastSelectedValue != null)
            {
                templaterepeater.SelectedIndex = templateList.FindIndex(x => x.TemplateID == lastSelectedValue);
            }

            templaterepeater.DataBind();
        }

        public void TemplateBorder_Change(Object src, EventArgs e)
        {
            if (TemplateBorder.SelectedItem.Value == "Y")
            {
                TableOptions.Visible = false;
                TableOptions.Text = defaultBorder;
            }
            else if (TemplateBorder.SelectedItem.Value == "N")
            {
                TableOptions.Visible = false;
                TableOptions.Text = "";
            }
            else if (TemplateBorder.SelectedItem.Value == "C")
            {
                TableOptions.Visible = true;
                TableOptions.Text = defaultBorder;
            }

        }

        private string loadHTMLLayout(string TemplateSource, ECN_Framework_Entities.Communicator.Layout layout)
        {
            string body = TemplateSource;
            HtmlDocument templateDoc = new HtmlDocument();
            templateDoc.LoadHtml(TemplateSource);
            HtmlNodeCollection tableNodes = templateDoc.DocumentNode.SelectNodes("//table");
            if (tableNodes != null && tableNodes.Count > 0)
            {
                foreach (HtmlNode table in templateDoc.DocumentNode.SelectNodes("//table"))
                {
                    bool slotsTable = table.InnerHtml.Contains("%%slot");
                    if (!slotsTable)
                    {
                        table.InnerHtml = "";
                    }

                }
            }
            body = templateDoc.DocumentNode.InnerHtml;

            body = StringFunctions.Replace(body, "%%slot1%%", @"<div id=div_slot1  class=""choose1"" style=""border:2px;border-style:dashed;border-color:orange;"" >" +
                                                              @"<div id=div_edit1  class=""popup1""><table><tr align=right><td> " +
                                                              @"<a href=""#""  class=""aspBtn"" onclick=""clickCreate(1)"">Edit</a></td><td><a href=""#""  class=""aspBtn""  onclick=""clickExisting(1)"">Existing</a> </td><td><a href=""#"" class=""aspBtn"" onclick=""clickDynamic(1)"">Dynamic</a> " +
                                                              @"</td></tr></table></div><div id=""%%SLOT1%%"">Slot1</div></div>");

            body = StringFunctions.Replace(body, "%%slot2%%", @"<div id=div_slot2  class=""choose2"" style=""border:2px;border-style:dashed;border-color:orange;"">" +
                                                                @"<div id=div_edit2 class=""popup2""><table><tr align=right><td> " +
                                                                @"<a href=""#""  class=""aspBtn""   onclick=""clickCreate(2)"">Edit</a></td><td><a href=""#""  class=""aspBtn""   onclick=""clickExisting(2)"">Existing</a>  </td><td><a href=""#"" class=""aspBtn"" onclick=""clickDynamic(2)"">Dynamic</a>" +
                                                                @"</td></tr></table></div><div id=""%%SLOT2%%"">Slot2</div></div>");

            body = StringFunctions.Replace(body, "%%slot3%%", @"<div id=div_slot3  class=""choose3"" style=""border:2px;border-style:dashed;border-color:orange;"" >" +
                                                             @"<div id=div_edit3 class=""popup3""><table><tr align=right><td> " +
                                                             @"<a href=""#""  class=""aspBtn""   onclick=""clickCreate(3)"">Edit</a></td><td><a href=""#""  class=""aspBtn""   onclick=""clickExisting(3)"">Existing</a>  </td><td><a href=""#"" class=""aspBtn"" onclick=""clickDynamic(3)"">Dynamic</a>" +
                                                             @"</td></tr></table></div><div id=""%%SLOT3%%"">Slot3</div></div>");

            body = StringFunctions.Replace(body, "%%slot4%%", @"<div id=div_slot4  class=""choose4"" style=""border:2px;border-style:dashed;border-color:orange;"" >" +
                                                             @"<div id=div_edit4 class=""popup4""><table><tr align=right><td> " +
                                                             @"<a href=""#""  class=""aspBtn""   onclick=""clickCreate(4)"">Edit</a></td><td><a href=""#""  class=""aspBtn""   onclick=""clickExisting(4)"">Existing</a>  </td><td><a href=""#"" class=""aspBtn"" onclick=""clickDynamic(4)"">Dynamic</a>" +
                                                             @"</td></tr></table></div><div id=""%%SLOT4%%"">Slot4</div></div>");

            body = StringFunctions.Replace(body, "%%slot5%%", @"<div id=div_slot5  class=""choose5"" style=""border:2px;border-style:dashed;border-color:orange;"" >" +
                                                             @"<div id=div_edit5 class=""popup5""><table><tr align=right><td> " +
                                                             @"<a href=""#""  class=""aspBtn""   onclick=""clickCreate(5)"">Edit</a></td><td><a href=""#""  class=""aspBtn""   onclick=""clickExisting(5)"">Existing</a>  </td><td><a href=""#"" class=""aspBtn"" onclick=""clickDynamic(5)"">Dynamic</a>" +
                                                             @"</td></tr></table></div><div id=""%%SLOT5%%"">Slot5</div></div>");

            body = StringFunctions.Replace(body, "%%slot6%%", @"<div id=div_slot6  class=""choose6"" style=""border:2px;border-style:dashed;border-color:orange;"" >" +
                                                             @"<div id=div_edit6 class=""popup6""><table><tr align=right><td> " +
                                                             @"<a href=""#""  class=""aspBtn""   onclick=""clickCreate(6)"">Edit</a></td><td><a href=""#""  class=""aspBtn""   onclick=""clickExisting(6)"">Existing</a> </td><td><a href=""#"" class=""aspBtn"" onclick=""clickDynamic(6)"">Dynamic</a> " +
                                                             @"</td></tr></table></div><div id=""%%SLOT6%%"">Slot6</div></div>");

            body = StringFunctions.Replace(body, "%%slot7%%", @"<div id=div_slot7  class=""choose7"" style=""border:2px;border-style:dashed;border-color:orange;"" >" +
                                                             @"<div id=div_edit7 class=""popup7""><table><tr align=right><td> " +
                                                             @"<a href=""#""  class=""aspBtn""   onclick=""clickCreate(7)"">Edit</a></td><td><a href=""#""  class=""aspBtn""   onclick=""clickExisting(7)"">Existing</a> </td><td><a href=""#"" class=""aspBtn"" onclick=""clickDynamic(7)"">Dynamic</a> " +
                                                             @"</td></tr></table></div><div id=""%%SLOT7%%"">Slot7</div></div>");

            body = StringFunctions.Replace(body, "%%slot8%%", @"<div id=div_slot8  class=""choose8"" style=""border:2px;border-style:dashed;border-color:orange;"" >" +
                                                             @"<div id=div_edit8 class=""popup8""><table><tr align=right><td> " +
                                                             @"<a href=""#""  class=""aspBtn""   onclick=""clickCreate(8)"">Edit</a></td><td><a href=""#""  class=""aspBtn""   onclick=""clickExisting(8)"">Existing</a> </td><td><a href=""#"" class=""aspBtn"" onclick=""clickDynamic(8)"">Dynamic</a> " +
                                                             @"</td></tr></table></div><div id=""%%SLOT8%%"">Slot8</div></div>");

            body = StringFunctions.Replace(body, "%%slot9%%", @"<div id=div_slot9  class=""choose9"" style=""border:2px;border-style:dashed;border-color:orange;"" >" +
                                                             @"<div id=div_edit9 class=""popup9""><table><tr align=right><td> " +
                                                             @"<a href=""#""  class=""aspBtn""   onclick=""clickCreate(9)"">Edit</a></td><td><a href=""#""  class=""aspBtn""   onclick=""clickExisting(9)"">Existing</a>  </td><td><a href=""#"" class=""aspBtn"" onclick=""clickDynamic(9)"">Dynamic</a>" +
                                                             @"</td></tr></table></div><div id=""%%SLOT9%%"">Slot9</div></div>");
            body = StringFunctions.Replace(body, "®", "&reg;");
            body = StringFunctions.Replace(body, "©", "&copy;");
            body = StringFunctions.Replace(body, "™", "&trade;");
            body = StringFunctions.Replace(body, "…", "...");
            body = StringFunctions.Replace(body, ((char)0).ToString(), "");
            body = @"<table width=""100%""><tr><td>" + body + "</td></tr></table>";

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(body);
            if (layout != null)
            {
                if (layout.Slot1 != null)
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT1%%']");
                    if(node!=null)
                        node.InnerHtml = layout.Slot1.ContentSource;
                }
                if (layout.Slot2 != null)
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT2%%']");
                    if (node != null)
                        node.InnerHtml = layout.Slot2.ContentSource;
                }
                if (layout.Slot3 != null)
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT3%%']");
                    if (node != null)
                        node.InnerHtml = layout.Slot3.ContentSource;
                }
                if (layout.Slot4 != null)
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT4%%']");
                    if (node != null)
                        node.InnerHtml = layout.Slot4.ContentSource;
                }
                if (layout.Slot5 != null)
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT5%%']");
                    if (node != null)
                        node.InnerHtml = layout.Slot5.ContentSource;
                }
                if (layout.Slot6 != null)
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT6%%']");
                    if (node != null)
                        node.InnerHtml = layout.Slot6.ContentSource;
                }
                if (layout.Slot7 != null)
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT7%%']");
                    if (node != null)
                        node.InnerHtml = layout.Slot7.ContentSource;
                }
                if (layout.Slot8 != null)
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT8%%']");
                    if (node != null)
                        node.InnerHtml = layout.Slot8.ContentSource;
                }
                if (layout.Slot9 != null)
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT9%%']");
                    if (node != null)
                        node.InnerHtml = layout.Slot9.ContentSource;
                }
            }

            return doc.DocumentNode.InnerHtml;
        }

        protected void CreateContent_Save(object sender, EventArgs e)
        {
            contentEditor1.ValidateContentInitialize(sender, e);
            int contentID = contentEditor1.SaveContent();
            if (contentID > 0)
            {
                contentEditor1.Reset();
                this.modalPopupCreateContent.Hide();
                updateContentDetail(contentID);
            }
        }

        protected void CreateContent_Close(object sender, EventArgs e)
        {
            contentEditor1.Reset();
            this.modalPopupCreateContent.Hide();
        }

        protected void ExistingContent_Hide(object sender, EventArgs e)
        {
            int contentID = contentExplorer1.selectedContentID;
            this.modalPopupExistingContent.Hide();
            updateContentDetail(contentID);
            contentExplorer1.Reset();
        }

        protected void btnClosePreview_Hide(object sender, EventArgs e)
        {
            this.modalPopupPreview.Hide();
        }

        private void updateContentDetail(int contentID)
        {
            if (contentID > 0)
            {
                string body = LabelPreview.Text;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(body);

                ECN_Framework_Entities.Communicator.Content content =
                ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(contentID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                if (HiddenField_SelectedSlot.Value.Equals("1"))
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT1%%']");
                    if (node != null && content.ContentSource != null)
                        node.InnerHtml = content.ContentSource;
                    HiddenField_Content1.Value = contentID.ToString();
                }
                if (HiddenField_SelectedSlot.Value.Equals("2"))
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT2%%']");
                    if (node != null && content.ContentSource != null)
                        node.InnerHtml = content.ContentSource;
                    HiddenField_Content2.Value = contentID.ToString();
                }
                if (HiddenField_SelectedSlot.Value.Equals("3"))
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT3%%']");
                    if (node != null && content.ContentSource != null)
                        node.InnerHtml = content.ContentSource;
                    HiddenField_Content3.Value = contentID.ToString();
                }
                if (HiddenField_SelectedSlot.Value.Equals("4"))
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT4%%']");
                    if (node != null && content.ContentSource != null)
                        node.InnerHtml = content.ContentSource;
                    HiddenField_Content4.Value = contentID.ToString();
                }
                if (HiddenField_SelectedSlot.Value.Equals("5"))
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT5%%']");
                    if (node != null && content.ContentSource != null)
                        node.InnerHtml = content.ContentSource;
                    HiddenField_Content5.Value = contentID.ToString();
                }
                if (HiddenField_SelectedSlot.Value.Equals("6"))
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT6%%']");
                    if (node != null && content.ContentSource != null)
                        node.InnerHtml = content.ContentSource;
                    HiddenField_Content6.Value = contentID.ToString();
                }
                if (HiddenField_SelectedSlot.Value.Equals("7"))
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT7%%']");
                    if (node != null && content.ContentSource != null)
                        node.InnerHtml = content.ContentSource;
                    HiddenField_Content7.Value = contentID.ToString();
                }
                if (HiddenField_SelectedSlot.Value.Equals("8"))
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT8%%']");
                    if (node != null && content.ContentSource != null)
                        node.InnerHtml = content.ContentSource;
                    HiddenField_Content8.Value = contentID.ToString();
                }
                if (HiddenField_SelectedSlot.Value.Equals("9"))
                {
                    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id='%%SLOT9%%']");
                    if (node != null && content.ContentSource != null)
                        node.InnerHtml = content.ContentSource;
                    HiddenField_Content9.Value = contentID.ToString();
                }
                LabelPreview.Text = doc.DocumentNode.InnerHtml;
            }
        }
    }
}
