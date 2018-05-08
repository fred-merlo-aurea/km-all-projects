using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Objects;
using CommonStringFunctions = KM.Common.StringFunctions;
using Enums = ECN_Framework_Common.Objects.Enums;

namespace ecn.communicator.main.ECNWizard.Content
{
    public partial class layoutEditor : System.Web.UI.UserControl
    {
        bool createAsNew = false;
        protected Panel[] Slots = new Panel[9];
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
                templateSlots = Convert.ToInt32(((TextBox)templaterepeater.SelectedItem.FindControl("SlotsTotal")).Text);
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

        private int getLayoutID()
        {
            if (Request.QueryString["LayoutID"] != null)
                return Convert.ToInt32(Request.QueryString["LayoutID"].ToString());
            else
                return 0;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            user = KMPlatform.BusinessLogic.User.GetByUserID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, false);
            if (!(Page.IsPostBack))
            {
                checkDynamicContent();
                contentExplorer1.enableSelectMode();
                loadTemplates();
                ToggleSlots();
                pnlMessageTypes.Visible = ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SetMessagePriority);
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
                    TemplateBorder.Items.FindByValue("N").Selected = true;
                }
                
            }           
            getLayoutSize();
            checkAutoBorder();
        }

        public void Initialize()
        {
            user = KMPlatform.BusinessLogic.User.GetByUserID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, false);
            checkDynamicContent();
            contentExplorer1.enableSelectMode();
            loadTemplates();
            ToggleSlots();
            pnlMessageTypes.Visible = ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.SetMessagePriority);
            BindDropDowns();
            LoadCategories();
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
                TemplateBorder.Items.FindByValue("N").Selected = true;
            }
            getLayoutSize();
            checkAutoBorder();
        }

        private void checkDynamicContent()
        {
                int layoutID = getLayoutID();
            ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(layoutID,  true);
            if (!ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.DynamicContent))
            {
                HyperLink1.NavigateUrl = "javascript:upgrade();";
                HyperLink2.NavigateUrl = "javascript:upgrade();";
                HyperLink3.NavigateUrl = "javascript:upgrade();";
                HyperLink4.NavigateUrl = "javascript:upgrade();";
                HyperLink5.NavigateUrl = "javascript:upgrade();";
                HyperLink6.NavigateUrl = "javascript:upgrade();";
                HyperLink7.NavigateUrl = "javascript:upgrade();";
                HyperLink8.NavigateUrl = "javascript:upgrade();";
                HyperLink9.NavigateUrl = "javascript:upgrade();";
            }           
            else if (layout == null)
            {
                HyperLink1.Visible = false;
                HyperLink2.Visible = false;
                HyperLink3.Visible = false;
                HyperLink4.Visible = false;
                HyperLink5.Visible = false;
                HyperLink6.Visible = false;
                HyperLink7.Visible = false;
                HyperLink8.Visible = false;
                HyperLink9.Visible = false;
            }
            else
            {
                HyperLink1.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=1&LayoutID=" + layoutID;
                HyperLink2.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=2&LayoutID=" + layoutID;
                HyperLink3.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=3&LayoutID=" + layoutID;
                HyperLink4.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=4&LayoutID=" + layoutID;
                HyperLink5.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=5&LayoutID=" + layoutID;
                HyperLink6.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=6&LayoutID=" + layoutID;
                HyperLink7.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=7&LayoutID=" + layoutID;
                HyperLink8.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=8&LayoutID=" + layoutID;
                HyperLink9.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=9&LayoutID=" + layoutID;
            }
        }

        protected void CreateContent_Show(object sender, EventArgs e)
        {
            contentEditor1.LoadFoldersDR(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID);
            contentEditor1.LoadUsersDR(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentCustomer.CustomerID);
            updateSlotDetail(((LinkButton)sender).ID);
            this.modalPopupCreateContent.Show();
            
            System.Web.UI.ScriptManager.RegisterStartupScript(upContentEditor, upContentEditor.GetType(), "paint", "repaint();", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(),"paint", "repaint();",true);
        }

        protected void ExistingContent_Show(object sender, EventArgs e)
        {
            updateSlotDetail(((LinkButton)sender).ID);          
            this.modalPopupExistingContent.Show();
            System.Web.UI.ScriptManager.RegisterStartupScript(upContentEditor, upContentEditor.GetType(), "paint", "repaint();", true);
            contentExplorer1.loadContentFoldersDD(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID);
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

        private void ToggleSlots()
        {
            Slots[0] = Slot1;
            Slots[1] = Slot2;
            Slots[2] = Slot3;
            Slots[3] = Slot4;
            Slots[4] = Slot5;
            Slots[5] = Slot6;
            Slots[6] = Slot7;
            Slots[7] = Slot8;
            Slots[8] = Slot9;
            for (int i = 0; i < 9; i++)
            {
                Slots[i].Visible = true;
                if (i >= templateSlots)
                {
                    Slots[i].Visible = false;
                    if (i == 0)
                    {
                        lblSlot1.Text = "";
                        hdnSlot1.Value = "0";
                    }
                    else if (i == 1)
                    {
                        lblSlot2.Text = "";
                        hdnSlot2.Value = "0";
                    }
                    else if (i == 2)
                    {
                        lblSlot3.Text = "";
                        hdnSlot3.Value = "0";
                    }
                    else if (i == 3)
                    {
                        lblSlot4.Text = "";
                        hdnSlot4.Value = "0";
                    }
                    else if (i == 4)
                    {
                        lblSlot5.Text = "";
                        hdnSlot5.Value = "0";
                    }
                    else if (i == 5)
                    {
                        lblSlot6.Text = "";
                        hdnSlot6.Value = "0";
                    }
                    else if (i == 6)
                    {
                        lblSlot7.Text = "";
                        hdnSlot7.Value = "0";
                    }
                    else if (i == 7)
                    {
                        lblSlot8.Text = "";
                        hdnSlot8.Value = "0";
                    }
                    else if (i == 8)
                    {
                        lblSlot9.Text = "";
                        hdnSlot9.Value = "0";
                    }

                   
                }
            }
        }

        private void loadTemplates(string category = null)
        {
            List<ECN_Framework_Entities.Communicator.Template> templateList =
            ECN_Framework_BusinessLayer.Communicator.Template.GetByStyleCode_NoAccessCheck(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID, "newsletter");

            if (category != null)
            {
                templateList.RemoveAll(x => x.Category != category);
            }

            
            templaterepeater.DataSource = templateList;

            int? lastSelectedValue = ViewState["templaterepeater_LastSelectedValue"] as int?;
            int templateValue = lastSelectedValue.HasValue ? lastSelectedValue.Value : 0;
            if (templateList.Any(x => x.TemplateID == templateValue))
            {
                lblTemplateName.Text = templateList.First(x => x.TemplateID == templateValue).TemplateName;
            }


            if (lastSelectedValue != null)
            {
                templaterepeater.SelectedIndex = templateList.FindIndex(x => x.TemplateID == lastSelectedValue);
            }

            templaterepeater.DataBind();

        }   

        private void updateContentDetail(int contentID)
        {
            if (contentID > 0)
            {
                ECN_Framework_Entities.Communicator.Content content =
                ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(contentID,  false);
                if (SelectedSlot.Value.Equals("1"))
                {
                    lblSlot1.Text = content.ContentTitle;
                    hdnSlot1.Value = content.ContentID.ToString();
                }
                else if (SelectedSlot.Value.Equals("2"))
                {
                    lblSlot2.Text = content.ContentTitle;
                    hdnSlot2.Value = content.ContentID.ToString();
                }
                else if (SelectedSlot.Value.Equals("3"))
                {
                    lblSlot3.Text = content.ContentTitle;
                    hdnSlot3.Value = content.ContentID.ToString();
                }
                else if (SelectedSlot.Value.Equals("4"))
                {
                    lblSlot4.Text = content.ContentTitle;
                    hdnSlot4.Value = content.ContentID.ToString();
                }
                else if (SelectedSlot.Value.Equals("5"))
                {
                    lblSlot5.Text = content.ContentTitle;
                    hdnSlot5.Value = content.ContentID.ToString();
                }
                else if (SelectedSlot.Value.Equals("6"))
                {
                    lblSlot6.Text = content.ContentTitle;
                    hdnSlot6.Value = content.ContentID.ToString();
                }
                else if (SelectedSlot.Value.Equals("7"))
                {
                    lblSlot7.Text = content.ContentTitle;
                    hdnSlot7.Value = content.ContentID.ToString();
                }
                else if (SelectedSlot.Value.Equals("8"))
                {
                    lblSlot8.Text = content.ContentTitle;
                    hdnSlot8.Value = content.ContentID.ToString();
                }
                else if (SelectedSlot.Value.Equals("9"))
                {
                    lblSlot9.Text = content.ContentTitle;
                    hdnSlot9.Value = content.ContentID.ToString();
                }
            }

        }        

        private void updateSlotDetail(string linkButtonID)
        {
            if ((linkButtonID.Equals("lnkbtnCreateContent1")) || (linkButtonID.Equals("lnkbtnExistingContent1")))
            {
                SelectedSlot.Value = "1";
            }
            else if ((linkButtonID.Equals("lnkbtnCreateContent2")) || (linkButtonID.Equals("lnkbtnExistingContent2")))
            {
                SelectedSlot.Value = "2";
            }
            else if ((linkButtonID.Equals("lnkbtnCreateContent3")) || (linkButtonID.Equals("lnkbtnExistingContent3")))
            {
                SelectedSlot.Value = "3";
            }
            else if ((linkButtonID.Equals("lnkbtnCreateContent4")) || (linkButtonID.Equals("lnkbtnExistingContent4")))
            {
                SelectedSlot.Value = "4";
            }
            else if ((linkButtonID.Equals("lnkbtnCreateContent5")) || (linkButtonID.Equals("lnkbtnExistingContent5")))
            {
                SelectedSlot.Value = "5";
            }
            else if ((linkButtonID.Equals("lnkbtnCreateContent6")) || (linkButtonID.Equals("lnkbtnExistingContent6")))
            {
                SelectedSlot.Value = "6";
            }
            else if ((linkButtonID.Equals("lnkbtnCreateContent7")) || (linkButtonID.Equals("lnkbtnExistingContent7")))
            {
                SelectedSlot.Value = "7";
            }
            else if ((linkButtonID.Equals("lnkbtnCreateContent8")) || (linkButtonID.Equals("lnkbtnExistingContent8")))
            {
                SelectedSlot.Value = "8";
            }
            else if ((linkButtonID.Equals("lnkbtnCreateContent9")) || (linkButtonID.Equals("lnkbtnExistingContent9")))
            {
                SelectedSlot.Value = "9";
            }
        }

        public void btnPreview_Click(object sender, EventArgs e)
        {           
            FCKeditor1.Text = PopulatePreview();
            this.modalPopupPreview.Show();

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

        private string[] getValidColumns(int template_id)
        {
            int valid_column_count = getValidColumnCount(template_id);

            string[] content_slot = { "''", "''", "''", "''", "''", "''", "''", "''", "''" };

            if (valid_column_count >= 1)
                content_slot[0] = hdnSlot1.Value;
            if (valid_column_count >= 2)
                content_slot[1] = hdnSlot2.Value;
            if (valid_column_count >= 3)
                content_slot[2] = hdnSlot3.Value;
            if (valid_column_count >= 4)
                content_slot[3] = hdnSlot4.Value;
            if (valid_column_count >= 5)
                content_slot[4] = hdnSlot5.Value;
            if (valid_column_count >= 6)
                content_slot[5] = hdnSlot6.Value;
            if (valid_column_count >= 7)
                content_slot[6] = hdnSlot7.Value;
            if (valid_column_count >= 8)
                content_slot[7] = hdnSlot8.Value;
            if (valid_column_count >= 9)
                content_slot[8] = hdnSlot9.Value;
            return content_slot;
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
                layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(layoutID, false);
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
            if (!content_slot[0].Equals("''") && Convert.ToInt32(content_slot[0]) != 0)
                layout.ContentSlot1 = Convert.ToInt32(content_slot[0]);
            if (!content_slot[1].Equals("''") && Convert.ToInt32(content_slot[1]) != 0)
                layout.ContentSlot2 = Convert.ToInt32(content_slot[1]);
            if (!content_slot[2].Equals("''") && Convert.ToInt32(content_slot[2]) != 0)
                layout.ContentSlot3 = Convert.ToInt32(content_slot[2]);
            if (!content_slot[3].Equals("''") && Convert.ToInt32(content_slot[3]) != 0)
                layout.ContentSlot4 = Convert.ToInt32(content_slot[3]);
            if (!content_slot[4].Equals("''") && Convert.ToInt32(content_slot[4]) != 0)
                layout.ContentSlot5 = Convert.ToInt32(content_slot[4]);
            if (!content_slot[5].Equals("''") && Convert.ToInt32(content_slot[5]) != 0)
                layout.ContentSlot6 = Convert.ToInt32(content_slot[5]);
            if (!content_slot[6].Equals("''") && Convert.ToInt32(content_slot[6]) != 0)
                layout.ContentSlot7 = Convert.ToInt32(content_slot[6]);
            if (!content_slot[7].Equals("''") && Convert.ToInt32(content_slot[7]) != 0)
                layout.ContentSlot8 = Convert.ToInt32(content_slot[7]);
            if (!content_slot[8].Equals("''") && Convert.ToInt32(content_slot[8]) != 0)
                layout.ContentSlot9 = Convert.ToInt32(content_slot[8]);
            layout.DisplayAddress = displayaddr;
            layout.TemplateID = Convert.ToInt32(TemplateID);
            ECN_Framework_BusinessLayer.Communicator.Layout.Save(layout, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            return layout.LayoutID;
        }

        public void CreateAsNewInitialize(object sender, System.EventArgs e)
        {
            createAsNew = true;
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
            ECN_Framework_Entities.Communicator.Layout layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(layoutID,  true);
            if (layout.CustomerID != ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID)
                throw new SecurityException();
            
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

            hdnSlot1.Value = layout.ContentSlot1.ToString();
            lblSlot1.Text = layout.Slot1.ContentTitle.ToString();

            if (col_count > 1)
            {
                hdnSlot2.Value = layout.ContentSlot2.ToString();
                lblSlot2.Text = layout.Slot2.ContentTitle.ToString();
            }
            if (col_count > 2)
            {
                hdnSlot3.Value = layout.ContentSlot3.ToString();
                lblSlot3.Text = layout.Slot3.ContentTitle.ToString();
            }
            if (col_count > 3)
            {
                hdnSlot4.Value = layout.ContentSlot4.ToString();
                lblSlot4.Text = layout.Slot4.ContentTitle.ToString();
            }
            if (col_count > 4)
            {
                hdnSlot5.Value = layout.ContentSlot5.ToString();
                lblSlot5.Text = layout.Slot5.ContentTitle.ToString();
            }
            if (col_count > 5)
            {
                hdnSlot6.Value = layout.ContentSlot6.ToString();
                lblSlot6.Text = layout.Slot6.ContentTitle.ToString();
            }
            if (col_count > 6)
            {
                hdnSlot7.Value = layout.ContentSlot7.ToString();
                lblSlot7.Text = layout.Slot7.ContentTitle.ToString();
            }
            if (col_count > 7)
            {
                hdnSlot8.Value = layout.ContentSlot8.ToString();
                lblSlot8.Text = layout.Slot8.ContentTitle.ToString();
            }
            if (col_count > 8)
            {
                hdnSlot9.Value = layout.ContentSlot9.ToString();
                lblSlot9.Text = layout.Slot9.ContentTitle.ToString();
            }
            if (ECN_Framework_BusinessLayer.Accounts.Customer.HasProductFeature(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.DynamicContent))
            {
                HyperLink1.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=1&LayoutID=" + layout.LayoutID;
                HyperLink2.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=2&LayoutID=" + layout.LayoutID;
                HyperLink3.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=3&LayoutID=" + layout.LayoutID;
                HyperLink4.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=4&LayoutID=" + layout.LayoutID;
                HyperLink5.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=5&LayoutID=" + layout.LayoutID;
                HyperLink6.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=6&LayoutID=" + layout.LayoutID;
                HyperLink7.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=7&LayoutID=" + layout.LayoutID;
                HyperLink8.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=8&LayoutID=" + layout.LayoutID;
                HyperLink9.NavigateUrl = "../../content/contentfilters.aspx?SlotNumber=9&LayoutID=" + layout.LayoutID;
            }
            ToggleSlots();
        }

        private int getValidColumnCount(int template_id)
        {
            ECN_Framework_Entities.Communicator.Template template = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID_NoAccessCheck(template_id);
            return template.SlotsTotal.Value;
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

        private void BindDropDowns()
        {
            List<ECN_Framework_Entities.Communicator.Folder> folderList = ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.CustomerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.CNT.ToString(), ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
            folderID.DataSource = folderList;
            folderID.DataBind();
            folderID.Items.Insert(0, "root");
            folderID.Items.FindByValue("root").Value = "0";

            List<ECN_Framework_Entities.Communicator.MessageType> messageTypeList = ECN_Framework_BusinessLayer.Communicator.MessageType.GetByBaseChannelID_NoAccessCheck(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.BaseChannelID);
            var result = (from src in messageTypeList
                          where src.IsActive == true
                         orderby src.SortOrder
                         select src).ToList();
            ddlMessageType.DataSource = result.OrderBy(x => x.SortOrder);
            ddlMessageType.DataBind();
            ddlMessageType.Items.Insert(0, new ListItem("Select a Message Type", "0"));
        }

        public void getLayoutSize()
        {
            string thesize = "";
            try
            {
                ECN_Framework_Entities.Communicator.Template template = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID_NoAccessCheck(Convert.ToInt32(TemplateID));
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
                        Convert.ToInt32(hdnSlot1.Value),
                        Convert.ToInt32(hdnSlot2.Value),
                        Convert.ToInt32(hdnSlot3.Value),
                        Convert.ToInt32(hdnSlot4.Value),
                        Convert.ToInt32(hdnSlot5.Value),
                        Convert.ToInt32(hdnSlot6.Value),
                        Convert.ToInt32(hdnSlot7.Value),
                        Convert.ToInt32(hdnSlot8.Value),
                        Convert.ToInt32(hdnSlot9.Value),
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
                ECN_Framework_Entities.Communicator.Template template = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID_NoAccessCheck(Convert.ToInt32(TemplateID));
                string TemplateSource = template.TemplateSource;

                tableOptions = TableOptions.Text;

                if (tableOptions.Length < 1)
                {
                    tableOptions = " cellpadding=0 cellspacing=0 ";
                }

                body = ECN_Framework_BusinessLayer.Communicator.Layout.EmailBody_NoAccessCheck(
                    TemplateSource, "",
                    tableOptions,
                    Convert.ToInt32(hdnSlot1.Value),
                    Convert.ToInt32(hdnSlot2.Value),
                    Convert.ToInt32(hdnSlot3.Value),
                    Convert.ToInt32(hdnSlot4.Value),
                    Convert.ToInt32(hdnSlot5.Value),
                    Convert.ToInt32(hdnSlot6.Value),
                    Convert.ToInt32(hdnSlot7.Value),
                    Convert.ToInt32(hdnSlot8.Value),
                    Convert.ToInt32(hdnSlot9.Value),
                    ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML,
                    false
                    );
                body = Regex.Replace(body, @"<link(.|\n)*?>", string.Empty);
                body = Regex.Replace(body, @"<meta(.|\n)*?>", string.Empty);
                body = Regex.Replace(body, @"<title(.|\n)*?>(.|\n)*?</title>", string.Empty);              
            }
            return body;

        }

        private void ValidateSlotContents()
        {
            string message = "";

            int valid_column_count = getValidColumnCount(Convert.ToInt32(TemplateID));
            if (valid_column_count >= 1)
            {
                if (hdnSlot1.Value == "0")
                {
                    message = "1,";
                }
            }
            if (valid_column_count >= 2)
            {
                if (hdnSlot2.Value == "0")
                {
                    message += "2,";
                }
            }
            if (valid_column_count >= 3)
            {
                if (hdnSlot3.Value == "0")
                {
                    message += "3,";
                }
            }
            if (valid_column_count >= 4)
            {
                if (hdnSlot4.Value == "0")
                {
                    message += "4,";
                }
            }
            if (valid_column_count >= 5)
            {
                if (hdnSlot5.Value == "0")
                {
                    message += "5,";
                }
            }
            if (valid_column_count >= 6)
            {
                if (hdnSlot6.Value == "0")
                {
                    message += "6,";
                }
            }
            if (valid_column_count >= 7)
            {
                if (hdnSlot7.Value == "0")
                {
                    message += "7,";

                }
            }
            if (valid_column_count >= 8)
            {
                if (hdnSlot8.Value == "0")
                {
                    message += "8,";
                }
            }
            if (valid_column_count >= 9)
            {
                if (hdnSlot9.Value == "0")
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

        private void throwECNException(string message)
        {
            ECNError ecnError = new ECNError(Enums.Entity.Layout, Enums.Method.Save, message);
            List<ECNError> errorList = new List<ECNError>();
            errorList.Add(ecnError);
            throw new ECNException(errorList, Enums.ExceptionLayer.WebSite);
        }
    }
}