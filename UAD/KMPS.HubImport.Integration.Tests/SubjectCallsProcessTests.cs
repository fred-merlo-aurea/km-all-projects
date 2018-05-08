using System;
using System.Configuration;
using System.IO;
using System.Net;
using KMPS.HubImport.Integration.Process;
using KMPS.Hubspot.Integration;
using log4net;
using NUnit.Framework;

namespace KMPS.HubImport.Integration.Tests
{
	/// <summary>
	/// Summary description for SubjectCallsProcessTests
	/// </summary>
	[TestFixture]
	public class SubjectCallsProcessTests
	{
		private SubjectCallsProcess _subjectCallsProcess;

		private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

		private Entity.Hubspot _hubspot;

		private KMPSLogger _kmpsLogger;

		private StreamWriter mainlog;
		private StreamWriter customLog;

		[SetUp]
		public void Setup()
		{
			_hubspot = new Entity.Hubspot();
			_hubspot.API = "dda629b2-5bf6-43ce-bc2b-028a332ba6c7";

			_kmpsLogger = new KMPSLogger(mainlog, customLog, 50);

			_subjectCallsProcess = new SubjectCallsProcess(logger,_hubspot, _kmpsLogger);

		}

		[TearDown]
		public void TearDown()
		{
			
		}

		[Test]
		public void TestGetSubjectFileAddressBasedOnCampaignIdAndApplicationId()
		{
			//Arrange
			var appiId = 2268;

			var campaignId = 25137174;

			//Act

			string address = _subjectCallsProcess.GetSubjectFileAddressBasedOnCampaignIdAndApplicationId(campaignId,appiId);


			//Assert

			Assert.AreEqual(address, "https://api.hubapi.com/email/public/v1/campaigns/25137174?hapikey=dda629b2-5bf6-43ce-bc2b-028a332ba6c7&appId=2268");

		}

		[Test]
		public void TestGetSubjectsFromContent()
		{
			// Arrange
			var contentPath = ConfigurationManager.AppSettings["UbmLifeSciencesSubjectsJson"];
			var filePath = Constants.HubspotJsonFilesPath + contentPath;
			var content = String.Empty;

			Uri uri = new Uri(filePath);
			var client = new WebClient();
			content = client.DownloadString(uri);
			

			// Act
			var subjects = _subjectCallsProcess.GetSubjectsFromContent(content);

			// Assert
			Assert.IsNotNull(subjects);

		}
	}
}
