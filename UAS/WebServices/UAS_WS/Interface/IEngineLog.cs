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
    public interface IEngineLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.EngineLog>> SelectAll(Guid accessKey);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.EngineLog>> SelectClientId(Guid accessKey, int clientId);
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="isRunning"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FrameworkUAS.Entity.EngineLog>> SelectIsRunning(Guid accessKey, bool isRunning);
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientId"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        [OperationContract]
        Response<FrameworkUAS.Entity.EngineLog> Select(Guid accessKey, int clientId, string engine);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> Save(Guid accessKey, FrameworkUAS.Entity.EngineLog x);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="engineLogId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> UpdateRefresh(Guid accessKey, int engineLogId);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientId"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> UpdateRefreshClientIdEngine(Guid accessKey, int clientId, string engine);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientId"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> UpdateRefreshClientIdEngineEnum(Guid accessKey, int clientId, FrameworkUAS.BusinessLogic.Enums.Engine engine);
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="engineLogId"></param>
        /// <param name="isRunning"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> UpdateIsRunning(Guid accessKey, int engineLogId, bool isRunning);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientId"></param>
        /// <param name="engine"></param>
        /// <param name="isRunning"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> UpdateIsRunningClientIdEngine(Guid accessKey, int clientId, string engine, bool isRunning);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientId"></param>
        /// <param name="engine"></param>
        /// <param name="isRunning"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> UpdateIsRunningClientIdEngineEnum(Guid accessKey, int clientId, FrameworkUAS.BusinessLogic.Enums.Engine engine, bool isRunning);
       
    }
}
