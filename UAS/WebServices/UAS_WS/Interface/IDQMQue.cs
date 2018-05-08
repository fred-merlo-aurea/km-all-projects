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
    public interface IDQMQue
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientID"></param>
        /// <param name="isDemo"></param>
        /// <param name="isADMS"></param>
        /// <returns></returns>
        [OperationContract(Name = "SelectForClient")]
        Response<List<FrameworkUAS.Entity.DQMQue>> Select(Guid accessKey, int clientID, bool isDemo, bool isADMS);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="isDemo"></param>
        /// <param name="isADMS"></param>
        /// <param name="isQued"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.DQMQue>> Select(Guid accessKey, bool isDemo, bool isADMS, bool isQued = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="isDemo"></param>
        /// <param name="isADMS"></param>
        /// <param name="isQued"></param>
        /// <param name="isCompleted"></param>
        /// <returns></returns>
        [OperationContract(Name = "SelectForIsCompleted")]
        Response<List<FrameworkUAS.Entity.DQMQue>> Select(Guid accessKey, bool isDemo, bool isADMS, bool isQued = false, bool isCompleted = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> Save(Guid accessKey, FrameworkUAS.Entity.DQMQue x);
    }
}
