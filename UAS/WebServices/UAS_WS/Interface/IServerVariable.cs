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
    public interface IServerVariable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Response<KMPlatform.Object.ServerVariable> GetServerVariables();
    }
}
