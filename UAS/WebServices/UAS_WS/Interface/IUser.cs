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
    public interface IUser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.User>> Select(Guid accessKey, bool includeObjects = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientGroupID"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        [OperationContract(Name = "ForClientGroup")]
        Response<List<KMPlatform.Entity.User>> Select(Guid accessKey, int clientGroupID, bool includeObjects = false);

        /// <summary>
        /// /
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="securityGroupID"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        [OperationContract(Name = "ForSecurityGroupID")]
        Response<List<KMPlatform.Entity.User>> Select(Guid accessKey, int clientID, int securityGroupID, bool includeObjects = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="securityGroupName"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        [OperationContract(Name = "ForSecurityGroupName")]
        Response<List<KMPlatform.Entity.User>> Select(Guid accessKey, int clientID, string securityGroupName, bool includeObjects = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        [OperationContract]
        Response<KMPlatform.Entity.User> LogIn(string userName, string password, bool includeObjects = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        [OperationContract(Name = "LogInWithAccessKey")]
        Response<KMPlatform.Entity.User> LogIn(Guid accessKey, bool includeObjects = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [OperationContract]
        Response<KMPlatform.Entity.User> SetUserObjects(Guid accessKey, KMPlatform.Entity.User user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [OperationContract]
        Response<KMPlatform.Entity.User> SearchUserName(Guid accessKey, string userName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        [OperationContract]
        Response<KMPlatform.Entity.User> FindForgotPassword(string emailAddress);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [OperationContract]
        Response<KMPlatform.Entity.User> SetAuthorizedUserObjects(Guid accessKey, KMPlatform.Entity.User user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [OperationContract]
        Response<string> GetEmailAddress(Guid accessKey, string userName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> EmailExist(Guid accessKey, string email);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchList"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<KMPlatform.Entity.User>> Search(Guid accessKey, string searchValue, List<KMPlatform.Entity.User> searchList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userID"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        [OperationContract]
        Response<KMPlatform.Entity.User> SelectUser(Guid accessKey, int userID, bool includeObjects = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Entity.User x, ECN_Framework_Entities.Communicator.EmailDirect ed = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> IsKmUser(Guid accessKey, KMPlatform.Entity.User x);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Response<string> GenerateTempPassword();
    }
}
