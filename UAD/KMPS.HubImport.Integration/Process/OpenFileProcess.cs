using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using KMPS.HubImport.Integration.Entity;
using KMPS.Hubspot.Integration;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KMPS.HubImport.Integration.Process
{
	public class OpenFileProcess
	{
		private readonly ILog _logger;
		private readonly Entity.Hubspot _hubspot;
		private readonly List<CampaignSubject> _campaignSubjects;
		private readonly DateTime _epoch;
		private readonly KMPSLogger _kmpsLogger;

		public OpenFileProcess(ILog logger, Entity.Hubspot hubspot, List<CampaignSubject> campaignSubjects, DateTime epoch, KMPSLogger kmpsLogger)
		{
			_logger = logger;
			_hubspot = hubspot;
			_campaignSubjects = campaignSubjects;
			_epoch = epoch;
			_kmpsLogger = kmpsLogger;
		}


		public string GetOpenFileUrl(int campaignId, int appId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(Constants.OpenUrl + "{0}&campaignId={1}&appId={2}&eventType={3}", _hubspot.API, campaignId,
				appId, "Open");
			return stringBuilder.ToString().Trim();
		}

		public string GetOpenFileJsonString(int id, int appId)
		{
			string openFileUrl = GetOpenFileUrl(id, appId);

			try
			{
				using (var syncClient = new WebClient())
				{
					_logger.Info("Download Subject File Json from address " + openFileUrl);
					var content = syncClient.DownloadString(openFileUrl);
					return content;
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message + " " + ex.StackTrace);
			}
			return String.Empty;
		}

		public List<Open> GetOpenFileContent(string content, CampaignSubject campaignSubject)
		{
			List<Open> openList = new List<Open>();
			try
			{
				JObject openContentJson = JObject.Parse(content);

				var events = JsonConvert.DeserializeObject<List<Event>>(openContentJson[Constants.EVENTS_KEY].ToString());

				foreach (var evnt in events)
				{
					Open open = new Open();
					if (evnt.Type == Constants.OPEN)
					{

						if (campaignSubject.Id == evnt.EmailCampaignId)
						{
							open.Pubcode = _hubspot.Pubcode;
							open.Subject = campaignSubject.Subject;
							open.EmailAddress = evnt.Recipient;
							open.OpenTime = _epoch.AddMilliseconds(evnt.Created);
							open.SendTime = _epoch.AddMilliseconds(evnt.SentBy.Created);
						}
					}
					openList.Add(open);
				}

				return openList;
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message + " " + ex.StackTrace);
			}

			return openList;
		}

		public string GetFileNameOpens(string clientName)
		{
			return "Opens_" + clientName + DateTime.Now.ToString("MM-dd-yyyy") + ".txt";
		}
		
		public List<Open> GetOpenFilesForAllSubjects()
		{

			List<Open> opens = new List<Open>();
			foreach (var campaignSubject in _campaignSubjects)
			{
				try
				{
					string content = GetOpenFileJsonString(campaignSubject.Id, campaignSubject.AppId);
					opens.AddRange(GetOpenFileContent(content, campaignSubject));
				}
				catch (Exception ex)
				{
					_kmpsLogger.LogCustomerExeception(ex, "GetOpenFilesForAllSubjects");
				}

			}
			_logger.Info("Write contact file to location " + _hubspot.Process[2].FtpFolderLocation + "with File Name" + GetFileNameOpens(_hubspot.ClientName));
			ExtractCSV.WriteCSV(opens, _hubspot.Process[2].FtpFolderLocation + GetFileNameOpens(_hubspot.ClientName), _kmpsLogger, _logger);
			return opens;
		}
	}
}