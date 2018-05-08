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
    public interface ISecurityGroup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="includeServices"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.SecurityGroup>> Select(Guid accessKey, bool includeServices = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientGroupID"></param>
        /// <param name="includeServices"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.SecurityGroup>> SelectForClientGroup(Guid accessKey, int clientGroupID, bool includeServices = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientGroupID"></param>
        /// <param name="includeServices"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.SecurityGroup>> SelectActiveForClientGroup(Guid accessKey, int clientGroupID, bool includeServices = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userID"></param>
        /// <param name="clientID"></param>
        /// <param name="isKMUser"></param>
        /// <param name="includeServices"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForUserClient")]
        Response<KMPlatform.Entity.SecurityGroup> Select(Guid accessKey, int userID, int clientID, bool isKMUser, bool includeServices = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="name"></param>
        /// <param name="clientGroupID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> SecurityGroupNameExists(Guid accessKey, string name, int clientGroupID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <param name="clientGroupID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Entity.SecurityGroup x, int clientGroupID, int userID);
    }
}
