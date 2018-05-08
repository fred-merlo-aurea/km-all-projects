using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;
using FrameworkUAS.Entity;

namespace UAS_WS.Interface
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IFpsMap
    {
        /// <summary>
        /// Insert / Update mapping record
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="fpsMap"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, FpsMap fpsMap);

        /// <summary>
        /// Delete references to all mapped rules by SourceFileId
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="sourceFileId"></param>
        /// <returns></returns>
        [OperationContract]
        Response<bool> Delete(Guid accessKey, int sourceFileId);
        
    }
}
