using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using ECN_Framework_Entities.Salesforce.Convertors;
using ECN_Framework_Entities.Salesforce;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ecn.communicator.main.Salesforce.Entity
{
    [Serializable]
    [DataContract]
    public class SF_LeadTag: SalesForceBase
    {
        public SF_LeadTag() { }

        [DataMember]
        public string ItemId { get; set; }
        [DataMember]
        public string Name { get; set; }

        public string TagDefinitionId { get; set; }
        [DataMember]
        public string Type { get; set; }

        private static List<SF_LeadTag> ConvertJsonList(List<string> json)
        {
            var converter = new TagConverter();
            return converter.Convert<SF_LeadTag>(json).ToList();
        }

        public static List<SF_LeadTag> GetByTagName(string accessToken, string tagName)
        {
            List<SF_LeadTag> retList = new List<SF_LeadTag>();
            string where = "WHERE Name = '" + tagName + "'";
            string query = SF_Utilities.SelectWhere(typeof(SF_LeadTag), where);

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
                retList = ConvertJsonList(jsonList);
            }
            catch (WebException ex)
            {
                SF_Utilities.LogWebException(ex, query.ToString());
            }


            return retList;
        }

        public static bool Insert(string accessToken, SF_LeadTag obj)
        {
            return SFUtilities.Insert(accessToken, obj, SFObject.LeadTag);
        }
    }
}