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
    public interface IClientUADUsersMap
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ClientUADUsersMap>> Select(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForClient")]
        Response<List<KMPlatform.Entity.ClientUADUsersMap>> SelectClient(Guid accessKey, int clientID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForUser")]
        Response<List<KMPlatform.Entity.ClientUADUsersMap>> SelectUser(Guid accessKey, int userID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForClientAndUser")]
        Response<KMPlatform.Entity.ClientUADUsersMap> Select(Guid accessKey, int clientID, int userID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> Save(Guid accessKey, KMPlatform.Entity.ClientUADUsersMap x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [OperationContract(Name="SaveClientAndUser")]
        Response<bool> Save(Guid accessKey, int clientID, int userID);
    }
}
