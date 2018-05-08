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
    public interface IApplicationServiceMap
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ApplicationServiceMap>> Select(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="applicationID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ApplicationServiceMap>> SelectForApplication(Guid accessKey, int applicationID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ApplicationServiceMap>> SelectForService(Guid accessKey, int serviceID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Entity.ApplicationServiceMap x);
    }
}
