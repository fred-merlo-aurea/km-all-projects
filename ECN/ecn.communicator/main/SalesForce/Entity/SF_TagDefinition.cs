using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using ECN_Framework_Entities.Salesforce.Convertors;

namespace ecn.communicator.main.Salesforce.Entity
{
    [Serializable]
    [DataContract]
    public class SF_TagDefinition
    {
        public SF_TagDefinition() { }

        #region Properties
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Type { get; set; }
        #endregion

        #region Data
        private static List<SF_TagDefinition> ConvertJsonList(List<string> jsonList)
        {
            var converter = new TagConverter();
            return converter.Convert<SF_TagDefinition>(jsonList).ToList();
        }

        public static List<SF_TagDefinition> GetAll(string accessToken)
        {
            List<SF_TagDefinition> retList = new List<SF_TagDefinition>();
            string query = SF_Utilities.SelectAllQuery(typeof(SF_TagDefinition));
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

        #endregion
    }
}