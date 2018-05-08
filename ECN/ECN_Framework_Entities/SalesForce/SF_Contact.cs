
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using ECN_Framework_Common.Interfaces;
using ECN_Framework_Entities.Salesforce.Convertors;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ECN_Framework_Entities.Salesforce
{
    [Serializable]
    [DataContract]
    public class SF_Contact : SalesForceBase, ISFContact
    {
        [XmlElement]
        public string Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime SystemModstamp { get; set; }
        public DateTime BirthDate { get; set; }
        public string MasterRecordId { get; set; }

        public SF_Contact()
        {
            Id = "null";
            IsDeleted = false;
            MasterRecordId = "null";
            SystemModstamp = DateTimeDefaultValue;
            BirthDate = DateTimeDefaultValue;
            base.Init();
        }

        #region Data
        private static List<SF_Contact> ConvertJsonList(List<string> json)
        {
            var converter = new ContactConverter();
            return converter.Convert<SF_Contact>(json).ToList();
        }

        public static List<SF_Contact> GetAll(string accessToken)
        {
            var query = SelectAllQuery(typeof(SF_Contact));
            var request = SFUtilities.CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }

        public static List<SF_Contact> GetList(string accessToken, string where)
        {
            var query = SelectWhere(typeof(SF_Contact), where);
            var request = SFUtilities.CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }

        public static List<SF_Contact> GetListForMS(string accessToken, string searchFor)
        {
            var query = "SELECT Id, Email, Master_Suppressed__c FROM Contact " + searchFor;
            var request = SFUtilities.CreateQueryRequest(accessToken, query.Replace(" ", "%20"), SF_Utilities.Method.GET, SF_Utilities.ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }
        public static List<SF_Contact> GetCampaignMembers(string accessToken, string CampaignID)
        {
            var query = "SELECT Id, IsDeleted, Email, Fax, FirstName, Name, Salutation, HomePhone, LastName, MailingCity, MailingState, MailingCountry, MailingPostalCode, MailingStreet, MobilePhone, Phone, Title, Master_Suppressed__c FROM Contact WHERE Id IN (SELECT ContactId FROM CampaignMember WHERE CampaignID = '" + CampaignID + "')"; //CampaignMember WHERE CampaignID = '" + CampaignID + "'";
            query = query.Replace(" ", "%20");
            
            var request = SFUtilities.CreateQueryRequest(accessToken, query, SF_Utilities.Method.GET, SF_Utilities.ResponseType.JSON);
            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }

        #endregion
    }
}