using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using ECN_Framework_Entities.Salesforce.Convertors;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ECN_Framework_Entities.Salesforce
{
    [Serializable]
    [DataContract]
    public class SF_Lead : LeadBase
    {
        private static List<SF_Lead> ConvertJsonList(List<string> json)
        {
            var converter = new LeadConverter();
            return converter.Convert<SF_Lead>(json).ToList();
        }

        public static List<SF_Lead> GetAll(string accessToken)
        {
            var query = SelectAllQuery(typeof(SF_Lead));
            WebRequest request = SFUtilities.CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }

        public static List<SF_Lead> GetList(string accessToken, string where)
        {
            var query = SelectWhere(typeof(SF_Lead), where);
            var request = SFUtilities.CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }

        public static List<SF_Lead> GetCampaignMembers(string accessToken, string CampaignID)
        {
            var query = "SELECT Id, FirstName, LastName, Name, Company, Street, City, State, PostalCode, Country, Email, Fax, MobilePhone, Phone, EmailBouncedDate FROM Lead WHERE Id in (SELECT LeadId FROM CampaignMember WHERE CampaignId = '" + CampaignID + "')";
            var request = SFUtilities.CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }
    }
}