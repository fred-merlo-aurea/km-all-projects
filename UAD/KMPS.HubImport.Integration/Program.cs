using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KMPS.HubImport.Integration.Entity;
using KMPS.HubImport.Integration.Process;
using log4net;
using Newtonsoft.Json;

namespace KMPS.Hubspot.Integration
{
	public class Program
	{
		private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

		StreamWriter customerLog;
		StreamWriter mainLog;

		private readonly KMPSLogger _kmpsLogger;

		
		public static void Main(string[] args)
		{

			log4net.Config.XmlConfigurator.Configure();
			Program program = new Program();
			program.start();

		}

		private void start()
		{
			int applicationId = int.Parse(ConfigurationManager.AppSettings["KMCommon_Application"]);
		

			mainLog = new StreamWriter(ConfigurationManager.AppSettings["MainLog"] + Constants.KmpsHubimportintegrationAppName + "_" + DateTime.Now.ToString("MM-dd-yyyy") + ".log", true);
			customerLog = new StreamWriter(ConfigurationManager.AppSettings["CustomLog"] + "_" + DateTime.Now.ToString("MM-dd-yyyy") + ".log", true);

			KMPSLogger kmpsLogger = new KMPSLogger(mainLog, customerLog, applicationId);

			kmpsLogger.MainLogWrite("Start Instance");
			var filePath = ConfigurationManager.AppSettings["HubspotConfiguration"];
			// Get hubspot configuration json

			var hubspot = GetHubSpotConfiguration(filePath);
			string fileName = GetFileName(hubspot.ClientName);


			kmpsLogger.CustomerLogWrite("Get the epoch time for utc conversion.");
			logger.Info("Get the epoch time for utc conversion.");
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


			kmpsLogger.MainLogWrite("Start Contact File Process");
			logger.Info("Start Contact File Process");
			ContactFileProcess contactFileProcess = new ContactFileProcess(logger, kmpsLogger);

			contactFileProcess.GetContactFile(hubspot, epoch, fileName);

			kmpsLogger.MainLogWrite("Start Campaigns  Process");
			logger.Info("Start Campaigns  Process");
			CampaignIdProcess campaignIdProcess = new CampaignIdProcess(logger, hubspot, epoch, kmpsLogger);

			CampaignJsonObject campaignJsonObject = campaignIdProcess.GetCampaignIdsForLast24Hrs();

			kmpsLogger.MainLogWrite("Start Subjects  Process");
			logger.Info("Start Subjects  Process");
			SubjectCallsProcess subjectCallsProcess = new SubjectCallsProcess(logger, hubspot, kmpsLogger);

			List<CampaignSubject> campaignSubjects = subjectCallsProcess.GetSubjects(campaignJsonObject);

			GetOpenFile(campaignSubjects, hubspot, epoch, kmpsLogger);

			GetClickFile(campaignSubjects, hubspot, epoch, kmpsLogger);
		}

		private static void GetClickFile(List<CampaignSubject> campaignSubjects, HubImport.Integration.Entity.Hubspot hubspot, DateTime epoch, KMPSLogger kmpsLogger)
		{
			logger.Info("Start ClickFile  Process");
			kmpsLogger.MainLogWrite("Start ClickFile  Process");
			ClickFileProcess clickFileProcess = new ClickFileProcess(logger, hubspot, campaignSubjects, epoch, kmpsLogger);
			clickFileProcess.GetClickFilesForAllSubjects();
		}

		public static void GetOpenFile(List<CampaignSubject> campaignSubjects, HubImport.Integration.Entity.Hubspot hubspot, DateTime epoch, KMPSLogger kmpsLogger)
		{
			logger.Info("Start OpenFile  Process");
			kmpsLogger.MainLogWrite("Start OpenFile  Process");
			OpenFileProcess openFileProcess = new OpenFileProcess(logger, hubspot, campaignSubjects, epoch, kmpsLogger);
			openFileProcess.GetOpenFilesForAllSubjects();
		}


		public  static HubImport.Integration.Entity.Hubspot GetHubSpotConfiguration(string filePath)
		{
			logger.Info("Get configuration file hubspot_integration.json.");
			HubImport.Integration.Entity.Hubspot hubspot = new HubImport.Integration.Entity.Hubspot();

			try
			{
				using (StreamReader file = File.OpenText(filePath))
				{
					JsonSerializer serializer = new JsonSerializer();
					hubspot = (HubImport.Integration.Entity.Hubspot)serializer.Deserialize(file, typeof(HubImport.Integration.Entity.Hubspot));
					
			
				}
			}
			catch (Exception ex)
			{
				logger.Error("There is an exception occured. Please see exception trace and resolve this" + ex.StackTrace);
			}
			
			return hubspot;
		}

		private static string GetFileName(string clientName)
		{
			return "HubSpot_" + clientName + DateTime.Now.ToString("MM-dd-yyyy") + ".txt";
		}
	}
}
