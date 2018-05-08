using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAS_WS.Interface
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Client>> Select(Guid accessKey, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientName"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForClientName")]
        Response<KMPlatform.Entity.Client> Select(Guid accessKey, string clientName, bool includeCustomProperties = false);

        [OperationContract]
        Response<KMPlatform.Entity.Client> SelectFtpFolder(Guid accessKey, string ftpFolder, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForClientID")]
        Response<KMPlatform.Entity.Client> Select(Guid accessKey, int clientID, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract]
        Response<KMPlatform.Entity.Client> SelectDefault(Guid accessKey, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Client>> SelectForAccessKey(Guid accessKey, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientGroupID"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Client>> SelectForClientGroup(Guid accessKey, int clientGroupID, bool includeCustomProperties = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Entity.Client x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchList"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Client>> Search(Guid accessKey, string searchValue, List<KMPlatform.Entity.Client> searchList, bool? isActive = null);

        #region Services and Features
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="service"></param>
        /// <param name="clientGroupID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> HasService(Guid accessKey, int clientID, KMPlatform.Enums.Services service, int clientGroupID = 1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="client"></param>
        /// <param name="service"></param>
        /// <param name="clientGroupID"></param>
        /// <returns></returns>
        [OperationContract(Name = "HasServiceForClient")]
        Response<bool> HasService(Guid accessKey, KMPlatform.Entity.Client client, KMPlatform.Enums.Services service, int clientGroupID = 1);

        /// <summary>
        /// /
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="service"></param>
        /// <param name="featureName"></param>
        /// <param name="clientGroupID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> HasFeature(Guid accessKey, int clientID, KMPlatform.Enums.Services service, string featureName, int clientGroupID = 1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="client"></param>
        /// <param name="service"></param>
        /// <param name="featureName"></param>
        /// <param name="clientGroupID"></param>
        /// <returns></returns>
        [OperationContract(Name = "HasFeatureForClient")]
        Response<bool> HasFeature(Guid accessKey, KMPlatform.Entity.Client client, KMPlatform.Enums.Services service, string featureName, int clientGroupID = 1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="clientGroupID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> HasFulfillmentService(Guid accessKey, int clientID, int clientGroupID = 1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="client"></param>
        /// <param name="clientGroupID"></param>
        /// <returns></returns>
        [OperationContract(Name = "HasFulfillmentForClient")]
        Response<bool> HasFulfillmentService(Guid accessKey, KMPlatform.Entity.Client client, int clientGroupID = 1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="clientGroupID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> UseUADSuppressionFeature(Guid accessKey, int clientID, int clientGroupID = 1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="client"></param>
        /// <param name="clientGroupID"></param>
        /// <returns></returns>
        [OperationContract(Name = "HasSuppressionForClient")]
        Response<bool> UseUADSuppressionFeature(Guid accessKey, KMPlatform.Entity.Client client, int clientGroupID = 1);

        [OperationContract]
        Response<FrameworkUAS.Object.ClientAdditionalProperties> GetClientAdditionalProperties(Guid accessKey, int clientId, bool isFileDeleted);
        #endregion
    }
}
