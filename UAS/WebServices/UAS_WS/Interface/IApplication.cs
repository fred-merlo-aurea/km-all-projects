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
    public interface IApplication
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        [ServiceKnownType(typeof(bool?))]
        Response<List<KMPlatform.Entity.Application>> Select(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForUser")]
        Response<List<KMPlatform.Entity.Application>> SelectForUser(Guid accessKey, int userID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="securityGroupID"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForSecurityGroup")]
        Response<List<KMPlatform.Entity.Application>> SelectForSecurityGroup(Guid accessKey, int securityGroupID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Application>> SelectForService(Guid accessKey, int serviceID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="serviceID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Application>> SelectOnlyEnabledForService(Guid accessKey, int serviceID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="serviceID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Application>> SelectOnlyEnabledForServiceUserID(Guid accessKey, int serviceID, int userID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchList"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Application>> Search(Guid accessKey, string searchValue, List<KMPlatform.Entity.Application> searchList, bool? isActive = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Entity.Application x);
    }
}
