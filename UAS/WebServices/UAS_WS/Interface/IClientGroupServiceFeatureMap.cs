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
    public interface IClientGroupServiceFeatureMap
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientGroupID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ClientGroupServiceFeatureMap>> SelectForClientGroup(Guid accessKey, int clientGroupID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientGroupID"></param>
        /// <param name="serviceFeatureID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ClientGroupServiceFeatureMap>> SelectForServiceFeature(Guid accessKey, int clientGroupID, int serviceFeatureID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientGroupID"></param>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ClientGroupServiceFeatureMap>> SelectForService(Guid accessKey, int clientGroupID, int serviceID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Entity.ClientGroupServiceFeatureMap x);
    }
}
