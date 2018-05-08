
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using KMPS.HubImport.Integration.Entity;
using KMPS.HubImport.Integration.Process;
using KMPS.Hubspot.Integration;
using log4net;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace KMPS.HubImport.Integration.Tests
{
	[TestFixture]
	public class OpenFileProcessTests
	{

		private static readonly ILog logger = LogManager.GetLogger(typeof(Program));
		private OpenFileProcess _openFileProcess;
		private  Entity.Hubspot _hubspot;
		private List<CampaignSubject> _campaignSubjects;
		private List<Open> _opens;

		private KMPSLogger _kmpsLogger;

		private StreamWriter mainlog;
		private StreamWriter customLog;
		[SetUp]
		public void Setup()
		{
			_hubspot = new Entity.Hubspot();
			_hubspot.API = "dda629b2-5bf6-43ce-bc2b-028a332ba6c7";

			_campaignSubjects = new List<CampaignSubject>
			{
				new CampaignSubject
				{
					Id = 2045185
				}
			};


			_kmpsLogger = new KMPSLogger(mainlog, customLog, 50);
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


			_openFileProcess = new OpenFileProcess(logger, _hubspot, _campaignSubjects, epoch, _kmpsLogger);
		}

		[TearDown]
		public void TearDown()
		{
			
		}

		[Test]
		public void TestGetOpenUrl()
		{
			// Arrange
			int campaignId = 25137174;
			int appId = 2268;

			// Act
			var address = _openFileProcess.GetOpenFileUrl(campaignId, appId);

			Assert.AreEqual(address,"https://api.hubapi.com/email/public/v1/events?hapikey=dda629b2-5bf6-43ce-bc2b-028a332ba6c7&campaignId=25137174&appId=2268&eventType=Open");
		}

		[Test]
		public void TestGetOpenFileContent()
		{
			// Arrange
			var contentPath = ConfigurationManager.AppSettings["UbmLifeSciencesOpenJson"];
			var filePath = Constants.HubspotJsonFilesPath + contentPath;
			var content = String.Empty;

			Uri uri = new Uri(filePath);
			var client = new WebClient();
			content = client.DownloadString(uri);

			CampaignSubject campaignSubject = new CampaignSubject
			{
				Id = 25137174
			};

			// Act
			var opens = _openFileProcess.GetOpenFileContent(content, campaignSubject);

			// Assert
			Assert.IsNotNull(opens);
		}


	}
}
