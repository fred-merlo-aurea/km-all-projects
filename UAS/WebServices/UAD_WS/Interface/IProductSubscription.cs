using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;
using System.Data;

namespace UAD_WS.Interface
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IProductSubscription
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="subscriptionID"></param>
        /// <param name="client"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAD.Entity.ProductSubscription>> Select(Guid accessKey, int subscriptionID, KMPlatform.Object.ClientConnections client, bool includeCustomProperties = false);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="pubSubscriptionID"></param>
        /// <param name="client"></param>
        /// <param name="clientDisplayName"></param>
        /// <returns></returns>
        [OperationContract]
        Response<FrameworkUAD.Entity.ProductSubscription> SelectProductSubscription(Guid accessKey, int pubSubscriptionID, KMPlatform.Object.ClientConnections client, string clientDisplayName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="xml"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> SaveBulkActionIDUpdate(Guid accessKey, string xml, KMPlatform.Object.ClientConnections client);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="productID"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> SelectCount(Guid accessKey, int productID, KMPlatform.Object.ClientConnections client);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="productID"></param>
        /// <param name="client"></param>
        /// <param name="clientDisplayName"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAD.Entity.ProductSubscription>> SelectPaging(Guid accessKey, int page, int pageSize, int productID, KMPlatform.Object.ClientConnections client, string clientDisplayName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="productID"></param>
        /// <param name="client"></param>
        /// <param name="clientDisplayName"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAD.Entity.ProductSubscription>> SelectPublication(Guid accessKey, int productID, KMPlatform.Object.ClientConnections client, string clientDisplayName);
        /// <summary>
        /// /
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="client"></param>
        /// <param name="clientDisplayName"></param>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="company"></param>
        /// <param name="title"></param>
        /// <param name="add1"></param>
        /// <param name="city"></param>
        /// <param name="regionCode"></param>
        /// <param name="zip"></param>
        /// <param name="country"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="sequenceID"></param>
        /// <param name="account"></param>
        /// <param name="publisherId"></param>
        /// <param name="publicationId"></param>
        /// <param name="subscriptionID"></param>
        /// <returns></returns>
        [OperationContract(Name = "SearchForProfile")]
        Response<List<FrameworkUAD.Entity.ProductSubscription>> Search(Guid accessKey, KMPlatform.Object.ClientConnections client, string clientDisplayName, string fName = "", string lName = "", string company = "", string title = "", string add1 = "", string city = "", string regionCode = "", string zip = "", string country = "", string email = "", string phone = "", int sequenceID = 0, string account = "", int publisherId = 0, int publicationId = 0, int subscriptionID = 0);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.ProductSubscription>> SearchSuggestMatch(Guid accessKey, KMPlatform.Object.ClientConnections client, int publisherId, int publicationId, string firstName = "", string lastName = "", string email = "");

        [OperationContract]
        Response<int> UpdateLock(Guid accessKey, int SubscriberID, bool IsLocked, int UserID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.ProductSubscription>> SelectSequence(Guid accessKey, int sequenceID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> UpdateQDate(Guid accessKey, int SubscriptionID, DateTime? QSourceDate, int UpdatedByUserID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.ProductSubscription>> SearchAddressZip(Guid accessKey, string address1, string zipCode, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> ClearWaveMailingInfo(Guid accessKey, int waveMailingID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> UpdateRequesterFlags(Guid accessKey, int productID, int issueID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> SaveBulkWaveMailing(Guid accessKey, string xml, int waveMailingID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.ProductSubscription x, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<int> ProfileSave(Guid accessKey, FrameworkUAD.Entity.ProductSubscription curr, FrameworkUAD.Entity.ProductSubscription orig, bool saveWaveMailing, int applicationID,
            KMPlatform.BusinessLogic.Enums.UserLogTypes ult, FrameworkUAS.Object.Batch batch, KMPlatform.Object.ClientConnections client, FrameworkUAD.Entity.ProductSubscription waveMail = null,
            FrameworkUAD.Entity.WaveMailingDetail waveMailDetail = null);

        [OperationContract]
        Response<int> FullSave(Guid accessKey, FrameworkUAD.Entity.ProductSubscription curr, FrameworkUAD.Entity.ProductSubscription orig, bool saveWaveMailing, int applicationID, KMPlatform.BusinessLogic.Enums.UserLogTypes ult,
            FrameworkUAS.Object.Batch batch, int clientID, bool madeResponseChange, bool madePaidChange, bool madeBillToChange,
            List<FrameworkUAD.Entity.ProductSubscriptionDetail> answers, FrameworkUAD.Entity.ProductSubscription waveMail = null, FrameworkUAD.Entity.WaveMailingDetail waveMailDetail = null, 
            FrameworkUAD.Entity.SubscriptionPaid subPaid = null, FrameworkUAD.Entity.PaidBillTo billTo = null, List<FrameworkUAD.Entity.ProductSubscriptionDetail> subscriberAnswers = null);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.ActionProductSubscription>> SelectActionSubscription(Guid accessKey, int productID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<FrameworkUAD.Entity.ActionProductSubscription>> SelectArchiveActionSubscription(Guid accessKey, int productID, int issueID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<DataTable> SelectForExport(Guid accessKey, int page, int pageSize, string columns, int productID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<DataTable> SelectForExportStatic(Guid accessKey, int productID, string cols, List<int> subs, KMPlatform.Object.ClientConnections client);
        [OperationContract(Name = "SelectForExportStaticIssueId")]
        Response<DataTable> SelectForExportStatic(Guid accessKey, int productID,int issueid, string cols, List<int> subs, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<FrameworkUAD.Entity.CopiesProductSubscription>> SelectAllActiveIDs(Guid accessKey, int productID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<Dictionary<int, int>> SaveDQM(Guid accessKey, FrameworkUAD.Entity.ProductSubscription x, string clientName, string clientDisplayName, int clientID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<FrameworkUAD.Object.PubSubscriptionAdHoc>> Get_AdHocs_PubSubscription(Guid accessKey, int pubID, int pubSubscriptionID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<string>> Get_AdHocs(Guid accessKey, int pubID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.ProductSubscription>> SelectForUpdate(Guid accessKey, int productID, int issueid, List<int> subs, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> RecordUpdate(Guid accessKey, HashSet<int> pubSubscriptionIds, string changes, int issueid, int productID, int userid, KMPlatform.Object.ClientConnections client);
      
    }
}
