using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Web.UI.WebControls;
using ECN_Framework_Common.Interfaces;
using ECN_Framework_Entities.Salesforce;
using ECN_Framework_Entities.Salesforce.Sorting;
using ECN_Framework_Entities.Salesforce.Convertors;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ecn.communicator.main.Salesforce.Entity
{
    [Serializable]
    [DataContract]
    public class SF_Contact : SalesForceBase, ISFContact
    {
        #region Properties
        [XmlElement]
        public string Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime SystemModstamp { get; set; }
        public DateTime BirthDate { get; set; }
        public string MasterRecordId { get; set; }
        #endregion

        public SF_Contact()
        {
            Id = "null";
            IsDeleted = false;
            MasterRecordId = "null";
            SystemModstamp = DateTimeDefaultValue;
            BirthDate = DateTimeDefaultValue;
            base.Init();
        }

        private static List<SF_Contact> ConvertJsonList(List<string> json)
        {
            var converter = new ContactConverter();
            return converter.Convert<SF_Contact>(json).ToList();
        }

        public static List<SF_Contact> GetAll(string accessToken)
        {
            var query = SF_Utilities.SelectAllQuery(typeof(SF_Contact));
            var request = SFUtilities.CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }
        public static List<SF_Contact> GetList(string accessToken, string where)
        {
            var query = SF_Utilities.SelectWhere(typeof(SF_Contact), where);
            var request = SFUtilities.CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }

        public static SF_Contact GetSingle(string accessToken, string where)
        {
           return SFUtilities.GetSingle<SF_Contact>(accessToken, where, new ContactConverter()) ?? new SF_Contact();
        }

        public static List<SF_Contact> Sort(List<SF_Contact> list, string sortBy, SortDirection sortDir)
        {
            var isAscending = sortDir == SortDirection.Ascending;
            var utility = new EntitySort();
            utility.ExcludeFromSorting<SF_Contact, bool>(x => x.Master_Suppressed__c);

            return utility.Sort(list, sortBy, isAscending).ToList();
        }
        public static List<SF_Contact> GetListForMS(string accessToken, string searchFor)
        {
            var query = "SELECT Id, Email, Master_Suppressed__c FROM Contact " + searchFor;
            var request = SFUtilities.CreateQueryRequest(accessToken, query.Replace(" ", "%20"), Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }
        public static List<SF_Contact> GetCampaignMembers(string accessToken, string CampaignID)
        {
            var contactsList = new List<SF_Contact>();
            var query = "SELECT Id, IsDeleted, Email, Fax, FirstName, Name, Salutation, HomePhone, LastName, MailingCity, MailingState, MailingCountry, MailingPostalCode, MailingStreet, MobilePhone, Phone, Title, Master_Suppressed__c FROM Contact WHERE Id IN (SELECT ContactId FROM CampaignMember WHERE CampaignID = '" + CampaignID + "')";
            query = query.Replace(" ", "%20");
            var request = SFUtilities.CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }

        public static bool Insert(string accessToken, SF_Contact obj)
        {
            return SFUtilities.Insert(accessToken, obj, SFObject.Contact);
        }
        public static bool Update(string accessToken, SF_Contact obj)
        {
            return SFUtilities.Update(accessToken, obj, SFObject.Contact, obj.Id);
        }

        public static bool OptOut(string accessToken, string Id)
        {
            bool success = false;

            //to minimize failures because of incomplete data coming from ECN
            string json = "{\"HasOptedOutOfEmail\":true}";
            HttpWebRequest req = (HttpWebRequest)SF_Utilities.CreateUpdateRequest(accessToken, json, SF_Utilities.SFObject.Contact, Id, SF_Utilities.ResponseType.JSON);
            try
            {
                WebResponse resp = req.GetResponse();

                StreamReader sr = new StreamReader(resp.GetResponseStream());
                StringBuilder result = new StringBuilder();
                while (sr.EndOfStream == false)
                {
                    string x = sr.ReadLine();
                    result.Append(x);
                }
                resp.Close();
                if (result.Length == 0)
                    return true;
                else return false;
            }
            catch (WebException ex)
            {
                success = false;
                SF_Utilities.LogWebException(ex, json.ToString());
            }
            return success;
        }

        public static bool MasterSuppress(string accessToken, string Id)
        {

            bool success = false;

            //to minimize failures because of incomplete data coming from ECN
            string json = "{\"Master_Suppressed__c\":true}";
            HttpWebRequest req = (HttpWebRequest)SF_Utilities.CreateUpdateRequest(accessToken, json, SF_Utilities.SFObject.Contact, Id, SF_Utilities.ResponseType.JSON);
            try
            {
                WebResponse resp = req.GetResponse();

                StreamReader sr = new StreamReader(resp.GetResponseStream());
                StringBuilder result = new StringBuilder();
                while (sr.EndOfStream == false)
                {
                    string x = sr.ReadLine();
                    result.Append(x);
                }
                resp.Close();
                if (result.Length == 0)
                    return true;
                else return false;
            }
            catch (WebException ex)
            {
                success = false;
                SF_Utilities.LogWebException(ex, json.ToString());
            }
            return success;
        }

        public static string GetXMLForMSJob(List<SF_Contact> list)
        {
            StringBuilder sbXMl = new StringBuilder();
            sbXMl.Append("<sObjects xmlns=\"http://www.force.com/2009/06/asyncapi/dataload\">");
            foreach (SF_Contact sf in list)
            {
                sbXMl.Append("<sObject>");
                sbXMl.Append("<id>" + sf.Id + "</id>");
                sbXMl.Append("<master_suppressed__c>" + sf.Master_Suppressed__c.ToString() + "</master_suppressed__c>");
                sbXMl.Append("</sObject>");
            }
            sbXMl.Append("</sObjects>");

            return sbXMl.ToString();
        }

        public static string GetXMLForOptOutJob(Dictionary<string, string> dicContacts)
        {
            return SFUtilities.GetXmlForOutJob(dicContacts);
        }
    }
}