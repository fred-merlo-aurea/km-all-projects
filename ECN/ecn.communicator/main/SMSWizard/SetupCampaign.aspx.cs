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
using ecn.common.classes;
using ecn.communicator.classes;

namespace ecn.communicator.main.SMSWizard
{

    public partial class SetupCampaign : ECN_Framework.WebPageHelper
    {

        Control ctlWizard;

        ArrayList WizardSteps = new ArrayList();


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

        private int WizardID
        {
            get
            {
                if (ViewState["WizardID"] == null)
                    return 0;
                else
                    return (int)ViewState["WizardID"];
            }

            set { ViewState["WizardID"] = value; }
        }

        private int getWizardID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["WizardID"].ToString());
            }
            catch
            {
                return 0;
            }
        }


        protected void Page_Load(object sender, System.EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Communicator.Enums.MenuCode.INDEX; 
            Master.SubMenu = "";
            Master.Heading = "";
            Master.HelpContent = "";
            Master.HelpTitle = "";	

            phError.Visible = false;

            btnCancel1.Attributes.Add("onclick", "javascript:return confirm('Are you sure you want to cancel?')");
            btnCancel2.Attributes.Add("onclick", "javascript:return confirm('Are you sure you want to cancel?')");
            WizardSteps.Add("controls/CampaignInfo.ascx");
            WizardSteps.Add("controls/GroupInfo.ascx");
            WizardSteps.Add("controls/ContentInfo.ascx");
            WizardSteps.Add("controls/Preview.ascx");
            WizardSteps.Add("controls/BlastSchedule.ascx");

            btnNext1.Attributes.Add("onmouseover", "this.src='/ecn.images/images/next_h.gif';");
            btnNext1.Attributes.Add("onmouseout", "this.src='/ecn.images/images/next.gif';");
            btnNext2.Attributes.Add("onmouseover", "this.src='/ecn.images/images/next_h.gif';");
            btnNext2.Attributes.Add("onmouseout", "this.src='/ecn.images/images/next.gif';");

            btnPrevious1.Attributes.Add("onmouseover", "this.src='/ecn.images/images/Previous_h.gif';");
            btnPrevious1.Attributes.Add("onmouseout", "this.src='/ecn.images/images/Previous.gif';");
            btnPrevious2.Attributes.Add("onmouseover", "this.src='/ecn.images/images/Previous_h.gif';");
            btnPrevious2.Attributes.Add("onmouseout", "this.src='/ecn.images/images/Previous.gif';");

            btnCancel1.Attributes.Add("onmouseover", "this.src='/ecn.images/images/Cancel_h.gif';");
            btnCancel1.Attributes.Add("onmouseout", "this.src='/ecn.images/images/Cancel.gif';");
            btnCancel2.Attributes.Add("onmouseover", "this.src='/ecn.images/images/Cancel_h.gif';");
            btnCancel2.Attributes.Add("onmouseout", "this.src='/ecn.images/images/Cancel.gif';");

            btnSave1.Attributes.Add("onmouseover", "this.src='/ecn.images/images/Save_h.gif';");
            btnSave1.Attributes.Add("onmouseout", "this.src='/ecn.images/images/Save.gif';");
            btnSave2.Attributes.Add("onmouseover", "this.src='/ecn.images/images/Save_h.gif';");
            btnSave2.Attributes.Add("onmouseout", "this.src='/ecn.images/images/Save.gif';");

            btnHome.Attributes.Add("onmouseover", "this.src='/ecn.images/images/campaignHome_h.gif';");
            btnHome.Attributes.Add("onmouseout", "this.src='/ecn.images/images/campaignHome.gif';");


            if (!IsPostBack)
            {
                if (!(KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser)))
                {
                    //if (!(KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "blastpriv") && KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "createblast")) || KMPlatform.BusinessLogic.User.HasPermission(Master.UserSession.CurrentUser.UserID,  "approvalblast"))
                    if (!KM.Platform.User.HasAccess(Master.UserSession.CurrentUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Blast, KMPlatform.Enums.Access.View))				
                    {
                        lblErrorMessage.Text = "ERROR - You don't have permission to create the message.";
                        phError.Visible = true;
                        btnNext1.Visible = false;
                        btnNext2.Visible = false;
                        return;
                    }
                }

                WizardID = getWizardID();
                if (WizardID > 0)
                {
                    ecn.communicator.classes.Wizard w = ecn.communicator.classes.Wizard.GetWizardbyID(WizardID);

                    if (w != null && w.BlastID == 0)
                    {
                        StepIndex = w.CompletedStep == 0 ? 1 : w.CompletedStep + 1;

                        EnableTabBar(w.CompletedStep + 1);
                    }
                    else
                    {
                        lblErrorMessage.Text = "ERROR - Message does not exist or has been completed.";
                        phError.Visible = true;
                        btnNext1.Visible = false;
                        btnNext2.Visible = false;
                        return;
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

        void LoadWizardStep(bool Initialize)
        {
            try
            {
                if (WizardID > 0)
                {
                    ecn.communicator.classes.Wizard w = ecn.communicator.classes.Wizard.GetWizardbyID(WizardID);
                    EnableTabBar(w.CompletedStep + 1);
                }
                else
                {
                    EnableTabBar(0);
                }

                ctlWizard = Page.LoadControl((string)WizardSteps[StepIndex - 1]);

                ctlWizard.ID = "ECNWizard";
                ((IWizard)ctlWizard).WizardID = WizardID;

                phwizContent.Controls.Clear();
                phwizContent.Controls.Add(ctlWizard);

                if (Initialize)
                    ((IWizard)ctlWizard).Initialize();

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

        private void EnableTabBar(int CompletedStep)
        {
            for (int i = 1; i <= 5; i++)
            {
                //ImageButton tab = (ImageButton)Page.FindControl("btnStep" + i);

                ContentPlaceHolder mainContent = (ContentPlaceHolder)this.Master.FindControl("ContentPlaceHolder1");
                ImageButton tab = (ImageButton)mainContent.FindControl("btnStep" + i);

                if (i == StepIndex)
                {
                    tab.ImageUrl = "/ecn.images/images/wiz_step" + i + "_s.jpg";
                    tab.Attributes.Add("style", "cursor:default");
                    tab.Attributes.Add("onclick", "");
                    tab.Attributes.Add("onmouseover", "");
                    tab.Attributes.Add("onmouseout", "");
                    tab.Enabled = false;
                }
                else if (i <= CompletedStep)
                {
                    tab.ImageUrl = "/ecn.images/images/wiz_step" + i + "_d.jpg";
                    tab.Attributes.Add("onmouseover", "this.src='/ecn.images/images/wiz_step" + i + "_h.jpg';");
                    tab.Attributes.Add("onmouseout", "this.src='/ecn.images/images/wiz_step" + i + "_d.jpg';");
                    tab.Attributes.Add("style", "cursor:hand");
                    tab.Attributes.Add("onclick", "");
                    tab.Enabled = true;
                }
                else
                {

                    tab.ImageUrl = "/ecn.images/images/wiz_step" + i + "_dd.jpg";
                    tab.Attributes.Add("style", "cursor:default");
                    tab.Attributes.Add("onclick", "javascript:return false;");
                    tab.Attributes.Add("onmouseover", "");
                    tab.Attributes.Add("onmouseout", "");
                    tab.Enabled = false;
                }
            }

        }
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }
        #region events commented
        //		this.btnStep1.Click += new System.Web.UI.ImageClickEventHandler(this.btnStep1_Click);
        //		this.btnStep2.Click += new System.Web.UI.ImageClickEventHandler(this.btnStep2_Click);
        //		this.btnStep3.Click += new System.Web.UI.ImageClickEventHandler(this.btnStep3_Click);
        //		this.btnStep4.Click += new System.Web.UI.ImageClickEventHandler(this.btnStep4_Click);
        //		this.btnStep5.Click += new System.Web.UI.ImageClickEventHandler(this.btnStep5_Click);
        //		this.btnPrevious1.Click += new System.Web.UI.ImageClickEventHandler(this.btnPrevious_Click);
        //		this.btnNext1.Click += new System.Web.UI.ImageClickEventHandler(this.btnNext_Click);
        //		this.btnPrevious2.Click += new System.Web.UI.ImageClickEventHandler(this.btnPrevious_Click);
        //		this.btnNext2.Click += new System.Web.UI.ImageClickEventHandler(this.btnNext_Click);
        //		this.Load += new System.EventHandler(this.Page_Load);
        //		this.PreRender += new System.EventHandler(this.Page_PreRender);
        #endregion

        private void InitializeComponent()
        {
            this.btnHome.Click += new System.Web.UI.ImageClickEventHandler(this.btnHome_Click);
            this.btnStep1.Click += new System.Web.UI.ImageClickEventHandler(this.btnStep1_Click);
            this.btnStep2.Click += new System.Web.UI.ImageClickEventHandler(this.btnStep2_Click);
            this.btnStep3.Click += new System.Web.UI.ImageClickEventHandler(this.btnStep3_Click);
            this.btnStep4.Click += new System.Web.UI.ImageClickEventHandler(this.btnStep4_Click);
            this.btnStep5.Click += new System.Web.UI.ImageClickEventHandler(this.btnStep5_Click);

        }
        #endregion

        protected void btnPrevious_Click(object sender, System.EventArgs e)
        {
            StepIndex--;
            LoadWizardStep(true);
        }

        protected void btnNext_Click(object sender, System.EventArgs e)
        {

            //((IWizard)ctlWizard).WizardID = WizardID;

            if (((IWizard)ctlWizard).Save())
            {
                WizardID = ((IWizard)ctlWizard).WizardID;

                StepIndex++;
                if (StepIndex == WizardSteps.Count + 1)
                    Response.Redirect("default.aspx");
                else
                    LoadWizardStep(true);
            }
        }

        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            if (((IWizard)ctlWizard) == null)
            {
                btnNext1.Visible = false;
                btnNext2.Visible = false;
                btnSave1.Visible = false;
                btnSave2.Visible = false;
                btnCancel1.Visible = false;
                btnCancel2.Visible = false;
                btnPrevious1.Visible = false;
                btnPrevious2.Visible = false;
                btnStep1.Visible = false;
                btnStep2.Visible = false;
                btnStep3.Visible = false;
                btnStep4.Visible = false;
                btnStep5.Visible = false;
            }
            else
            {
                if (((IWizard)ctlWizard).ErrorMessage != string.Empty)
                {
                    lblErrorMessage.Text = ((IWizard)ctlWizard).ErrorMessage;
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
            //((IWizard)ctlWizard).WizardID = WizardID;
            if (((IWizard)ctlWizard).Save())
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
            //((IWizard)ctlWizard).WizardID = WizardID;

            if (((IWizard)ctlWizard).Save())
                Response.Redirect("default.aspx");
        }

        private void btnHome_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //((IWizard)ctlWizard).WizardID = WizardID;

            if (((IWizard)ctlWizard) == null)
            {
                Response.Redirect("default.aspx");
            }
            else
            {
                if (((IWizard)ctlWizard).Save())
                    Response.Redirect("default.aspx");
            }
        }

    }
}
