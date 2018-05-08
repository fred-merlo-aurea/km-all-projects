using System;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAS_WS.Interface
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    public interface IUserAuthorizationLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AccessKey"></param>
        /// <param name="userAuthLogId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> LogOut(Guid AccessKey, int userAuthLogId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="uAuth"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<KMPlatform.Entity.UserAuthorizationLog> SetUserAuthLog(Guid accessKey, FrameworkUAS.Object.UserAuthorization uAuth, int userID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AccessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid AccessKey, KMPlatform.Entity.UserAuthorizationLog x);
    }
}
