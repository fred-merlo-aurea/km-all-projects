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
    public interface IClientGroupClientMap
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ClientGroupClientMap>> Select(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientGroupID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ClientGroupClientMap>> SelectForClientGroup(Guid accessKey, int clientGroupID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ClientGroupClientMap>> SelectForClientID(Guid accessKey, int clientID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Entity.ClientGroupClientMap x);
    }
}
