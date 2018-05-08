using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using KMPS.HubImport.Integration.Entity;
using KMPS.Hubspot.Integration;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KMPS.HubImport.Integration.Process
{
	public class CampaignIdProcess
	{
		private readonly ILog _logger;
		private readonly DateTime _epoch;
		private readonly Entity.Hubspot _hubspot;

		public CampaignIdProcess(ILog logger, Entity.Hubspot hubspot, DateTime epoch, KMPSLogger kmpsLogger)
		{
			_logger = logger;
			_epoch = epoch;
			_hubspot = hubspot;
		}


		public CampaignJsonObject GetCampaignIdsForLast24Hrs()
		{
			_logger.Info("Get Campaign Address ");
			string campaignRecentAddress = GetCampaignAddress(_hubspot);

			try
			{
				using (var syncClient = new WebClient())
				{

					string content = GetRecentCampaignIdJson(syncClient, campaignRecentAddress);
					CampaignJsonObject campaignJsonObject= GetCampaignIdJsonObject(content);
					campaignJsonObject = GetCampaignIdsRecursiveCall(campaignJsonObject);
					return campaignJsonObject;

				}
			}
			catch (Exception ex)
			{
				_logger.Error("There is problem with json please see stack trace for error" + ex.Message + ex.StackTrace);
			}
			return null;
		}

		public CampaignJsonObject GetCampaignIdsRecursiveCall(CampaignJsonObject campaignJsonObject)
		{
			var campaigns = campaignJsonObject.Campaigns.Where(x => x.LastUpdatedDateTime >= DateTime.Now.AddHours(-24));

			while(campaigns.Count() == campaignJsonObject.Campaigns.Count)
			{
				
				var campaignRecentAddress = GetCampignAddressWithOffset(campaignJsonObject.Offset);

				try
				{
					using (var syncClient = new WebClient())
					{
						string content = GetRecentCampaignIdJson(syncClient, campaignRecentAddress);
						CampaignJsonObject recursiveCampaignJsonObject = GetCampaignIdJsonObject(content);
						campaigns = recursiveCampaignJsonObject.Campaigns.Where(x=>x.LastUpdatedDateTime >= DateTime.Now.AddHours(-24));
						campaignJsonObject = recursiveCampaignJsonObject;
					}
				}
				catch (Exception ex)
				{
					_logger.Error("There is problem with json please see stack trace for error" + ex.Message + ex.StackTrace);
				}
			}
			campaignJsonObject.Campaigns = campaignJsonObject.Campaigns.Where(x => x.LastUpdatedDateTime >= DateTime.Now.AddHours(-24)).ToList();
			return campaignJsonObject;
		}

		private string GetCampignAddressWithOffset(string offset)
		{
			return Constants.RecentCampaignURL + _hubspot.API + "&offset=" + offset;
		}

		private string GetCampaignAddress(Entity.Hubspot hubspot)
		{
			return Constants.RecentCampaignURL + hubspot.API;
		}

		public string GetRecentCampaignIdJson(WebClient syncClient, string contactRecentAddress)
		{
			_logger.Info("Download Campaign File Json from address " + contactRecentAddress);
			try
			{
				var content = syncClient.DownloadString(contactRecentAddress);
				return content;
			}
			catch (Exception ex)
			{
				_logger.Error("Unable to downloadstring as hubspot address is invalid." + ex.Message);
			}
			return String.Empty;
		}

		public CampaignJsonObject GetCampaignIdJsonObject(string content)
		{
			CampaignJsonObject campaignJsonObject = new CampaignJsonObject();
			campaignJsonObject.Campaigns = new List<Campaign>();

			JObject campaignIdsJson = JObject.Parse(content);

			

			var deserializeObjectCampaigns = JsonConvert.DeserializeObject<List<Campaign>>(campaignIdsJson[Constants.CAMPAIGNS].ToString());

			campaignJsonObject.Offset = GetParseOffSet(campaignIdsJson);

			try
			{
				foreach (var campaign in deserializeObjectCampaigns)
				{
					campaign.LastUpdatedDateTime = campaign.GetDateTimeFromTimeStamp(_epoch, campaign.LastUpdatedTime);
					campaignJsonObject.Campaigns.Add(campaign);
				}
			}
			catch (Exception ex)
			{
				_logger.Error("There is problem for retrieving campaigns" + ex.Message + ex.StackTrace);
			}
			
			return campaignJsonObject;

		}

		private string GetParseOffSet(JObject campaignIdsJson)
		{
			return campaignIdsJson[Constants.CAMPAIGN_OFF_SET].ToString();
		}
	}
}