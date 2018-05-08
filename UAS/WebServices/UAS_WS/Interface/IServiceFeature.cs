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
    public interface IServiceFeature
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ServiceFeature>> Select(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForService")]
        Response<List<KMPlatform.Entity.ServiceFeature>> Select(Guid accessKey, int serviceID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="serviceFeatureID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<KMPlatform.Entity.ServiceFeature> SelectServiceFeature(Guid accessKey, int serviceFeatureID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.ServiceFeature>> SelectOnlyEnabled(Guid accessKey, int serviceID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> Save(Guid accessKey, KMPlatform.Entity.ServiceFeature x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> SaveReturnId(Guid accessKey, KMPlatform.Entity.ServiceFeature x);
    }
}
