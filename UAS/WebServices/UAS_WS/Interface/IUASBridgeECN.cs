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
    public interface IUASBridgeECN
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.UASBridgeECN>> Select(Guid accessKey, int userID);
    }
}
