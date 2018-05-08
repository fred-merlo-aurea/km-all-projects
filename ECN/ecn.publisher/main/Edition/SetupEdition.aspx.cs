using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using ecn.controls;
using ecn.publisher.helpers;
using ECN.Common.Helpers;
using KM.Common;
using KM.Common.Extensions;
using KM.Common.Utilities.Email;
using pdftron;
using pdftron.PDF;
using pdftron.SDF;
using EntitiesPubisher = ECN_Framework_Entities.Publisher;
using BusinessPublisher = ECN_Framework_BusinessLayer.Publisher;
using CommonECNException = ECN_Framework_Common.Objects.ECNException;
using DiagnosticsTrace = System.Diagnostics.Trace;

namespace ecn.publisher.main.Edition
{
    public partial class SetupEdition : ECN_Framework_BusinessLayer.Application.WebPageHelper
    {
        int CustomerID = 0;
        string Image_Path = string.Empty;
        int[] arrResolution = { 450, 618, 750, 874, 1050, 1130, 1290 };
        readonly int Thumbnailsize = 150;
        private const string AdminToEmailConfigurationKey = "DE_NOTIFICATION_TO_EMAIL";
        private const string AdminFromEmailConfigurationKey = "DE_NOTIFICATION_FROM_EMAIL";
        private const string AdminEmailSubject = "Digital Edition has been uploaded.";
        private const string AppSettingImagesVirtualPath = "Images_VirtualPath";
        private const string CurrentWizardStepImageUrlTemplate = "/ecn.images/images/ed_step{0}_s.jpg";
        private const string CompletedWizardStepImageUrlTemplate = "/ecn.images/images/ed_step{0}_d.jpg";
        private const string ContentPlaceholderName = "ContentPlaceHolder1";
        private const string DeletedFileTemplate = "{0}-Deleted-{1:MM-dd-yyyy-hh-mm-ss}";
        private const string ErrorParseTemplate = "Couldn't parse {0} from '{1}'";
        private const string ErrorUploadPdfFile = "Please upload the PDF file.";
        private const string ErrorInvalidFileFormat = "Invalid File Format. Please upload PDF file.";
        private const string ExtensionPdf = "pdf";
        private const string FinishText = "Finish&nbsp;&raquo;";
        private const string HtmlBrTemplate = "{0}<BR>";
        private const string MouseOverImageUrlTemplate = "/ecn.images/images/ed_step{0}_h.jpg";
        private const string ScriptDefaultAspx =
            "<script language='javascript'>document.location.href = 'default.aspx';</script>";
        private const string SubDirCustomers = "customers";
        private const string SubDirPublisher = "Publisher";
        private const string SubDirTemp = "Temp";
        private const int GuildStartCharIndex = 1;
        private const int GuidStopCharIndex = 5;
        private const string TabName = "ibStep";
        private const string UncompletedWizardStepImageUrlTemplate = "/ecn.images/images/ed_step{0}_dd.jpg";
        private const string ZeroString = "0";

        ECN_Framework_Entities.Publisher.Edition objEdition = new  ECN_Framework_Entities.Publisher.Edition();

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
            get { return ViewStateHelper.GetFromViewState(ViewState, nameof(CompletedStep), 1); }
            set { ViewStateHelper.SetViewState(ViewState, nameof(CompletedStep), value); }
        }

        private int EditionID
        {
            get { return ViewStateHelper.GetFromViewState(ViewState, nameof(EditionID), 0); }
            set { ViewStateHelper.SetViewState(ViewState, nameof(EditionID), value); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.CurrentMenuCode = ECN_Framework_Common.Objects.Publisher.Enums.MenuCode.EDITION;
            Master.SubMenu = "Add Edition";
            Master.Heading = "Add/Edit Edition";
            Master.HelpContent = "";
            Master.HelpTitle = "";

            phError.Visible = false;
            CustomerID = Master.UserSession.CurrentCustomer.CustomerID;

            btnNext1.Attributes.Add("onmouseover", "this.src='/ecn.images/images/next_h.gif';");
            btnNext1.Attributes.Add("onmouseout", "this.src='/ecn.images/images/next.gif';");
            btnNext2.Attributes.Add("onmouseover", "this.src='/ecn.images/images/next_h.gif';");
            btnNext2.Attributes.Add("onmouseout", "this.src='/ecn.images/images/next.gif';");

            btnPrevious1.Attributes.Add("onmouseover", "this.src='/ecn.images/images/Previous_h.gif';");
            btnPrevious1.Attributes.Add("onmouseout", "this.src='/ecn.images/images/Previous.gif';");
            btnPrevious2.Attributes.Add("onmouseover", "this.src='/ecn.images/images/Previous_h.gif';");
            btnPrevious2.Attributes.Add("onmouseout", "this.src='/ecn.images/images/Previous.gif';");

            if (!IsPostBack)
            {
                if (Master.UserSession.CurrentUser.UserName.ToLower() == "dcrandall@teckman.com")
                    pnlEditionType.Visible = true;
                else
                    pnlEditionType.Visible = false;


                ddlList.DataSource = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(CustomerID, Master.UserSession.CurrentUser);
                ddlList.DataBind();
                ddlPublicationList.Items.Insert(0, new ListItem("----- Select List -----", ""));

                ddlPublicationList.DataSource = ECN_Framework_BusinessLayer.Publisher.Publication.GetByCustomerID(CustomerID, Master.UserSession.CurrentUser);
                ddlPublicationList.DataBind();
                ddlPublicationList.Items.Insert(0, new ListItem("----- Select Publication -----", ""));

                ddlPublicationList.ClearSelection();

                try
                {
                    EditionID = Convert.ToInt32(Request.QueryString["EditionID"].ToString());
                }
                catch
                {
                    EditionID = 0;
                }

                if (EditionID > 0)
                {
                    pnlEdit.Visible = true;
                    lblUploadLabel.Text = "ReUpload PDF File";
                    lblreuploadmessage.Visible = true;

                    CompletedStep = 2;
                    LoadEdition();
                }
                else
                {
                    pnlEdit.Visible = false;
                    lblUploadLabel.Text = "PDF File";
                    lblreuploadmessage.Visible = false;
                    StepIndex = 1;
                }
                LoadStep();
            }
        }

        protected void ddlEditionType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ddlEditionType.SelectedItem.Text.ToUpper() == "COUPON")
                pnlCouponType.Visible = true;
            else
                pnlCouponType.Visible = false;
        }

        private void LoadEdition()
        {
            ECN_Framework_Entities.Publisher.Edition edition = ECN_Framework_BusinessLayer.Publisher.Edition.GetByCustomerID(CustomerID, Master.UserSession.CurrentUser).Find(x => x.EditionID == EditionID);

            tbEditionName.Text = edition.EditionName;
            tbActivationDate.Text = edition.EnableDate == null ? edition.EnableDate.ToString() : edition.EnableDate.GetValueOrDefault().ToString("MM/dd/yyyy");
            tbDeActivationDate.Text = edition.DisableDate == null ? edition.DisableDate.ToString() : edition.DisableDate.GetValueOrDefault().ToString("MM/dd/yyyy"); 

            rbSecured.ClearSelection();

            if (edition.IsLoginRequired.Value)
                rbSecured.Items.FindByValue("1").Selected = true;
            else
                rbSecured.Items.FindByValue("0").Selected = true;

            ddlPublicationList.ClearSelection();
            ddlPublicationList.Items.FindByValue(edition.PublicationID.ToString()).Selected = true;

            lblFileName.Text = edition.FileName;
            lblTotalPages.Text = edition.Pages.ToString();

            ddlStatus.ClearSelection();
            ddlStatus.Items.FindByValue(edition.Status.ToString()).Selected = true;
        }

        void LoadStep()
        {
            try
            {
                pnl1.Visible = false;
                pnl2.Visible = false;

                switch (StepIndex)
                {
                    case 1:
                        btnPrevious1.Visible = false;
                        btnPrevious2.Visible = false;
                        btnNext1.Text = "Next&nbsp;&raquo;";
                        btnNext2.Text = "Next&nbsp;&raquo;";
                        btnNext1.OnClientClick = "";
                        btnNext2.OnClientClick = "";
                        pnl1.Visible = true;
                        break;
                    case 2:
                        btnPrevious1.Visible = true;
                        btnPrevious2.Visible = true;
                        btnNext1.Text = "Finish&nbsp;&raquo;";
                        btnNext2.Text = "Finish&nbsp;&raquo;";

                        if (EditionID > 0)
                        {
                            btnNext1.OnClientClick = "return reuploadconfirm();";
                            btnNext2.OnClientClick = "return reuploadconfirm();";
                        }
                        pnl2.Visible = true;
                        break;
                }
                EnableTabBar();
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                phError.Visible = true;
                btnNext1.Visible = false;
                btnNext2.Visible = false;
            }
        }

        private void EnableTabBar()
        {
            var mainContent = Master.FindControl(ContentPlaceholderName) as ContentPlaceHolder;
            if (mainContent == null)
            {
                return;
            }

            var numberOfWizardSteps = 2;
            TabBarHelpers.EnableTabBar(
                numberOfWizardSteps,
                StepIndex,
                CompletedStep,
                stepIndex => mainContent.FindControl($"{TabName}{stepIndex}") as ImageButton,
                stepIndex => string.Format(CurrentWizardStepImageUrlTemplate, stepIndex),
                stepIndex => string.Format(UncompletedWizardStepImageUrlTemplate, stepIndex),
                stepIndex => string.Format(CompletedWizardStepImageUrlTemplate, stepIndex),
                stepIndex => string.Format(MouseOverImageUrlTemplate, stepIndex),
                stepIndex => string.Format(CompletedWizardStepImageUrlTemplate, stepIndex));
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            var virtualPathSetting = ConfigurationManager.AppSettings[AppSettingImagesVirtualPath];

            if (FinishText.Equals(btnNext1.Text, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    if (Page.IsValid)
                    {
                        btnNextOnPageValid(virtualPathSetting);
                    }
                }
                catch (Exception exception)
                {
                    lblErrorMessage.Text = exception.Message;
                    lblErrorMessage.Visible = true;
                    phError.Visible = true;
                }
            }
            else
            {
                if (StepIndex > CompletedStep)
                {
                    CompletedStep = StepIndex;
                }

                StepIndex++;
                LoadStep();
            }
        }

        private void btnNextOnPageValid(string virtualPathSetting)
        {
            var editionFileName = lbluploadedfile.Text;

            FillEditionOnNext(editionFileName);

            objEdition.CustomerID = CustomerID;

            if (SetErrorVisibility(editionFileName))
            {
                return;
            }

            var tempPath = Path.Combine(
                virtualPathSetting,
                SubDirCustomers,
                Master.UserSession.CurrentCustomer.CustomerID.ToString(),
                SubDirPublisher,
                Guid.NewGuid().ToString().Substring(GuildStartCharIndex, GuidStopCharIndex));
            var tempServerPath = Server.MapPath(tempPath);

            if (!string.IsNullOrWhiteSpace(editionFileName))
            {
                MoveImageFile(virtualPathSetting, tempServerPath, editionFileName);
            }

            SaveEdition();

            if (!string.IsNullOrWhiteSpace(editionFileName))
            {
                MoveEditionAndEmail(virtualPathSetting, tempServerPath, editionFileName);
            }

            Response.ClearContent();
            Response.Write(ScriptDefaultAspx);
        }

        private void MoveImageFile(string virtualPathSetting, string tempServerPath, string editionFileName)
        {
            if (EditionID > 0)
            {
                MoveImage(virtualPathSetting);
            }
            else
            {
                Image_Path = tempServerPath;
            }

            if (!Directory.Exists(Image_Path))
            {
                Directory.CreateDirectory(Image_Path);
            }
            else
            {
                try
                {
                    Directory.Delete(Image_Path, true);
                    Directory.CreateDirectory(Image_Path);
                }
                catch (Exception ex)
                {
                    DiagnosticsTrace.TraceError(ex.ToString());
                }
            }

            var virtualPath = Path.Combine(
                virtualPathSetting,
                SubDirCustomers,
                Master.UserSession.CurrentCustomer.CustomerID.ToString(),
                SubDirPublisher,
                SubDirTemp,
                editionFileName);
            var serverPath = Server.MapPath(virtualPath);
            var imagePath = Path.Combine(Image_Path, editionFileName);
            File.Move(serverPath, imagePath);
            ConvertDE(imagePath);
        }

        private void MoveEditionAndEmail(string virtualPathSetting, string tempServerPath, string editionFileName)
        {
            if (Directory.Exists(tempServerPath))
            {
                var dirName1 = Path.Combine(
                    virtualPathSetting,
                    SubDirCustomers,
                    Master.UserSession.CurrentCustomer.CustomerID.ToString(),
                    SubDirPublisher,
                    objEdition.EditionID.ToString());
                var serverDestDirName = Server.MapPath(dirName1);
                Directory.Move(tempServerPath, serverDestDirName);
            }
            //Send email to AccountExecutive/AccountManager/Roderick/Sunil
            var channelName = Master.UserSession.CurrentBaseChannel.BaseChannelName;
            var customername = Master.UserSession.CurrentCustomer.CustomerName;

            var emailBody = GetEmailBody(channelName, customername, editionFileName);

            var addressTo = ConfigurationManager.AppSettings[AdminToEmailConfigurationKey];
            var addressFrom = ConfigurationManager.AppSettings[AdminFromEmailConfigurationKey];
            var emailService = new EmailService(new EmailClient(), new ConfigurationProvider());
            var emailMessage = new EmailMessage
            {
                From = addressFrom,
                Subject = AdminEmailSubject,
                Body = emailBody
            };
            emailMessage.To.Add(addressTo);
            emailService.SendEmail(emailMessage);
        }

        private void MoveImage(string virtualPathSetting)
        {
            var imageVirtualPath = Path.Combine(
                virtualPathSetting,
                SubDirCustomers,
                Master.UserSession.CurrentCustomer.CustomerID.ToString(),
                SubDirPublisher,
                objEdition.EditionID.ToString());
            Image_Path = Server.MapPath(imageVirtualPath);

            if (Directory.Exists(Image_Path))
            {
                var deletedFileName = string.Format(DeletedFileTemplate, objEdition.EditionID, DateTime.Now);
                var moveVirtualPath = Path.Combine(
                    virtualPathSetting,
                    SubDirCustomers,
                    Master.UserSession.CurrentCustomer.CustomerID.ToString(),
                    SubDirPublisher,
                    deletedFileName);
                var moveServerPath = Server.MapPath(moveVirtualPath);
                Directory.Move(Image_Path, moveServerPath);
            }
        }

        private bool SetErrorVisibility(string editionFileName)
        {
            if (EditionID == 0)
            {
                if (string.IsNullOrWhiteSpace(editionFileName))
                {
                    lblErrorMessage.Text = ErrorUploadPdfFile;
                    phError.Visible = true;
                    return true;
                }

                if (!editionFileName.EndsWith(ExtensionPdf, StringComparison.OrdinalIgnoreCase))
                {
                    lblErrorMessage.Text = ErrorInvalidFileFormat;
                    phError.Visible = true;
                    return true;
                }
            }
            return false;
        }

        private void SaveEdition()
        {
            try
            {
                BusinessPublisher.Edition.Save(objEdition, Master.UserSession.CurrentUser);
            }
            catch (CommonECNException ecnException)
            {
                var builder = new StringBuilder();
                foreach (var err in ecnException.ErrorList)
                {
                    builder.AppendFormat(HtmlBrTemplate, err.ErrorMessage);
                }

                lblErrorMessage.Text = builder.ToString();
                phError.Visible = true;
            }
        }

        private void FillEditionOnNext(string editionFileName)
        {
            objEdition = new EntitiesPubisher.Edition();
            objEdition.EditionID = EditionID;
            objEdition.EditionName = tbEditionName.Text;
            objEdition.PublicationID = ToInt32WithThrow(ddlPublicationList.SelectedValue);
            objEdition.Status = ddlStatus.SelectedItem.Value;
            objEdition.EnableDate = ToDateTimeNullable(tbActivationDate.Text);
            objEdition.DisableDate = ToDateTimeNullable(tbDeActivationDate.Text);
            objEdition.FileName = editionFileName;
            objEdition.Pages = ToInt32(lblTotalPages.Text);
            objEdition.IsSearchable = false;
            objEdition.IsLoginRequired = rbSecured.SelectedItem.Value != ZeroString;
            if (objEdition.EditionID > 0)
            {
                objEdition.UpdatedUserID = Master.UserSession.CurrentUser.UserID;
            }
            else
            {
                objEdition.CreatedUserID = Master.UserSession.CurrentUser.UserID;
            }
        }

        private string GetEmailBody(string channelName, string customerName, string editionFileName)
        {
            var imageDomainPath = ConfigurationManager.AppSettings[AppSettingImagesVirtualPath];

            var builder = new StringBuilder();
            builder.AppendFormat("<table border='1' cellpadding='3' cellspacing='0'><tr><td>Channel :</td><td>{0}</td></tr>", channelName);
            builder.AppendFormat("<tr><td>Customer :</td><td>{0}</td></tr>", customerName);
            builder.AppendFormat("<tr><td>Publication :</td><td>{0}<BR></td></tr>", ddlPublicationList.SelectedItem.Text);
            builder.AppendFormat("<tr><td>Edition Name :</td><td>{0}</td></tr>", objEdition.EditionName);
            builder.AppendFormat("<tr><td>Activation Date :</td><td>{0}</td></tr>", tbActivationDate.Text);
            builder.AppendFormat("<tr><td>De-Activation Date :</td><td>{0}</td></tr>", tbDeActivationDate.Text);
            builder.AppendFormat("<tr><td>Status :</td><td>{0}</td></tr>", ddlStatus.SelectedItem.Value);
            builder.AppendFormat(
                "<tr><td>PDF File :</td><td><a href='{0}/customers/{1}/Publisher/{2}/{3}' target='blank'>{3}</a></td></tr>", 
                imageDomainPath, 
                Master.UserSession.CurrentCustomer.CustomerID, 
                objEdition.EditionID, 
                editionFileName);
            builder.Append("<tr><td></td><td></td></tr>");
            builder.Append("</table>");
            var emailBody = builder.ToString();
            return emailBody;
        }

        private void ConvertDE(string PDFFileName)
        {
            //objEdition = ECN_Framework_BusinessLayer.Publisher.Edition.GetByEditionID(EditionID, Master.UserSession.CurrentUser);

            PDFNet.Initialize(ConfigurationManager.AppSettings["PDFTron_LicenseKey"].ToString());
            PDFNet.SetResourcesPath(Server.MapPath("../../resources/"));

            PDFDoc pdfdoc = new PDFDoc(PDFFileName);
            pdfdoc.InitSecurityHandler();
            initNotify("Extraction Table of Contents...!");
            objEdition.xmlTOC = pdfdoc.GetFirstBookmark().ToTableOfContents();

            try
            {
                GetPageDetails(pdfdoc, objEdition);
                GenerateImages(pdfdoc);
            }
            catch
            {
                throw;
            }
            finally
            {
                pdfdoc.Close();
            }
       }

        private void GetPageDetails(PDFDoc pdfdoc, ECN_Framework_Entities.Publisher.Edition objEdition)
        {
            int x1 = 0;
            int y1 = 0;
            int x2 = 0;
            int y2 = 0;
            int num_annots = 0;
            string lnk;

            ECN_Framework_Entities.Publisher.Page ecnPage = null;
            ECN_Framework_Entities.Publisher.Link pageLink = null;
            objEdition.PageList = new List<ECN_Framework_Entities.Publisher.Page>();
            
            pdftron.PDF.Page pdfpage;

            for (PageIterator itr = pdfdoc.GetPageIterator(); itr.HasNext(); itr.Next())
            {
                pdfpage = itr.Current();

                ecnPage = new ECN_Framework_Entities.Publisher.Page();

                ecnPage.PageNumber = itr.GetPageNumber();

                num_annots = pdfpage.GetNumAnnots();

                for (int i = 0; i < num_annots; ++i)
                {
                    lnk = "";
                    Annot annot = pdfpage.GetAnnot(i);

                    try
                    {
                        switch (annot.GetType())
                        {
                            case Annot.Type.e_Link:
                                
                                Rect bbox = annot.GetRect();
                                Rect cbox = pdfpage.GetCropBox();

                                x1 = Convert.ToInt32((bbox.x1 - cbox.x1));
                                y1 = Convert.ToInt32((pdfpage.GetPageHeight() - bbox.y2 + cbox.y1));
                                x2 = Convert.ToInt32((bbox.x2 - cbox.x1));
                                y2 = Convert.ToInt32((pdfpage.GetPageHeight() - bbox.y1 + cbox.y1));


                                pdftron.PDF.Action action = annot.GetLinkAction();

                                if (action.GetType() == pdftron.PDF.Action.Type.e_GoTo)
                                {
                                    pageLink = new ECN_Framework_Entities.Publisher.Link();
                                    pageLink.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                                    pageLink.LinkType = "GoTo";
                                    pageLink.LinkURL = action.GetDest().GetPage().GetIndex().ToString();
                                    pageLink.x1 = x1;
                                    pageLink.y1 = y1;
                                    pageLink.x2 = x2;
                                    pageLink.y2 = y2;
                                    pageLink.CreatedUserID = Master.UserSession.CurrentUser.UserID;
                                    ecnPage.LinkList.Add(pageLink);
                                }

                                else if (action.GetType() == pdftron.PDF.Action.Type.e_URI)
                                {
                                    try
                                    {
                                        lnk = action.GetSDFObj().Get("URI").Value().GetAsPDFText();
                                    }
                                    catch
                                    {
                                        lnk = "";
                                    }
                                    if (!(lnk.ToLower().StartsWith("http://") || lnk.ToLower().StartsWith("mailto:") || lnk.ToLower().StartsWith("https://")))
                                        lnk = "http://" + lnk;

                                    pageLink = new ECN_Framework_Entities.Publisher.Link();
                                    pageLink.LinkType = "URI";
                                    pageLink.CustomerID = Master.UserSession.CurrentUser.CustomerID;
                                    pageLink.LinkURL = lnk;
                                    pageLink.x1 = x1;
                                    pageLink.y1 = y1;
                                    pageLink.x2 = x2;
                                    pageLink.y2 = y2;
                                    pageLink.CreatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
                                    ecnPage.LinkList.Add(pageLink);
                                }
                                break;
                        }
                    }
                    catch { }

                }

                ecnPage.DisplayNumber = itr.GetPageNumber().ToString();
                ecnPage.Width = Convert.ToInt32(pdfpage.GetPageWidth());
                ecnPage.Height = Convert.ToInt32(pdfpage.GetPageHeight());
                ecnPage.TextContent = pdfpage.GetTextContent();
                ecnPage.EditionID = objEdition.EditionID;
                ecnPage.CustomerID = objEdition.CustomerID;
                if (ecnPage.PageID > 0)
                    ecnPage.UpdatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
                else
                    ecnPage.CreatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
              
                 if (ecnPage != null)
                    objEdition.PageList.Add(ecnPage);
               
                //Notify(Convert.ToString(2), "Extraction Links - Page " + itr.GetPageNumber() + " of " + pdfdoc.GetPageCount() + "  : ");
            }
            objEdition.Pages = objEdition.PageList.Count;
            objEdition.UpdatedUserID = Convert.ToInt32(Master.UserSession.CurrentUser.UserID);
        }

        private void GenerateImages(PDFDoc doc)
        {
            string bgImageName = string.Empty;
            string fgImageName = string.Empty;


            PDFDraw draw = new PDFDraw();
            draw.SetDPI(72); // Set the output resolution is to 72 DPI.
            draw.SetImageSmoothing(true);
            draw.SetDrawAnnotations(false);

            ObjSet hint_set = new ObjSet();

            // Use optional encoder parameter to specify JPEG quality.
            pdftron.SDF.Obj encoder_param = hint_set.CreateDict();
            encoder_param.PutNumber("Quality", 90);
            draw.SetImageSmoothing(true);

            ECN_Framework_Entities.Publisher.Page ecnPage;
            pdftron.PDF.Page pdfpage;


            for (PageIterator itr = doc.GetPageIterator(); itr.HasNext(); itr.Next())
            {

                Notify(((100 * itr.GetPageNumber())/ doc.GetPageCount()).ToString(), "Converting Page " + itr.GetPageNumber() + " of " + doc.GetPageCount() + "  : ");


                pdfpage = itr.Current();

                ecnPage = objEdition.PageList.Find(x=>x.PageNumber == itr.GetPageNumber() - 1);
                    
                try
                {
                    for (int iA = 0; iA < arrResolution.Length; iA++)
                    {
                        if (itr.GetPageNumber() == 1)
                            if (!System.IO.Directory.Exists(Image_Path + arrResolution[iA].ToString() + "/"))
                                Directory.CreateDirectory(Image_Path + arrResolution[iA].ToString() + "/");

                        draw.SetImageSize(getImageWidth(pdfpage.GetPageWidth(), pdfpage.GetPageHeight(), arrResolution[iA]), arrResolution[iA]);

                        draw.Export(pdfpage, string.Format("{0}{1:d}.jpg", Image_Path + arrResolution[iA].ToString() + "/", itr.GetPageNumber()), "JPEG", encoder_param);
                    }

                    if (!System.IO.Directory.Exists(Image_Path + Thumbnailsize + "/"))
                        Directory.CreateDirectory(Image_Path + Thumbnailsize + "/");

                    // Create ThumbNail Image
                    draw.SetImageSize(getImageWidth(pdfpage.GetPageWidth(), pdfpage.GetPageHeight(), Thumbnailsize), Thumbnailsize);
                    draw.Export(pdfpage, string.Format("{0}{1:d}.png", Image_Path + Thumbnailsize + "/", itr.GetPageNumber()), "PNG");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " -  Page No - " + itr.GetPageNumber());
                }
            }
            draw.Dispose();
        }

        private int getImageWidth(double width, double height, int res)
        {
            return (int)(width * res / height);
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            StepIndex--;
            LoadStep();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void ibStep_Command(object sender, CommandEventArgs e)
        {
            StepIndex = Convert.ToInt32(e.CommandArgument);
            LoadStep();
        }

        protected void ibSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    if (FileUpload1.PostedFile.FileName != "")
                    {
                        if (FileUpload1.PostedFile.ContentLength > 200000000)
                        {
                            lblErrorMessage.Text = "ERROR: File size is too large. Maximum allowed is 200 MB";
                            phError.Visible = true;
                            return;
                        }
                    }

                    string pdffilename = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName).Replace(" ", "_");

                    if (pdffilename == string.Empty)
                    {
                        lblErrorMessage.Text = "Please select the PDF file to upload.";
                        phError.Visible = true;
                        return;
                    }
                    else if (!pdffilename.ToLower().EndsWith("pdf"))
                    {
                        lblErrorMessage.Text = "Invalid File Format. Please upload PDF file.";
                        phError.Visible = true;
                        return;
                    }

                    Image_Path = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Images_VirtualPath"] + "/customers/" + Master.UserSession.CurrentCustomer.CustomerID + "/Publisher/Temp/");

                    if (!System.IO.Directory.Exists(Image_Path))
                    {
                        Directory.CreateDirectory(Image_Path);
                    }


                    FileUpload1.PostedFile.SaveAs(Image_Path + pdffilename);
                    lbluploadedfile.Text = pdffilename;
                    pnlUploadedFile.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                lblErrorMessage.Visible = true;
                phError.Visible = true;
            }
        }

        #region ProgressBar
        public void initNotify(string StrSplash)
        {
            // Only do this on the first call to the page
            //Register loadingNotifier.js for showing the Progress Bar
            Response.Write(string.Format(@"<link href='/ecn.accounts/styles/progressbar.css' type='text/css' rel='stylesheet' />
				<body><script type='text/javascript' src='/ecn.accounts/scripts/ProgressBar.js'></script>
              <script language='javascript' type='text/javascript'>
              initLoader('{0}');
              </script></body>", StrSplash));
            // Send it to the client
            Response.Flush();


        }
        public void Notify(string strPercent, string strMessage)
        {
            //Update the Progress bar
            Response.Write(string.Format("<script language='javascript' type='text/javascript'>setProgress({0},'{1}'); </script>", strPercent, strMessage));
            Response.Flush();
        }
        #endregion

        private static int ToInt32(string str)
        {
            int result;
            int.TryParse(str, out result);
            return result;
        }

        private static int ToInt32WithThrow(string str)
        {
            int result;
            if (!int.TryParse(str, out result))
            {
                var exceptionMessage = string.Format(ErrorParseTemplate, typeof(int).Name, str);
                throw new InvalidOperationException(exceptionMessage);
            }
            return result;
        }

        private static DateTime? ToDateTimeNullable(string str)
        {
            DateTime result;
            if (!DateTime.TryParse(str, out result))
            {
                return null;
            }
            return result;
        }
    }
}
