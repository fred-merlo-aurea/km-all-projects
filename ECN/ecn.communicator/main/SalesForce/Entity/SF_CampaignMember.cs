using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using ECN_Framework_Entities.Salesforce;
using ECN_Framework_Entities.Salesforce.Convertors;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ecn.communicator.main.Salesforce.Entity
{
    [Serializable]
    [DataContract]
    public class SF_CampaignMember : SalesForceBase
    {
        public SF_CampaignMember() { }
        #region Properties
        //[DataMember]
        public string CampaignId { get; set; }
        //[DataMember]
        public string Id { get; set; }
        //[DataMember]
        public string ContactId { get; set; }
        //[DataMember]
        public string CreatedById { get; set; }
        //[DataMember]
        public DateTime CreatedDate { get; set; }
        //[DataMember]
        public bool IsDeleted { get; set; }
        //[DataMember]
        public DateTime FirstRespondedDate { get; set; }
        //[DataMember]
        public string LastModifiedById { get; set; }
        //[DataMember]
        public DateTime LastModifiedDate { get; set; }
        //[DataMember]
        public string LeadId { get; set; }
        //[DataMember]
        public bool HasResponded { get; set; }
        [DataMember]
        public string Status { get; set; }
        //[DataMember]
        public DateTime SystemModstamp { get; set; }
        #endregion

        private static List<SF_CampaignMember> ConvertJsonList(List<string> json)
        {
            var converter = new CampaignMemberConverter();
            return converter.Convert<SF_CampaignMember>(json).ToList();
        }

        public static List<SF_CampaignMember> GetAll(string accessToken)
        {
            var query = SF_Utilities.SelectAllQuery(typeof(SF_CampaignMember));
            var request = SFUtilities.CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }

        public static List<SF_CampaignMember> GetList(string accessToken, string where)
        {
            var query = SF_Utilities.SelectWhere(typeof(SF_CampaignMember), where);
            var request = SFUtilities.CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }

        public static string GetXMLForUpdateJob(Dictionary<string, string> masterList)
        {
            StringBuilder returnXML = new StringBuilder();
            returnXML.Append("<sObjects xmlns=\"http://www.force.com/2009/06/asyncapi/dataload\">");
            foreach (KeyValuePair<string, string> sf in masterList)
            {
                returnXML.Append("<sObject>");
                returnXML.Append("<id>" + sf.Key + "</id>");
                returnXML.Append("<status>" + sf.Value + "</status>");
                returnXML.Append("</sObject>");
            }
            returnXML.Append("</sObjects>");
            return returnXML.ToString();
        }

    }
}