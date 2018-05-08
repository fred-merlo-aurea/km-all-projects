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
    public interface IUserClientSecurityGroupMap
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.UserClientSecurityGroupMap>> Select(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.UserClientSecurityGroupMap>> SelectForUser(Guid accessKey, int userID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.UserClientSecurityGroupMap>> SelectForUserAuthorization(Guid accessKey, int userID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.UserClientSecurityGroupMap>> SelectForClient(Guid accessKey, int clientID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="securityGroupID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.UserClientSecurityGroupMap>> SelectForSecurityGroup(Guid accessKey, int securityGroupID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Entity.UserClientSecurityGroupMap x);
    }
}
