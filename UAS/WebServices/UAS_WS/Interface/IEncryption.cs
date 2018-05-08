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
    public interface IEncryption
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="ec"></param>
        /// <returns></returns>
        [OperationContract]
        Response<FrameworkUAS.Object.Encryption> Encrypt(Guid accessKey, FrameworkUAS.Object.Encryption ec);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="ec"></param>
        /// <returns></returns>
        [OperationContract]
        Response<FrameworkUAS.Object.Encryption> Decrypt(Guid accessKey, FrameworkUAS.Object.Encryption ec);
    }
}
