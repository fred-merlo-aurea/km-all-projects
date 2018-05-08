using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Fakes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;
using NUnit.Framework;
using Shouldly;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using LayoutEntity = ECN_Framework_Entities.Communicator.Layout;
namespace ecn.activityengines.Tests
{
    public partial class publicPreviewTest
    {
        private const string KMCommonApplicatoinKey = "KMCommon_Application";
        private const string OpenClickUseOldSiteKey = "OpenClick_UseOldSite";
        private const string MvcActivityDomainPathKey = "MVCActivity_DomainPath";
        private const string ImageDomainPathKey = "Image_DomainPath";

        [Test]
        public void Page_Load_LayoutIdAboveZeroAndUserIdNotZero_WorkAsExpecetd()
        {
            //Arrange
            var layoutId = GetAnyNumber();
            SetLayoutId(layoutId);
            var expectedLogs = new[]
            {
                "PageLoad",
                "Got query string values",
                "LayoutID > 0",
                "Got CustomerID and UserID",
                "Get Preview",
                "Preview Loaded"
            };
            var isMobile = true;
            var urlWithQueryString = new Uri("http://doman.some/?querySting");
            var customerId = GetAnyNumber();
            var userId = GetAnyNumber();
            var layoutEntity = GetLayoutEntity(customerId);
            var html = GetUniqueString();
            _mocksContext.Request.Setup(request => request.Url)
                .Returns(urlWithQueryString);
            _mocksContext.PublicPreview.Setup(preview => preview.IsMobileBrowser())
                .Returns(isMobile);
            _mocksContext.LayoutBusiness.Setup(layout => layout.GetByLayoutIDNoAccessCheck(layoutId, false))
                .Returns(layoutEntity);
            _mocksContext.LayoutBusiness.Setup(layout => layout.GetLayoutUserId(layoutId))
                .Returns(userId);
            _mocksContext.LayoutBusiness
                .Setup(layout => layout.GetPreviewNoAccessCheck(layoutId, ContentTypeCode.HTML, isMobile, customerId,
                    null, null, null))
                .Returns(html);

            //Act
            CallPageLoad();

            //Assert
            _mocksContext.PublicPreview.VerifyLogs(expectedLogs);
            GetLiteralPreviewText().ShouldBe(html);
        }

        [Test]
        public void Page_Load_LayoutIdAboveZeroAndUserIdZero_WorkAsExpecetd()
        {
            //Arrange
            var layoutId = GetAnyNumber();
            SetLayoutId(layoutId);
            var expectedLogs = new[]
            {
                "PageLoad",
                "Got query string values",
            };
            string invalidLinkErrorMessageBuilder = GetInvalidLinkErrorMessage();
            var isMobile = true;
            var urlWithQueryString = new Uri("http://doman.some/?querySting");
            var customerId = GetAnyNumber();
            var userId = Zero;
            var layoutEntity = GetLayoutEntity(customerId);
            _mocksContext.Request.Setup(request => request.Url)
                .Returns(urlWithQueryString);
            _mocksContext.PublicPreview.Setup(preview => preview.IsMobileBrowser())
                .Returns(isMobile);
            _mocksContext.LayoutBusiness.Setup(layout => layout.GetByLayoutIDNoAccessCheck(layoutId, false))
                .Returns(layoutEntity);
            _mocksContext.LayoutBusiness.Setup(layout => layout.GetLayoutUserId(layoutId))
                .Returns(userId);

            //Act
            CallPageLoad();

            //Assert
            _mocksContext.PublicPreview.VerifyLogs(expectedLogs);
            GetLiteralPreviewText().ShouldBe(invalidLinkErrorMessageBuilder);
        }

        [Test]
        public void Page_Load_LayoutIdAboveZeroAndUserIdNotZeroWhenExceptionThrown_ShouldBeLogged()
        {
            //Arrange
            var layoutId = GetAnyNumber();
            SetLayoutId(layoutId);
            var invalidLinkErrorMessageBuilder = GetInvalidLinkErrorMessage();
            var isMobile = true;
            var urlWithQueryString = new Uri("http://doman.some/?querySting");
            var customerId = GetAnyNumber();
            var userId = Zero;
            var error = $"Invalid LayoutID: {layoutId}";
            var sourceMethod = "PublicPreview.Page_Load";                        
            _mocksContext.Request.Setup(request => request.Url)
                .Returns(urlWithQueryString);
            _mocksContext.PublicPreview.Setup(preview => preview.IsMobileBrowser())
                .Returns(isMobile);
            _mocksContext.LayoutBusiness.Setup(layout => layout.GetByLayoutIDNoAccessCheck(layoutId, false))
                .Throws<Exception>();
            _mocksContext.LayoutBusiness.Setup(layout => layout.GetLayoutUserId(layoutId))
                .Returns(userId);

            //Act
            CallPageLoad();

            //Assert
            GetLiteralPreviewText().ShouldBe(invalidLinkErrorMessageBuilder);
            _mocksContext.ApplicationLog.VerifyLogNonCriticalError(error, sourceMethod, _applicationId);
        }

        [Test]
        public void Page_Load_BlastIdAndEmailIdAboveZeroWhenExceptionThrown_ShouldBeLogged()
        {
            //Arrange
            var blastId = GetAnyNumber();
            var emailId = GetAnyNumber();
            SetBlasteId(blastId);
            SetEmailId(emailId);
            var exceptionMessage = GetUniqueString();
            var exception = new Exception(exceptionMessage);
            _mocksContext.Encryption.Setup(encryption => encryption.GetCurrentByApplicationId(_applicationId))
                .Throws(exception);
            var error = $"Unknown issue. BlastID: {blastId} , EmailID: {emailId} \r\n {exceptionMessage}";
            var sourceMethod = "PublicPreview.Page_Load";

            //Act
            CallPageLoad();

            //Assert
            GetLiteralPreviewText().ShouldBe(GetInvalidLinkErrorMessage());
            _mocksContext.ApplicationLog.VerifyLogNonCriticalError(error, sourceMethod, _applicationId);
        }

        [Test]
        public void Page_Load_BlastIdAndEmailIdAboveZeroAndBlastIsNull_WorksAsExpected()
        {
            //Arrange
            var blastId = GetAnyNumber();
            var emailId = GetAnyNumber();
            SetBlasteId(blastId);
            SetEmailId(emailId);            
            var expectedLogs = new[]
            {
                "BlastID > 0 and EmailID > 0",
                "Got Encryption object",
                "Got Blast object"
            };
            var error = $"Invalid BlastID: {blastId}";
            var sourceMethod = "PublicPreview.Page_Load";

            //Act
            CallPageLoad();

            //Assert
            GetLiteralPreviewText().ShouldBe(GetInvalidLinkErrorMessage());
            _mocksContext.ApplicationLog.VerifyLogNonCriticalError(error, sourceMethod, _applicationId);
            _mocksContext.PublicPreview.VerifyLogs(expectedLogs);
        }

        [Test]
        public void Page_Load_BlastIdAndEmailIdAboveZeroAndInvalidBlast_WorksAsExpected()
        {
            //Arrange
            var blastId = GetAnyNumber();
            var emailId = GetAnyNumber();
            SetBlasteId(blastId);
            SetEmailId(emailId);
            var expectedLogs = new[]
            {
                "Check for merged email",
                "Got Merged email"
            };            
            var error = $"Invalid BlastID: {blastId} or EmailID: {emailId}";
            var sourceMethod = "PublicPreview.Page_Load";
            _mocksContext.BlastBusiness.Setup(blast => blast.GetByBlastIDNoAccessCheck(blastId, false))
                .Returns(GetBlast<BlastSMS>());
            _mocksContext.EmailHistory.Setup(emailHistory => emailHistory.FindMergedEmailID(emailId))
                .Returns(emailId);

            //Act
            CallPageLoad();

            //Assert
            GetLiteralPreviewText().ShouldBe(GetInvalidLinkErrorMessage());
            _mocksContext.ApplicationLog.VerifyLogNonCriticalError(error, sourceMethod, _applicationId);
            _mocksContext.PublicPreview.VerifyLogs(expectedLogs);
        }

        [Test]
        public void Page_Load_BlastIdAndEmailIdAboveZeroWithBlastLayout_WorksAsExpected()
        {
            //Arrange
            var blastId = GetAnyNumber();
            var emailId = GetAnyNumber();
            SetBlasteId(blastId);
            SetEmailId(emailId);
            var expectedLogs = new[]
            {
                "Trigger blast",
                "Got Customer and BC objects",
                "Get refBlast and GroupID",
                "Got HTML Preview",
                "Check if email is in seed list",
                "Got Seed list email",
                "Start getting Layout preview",
                "Got Layout Preview",
                "Done with RSS Feed replacement",
                "Done with link rewriter"
            };
            var blastType = BlastType.Layout;
            var blast = GetBlast<BlastSMS>(blastType, blastId);
            var customer = GetCustomer();
            var baseChannel = GetBaseChannel();
            var htmlPreviewTable = GetHtmlPreviewTable();
            var refBlastId = GetAnyNumber();
            var refBlast = GetBlast<BlastSMS>(blastType, refBlastId);
            var emailColumnsNamesTable = GetEmailColumnNamesTable();
            var email = GetEmail();
            var useOldSite = false;
            _mocksContext.AppSettings.Add(OpenClickUseOldSiteKey, useOldSite.ToString());
            _mocksContext.AppSettings.Add(MvcActivityDomainPathKey, string.Empty);
            _mocksContext.BlastBusiness.Setup(blastBusiness => blastBusiness.GetByBlastIDNoAccessCheck(blastId, false))
                .Returns(blast);
            _mocksContext.BlastBusiness.Setup(blastBusiness =>
                    blastBusiness.GetByBlastIDNoAccessCheck(refBlastId, false))
                .Returns(refBlast);
            _mocksContext.EmailGroup
                .Setup(emailGroup => emailGroup.EmailExistsInCustomerSeedList(emailId, blast.CustomerID.Value))
                .Returns(true);
            _mocksContext.CustomerBusiness
                .Setup(customerBusiness => customerBusiness.GetByCustomerID(blast.CustomerID.Value, false))
                .Returns(customer);
            _mocksContext.BaseChannelBusiness
                .Setup(channelBusiness => channelBusiness.GetByBaseChannelID(customer.BaseChannelID.Value))
                .Returns(baseChannel);
            _mocksContext.BlastBusiness.Setup(blastBusiness => blastBusiness.GetHTMLPreview(refBlastId, emailId))
                .Returns(htmlPreviewTable);
            _mocksContext.BlastSingleBusiness
                .Setup(blastSingleBusiness => blastSingleBusiness.GetRefBlastID(blastId, emailId,
                    blast.CustomerID.Value, blastType.ToString()))
                .Returns(refBlastId);
            _mocksContext.Email.Setup(emailBusiness => emailBusiness.GetByEmailIDNoAccessCheck(emailId))
                .Returns(email);
            _mocksContext.Email.Setup(emailBusiness => emailBusiness.GetColumnNames())
                .Returns(emailColumnsNamesTable);

            //Act
            CallPageLoad();

            //Assert
            _mocksContext.PublicPreview.VerifyLogs(expectedLogs);
        }

        [Test]
        public void Page_Load_BlastIdAndEmailIdAboveZeroWithBlastNotLayoutAndNullSlots_WorksAsExpected()
        {
            //Arrange
            var blastId = GetAnyNumber();
            var emailId = GetAnyNumber();
            SetBlasteId(blastId);
            SetEmailId(emailId);
            var blastType = BlastType.Unknown;
            var blast = GetBlast<BlastSMS>(blastType, blastId);
            var customer = GetCustomer();
            var baseChannel = GetBaseChannel();
            var htmlPreviewTable = GetHtmlPreviewTable(addRows: true);
            var refBlastId = GetAnyNumber();
            var refBlast = GetBlast<BlastSMS>(blastId: refBlastId);
            var emailColumnsNamesTable = GetEmailColumnNamesTable();
            var email = GetEmail();
            var useOldSite = false;
            var expectedQuery = " select * from layout l, template t  where l.layoutid=" +
                $"{blast.LayoutID.Value}  and l.templateid=t.templateid and l.IsDeleted = 0 and t.IsDeleted = 0";
            var layoutTable = GetLayoutTable();
            _mocksContext.AppSettings.Add(OpenClickUseOldSiteKey, useOldSite.ToString());
            _mocksContext.AppSettings.Add(MvcActivityDomainPathKey, string.Empty);
            _mocksContext.BlastBusiness.Setup(blastBusiness => blastBusiness.GetByBlastIDNoAccessCheck(blastId, false))
                .Returns(blast);
            _mocksContext.BlastBusiness.Setup(blastBusiness =>
                    blastBusiness.GetByBlastIDNoAccessCheck(refBlastId, false))
                .Returns(refBlast);
            _mocksContext.EmailGroup
                .Setup(emailGroup => emailGroup.EmailExistsInCustomerSeedList(emailId, blast.CustomerID.Value))
                .Returns(true);
            _mocksContext.CustomerBusiness
                .Setup(customerBusiness => customerBusiness.GetByCustomerID(blast.CustomerID.Value, false))
                .Returns(customer);
            _mocksContext.BaseChannelBusiness
                .Setup(channelBusiness => channelBusiness.GetByBaseChannelID(customer.BaseChannelID.Value))
                .Returns(baseChannel);
            _mocksContext.BlastBusiness.Setup(blastBusiness => blastBusiness.GetHTMLPreview(blastId, emailId))
                .Returns(htmlPreviewTable);
            _mocksContext.BlastSingleBusiness
                .Setup(blastSingleBusiness => blastSingleBusiness.GetRefBlastID(blastId, emailId,
                    blast.CustomerID.Value, blastType.ToString()))
                .Returns(refBlastId);
            _mocksContext.Email.Setup(emailBusiness => emailBusiness.GetByEmailIDNoAccessCheck(emailId))
                .Returns(email);
            _mocksContext.Email.Setup(emailBusiness => emailBusiness.GetColumnNames())
                .Returns(emailColumnsNamesTable);
            _mocksContext.ContentFilter.Setup(contentFilter => contentFilter.HasDynamicContent(blast.LayoutID.Value))
                .Returns(true);
            _mocksContext.DataFunctions.Setup(dataFuncitons => dataFuncitons.GetDataTable(expectedQuery))
                .Returns(layoutTable);

            //Act
            CallPageLoad();

            //Assert
            _mocksContext.TemplateFunctions.Verify(functions => functions.EmailHTMLBody(It.IsAny<string>(),
                It.IsAny<string>(), Zero, Zero, Zero, Zero, Zero, Zero, Zero, Zero, Zero));
        }

        [Test]
        public void Page_Load_BlastIdAndEmailIdAboveZeroWithBlastNotLayoutAndNotNullSlots_WorksAsExpected()
        {
            //Arrange
            var blastId = GetAnyNumber();
            var emailId = GetAnyNumber();
            SetBlasteId(blastId);
            SetEmailId(emailId);
            var blastType = BlastType.Unknown;
            var blast = GetBlast<BlastSMS>(blastType, blastId);
            var customer = GetCustomer();
            var baseChannel = GetBaseChannel();
            var htmlPreviewTable = GetHtmlPreviewTable(addRows: true, nullSlots: false);
            var refBlastId = GetAnyNumber();
            var refBlast = GetBlast<BlastSMS>(blastId: refBlastId);
            var emailColumnsNamesTable = GetEmailColumnNamesTable();
            var email = GetEmail();
            var useOldSite = false;
            var expectedQuery = " select * from layout l, template t  where l.layoutid=" +
                $"{blast.LayoutID.Value}  and l.templateid=t.templateid and l.IsDeleted = 0 and t.IsDeleted = 0";
            var layoutTable = GetLayoutTable();
            _mocksContext.AppSettings.Add(OpenClickUseOldSiteKey, useOldSite.ToString());
            _mocksContext.AppSettings.Add(MvcActivityDomainPathKey, string.Empty);
            _mocksContext.BlastBusiness.Setup(blastBusiness => blastBusiness.GetByBlastIDNoAccessCheck(blastId, false))
                .Returns(blast);
            _mocksContext.BlastBusiness.Setup(blastBusiness =>
                    blastBusiness.GetByBlastIDNoAccessCheck(refBlastId, false))
                .Returns(refBlast);
            _mocksContext.EmailGroup
                .Setup(emailGroup => emailGroup.EmailExistsInCustomerSeedList(emailId, blast.CustomerID.Value))
                .Returns(true);
            _mocksContext.CustomerBusiness
                .Setup(customerBusiness => customerBusiness.GetByCustomerID(blast.CustomerID.Value, false))
                .Returns(customer);
            _mocksContext.BaseChannelBusiness
                .Setup(channelBusiness => channelBusiness.GetByBaseChannelID(customer.BaseChannelID.Value))
                .Returns(baseChannel);
            _mocksContext.BlastBusiness.Setup(blastBusiness => blastBusiness.GetHTMLPreview(blastId, emailId))
                .Returns(htmlPreviewTable);
            _mocksContext.BlastSingleBusiness
                .Setup(blastSingleBusiness => blastSingleBusiness.GetRefBlastID(blastId, emailId,
                    blast.CustomerID.Value, blastType.ToString()))
                .Returns(refBlastId);
            _mocksContext.Email.Setup(emailBusiness => emailBusiness.GetByEmailIDNoAccessCheck(emailId))
                .Returns(email);
            _mocksContext.Email.Setup(emailBusiness => emailBusiness.GetColumnNames())
                .Returns(emailColumnsNamesTable);
            _mocksContext.ContentFilter.Setup(contentFilter => contentFilter.HasDynamicContent(blast.LayoutID.Value))
                .Returns(true);
            _mocksContext.DataFunctions.Setup(dataFuncitons => dataFuncitons.GetDataTable(expectedQuery))
                .Returns(layoutTable);

            //Act
            CallPageLoad();

            //Assert
            _mocksContext.TemplateFunctions.Verify(functions => functions.EmailHTMLBody(It.IsAny<string>(),
                It.IsAny<string>(), One, One, One, One, One, One, One, One, One));
        }

        [Test]
        public void Page_Load_BlastIdAndEmailIdAboveZeroWithBlastAndTestBlastY_WorksAsExpected()
        {
            //Arrange
            const string TestBlastY = "Y";
            var blastId = GetAnyNumber();
            var emailId = GetAnyNumber();
            SetBlasteId(blastId);
            SetEmailId(emailId);
            var blastType = BlastType.Unknown;
            var blast = GetBlast<BlastSMS>(blastType, blastId, TestBlastY);
            var customer = GetCustomer();
            var baseChannel = GetBaseChannel();
            var htmlPreviewTable = GetHtmlPreviewTable(addRows: true, nullSlots: false);
            var refBlastId = GetAnyNumber();
            var refBlast = GetBlast<BlastSMS>(blastId: refBlastId, testBlast: TestBlastY);
            var emailColumnsNamesTable = GetEmailColumnNamesTable();
            var email = GetEmail();
            var useOldSite = false;
            var layoutTable = GetLayoutTable();
            var campaignItemTestBlast = new CampaignItemTestBlast
            {
                CampaignItemTestBlastID = GetAnyNumber()
            };
            _mocksContext.AppSettings.Add(OpenClickUseOldSiteKey, useOldSite.ToString());
            _mocksContext.AppSettings.Add(MvcActivityDomainPathKey, string.Empty);
            _mocksContext.BlastBusiness.Setup(blastBusiness => blastBusiness.GetByBlastIDNoAccessCheck(blastId, false))
                .Returns(blast);
            _mocksContext.BlastBusiness.Setup(blastBusiness =>
                    blastBusiness.GetByBlastIDNoAccessCheck(refBlastId, false))
                .Returns(refBlast);
            _mocksContext.EmailGroup
                .Setup(emailGroup => emailGroup.EmailExistsInCustomerSeedList(emailId, blast.CustomerID.Value))
                .Returns(true);
            _mocksContext.CustomerBusiness
                .Setup(customerBusiness => customerBusiness.GetByCustomerID(blast.CustomerID.Value, false))
                .Returns(customer);
            _mocksContext.BaseChannelBusiness
                .Setup(channelBusiness => channelBusiness.GetByBaseChannelID(customer.BaseChannelID.Value))
                .Returns(baseChannel);
            _mocksContext.BlastBusiness.Setup(blastBusiness => blastBusiness.GetHTMLPreview(blastId, emailId))
                .Returns(htmlPreviewTable);
            _mocksContext.BlastSingleBusiness
                .Setup(blastSingleBusiness => blastSingleBusiness.GetRefBlastID(blastId, emailId,
                    blast.CustomerID.Value, blastType.ToString()))
                .Returns(refBlastId);
            _mocksContext.Email.Setup(emailBusiness => emailBusiness.GetByEmailIDNoAccessCheck(emailId))
                .Returns(email);
            _mocksContext.Email.Setup(emailBusiness => emailBusiness.GetColumnNames())
                .Returns(emailColumnsNamesTable);
            _mocksContext.ContentFilter.Setup(contentFilter => contentFilter.HasDynamicContent(blast.LayoutID.Value))
                .Returns(true);
            _mocksContext.DataFunctions.Setup(dataFuncitons => dataFuncitons.GetDataTable(It.IsAny<string>()))
                .Returns(layoutTable);
            _mocksContext.CampaignItemTestBlast
                .Setup(testBlast => testBlast.GetByBlastIDNoAccessCheck(blast.BlastID, false))
                .Returns(campaignItemTestBlast);

            //Act
            CallPageLoad();

            //Assert
            _mocksContext.TemplateFunctions.Verify(functions => functions.EmailHTMLBody(It.IsAny<string>(),
                It.IsAny<string>(), One, One, One, One, One, One, One, One, One));
            _mocksContext.CampaignItem
                .Verify(item => item.GetByCampaignItemTestBlastIDNoAccessCheck(
                    campaignItemTestBlast.CampaignItemTestBlastID, false));
        }

        [Test]
        public void Page_Load_BlastIdAndEmailIdAboveZeroWithBlastTestBlastAndTransnippetNegative_ShouldThrowException()
        {
            //Arrange
            var blastId = GetAnyNumber();
            var emailId = GetAnyNumber();
            SetBlasteId(blastId);
            SetEmailId(emailId);
            var blastType = BlastType.Unknown;
            var blast = GetBlast<BlastSMS>(blastType, blastId);
            var customer = GetCustomer();
            var baseChannel = GetBaseChannel();
            var htmlPreviewTable = GetHtmlPreviewTable(addRows: true, nullSlots: false);
            var refBlastId = GetAnyNumber();
            var refBlast = GetBlast<BlastSMS>(blastId: refBlastId);
            var emailColumnsNamesTable = GetEmailColumnNamesTable();
            var email = GetEmail();
            var useOldSite = false;
            var layoutTable = GetLayoutTable();
            var campaignItemTestBlast = new CampaignItemTestBlast
            {
                CampaignItemTestBlastID = GetAnyNumber()
            };            
            _mocksContext.AppSettings.Add(OpenClickUseOldSiteKey, useOldSite.ToString());
            _mocksContext.AppSettings.Add(MvcActivityDomainPathKey, string.Empty);
            _mocksContext.BlastBusiness.Setup(blastBusiness => blastBusiness.GetByBlastIDNoAccessCheck(blastId, false))
                .Returns(blast);
            _mocksContext.BlastBusiness.Setup(blastBusiness =>
                    blastBusiness.GetByBlastIDNoAccessCheck(refBlastId, false))
                .Returns(refBlast);
            _mocksContext.EmailGroup
                .Setup(emailGroup => emailGroup.EmailExistsInCustomerSeedList(emailId, blast.CustomerID.Value))
                .Returns(true);
            _mocksContext.CustomerBusiness
                .Setup(customerBusiness => customerBusiness.GetByCustomerID(blast.CustomerID.Value, false))
                .Returns(customer);
            _mocksContext.BaseChannelBusiness
                .Setup(channelBusiness => channelBusiness.GetByBaseChannelID(customer.BaseChannelID.Value))
                .Returns(baseChannel);
            _mocksContext.BlastBusiness.Setup(blastBusiness => blastBusiness.GetHTMLPreview(blastId, emailId))
                .Returns(htmlPreviewTable);
            _mocksContext.BlastSingleBusiness
                .Setup(blastSingleBusiness => blastSingleBusiness.GetRefBlastID(blastId, emailId,
                    blast.CustomerID.Value, blastType.ToString()))
                .Returns(refBlastId);
            _mocksContext.Email.Setup(emailBusiness => emailBusiness.GetByEmailIDNoAccessCheck(emailId))
                .Returns(email);
            _mocksContext.Email.Setup(emailBusiness => emailBusiness.GetColumnNames())
                .Returns(emailColumnsNamesTable);
            _mocksContext.ContentFilter.Setup(contentFilter => contentFilter.HasDynamicContent(blast.LayoutID.Value))
                .Returns(true);
            _mocksContext.DataFunctions.Setup(dataFuncitons => dataFuncitons.GetDataTable(It.IsAny<string>()))
                .Returns(layoutTable);
            _mocksContext.CampaignItemTestBlast
                .Setup(testBlast => testBlast.GetByBlastIDNoAccessCheck(blast.BlastID, false))
                .Returns(campaignItemTestBlast);
            _mocksContext.Content.Setup(content => content.CheckForTransnippet(It.IsAny<string>()))
                .Returns(NegativeOne);
            var expectedExceptionMessage = "Error Transnippet in HTML content";
            var error = $"Unknown issue. BlastID: {blastId} , EmailID: {emailId} \r\n {expectedExceptionMessage}";
            var sourceMethod = "PublicPreview.Page_Load";

            //Act
            CallPageLoad();

            //Assert
            _mocksContext.ApplicationLog.VerifyLogNonCriticalError(error, sourceMethod, _applicationId);
        }

        [Test]
        public void Page_Load_BlastIdAndEmailIdAboveZeroWithBlastTestBlastAndTransnippetAboveZero_WorksAsExpected()
        {
            //Arrange
            var blastId = GetAnyNumber();
            var emailId = GetAnyNumber();
            SetBlasteId(blastId);
            SetEmailId(emailId);
            var blastType = BlastType.Unknown;
            var blast = GetBlast<BlastSMS>(blastType, blastId);
            var customer = GetCustomer();
            var baseChannel = GetBaseChannel();
            var htmlPreviewTable = GetHtmlPreviewTable(addRows: true, nullSlots: false);
            var refBlastId = GetAnyNumber();
            var refBlast = GetBlast<BlastSMS>(blastId: refBlastId);
            var emailColumnsNamesTable = GetEmailColumnNamesTable();
            var email = GetEmail();
            var useOldSite = false;
            var layoutTable = GetLayoutTable();
            var campaignItemTestBlast = new CampaignItemTestBlast
            {
                CampaignItemTestBlastID = GetAnyNumber()
            };            
            _mocksContext.AppSettings.Add(OpenClickUseOldSiteKey, useOldSite.ToString());
            _mocksContext.AppSettings.Add(MvcActivityDomainPathKey, string.Empty);
            _mocksContext.BlastBusiness.Setup(blastBusiness => blastBusiness.GetByBlastIDNoAccessCheck(blastId, false))
                .Returns(blast);
            _mocksContext.BlastBusiness.Setup(blastBusiness =>
                    blastBusiness.GetByBlastIDNoAccessCheck(refBlastId, false))
                .Returns(refBlast);
            _mocksContext.EmailGroup
                .Setup(emailGroup => emailGroup.EmailExistsInCustomerSeedList(emailId, blast.CustomerID.Value))
                .Returns(true);
            _mocksContext.CustomerBusiness
                .Setup(customerBusiness => customerBusiness.GetByCustomerID(blast.CustomerID.Value, false))
                .Returns(customer);
            _mocksContext.BaseChannelBusiness
                .Setup(channelBusiness => channelBusiness.GetByBaseChannelID(customer.BaseChannelID.Value))
                .Returns(baseChannel);
            _mocksContext.BlastBusiness.Setup(blastBusiness => blastBusiness.GetHTMLPreview(blastId, emailId))
                .Returns(htmlPreviewTable);
            _mocksContext.BlastSingleBusiness
                .Setup(blastSingleBusiness => blastSingleBusiness.GetRefBlastID(blastId, emailId,
                    blast.CustomerID.Value, blastType.ToString()))
                .Returns(refBlastId);
            _mocksContext.Email.Setup(emailBusiness => emailBusiness.GetByEmailIDNoAccessCheck(emailId))
                .Returns(email);
            _mocksContext.Email.Setup(emailBusiness => emailBusiness.GetColumnNames())
                .Returns(emailColumnsNamesTable);
            _mocksContext.ContentFilter.Setup(contentFilter => contentFilter.HasDynamicContent(blast.LayoutID.Value))
                .Returns(true);
            _mocksContext.DataFunctions.Setup(dataFuncitons => dataFuncitons.GetDataTable(It.IsAny<string>()))
                .Returns(layoutTable);
            _mocksContext.CampaignItemTestBlast
                .Setup(testBlast => testBlast.GetByBlastIDNoAccessCheck(blast.BlastID, false))
                .Returns(campaignItemTestBlast);
            _mocksContext.Content.Setup(content => content.CheckForTransnippet(It.IsAny<string>()))
                .Returns(GetAnyNumber());

            //Act
            CallPageLoad();

            //Assert
            _mocksContext.Content.Verify(content => content.ModifyHTML(It.IsAny<string>(), It.IsAny<DataTable>()));
        }

        [Test]
        public void Page_Load_BlastIdAndEmailIdAboveZeroWithBlastAndTestBlastAndCampaignItemNotNull_WorksAsExpected()
        {
            //Arrange
            var blastId = GetAnyNumber();
            var emailId = GetAnyNumber();
            SetBlasteId(blastId);
            SetEmailId(emailId);
            var blastType = BlastType.Unknown;
            var blast = GetBlast<BlastSMS>(blastType, blastId);
            var customer = GetCustomer();
            var baseChannel = GetBaseChannel();
            var htmlPreviewTable = GetHtmlPreviewTable(addRows: true, nullSlots: false);
            var refBlastId = GetAnyNumber();
            var refBlast = GetBlast<BlastSMS>(blastId: refBlastId);
            var emailColumnsNamesTable = GetEmailColumnNamesTable();
            var email = GetEmail();
            var useOldSite = false;
            var layoutTable = GetLayoutTable();
            var campaignItem = new CampaignItem
            {
                CampaignItemID = GetAnyNumber()
            };
            var fbProfile = new Dictionary<string, string>
            {
                {"link", GetUniqueString() }
            };
            var encryptionEntity = new Encryption
            {
                ID = GetAnyNumber()
            };
            var matchingString = GetUniqueString();
            var imageDomainPath = GetUniqueString();
            var imagePath = GetUniqueString();
            var shareLink = GetUniqueString();
            var socialMedia = GetSocialMedia(matchingString, imagePath, shareLink);
            var expectedStrings = new[]
            {
                $"<td><a href=\"{matchingString}\"><img width=\"30px\" "+
                "style=\"text-decoration:none;border:none;height:30px;width:30px;\" src=\"" +
                $"{imageDomainPath}{imagePath}\" alt=\"FaceBook\" /></a></td>",
                $"<td><a href=\"{matchingString}\"><img style=\"text-decoration:none;border:"+
                $"none;height:30px;width:30px;\" src=\"{imageDomainPath}{imagePath}\" alt=\"Twitter\" /></a>"+
                "</td>",
                $"<td><a href=\"{matchingString}\"><img style=\"text-decoration:none;border:none"+
                $";height:30px;width:30px;\" src=\"{imageDomainPath}{imagePath}\" alt=\"LinkedIn\" /></a></td>",
                $"<a href=\"{shareLink}\"><img src=\"{imageDomainPath}{imagePath}\" alt=\"FB "+
                "Like\" style=\"border:none;height:30px;width:30px;text-decoration:none;\" /></a>",
                $"<td><a href=\"{matchingString}\"><img style=\"text-decoration:none;border:none"+
                $";height:30px;width:30px;\" src=\"{imageDomainPath}{imagePath}\" alt=\"Forward\" /></a></td>"
            };
            _mocksContext.AppSettings.Add("Activity_DomainPath", GetUniqueString());
            _mocksContext.AppSettings.Add("Social_DomainPath", GetUniqueString());
            _mocksContext.AppSettings.Add("SocialClick", GetUniqueString());
            _mocksContext.AppSettings.Add(ImageDomainPathKey, imageDomainPath);
            _mocksContext.AppSettings.Add(OpenClickUseOldSiteKey, useOldSite.ToString());
            _mocksContext.AppSettings.Add(MvcActivityDomainPathKey, string.Empty);
            _mocksContext.BlastBusiness.Setup(blastBusiness => blastBusiness.GetByBlastIDNoAccessCheck(blastId, false))
                .Returns(blast);
            _mocksContext.BlastBusiness.Setup(blastBusiness =>
                    blastBusiness.GetByBlastIDNoAccessCheck(refBlastId, false))
                .Returns(refBlast);
            _mocksContext.EmailGroup
                .Setup(emailGroup => emailGroup.EmailExistsInCustomerSeedList(emailId, blast.CustomerID.Value))
                .Returns(true);
            _mocksContext.CustomerBusiness
                .Setup(customerBusiness => customerBusiness.GetByCustomerID(blast.CustomerID.Value, false))
                .Returns(customer);
            _mocksContext.BaseChannelBusiness
                .Setup(channelBusiness => channelBusiness.GetByBaseChannelID(customer.BaseChannelID.Value))
                .Returns(baseChannel);
            _mocksContext.BlastBusiness.Setup(blastBusiness => blastBusiness.GetHTMLPreview(blastId, emailId))
                .Returns(htmlPreviewTable);
            _mocksContext.BlastSingleBusiness
                .Setup(blastSingleBusiness => blastSingleBusiness.GetRefBlastID(blastId, emailId,
                    blast.CustomerID.Value, blastType.ToString()))
                .Returns(refBlastId);
            _mocksContext.Email.Setup(emailBusiness => emailBusiness.GetByEmailIDNoAccessCheck(emailId))
                .Returns(email);
            _mocksContext.Email.Setup(emailBusiness => emailBusiness.GetColumnNames())
                .Returns(emailColumnsNamesTable);
            _mocksContext.ContentFilter.Setup(contentFilter => contentFilter.HasDynamicContent(blast.LayoutID.Value))
                .Returns(true);
            _mocksContext.DataFunctions.Setup(dataFuncitons => dataFuncitons.GetDataTable(It.IsAny<string>()))
                .Returns(layoutTable);
            _mocksContext.CampaignItem.Setup(item => item.GetByBlastIDNoAccessCheck(blast.BlastID, false))
                .Returns(campaignItem);
            _mocksContext.CampaignItemSocialMedia.Setup(item => item.GetByCampaignItemID(campaignItem.CampaignItemID))
                .Returns(GetCampaignItemSocialMediaList());
            _mocksContext.SocialMedia.Setup(social => social.GetSocialMediaById(It.IsAny<int>()))
                .Returns(socialMedia);
            _mocksContext.SocialMedia.Setup(social => social.GetBySocialMediaAuthID(It.IsAny<int>()))
                .Returns(new SocialMediaAuth());
            _mocksContext.SocialMedia.Setup(social => social.GetFBUserProfile(It.IsAny<string>()))
                .Returns(fbProfile);
            _mocksContext.SocialMedia.Setup(social => social.GetSocialMediaCanShare())
                .Returns(GetSocialMediaCanShare());
            _mocksContext.Encryption.Setup(encryption => encryption.GetCurrentByApplicationId(It.IsAny<int>()))
                .Returns(encryptionEntity);

            //Act
            CallPageLoad();

            //Assert
            foreach (var expectedString in expectedStrings)
            {
                GetLiteralPreviewText().ShouldContain(expectedString);
            }
        }

        [Test]
        public void Page_Load_BlastIdAndEmailIdAboveZeroWithBlastAndBreakupHtmlMail_WorksAsExpected()
        {
            //Arrange
            var blastId = GetAnyNumber();
            var emailId = GetAnyNumber();
            SetBlasteId(blastId);
            SetEmailId(emailId);
            var blastType = BlastType.Unknown;
            const string TestBlast = "y";
            var blast = GetBlast<BlastSMS>(blastType, blastId, TestBlast);
            var customer = GetCustomer();
            var baseChannel = GetBaseChannel();
            var htmlPreviewTable = GetHtmlPreviewTable(addRows: true, nullSlots: false);
            var refBlastId = GetAnyNumber();
            var refBlast = GetBlast<BlastSMS>(blastId: refBlastId);
            var emailColumnsNamesTable = GetEmailColumnNamesTable();
            var email = GetEmail();
            var useOldSite = false;
            var layoutTable = GetLayoutTable();
            var campaignItemTestBlast = new CampaignItemTestBlast
            {
                CampaignItemTestBlastID = GetAnyNumber()
            };
            _mocksContext.AppSettings.Add(OpenClickUseOldSiteKey, useOldSite.ToString());
            _mocksContext.AppSettings.Add(MvcActivityDomainPathKey, string.Empty);
            _mocksContext.BlastBusiness.Setup(blastBusiness => blastBusiness.GetByBlastIDNoAccessCheck(blastId, false))
                .Returns(blast);
            _mocksContext.BlastBusiness.Setup(blastBusiness =>
                    blastBusiness.GetByBlastIDNoAccessCheck(refBlastId, false))
                .Returns(refBlast);
            _mocksContext.EmailGroup
                .Setup(emailGroup => emailGroup.EmailExistsInCustomerSeedList(emailId, blast.CustomerID.Value))
                .Returns(true);
            _mocksContext.CustomerBusiness
                .Setup(customerBusiness => customerBusiness.GetByCustomerID(blast.CustomerID.Value, false))
                .Returns(customer);
            _mocksContext.BaseChannelBusiness
                .Setup(channelBusiness => channelBusiness.GetByBaseChannelID(customer.BaseChannelID.Value))
                .Returns(baseChannel);
            _mocksContext.BlastBusiness.Setup(blastBusiness => blastBusiness.GetHTMLPreview(blastId, emailId))
                .Returns(htmlPreviewTable);
            _mocksContext.BlastSingleBusiness
                .Setup(blastSingleBusiness => blastSingleBusiness.GetRefBlastID(blastId, emailId,
                    blast.CustomerID.Value, blastType.ToString()))
                .Returns(refBlastId);
            _mocksContext.Email.Setup(emailBusiness => emailBusiness.GetByEmailIDNoAccessCheck(emailId))
                .Returns(email);
            _mocksContext.Email.Setup(emailBusiness => emailBusiness.GetColumnNames())
                .Returns(emailColumnsNamesTable);
            _mocksContext.ContentFilter.Setup(contentFilter => contentFilter.HasDynamicContent(blast.LayoutID.Value))
                .Returns(true);
            _mocksContext.DataFunctions.Setup(dataFuncitons => dataFuncitons.GetDataTable(It.IsAny<string>()))
                .Returns(layoutTable);
            _mocksContext.CampaignItemTestBlast
                .Setup(testBlast => testBlast.GetByBlastIDNoAccessCheck(blast.BlastID, false))
                .Returns(campaignItemTestBlast);
            _mocksContext.Content.Setup(content => content.CheckForTransnippet(It.IsAny<string>()))
                .Returns(GetAnyNumber());
            _mocksContext.Content.Setup(content => content.ModifyHTML(It.IsAny<string>(), It.IsAny<DataTable>()))
                .Returns(GetHtml());
            _mocksContext.BlastLink.Setup(blastLink => blastLink.GetByBlastLinkID(blastId, It.IsAny<int>()))
                .Returns(GetLink());
            ShimDataRow.AllInstances.ItemGetString = (row, name) =>
            {
                if (row.Table.Columns.Contains(name))
                {
                    return ShimsContext.ExecuteWithoutShims(() => row[name]);
                }
                else
                {
                    return GetUniqueString();
                }
            };

            //Act
            CallPageLoad();

            //Assert
            var html = GetLiteralPreviewText();
            foreach (var line in GetScript())
            {
                html.ShouldContain(line);
            }
        }

        private BlastLink GetLink()
        {
            return new BlastLink
            {
                LinkURL = "http://Somedomain.link/%%slot1%%"
            };
        }

        private string GetHtml()
        {
            const string Separator = "%%";
            var lidString = $"lid={GetAnyNumber()}";
            var lines = new[]
            {
                $"##{GetUniqueString()},{GetUniqueString()}|{GetUniqueString()}|{GetUniqueString()}|"+
                $"{GetUniqueString()}|{GetUniqueString()}## ##TRANSNIPPET|",
                "ECN_Encrypt_Open",
                GetUniqueString(),
                $"ECN_Encrypt_Any_Thing{lidString}",
                GetUniqueString(),
                $"ECN_Encrypt_Any_",
                GetUniqueString(),
                $"ECN_Encrypt_Any{lidString}",
                GetUniqueString(),
                "ECN_Encrypt_Open",
                GetUniqueString(),
            };
            return string.Join(Separator, lines);
        }

        private List<SocialMedia> GetSocialMediaCanShare()
        {
            var anyString = GetUniqueString();
            return new List<SocialMedia>
            {
                GetSocialMedia(anyString, anyString, anyString, 4),
                GetSocialMedia(anyString, anyString, anyString, 5),
                GetSocialMedia(anyString, anyString, anyString, 6)
            };
        }

        private SocialMedia GetSocialMedia(
            string matchingString,
            string imagePath,
            string shareLink,
            int? id = null)
        {
            return new SocialMedia
            {
                SocialMediaID = id ?? GetAnyNumber(),
                MatchString = matchingString,
                ImagePath = imagePath,
                ShareLink = shareLink,
            };
        }

        private List<CampaignItemSocialMedia> GetCampaignItemSocialMediaList()
        {
            const int IdMax = 5;
            return Enumerable.Range(1, IdMax)
                .Select(id => new CampaignItemSocialMedia
                {
                    SimpleShareDetailID = null,
                    SocialMediaID = id,
                    SocialMediaAuthID = GetAnyNumber()
                }).ToList();
        }

        private DataTable GetLayoutTable()
        {
            var result = new DataTable();
            var stringColumns = new[]
            {
                "TemplateSource",
                "TableOptions",
                "TemplateText"
            };
            var numericColumns = new[]
            {
                "ContentSlot1",
                "ContentSlot2",
                "ContentSlot3",
                "ContentSlot4",
                "ContentSlot5",
                "ContentSlot6",
                "ContentSlot7",
                "ContentSlot8",
                "ContentSlot9",
            };
            foreach (var column in stringColumns.Concat(numericColumns))
            {
                result.Columns.Add(column);
            }
            var row = result.NewRow();
            foreach (var column in stringColumns)
            {
                row[column] = GetUniqueString();
            }
            foreach (var column in numericColumns)
            {
                row[column] = Zero;
            }
            result.Rows.Add(row);
            return result;
        }

        private Email GetEmail()
        {
            return new Email
            {
            };
        }

        private DataTable GetEmailColumnNamesTable()
        {
            var result = new DataTable();
            result.Columns.Add();
            foreach (var property in GetEmailProperties())
            {
                var row = result.NewRow();
                row[0] = property;
                result.Rows.Add(row);
            }

            return result;
        }

        private DataTable GetHtmlPreviewTable(bool addRows = false, bool nullSlots = true)
        {
            var result = new DataTable();
            var additionalProperties = new[]
            {
                "BlastID",
                "LayoutID",
                "GroupID"
            };
            var slotsColumns = new[]
            {
                "slot1",
                "slot2",
                "slot3",
                "slot4",
                "slot5",
                "slot6",
                "slot7",
                "slot8",
                "slot9",
            };
            var allColumns = GetEmailProperties()
                .Union(additionalProperties)
                .Union(slotsColumns);
            foreach (var property in allColumns)
            {
                result.Columns.Add(property);
            }
            if (addRows)
            {
                var row = result.NewRow();
                if (!nullSlots)
                {
                    foreach (var slot in slotsColumns)
                    {
                        row[slot] = $"{One}";
                    }
                }
                result.Rows.Add(row);
            }
            return result;
        }

        private IEnumerable<string> GetEmailProperties()
        {
            var email = new Email();
            foreach (var property in email.GetType().GetProperties())
            {
                if (property.GetValue(email) != null)
                {
                    yield return property.Name;
                }
            }
        }

        private BaseChannel GetBaseChannel()
        {
            return new BaseChannel
            {
            };
        }

        private Customer GetCustomer()
        {
            return new Customer
            {
                BaseChannelID = GetAnyNumber()
            };
        }

        private T GetBlast<T>(BlastType? blastType = null, int? blastId = null, string testBlast = EmptyString)
            where T : BlastAbstract, new()
        {
            return new T
            {
                BlastID = blastId ?? GetAnyNumber(),
                GroupID = GetAnyNumber(),
                CustomerID = GetAnyNumber(),
                BlastType = blastType?.ToString() ?? string.Empty,
                LayoutID = GetAnyNumber(),
                TestBlast = testBlast
            };
        }

        private LayoutEntity GetLayoutEntity(int? customerId)
        {
            return new LayoutEntity
            {
                CustomerID = customerId
            };
        }

        private string GetLiteralPreviewText()
        {
            const string FieldName = "literalPreview";
            var literal = _publicPreviewPrivateObject.GetField(FieldName) as Literal;
            return literal?.Text;
        }

        private void SetLayoutId(int id)
        {
            const string FieldName = "LayoutID";
            _publicPreviewPrivateObject.SetField(FieldName, id);
        }

        private void SetBlasteId(int id)
        {
            const string FieldName = "BlastID";
            _publicPreviewPrivateObject.SetField(FieldName, id);
        }

        private void SetEmailId(int id)
        {
            const string FieldName = "EmailID";
            _publicPreviewPrivateObject.SetField(FieldName, id);
        }

        private void CallPageLoad()
        {
            const string MethodName = "Page_Load";
            object sender = null;
            EventArgs eventArgs = null;
            _publicPreviewPrivateObject.Invoke(MethodName, sender, eventArgs);
        }

        private static string GetInvalidLinkErrorMessage()
        {
            var segments = new[]
            {
                "We're sorry, but the link you are requesting appears to be ",
                "invalid.<br><br>If you typed the link in your email browser, please either retype the link making ",
                "note of case sensitivity ('a' and 'A' are different), or click on the link in the original email " ,
                "you received.<br><br>If you clicked on the link in a text version email, it may have been broken " ,
                "by your email program. Please reply to the email you received and let us know which link caused a " ,
                "problem. We’ll correct the problem and send you a replacement link.<br><br>Sincerely,<br>Customer " ,
                "Support"
            };
            return String.Join(string.Empty, segments);
        }

        private string[] GetScript()
        {
            return new[]
            {
                @"<script type=""text/javascript"" src=""http://ecn5.com/ecn.accounts/scripts/js/jquery-1.7.2.min.js""></script>",
                @"<script src=""http://ecn5.com/ecn.accounts/scripts/js/jquery-ui-1.8.22.custom.min.js"" type=""text/javascript""></script>",
                @"<script type=""text/javascript"" src=""http://cdn.jsdelivr.net/qtip2/2.2.1/jquery.qtip.js""></script>",
                @"<script src=""http://cdn.jsdelivr.net/jquery.cookie/1.4.0/jquery.cookie.min.js""></script>",
            };
        }
    }
}
