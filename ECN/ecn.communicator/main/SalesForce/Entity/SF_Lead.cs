using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Web.UI.WebControls;
using ECN_Framework_Entities.Salesforce;
using ECN_Framework_Entities.Salesforce.Sorting;
using ECN_Framework_Entities.Salesforce.Convertors;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ecn.communicator.main.Salesforce.Entity
{
    [Serializable]
    [DataContract]
    public class SF_Lead : LeadBase
    {
        #region Data
        private static List<SF_Lead> ConvertJsonList(List<string> json)
        {
            var converter = new LeadConverter();
            return converter.Convert<SF_Lead>(json).ToList();
        }

        private static SF_Lead ConvertJsonItem(List<string> json)
        {
            var converter = new LeadConverter();
            return converter.Convert<SF_Lead>(json).FirstOrDefault() ?? new SF_Lead();
        }

        public static List<SF_Lead> GetAll(string accessToken)
        {
            var query = SF_Utilities.SelectAllQuery(typeof(SF_Lead));
            var request = SFUtilities.CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }
        public static SF_Lead GetEmail(string accessToken, string email)
        {
            //kkitchens@randallreilly.com
            SF_Lead lead = null;
            string where = "WHERE Email = '" + email + "'";
            string query = SF_Utilities.SelectWhere(typeof(SF_Lead), where);
            WebRequest req = SF_Utilities.CreateQueryRequest(accessToken, query, SF_Utilities.Method.GET, SF_Utilities.ResponseType.JSON);
            try
            {
                WebResponse resp = req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                List<string> jsonList = new List<string>();
                System.Text.StringBuilder json = new System.Text.StringBuilder();
                while (sr.EndOfStream == false)
                {
                    string x = sr.ReadLine();
                    json.AppendLine(x);
                    jsonList.Add(x.Trim());
                }
                resp.Close();
                SF_Utilities.WriteToLog(json.ToString());

                lead = ConvertJsonItem(jsonList);


                SF_Utilities.WriteToLog("******Lead***********");
                SF_Utilities.WriteToLog("Id : " + lead.Id.ToString());
                SF_Utilities.WriteToLog("FirstName : " + lead.FirstName.ToString());
                SF_Utilities.WriteToLog("LastName : " + lead.LastName.ToString());
                SF_Utilities.WriteToLog("Street : " + lead.Street.ToString());
                SF_Utilities.WriteToLog("City : " + lead.City.ToString());
                SF_Utilities.WriteToLog("State : " + lead.State.ToString());
                SF_Utilities.WriteToLog("PostalCode : " + lead.PostalCode.ToString());
                SF_Utilities.WriteToLog("Country : " + lead.Country.ToString());
                SF_Utilities.WriteToLog("Email : " + lead.Email.ToString());
                SF_Utilities.WriteToLog("Fax : " + lead.Fax.ToString());
                SF_Utilities.WriteToLog("MobilePhone : " + lead.MobilePhone.ToString());
                SF_Utilities.WriteToLog("Phone : " + lead.Phone.ToString());
                SF_Utilities.WriteToLog("HasOptedOutOfEmail : " + lead.HasOptedOutOfEmail.ToString());
                SF_Utilities.WriteToLog("LastModifiedDate : " + lead.LastModifiedDate.ToString());
                SF_Utilities.WriteToLog("*********************");

            }
            catch (WebException ex)
            {
                SF_Utilities.LogWebException(ex, query.ToString());
            }
            return lead;
        }
        public static List<SF_Lead> GetList(string accessToken, string where)
        {
            var query = SF_Utilities.SelectWhere(typeof(SF_Lead), where);
            var request = SFUtilities.CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }
        public static SF_Lead GetSingle(string accessToken, string where)
        {
            return SFUtilities.GetSingle<SF_Lead>(accessToken, where, new LeadConverter()) ?? new SF_Lead();
        }
        public static List<SF_Lead> GetByTagList(string accessToken, List<SF_LeadTag> tags)
        {
            var sfTags = new StringBuilder();
            foreach (var leadTag in tags)
            {
                sfTags.Append(string.Format("'{0}',", leadTag.ItemId));
            }

            var where = string.Format("WHERE Id in ({0})", sfTags.ToString().TrimEnd(','));
            var query = SF_Utilities.SelectWhere(typeof(SF_Lead), where);
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

        public static bool Insert(string accessToken, SF_Lead obj)
        {
            return SFUtilities.Insert(accessToken, obj, SFObject.Lead);
        }
        public static bool Update(string accessToken, SF_Lead obj)
        {
            return SFUtilities.Update(accessToken, obj, SFObject.Lead, obj.Id);
        }

        public static List<SF_Lead> Sort(List<SF_Lead> list, string sortBy, SortDirection sortDir)
        {
            var isAscending = sortDir == SortDirection.Ascending;
            var utility = new EntitySort();
            return utility.Sort(list, sortBy, isAscending).ToList();
        }

        public static bool OptOut(string accessToken, string Id)
        {
            bool success = false;
            //to minimize failures because of incomplete data coming from ECN
            string json = "{\"HasOptedOutOfEmail\":true}";
            HttpWebRequest req = (HttpWebRequest)SF_Utilities.CreateUpdateRequest(accessToken, json, SF_Utilities.SFObject.Lead, Id, SF_Utilities.ResponseType.JSON);
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

        public static string GetXMLForOptOutJob(Dictionary<string, string> dicLeads)
        {
            return SFUtilities.GetXmlForOutJob(dicLeads);
        }
        #endregion
    }
}