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
    public interface IClientFTP
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.ClientFTP>> Select(Guid accessKey, int clientID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAS.Entity.ClientFTP x);
    }
}
