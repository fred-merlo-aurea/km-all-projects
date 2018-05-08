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
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IUserAuthorization
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="saltValue"></param>
        /// <param name="authSource"></param>
        /// <param name="ipAddress"></param>
        /// <param name="serverVariables"></param>
        /// <param name="appVersion"></param>
        /// <param name="useLite"></param>
        /// <returns></returns>
        [OperationContract]
        Response<KMPlatform.Object.UserAuthorization> Login(string userName, string password, string saltValue, string authSource, string ipAddress = "", KMPlatform.Object.ServerVariable serverVariables = null, string appVersion = "", bool useLite = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="authSource"></param>
        /// <param name="ipAddress"></param>
        /// <param name="serverVariables"></param>
        /// <param name="appVersion"></param>
        /// <returns></returns>
        [OperationContract(Name = "LoginWithAccessKey")]
        Response<KMPlatform.Object.UserAuthorization> Login(Guid accessKey, string authSource, string ipAddress = "", KMPlatform.Object.ServerVariable serverVariables = null, string appVersion = "");

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Response<string> GetIpAddress();
    }
}
