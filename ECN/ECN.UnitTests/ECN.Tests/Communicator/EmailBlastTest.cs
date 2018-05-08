using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ecn.common.classes;
using ecn.communicator.classes;
using ecn.communicator.classes.EmailWriter;
using ecn.communicator.classes.EmailWriter.Fakes;
using ecn.communicator.classes.Fakes;
using ECN.Tests.Common.DataFunctions_cs;
using ECN_Framework.Common.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Communicator;
using ECN_Framework_Entities.Content;
using KMPlatform.BusinessLogic.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using Accounts = ECN_Framework_Entities.Accounts;
using KmEntity = KM.Common.Entity;
using KMPlatformEntity = KMPlatform.Entity;

namespace ECN.Tests.Communicator
{
    /// <summary>
    /// Unit test for <see cref="EmailBlast"/> class.
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class EmailBlastTest : Fakes
    {
        private const string GetMimeMessage = "GetMIMEMessage";
        private const string BreakupHtmlMail = "BreakupHTMLMail";
        private const string BreakupTextMail = "BreakupTextMail";
        private const string SocialShareUsed = "SocialShareUsed";
        private const string PersonalizedContent = "_PersonalizedContent";
        private const string BreakupSubject = "BreakupSubject";
        private const string BreakupFromEmail = "BreakupFromEmail";
        private const string BreakupFromName = "BreakupFromName";
        private const string BreakupReplyToEmail = "breakupReplyToEmail";
        private const string MtaName = "_MTAName";
        private const string MtaNameValue = "PORT25";
        private const string Type = "type";
        private const string DefaultUrl = "http://km.all.com";
        private const string KmCommonApplicationValue = "100";
        private const string PersonalizedContentId = "1";
        private const string SocialClickButton = "Click_Next";
        private const string DefaultSplitter = "%%";
        private const string ParallelThreads = "1";
        private const int CustomerId = 1;
        private EmailBlast _emailBlast;
        private PrivateObject _privateObject;

        [SetUp]
        public void SetUp()
        {
            SetupFakes();
            var blastID = 1;
            var emailclone = new DataTable();
            var htmlbody = string.Empty;
            var textbody = string.Empty;
            var resend = true;
            var testBlast = true;
            var dynamicFromName = string.Empty;
            var dynamicFromEmail = string.Empty;
            var dynamicReplyToEmail = string.Empty;
            var omitDocType = true;

            var blast = CreateBlastObject();
            CreateBlastPageFakeObject(bool.FalseString.ToLower());
            _emailBlast = new EmailBlast(blastID, emailclone, htmlbody, textbody, resend, testBlast,
                dynamicFromName, dynamicFromEmail, dynamicReplyToEmail, omitDocType, blast);
            var blastConfig = new BlastConfig(CustomerId);
            blastConfig.GetType().GetField(MtaName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(blastConfig, MtaNameValue);
            _emailBlast.blastconfig = blastConfig;
            _emailBlast.HasTransnippets = true;
            _privateObject = new PrivateObject(_emailBlast);

            var transnippet = CreateTransnippet();
            TransnippetHolder.Transnippet = transnippet;
            TransnippetHolder.TransnippetTablesHTML = transnippet;
            TransnippetHolder.TransnippetTablesTxt = transnippet;
        }

        [TearDown]
        public void Teardown()
        {
            ReleaseFakes();
        }

        [Test]
        public void GetMIMEMessage_FormatTypeCodeIsHtmlAndBlastTypeIsPersonalization_ReturnsMessageObject()
        {
            // Arrange
            SetClassPrivateObjectValue();
            ShimContentTransnippet.CheckForTransnippetString = (x) => { return 1; };
            var table = GetTable();
            var row = table.Rows[0];
            var emailProfileDataSet = table.Select();
            var parameters = new object[] { row, emailProfileDataSet };

            // Act
            var mimeMessage = _privateObject.Invoke(GetMimeMessage, parameters) as string;

            // Assert
            mimeMessage.ShouldNotBeNullOrEmpty();
        }

        [TestCase("text","htm")]
        [TestCase("Personalization", "html")]
        public void GetMIMEMessage_OpenClickUseOldSiteIsTrue_ReturnsMessageObject(string type, string formatCode)
        {
            // Arrange
            ShimConfigurationManager.AppSettingsGet = () => { return CreateAppSettings(bool.TrueString.ToLower()); };
            _emailBlast.BlastEntity.EnableCacheBuster = false;
            SetClassPrivateObjectValue();
            ShimContentTransnippet.CheckForTransnippetString = (x) => { return 0; };
            var table = GetTable(formatCode);
            _privateObject.SetFieldOrProperty(Type, type);
            var row = table.Rows[0];
            var emailProfileDataSet = table.Select();
            var parameters = new object[] { row, emailProfileDataSet };

            // Act
            var mimeMessage = _privateObject.Invoke(GetMimeMessage, parameters) as string;

            // Assert
            mimeMessage.ShouldNotBeNullOrEmpty();
        }

        private void SetClassPrivateObjectValue()
        {
            var personalizedContent = CreatePersonalizedContentObject();
            var breakupHTMLMail = CreateMailMessage();
            var breakupSubject = CreateBreakupSubject();
            var socialShareUsed = CreateSocialShareUsedObject();
            _privateObject.SetFieldOrProperty(BreakupHtmlMail, breakupHTMLMail.ToArray());
            _privateObject.SetFieldOrProperty(BreakupTextMail, breakupHTMLMail.ToArray());
            _privateObject.SetFieldOrProperty(SocialShareUsed, socialShareUsed);
            _privateObject.SetFieldOrProperty(PersonalizedContent, personalizedContent);
            _privateObject.SetFieldOrProperty(BreakupSubject, breakupSubject.ToArray());
            _privateObject.SetFieldOrProperty(BreakupFromEmail, breakupSubject.ToArray());
            _privateObject.SetFieldOrProperty(BreakupFromName, breakupSubject.ToArray());
            _privateObject.SetFieldOrProperty(BreakupReplyToEmail, breakupSubject.ToArray());
        }

        private Dictionary<long, PersonalizedContent> CreatePersonalizedContentObject()
        {
            return new Dictionary<long, PersonalizedContent>
            {
                {
                    100,
                    new PersonalizedContent
                    {
                        BlastID = 1,
                        IsValid = true,
                        HTMLContent = GetMailContent(),
                        TEXTContent = GetMailContent(),
                        EmailSubject = GetMailContent(),
                    }
                }
            };
        }

        private Dictionary<int, string> CreateSocialShareUsedObject()
        {
            var socialShareUsed = new Dictionary<int, string>();
            socialShareUsed.Add(1, "facebook");
            socialShareUsed.Add(2, "twitter");
            socialShareUsed.Add(3, "linkedIn");
            socialShareUsed.Add(4, "instagram");
            socialShareUsed.Add(5, "googlePlus");
            return socialShareUsed;
        }

        private List<string> CreateBreakupSubject()
        {
            var breakupSubject = new List<string>();
            breakupSubject.Add("ECN_Encrypt_Open");
            breakupSubject.Add("PersonalizedContentID");
            breakupSubject.Add("FormatTypeCode");
            breakupSubject.Add("EmailAddress");
            return breakupSubject;
        }

        private static List<string> CreateMailMessage()
        {
            var breakupHtmlMail = new List<string>();
            breakupHtmlMail.Add("ECN_Encrypt_Open");
            breakupHtmlMail.Add("ECN_Encrypt_Demo1");
            breakupHtmlMail.Add("ECN_Encrypt_Open");
            breakupHtmlMail.Add("ECN_Encrypt_Demo1_Demo2_Demo3_Demo4");
            breakupHtmlMail.Add("ECN_Encrypt_Open");
            breakupHtmlMail.Add("ECN_Encrypt_Demo1");
            breakupHtmlMail.Add("ECN_Encrypt_Open");
            breakupHtmlMail.Add("ECN_Encrypt_Open");
            breakupHtmlMail.Add("SamleTest12345");
            breakupHtmlMail.Add("SamleTest12345");
            breakupHtmlMail.Add("SamleTest12345");
            breakupHtmlMail.Add("Admin_Unit_Test");
            breakupHtmlMail.Add("slot1");
            return breakupHtmlMail;
        }

        private static DataTable GetTable(string formatTypeCode = "html")
        {
            var table = new DataTable();
            table.Columns.Add("EmailID", typeof(string));
            table.Columns.Add("ECN_Encrypt_Open", typeof(string));
            table.Columns.Add("PersonalizedContentID", typeof(long));
            table.Columns.Add("GroupID", typeof(string));
            table.Columns.Add("FormatTypeCode", typeof(string));
            table.Columns.Add("EmailAddress", typeof(string));
            table.Columns.Add("BounceAddress", typeof(string));
            table.Columns.Add("MailRoute", typeof(string));
            table.Columns.Add("BlastID", typeof(string));
            table.Columns.Add("IsMTAPriority", typeof(string));
            table.Columns.Add("CustomerID", typeof(string));
            table.Columns.Add("SamleTest12345", typeof(string));
            table.Columns.Add("Admin_Unit_Test", typeof(string));
            table.Columns.Add("ECN_Encrypt_Demo1", typeof(string));
            table.Columns.Add("ECN_Encrypt_Demo1_Demo2_Demo3_Demo4", typeof(string));
            table.Columns.Add("slot1", typeof(string));

            table.Rows.Add("test@unittest.com", "ECN_Encrypt_Open", 100, "20", formatTypeCode, "testAdmin@unittest.com",
                "testAdmin@unittest.com", "MailRoute", "1", "1", "1", "SamleTest12345", "Admin_Unit_Test", "ECN_Encrypt_Demo1", "CN_Encrypt_Demo1_Demo2_Demo3_Demo4", "slot1");
            table.Rows.Add("test@unittest.com", "ECN_Encrypt_Open", 100, "20", formatTypeCode, "testAdmin@unittest.com",
                "testAdmin@unittest.com", "MailRoute", "1", "1", "1", "SamleTest12345", "Admin_Unit_Test", "ECN_Encrypt_Demo1", "CN_Encrypt_Demo1_Demo2_Demo3_Demo4", "slot1");
            return table;
        }

        private string GetMailContent()
        {
            return string.Join(DefaultSplitter, CreateMailMessage());
        }

        private NameValueCollection CreateAppSettings(string useOldSite)
        {
            var collection = new NameValueCollection();
            collection.Add("ECNEngineAccessKey", Guid.NewGuid().ToString());
            collection.Add("Image_DomainPath", DefaultUrl);
            collection.Add("KMCommon_Application", KmCommonApplicationValue);
            collection.Add("OpenClick_UseOldSite", useOldSite);
            collection.Add("Activity_DomainPath", DefaultUrl);
            collection.Add("MVCActivity_DomainPath", DefaultUrl);
            collection.Add("PersonalizedContentID", PersonalizedContentId);
            collection.Add("Social_DomainPath", DefaultUrl);
            collection.Add("SocialClick", SocialClickButton);
            collection.Add("ParallelThreads", ParallelThreads);
            return collection;
        }

        private List<string> CreateTransnippet()
        {
            var transnippet = new List<string>();
            transnippet.Add("Test,Admin|123456678,test12345|EmailID,ECN_Encrypt_Open");
            transnippet.Add("Test,Admin|123456678,test12345|ECN_Encrypt_Open,EmailID");
            transnippet.Add("Test,Admin|123456678,test12345|123,321");
            return transnippet;
        }

        private Blast CreateBlastObject()
        {
            return new Blast
            {
                CustomerID = 12,
                EmailSubject = "Unit Test",
                EmailFrom = "test@unittest.com",
                EmailFromName = "Admin",
                BlastType = "Personalization",
                EnableCacheBuster = true
            };
        }

        private Accounts.BaseChannel CreateBaseChannelObject()
        {
            return new Accounts.BaseChannel
            {
                AccessKey = Guid.NewGuid(),
                BaseChannelGuid = Guid.NewGuid(),
                BaseChannelID = 1
            };
        }

        private void CreateBlastPageFakeObject(string useOldSite)
        {
            var colloection = CreateAppSettings(useOldSite);
            ShimConfigurationManager.AppSettingsGet = () => { return colloection; };
            ShimBaseChannel.GetByBaseChannelIDInt32 = (x) => { return CreateBaseChannelObject(); };
            ecn.common.classes.Fakes.ShimTemplateFunctions.imgRewriterStringInt32 = (x, y) => { return x; };
            ShimChannelCheck.ConstructorInt32 = (x, y) => { };
            ShimContentTransnippet.CheckForTransnippetString = (x) => { return 0; };
            ShimUser.GetByAccessKeyStringBoolean = (x, y) => { return new KMPlatformEntity.User(); };
            ShimCampaignItemTestBlast.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => { return new CampaignItemTestBlast(); };
            ShimCampaignItem.GetByCampaignItemTestBlastID_NoAccessCheckInt32Boolean = (x, y) => { return new CampaignItem { CampaignID = 1 }; };
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => { return new CampaignItem { CampaignID = 1 }; };
            ShimCampaignItemSocialMedia.GetByCampaignItemIDInt32 = (x) => { return new List<CampaignItemSocialMedia>(); };
            KM.Common.Fakes.ShimEncryption.Base64EncryptStringEncryption = (x, y) => { return DefaultUrl; };
            KM.Common.Fakes.ShimEncryption.EncryptStringEncryption = (x, y) => { return Guid.NewGuid().ToString(); };
            KM.Common.Entity.Fakes.ShimEncryption.GetCurrentByApplicationIDInt32 = (x) => { return new KmEntity.Encryption { ID = 1 }; };
            ShimTransnippetHolder.TransnippetsCountGet = () => { return 1; };
            ShimContentTransnippet.ModifyHTMLStringDataTable = (x, y) => { return x; };
            ShimContentTransnippet.ModifyTEXTStringDataTable = (x, y) => { return x; };
        }
    }
}
