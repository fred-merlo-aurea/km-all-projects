using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ECN.Common.Helpers;

namespace ecn.communicator.main.ECNWizard
{
    public partial class wizardSetup_Numbers : ECN_Framework.WebPageHelper
    {
        private const string BlastTypeName = "BlastType";
        private const string AlignCenter = "center";
        private const string HoverImageFormat = "/ecn.images/images/ECNWizard/{0}_hover.png";
        private const string ImageFormat = "/ecn.images/images/ECNWizard/{0}.png";
        private const string InActiveImageFormat = "/ecn.images/images/ECNWizard/{0}_inactive.png";
        private const string HoverMouseOver = "this.src='/ecn.images/images/ECNWizard/{0}_hover.png';";
        private const string MouseOut = "this.src='/ecn.images/images/ECNWizard/{0}.png';";
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

        private string getBlastType()
        {
            return RequestQueryString(BlastTypeName, string.Empty);
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            btnNext2.ImageUrl = "/ecn.images/images/ECNWizard/next.png";
            btnNext2.Attributes.Add("onmouseover", "this.src='/ecn.images/images/ECNWizard/next_h.png';");
            btnNext2.Attributes.Add("onmouseout", "this.src='/ecn.images/images/ECNWizard/next.png';");


            btnPrevious2.ImageUrl = "/ecn.images/images/ECNWizard/previous.png";
            btnPrevious2.Attributes.Add("onmouseover", "this.src='/ecn.images/images/ECNWizard/previous_h.png';");
            btnPrevious2.Attributes.Add("onmouseout", "this.src='/ecn.images/images/ECNWizard/previous.png';");

            //btnCancel2.ImageUrl = "/ecn.images/images/ECNWizard/next.png";
            //btnCancel2.Attributes.Add("onmouseover", "this.src='/ecn.images/images/ECNWizard/next_h.png';");
            //btnCancel2.Attributes.Add("onmouseout", "this.src='/ecn.images/images/ECNWizard/next.png';");

            btnSave2.ImageUrl = "/ecn.images/images/ECNWizard/save.png";
            btnSave2.Attributes.Add("onmouseover", "this.src='/ecn.images/images/ECNWizard/save_h.png';");
            btnSave2.Attributes.Add("onmouseout", "this.src='/ecn.images/images/ECNWizard/save.png';");


            //btnHome.Attributes.Add("onmouseover", "this.src='/ecn.images/images/campaignHome_h.gif';");
            //btnHome.Attributes.Add("onmouseout", "this.src='/ecn.images/images/campaignHome.gif';");

            if (!IsPostBack)
            {
                CampaignItemID = getCampaignItemID();
                if (CampaignItemID > 0)
                {
                    ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
                    StepIndex = ci.CompletedStep == null ? 1 : Convert.ToInt32(ci.CompletedStep) + 1;
                    EnableTabBar(Convert.ToInt32(ci.CompletedStep) + 1);
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

        void LoadWizardStep(bool Initialize)
        {
            try
            {
                if (CampaignItemID > 0)
                {
                    ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(CampaignItemID, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, true);
                    EnableTabBar(Convert.ToInt32(ci.CompletedStep) + 1);
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

                //btnNext1.Visible = true;
                btnNext2.Visible = true;
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                phError.Visible = true;
                //btnNext1.Visible = false;
                btnNext2.Visible = false;
            }
        }

        private void EnableTabBar(int CompletedStep)
        {
            tabTypes = GetTabs();
            var tabLocation = "~/main/ecnwizard/controls/";

            var tableRows = new HtmlTableRow();
            var rowDesc = new HtmlTableRow();
            var tabColumn = new HtmlTableCell();
            var tabDesc = new HtmlTableCell();
            var myWizardSteps = new ArrayList();

            phError.Visible = false;
            btnCancel2.Attributes.Add("onclick", "javascript:return confirm('Are you sure you want to cancel?')");

            tabsCollectionTable.Controls.Clear();

            for (var index = 1; index <= tabTypes.Count; index++)
            {
                myWizardSteps.Add(string.Format("{0}{1}.ascx", tabLocation, tabTypes[index - 1].ToString()));
                tabColumn = new HtmlTableCell
                {
                    Align = AlignCenter
                };

                tabDesc = new HtmlTableCell
                {
                    Align = AlignCenter
                };

                var tabImage = new ImageButton
                {
                    ID = tabTypes[index - 1].ToString(),
                    CausesValidation = true
                };

                var lblDesc = new Label();
               
                if (tabImage.ID.Equals("CampaignInfo"))
                {
                    tabImage.Click += new ImageClickEventHandler(this.btnStep1_Click);
                    lblDesc.Text = "Campaign";
                }
                else if (tabImage.ID.Equals("Content"))
                {
                    tabImage.Click += new ImageClickEventHandler(this.btnStep3_Click);
                    lblDesc.Text = "Content";
                }
                else if (tabImage.ID.Equals("GroupInfo"))
                {
                    tabImage.Click += new ImageClickEventHandler(this.btnStep2_Click);
                    lblDesc.Text = "Recipients";
                }               
                else if (tabImage.ID.Equals("Preview"))
                {
                    tabImage.Click += new ImageClickEventHandler(this.btnStep4_Click);
                    lblDesc.Text = "Summary";
                }
                else if (tabImage.ID.Equals("BlastSchedule"))
                {
                    tabImage.Click += new ImageClickEventHandler(this.btnStep5_Click);
                    lblDesc.Text = "Schedule";
                }

                lblDesc.Font.Bold = true;
                lblDesc.Font.Size = FontUnit.Small;
                tabColumn.Controls.Add(tabImage);
                tabDesc.Controls.Add(lblDesc);

                rowDesc.Cells.Add(tabDesc);

                tableRows.Cells.Add(tabColumn);
                tabsCollectionTable.Rows.Add(tableRows);
                tabsCollectionTable.Rows.Add(rowDesc);

                tabDesc = new HtmlTableCell();
                rowDesc.Cells.Add(tabDesc);
                tabsCollectionTable.Rows.Add(rowDesc);

                tabImage = AddImageAttributes(tabImage, index, CompletedStep);

                //For adding arrow between steps
                if (index < tabTypes.Count)
                {
                    tableRows.Cells.Add(GetArrow());
                    tabsCollectionTable.Rows.Add(tableRows);
                }
            }

            WizardSteps = myWizardSteps;           
        }

        private HtmlTableCell GetArrow()
        {
            var tabColumn = new HtmlTableCell
            {
                Width = "75%"
            };
            tabColumn.Style.Add("padding-left", "0px");
            var arrows = new Image
            {
                ImageUrl = "/ecn.images/images/ECNWizard/next_arrow.jpg"
            };
            tabColumn.Controls.Add(arrows);

            return tabColumn;
        }

        private ImageButton AddImageAttributes(ImageButton tabImageButton, int index, int completedStep)
        {
            if (index == StepIndex)
            {
                tabImageButton.ImageUrl = string.Format(HoverImageFormat, index);
                tabImageButton.Attributes.Add("style", "cursor:default");
                tabImageButton.Attributes.Add("onclick", string.Empty);
                tabImageButton.Attributes.Add("onmouseover", string.Empty);
                tabImageButton.Attributes.Add("onmouseout", string.Empty);
                tabImageButton.Enabled = false;
            }
            else if (index <= completedStep)
            {
                tabImageButton.ImageUrl = string.Format(ImageFormat, index);
                tabImageButton.Attributes.Add("onmouseover", string.Format(HoverImageFormat, index));
                tabImageButton.Attributes.Add("onmouseout", string.Format(MouseOut, index));
                tabImageButton.Attributes.Add("style", "cursor:hand");
                tabImageButton.Attributes.Add("onclick", string.Empty);
                tabImageButton.Enabled = true;
            }
            else
            {
                tabImageButton.ImageUrl = string.Format(InActiveImageFormat, index);
                tabImageButton.Attributes.Add("style", "cursor:default");
                tabImageButton.Attributes.Add("onclick", "javascript:return false;");
                tabImageButton.Attributes.Add("onmouseover", string.Empty);
                tabImageButton.Attributes.Add("onmouseout", string.Empty);
                tabImageButton.Enabled = false;
            }

            return tabImageButton;
        }

        private ArrayList GetTabs()
        {
            var blastType = getBlastType();
            var myTabs = new ArrayList
            {
                "WizardCampaign"
            };

            switch (blastType)
            {
                case "regular":
                    myTabs.Add("WizardGroup");
                    myTabs.Add("WizardContent_Regular");
                    myTabs.Add("WizardPreview_Regular");
                    break;
                case "sms":
                    myTabs.Add("WizardGroup");
                    myTabs.Add("WizardContent_SMS");
                    myTabs.Add("WizardPreview_SMS");
                    break;
                case "ab":
                    myTabs.Add("WizardGroup");
                    myTabs.Add("WizardContent_AB");
                    myTabs.Add("WizardPreview_AB");
                    break;
                case "champion":
                    myTabs.Add("WizardContent_Champion");
                    break;
            }

            myTabs.Add("WizardSchedule");

            return myTabs;
        }

        protected void btnPrevious_Click(object sender, System.EventArgs e)
        {
            StepIndex--;
            LoadWizardStep(true);
        }

        protected void btnNext_Click(object sender, System.EventArgs e)
        {

            //((IECNWizard)ctlWizard).WizardID = WizardID;

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

        protected void Page_PreRender(object sender, System.EventArgs e)
        {

            if (((IECNWizard)ctlWizard) == null)
            {
                //btnNext1.Visible = false;
                btnNext2.Visible = false;
                //btnSave1.Visible = false;
                btnSave2.Visible = false;
                //btnCancel1.Visible = false;
                btnCancel2.Visible = false;
                //btnPrevious1.Visible = false;
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
                    //btnPrevious1.Visible = false;
                    btnPrevious2.Visible = false;
                }
                else
                {
                    //btnPrevious1.Visible = true;
                    btnPrevious2.Visible = true;
                }

                if (StepIndex == WizardSteps.Count)
                {
                    //btnNext1.Visible = false;
                    btnNext2.Visible = false;
                    //btnSave1.Visible = false;
                    btnSave2.Visible = false;
                }
                else
                {
                    //btnNext1.Visible = true;
                    btnNext2.Visible = true;
                    //btnSave1.Visible = true;
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
            SaveonTabClick(3);
        }

        private void btnStep4_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveonTabClick(4);
        }

        private void btnStep5_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveonTabClick(5);
        }

        private void SaveonTabClick(int Step)
        {
            ((IECNWizard)ctlWizard).CampaignItemID = CampaignItemID;
            if (((IECNWizard)ctlWizard).Save())
            {
                StepIndex = Step;
                LoadWizardStep(true);
            }
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            ((IECNWizard)ctlWizard).CampaignItemID = CampaignItemID;
            if (((IECNWizard)ctlWizard).Save())
                Response.Redirect("default.aspx");
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