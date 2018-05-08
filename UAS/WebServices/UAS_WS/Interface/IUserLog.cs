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
    public interface IUserLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="applicationID"></param>
        /// <param name="userLogType"></param>
        /// <param name="userID"></param>
        /// <param name="objectName"></param>
        /// <param name="originalObjectJson"></param>
        /// <param name="newObjectJson"></param>
        /// <returns></returns>
        [OperationContract]
        Response<KMPlatform.Entity.UserLog> CreateLog(Guid accessKey, int applicationID, KMPlatform.BusinessLogic.Enums.UserLogTypes userLogType,
                                                          int userID, string objectName, string originalObjectJson, string newObjectJson);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.UserLog>> Select(Guid accessKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userLogID"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForUserLog")]
        Response<KMPlatform.Entity.UserLog> Select(Guid accessKey, int userLogID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Entity.UserLog x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.UserLog>> SaveBulkInsert(Guid accessKey, List<KMPlatform.Entity.UserLog> x, KMPlatform.Entity.Client client);
    }
}
