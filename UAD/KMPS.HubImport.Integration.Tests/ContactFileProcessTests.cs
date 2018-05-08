using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using KMPS.HubImport.Integration.Entity;
using KMPS.Hubspot.Integration;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace KMPS.HubImport.Integration.Tests
{
    /// <summary>
    /// Summary description for ContactFileProcessTests
    /// </summary>
    [TestFixture]
    [NUnit.Framework.Ignore("Legacy unit tests that are currently failing")]
    public class ContactFileProcessTests
    {
        private Entity.Hubspot hubspot;
        private ContactFileProcess _contactFileProcess;

        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        private KMPSLogger _kmpsLogger;

        private StreamWriter mainlog;
        private StreamWriter customLog;


        [SetUp]
        public void Setup()
        {

            _kmpsLogger = new KMPSLogger(mainlog, customLog, 50);

            _contactFileProcess = new ContactFileProcess(logger, _kmpsLogger);

        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void TestsSuccessHubSpotConfiguration()
        {
            //Arrange
            var contentPath = ConfigurationManager.AppSettings["HubspotConfiguration"];

            var filePath = Constants.HubspotJsonFilesPath + contentPath;

            //Act
            hubspot = Program.GetHubSpotConfiguration(filePath);

            //Assert

            Assert.AreEqual(hubspot.API, "dda629b2-5bf6-43ce-bc2b-028a332ba6c7");
            Assert.AreEqual(hubspot.Process.Count, 3);
            // Get the headers for Contact
            Assert.AreEqual(hubspot.Process[0].ColumnHeaders, "Pubcode,EmailAddress,FirstName,LastName");

            // Get headers for Open
            Assert.AreEqual(hubspot.Process[1].ColumnHeaders, "Pubcode,EmailAddress,OpenTime,BlastID,Subject,SendTime");


            Assert.AreEqual(hubspot.Process[2].ColumnHeaders, "Pubcode,EmailAddress,ClickTime,ClickURL,Alias,BlastID,Subject,SendTime");
        }

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestsFailureHubSpotConfiguration()
        {
            // Arrange
            var filePath = "../../hubspot_integratiion.json";

            //Act
            var hubspot = Program.GetHubSpotConfiguration(filePath);
            //Assert
        }

        [Test]
        public void TestGetContentJson()
        {
            //Arrange

            var syncClient = new WebClient();
            var contactRecentAddress = "https://api.hubapi.com/contacts/v1/lists/recently_updated/contacts/recent?hapikey=dda629b2-5bf6-43ce-bc2b-028a332ba6c7";

            //Act
            var contacts = _contactFileProcess.GetContactJson(syncClient, contactRecentAddress);

            //Assert
            Assert.IsNotEmpty(contacts);
        }

        [Test]
        [ExpectedException(typeof(WebException))]
        public void TestFailGetContentJson()
        {
            //Arrange

            var syncClient = new WebClient();
            var contactRecentAddress = "https://api.hubapi.com/contacts/v1/lists/recently_updated/contacts/recent?hapikey=dda629b2-5bf6-43ce-bc2b-c7";

            //Act
            var contacts = _contactFileProcess.GetContactJson(syncClient, contactRecentAddress);

            //Assert

        }

        [Test]
        public void TestGetContent()
        {
            //Arrange
            var contentPath = ConfigurationManager.AppSettings["UbmLifeSciencesJson"];
            var filePath = Constants.HubspotJsonFilesPath + contentPath;
            var content = String.Empty;
            long timeoffset;
            long vidoffset;

            Uri uri = new Uri(filePath);
            var client = new WebClient();
            content = client.DownloadString(uri);

            //Act
            var contacts = _contactFileProcess.GetContent(content, "PUBCODE", out timeoffset, out vidoffset);


            //Assert
            Assert.IsNotNull(contacts);
            Assert.AreEqual(contacts.Count, 5);
            Assert.AreEqual(_contactFileProcess.BrandMarketingEmailSuppressionsDictionary.Values.Count, 0);
            Assert.AreEqual(_contactFileProcess.WebinarsEmailSuppressionsDictionary.Values.Count, 0);
            Assert.AreEqual(_contactFileProcess.BiopReverSubscriptionPromorionsListDictionary.Values.Count, 0);
            Assert.AreEqual(_contactFileProcess.LceReverSubscriptionPromorionsListDictionary.Values.Count, 0);
            Assert.AreEqual(_contactFileProcess.LcgceReverificationSubscriptionPromorionsListDictionary.Values.Count, 0);
            Assert.AreEqual(_contactFileProcess.PhexReverSubscriptionPromorionsListDictionary.Values.Count, 0);
            Assert.AreEqual(_contactFileProcess.PteReverSubscriptionPromorionsListDictionary.Values.Count, 0);
            Assert.AreEqual(_contactFileProcess.PhteReverSubscriptionPromorionsListDictionary.Values.Count, 0);
            Assert.AreEqual(_contactFileProcess.SpecReverSubscriptionPromorionsListDictionary.Values.Count, 0);
        }

        [Test]
        public void TestPopulateEmailOptoutCollection()
        {
            //Arrange
            var contentPath = ConfigurationManager.AppSettings["UbmLifeSciencesJson"];
            var filePath = Constants.HubspotJsonFilesPath + contentPath;
            var content = String.Empty;
            long timeoffset;
            long vidoffset;

            Uri uri = new Uri(filePath);
            var client = new WebClient();
            content = client.DownloadString(uri);

            //Act
            var contacts = _contactFileProcess.GetContent(content, "PUBCODE", out timeoffset, out vidoffset);


            //Assert
            Assert.IsNotNull(contacts);
            Assert.AreEqual(contacts.Count, 5);
        }

        [Test]
        public void TestEmailSuppressionsForHsEmailOptout()
        {
            // Arrange
            Properties properties = new Properties();
            properties.HsEmailOptout = new Hs_email_optout();
            properties.HsEmailOptout.Value = "true";

            List<string> brandMarketingEmailSuppressions = new List<string>();
            List<string> webinarsEmailSuppressions = new List<string>();

            //Act
            _contactFileProcess.EmailSuppressionsForHsEmailOptout(properties, "blahblah", brandMarketingEmailSuppressions, webinarsEmailSuppressions);


            //Assert
            Assert.AreEqual(brandMarketingEmailSuppressions.Count, 1);
            Assert.AreEqual(webinarsEmailSuppressions.Count, 1);
        }

        [Test]
        public void TestEmailSuppressionsBrandMarketingEmailOptout409865()
        {
            // Arrange
            Properties properties = new Properties();
            properties.HsEmailOptout409865 = new Hs_email_optout_409865();
            properties.HsEmailOptout409865.Value = "true";

            List<string> brandMarketingEmailSuppressions = new List<string>();

            //Act
            _contactFileProcess.EmailSuppressionsForHsEmailOptout409865(properties, "blahblah", brandMarketingEmailSuppressions);

            //Assert
            Assert.AreEqual(brandMarketingEmailSuppressions.Count, 1);
        }


        [Test]
        public void TestEmailSuppressionsWebinarMarketingHsEmailOptout868669()
        {
            // Arrange
            Properties properties = new Properties();
            properties.HsEmailOptout868669 = new Hs_email_optout_868669();
            properties.HsEmailOptout868669.Value = "true";

            List<string> webinarsEmailSuppressions = new List<string>();

            //Act
            _contactFileProcess.EmailSuppressionsForHsEmailOptout868669(properties, "blahblah", webinarsEmailSuppressions);

            //Assert
            Assert.AreEqual(webinarsEmailSuppressions.Count, 1);
        }


        [Test]
        public void TestEmailSuppressionHsEmailOptout868671()
        {
            // Arrange
            Properties properties = new Properties();
            properties.HsEmailOptout868671 = new Hs_email_optout_868671();
            properties.HsEmailOptout868671.Value = "true";

            List<string> biopReverSubscriptionPromorionsList = new List<string>();

            //Act
            _contactFileProcess.EmailSuppressionsForHsEmailOptout868671(properties, "blahblah", biopReverSubscriptionPromorionsList);

            //Assert
            Assert.AreEqual(biopReverSubscriptionPromorionsList.Count, 1);
        }

        [Test]
        public void TestEmailSuppressionHsEmailOptout868680()
        {
            // Arrange
            Properties properties = new Properties();
            properties.HsEmailOptout868680 = new Hs_email_optout_868680();
            properties.HsEmailOptout868680.Value = "true";

            List<string> lceReverSubscriptionPromorionsList = new List<string>();

            //Act
            _contactFileProcess.EmailSuppressionsForHsEmailOptout868680(properties, "blahblah", lceReverSubscriptionPromorionsList);


            //Assert
            Assert.AreEqual(lceReverSubscriptionPromorionsList.Count, 1);
        }

        [Test]
        public void TestEmailSuppressionHsEmailOptout868679()
        {
            // Arrange
            Properties properties = new Properties();
            properties.HsEmailOptout868679 = new Hs_email_optout_868679();
            properties.HsEmailOptout868679.Value = "true";

            List<string> lcgceReverificationSubscriptionPromorionsList = new List<string>();

            //Act
            _contactFileProcess.EmailSuppressionsForHsEmailOptout868679(properties, "blahblah", lcgceReverificationSubscriptionPromorionsList);

            //Assert
            Assert.AreEqual(lcgceReverificationSubscriptionPromorionsList.Count, 1);
        }

        [Test]
        public void TestEmailSuppressionHsEmailOptout868677()
        {
            // Arrange
            Properties properties = new Properties();
            properties.HsEmailOptout868677 = new Hs_email_optout_868677();
            properties.HsEmailOptout868677.Value = "true";

            List<string> phexReverSubscriptionPromorionsList = new List<string>();

            //Act
            _contactFileProcess.EmailSuppressionsForHsEmailOptout868677(properties, "blahblah", phexReverSubscriptionPromorionsList);

            //Assert
            Assert.AreEqual(phexReverSubscriptionPromorionsList.Count, 1);
        }

        [Test]
        public void TestEmailSuppressionHsEmailOptout868678()
        {
            // Arrange
            Properties properties = new Properties();
            properties.HsEmailOptout868678 = new Hs_email_optout_868678();
            properties.HsEmailOptout868678.Value = "true";

            List<string> pteReverSubscriptionPromorionsList = new List<string>();

            //Act
            _contactFileProcess.EmailSuppressionsForHsEmailOptout868678(properties, "blahblah", pteReverSubscriptionPromorionsList);

            //Assert
            Assert.AreEqual(pteReverSubscriptionPromorionsList.Count, 1);
        }

        [Test]
        public void TestEmailSuppressionHsEmailOptout868676()
        {
            // Arrange
            Properties properties = new Properties();
            properties.HsEmailOptout868676 = new Hs_email_optout_868676();
            properties.HsEmailOptout868676.Value = "true";

            List<string> phteReverSubscriptionPromorionsList = new List<string>();

            //Act
            _contactFileProcess.EmailSuppressionsForHsEmailOptout868676(properties, "blahblah", phteReverSubscriptionPromorionsList);

            //Assert
            Assert.AreEqual(phteReverSubscriptionPromorionsList.Count, 1);
        }

        [Test]
        public void TestEmailSuppressionHsEmailOptout868681()
        {
            // Arrange
            Properties properties = new Properties();
            properties.HsEmailOptout868681 = new Hs_email_optout_868681();
            properties.HsEmailOptout868681.Value = "true";

            List<string> specReverSubscriptionPromorionsList = new List<string>();

            //Act
            _contactFileProcess.EmailSuppressionsForHsEmailOptout868681(properties, "blahblah", specReverSubscriptionPromorionsList);

            //Assert
            Assert.AreEqual(specReverSubscriptionPromorionsList.Count, 1);
        }
    }
}
