using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using ecn.controls;
using ECN_Framework_Common.Objects;
using CommonFunctions = ECN_Framework_Common.Functions;
using PubBLL = ECN_Framework_BusinessLayer.Publisher;
using PubEntity = ECN_Framework_Entities.Publisher;

namespace ecn.publisher.main.Publication
{
    public partial class SetupPublication : ECN_Framework_BusinessLayer.Application.WebPageHelper
    {
        private const string CurrentWizardStepImageUrlTemplate = "/ecn.images/images/pub_step{0}_s.jpg";
        private const string UncompletedWizardStepImageUrlTemplate = "/ecn.images/images/pub_step{0}_d.jpg";
        private const string MouseOverImageUrlTemplate = "/ecn.images/images/pub_step{0}_h.jpg";
        private const string ContentPlaceholderName = "ContentPlaceHolder1";
        private const string TabName = "ibStep";
        private const string PublicationUrlExistsErrorMessage = "Publication URL already exists. Please enter different Publication URL.";
        private const string PublicationNameExistsErrorMessage = "Publication already exists. Please enter different name.";
        private const string LastStepText = "Finish&nbsp;&raquo;";
        private const string InvalidImageFormatErrorMessage = "Invalid Image format. Select Only gif or jpg image.";
        private const string SingleWhiteSpace = " ";
        private const string SingleUnderscore = "_";
        private const string BreakLineTag = "<BR>";

        int CustomerID = 0;

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

        private int CompletedStep
        {
            get
            {
                if (ViewState["CompletedStep"] == null)
                    return 1;
                else
                    return (int)ViewState["CompletedStep"];
            }

            set { ViewState["CompletedStep"] = value; }
        }

        private int PublicationID()
        {
            try
            {
                return Convert.ToInt32(Request.QueryString["PublicationID"].ToString());
            }
            catch
            {
                return 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Publisher.Enums.MenuCode.PUBLICATION;
            Master.SubMenu = "Add Publication";
            Master.Heading = "Add/Edit Publication";
            Master.HelpContent = "";
            Master.HelpTitle = "";	

            CustomerID = Master.UserSession.CurrentUser.CustomerID;

            phError.Visible = false;
            lblErrorMessage.Text = "";

            lbtnNext1.Attributes.Add("onmouseover", "this.src='/ecn.images/images/next_h.gif';");
            lbtnNext1.Attributes.Add("onmouseout", "this.src='/ecn.images/images/next.gif';");
            lbtnNext2.Attributes.Add("onmouseover", "this.src='/ecn.images/images/next_h.gif';");
            lbtnNext2.Attributes.Add("onmouseout", "this.src='/ecn.images/images/next.gif';");

            lbtnPrevious1.Attributes.Add("onmouseover", "this.src='/ecn.images/images/Previous_h.gif';");
            lbtnPrevious1.Attributes.Add("onmouseout", "this.src='/ecn.images/images/Previous.gif';");
            lbtnPrevious2.Attributes.Add("onmouseover", "this.src='/ecn.images/images/Previous_h.gif';");
            lbtnPrevious2.Attributes.Add("onmouseout", "this.src='/ecn.images/images/Previous.gif';");

            if (!IsPostBack)
            {
                List<PubEntity.Category> lCategory = PubBLL.Category.GetAll();

                ddlCategory.DataSource = lCategory.OrderBy(x => x.CategoryName);
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("----- Select Category -----", "0"));

                ddlFrequency.DataSource = PubBLL.Frequency.GetAll();
                ddlFrequency.DataBind();
                ddlFrequency.Items.Insert(0, new ListItem("----- Select Frequency -----", "0"));

                if (PublicationID() > 0)
                {
                    CompletedStep = 3;
                    LoadPublication();
                }
                else
                {
                    StepIndex = 1;
                }
                LoadStep();
            }
        }

        private void LoadPublication()
        {
            PubEntity.Publication pub = PubBLL.Publication.GetByPublicationID(PublicationID(), Master.UserSession.CurrentUser);
            lblGroupID.Text = pub.GroupID.ToString();
            tbPublicationName.Text = pub.PublicationName;

            try
            {
                ddlPublicationType.ClearSelection();
                ddlPublicationType.Items.FindByValue(pub.PublicationType).Selected = true;
            }
            catch { }
            try
            {
                ddlCategory.Items.FindByValue(pub.CategoryID.ToString()).Selected = true;
            }
            catch { }
            tbCirculation.Text = (pub.Circulation == 0 ? "" : pub.Circulation.ToString());
            try
            {
                ddlFrequency.Items.FindByValue(pub.FrequencyID.ToString()).Selected = true;
            }
            catch { }
            tbPublicationAlias.Text = pub.PublicationCode;

            cbActive.Checked = pub.Active;

            tbContactAddress1.Text = pub.ContactAddress1;
            tbContactAddress2.Text = pub.ContactAddress2;
            tbContactEmail.Text = pub.ContactEmail;
            tbContactPhone.Text = pub.ContactPhone;
            tbContactFormLink.Text = pub.ContactFormLink;

            if (pub.SubscriptionOption == 0)
                rbSubOptn6.Checked = true;
            else
            {
                switch (pub.SubscriptionOption)
                {
                    case 1:
                        rbSubOptn1.Checked = true;
                        break;
                    case 2:
                        rbSubOptn2.Checked = true;
                        break;
                    case 3:
                        rbSubOptn3.Checked = true;
                        break;
                    case 4:
                        rbSubOptn4.Checked = true;
                        break;
                    case 5:
                        rbSubOptn5.Checked = true;
                        tbSubscriptionFormLink.Text = pub.SubscriptionFormLink;
                        break;
                    case 0:
                        rbSubOptn6.Checked = true;
                        break;
                }
            }
            lblogoURL.Text = pub.LogoURL;
            tbLogoLink.Text = pub.LogoLink;

            if (pub.LogoURL != "")
                imgLogo.ImageUrl = pub.LogoLink;
        }

        void LoadStep()
        {
            try
            {
                pnl1.Visible = false;
                pnl2.Visible = false;
                pnl3.Visible = false;

                switch (StepIndex)
                {
                    case 1:
                        lbtnPrevious1.Visible = false;
                        lbtnPrevious2.Visible = false;
                        lbtnNext1.Text = "Next&nbsp;&raquo;";
                        lbtnNext2.Text = "Next&nbsp;&raquo;";
                        pnl1.Visible = true;
                        break;
                    case 2:
                        lbtnPrevious1.Visible = true;
                        lbtnPrevious2.Visible = true;
                        lbtnNext1.Text = "Next&nbsp;&raquo;";
                        lbtnNext2.Text = "Next&nbsp;&raquo;";
                        pnl2.Visible = true;
                        break;
                    case 3:
                        lbtnPrevious1.Visible = true;
                        lbtnPrevious2.Visible = true;
                        lbtnNext1.Text = "Finish&nbsp;&raquo;";
                        lbtnNext2.Text = "Finish&nbsp;&raquo;";
                        pnl3.Visible = true;
                        break;
                }
                EnableTabBar();
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                phError.Visible = true;
                lbtnNext1.Visible = false;
                lbtnNext2.Visible = false;
            }
        }

        private void EnableTabBar()
        {
            var mainContent = Master.FindControl(ContentPlaceholderName) as ContentPlaceHolder;
            if (mainContent == null)
            {
                return;
            }

            var numberOfWizardSteps = 3;
            TabBarHelpers.EnableTabBar(
                numberOfWizardSteps,
                StepIndex,
                CompletedStep,
                stepIndex => mainContent.FindControl($"{TabName}{stepIndex}") as ImageButton,
                stepIndex => string.Format(CurrentWizardStepImageUrlTemplate, stepIndex),
                stepIndex => string.Format(UncompletedWizardStepImageUrlTemplate, stepIndex),
                stepIndex => string.Format(UncompletedWizardStepImageUrlTemplate, stepIndex),
                stepIndex => string.Format(MouseOverImageUrlTemplate, stepIndex),
                stepIndex => string.Format(UncompletedWizardStepImageUrlTemplate, stepIndex));
        }

        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            if (lbtnNext1.Text == LastStepText)
            {
                if (Page.IsValid)
                {
                    SavePublication();
                }
            }
            else
            {
                if (StepIndex == 1)
                {
                    var publicationName = tbPublicationName.Text.Replace("'", "''");
                    if (PubBLL.Publication.Exists(PublicationID(), publicationName, CustomerID))
                    {
                        lblErrorMessage.Text = PublicationNameExistsErrorMessage;
                        phError.Visible = true;
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(tbPublicationAlias.Text))
                    {
                        var publicationCode = tbPublicationAlias.Text.Replace("'", "''");
                        if (PubBLL.Publication.ExistsAlias(PublicationID(), publicationCode))
                        {
                            lblErrorMessage.Text = PublicationUrlExistsErrorMessage;
                            phError.Visible = true;
                            return;
                        }
                    }
                }

                if (StepIndex > CompletedStep)
                {
                    CompletedStep = StepIndex;
                }

                StepIndex++;
                LoadStep();
            }
        }

        private void SavePublication()
        {
            string errorMessage;
            var publicationEntity = CreatePublicationEntity(out errorMessage);

            if (!errorMessage.IsNullOrWhiteSpace())
            {
                lblErrorMessage.Text = errorMessage;
                phError.Visible = true;
                return;
            }

            try
            {
                PubBLL.Publication.Save(ref publicationEntity, Master.UserSession.CurrentUser);
            }
            catch (ECNException ecnex)
            {
                var errorStringBuilder = new StringBuilder();

                foreach (var error in ecnex.ErrorList)
                {
                    errorStringBuilder.Append(error.ErrorMessage + BreakLineTag);
                }

                lblErrorMessage.Text = errorStringBuilder.ToString();
                phError.Visible = true;
                return;
            }

            Response.Redirect("default.aspx");
        }

        private PubEntity.Publication CreatePublicationEntity(out string errorMessage)
        {
            errorMessage = null;

            var publicationEntity = new PubEntity.Publication
            {
                PublicationID = PublicationID(),
                PublicationName = tbPublicationName.Text,
                PublicationType = ddlPublicationType.SelectedItem.Value,
                CategoryID = Convert.ToInt32(ddlCategory.SelectedItem.Value),
                Circulation = tbCirculation.Text.IsNullOrWhiteSpace() ? 0 : Convert.ToInt32(tbCirculation.Text),
                FrequencyID = Convert.ToInt32(ddlFrequency.SelectedItem.Value),
                PublicationCode = tbPublicationAlias.Text,
                CustomerID = CustomerID,
                GroupID = Convert.ToInt32(lblGroupID.Text),
                Active = cbActive.Checked,
                ContactAddress1 = tbContactAddress1.Text,
                ContactAddress2 = tbContactAddress2.Text,
                ContactEmail = tbContactEmail.Text,
                ContactPhone = tbContactPhone.Text,
                ContactFormLink = tbContactFormLink.Text,
                SubscriptionOption = GetSubscriptionOption(),
                EnableSubscription = GetEnableSubscription(),
                SubscriptionFormLink = GetSubscriptionFormLink()
            };
            
            if (fBrowse.PostedFile.FileName.IsNullOrWhiteSpace())
            {
                publicationEntity.LogoURL = lblogoURL.Text;
            }
            else if (!fBrowse.PostedFile.FileName.ToLower().EndsWith(".gif") &&
                     !fBrowse.PostedFile.FileName.ToLower().EndsWith(".jpg"))
            {
                errorMessage = InvalidImageFormatErrorMessage;
                phError.Visible = true;
            }
            else
            {
                var filename = GetFileName();
                var directoryUploadPath = GetDirectoryUploadPath();

                if (!Directory.Exists(directoryUploadPath))
                {
                    Directory.CreateDirectory(directoryUploadPath);
                }

                var fileUploadPath = GetFileUploadPath(directoryUploadPath, filename);
                fBrowse.PostedFile.SaveAs(fileUploadPath);
                publicationEntity.LogoURL = GetLogoUrl(filename);
            }

            publicationEntity.LogoLink = tbLogoLink.Text;
            if (publicationEntity.PublicationID > 0)
            {
                publicationEntity.UpdatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
            }
            else
            {
                publicationEntity.CreatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
            }

            return publicationEntity;
        }

        private string GetFileUploadPath(string directoryUploadPath, string filename)
        {
            if (directoryUploadPath == null || filename == null)
            {
                return null;
            }

            return Path.Combine(directoryUploadPath, filename);
        }

        private string GetLogoUrl(string filename)
        {
            return $"{ConfigurationManager.AppSettings["Image_DomainPath"]}/customers/{CustomerID}/images/{filename}";
        }

        private string GetDirectoryUploadPath()
        {
            var imagesPath = System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"];
            var uploadPath = $"{imagesPath}/customers/{CustomerID}/images";

            return Server.MapPath(uploadPath);
        }

        private string GetFileName()
        {
            var filename = CommonFunctions.StringFunctions.Replace(
                Path.GetFileName(fBrowse.PostedFile.FileName),
                SingleWhiteSpace,
                SingleUnderscore);
            filename = CommonFunctions.StringFunctions.Replace(filename, "\'", SingleUnderscore);
            return filename;
        }

        private string GetSubscriptionFormLink()
        {
            if (rbSubOptn5.Checked)
            {
                return tbSubscriptionFormLink.Text;
            }

            return string.Empty;
        }

        private bool GetEnableSubscription()
        {
            return rbSubOptn4.Checked || rbSubOptn5.Checked;
        }

        private int? GetSubscriptionOption()
        {
            if (rbSubOptn1.Checked)
            {
                return 1;
            }

            if (rbSubOptn2.Checked)
            {
                return 2;
            }

            if (rbSubOptn3.Checked)
            {
                return 3;
            }

            if (rbSubOptn4.Checked)
            {
                return 4;
            }

            if (rbSubOptn5.Checked)
            {
                return 5;
            }

            if (rbSubOptn6.Checked)
            {
                return 0;
            }

            return null;
        }

        protected void lbtnPrevious_Click(object sender, EventArgs e)
        {
            StepIndex--;
            LoadStep();
        }

        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void ibStep_Command(object sender, CommandEventArgs e)
        {
            StepIndex = Convert.ToInt32(e.CommandArgument);
            LoadStep();
        }
    }
}
