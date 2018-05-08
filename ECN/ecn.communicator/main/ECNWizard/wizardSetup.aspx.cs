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
using System.Collections.Generic;
using System.Transactions;
using ECN.Common.Helpers;
using ECN_Framework_Common.Objects;

namespace ecn.communicator.main.ECNWizard
{
    public partial class wizardSetup : ECN_Framework.WebPageHelper
    {
        private const string CampaignItemTypeName = "CampaignItemType";
        private const string WizardCampaign = "WizardCampaign";
        private const string Regular = "regular";
        private const string SocialShare = "SocialShare";
        private const string WizardGroup = "WizardGroup";
        private const string WizardContentRegular = "WizardContent_Regular";
        private const string WizardPreviewRegular = "WizardPreview_Regular";
        private const string WizardContentSms = "WizardContent_SMS";
        private const string WizardPreviewSms = "WizardPreview_SMS";
        private const string WizardContentAb = "WizardContent_AB";
        private const string WizardPreviewAb = "WizardPreview_AB";
        private const string WizardContentChampion = "WizardContent_Champion";
        private const string WizardSfCampaign = "WizardSFCampaign";
        private const string WizardPreviewSf = "WizardPreview_SF";
        private const string WizardSchedule = "WizardSchedule";
        private const string OnClick = "onclick";
        private const string OnMouseOver = "onmouseover";
        private const string OnMouseOut = "onmouseout";
        private const string JavascriptCancelConfirmation = "javascript:return confirm('Are you sure you want to cancel?')";
        private const string JavascriptReturnFalse = "javascript:return false;";
        private const string Center = "center";
        private const string Campaign = "Campaign";
        private const string Recipients = "Recipients";
        private const string Summary = "Summary";
        private const string Schedule = "Schedule";
        private const string SocialSpaceShare = "Social Share";
        private const string CampaignSmall = "campaign";
        private const string Layout = "layout";
        private const string Group = "group";
        private const string Preview = "preview";
        private const string ScheduleSmall = "schedule";
        private const string Social = "social";
        private const string ImagesPath = "/ecn.images/images/ECNWizard/";
        private const string Style = "style";
        private const string CursorDefault = "cursor:default";
        private const string CursorHand = "cursor:hand";
        private const string ImageCurrentSuffix = "_current.png";
        private const string ImageCompletedSuffix = "_completed.png";
        private const string ImageInactiveSuffix = "_inactive.png";
        private const string Sms = "sms";
        private const string Ab = "ab";
        private const string Champion = "champion";
        private const string SalesForce = "salesforce";
        Control ctlWizard;

        private ArrayList WizardSteps
        {
            get
            {
                if (ViewState["WizardSteps"] == null)
                    return new ArrayList();
                else
                    return (ArrayList)ViewState["WizardSteps"];
            }

            set { ViewState["WizardSteps"] = value; }
        }

        private ArrayList tabTypes
        {
            get
            {
                if (ViewState["tabTypes"] == null)
                    return new ArrayList();
                else
                    return (ArrayList)ViewState["tabTypes"];
            }

            set { ViewState["tabTypes"] = value; }
        }

        private int StepIndex
        {
            get
            {
                if (ViewState["StepIndex"] == null)
                    return 1;
                else
                    return (int)ViewState["StepIndex"];
            }

            set { ViewState["StepIndex"] = value; }
        }

        private int CampaignItemID
        {
            get { return ViewStateHelper.GetFromViewState(ViewState, nameof(CampaignItemID), 0); }
            set { ViewStateHelper.SetViewState(ViewState, nameof(CampaignItemID), value); }
        }

        private string getCampaignItemType()
        {
            return RequestQueryString(CampaignItemTypeName, string.Empty);
        }

        private int getPrePopBlastID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["PrePopBlastID"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private int getPrePopSmartSegmentID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["PrePopSmartSegmentID"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        private void setECNError(ECN_Framework_Common.Objects.ECNException ecnException)
        {
            EcnErrorHelper.SetEcnError(phError, lblErrorMessage, ecnException);
        }

        private void setECNWarning(ECN_Framework_Common.Objects.ECNWarning ecnException, bool bVisible = true)
        {
            phWarning.Visible = bVisible;
            lblWarningMessage.Text = string.Empty;
            foreach (ECN_Framework_Common.Objects.ECNWarning ecnError in ecnException.ErrorList)
            {
                lblWarningMessage.Text = lblErrorMessage.Text + "<br/>" + ecnError.Entity + ": " + ecnError.WarningMessage;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            phError.Visible = false;
            btnNext1.Attributes.Add(OnMouseOver, "this.src='/ecn.images/images/next_h.gif';");
            btnNext1.Attributes.Add(OnMouseOut, "this.src='/ecn.images/images/next.gif';");
            btnNext2.Attributes.Add(OnMouseOver, "this.src='/ecn.images/images/next_h.gif';");
            btnNext2.Attributes.Add(OnMouseOut, "this.src='/ecn.images/images/next.gif';");

            btnPrevious1.Attributes.Add(OnMouseOver, "this.src='/ecn.images/images/Previous_h.gif';");
            btnPrevious1.Attributes.Add(OnMouseOut, "this.src='/ecn.images/images/Previous.gif';");
            btnPrevious2.Attributes.Add(OnMouseOver, "this.src='/ecn.images/images/Previous_h.gif';");
            btnPrevious2.Attributes.Add(OnMouseOut, "this.src='/ecn.images/images/Previous.gif';");

            btnCancel1.Attributes.Add(OnMouseOver, "this.src='/ecn.images/images/Cancel_h.gif';");
            btnCancel1.Attributes.Add(OnMouseOut, "this.src='/ecn.images/images/Cancel.gif';");
            btnCancel2.Attributes.Add(OnMouseOver, "this.src='/ecn.images/images/Cancel_h.gif';");
            btnCancel2.Attributes.Add(OnMouseOut, "this.src='/ecn.images/images/Cancel.gif';");

            btnSave1.Attributes.Add(OnMouseOver, "this.src='/ecn.images/images/Save_h.gif';");
            btnSave1.Attributes.Add(OnMouseOut, "this.src='/ecn.images/images/Save.gif';");
            btnSave2.Attributes.Add(OnMouseOver, "this.src='/ecn.images/images/Save_h.gif';");
            btnSave2.Attributes.Add(OnMouseOut, "this.src='/ecn.images/images/Save.gif';");

            if (!IsPostBack)
            {
                checkPrepopulateBlast();
                CampaignItemID = getCampaignItemID();
                if (CampaignItemID > 0)
                {
                    if (Request.QueryString.ToString().Contains("simple=") && (Request.QueryString.ToString().Contains("code=") || Request.QueryString.ToString().Contains("error=") || Request.QueryString.ToString().Contains("denied=")))
                    {
                        if (getCampaignItemType().Equals(Champion, StringComparison.OrdinalIgnoreCase))
                        {
                            StepIndex = 3;
                            EnableTabBar(StepIndex + 1);
                        }
                        else
                        {
                            StepIndex = 4;
                            EnableTabBar(StepIndex + 1);
                        }
                    }
                    else
                    {
                        ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);

                        StepIndex = ci.CompletedStep == null ? 1 : Convert.ToInt32(ci.CompletedStep) + 1;
                        EnableTabBar(Convert.ToInt32(ci.CompletedStep) + 1);

                    }
                }
                else
                {
                    StepIndex = 1;
                }
                LoadWizardStep(true);
            }
            else
            {
                LoadWizardStep(false);
            }

        }

        private void checkPrepopulateBlast()
        {
            int PrePopBlastID = getPrePopBlastID();
            if (PrePopBlastID > 0)
            {
                ECN_Framework_Entities.Communicator.Blast sourceBlast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(PrePopBlastID, Master.UserSession.CurrentUser, false);
                int PrePopSmartSegmentID = getPrePopSmartSegmentID();
                if (PrePopSmartSegmentID > 0)
                {
                    ECN_Framework_Entities.Communicator.CampaignItem ciSource =
                    ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID(PrePopBlastID, Master.UserSession.CurrentUser, false);

                    ECN_Framework_Entities.Communicator.CampaignItem ciNew = new ECN_Framework_Entities.Communicator.CampaignItem();
                    ciNew.CampaignItemType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Regular.ToString();
                    ciNew.CampaignItemName = "Suppressed-BlastID " + PrePopBlastID.ToString() + DateTime.Now.ToString();
                    ciNew.CampaignItemNameOriginal = "Suppressed-BlastID " + PrePopBlastID.ToString();
                    ciNew.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                    ciNew.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                    ciNew.FromName = ciSource.FromName;
                    ciNew.FromEmail = ciSource.FromEmail;
                    ciNew.ReplyTo = ciSource.ReplyTo;
                    ciNew.BlastField1 = ciSource.BlastField1;
                    ciNew.BlastField2 = ciSource.BlastField2;
                    ciNew.BlastField3 = ciSource.BlastField3;
                    ciNew.BlastField4 = ciSource.BlastField4;
                    ciNew.BlastField5 = ciSource.BlastField5;
                    ciNew.CampaignID = ciSource.CampaignID;
                    ciNew.CampaignItemFormatType = ciSource.CampaignItemFormatType;
                    ciNew.IsHidden = false;
                    ciNew.CompletedStep = 4;

                    List<ECN_Framework_Entities.Communicator.CampaignItemBlast> ciBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlast>();

                    ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlastNew = new ECN_Framework_Entities.Communicator.CampaignItemBlast();
                    ciBlastNew.CampaignItemID = ciNew.CampaignItemID;
                    ciBlastNew.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                    ciBlastNew.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                    //ciBlastNew.SmartSegmentID = PrePopSmartSegmentID;
                    ciBlastNew.GroupID = sourceBlast.GroupID;
                    ciBlastNew.LayoutID = sourceBlast.LayoutID;
                    ciBlastNew.DynamicFromEmail = sourceBlast.DynamicFromEmail;
                    ciBlastNew.DynamicFromName = sourceBlast.DynamicFromName;
                    ciBlastNew.DynamicReplyTo = sourceBlast.DynamicReplyToEmail;
                    ciBlastNew.EmailSubject = sourceBlast.EmailSubject;

                    

                    List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast> refBlastList = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast>();
                    ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast ciRefBlastNew = new ECN_Framework_Entities.Communicator.CampaignItemBlastRefBlast();
                    ciRefBlastNew.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                    ciRefBlastNew.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                    ciRefBlastNew.RefBlastID = sourceBlast.BlastID;

                    refBlastList.Add(ciRefBlastNew);
                    ciBlastNew.RefBlastList = refBlastList;

                    ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                    cibf.SmartSegmentID = PrePopSmartSegmentID;
                    cibf.RefBlastIDs = sourceBlast.BlastID.ToString();
                    ciBlastNew.Filters.Add(cibf);

                    ciBlastList.Add(ciBlastNew);



                    using (TransactionScope scope = new TransactionScope())
                    {
                        ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save(ciNew, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        foreach (ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast in ciBlastList)
                        {
                            ciBlast.CampaignItemID = ciNew.CampaignItemID;
                        }
                        ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Save(ciNew.CampaignItemID, ciBlastList, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser);
                        scope.Complete();
                    }
                    Response.Redirect("wizardSetup.aspx?campaignItemID=" + ciNew.CampaignItemID + "&campaignItemType=" + ciNew.CampaignItemType);
                }
            }
        }

        void LoadWizardStep(bool Initialize)
        {
            try
            {
                if (CampaignItemID > 0)
                {
                    ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, false);
                    EnableTabBar(Convert.ToInt32(ci.CompletedStep) + 1);
                    //Displays campaign item name in upper left of wizard control, with hacky shortening for long names
                    if (ci.CampaignItemName.Length <= 30)
                    {
                        lblCampaignItemName.Text = "Campaign Item: " + ci.CampaignItemName;
                    }
                    else
                    {
                        lblCampaignItemName.Text = "Campaign Item: " + ci.CampaignItemName.Substring(0, 30) + "...";
                    }
                }
                else
                {
                    EnableTabBar(0);
                }

                ctlWizard = Page.LoadControl((string)WizardSteps[StepIndex - 1]);
                ctlWizard.ID = "ECNWizard";
                ((IECNWizard)ctlWizard).CampaignItemID = CampaignItemID;

                phwizContent.Controls.Clear();
                phwizContent.Controls.Add(ctlWizard);

                if (Initialize)
                    ((IECNWizard)ctlWizard).Initialize();

                btnNext1.Visible = true;
                btnNext2.Visible = true;
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                phError.Visible = true;
                btnNext1.Visible = false;
                btnNext2.Visible = false;
            }
        }

        private void EnableTabBar(int completedStep)
        {
            var campaignItemType = getCampaignItemType();

            var myTabs = new ArrayList { WizardCampaign };

            if(campaignItemType.Equals(Regular, StringComparison.OrdinalIgnoreCase))
            {
                myTabs.AddRange(new[] { WizardGroup, WizardContentRegular, SocialShare, WizardPreviewRegular });
            }
            else if(campaignItemType.Equals(Sms, StringComparison.OrdinalIgnoreCase))
            {
                myTabs.AddRange(new[] { WizardGroup, WizardContentSms, WizardPreviewSms });
            }
            else if(campaignItemType.Equals(Ab, StringComparison.OrdinalIgnoreCase))
            {
                myTabs.AddRange(new[] { WizardGroup, WizardContentAb, WizardPreviewAb });
            }
            else if(campaignItemType.Equals(Champion, StringComparison.OrdinalIgnoreCase))
            {
                myTabs.AddRange(new[] { WizardContentChampion, SocialShare });
            }
            else if(campaignItemType.Equals(SalesForce, StringComparison.OrdinalIgnoreCase))
            {
                myTabs.AddRange(new[] { WizardSfCampaign, WizardContentRegular, SocialShare, WizardPreviewSf });
            }

            myTabs.Add(WizardSchedule);
            tabTypes = myTabs;

            phError.Visible = false;
            btnCancel1.Attributes.Add(OnClick, JavascriptCancelConfirmation);
            btnCancel2.Attributes.Add(OnClick, JavascriptCancelConfirmation);

            var myWizardSteps = GetWizardSteps(completedStep);

            WizardSteps = myWizardSteps;
        }

        private ArrayList GetWizardSteps(int completedStep)
        {
            const string tabLocation = "~/main/ecnwizard/controls/";
            var tableRows = new HtmlTableRow();
            var rowDesc = new HtmlTableRow();
            var myWizardSteps = new ArrayList();
            tabsCollectionTable.Controls.Clear();
            var lblDesc = new Label();

            for(var i = 1; i <= tabTypes.Count; i++)
            {
                myWizardSteps.Add($"{tabLocation}{tabTypes[i - 1]}.ascx");
                var tabColumn = new HtmlTableCell {Align = Center};
                var tabImage = new ImageButton {ID = tabTypes[i - 1].ToString(), CausesValidation = true};

                if(tabImage.ID.Equals(WizardCampaign))
                {
                    PopulateTabImage(i, completedStep, tabImage, lblDesc, Campaign, CampaignSmall, btnStep1_Click);
                }
                else if(tabImage.ID.Equals(WizardContentRegular) || tabImage.ID.Equals(WizardContentSms) ||
                        tabImage.ID.Equals(WizardContentAb) || tabImage.ID.Equals(WizardContentChampion))
                {
                    PopulateTabImage(i, completedStep, tabImage, lblDesc, nameof(Content), Layout, btnStep3_Click);
                }
                else if(tabImage.ID.Equals(WizardGroup) || tabImage.ID.Equals(WizardSfCampaign))
                {
                    PopulateTabImage(i, completedStep, tabImage, lblDesc, Recipients, Group, btnStep2_Click);
                }
                else if(tabImage.ID.Equals(WizardPreviewRegular) || 
                        tabImage.ID.Equals(WizardPreviewAb) ||
                        tabImage.ID.Equals(WizardPreviewSms) || 
                        tabImage.ID.Equals(WizardPreviewSf))
                {
                    PopulateTabImage(i, completedStep, tabImage, lblDesc, Summary, Preview, btnStep5_Click);
                }
                else if(tabImage.ID.Equals(WizardSchedule))
                {
                    PopulateTabImage(i, completedStep, tabImage, lblDesc, Schedule, ScheduleSmall, btnStep6_Click);
                }
                else if(tabImage.ID.Equals(SocialShare))
                {
                    PopulateTabImage(i, completedStep, tabImage, lblDesc, SocialSpaceShare, Social, btnStep4_Click);
                }
                else if(string.IsNullOrWhiteSpace(tabImage.ID))
                {
                    lblDesc.Font.Bold = true;
                }

                lblDesc.Font.Size = FontUnit.Small;
                tabColumn.Controls.Add(tabImage);
                tableRows.Cells.Add(tabColumn);
                rowDesc.Cells.Add(new HtmlTableCell { Align = Center });
                tabsCollectionTable.Rows.Add(tableRows);
                tabsCollectionTable.Rows.Add(rowDesc);
                rowDesc.Cells.Add(new HtmlTableCell());
                tabsCollectionTable.Rows.Add(rowDesc);
            }

            return myWizardSteps;
        }

        private void PopulateTabImage(
            int tabIndex, 
            int completedStep, 
            ImageButton tabImage, 
            Label lblDesc,
            string imagePrefix, 
            string labelText, 
            Action<object, ImageClickEventArgs> eventHandler)
        {
            if (tabIndex == StepIndex)
            {
                tabImage.ImageUrl = $"{ImagesPath}{imagePrefix}{ImageCurrentSuffix}";
                tabImage.Attributes.Add(Style, CursorDefault);
                tabImage.Attributes.Add(OnClick, string.Empty);
                tabImage.Attributes.Add(OnMouseOver, string.Empty);
                tabImage.Attributes.Add(OnMouseOut, string.Empty);
                tabImage.Enabled = false;
            }
            else if (tabIndex <= completedStep)
            {
                tabImage.ImageUrl = $"{ImagesPath}{imagePrefix}{ImageCompletedSuffix}";
                tabImage.Attributes.Add(OnMouseOver,
                    $"this.src=\'{ImagesPath}{imagePrefix}{ImageCurrentSuffix}\';");
                tabImage.Attributes.Add(OnMouseOut,
                    $"this.src=\'{ImagesPath}{imagePrefix}{ImageCompletedSuffix}\';");
                tabImage.Attributes.Add(Style, CursorHand);
                tabImage.Attributes.Add(OnClick, string.Empty);
                tabImage.Enabled = true;
            }
            else
            {
                tabImage.ImageUrl = $"{ImagesPath}{imagePrefix}{ImageInactiveSuffix}";
                tabImage.Attributes.Add(Style, CursorDefault);
                tabImage.Attributes.Add(OnClick, JavascriptReturnFalse);
                tabImage.Attributes.Add(OnMouseOver, string.Empty);
                tabImage.Attributes.Add(OnMouseOut, string.Empty);
                tabImage.Enabled = false;
            }

            tabImage.Click += (sender, eventArgs) => eventHandler?.Invoke(sender, eventArgs);
            lblDesc.Text = labelText;
        }

        protected void btnPrevious_Click(object sender, System.EventArgs e)
        {
            StepIndex--;
            LoadWizardStep(true);
        }

        protected void btnNext_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (((IECNWizard)ctlWizard).Save())
                {
                    CampaignItemID = ((IECNWizard)ctlWizard).CampaignItemID;
                    StepIndex++;
                    if (StepIndex == WizardSteps.Count + 1)
                        Response.Redirect("default.aspx");
                    else
                        LoadWizardStep(true);
                }
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
            catch (ECN_Framework_Common.Objects.ECNWarning ex)
            {
                setECNWarning(ex, false);
            }
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {

            if (((IECNWizard)ctlWizard) == null)
            {
                btnNext1.Visible = false;
                btnNext2.Visible = false;
                btnSave1.Visible = false;
                btnSave2.Visible = false;
                btnCancel1.Visible = false;
                btnCancel2.Visible = false;
                btnPrevious1.Visible = false;
                btnPrevious2.Visible = false;
            }
            else
            {
                if (((IECNWizard)ctlWizard).ErrorMessage != string.Empty)
                {
                    lblErrorMessage.Text = ((IECNWizard)ctlWizard).ErrorMessage;
                    phError.Visible = true;
                }

                if (StepIndex == 1)
                {
                    btnPrevious1.Visible = false;
                    btnPrevious2.Visible = false;
                }
                else
                {
                    btnPrevious1.Visible = true;
                    btnPrevious2.Visible = true;
                }

                if (StepIndex == WizardSteps.Count)
                {
                    btnNext1.Visible = false;
                    btnNext2.Visible = false;
                    btnSave1.Visible = false;
                    btnSave2.Visible = false;
                }
                else
                {
                    btnNext1.Visible = true;
                    btnNext2.Visible = true;
                    btnSave1.Visible = true;
                    btnSave2.Visible = true;

                }
            }
        }

        private void btnStep1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveonTabClick(1);
        }

        private void btnStep2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveonTabClick(2);
        }

        private void btnStep3_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (getCampaignItemType().ToLower().Equals(Champion))
                SaveonTabClick(2);
            else
                SaveonTabClick(3);
        }

        private void btnStep4_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (getCampaignItemType().ToLower().Equals(Champion))
            {
                SaveonTabClick(3);
            }
            else
            {
                SaveonTabClick(4);
            }
        }

        private void btnStep5_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveonTabClick(5);
        }
        private void btnStep6_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (getCampaignItemType().ToLower().Equals(Champion))
                SaveonTabClick(4);
            else if (getCampaignItemType().ToLower().Equals(Ab))
                SaveonTabClick(5);
            else
                SaveonTabClick(6);
        }

        private void SaveonTabClick(int Step)
        {
            try
            {
                ((IECNWizard)ctlWizard).CampaignItemID = CampaignItemID;
                if (((IECNWizard)ctlWizard).Save())
                {
                    StepIndex = Step;
                    LoadWizardStep(true);
                }
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
            catch (ECN_Framework_Common.Objects.ECNWarning ex)
            {
                setECNWarning(ex, false);
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                ((IECNWizard)ctlWizard).CampaignItemID = CampaignItemID;
                if (((IECNWizard)ctlWizard).Save())
                    Response.Redirect("default.aspx");
            }
            catch (ECN_Framework_Common.Objects.ECNException ex)
            {
                setECNError(ex);
            }
            catch (ECN_Framework_Common.Objects.ECNWarning ex)
            {
                setECNWarning(ex, false);
            }
        }

        private void btnHome_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ((IECNWizard)ctlWizard).CampaignItemID = CampaignItemID;

            if (((IECNWizard)ctlWizard) == null)
            {
                Response.Redirect("default.aspx");
            }
            else
            {
                if (((IECNWizard)ctlWizard).Save())
                    Response.Redirect("default.aspx");
            }
        }
    }
}