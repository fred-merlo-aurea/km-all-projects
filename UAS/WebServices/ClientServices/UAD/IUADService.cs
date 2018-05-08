using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;
using System.ServiceModel.Web;

namespace ClientServices.UAD
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUADService" in both code and config file together.
    [ServiceContract]
    public interface IUADService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "SaveSubscriber/?accessKey={accessKey}",
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Response<FrameworkUAD.ServiceResponse.SavedSubscriber> SaveSubscriber(Guid accessKey, FrameworkUAD.Object.SaveSubscriber newSubscriber);

        [OperationContract]
        [WebGet(UriTemplate = "GetSubscriber/?accessKey={accessKey}&email={emailAddress}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Response<List<FrameworkUAD.Object.Subscription>> GetSubscriber(Guid accessKey, string emailAddress);

        [OperationContract]
        Response<List<FrameworkUAD.Object.SubscriberConsensus>> GetDemographics(Guid accessKey, string emailAddress, List<FrameworkUAD.Object.SubscriberConsensusDemographic> dimensions = null);
        
        [OperationContract]
        Response<List<FrameworkUAD.Object.SubscriberProduct>> GetProductDemographics(Guid accessKey, string emailAddress, string productCode, List<FrameworkUAD.Object.SubscriberProductDemographic> dimensions = null);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Response<List<FrameworkUAD.Object.CustomField>> GetCustomFields(Guid accessKey, string productCode = "");
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Response<List<FrameworkUAD.Object.CustomField>> GetConsensusCustomFields(Guid accessKey);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Response<List<FrameworkUAD.Object.CustomFieldValue>> GetCustomFieldValues(Guid accessKey, string productCode = "", string customFieldName = "");
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Response<List<FrameworkUAD.Object.CustomFieldValue>> GetConsensusCustomFieldValues(Guid accessKey, string customFieldName = "");

        #region Paid methods
        [OperationContract]
        [WebInvoke(UriTemplate = "SavePaidSubscriber/?accessKey={accessKey}",
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Response<int> SavePaidSubscriber(Guid accessKey, FrameworkUAD.Object.PaidSubscription newPaidSubscription);
        #endregion
    }
}
