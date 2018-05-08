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
    public class SF_User
    {
        public SF_User() { }

        #region Properties
        public string Id { get; set; }
        [DataMember]
        public string AboutMe { get; set; }
        [DataMember]
        public string Alias { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string CommunityNickName { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string Department { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string MobilePhone { get; set; }

        public string Name { get; set; }

        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Username { get; set; }

        #endregion

        #region Data
        private static List<SF_User> ConvertJsonList(List<string> json)
        {
            var converter = new UserConverter();
            return converter.Convert<SF_User>(json).ToList();
        }

        public static List<SF_User> GetAll(string accessToken)
        {
            List<SF_User> list = new List<SF_User>();
            string query = SF_Utilities.SelectAllQuery(typeof(SF_User));
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

        #endregion
    }
}