using System;
using System.Configuration;
using System.IO;
using System.Net;
using KMPS.HubImport.Integration.Process;
using KMPS.Hubspot.Integration;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace KMPS.HubImport.Integration.Tests
{
	[TestFixture]
	public class CampaignIdProcessTests
	{
		private CampaignIdProcess _campaignIdProcess;

		private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

		private  Entity.Hubspot _hubspot;

		private KMPSLogger _kmpsLogger;

		private StreamWriter mainlog;
		private StreamWriter customLog;

		[SetUp]
		public void Setup()
		{
			DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

			_hubspot = new Entity.Hubspot();
			_hubspot.API = "dda629b2-5bf6-43ce-bc2b-028a332ba6c7";

			_kmpsLogger = new KMPSLogger(mainlog, customLog, 50);
			
			_campaignIdProcess = new CampaignIdProcess(logger, _hubspot, epoch, _kmpsLogger);
		}

		[TearDown]
		public void TearDown()
		{
			
		}

		[Test]
		public void TestGetContentJson()
		{
			// Arrange
			var syncClient = new WebClient();
			var campaignRecentAddress = "https://api.hubapi.com/contacts/v1/lists/recently_updated/contacts/recent?hapikey=dda629b2-5bf6-43ce-bc2b-028a332ba6c7";


			// Act
			var campaigns = _campaignIdProcess.GetRecentCampaignIdJson(syncClient, campaignRecentAddress);


			// Assert
			Assert.IsNotNull(campaigns);
		}

		[Test]
		[ExpectedException(typeof(WebException))]
		public void TestFailGetContentJson()
		{
			//Arrange

			var syncClient = new WebClient();
			var campaignRecebtAddress = "https://api.hubapi.com/contacts/v1/lists/recently_updated/contacts/recent?hapikey=dda629b2-5bf6-43ce-bc2b-028a3";

			//Act
			var contacts = _campaignIdProcess.GetRecentCampaignIdJson(syncClient, campaignRecebtAddress);

			//Assert

		}


		[Test]
		public void TestGetCampaignIds()
		{
			// Arrange
			var contentPath = ConfigurationManager.AppSettings["UbmLifeSciencesCampiagnIdsJson"];
			var filePath = Constants.HubspotJsonFilesPath + contentPath;
			var content = String.Empty;

			Uri uri = new Uri(filePath);
			var client = new WebClient();
			content = client.DownloadString(uri);
			

			// Act
			var campaignJsonObject = _campaignIdProcess.GetCampaignIdJsonObject(content);


			// Assert
			Assert.IsNotNull(campaignJsonObject);
			Assert.IsNotNull(campaignJsonObject.Campaigns);
			Assert.AreEqual(campaignJsonObject.Campaigns.Count, 10);
		}


		[Test]
		[NUnit.Framework.Ignore("Currently this work for only certain campaign ids ..need to setupdata to make this tests pass...")]
		public void TestGetCampaignsRecursiveCall()
		{
			// Arrange
			var contentPath = ConfigurationManager.AppSettings["UbmLifeSciencesCampiagnIdsJson"];
			var filePath = @"C://" + contentPath;
			var content = String.Empty;

			Uri uri = new Uri(filePath);
			var client = new WebClient();
			content = client.DownloadString(uri);
			var campaignJsonObject = _campaignIdProcess.GetCampaignIdJsonObject(content);
			

			// Act

			var campaigns = _campaignIdProcess.GetCampaignIdsRecursiveCall(campaignJsonObject);


			// Assert
			Assert.IsNotNull(campaigns);
			Assert.AreEqual(campaigns.Campaigns.Count, 1);
		}

	}
}
