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
	public class ClickFileProcess
	{
		private readonly ILog _logger;
		private readonly Entity.Hubspot _hubspot;
		private readonly List<CampaignSubject> _campaignSubjects;
		private readonly DateTime _epoch;
		private readonly KMPSLogger _kmpsLogger;


		public ClickFileProcess(ILog logger, Entity.Hubspot hubspot, List<CampaignSubject> campaignSubjects, DateTime epoch, KMPSLogger kmpsLogger)
		{
			_logger = logger;
			_hubspot = hubspot;
			_campaignSubjects = campaignSubjects;
			_epoch = epoch;
			_kmpsLogger = kmpsLogger;
		}

		public string GetClickFileUrl(int campaignId, int appId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(Constants.ClickUrl + "{0}&campaignId={1}&appId={2}&eventType={3}", _hubspot.API, campaignId,
				appId, "click");
			return stringBuilder.ToString().Trim();
		}

		public List<Click> GetClickFileContent(string content, CampaignSubject campaignSubject)
		{
			List<Click> clickList = new List<Click>();
			try
			{
				JObject clickContentJson = JObject.Parse(content);

				var events = JsonConvert.DeserializeObject<List<Event>>(clickContentJson[Constants.EVENTS_KEY].ToString());

				foreach (var evnt in events)
				{
					Click click = new Click();
					if (evnt.Type == Constants.CLICK)
					{

						if (campaignSubject.Id == evnt.EmailCampaignId)
						{
							click.Pubcode = _hubspot.Pubcode;
							click.Subject = campaignSubject.Subject;
							click.EmailAddress = evnt.Recipient;
							click.ClickTime = _epoch.AddMilliseconds(evnt.Created);
							click.SendTime = _epoch.AddMilliseconds(evnt.SentBy.Created);
							click.ClickURL = evnt.Url;
							click.BlastId = evnt.EmailCampaignId;
						}
					}
					clickList.Add(click);
				}

				return clickList;
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message + " " + ex.StackTrace);
			}

			return clickList;
		}

		public string GetClickFileJsonString(CampaignSubject campaignSubject)
		{
			string clickFileUrl = GetClickFileUrl(campaignSubject.Id, campaignSubject.AppId);

			try
			{
				using (var syncClient = new WebClient())
				{
					_logger.Info("Download Subject File Json from address " + clickFileUrl);
					var content = syncClient.DownloadString(clickFileUrl);
					return content;
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message + " " + ex.StackTrace);
			}
			return String.Empty;
		}

		public List<Click> GetClickFilesForAllSubjects()
		{
			List<Click> clicks = new List<Click>();
			foreach (var campaignSubject in _campaignSubjects)
			{
				try
				{
					string content = GetClickFileJsonString(campaignSubject);
					clicks.AddRange(GetClickFileContent(content, campaignSubject));
				}
				catch (Exception ex)
				{
					_kmpsLogger.LogCustomerExeception(ex, "GetClicksForAllSubjects");
				}
			}
			_logger.Info("Write contact file to location " + _hubspot.Process[2].FtpFolderLocation + "with File Name" + GetFileNameClicks(_hubspot.ClientName));

			ExtractCSV.WriteCSV(clicks, _hubspot.Process[2].FtpFolderLocation + GetFileNameClicks(_hubspot.ClientName), _kmpsLogger, _logger);
			return clicks;
		}

		public string GetFileNameClicks(string clientName)
		{
			return "Clicks_" + clientName + DateTime.Now.ToString("mmddyyyy") + ".txt";
		}
	}
}