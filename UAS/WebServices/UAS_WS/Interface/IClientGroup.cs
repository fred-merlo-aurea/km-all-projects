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
    public interface IClientGroup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ClientGroup>> Select(Guid accessKey, bool includeObjects = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ClientGroup>> SelectClient(Guid accessKey, int clientID, bool includeObjects = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userID"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ClientGroup>> SelectForUser(Guid accessKey, int userID, bool includeObjects = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userID"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ClientGroup>> SelectForUserAuthorization(Guid accessKey, int userID, bool includeObjects = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientGroupID"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForClientGroup")]
        Response<KMPlatform.Entity.ClientGroup> Select(Guid accessKey, int clientGroupID, bool includeObjects = false);

        [OperationContract]
        Response<List<KMPlatform.Entity.ClientGroup>> SelectLite(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Entity.ClientGroup x);
    }
}
