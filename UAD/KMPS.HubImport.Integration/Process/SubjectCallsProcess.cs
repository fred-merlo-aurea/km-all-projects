using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using KMPS.HubImport.Integration.Entity;
using KMPS.Hubspot.Integration;
using log4net;
using Newtonsoft.Json;

namespace KMPS.HubImport.Integration.Process
{
	public class SubjectCallsProcess
	{
		private readonly ILog _logger;
		private readonly Entity.Hubspot _hubspot;

		public SubjectCallsProcess(ILog logger, Entity.Hubspot hubspot, KMPSLogger kmpsLogger)
		{
			_logger = logger;
			_hubspot = hubspot;
		}

		public List<CampaignSubject> GetSubjects(CampaignJsonObject campaignJsonObject)
		{
			List<CampaignSubject> campaignSubjects = new List<CampaignSubject>();

			foreach (var campaign in campaignJsonObject.Campaigns)
			{
				var subjectAddress = GetSubjectFileAddressBasedOnCampaignIdAndApplicationId(campaign.Id, campaign.AppId);

				try
				{
					using (var syncClient = new WebClient())
					{
						var content = GetSubjectJson(syncClient, subjectAddress);

						_logger.Info("Get Subjects from Json" +content);
						var subject = GetSubjectsFromContent(content);

						campaignSubjects.Add(subject);

					}
				}
				catch (Exception Ex)
				{
					_logger.Error(Ex.Message);
				}
				

			}
			return campaignSubjects;
		}

		public CampaignSubject GetSubjectsFromContent(string content)
		{
			_logger.Info("Deserialize Subjects");
			CampaignSubject campaignSubject = JsonConvert.DeserializeObject<CampaignSubject>(content);

			return campaignSubject;
		}

		public string GetSubjectJson(WebClient syncClient, string subjectUrl)
		{
			_logger.Info("Download Subject File Json from address " + subjectUrl);
			try
			{
				var content = syncClient.DownloadString(subjectUrl);
				return content;
			}
			catch (Exception ex)
			{
				_logger.Error("Unable to downloadstring as hubspot address is invalid " + ex.Message);
			}
			return String.Empty;
		}

		public string GetSubjectFileAddressBasedOnCampaignIdAndApplicationId(int campaignId, int appiId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(Constants.SubjectUrl + "{0}?hapikey={1}&appId={2}", campaignId, _hubspot.API, appiId);
			return stringBuilder.ToString();
		}
	}
}