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
    public interface IMenu
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="includeFeatures"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Menu>> Select(Guid accessKey, bool includeFeatures = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="securityGroupID"></param>
        /// <param name="hasAccess"></param>
        /// <param name="includeFeatures"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForSecurityGroup")]
        Response<List<KMPlatform.Entity.Menu>> Select(Guid accessKey, int securityGroupID, bool hasAccess, bool includeFeatures = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="securityGroupID"></param>
        /// <param name="hasAccess"></param>
        /// <param name="applicationID"></param>
        /// <param name="includeFeatures"></param>
        /// <returns></returns>
        [OperationContract(Name = "SelectForSecurityGroupAndApplication")]
        Response<List<KMPlatform.Entity.Menu>> Select(Guid accessKey, int securityGroupID, bool hasAccess, int applicationID, bool includeFeatures = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="applicationID"></param>
        /// <param name="isActive"></param>
        /// <param name="includeFeatures"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Menu>> SelectForApplication(Guid accessKey, int applicationID, bool isActive, bool includeFeatures = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="applicationID"></param>
        /// <param name="userID"></param>
        /// <param name="isActive"></param>
        /// <param name="includeFeatures"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Menu>> SelectForApplicationUserID(Guid accessKey, int applicationID, int userID, bool isActive, bool includeFeatures = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userID"></param>
        /// <param name="securityGroupID"></param>
        /// <param name="isActive"></param>
        /// <param name="hasAccess"></param>
        /// <param name="includeFeatures"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.Menu>> SelectForUser(Guid accessKey, int userID, int securityGroupID, bool isActive, bool hasAccess, bool includeFeatures = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Entity.Menu x);
    }
}
