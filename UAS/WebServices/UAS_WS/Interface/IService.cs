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
    public interface IService
    {
        /// <summary>
        /// /
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Service>> Select(Guid accessKey, bool includeObjects = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="serviceID"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForService")]
        Response<KMPlatform.Entity.Service> Select(Guid accessKey, int serviceID, bool includeObjects = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userID"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Service>> SelectForUser(Guid accessKey, int userID, bool includeObjects = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Service>> SelectForUserAuthorization(Guid accessKey, int userID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [OperationContract]
        Response<KMPlatform.Entity.Service> SetObjectsForUserAuthorization(Guid accessKey, KMPlatform.Entity.Service service);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [OperationContract]
        Response<KMPlatform.Entity.Service> SetObjects(Guid accessKey, KMPlatform.Entity.Service service);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="service"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForServiceName")]
        Response<KMPlatform.Entity.Service> Select(Guid accessKey, KMPlatform.Enums.Services service, bool includeObjects = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Entity.Service x);
    }
}
