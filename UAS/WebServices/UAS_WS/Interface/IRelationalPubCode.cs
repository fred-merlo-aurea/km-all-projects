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
    public interface IRelationalPubCode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForClient")]
        Response<List<FrameworkUAS.Entity.RelationalPubCode>> Select(Guid accessKey, int clientID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="specialFileName"></param>
        /// <returns></returns>
        [OperationContract(Name="SelectForFile")]
        Response<List<FrameworkUAS.Entity.RelationalPubCode>> Select(Guid accessKey, int clientID, string specialFileName);
    }
}
