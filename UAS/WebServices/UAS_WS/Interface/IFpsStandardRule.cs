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
    public interface IFpsStandardRule
    {
        /// <summary>
        /// returns a list of ALL Standard Rules which are KM defined and controlled
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        [OperationContract]
        Response<List<FpsStandardRule>> Select(Guid accessKey);

        /// <summary>
        /// Insert/Update record
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="fpsStandardRule"></param>
        /// <returns></returns>
        [OperationContract]
        Response<int> Save(Guid accessKey, FpsStandardRule fpsStandardRule);

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
