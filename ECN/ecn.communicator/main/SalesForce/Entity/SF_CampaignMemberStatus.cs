using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using ECN_Framework_Entities.Salesforce.Convertors;
using ECN_Framework_Entities.Salesforce;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ecn.communicator.main.Salesforce.Entity
{
    [Serializable]
    [DataContract]
    public class SF_CampaignMemberStatus: SalesForceBase
    {
        [DataMember]
        public string CampaignId { get; set; }
        public string Id { get; set; }
        public bool IsDeleted { get; set; }
        [DataMember]
        public bool IsDefault { get; set; }
        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public bool HasResponded { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        public DateTime SystemModstamp { get; set; }

        private static List<SF_CampaignMemberStatus> ConvertJsonList(List<string> json)
        {
            var converter = new CampaignMemberStatusConverter();
            return converter.Convert<SF_CampaignMemberStatus>(json).ToList();
        }

        public static List<SF_CampaignMemberStatus> GetAll(string accessToken)
        {
            List<SF_CampaignMemberStatus> list = new List<SF_CampaignMemberStatus>();
            string query = SF_Utilities.SelectAllQuery(typeof(SF_CampaignMemberStatus));
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

        public static List<SF_CampaignMemberStatus> GetList(string accessToken, string where)
        {
            List<SF_CampaignMemberStatus> list = new List<SF_CampaignMemberStatus>();
            string query = SF_Utilities.SelectWhere(typeof(SF_CampaignMemberStatus), where);
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

        public static bool Insert(string accessToken, SF_CampaignMemberStatus obj)
        {
            return SFUtilities.Insert(accessToken, obj, SFObject.CampaignMemberStatus);
        }
    }
}