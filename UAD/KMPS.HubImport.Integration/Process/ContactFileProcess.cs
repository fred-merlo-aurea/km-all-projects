using System;
using System.Collections.Generic;
using System.Net;
using KMPS.HubImport.Integration.Entity;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KMPS.Hubspot.Integration
{
	public class ContactFileProcess
	{
		private readonly ILog _logger;
		private readonly KMPSLogger _kmpsLogger;
		private readonly List<string> _brandMarketingEmailSuppressions = new List<string>();
		private readonly List<string> _webinarsEmailSuppressions = new List<string>();


		private readonly List<string> _biopReverSubscriptionPromorionsList = new List<string>();
		private readonly List<string> _lceReverSubscriptionPromorionsList = new List<string>();
		private readonly List<string> _lcgceReverificationSubscriptionPromorionsList = new List<string>();
		private readonly List<string> _phexReverSubscriptionPromorionsList = new List<string>();
		private readonly List<string> _pteReverSubscriptionPromorionsList = new List<string>();
		private readonly List<string> _phteReverSubscriptionPromorionsList = new List<string>();
		private readonly List<string> _specReverSubscriptionPromorionsList= new List<string>();

        private readonly KMPlatform.Entity.User _user = KMPlatform.BusinessLogic.User.ECN_GetByAccessKey(System.Configuration.ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), true);
        //private readonly KMPlatform.Entity.User _user = KMPlatform.BusinessLogic.User.GetByAccessKey(System.Configuration.ConfigurationManager.AppSettings["ECNEngineAccessKey"].ToString(), false);

        public Dictionary<int, List<string>> BrandMarketingEmailSuppressionsDictionary = new Dictionary<int, List<string>>();
		public Dictionary<int, List<string>> WebinarsEmailSuppressionsDictionary = new Dictionary<int, List<string>>();
		public Dictionary<int, List<string>> BiopReverSubscriptionPromorionsListDictionary = new Dictionary<int, List<string>>();
		public Dictionary<int, List<string>> LceReverSubscriptionPromorionsListDictionary = new Dictionary<int, List<string>>();
		public Dictionary<int, List<string>> LcgceReverificationSubscriptionPromorionsListDictionary = new Dictionary<int, List<string>>();
		public Dictionary<int, List<string>> PhexReverSubscriptionPromorionsListDictionary = new Dictionary<int, List<string>>();
		public Dictionary<int, List<string>> PteReverSubscriptionPromorionsListDictionary = new Dictionary<int, List<string>>();
		public Dictionary<int, List<string>> PhteReverSubscriptionPromorionsListDictionary = new Dictionary<int, List<string>>();
		public Dictionary<int, List<string>> SpecReverSubscriptionPromorionsListDictionary = new Dictionary<int, List<string>>();

		public ContactFileProcess(ILog logger, KMPSLogger kmpsLogger)
		{
			_logger = logger;
			_kmpsLogger = kmpsLogger;
		}


        public void GetContactFile(HubImport.Integration.Entity.Hubspot hubspot, DateTime epoch, string fileName)
        {
            long timeoffset, vidoffset;
            List<Contact> contactCollection = new List<Contact>();


            var contactRecentAddress = GetContactRecentAddress(hubspot);

            try
            {
                using (var syncClient = new WebClient())
                {
                    var content = GetContactJson(syncClient, contactRecentAddress);
                    contactCollection = GetContent(content, hubspot.Pubcode, out timeoffset, out vidoffset);

                    var day = epoch.AddMilliseconds(timeoffset).Date;

                    _kmpsLogger.CustomerLogWrite("Validate time offset from url is for last 24 hours");
                    _logger.Info("Validate time offset from url is for last 24 hours");

                    while (day >= DateTime.Now.AddHours(-24).Date)
                    //while (day >= DateTime.Now.AddDays(-30).Date)//wgh - remove before live
                    {
                        var contentpreviousTimeStamp =
                            syncClient.DownloadString(contactRecentAddress + "&property=lastname&property=firstname&property=hs_email_optout&property=hs_email_optout_409865&property=hs_email_optout_868669&property=hs_email_optout_868671&property=hs_email_optout_868680&property=hs_email_optout_868679&property=hs_email_optout_868677&property=hs_email_optout_868678&property=hs_email_optout_868676&property=hs_email_optout_868681&timeOffSet=" + timeoffset + "&vidOffset=" + vidoffset);
                        contactCollection.AddRange(GetContent(contentpreviousTimeStamp, hubspot.Pubcode, out timeoffset, out vidoffset));
                        day = epoch.AddMilliseconds(timeoffset).Date;
                    }

                    BiopReverSubscriptionPromorionsListDictionary.Add(139578, _biopReverSubscriptionPromorionsList);
                    SendUnsubToECN(_biopReverSubscriptionPromorionsList, 3384, 139578);//writes unsubscribe to group in Email Marketing

                    LceReverSubscriptionPromorionsListDictionary.Add(139569, _lceReverSubscriptionPromorionsList);
                    SendUnsubToECN(_lceReverSubscriptionPromorionsList, 3384, 139569);//writes unsubscribe to group in Email Marketing

                    LcgceReverificationSubscriptionPromorionsListDictionary.Add(139572, _lcgceReverificationSubscriptionPromorionsList);
                    SendUnsubToECN(_lcgceReverificationSubscriptionPromorionsList, 3384, 139572);//writes unsubscribe to group in Email Marketing

                    PhexReverSubscriptionPromorionsListDictionary.Add(139571, _phexReverSubscriptionPromorionsList);
                    SendUnsubToECN(_phexReverSubscriptionPromorionsList, 3384, 139571);//writes unsubscribe to group in Email Marketing

                    PteReverSubscriptionPromorionsListDictionary.Add(139573, _pteReverSubscriptionPromorionsList);
                    SendUnsubToECN(_pteReverSubscriptionPromorionsList, 3384, 139573);//writes unsubscribe to group in Email Marketing

                    PhteReverSubscriptionPromorionsListDictionary.Add(138748, _phteReverSubscriptionPromorionsList);
                    SendUnsubToECN(_phteReverSubscriptionPromorionsList, 3384, 138748);//writes unsubscribe to group in Email Marketing

                    SpecReverSubscriptionPromorionsListDictionary.Add(138491, _specReverSubscriptionPromorionsList);
                    SendUnsubToECN(_specReverSubscriptionPromorionsList, 3384, 138491);//writes unsubscribe to group in Email Marketing

                    //if already unsubscribed and we get a ms we don't update to ms...that is why we do ms after unsubscribe
                    BrandMarketingEmailSuppressionsDictionary.Add(3383, _brandMarketingEmailSuppressions);
                    SendMSToECN(_brandMarketingEmailSuppressions, 3383, 105782);//writes to master suppression in Email Marketing

                    WebinarsEmailSuppressionsDictionary.Add(3573, _webinarsEmailSuppressions);
                    SendMSToECN(_webinarsEmailSuppressions, 3573, 162127);//writes to master suppression in Email Marketing

                    _kmpsLogger.CustomerLogWrite("Write contact file to location " + hubspot.Process[0].FtpFolderLocation + "with File Name" + fileName);
                    _logger.Info("Write contact file to location " + hubspot.Process[0].FtpFolderLocation + "with File Name" + fileName);

                    ExtractCSV.WriteCSV(contactCollection, hubspot.Process[0].FtpFolderLocation + fileName, _kmpsLogger, _logger);
                }
            }
            catch (Exception ex)
            {
                _kmpsLogger.LogMainExeception(ex, "Main.Step: ");
                _logger.Error("There is error in file " + ex.Message + ex.StackTrace);
            }

            _kmpsLogger.MainLogWrite("End Contact Process");
        }

        //writes to master suppression in Email Marketing
        private void SendMSToECN(List<string> emailList, int custID, int groupID)
        {
            if (emailList.Count > 0)
            {
                string xmlUDF = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>";
                System.Text.StringBuilder xmlInsert = new System.Text.StringBuilder();

                xmlInsert.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
                foreach (string contact in emailList)
                {
                    xmlInsert.Append("<Emails><emailaddress>" + contact + "</emailaddress></Emails>");
                }

                ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(_user, custID, groupID, xmlInsert.ToString() + "</XML>", xmlUDF, "HTML", "M", true, "", "KMPS.HubImport.Integration.SendMSToECN");
            }

            _kmpsLogger.CustomerLogWrite("Sent " + emailList.Count.ToString() + " emails to group" + groupID.ToString());
            _logger.Info("Sent " + emailList.Count.ToString() + " emails to group" + groupID.ToString());
        }

        //writes unsubscribe to group in Email Marketing
        private void SendUnsubToECN(List<string> emailList, int custID, int groupID)
        {
            if (emailList.Count > 0)
            {
                string xmlUDF = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>";
                System.Text.StringBuilder xmlInsert = new System.Text.StringBuilder();

                xmlInsert.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
                foreach (string contact in emailList)
                {
                    xmlInsert.Append("<Emails><emailaddress>" + contact + "</emailaddress><subscribetypecode>U</subscribetypecode></Emails>");
                }

                ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails(_user, custID, groupID, xmlInsert.ToString() + "</XML>", xmlUDF, "HTML", "U", true, "", "KMPS.HubImport.Integration.SendUnsubToECN");
            }
            _kmpsLogger.CustomerLogWrite("Sent " + emailList.Count.ToString() + " emails to group" + groupID.ToString());
            _logger.Info("Sent " + emailList.Count.ToString() + " emails to group" + groupID.ToString());
        }

        public string GetContactJson(WebClient syncClient, string contactRecentAddress)
		{
			_logger.Info("Download Contact File Json from address " +contactRecentAddress);
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

		public string GetContactRecentAddress(HubImport.Integration.Entity.Hubspot hubspot)
		{
			return Constants.RecentContactURL + hubspot.API;
		}

		public  List<Contact> GetContent(string content, string pubCode, out long timeoffset, out long vidoffset)
		{
			_logger.Info("Get Contact Collection");
			List<Contact> contactCollection = new List<Contact>();
			JObject contactJson = JObject.Parse(content);
			var contacts = contactJson[Constants.CONTACTS_KEY];
			long parseVidOffset = 0;
			long parseTimeOffset = 0;

			// No need to test these two as they are coming from hubspot webapi

			parseVidOffset = GetParseVidOffSet(contactJson, parseVidOffset);

			parseTimeOffset = GetParseTimeOffSet(contactJson, parseTimeOffset);

			try
			{
				foreach (var contact in contacts)
				{
					var item = new Contact();

					item.PubCode = pubCode;

					JObject deserializeProperties = (JObject)JsonConvert.DeserializeObject(contact[Constants.PROPERTIES_KEY].ToString());
					Properties properties = deserializeProperties.ToObject<Properties>();                    


					JArray deserializeIdentityProfiles = (JArray)JsonConvert.DeserializeObject(contact[Constants.IDENTITY_PROFILES_KEY].ToString());
					List<IdentityProfile> identityProfiles = deserializeIdentityProfiles.ToObject<List<IdentityProfile>>();

					if (properties == null && identityProfiles == null)
					{
						continue;
					}

					if (properties != null)
					{
                        if(properties.FirstName != null)
                            item.FirstName = properties.FirstName.Value;
                        if(properties.LastName != null)
                            item.LastName = properties.LastName.Value;
                    }

					if (identityProfiles != null && identityProfiles.Count > 0)
					{
						if (identityProfiles[0].Identities != null && identityProfiles[0].Identities.Count > 0)
						{
                            //we are doing this as we don't know where the email identity will be
                            for (int i = 0; i < identityProfiles[0].Identities.Count; i++)
                            {
                                var identity = identityProfiles[0].Identities[i];
                                if (identity.Type == "EMAIL")
                                {
                                    item.Email = identity.Value;
                                    break;
                                }
                            }
                        }
					}
					
					EmailSuppressionsForHsEmailOptout(properties, item.Email, _brandMarketingEmailSuppressions, _webinarsEmailSuppressions);

					EmailSuppressionsForHsEmailOptout409865(properties, item.Email, _brandMarketingEmailSuppressions);

					EmailSuppressionsForHsEmailOptout868669(properties, item.Email, _webinarsEmailSuppressions);

					EmailSuppressionsForHsEmailOptout868671(properties, item.Email, _biopReverSubscriptionPromorionsList);

					EmailSuppressionsForHsEmailOptout868680(properties, item.Email, _lceReverSubscriptionPromorionsList);

					EmailSuppressionsForHsEmailOptout868679(properties, item.Email, _lcgceReverificationSubscriptionPromorionsList);

					EmailSuppressionsForHsEmailOptout868677(properties, item.Email, _phexReverSubscriptionPromorionsList);

					EmailSuppressionsForHsEmailOptout868678(properties, item.Email, _pteReverSubscriptionPromorionsList);

					EmailSuppressionsForHsEmailOptout868676(properties, item.Email, _phteReverSubscriptionPromorionsList);

					EmailSuppressionsForHsEmailOptout868681(properties, item.Email, _specReverSubscriptionPromorionsList);

                    contactCollection.Add(item);
				}

			}
			catch (Exception ex)
			{
				_logger.Error("There is problem with json please see stack trace for error" + ex.Message + ex.StackTrace);
			}
			timeoffset = parseTimeOffset;
			vidoffset = parseVidOffset;
			return contactCollection;

		}

        public void EmailSuppressionsForHsEmailOptout868669(Properties properties, string emailAddress, List<string> webinarsEmailSuppressions)
        {
            if (properties != null && emailAddress != string.Empty)
            {
                if (properties.HsEmailOptout868669 != null)
                {
                    if (properties.HsEmailOptout868669.Value == "true")
                    {
                        _logger.Info("Get Email Suppression for HsEmailOptout868669");
                        if(!webinarsEmailSuppressions.Contains(emailAddress))
                            webinarsEmailSuppressions.Add(emailAddress);
                    }
                }
            }
        }

        public void EmailSuppressionsForHsEmailOptout868671(Properties properties, string emailAddress, List<string> biopReverSubscriptionPromorionsList)
        {
            if (properties != null && emailAddress != string.Empty)
            {
                if (properties.HsEmailOptout868671 != null)
                {
                    if (properties.HsEmailOptout868671.Value == "true")
                    {
                        _logger.Info("Get Email Suppression for HsEmailOptout868671");
                        if(!biopReverSubscriptionPromorionsList.Contains(emailAddress))
                            biopReverSubscriptionPromorionsList.Add(emailAddress);
                    }
                }
            }
        }

        public void EmailSuppressionsForHsEmailOptout868680(Properties properties, string emailAddress, List<string> lceReverSubscriptionPromorionsList)
        {
            if (properties != null && emailAddress != string.Empty)
            {
                if (properties.HsEmailOptout868680 != null)
                {
                    if (properties.HsEmailOptout868680.Value == "true")
                    {
                        _logger.Info("Get Email Suppression for HsEmailOptout868680");
                        if(!lceReverSubscriptionPromorionsList.Contains(emailAddress))
                            lceReverSubscriptionPromorionsList.Add(emailAddress);
                    }
                }
            }
        }

        public void EmailSuppressionsForHsEmailOptout409865(Properties properties, string emailAddress, List<string> brandMarketingEmailSuppressions)
        {
            if (properties != null && emailAddress != string.Empty)
            {
                if (properties.HsEmailOptout409865 != null)
                {
                    if (properties.HsEmailOptout409865.Value == "true")
                    {
                        _logger.Info("Get Email Suppression for HsEmailOptout409865");
                        if(!brandMarketingEmailSuppressions.Contains(emailAddress))
                            brandMarketingEmailSuppressions.Add(emailAddress);
                    }
                }
            }
        }

        public void EmailSuppressionsForHsEmailOptout(Properties properties, string emailAddress, List<string> brandMarketingEmailSuppressions, List<string> webinarsEmailSuppressions)
        {
            if (properties != null && emailAddress != string.Empty)
            {
                if (properties.HsEmailOptout != null)
                {
                    if (properties.HsEmailOptout.Value == "true")
                    {
                        _logger.Info("Get Email Suppression for HsEmailOptout");
                        if(!brandMarketingEmailSuppressions.Contains(emailAddress))
                            brandMarketingEmailSuppressions.Add(emailAddress);
                        if(!webinarsEmailSuppressions.Contains(emailAddress))
                            webinarsEmailSuppressions.Add(emailAddress);
                    }
                }
            }
        }

        public void EmailSuppressionsForHsEmailOptout868679(Properties properties, string emailAddress, List<string> lcgceReverificationSubscriptionPromorionsList)
        {
            if (properties != null && emailAddress != string.Empty)
            {
                if (properties.HsEmailOptout868679 != null)
                {
                    if (properties.HsEmailOptout868679.Value == "true")
                    {
                        _logger.Info("Get Email Suppression for HsEmailOptout868679");
                        if(!lcgceReverificationSubscriptionPromorionsList.Contains(emailAddress))
                            lcgceReverificationSubscriptionPromorionsList.Add(emailAddress);
                    }
                }
            }
        }

        public void EmailSuppressionsForHsEmailOptout868677(Properties properties, string emailAddress, List<string> phexReverSubscriptionPromorionsList)
        {
            if (properties != null && emailAddress != string.Empty)
            {
                if (properties.HsEmailOptout868677 != null)
                {
                    if (properties.HsEmailOptout868677.Value == "true")
                    {
                        _logger.Info("Get Email Suppression for HsEmailOptout868677");
                        if(!phexReverSubscriptionPromorionsList.Contains(emailAddress))
                            phexReverSubscriptionPromorionsList.Add(emailAddress);
                    }
                }
            }
        }

        public void EmailSuppressionsForHsEmailOptout868678(Properties properties, string emailAddress, List<string> pteReverSubscriptionPromorionsList)
        {
            if (properties != null && emailAddress != string.Empty)
            {
                if (properties.HsEmailOptout868678 != null)
                {
                    if (properties.HsEmailOptout868678.Value == "true")
                    {
                        _logger.Info("Get Email Suppression for HsEmailOptout868678");
                        if(!pteReverSubscriptionPromorionsList.Contains(emailAddress))
                            pteReverSubscriptionPromorionsList.Add(emailAddress);
                    }
                }
            }
        }

        public void EmailSuppressionsForHsEmailOptout868676(Properties properties, string emailAddress, List<string> phteReverSubscriptionPromorionsList)
        {
            if (properties != null && emailAddress != string.Empty)
            {
                if (properties.HsEmailOptout868676 != null)
                {
                    if (properties.HsEmailOptout868676.Value == "true")
                    {
                        _logger.Info("Get Email Suppression for HsEmailOptout868676");
                        if(!phteReverSubscriptionPromorionsList.Contains(emailAddress))
                            phteReverSubscriptionPromorionsList.Add(emailAddress);
                    }
                }
            }
        }

        public void EmailSuppressionsForHsEmailOptout868681(Properties properties, string emailAddress, List<string> specReverSubscriptionPromorionsList)
        {
            if (properties != null && emailAddress != string.Empty)
            {
                if (properties.HsEmailOptout868681 != null)
                {
                    if (properties.HsEmailOptout868681.Value == "true")
                    {
                        _logger.Info("Get Email Suppression for HsEmailOptout868681");
                        if(!specReverSubscriptionPromorionsList.Contains(emailAddress))
                            specReverSubscriptionPromorionsList.Add(emailAddress);
                    }
                }
            }
        }

        private long GetParseTimeOffSet(JObject contactJson, long parseTimeOffset)
		{
			_logger.Info("Parse time off is " +contactJson[Constants.TIME_OFFSET_KEY].ToString());
			try
			{
				if (Int64.TryParse(contactJson[Constants.TIME_OFFSET_KEY].ToString(), out parseTimeOffset))
				{
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message + " " + ex.StackTrace);
			}
			return parseTimeOffset;
		}

		private long GetParseVidOffSet(JObject contactJson, long parseVidOffset)
		{
			_logger.Info("Vid_offset_key is " + contactJson[Constants.VID_OFFSET_KEY].ToString());
			try
			{
				if (Int64.TryParse(contactJson[Constants.VID_OFFSET_KEY].ToString(), out parseVidOffset))
				{
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message + " " + ex.StackTrace);
			}
			return parseVidOffset;
		}
	}


}