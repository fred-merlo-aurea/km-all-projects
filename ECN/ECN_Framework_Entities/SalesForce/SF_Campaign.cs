using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Runtime.Serialization;
using ECN_Framework_Common.Interfaces;
using ECN_Framework_Entities.Salesforce.Convertors;

namespace ECN_Framework_Entities.Salesforce
{
    [Serializable]
    [DataContract]
    public class SF_Campaign : ISFCampaign
    {
        public SF_Campaign() { }
        #region Properties
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public double ActualCost { get; set; }
        [DataMember]
        public double BudgetedCost { get; set; }
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string CampaignMemberRecordTypeId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string OwnerId { get; set; }
        [DataMember]
        public int NumberOfConvertedLeads { get; set; }
        [DataMember]
        public string CreatedById { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        public decimal ExpectedResponse { get; set; }
        [DataMember]
        public double ExpectedRevenue { get; set; }
        [DataMember]
        public DateTime LastActivityDate { get; set; }
        [DataMember]
        public string LastModifiedById { get; set; }
        [DataMember]
        public DateTime LastModifiedDate { get; set; }
        [DataMember]
        public DateTime LastReferencedDate { get; set; }
        [DataMember]
        public DateTime LastViewedDate { get; set; }
        [DataMember]
        public double NumberSent { get; set; }
        [DataMember]
        public int NumberOfOpportunities { get; set; }
        [DataMember]
        public int NumberOfWonOpportunities { get; set; }
        [DataMember]
        public string ParentId { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public DateTime SystemModstamp { get; set; }
        [DataMember]
        public int NumberOfContacts { get; set; }
        [DataMember]
        public int NumberOfLeads { get; set; }
        [DataMember]
        public int NumberOfResponses { get; set; }
        [DataMember]
        public double AmountAllOpportunities { get; set; }
        [DataMember]
        public double AmountWonOpportunities { get; set; }
        [DataMember]
        public string Type { get; set; }
        #endregion

        private static List<SF_Campaign> ConvertJsonList(List<string> json)
        {
            var converter = new CampaignConverter();
            return converter.Convert<SF_Campaign>(json).ToList();
        }

        public static List<SF_Campaign> GetList(string accessToken, string where)
        {
            List<SF_Campaign> list = new List<SF_Campaign>();
            string query = SF_Utilities.SelectWhere(typeof(SF_Campaign), where);
            WebRequest req = SF_Utilities.CreateQueryRequest(accessToken, query, SF_Utilities.Method.GET, SF_Utilities.ResponseType.JSON);
            try
            {
                WebResponse resp = req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                List<string> jsonList = new List<string>();
                while (sr.EndOfStream == false)
                {
                    string x = sr.ReadLine();
                    jsonList.Add(x.Trim());
                }
                resp.Close();
                list = ConvertJsonList(jsonList);
            }
            catch (WebException ex)
            {
                SF_Utilities.LogWebException(ex, query.ToString());
            }
            return list;
        }
    }
}