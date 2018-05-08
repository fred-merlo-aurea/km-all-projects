using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using ECN_Framework_Common.Functions;
using ECN_Framework_Common.Interfaces;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Salesforce.Helpers;
using ECN_Framework_Entities.Salesforce.Interfaces;

namespace ECN_Framework_Entities.Salesforce
{
    public class SalesForceBase
    {
        private const string DonePattern = ".done\" :.";
        private const string NextRecordsUrlPattern = ".nextRecordsUrl\" :.";
        private const string NullString = "null";
        protected readonly DateTime DateTimeDefaultValue = new DateTime(1900, 1, 1);

        protected static ISFUtilities _sfUtilities;

        public string AccountId { get; set; }
        [DataMember]
        [XmlElement]
        public string Email { get; set; }
        [DataMember]
        [XmlElement]
        public string Fax { get; set; }
        [DataMember]
        [XmlElement]
        public string FirstName { get; set; }
        [DataMember]
        [XmlElement]
        public string Salutation { get; set; }
        public string Name { get; set; }
        [DataMember]
        [XmlElement]
        public string OtherCity { get; set; }
        [DataMember]
        [XmlElement]
        public string OtherStreet { get; set; }
        [DataMember]
        [XmlElement]
        public string OtherState { get; set; }
        [DataMember]
        [XmlElement]
        public string OtherPostalCode { get; set; }
        [DataMember]
        [XmlElement]
        public string OtherCountry { get; set; }
        [DataMember]
        [XmlElement]
        public double OtherLatitude { get; set; }
        [DataMember]
        [XmlElement]
        public double OtherLongitude { get; set; }
        [DataMember]
        [XmlElement]
        public string OtherPhone { get; set; }
        [DataMember]
        [XmlElement]
        public string AssistantPhone { get; set; }
        [DataMember]
        [XmlElement]
        public string AssistantName { get; set; }
        public string LeadSource { get; set; }
        [DataMember]
        [XmlElement]
        public string Description { get; set; }
        public string OwnerId { get; set; }
        [DataMember]
        [XmlElement]
        public string HomePhone { get; set; }
        [DataMember]
        [XmlElement]
        public string LastName { get; set; }
        [DataMember]
        [XmlElement]
        public string MailingCity { get; set; }
        [DataMember]
        [XmlElement]
        public string MailingState { get; set; }
        [DataMember]
        [XmlElement]
        public string MailingCountry { get; set; }
        [DataMember]
        [XmlElement]
        public string MailingPostalCode { get; set; }
        [DataMember]
        [XmlElement]
        public string MailingStreet { get; set; }
        [DataMember]
        [XmlElement]
        public double MailingLatitude { get; set; }
        [DataMember]
        [XmlElement]
        public double MailingLongitude { get; set; }
        [DataMember]
        [XmlElement]
        public string MobilePhone { get; set; }
        [DataMember]
        [XmlElement]
        public string Phone { get; set; }
        [DataMember]
        [XmlElement]
        public string Title { get; set; }
        [DataMember]
        [XmlElement]
        public string Department { get; set; }
        [DataMember]
        [XmlElement]
        public bool HasOptedOutOfEmail { get; set; }
        [DataMember]
        [XmlElement]
        public bool DoNotCall { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedById { get; set; }
        public DateTime LastActivityDate { get; set; }
        public DateTime LastCURequestDate { get; set; }
        public DateTime LastViewedDate { get; set; }
        public DateTime LastReferencedDate { get; set; }
        public string EmailBouncedReason { get; set; }
        public DateTime EmailBouncedDate { get; set; }
        public string JigsawContactId { get; set; }
        [DataMember]
        [XmlElement]
        public bool Master_Suppressed__c { get; set; }

        protected void Init()
        {
            AccountId = NullString;
            Email = NullString;
            Fax = NullString;
            FirstName = NullString;
            Salutation = NullString;
            Name = NullString;
            OtherCity = NullString;
            OtherCountry = NullString;
            OtherLatitude = 0.0;
            OtherLongitude = 0.0;
            OtherPhone = NullString;
            OtherPostalCode = NullString;
            OtherState = NullString;
            OtherStreet = NullString;
            AssistantName = NullString;
            AssistantPhone = NullString;
            LeadSource = NullString;
            Description = NullString;
            OwnerId = NullString;
            HomePhone = NullString;
            LastName = NullString;
            MailingCity = NullString;
            MailingState = NullString;
            MailingCountry = NullString;
            MailingPostalCode = NullString;
            MailingStreet = NullString;
            MobilePhone = NullString;
            Phone = NullString;
            Title = NullString;
            Department = NullString;
            HasOptedOutOfEmail = false;
            DoNotCall = false;
            CreatedDate = DateTimeDefaultValue;
            CreatedById = NullString;
            LastModifiedDate = DateTimeDefaultValue;
            LastModifiedById = NullString;
            LastActivityDate = DateTimeDefaultValue;
            LastCURequestDate = DateTimeDefaultValue;
            LastViewedDate = DateTimeDefaultValue;
            LastReferencedDate = DateTimeDefaultValue;
            EmailBouncedReason = NullString;
            EmailBouncedDate = DateTimeDefaultValue;
            JigsawContactId = NullString;
        }

        protected static ISFUtilities SFUtilities
        {
            get
            {
                if (_sfUtilities == null)
                {
                    _sfUtilities = new SFUtilitiesAdapter();
                }

                return _sfUtilities;
            }
        }

        public static void InitializeSFUtilities(ISFUtilities sfUtilities)
        {
            _sfUtilities = sfUtilities;
        }

        public static IList<string> GetNotDoneItems(string query, WebRequest request)
        {
            var isAllDone = false;
            var jsonList = new List<string>();
            try
            {
                var response = request.GetResponse();
                var responseStream = new StreamReader(response.GetResponseStream());
                while (!isAllDone)
                {
                    var properties = new List<string>();
                    while (responseStream.EndOfStream == false)
                    {
                        var property = responseStream.ReadLine();
                        properties.Add(property.Trim());
                    }

                    var isItemDone = string.Empty;
                    try
                    {
                        var doneExpression = new Regex(DonePattern);
                        foreach (var property in properties)
                        {
                            if (doneExpression.Match(property).Success)
                            {
                                isItemDone = property.Split(':')[1].Trim().TrimEnd(',');
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("An error occurred while checking item properties: {0}", ex);
                        isAllDone = true;
                    }

                    bool.TryParse(isItemDone, out isAllDone);
                    if (!isAllDone)
                    {
                        var nextRecordExpression = new Regex(NextRecordsUrlPattern);
                        foreach (var property in properties)
                        {
                            if (nextRecordExpression.Match(property).Success)
                            {
                                string nextRecordUrl = property.Split(':')[1].Trim().TrimEnd(',');
                                nextRecordUrl = nextRecordUrl.Replace("\"", string.Empty);

                                responseStream = SFUtilities.GetNextURL(nextRecordUrl);
                                break;
                            }
                        }
                    }

                    jsonList.AddRange(properties);
                    response.Close();
                }
            }
            catch (WebException ex)
            {
                SFUtilities.LogWebException(ex, query.ToString());
            }

            return jsonList;
        }
    }
}
