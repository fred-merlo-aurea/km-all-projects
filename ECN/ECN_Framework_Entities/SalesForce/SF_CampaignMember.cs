using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ECN_Framework_Entities.Salesforce.Convertors;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;


namespace ECN_Framework_Entities.Salesforce
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
        public string LeadId { get; set; }
        //[DataMember]
        public bool HasResponded { get; set; }
        [DataMember]
        public string Status { get; set; }
        //[DataMember]
        public DateTime SystemModstamp { get; set; }
        //[DataMember]
        public string LastModifiedById { get; set; }
        //[DataMember]
        public DateTime LastModifiedDate { get; set; }

        #endregion

        private static List<SF_CampaignMember> ConvertJsonList(List<string> json)
        {
            var converter = new CampaignMemberConverter();
            return converter.Convert<SF_CampaignMember>(json).ToList();
        }

        public static List<SF_CampaignMember> GetAll(string accessToken)
        {
            var query = SelectAllQuery(typeof(SF_CampaignMember));
            var request = SFUtilities.CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }

        public static List<SF_CampaignMember> GetList(string accessToken, string where)
        {
            var query = SelectWhere(typeof(SF_CampaignMember), where);
            var request = SFUtilities.CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }
    }
}