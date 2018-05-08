using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using ECN_Framework_Entities.Salesforce;
using ECN_Framework_Entities.Salesforce.Convertors;
using ECN_Framework_Entities.Salesforce.Sorting;
using static ECN_Framework_Entities.Salesforce.SF_Utilities;

namespace ecn.communicator.main.Salesforce.Entity
{
    [Serializable]
    [DataContract]
    public class SF_Account : SalesForceBase
    {
        public SF_Account()
        {
            Id = string.Empty;
            IsDeleted = false;
            MasterRecordId = string.Empty;
            Name = string.Empty;
            Type = string.Empty;
            ParentId = string.Empty;
            Description = string.Empty;
            BillingStreet = string.Empty;
            BillingCity = string.Empty;
            BillingState = string.Empty;
            BillingPostalCode = string.Empty;
            BillingCountry = string.Empty;
            BillingLatitude = 0.0;
            BillingLongitude = 0.0;
            Phone = string.Empty;
            Fax = string.Empty;
            Website = string.Empty;
            Industry = string.Empty;
            AnnualRevenue = 0.0;
            OwnerId = string.Empty;
            CreatedDate = DateTime.Now;
            CreatedById = string.Empty;
            LastModifiedDate = DateTime.Now;
            LastModifiedById = string.Empty;
            SystemModstamp = new DateTime(1900, 1, 1);
            LastActivityDate = new DateTime(1900, 1, 1);
            LastViewedDate = new DateTime(1900, 1, 1);
            LastReferencedDate = new DateTime(1900, 1, 1);
            JigsawCompanyId = string.Empty;
        }
        #region Properties
        //[DataMember]
        public string Id { get; set; }
        //[DataMember]
        public bool IsDeleted { get; set; }
        //[DataMember]
        public string MasterRecordId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Type { get; set; }
        //[DataMember]
        public string ParentId { get; set; }
        [DataMember]
        public string BillingStreet { get; set; }
        [DataMember]
        public string BillingCity { get; set; }
        [DataMember]
        public string BillingState { get; set; }
        [DataMember]
        public string BillingPostalCode { get; set; }
        [DataMember]
        public string BillingCountry { get; set; }
        [DataMember]
        public double BillingLatitude { get; set; }
        [DataMember]
        public double BillingLongitude { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Website { get; set; }
        [DataMember]
        public string Industry { get; set; }
        //[DataMember]
        public double AnnualRevenue { get; set; }
        [DataMember]
        public string Description { get; set; }
        //[DataMember]
        public string OwnerId { get; set; }
        //[DataMember]
        public DateTime CreatedDate { get; set; }
        //[DataMember]
        public string CreatedById { get; set; }
        // [DataMember]
        public DateTime LastModifiedDate { get; set; }
        //[DataMember]
        public string LastModifiedById { get; set; }
        //[DataMember]
        public DateTime SystemModstamp { get; set; }
        // [DataMember]
        public DateTime LastActivityDate { get; set; }
        //[DataMember]
        public DateTime LastViewedDate { get; set; }
        //[DataMember]
        public DateTime LastReferencedDate { get; set; }
        //[DataMember]
        public string JigsawCompanyId { get; set; }
        #endregion

        #region Data
        private static List<SF_Account> ConvertJsonList(List<string> json)
        {
            var converter = new AccountConverter();
            return converter.Convert<SF_Account>(json).ToList();
        }
        public static List<SF_Account> GetAll(string accessToken)
        {
            var query = SF_Utilities.SelectAllQuery(typeof(SF_Account));
            var request = SFUtilities.CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }
        public static List<SF_Account> GetList(string accessToken, string where)
        {
            var query = SF_Utilities.SelectWhere(typeof(SF_Account), where);
            var request = SFUtilities.CreateQueryRequest(accessToken, query, Method.GET, ResponseType.JSON);

            var jsonList = GetNotDoneItems(query, request);
            return ConvertJsonList(jsonList.ToList());
        }
        public static List<SF_Account> Sort(List<SF_Account> list, string sortBy, SortDirection sortDir)
        {
            var isAscending = sortDir == SortDirection.Ascending;
            var utility = new EntitySort();
            return utility.Sort(list, sortBy, isAscending).ToList();
        }
        public static bool Insert(string accessToken, SF_Account obj)
        {
            return SFUtilities.Insert(accessToken, obj, SFObject.Account);
        }
        public static bool Update(string accessToken, SF_Account obj)
        {
            return SFUtilities.Update(accessToken, obj, SFObject.Account, obj.Id);
        }

        #endregion
    }
}