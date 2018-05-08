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

namespace KMPS.HubImport.Integration.Tests
{
	[TestFixture]
	public class ClickFileProcessTests
	{
		private static readonly ILog logger = LogManager.GetLogger(typeof(Program));
		private ClickFileProcess _clickFileProcess;
		private Entity.Hubspot _hubspot;
		private List<CampaignSubject> _campaignSubjects;
		private List<Click> _clicks;

		private StreamWriter mainlog;
		private StreamWriter customLog;

		[SetUp]
		public void Setup()
		{
			_hubspot = new Entity.Hubspot();
			_hubspot.API = "dda629b2-5bf6-43ce-bc2b-028a332ba6c7";
			_hubspot.Pubcode = "HUBSPOT";



			KMPSLogger kmpsLogger = new KMPSLogger(mainlog, customLog, 50);

			_campaignSubjects = new List<CampaignSubject>
			{
				new CampaignSubject
				{
					Id = 2045185
				}
			};

			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			string fileName = "";

			_clickFileProcess = new ClickFileProcess(logger, _hubspot, _campaignSubjects, epoch, kmpsLogger);
		}

		[TearDown]
		public void TearDown()
		{
			
		}

		[Test]
		public void TestGetClickUrl()
		{
			// Arrange
			int campaignId = 25137174;
			int appId = 2268;

			// Act
			var address = _clickFileProcess.GetClickFileUrl(campaignId, appId);

			Assert.AreEqual(address, "https://api.hubapi.com/email/public/v1/events?hapikey=dda629b2-5bf6-43ce-bc2b-028a332ba6c7&campaignId=25137174&appId=2268&eventType=click");
		}

		[Test]
		public void TestGetClickFilesForAllSubjects()
		{
			// Arrange
			var contentPath = ConfigurationManager.AppSettings["UbmLifeSciencesClickJson"];
			var filePath = Constants.HubspotJsonFilesPath + contentPath;
			var content = String.Empty;

			Uri uri = new Uri(filePath);
			var client = new WebClient();
			content = client.DownloadString(uri);

			CampaignSubject campaignSubject = new CampaignSubject
			{
				Id = 25137174,
				Subject = "Great Resources on Lab Scale Chromatography"
			};

			// Act
			var clicks = _clickFileProcess.GetClickFileContent(content, campaignSubject);

			// Assert
			Assert.IsNotNull(clicks);
			Assert.AreEqual(10, clicks.Count);
			Assert.AreEqual("HUBSPOT", clicks[0].Pubcode);
			Assert.AreEqual("Great Resources on Lab Scale Chromatography", clicks[0].Subject);
			Assert.AreEqual("barczynskid@bioton.pl", clicks[0].EmailAddress);
			Assert.AreEqual("https://www.youtube.com/watch?v=M_a6HPMR5fs&index=2&list=PLrAEgIY86I6yv23Cnp7IwM03dvk-X_hwy&utm_campaign=BioRad&utm_source=hs_automation&utm_medium=email&utm_content=25137151&_hsmi=25137174", clicks[0].ClickURL);
			Assert.AreEqual(25137174, clicks[0].BlastId);
			
		}
	}
}
